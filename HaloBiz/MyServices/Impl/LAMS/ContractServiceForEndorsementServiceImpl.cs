using System.Threading.Tasks;
using HalobizMigrations.Data;
using AutoMapper;
using HaloBiz.DTOs.ReceivingDTOs.LAMS;
using HaloBiz.DTOs.TransferDTOs.LAMS;
using HaloBiz.Repository.LAMS;
using Halobiz.Common.DTOs.ApiDTOs;
using System.Collections.Generic;
using HaloBiz.MyServices.LAMS;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using HalobizMigrations.Models;
using System.Linq;
using System;
using Microsoft.AspNetCore.Http;
using HaloBiz.Helpers;

namespace HaloBiz.MyServices.Impl.LAMS
{
    public class ContractServiceForEndorsementServiceImpl : IContractServiceForEndorsementService
    {
        private readonly HalobizContext _context;
        private readonly IMapper _mapper;
        private readonly ILeadConversionService _leadConversionService;
        private readonly IApprovalService _approvalService;
        private readonly IContractServiceForEndorsementRepository  _cntServiceForEndorsemntRepo;
        private readonly ILogger<ContractServiceForEndorsementServiceImpl> _logger;
        private readonly IConfiguration _configuration;
        private  long loggedInUserId;

        public ContractServiceForEndorsementServiceImpl( IContractServiceForEndorsementRepository  cntServiceForEndorsemntRepo,
                                            HalobizContext context,
                                            IMapper mapper,
                                            ILeadConversionService leadConversionService,
                                            IApprovalService approvalService,
                                            ILogger<ContractServiceForEndorsementServiceImpl> logger,
                                            IConfiguration configuration)
        {
            _context = context;
            _mapper = mapper;
            _leadConversionService = leadConversionService;
            _approvalService = approvalService;
            _cntServiceForEndorsemntRepo = cntServiceForEndorsemntRepo;
            _configuration = configuration;
            _logger = logger;
        }

        public async Task<ApiCommonResponse> GetNewContractAdditionEndorsement(long customerDivisionId)
        {
            var contract =  await _context.Contracts.Where(x => x.CustomerDivisionId == customerDivisionId && !x.IsApproved)
                                .Include(x => x.ContractServices)
                                    .ThenInclude(x=>x.Service)
                                .Include(x => x.ContractServices)
                                    .ThenInclude(x=>x.SbutoContractServiceProportions)
                                        .ThenInclude(x=>x.UserInvolved)
                                .FirstOrDefaultAsync();
            if (contract == null)
            {
                return CommonResponse.Send(ResponseCodes.NO_DATA_AVAILABLE);
            }

            return CommonResponse.Send(ResponseCodes.SUCCESS, contract);
        }

        public async Task<ApiCommonResponse> AddNewRetentionContractServiceForEndorsement(HttpContext httpContext, List<ContractServiceForEndorsementReceivingDto> contractServiceForEndorsementDtos)
        {
            if (!contractServiceForEndorsementDtos.Any())
            {
                return CommonResponse.Send(ResponseCodes.FAILURE, null, "No contract service specified");
            }

            //if (ValidateAdminAccompaniesDirectService(contractServiceForEndorsementDtos))
            //{
            //    return CommonResponse.Send(ResponseCodes.FAILURE, null, "Admin service must accompany direct service");
            //}

            using var transaction = await _context.Database.BeginTransactionAsync();

            var id = httpContext.GetLoggedInUserId();
            bool createNewContract = contractServiceForEndorsementDtos.Any(x=>x.ContractId==0 && x.PreviousContractServiceId==0);
            Contract newContract = null;
            List<ContractService> newContractServices = new List<ContractService>();

            if (createNewContract)
            {
                var contractDetail = contractServiceForEndorsementDtos.FirstOrDefault();

                //check if there is a pending contract addition for this guy
                if (_context.Contracts.Any(x=>x.CustomerDivisionId==contractDetail.CustomerDivisionId && !x.IsApproved))
                {
                    return CommonResponse.Send(ResponseCodes.FAILURE, null, "You have pending contract waiting approval");
                }

                newContract = new Contract {
                    CreatedAt = DateTime.Now,
                    CreatedById = id,
                    CustomerDivisionId = contractDetail.CustomerDivisionId,
                    Version = (int) VersionType.Latest,
                   GroupContractCategory =  contractDetail.GroupContractCategory,
                   GroupInvoiceNumber = contractDetail.GroupInvoiceNumber,
                   IsApproved = false,
                   HasAddedSBU = false,
                   Caption = contractDetail.DocumentUrl
                };

                var entity = await _context.Contracts.AddAsync(newContract);
                await _context.SaveChangesAsync();
                newContract = entity.Entity;
            }

            foreach (var item in contractServiceForEndorsementDtos)
            {              

                bool alreadyExists = false;              
                if(item.ContractId != 0)
                {
                    alreadyExists = await _context.ContractServiceForEndorsements
                       .AnyAsync(x => x.ContractId == item.ContractId && x.PreviousContractServiceId == item.PreviousContractServiceId
                                   && x.CustomerDivisionId == item.CustomerDivisionId && x.ServiceId == item.ServiceId
                                   && !x.IsApproved && !x.IsDeclined && x.IsConvertedToContractService != true && !x.IsDeleted);
                }

                if (alreadyExists)
                {
                    return CommonResponse.Send(ResponseCodes.FAILURE, null, $"There is already an endorsement request for the contract service with id {item.ContractId}");
                }

                //check if this is nenewal and the previous contract has not
                var previouslyRenewal = await _context.ContractServiceForEndorsements
                                                .Include(x => x.EndorsementType)
                                                .Where(x => x.PreviousContractServiceId == item.PreviousContractServiceId && x.EndorsementType.Caption.Contains("retention"))
                                                .FirstOrDefaultAsync();

                if (previouslyRenewal != null)
                {
                    return CommonResponse.Send(ResponseCodes.FAILURE,null, "There has been a retention on this contract service");
                }               

                item.CreatedById = id;
                if (createNewContract)
                {
                    if (item.InvoicingInterval == TimeCycle.MonthlyProrata)
                    {
                        if (item.ContractEndDate.Value.AddDays(1).Day != 1)
                        {
                            return CommonResponse.Send(ResponseCodes.FAILURE, null, $"Contract end date must be last day of month for tag {item.UniqueTag}");
                        }
                    }

                    var contractService = _mapper.Map<ContractService>(item);
                    contractService.ContractId = newContract.Id;
                    newContractServices.Add(contractService);
                }
            }         

            var entityToSaveList = _mapper.Map<List<ContractServiceForEndorsement>>(contractServiceForEndorsementDtos);
                        
            try
            {
                if (createNewContract)
                {
                    await _context.ContractServices.AddRangeAsync(newContractServices);
                    await _context.SaveChangesAsync();
                }
                else
                {
                    foreach (var item in entityToSaveList)
                    {
                        var savedEntity = await _cntServiceForEndorsemntRepo.SaveContractServiceForEndorsement(item);
                        if (savedEntity == null)
                        {
                            return CommonResponse.Send(ResponseCodes.FAILURE, null, "Some system errors occurred");
                        }

                        bool successful = await _approvalService.SetUpApprovalsForContractModificationEndorsement(savedEntity, httpContext);
                        if (!successful)
                        {
                            return CommonResponse.Send(ResponseCodes.FAILURE, null, "Could not set up approvals for service endorsement.");
                        }
                    }
                }

                await _context.SaveChangesAsync();
                await transaction.CommitAsync();

                if(createNewContract)
                {
                    var contract = await _context.ContractServices
                            .Where(x => x.ContractId == newContract.Id)
                           .Include(x=>x.Contract)
                           .ToListAsync();
                    return CommonResponse.Send(ResponseCodes.SUCCESS, contract);
                }

                return CommonResponse.Send(ResponseCodes.SUCCESS);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                _logger.LogError(ex.StackTrace);
                return CommonResponse.Send(ResponseCodes.FAILURE, null, ex.Message);
            }
        }

