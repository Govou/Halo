using AutoMapper;
using HalobizMigrations.Data;
using HalobizMigrations.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using OnlinePortalBackend.DTOs.TransferDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OnlinePortalBackend.Repository.Impl
{
    public class EndorsementRepositoryImpl : IEndorsementRepository
    {
        private readonly HalobizContext _context;
        private readonly ILogger<EndorsementRepositoryImpl> _logger;
        private readonly IMapper _mapper;
        public EndorsementRepositoryImpl(HalobizContext context,
            ILogger<EndorsementRepositoryImpl> logger,
            IMapper mapper)
        {
            _logger = logger;
            _context = context;
            _mapper = mapper;
        }
        public async Task<IEnumerable<ContractServiceForEndorsement>> FindEndorsements(long userId)
        {
            return _context.ContractServiceForEndorsements.Where(x => x.CustomerDivisionId == userId);
        }

        public async Task<ContractServiceForEndorsement> FindEndorsementById(long userId, long Id)
        {
            return _context.ContractServiceForEndorsements.FirstOrDefault(x => x.CreatedById == userId && x.Id == Id);
        }

        public async Task<ContractServiceDTO> GetContractService(int id)
        {
            var contractService = _context.ContractServices.Include(x => x.Contract).Include(x => x.Service).FirstOrDefault(x => x.Id == id);
            var servEndorsements = new List<ServiceEndorsement>();
            if (contractService == null)
                return null;
            var service = _context.Services.Include(x => x.ServiceType).Include(x => x.ServiceCategory).Include(x => x.AdminRelationship).FirstOrDefault(x => x.Id == contractService.ServiceId);
            var endorsementHistory = _context.ContractServiceForEndorsements.Include(x => x.EndorsementType).Where(x => x.ContractId == contractService.ContractId && x.ServiceId == contractService.ServiceId && x.UniqueTag == contractService.UniqueTag && x.IsApproved == true);
            foreach (var item in endorsementHistory)
            {
                servEndorsements.Add(new ServiceEndorsement
                {
                    Date = item.CreatedAt,
                    Description = item.EndorsementDescription,
                    Type = item.EndorsementType.Caption
                });
            }
            var result = new ContractServiceDTO
            {
                ContractServiceId = (int)contractService.Id,
                ServiceName = service.Name,
                ServiceCategory = service.ServiceCategory.Name,
                Quantity = (int)contractService.Quantity,
                ImageUrl = service.ImageUrl,
                Isvatable = service.IsVatable.Value,
                UnitPrice = service.UnitPrice,
                ServiceCode = service.ServiceCode,
                ServiceType = service.ServiceType?.Caption,
                ServiceDescription = service.Description,
                HasDirectComponent = service.AdminRelationship?.DirectServiceId != null && service.AdminRelationship?.AdminService != null ? true : false,
                HasAdminComponent = service.AdminRelationship?.AdminServiceId != null && service.AdminRelationship?.DirectService != null ? true : false,
                TotalContractValue = (int)contractService.Quantity * service.UnitPrice,
                ContractId = (int)contractService.Contract.Id,
                ServiceId = (int)service.Id,
                EndorsementHistory = servEndorsements
            };

            return result;
        }

        public async Task<IEnumerable<ContractDTO>> GetContractServices(int userId)
        {
            var contractServiceDTOs = new List<ContractServiceDTO>();
            var contractDTOs = new List<ContractDTO>();
            var contracts = _context.Contracts.Include(x => x.ContractServices).Where(x => x.CustomerDivisionId == userId && !x.IsDeleted && !x.IsDeleted && x.IsApproved);
            if (contracts == null)
            {
                return null;
            }
            var contractServices = new List<ContractService>();
            foreach (var item in contracts)
            {
                contractServices.AddRange(item.ContractServices);
            }
            var validContractServices = contractServices.Where(x => x.ContractEndDate > DateTime.Today && x.Version == 0);
            foreach (var contractService in validContractServices)
            {
                var service = _context.Services.Include(x => x.ServiceType).Include(x => x.ServiceCategory).Include(x => x.AdminRelationship).FirstOrDefault(x => x.Id == contractService.ServiceId);

                var result = new ContractServiceDTO
                {
                    ContractServiceId = (int)contractService.Id,
                    ServiceName = service.Name,
                    ServiceCategory = service.ServiceCategory.Name,
                    Quantity = (int)contractService.Quantity,
                    Isvatable = service.IsVatable.Value,
                    ImageUrl = service.ImageUrl,
                    UnitPrice = service.UnitPrice,
                    ServiceCode = service.ServiceCode,
                    ServiceType = service.ServiceType?.Caption,
                    ServiceDescription = service.Description,
                    HasDirectComponent = service.AdminRelationship?.DirectServiceId != null && service.AdminRelationship?.AdminService != null ? true : false,
                    HasAdminComponent = service.AdminRelationship?.AdminServiceId != null && service.AdminRelationship?.DirectService != null ? true : false,
                    TotalContractValue = (int)contractService.Quantity * service.UnitPrice,
                    ContractId = (int)contractService.ContractId,
                    ServiceId = (int)service.Id,
                    UniqueTag = contractService.UniqueTag
                };

                contractServiceDTOs.Add(result);
            }
            var grpContractServices = contractServiceDTOs.GroupBy(x => x.ContractId);
            foreach (var contractService in grpContractServices)
            {
                contractDTOs.Add(new ContractDTO
                {
                    Id = contractService.Key,
                    ContractServices = contractService.ToArray()
                });
            }

            return contractDTOs;
        }

        public async Task<ContractServiceForEndorsement> SaveContractServiceForEndorsement(ContractServiceForEndorsement entity)
        {
            try
            {
                var contractServiceForEndorsementEntity = await _context.ContractServiceForEndorsements.AddAsync(entity);
                int affected = await _context.SaveChangesAsync();
                return affected > 0 ? entity : null;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.StackTrace);
                throw;
            }
        }

        public async Task<bool> SetUpApprovalsForContractModificationEndorsement(ContractServiceForEndorsement contractServiceForEndorsement)
        {
            try
            {
                var endorsementType = await _context.EndorsementTypes.SingleOrDefaultAsync(x => x.Id == contractServiceForEndorsement.EndorsementTypeId);
                if (endorsementType == null)
                {
                    return false;
                }

                var module = await FindProcessesRequiringApprovalByCaption(endorsementType.Caption);
                if (module == null)
                {
                    return false;
                }

                var approvalLimits = await GetApprovalLimitsByModule(module.Id);
                if (approvalLimits == null)
                {
                    return false;
                }

                if (!approvalLimits.Any()) return false;

                List<Approval> approvals = new List<Approval>();

                contractServiceForEndorsement.Service = await GetServiceInformationForApprovals(contractServiceForEndorsement.ServiceId);
                contractServiceForEndorsement.Branch = await _context.Branches.FindAsync(contractServiceForEndorsement.BranchId);

                if (!contractServiceForEndorsement.BillableAmount.HasValue) return false;

                double amountChangeValue = contractServiceForEndorsement.BillableAmount.Value;

                var edType = endorsementType.Caption.ToLower();
                if (edType.Contains("topup") || edType.Contains("reduction"))
                {
                    var prevContractService = await _context.ContractServices.AsNoTracking()
                                                    .Where(x => x.Id == contractServiceForEndorsement.PreviousContractServiceId)
                                                    .SingleOrDefaultAsync();

                    amountChangeValue = Math.Abs(contractServiceForEndorsement.BillableAmount.Value - prevContractService.BillableAmount.Value);
                }

                var orderedList = approvalLimits
                    .Where(x => amountChangeValue > x.UpperlimitValue ||
                                (amountChangeValue <= x.UpperlimitValue &&
                                amountChangeValue >= x.LowerlimitValue))
                    .OrderBy(x => x.Sequence);

                var customerDivision = await _context.CustomerDivisions.Where(x => x.Id == contractServiceForEndorsement.CustomerDivisionId)
                    .Include(x => x.Customer)
                    .FirstOrDefaultAsync();

                foreach (var item in orderedList)
                {
                    var approvalLevelInfo = item.ApproverLevel;

                    long responsibleId = GetWhoIsResponsible(item, contractServiceForEndorsement.Service, contractServiceForEndorsement.Branch);

                    var approval = new Approval
                    {
                        ContractId = contractServiceForEndorsement.ContractId,
                        ContractServiceForEndorsementId = contractServiceForEndorsement.Id,
                        Caption = $"Approval needed to endorse {endorsementType.Caption} for contract service {contractServiceForEndorsement.Service.Name} for client {customerDivision.DivisionName} under {customerDivision.Customer.GroupName}",
                        CreatedById = _context.UserProfiles.FirstOrDefault(x => x.Email.ToLower().Contains("online.portal")).Id,
                        Sequence = item.Sequence,
                        ResponsibleId = responsibleId,
                        IsApproved = false,
                        DateTimeApproved = null,
                        Level = item.ApproverLevel.Caption,
                        ServicesId = contractServiceForEndorsement.ServiceId
                    };

                    approvals.Add(approval);
                }

                if (approvals.Any())
                {
           
                    var successful = await SaveApprovalRange(approvals);
                    if (successful)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                _logger.LogInformation(ex.Message);
                _logger.LogInformation(ex.StackTrace);
                throw;
            }
        }

        private async Task<bool> SaveApprovalRange(List<Approval> approvals)
        {
            try
            {
                await _context.Approvals.AddRangeAsync(approvals);
                var affected = await _context.SaveChangesAsync();
                _context.ChangeTracker.Clear();
                return affected > 0 ? true : false;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.StackTrace);
                throw;
            }
        }

        private long GetWhoIsResponsible(ApprovalLimit item, Service service, Branch branch)
        {
            if (item.ApproverLevel.Caption == "Branch Head")
            {
                return branch?.HeadId ?? throw new Exception($"No head set up for branch head in {branch?.Name}");
            }
            else if (item.ApproverLevel.Caption == "Division Head")
            {
                return service?.Division?.HeadId ?? throw new Exception($"No head set up for division head in {service?.Division?.Name}");
            }
            else if (item.ApproverLevel.Caption == "Operating Entity Head")
            {
                return service?.OperatingEntity?.HeadId ?? throw new Exception($"No head set up for operating entity head in {service?.OperatingEntity?.Name}");
            }
            else if (item.ApproverLevel.Caption == "CEO")
            {
                return service?.Division?.Company?.HeadId ?? throw new Exception($"No head set up for CEO head in {service?.Division?.Company?.Name}");
            }
            else
            {
                throw new Exception($"No approval person set up approval level {item?.ApproverLevel}");
            }
        }

        private async Task<ProcessesRequiringApproval> FindProcessesRequiringApprovalByCaption(string caption)
        {
            return await _context.ProcessesRequiringApprovals
             .FirstOrDefaultAsync(x => x.Caption == caption && x.IsDeleted == false);
        }

        private async Task<Service> GetServiceInformationForApprovals(long servicesId)
        {
            var services = await _context.Services.AsNoTracking().Where(x => x.Id == servicesId)
                    .Include(x => x.Division)
                    .ThenInclude(x => x.Company)
                    .Include(x => x.OperatingEntity)
                    .ThenInclude(x => x.Head)
                    .FirstOrDefaultAsync();

            return services;
        }
        private async Task<IEnumerable<ApprovalLimit>> GetApprovalLimitsByModule(long moduleId)
        {
            return await _context.ApprovalLimits
                .Where(x => x.ProcessesRequiringApprovalId == moduleId && x.IsBypassRequired == false && x.IsDeleted == false)
                .Include(x => x.ApproverLevel)
                .Include(x => x.ProcessesRequiringApproval)
                .OrderBy(x => x.Caption)
                .ToListAsync();
        }

        public async Task<EndorsementTrackingDTO> TrackEndorsement(long endorsementId)
        {
            var contractServiceEndorsement = _context.ContractServiceForEndorsements.FirstOrDefault(x => x.Id == endorsementId);
            var approvals = _context.Approvals.Include(x => x.ContractServiceForEndorsement).Where(x => x.ContractServiceForEndorsementId == endorsementId && !x.IsDeleted).ToList();
            var serviceName = string.Empty;
            double requestExecution = 0;
            var approvalStatus = string.Empty;
            if (approvals.Count() > 0)
            {
                double approvedCount = approvals.Where(x => x.IsApproved == true).Count();
                requestExecution = (approvedCount / approvals.Count()) * 100;
                requestExecution = Math.Round(requestExecution, 2);
                var service = approvals.FirstOrDefault()?.ContractServiceForEndorsement?.ServiceId;
                if (service != null)
                   serviceName = _context.Services.FirstOrDefault(x => x.Id == service.Value)?.Name;

                if (contractServiceEndorsement.IsDeclined)
                {
                    approvalStatus = "Request Declined";
                    requestExecution = 100;
                }
                else if (contractServiceEndorsement.IsApproved)
                {
                    approvalStatus = "Request Approved";
                    requestExecution = 100;
                }
                else
                {
                    approvalStatus = "Request Pending";
                }
            }

           
            var endorsementHistoryCount = _context.ContractServiceForEndorsements.Where(x => x.PreviousContractServiceId == contractServiceEndorsement.PreviousContractServiceId && x.IsConvertedToContractService.Value).Count();

            var endorsementTracking = new EndorsementTrackingDTO
            {
                EndorsementProcessingCount = approvals.Count(),
                RequestExecution = Math.Floor((decimal)requestExecution).ToString() + "%",
                EndorsementRequestDate = contractServiceEndorsement.FulfillmentStartDate.Value,
                ServiceName = serviceName,
                EndorsementHistoryCount = endorsementHistoryCount,
                ApprovalStatus = approvalStatus
            };

            return endorsementTracking;
        }
    }
}