        //private bool ValidateAdminAccompaniesDirectService(List<ContractServiceForEndorsementReceivingDto> ContractServices)
        //{
        //    var isValidCount = 0;
        //    var adminServiceCount = 0;

        //    foreach (var contractService in ContractServices)
        //    {
        //        var directServiceExist = false;
        //        var adminServiceExist = false;
        //        var adminDirectService = _context.ServiceRelationships.FirstOrDefault(x => x.DirectServiceId == contractService.ServiceId || x.AdminServiceId == contractService.ServiceId);
        //        foreach (var item in ContractServices)
        //        {
        //            if (item.ServiceId == adminDirectService.AdminServiceId)
        //            {
        //                adminServiceExist = true;
        //                adminServiceCount++;
        //            }
        //            if (item.ServiceId == adminDirectService.DirectServiceId)
        //            {
        //                directServiceExist = true;
        //            }
        //        }
        //        if (directServiceExist && adminServiceExist)
        //        {
        //            isValidCount++;
        //        }
        //    }

        //    if (isValidCount == adminServiceCount)
        //    {
        //        return true;
        //    }
        //    return false;
        //}

        private async Task<bool> ValidateContractToRenew(ContractServiceForEndorsement contractServiceForEndorsement)
        {
            if (contractServiceForEndorsement.PreviousContractServiceId == null || contractServiceForEndorsement.PreviousContractServiceId == 0)
            {
                return false;
            }

            if(contractServiceForEndorsement.PreviousContractServiceId != null && contractServiceForEndorsement.PreviousContractServiceId > 0)
            {
                var contractService = await _context.ContractServices
                        .FirstOrDefaultAsync(x => x.Id == contractServiceForEndorsement.PreviousContractServiceId);

                if (contractService == null)
                {
                    return false;
                }
                if (contractService.ContractEndDate >= contractServiceForEndorsement.ContractStartDate)
                {
                    return false;
                }
            }

            return true;
        }

        public async Task<ApiCommonResponse> GetUnApprovedContractServiceForEndorsement()
        {
            var contractServiceForEndorsement = await _cntServiceForEndorsemntRepo.FindAllUnApprovedContractServicesForEndorsement();
            var contractServiceToEndorseTransferDto =
                _mapper.Map<IEnumerable<ContractServiceForEndorsementTransferDto>>(contractServiceForEndorsement);
            return CommonResponse.Send(ResponseCodes.SUCCESS,contractServiceToEndorseTransferDto);
        }

        public async Task<ApiCommonResponse> GetEndorsementDetailsById(long endorsementId)
        {
            var contractServiceForEndorsement = await _cntServiceForEndorsemntRepo.GetEndorsementDetailsById(endorsementId);
            var contractServiceToEndorseTransferDto =
                _mapper.Map<ContractServiceForEndorsementTransferDto>(contractServiceForEndorsement);
            return CommonResponse.Send(ResponseCodes.SUCCESS,contractServiceToEndorseTransferDto);
        }

        public async Task<ApiCommonResponse> GetEndorsementHistory(long contractServiceId)
        {
            var possibleDates = await _cntServiceForEndorsemntRepo.GetEndorsementHistory(contractServiceId);
            return CommonResponse.Send(ResponseCodes.SUCCESS,possibleDates);
        }

        public async Task<ApiCommonResponse> GetAllPossibleEndorsementStartDate(long contractServiceId)
        {
            var possibleDates = await _cntServiceForEndorsemntRepo.FindAllPossibleEndorsementStartDate(contractServiceId);
            return CommonResponse.Send(ResponseCodes.SUCCESS,possibleDates);
        }

        public async Task<ApiCommonResponse> ApproveContractServiceForEndorsement(long Id, long sequence, bool isApproved, HttpContext httpContext)
        {
            if (isApproved)
            {
                try
                {
                    using var transaction = await _context.Database.BeginTransactionAsync();

                    var approvalsForTheEndorsement = await _context.Approvals.Where(x => !x.IsDeleted && x.ContractServiceForEndorsementId == Id).ToListAsync();

                    var theApproval = approvalsForTheEndorsement.SingleOrDefault(x => x.Sequence == sequence);

                    if (theApproval == null)
                    {
                        return CommonResponse.Send(ResponseCodes.FAILURE, null, "There is no sequence for this approval");
                    }

                    theApproval.IsApproved = true;
                    theApproval.DateTimeApproved = DateTime.Now;
                    _context.Approvals.Update(theApproval);
                    await _context.SaveChangesAsync();

                    bool allApprovalsApproved = approvalsForTheEndorsement.All(x => x.IsApproved);

                    // Return scenario 1
                    // All the approvals for endorsement not yet approved.
                    if (allApprovalsApproved)
                    {
                       var response = await ConvertContractServiceForEndorsement(httpContext, Id);
                        if (response.responseCode == "00")
                        {
                            await transaction.CommitAsync();
                            return CommonResponse.Send(ResponseCodes.REFRESH_APPROVALS, null, "The approval was successful and converted");
                        }
                        else
                        {
                            return response;
                        }
                       
                    }

                    //var entityToApprove = await _cntServiceForEndorsemntRepo.FindContractServiceForEndorsementById(Id);
                    //if (entityToApprove == null)
                    //{
                    //    return CommonResponse.Send(ResponseCodes.NO_DATA_AVAILABLE);;
                    //}
                    //entityToApprove.IsApproved = isApproved;
                    //var approvedEntity = await _cntServiceForEndorsemntRepo.UpdateContractServiceForEndorsement(entityToApprove);
                    //if (approvedEntity == null)
                    //{
                    //    return CommonResponse.Send(ResponseCodes.FAILURE, null, "Some system errors occurred");
                    //}

                    //var contractServiceToEndorseTransferDto = _mapper.Map<ContractServiceForEndorsementTransferDto>(approvedEntity);

                    await transaction.CommitAsync();
                    return CommonResponse.Send(ResponseCodes.SUCCESS, null, "The approval was successful");
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex.StackTrace);
                    return CommonResponse.Send(ResponseCodes.FAILURE, null, ex.Message);
                }
            }
            else
            {
                var endorsementToUpdate = await _cntServiceForEndorsemntRepo.FindContractServiceForEndorsementById(Id);
                if (endorsementToUpdate == null)
                {
                    return CommonResponse.Send(ResponseCodes.NO_DATA_AVAILABLE);;
                }

                endorsementToUpdate.IsDeclined = true;

                var updatedEndorsement = await _cntServiceForEndorsemntRepo.UpdateContractServiceForEndorsement(endorsementToUpdate);

                if (updatedEndorsement == null)
                {
                    return CommonResponse.Send(ResponseCodes.FAILURE, null, "Some system errors occurred");
                }

                var contractServiceToEndorseTransferDto = _mapper.Map<ContractServiceForEndorsementTransferDto>(updatedEndorsement);
                return CommonResponse.Send(ResponseCodes.SUCCESS,contractServiceToEndorseTransferDto);
            }
        }

        private async Task<ApiCommonResponse> ConvertContractServiceForEndorsement(HttpContext httpContext, long Id)
        {
            
            try
            {
                this.loggedInUserId = httpContext.GetLoggedInUserId();
                var contractServiceForEndorsement = await _cntServiceForEndorsemntRepo
                                            .FindContractServiceForEndorsementById(Id);

                if (contractServiceForEndorsement == null)
                {
                    return CommonResponse.Send(ResponseCodes.NO_DATA_AVAILABLE); ;
                }

                var contractServiceToRetire = await _context.ContractServices
                        .FirstOrDefaultAsync(x => x.Id == contractServiceForEndorsement.PreviousContractServiceId);

                var contractServiceToSave = _mapper.Map<ContractService>(contractServiceForEndorsement);
                contractServiceToSave.Id = 0;


                var contractServiceEntity = await _context.ContractServices.AddAsync(contractServiceToSave);
                await _context.SaveChangesAsync();


                var contractService = await _context.ContractServices
                    .Where(x => x.Id == contractServiceToSave.Id)
                    .Include(x => x.Service)
                    .FirstOrDefaultAsync(); //contractServiceEntity.Entity;


                var contract = await _context.Contracts
                            .Include(x => x.CustomerDivision)
                            .FirstOrDefaultAsync(x => x.Id == contractService.ContractId);

                var customerDivision = contract.CustomerDivision;

                var service = await _context.Services
                            .FirstOrDefaultAsync(x => x.Id == contractServiceForEndorsement.ServiceId);

                string endorsementType = contractServiceForEndorsement.EndorsementType.Caption;

                if (endorsementType.ToLower().Contains("addition"))
                {
                    await AddServiceEndorsement(contractService, contractServiceForEndorsement, service, customerDivision);

                }
                else if (endorsementType.ToLower().Contains("topup"))
                {
                    //this new contract would have the same start and end date as the on to retire
                    await RetainContractValues(contractService, contractServiceToRetire);

                    await ServiceTopUpGoingForwardEndorsement(contractServiceToRetire,
                                                                contractService,
                                                                customerDivision,
                                                                service,
                                                                contractServiceForEndorsement
                                                                );
                }
                else if (endorsementType.ToLower().Contains("reduction"))
                {

                    //this new contract would have the same start and end date as the on to retire
                    await RetainContractValues(contractService, contractServiceToRetire);

                    await ServiceReductionGoingForwardEndorsement(contractServiceToRetire,
                                                                contractService,
                                                                customerDivision,
                                                                service,
                                                                contractServiceForEndorsement
                                                                );
                }
                else if (endorsementType.ToLower().Contains("retention"))
                {
                    await RetainSbuInfo(contractService, contractServiceToRetire, true);

                    await ServiceRenewalEndorsement(contractService,
                                                    service,
                                                    customerDivision);
                }
                else if (endorsementType.ToLower().Contains("credit"))
                {
                    var currentContractService = await _context.ContractServices
                        .FirstOrDefaultAsync(x => x.Id == contractServiceForEndorsement.PreviousContractServiceId);

                    await CreditNoteEndorsement(currentContractService,
                                                customerDivision,
                                                service,
                                                contractServiceForEndorsement);
                }
                else if (endorsementType.ToLower().Contains("debit"))
                {
                    var currentContractService = await _context.ContractServices
                        .FirstOrDefaultAsync(x => x.Id == contractServiceForEndorsement.PreviousContractServiceId);

                    await DebitNoteEndorsement(currentContractService,
                                               customerDivision,
                                               service,
                                               contractServiceForEndorsement);
                }
                else
                {
                    throw new Exception("Invalid Endorsement Type");
                }

                contractServiceForEndorsement.IsConvertedToContractService = true;
                contractServiceForEndorsement.IsApproved = true;
                _context.ContractServiceForEndorsements.Update(contractServiceForEndorsement);
                await _context.SaveChangesAsync();


                return CommonResponse.Send(ResponseCodes.SUCCESS);
            }
            catch (System.Exception e)
            {
                _logger.LogError(e.Message);
                _logger.LogError(e.StackTrace);
                return CommonResponse.Send(ResponseCodes.FAILURE, null, "Some system errors occurred");
            }

        }
       

    public async Task<ApiCommonResponse> JobPostingRenewContractService(HttpContext httpContext)
        {
            using (var transaction = await _context.Database.BeginTransactionAsync())
            {
                try
                {
                    this.loggedInUserId = httpContext.GetLoggedInUserId();

                    //get all the contract services that are due for renwal
                    var today = DateTime.Now.Date;
                    var allContractServicesEndingToday = await _context.ContractServices
                                                .Where(x => x.ContractEndDate.Value.Date == today)
                                                .Include(x => x.ClientPolicy)
                                                .Include(x => x.Service)
                                                .ToListAsync();

                    var contractServicesToRenew = allContractServicesEndingToday.Where(x => x.ClientPolicy != null && x.ClientPolicy?.AutoRenew==true).ToList();
                   
                    if (contractServicesToRenew.Count == 0)
                    {
                        return CommonResponse.Send(ResponseCodes.NO_DATA_AVAILABLE, null, "No renewal policy for today");
                    }

                    foreach (var contractServiceToRetire in contractServicesToRenew)
                    { 
                        if (contractServiceToRetire.InvoicingInterval == (int) TimeCycle.Adhoc || contractServiceToRetire.InvoicingInterval==(int) TimeCycle.OneTime)
                        {
                            continue;
                        }

                        var clientPolicy = contractServiceToRetire.ClientPolicy;
                         var contractServiceToSave = _mapper.Map<ContractService>(contractServiceToRetire);
                        var contractEndDate = DateTime.Now.AddMonths((int)clientPolicy.RenewalTenor);
                        contractServiceToSave.ContractEndDate = contractEndDate;
                        contractServiceToSave.ContractStartDate = DateTime.Today;
                        contractServiceToSave.FirstInvoiceSendDate = DateTime.Now.AddDays(5);

                        contractServiceToSave.Id = 0;

                        var contractServiceEntity = await _context.ContractServices.AddAsync(contractServiceToSave);
                        await _context.SaveChangesAsync();

                        var contractService = contractServiceEntity.Entity;

                        var contract = await _context.Contracts
                                    .Where(x => x.Id == contractServiceToRetire.ContractId)
                                    .Include(x => x.CustomerDivision)
                                    .FirstOrDefaultAsync();

                        var customerDivision = contract.CustomerDivision;
                        
                        await RetainSbuInfo(contractService, contractServiceToRetire, true);

                        await ServiceRenewalEndorsement(

                                                        contractService,
                                                        contractServiceToRetire.Service,
                                                        customerDivision);

                        //now change the contractServiceId of this in the clientPolicy
                        clientPolicy.ContractServiceId = contractService.Id;
                        clientPolicy.ContractService = contractService;
                        clientPolicy.NextRateReviewDate = contractEndDate.AddDays(1);
                        clientPolicy.UpdatedAt = DateTime.Today;
                        _context.ClientPolicies.Update(clientPolicy);
                        await _context.SaveChangesAsync();
                    }

                    await _context.SaveChangesAsync();

                    await transaction.CommitAsync();

                    return CommonResponse.Send(ResponseCodes.SUCCESS);
                }
                catch (System.Exception e)
                {
                    await transaction.RollbackAsync();
                    _logger.LogError(e.Message);
                    _logger.LogError(e.StackTrace);
                    return CommonResponse.Send(ResponseCodes.FAILURE, null, "Some system errors occurred");
                }
            }

        }

        private async Task<bool> RetainSbuInfo(ContractService contractService, ContractService contractServiceToRetire, bool isRenewal=false)
        {
            try
            {
                var props = _context.SbutoContractServiceProportions.Where(x => x.ContractServiceId == contractServiceToRetire.Id);

                if (isRenewal)
                {
                    var newProbs = _mapper.Map<List<SbutoContractServiceProportion>>(props);
                    foreach (var item in newProbs)
                    {
                        item.ContractServiceId = contractService.Id;
                        item.Id = 0;
                    }

                    await _context.AddRangeAsync(newProbs);
                }
                else
                {
                    foreach (var item in props)
                    {
                        item.ContractServiceId = contractService.Id;
                    }
                }
               

                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.StackTrace);
                throw;
            }

            return true;
        }

        private async Task<bool> RetainContractValues(ContractService contractService, ContractService contractServiceToRetire)
        {
            try
            {
                contractService.ContractStartDate = contractServiceToRetire.ContractStartDate;
                contractService.ContractEndDate = contractServiceToRetire.ContractEndDate;
                contractService.InvoicingInterval = contractServiceToRetire.InvoicingInterval;
                contractService.UniqueTag = contractServiceToRetire.UniqueTag;

                await _context.SaveChangesAsync();
                await RetainSbuInfo(contractService, contractServiceToRetire);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.StackTrace);
                throw;
            }

            return true;
        }

        //public async Task<ApiCommonResponse> ConvertDebitCreditNoteEndorsement(HttpContext httpContext, long id)
        //{
        //    using (var transaction = await _context.Database.BeginTransactionAsync())
        //    {
        //        try
        //        {
        //            loggedInUserId = httpContext.GetLoggedInUserId();
        //            var contractServiceForEndorsement = await _cntServiceForEndorsemntRepo
        //                                        .FindContractServiceForEndorsementById(id);

        //            if (contractServiceForEndorsement == null)
        //            {
        //                return CommonResponse.Send(ResponseCodes.NO_DATA_AVAILABLE);;
        //            }

        //            contractServiceForEndorsement.IsConvertedToContractService = true;
        //            _context.ContractServiceForEndorsements.Update(contractServiceForEndorsement);
        //            await _context.SaveChangesAsync();

        //            var contract = await _context.Contracts
        //                        .Include(x => x.CustomerDivision)
        //                            .ThenInclude(x=>x.Customer)
        //                                .ThenInclude(x=>x.GroupType)
        //                        .FirstOrDefaultAsync(x => x.Id == contractServiceForEndorsement.ContractId);

        //            var customerDivision = contract.CustomerDivision;

        //            var service = await _context.Services
        //                        .FirstOrDefaultAsync(x => x.Id == contractServiceForEndorsement.ServiceId);

        //            string endorsementType = contractServiceForEndorsement.EndorsementType.Caption;

        //            if (endorsementType.ToLower().Contains("credit"))
        //            {
        //                var currentContractService = await _context.ContractServices
        //                    .FirstOrDefaultAsync(x => x.Id == contractServiceForEndorsement.PreviousContractServiceId);

        //                await CreditNoteEndorsement(currentContractService,
        //                                            customerDivision,
        //                                            service,
        //                                            contractServiceForEndorsement);
        //            }
        //            else if (endorsementType.ToLower().Contains("debit"))
        //            {
        //                var currentContractService = await _context.ContractServices
        //                    .FirstOrDefaultAsync(x => x.Id == contractServiceForEndorsement.PreviousContractServiceId);

        //                await DebitNoteEndorsement(currentContractService,
        //                                           customerDivision,
        //                                           service,
        //                                           contractServiceForEndorsement);
        //            }
        //            else
        //            {
        //                throw new Exception("Invalid Endorsement Type");
        //            }

        //            await transaction.CommitAsync();

        //            return CommonResponse.Send(ResponseCodes.SUCCESS);
        //        }
        //        catch (Exception ex)
        //        {
        //            await transaction.RollbackAsync();
        //            _logger.LogError(ex.Message);
        //            _logger.LogError(ex.StackTrace);
        //            return CommonResponse.Send(ResponseCodes.FAILURE, null, "Some system errors occurred");
        //        }
        //    }
        //}

        private async Task<bool> AddServiceEndorsement(ContractService contractService, ContractServiceForEndorsement contractServiceForEndorsement, Service service, CustomerDivision customerDivision)
        {
            var salesVoucherName = this._configuration.GetSection("VoucherTypes:SalesInvoiceVoucher").Value;
            var financialVoucherType = await _context.FinanceVoucherTypes
                            .FirstOrDefaultAsync(x => x.VoucherType.ToLower() == salesVoucherName.ToLower());

          
            var contract = await _context.Contracts.Where(x => x.Id == contractService.ContractId).FirstOrDefaultAsync();
            contractService.Contract = contract;
            await _leadConversionService.GenerateInvoices(contractService, customerDivision.Id, service.ServiceCode, this.loggedInUserId);

            await _leadConversionService.GenerateAmortizations(contractService, customerDivision, (double)contractService?.BillableAmount);

            return true;

        }

        private async Task<bool> TerminateContractService(ContractService contractServiceToTerminate, ContractServiceForEndorsement contractServiceForEndorsement)
        {
            throw new Exception("this method is not mentained");
            ////to check
            //if (string.IsNullOrWhiteSpace(contractServiceToTerminate.Contract.GroupInvoiceNumber))
            //{
            //    await UpdateInvoices(contractServiceToTerminate, contractServiceForEndorsement, false, false);
            //}
            //else
            //{
            //    await UpdateInvoices(contractServiceToTerminate, contractServiceForEndorsement, false, true);
            //}

            //var terminatedContractServiceToNegateAmmortization = _mapper.Map<ContractService>(contractServiceToTerminate);

            ////terminatedContractServiceToNegateAmmortization.BillableAmount *= -1;
            // terminatedContractServiceToNegateAmmortization.ContractStartDate = contractServiceForEndorsement.DateForNewContractToTakeEffect;
            //terminatedContractServiceToNegateAmmortization.BillableAmount = contractServiceToTerminate.BillableAmount;

            //await _leadConversionService.GenerateAmortizations(terminatedContractServiceToNegateAmmortization, contractServiceToTerminate.Contract.CustomerDivision);

            //return true;
        }

        private async Task<bool> ServiceTopUpGoingForwardEndorsement(ContractService retiredContractService,
                                                                    ContractService newContractService,
                                                                    CustomerDivision customerDivision,
                                                                    Service service,
                                                                    ContractServiceForEndorsement contractServiceForEndorsement
                                                                    )
        {
            var salesTopUpVoucher = this._configuration.GetSection("VoucherTypes:SalesTopupVoucher").Value;

            var financialVoucherType = await _context.FinanceVoucherTypes
                            .FirstOrDefaultAsync(x => x.VoucherType.ToLower() == salesTopUpVoucher.ToLower());

            await _leadConversionService.CreateTaskAndDeliverables(newContractService, customerDivision.Id, "Service Topup", this.loggedInUserId);

            await UpdateInvoices(newContractService, contractServiceForEndorsement);

            var description = $"Service Topup for {service.Name} with serviceId: {service.Id} for client: {customerDivision.DivisionName}. quantity increase of {newContractService.Quantity - retiredContractService.Quantity}";
           var result = await RetireContractService(
                                        retiredContractService,
                                        newContractService,
                                        description,
                                        contractServiceForEndorsement.EndorsementTypeId
                                        );                     


            newContractService.ContractStartDate = contractServiceForEndorsement.DateForNewContractToTakeEffect;
            double oldAmount = (double)(retiredContractService.BillableAmount);
            double newAmount = (double)(newContractService.BillableAmount);
            var difference = newAmount - oldAmount; //will be +ve
            await _leadConversionService.GenerateAmortizations(newContractService, customerDivision, difference, contractServiceForEndorsement);

            return true;
        }

        private async Task<bool> ServiceReductionGoingForwardEndorsement(ContractService retiredContractService,
                                                                    ContractService newContractService,
                                                                    CustomerDivision customerDivision,
                                                                    Service service,
                                                                    ContractServiceForEndorsement contractServiceForEndorsement
                                                                    )
        {
            var salesReductionVoucher = this._configuration.GetSection("VoucherTypes:SalesReductionVoucher").Value;

            var financialVoucherType = await _context.FinanceVoucherTypes
                            .FirstOrDefaultAsync(x => x.VoucherType.ToLower() == salesReductionVoucher.ToLower());


            await UpdateInvoices(newContractService, contractServiceForEndorsement);

            var description = $"Service Reduction for {service.Name} with serviceId: {service.Id} for client: {customerDivision.DivisionName}. quantity reduction of {retiredContractService.Quantity - newContractService.Quantity }";
            var result = await RetireContractService(
                                        retiredContractService,
                                        newContractService,
                                        description,
                                        contractServiceForEndorsement.EndorsementTypeId
                                        );

           
            //for billable amount the different should be substracted as the billable amount
            double oldAmount = (double)(retiredContractService.BillableAmount);
            double newAmount =  (double) (newContractService.BillableAmount);
            var difference = newAmount - oldAmount; //will be -ve
            var (success, message) = await _leadConversionService.GenerateAmortizations(newContractService, customerDivision, difference, contractServiceForEndorsement);

            if (success)
            {
                //check if this is cancellation
                if (newContractService.Quantity == 0)
                {
                    var end = (DateTime)contractServiceForEndorsement.DateForNewContractToTakeEffect;
                    newContractService.ContractEndDate = end.AddDays(-1); //previous day
                    newContractService.Vat = retiredContractService.Vat;
                    newContractService.BillableAmount = retiredContractService.BillableAmount;
                    newContractService.Quantity = retiredContractService.Quantity;
                    newContractService.UnitPrice = retiredContractService.UnitPrice;
                    await _context.SaveChangesAsync();
                }
            }            

            return true;
        }

        private async Task<bool> ServiceRenewalEndorsement(ContractService newContractService,
                                                            Service service, CustomerDivision customerDivision)
        {

            var renewalVoucherName = this._configuration.GetSection("VoucherTypes:SalesRetentionVoucher").Value;
            var financialVoucherType = await _context.FinanceVoucherTypes
                            .FirstOrDefaultAsync(x => x.VoucherType.ToLower() == renewalVoucherName.ToLower());


                await  _leadConversionService.GenerateInvoices(newContractService,
                                                                customerDivision.Id,
                                                                service.ServiceCode,
                                                                this.loggedInUserId);
            

            await _leadConversionService.GenerateAmortizations(newContractService, customerDivision, (double) newContractService.BillableAmount);

            var description = $"Service Renewal for {service.Name} with serviceId: {service.Id} for client: {customerDivision.DivisionName}.";

            return true;
        }

        private async Task<bool> CreditNoteEndorsement(ContractService currentContractService,
                                                        CustomerDivision customerDivision,
                                                        Service service,
                                                        ContractServiceForEndorsement contractServiceForEndorsement)
        {

            var financialVoucherType = await _context.FinanceVoucherTypes
                            .FirstOrDefaultAsync(x => x.VoucherType == "Credit Note");

            var contractServiceDifference = _mapper.Map<ContractService>(currentContractService);

            contractServiceDifference.BillableAmount = contractServiceForEndorsement.BillableAmount;
            contractServiceDifference.Vat = contractServiceForEndorsement.Vat;

            await _leadConversionService.CreateAccounts(
                                            contractServiceDifference,
                                            customerDivision,
                                            (long)contractServiceForEndorsement.BranchId,
                                            (long)contractServiceForEndorsement.OfficeId,
                                            service,
                                            financialVoucherType,
                                            null,
                                            loggedInUserId,
                                            true,
                                            null);

            return true;
        }

        private async Task<bool> DebitNoteEndorsement(ContractService currentContractService,
                                                        CustomerDivision customerDivision,
                                                        Service service,
                                                        ContractServiceForEndorsement contractServiceForEndorsement)
        {
            //var salesTopUpVoucher = this._configuration.GetSection("VoucherTypes:SalesTopupVoucher").Value;

            var financialVoucherType = await _context.FinanceVoucherTypes
                            .FirstOrDefaultAsync(x => x.VoucherType == "Debit Note");

            var contractServiceDifference = _mapper.Map<ContractService>(currentContractService);

            contractServiceDifference.BillableAmount = contractServiceForEndorsement.BillableAmount;
            contractServiceDifference.Vat = contractServiceForEndorsement.Vat;

            await _leadConversionService.CreateAccounts(
                                            contractServiceDifference,
                                            customerDivision,
                                            (long)contractServiceForEndorsement.BranchId,
                                            (long)contractServiceForEndorsement.OfficeId,
                                            service,
                                            financialVoucherType,
                                            null,
                                            loggedInUserId,
                                            false, null);

            return true;
        }

        private ContractService GetContractServiceDifference(ContractService retiredContractService, ContractService newContractService)
        {
            var contractServcie = _mapper.Map<ContractService>(newContractService);
            contractServcie.Vat = contractServcie.Vat - retiredContractService.Vat;
            contractServcie.Quantity = contractServcie.Quantity - retiredContractService.Quantity;
            contractServcie.BillableAmount = contractServcie.BillableAmount - retiredContractService.BillableAmount;
            contractServcie.Budget = contractServcie.Budget = retiredContractService.Budget;
            return contractServcie;
        }

        private async Task<bool> RetireContractService(ContractService retiredContractService, ContractService newContractService, string description, long endorsementTypeId)
        {
            try
            {
                EndorsementTypeTracker tracker = new EndorsementTypeTracker()
                {
                    PreviousContractServiceId = retiredContractService.Id,
                    NewContractServiceId = newContractService.Id,
                    DescriptionOfChange = description,
                    ApprovedById = this.loggedInUserId,
                    EndorsementTypeId = endorsementTypeId,
                };

                await _context.EndorsementTypeTrackers.AddAsync(tracker);
                retiredContractService.Version = (int)VersionType.Previous;
                               

                _context.ContractServices.Update(retiredContractService);
                return await _context.SaveChangesAsync() > 0;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.StackTrace);
                throw;
            }

        }

        private async Task<bool> UpdateInvoices(ContractService contractService, ContractServiceForEndorsement contractServiceForEndorsement)
        {
            IEnumerable<Invoice> invoices = null;

            try
            {
                // update the invoices to have this new contract id
                var invoicesToUpdate = await _context.Invoices.Where(x => x.ContractServiceId == contractServiceForEndorsement.PreviousContractServiceId && !x.IsDeleted).ToListAsync();
                foreach (var invoice in invoicesToUpdate)
                {
                    invoice.ContractServiceId = contractService.Id;
                }

                _context.Invoices.UpdateRange(invoicesToUpdate);
                int affected = await _context.SaveChangesAsync();

                if(affected > 0)
                {
                    invoices = invoicesToUpdate
                                   .Where(x => x.StartDate >= contractServiceForEndorsement.DateForNewContractToTakeEffect && !x.IsDeleted);

                    var billbalbleForInvoicingPeriod = CalculateTotalBillableForPeriod(contractService);

                    foreach (var invoice in invoices)
                    {
                        invoice.Value = billbalbleForInvoicingPeriod;
                        invoice.Quantity = contractService.Quantity;
                        invoice.UnitPrice = (double) contractService.UnitPrice;
                        invoice.Discount = (double) contractService.Discount;
                    }

                    _context.Invoices.UpdateRange(invoices);
                    await _context.SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.StackTrace);
                throw;
            }

            return true;
        }

        private async Task<bool> GenerateGroupInvoiceDetails(ContractService contractService)
        {
            GroupInvoiceDetail groupInvoiceDetails = new GroupInvoiceDetail()
            {
                InvoiceNumber = contractService.Contract.GroupInvoiceNumber,
                Description = $"Invoice details for Group Invoice {contractService.Contract.GroupInvoiceNumber}",
                UnitPrice = (double) contractService.UnitPrice,
                Quantity =(int) contractService.Quantity,
                Vat  = (double) contractService.Vat,
                Value = (double) (contractService.BillableAmount - contractService.Vat),
                BillableAmount = (double) contractService.BillableAmount,
                ContractServiceId = contractService.Id
            };

            await _context.GroupInvoiceDetails.AddAsync(groupInvoiceDetails);
            await _context.SaveChangesAsync();
            return true;
        }

        private async Task<bool> ReverseInvoicesAndReceipts(long contractServiceId)
        {
            IEnumerable<Invoice> invoicesToBeReversed = await _context.Invoices
                        .Include(invoice => invoice.Receipts)
                        .Where(invoice => !invoice.IsDeleted && !invoice.IsReversed.Value && invoice.IsReversalInvoice.Value)
                        .ToListAsync();
            List<Receipt> reversalReeceipts = new List<Receipt>();
            List<Invoice> reversalInvoices = new List<Invoice>();

            foreach (var invoice in invoicesToBeReversed)
            {
                reversalInvoices.Add(GenerateReverselInvoice(invoice));
                invoice.IsReversed = true;

                foreach (var receipt in invoice.Receipts)
                {
                    receipt.IsReversed = true;
                    reversalReeceipts.Add(GenerateReversalReceipt(receipt));
                }
            }

            await _context.Invoices.AddRangeAsync(reversalInvoices);
            await _context.SaveChangesAsync();
            await _context.Receipts.AddRangeAsync(reversalReeceipts);
            await _context.SaveChangesAsync();
            _context.UpdateRange(invoicesToBeReversed);
            await _context.SaveChangesAsync();
            return false;
        }

        private Invoice GenerateReverselInvoice(Invoice invoice)
        {
            Invoice invoiceToReverse = _mapper.Map<Invoice>(invoice);
            invoiceToReverse.Value *= -1;
            invoiceToReverse.IsInvoiceSent = true;
            invoiceToReverse.IsReversalInvoice = true;
            invoiceToReverse.CreatedAt = DateTime.Now;
            invoiceToReverse.UpdatedAt = DateTime.Now;
            invoiceToReverse.Id = 0;
            invoiceToReverse.CreatedById = this.loggedInUserId;

            return invoiceToReverse;
        }

        private  Receipt GenerateReversalReceipt(Receipt receipt)
        {
            Receipt receiptToReverse = _mapper.Map<Receipt>(receipt);
            receiptToReverse.InvoiceValue*= -1;
            receiptToReverse.InvoiceValueBalanceAfterReceipting = 0;
            receiptToReverse.InvoiceValueBalanceBeforeReceipting = 0;
            receiptToReverse.Caption = $"Reversal of {receiptToReverse.Caption}";
            receiptToReverse.DateAndTimeOfFundsReceived = DateTime.Now;
            receiptToReverse.ReceiptValue*= -1;
            receiptToReverse.UpdatedAt = DateTime.Now;
            receiptToReverse.CreatedAt = DateTime.Now;
            receiptToReverse.CreatedById = this.loggedInUserId;
            receiptToReverse.IsReversalReceipt = true;
            receiptToReverse.Id= 0;

            return receiptToReverse;

        }

        private static double GenerateWeeklyAmount(DateTime startDate, DateTime endDate, double amountPerMonth, TimeCycle timeCylce)
        {
            var interval = 0; //timeCylce == TimeCycle.Weekly ? 7 : 14;
            var numberOfMonths = 0;
            var daysBetweenStartAndEndDate = (endDate.Subtract(startDate).TotalDays + 1) < interval ?
                           (int) interval : (endDate.Subtract(startDate).TotalDays + 1);
            var numberOfPayment = Math.Floor(daysBetweenStartAndEndDate / interval);
            startDate = startDate.AddMonths(1);

            while(startDate < endDate)
            {
                numberOfMonths++;
                startDate = startDate.AddMonths(1);
            }
            numberOfMonths = numberOfMonths == 0 ? 1 : numberOfMonths;
            var totalAmountPayable = numberOfMonths * amountPerMonth;
            var amountToPay = Math.Round(totalAmountPayable / numberOfPayment, 4);
            return amountToPay;
        }

        private double CalculateTotalBillableForPeriod(ContractService contractService)
        {
            int interval = 0;

            try
            {
                DateTime startDate = (DateTime)contractService.ContractStartDate;
                DateTime endDate = (DateTime)contractService.ContractEndDate;
                TimeCycle cycle = (TimeCycle)contractService.InvoicingInterval;
                double amount = (double) contractService.BillableAmount;

                switch (cycle)
                {                   
                    case TimeCycle.Monthly:
                        interval = 1;
                        break;
                    case TimeCycle.BiMonthly:
                        interval = 2;
                        break;
                    case TimeCycle.Quarterly:
                        interval = 3;
                        break;
                    case TimeCycle.BiAnnually:
                        interval = 6;
                        break;
                    case TimeCycle.Annually:
                        interval = 12;
                        break;                    
                }

               if (cycle == TimeCycle.OneTime)
                {
                    return amount;
                }
                else
                {
                    return amount * (double)interval;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.StackTrace);
                throw;
            }
        }
    }
}
