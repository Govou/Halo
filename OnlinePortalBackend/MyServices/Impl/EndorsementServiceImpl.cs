﻿using AutoMapper;
using Halobiz.Common.DTOs.ApiDTOs;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using OnlinePortalBackend.DTOs.ApiDTOs;
using OnlinePortalBackend.DTOs.TransferDTOs;
using OnlinePortalBackend.Helpers;
using OnlinePortalBackend.Repository;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Flurl.Http;
using Microsoft.Extensions.Configuration;
using System;
using HalobizMigrations.Data;
using Halobiz.Common.DTOs.TransferDTOs;
using OnlinePortalBackend.Adapters;
using Microsoft.EntityFrameworkCore;
using Halobiz.Common.Helpers;
using HalobizMigrations.Models;
using TimeCycle = Halobiz.Common.Helpers.TimeCycle;
using Halobiz.Common.DTOs.ReceivingDTOs;
using VersionType = Halobiz.Common.DTOs.ReceivingDTOs.VersionType;

namespace OnlinePortalBackend.MyServices.Impl
{
    public class EndorsementServiceImpl : IEndorsementService
    {
        private readonly ILogger<EndorsementServiceImpl> _logger;
        private readonly IEndorsementRepository _endorsementRepo;
        private readonly IMapper _mapper;
        private readonly IConfiguration _configuration;
        private string _HalobizBaseUrl;
        private readonly HalobizContext _context;
        private readonly IApiInterceptor _apiInterceptor;
        public EndorsementServiceImpl(ILogger<EndorsementServiceImpl> logger,
            IEndorsementRepository endorsementRepo,
            IMapper mapper,
            IConfiguration configuration,
            HalobizContext context,
            IApiInterceptor apiInterceptor
            )
        {
            _logger = logger;
            _endorsementRepo = endorsementRepo;
            _mapper = mapper;
            _configuration = configuration;
            _context = context;
            _apiInterceptor = apiInterceptor;
            _HalobizBaseUrl = _configuration["HalobizBaseUrl"] ?? _configuration.GetSection("AppSettings:HalobizBaseUrl").Value;
        }

        public async Task<ApiCommonResponse> EndorsementTopUp(int userId, List<EndorsementDTO> endorsements)
        {
            ApiCommonResponse responseData = new ApiCommonResponse();
            var endorsementDetailDTOs = new List<ContractServiceForEndorsementReceivingDto>();
            var topup = _context.EndorsementTypes.FirstOrDefault(x => x.Caption.ToLower().Contains("topup"))?.Id;

            if (topup == null)
            {
                responseData.responseMsg = "An error occured";
                return responseData;
            }

            foreach (var item in endorsements)
            {
                //  item.CreatedById = userId;
                item.EndorsementType = (int)topup.Value;
            }

            foreach (var item in endorsements)
            {
                var contractService = _context.ContractServices.Include(x => x.Contract).Include(x => x.Service).AsNoTracking().FirstOrDefault(x => x.Id == item.ContractServiceId && x.Version == 0);
                var amountWithoutVat = contractService.UnitPrice * item.Quantity - contractService.Discount;
                var amount = 0.0;

                if (contractService.Vat > 0)
                {
                    var amountWithVat = amountWithoutVat + (0.075 * amountWithoutVat);
                    amount = amountWithVat.Value;
                }
                else
                {
                    amount = amountWithoutVat.Value;
                }
                
                var endorsementDTO = new ContractServiceForEndorsementReceivingDto
                {
                    UnitPrice = contractService.UnitPrice,
                    Budget = contractService.Budget,
                    Discount = contractService.Discount,
                    ActivationDate = item.DateOfEffect,
                    ContractId = contractService.ContractId,
                    BranchId = contractService.BranchId.Value,
                    ContractService = item.ContractServiceId,
                    BillableAmount = amount,
                    AdminDirectTie = contractService.AdminDirectTie,
                    VAT = contractService.Vat,
                    ServiceId = contractService.ServiceId,
                    ContractStartDate = contractService.ContractStartDate,
                    ContractEndDate = contractService.ContractEndDate,
                    DocumentUrl = item.DocumentUrl,
                    BeneficiaryName = contractService.BeneficiaryName,
                    IsRequestedForApproval = true,
                    CustomerDivisionId = contractService.Contract.CustomerDivisionId,
                    BeneficiaryIdentificationType = contractService.BeneficiaryIdentificationType,
                    BenificiaryIdentificationNumber = contractService.BenificiaryIdentificationNumber,
                    EndorsementDescription = item.Description,
                    Dropofflocation = contractService.Dropofflocation,
                    DropoffDateTime = contractService.DropoffDateTime,
                    UniqueTag = contractService.UniqueTag,
                    FulfillmentEndDate = contractService.FulfillmentEndDate,
                    InvoiceCycleInDays = contractService.InvoiceCycleInDays,
                    FirstInvoiceSendDate = contractService.FirstInvoiceSendDate,
                    InvoicingInterval = (TimeCycle)contractService.InvoicingInterval.Value,
                    EndorsementTypeId = item.EndorsementType,
                    FulfillmentStartDate = contractService.FulfillmentStartDate,
                    Quantity = item.Quantity,
                    PreviousContractServiceId = item.ContractServiceId,
                    PaymentCycle = (TimeCycle)contractService.PaymentCycle.Value,
                    ProblemStatement = contractService.ProblemStatement,
                    TentativeDateOfSiteVisit = contractService.TentativeDateOfSiteVisit,
                    OfficeId = contractService.OfficeId,
                    TentativeProofOfConceptStartDate = contractService.TentativeProofOfConceptStartDate,
                    PaymentCycleInDays = contractService.PaymentCycleInDays,
                    PickupLocation = contractService.PickupLocation,
                    PickupDateTime = contractService.PickupDateTime,
                    TentativeProofOfConceptEndDate = contractService.TentativeProofOfConceptEndDate,
                    ProgramDuration = contractService.ProgramDuration,
                    ProgramCommencementDate = contractService.ProgramCommencementDate,
                    ProgramEndDate = contractService.ProgramEndDate,
                    TentativeDateForSiteSurvey = contractService.TentativeDateForSiteSurvey,
                    TentativeFulfillmentDate = contractService.TentativeFulfillmentDate,
                    DateForNewContractToTakeEffect = item.DateOfEffect,
                    QuoteServiceId = contractService.QuoteServiceId
                };

                if (string.IsNullOrEmpty(contractService.Contract.GroupInvoiceNumber))
                {
                    endorsementDTO.InvoicingInterval = 0;
                }
                endorsementDetailDTOs.Add(endorsementDTO);
            }



            try
            {
               
                responseData = await AddNewRetentionContractServiceForEndorsement(endorsementDetailDTOs);
            }
            catch (Exception ex)
            {
                _logger.LogInformation(ex.Message);
                responseData.responseMsg = "An error occured";
            }
            return responseData;
        }

        public async Task<ApiCommonResponse> FetchEndorsements(int userId)
        {
            var endorsementList = new EndorsementList();
            var endorsements = await _endorsementRepo.FindEndorsements(userId);
            if (endorsements.Count() == 0)
            {
                return CommonResponse.Send(ResponseCodes.NO_DATA_AVAILABLE); ;
            }
            var endorsementDTOs = _mapper.Map<IEnumerable<EndorsementDTO>>(endorsements);

            foreach (var item in endorsementDTOs)
            {
                item.endorsementTracking = await _endorsementRepo.TrackEndorsement(item.Id);
            }
            endorsementList.EndorsementProcessingCount = endorsementDTOs.Where(x => !x.endorsementTracking.RequestExecution.Contains("100")).Count();
            endorsementList.EndorsementHistoryCount = endorsementDTOs.Select(x => x.endorsementTracking.EndorsementHistoryCount).Count();
            endorsementList.EndorsementDTOs = endorsementDTOs;
            return CommonResponse.Send(ResponseCodes.SUCCESS, endorsementList);
        }

        public async Task<ApiCommonResponse> TrackEndorsement(long endorsementId)
        {
            var endorsement = await _endorsementRepo.TrackEndorsement(endorsementId);
            if (endorsement == null)
            {
                return CommonResponse.Send(ResponseCodes.NO_DATA_AVAILABLE); ;
            }

            return CommonResponse.Send(ResponseCodes.SUCCESS, endorsement);
        }

        public async Task<ApiCommonResponse> EndorsementReduction(int userId, List<EndorsementDTO> endorsements)
        {

            ApiCommonResponse responseData = new ApiCommonResponse();
            var endorsementDetailDTOs = new List<ContractServiceForEndorsementReceivingDto>();
            var reduction = _context.EndorsementTypes.FirstOrDefault(x => x.Caption.ToLower().Contains("reduction"))?.Id;

            if (reduction == null)
            {
                responseData.responseMsg = "An error occured";
                return responseData;
            }

            foreach (var item in endorsements)
            {
                //  item.CreatedById = userId;
                item.EndorsementType = (int)reduction.Value;
            }

            foreach (var item in endorsements)
            {
                var contractService = _context.ContractServices.Include(x => x.Contract).Include(x => x.Service).AsNoTracking().FirstOrDefault(x => x.Id == item.ContractServiceId && x.Version == 0);

                var amountWithoutVat = contractService.UnitPrice * item.Quantity - contractService.Discount;
                var amount = 0.0;

                if (contractService.Vat > 0)
                {
                    var amountWithVat = amountWithoutVat + (0.075 * amountWithoutVat);
                    amount = amountWithVat.Value;
                }
                else
                {
                    amount = amountWithoutVat.Value;
                }
                var endorsementDTO = new ContractServiceForEndorsementReceivingDto
                {
                    UnitPrice = contractService.UnitPrice,
                    Budget = contractService.Budget,
                    Discount = contractService.Discount,
                    ActivationDate = item.DateOfEffect,
                    ContractId = contractService.ContractId,
                    BranchId = contractService.BranchId.Value,
                    ContractService = item.ContractServiceId,
                    BillableAmount = amount,
                    AdminDirectTie = contractService.AdminDirectTie,
                    VAT = contractService.Vat,
                    ServiceId = contractService.ServiceId,
                    ContractStartDate = contractService.ContractStartDate,
                    ContractEndDate = contractService.ContractEndDate,
                    DocumentUrl = item.DocumentUrl,
                    BeneficiaryName = contractService.BeneficiaryName,
                    IsRequestedForApproval = true,
                    CustomerDivisionId = contractService.Contract.CustomerDivisionId,
                    BeneficiaryIdentificationType = contractService.BeneficiaryIdentificationType,
                    BenificiaryIdentificationNumber = contractService.BenificiaryIdentificationNumber,
                    EndorsementDescription = item.Description,
                    Dropofflocation = contractService.Dropofflocation,
                    DropoffDateTime = contractService.DropoffDateTime,
                    UniqueTag = contractService.UniqueTag,
                    FulfillmentEndDate = contractService.FulfillmentEndDate,
                    InvoiceCycleInDays = contractService.InvoiceCycleInDays,
                    FirstInvoiceSendDate = contractService.FirstInvoiceSendDate,
                    InvoicingInterval = (TimeCycle)contractService.InvoicingInterval.Value,
                    EndorsementTypeId = item.EndorsementType,
                    FulfillmentStartDate = contractService.FulfillmentStartDate,
                    Quantity = item.Quantity,
                    PreviousContractServiceId = item.ContractServiceId,
                    PaymentCycle = (TimeCycle)contractService.PaymentCycle.Value,
                    ProblemStatement = contractService.ProblemStatement,
                    TentativeDateOfSiteVisit = contractService.TentativeDateOfSiteVisit,
                    OfficeId = contractService.OfficeId,
                    TentativeProofOfConceptStartDate = contractService.TentativeProofOfConceptStartDate,
                    PaymentCycleInDays = contractService.PaymentCycleInDays,
                    PickupLocation = contractService.PickupLocation,
                    PickupDateTime = contractService.PickupDateTime,
                    TentativeProofOfConceptEndDate = contractService.TentativeProofOfConceptEndDate,
                    ProgramDuration = contractService.ProgramDuration,
                    ProgramCommencementDate = contractService.ProgramCommencementDate,
                    ProgramEndDate = contractService.ProgramEndDate,
                    TentativeDateForSiteSurvey = contractService.TentativeDateForSiteSurvey,
                    TentativeFulfillmentDate = contractService.TentativeFulfillmentDate,
                    DateForNewContractToTakeEffect = item.DateOfEffect,
                    QuoteServiceId = contractService.QuoteServiceId,
                };

                if (string.IsNullOrEmpty(contractService.Contract.GroupInvoiceNumber))
                {
                    endorsementDTO.InvoicingInterval = 0;
                }
                endorsementDetailDTOs.Add(endorsementDTO);
            }

            try
            {
                responseData = await AddNewRetentionContractServiceForEndorsement(endorsementDetailDTOs);

            }
            catch (Exception ex)
            {
                _logger.LogInformation(ex.Message);
                responseData.responseMsg = "An error occured";
            }
            return responseData;

        }

        public Task<ApiCommonResponse> FindEndorsement(HttpContext context, int id)
        {
            throw new NotImplementedException();
        }

        public async Task<ApiCommonResponse> GetContractService(int id)
        {
            var contractService = await _endorsementRepo.GetContractService(id);
            if (contractService == null)
            {
                return CommonResponse.Send(ResponseCodes.NO_DATA_AVAILABLE); ;
            }
            return CommonResponse.Send(ResponseCodes.SUCCESS, contractService);
        }

        public async Task<ApiCommonResponse> GetContractServices(int userId)
        {
            var contractServices = await _endorsementRepo.GetContractServices(userId);
            if (contractServices.Count() == 0)
            {
                return CommonResponse.Send(ResponseCodes.NO_DATA_AVAILABLE); ;
            }
            return CommonResponse.Send(ResponseCodes.SUCCESS, contractServices);
        }

        public async Task<ApiCommonResponse> GetPossibleDatesOfEffectForEndorsement(int contractServiceId)
        {
            var contractDates = await GetDatesOfEffectForEndorsement(contractServiceId);
            if (contractDates == null || contractDates.Count() == 0)
            {
                return CommonResponse.Send(ResponseCodes.NO_DATA_AVAILABLE); ;
            }
            return CommonResponse.Send(ResponseCodes.SUCCESS, contractDates);
        }

        private async Task<IEnumerable<DateTime>> GetDatesOfEffectForEndorsement(int contractServiceId)
        {
            var contractService = _context.ContractServices.FirstOrDefault(x => x.Id == contractServiceId);

            if (contractService == null)
                return null;


            var possibleDateList = await _context.Invoices
               .Where(x => !x.IsReversalInvoice.Value && !x.IsDeleted && !x.IsReversed.Value
                       && x.StartDate > DateTime.Now && x.ContractServiceId == contractServiceId && x.Quantity > 0 && x.IsAccountPosted == false)
               .OrderBy(x => x.StartDate)
               .Select(x => x.StartDate).ToListAsync();

            return possibleDateList;
        }

        private async Task<ApiCommonResponse> AddNewRetentionContractServiceForEndorsement(List<ContractServiceForEndorsementReceivingDto> contractServiceForEndorsement)
        {
            if (!contractServiceForEndorsement.Any())
            {
                return CommonResponse.Send(ResponseCodes.FAILURE, null, "No contract service specified");
            }

            var contractServiceForEndorsementDtos =  LinkAdminDirectServiceForEndorsement(contractServiceForEndorsement);

            if (contractServiceForEndorsementDtos == null)
            {
                return CommonResponse.Send(ResponseCodes.FAILURE, null, "Admin service cannot be endorsed");
            }

            using var transaction = await _context.Database.BeginTransactionAsync();

            var id = _context.UserProfiles.FirstOrDefault(x => x.Email.ToLower().Contains("online.portal")).Id;
            bool createNewContract = contractServiceForEndorsementDtos.Any(x => x.ContractId == 0);
            Contract newContract = null;
            List<ContractService> newContractServices = new List<ContractService>();

            if (createNewContract)
            {
                var contractDetail = contractServiceForEndorsementDtos.FirstOrDefault();
                newContract = new Contract
                {
                    CreatedAt = DateTime.Now,
                    CreatedById = id,
                    CustomerDivisionId = contractDetail.CustomerDivisionId,
                    Version = (int)VersionType.Latest,
                    GroupContractCategory = contractDetail.GroupContractCategory,
                    GroupInvoiceNumber = contractDetail.GroupInvoiceNumber,
                    IsApproved = false
                };

                var entity = await _context.Contracts.AddAsync(newContract);
                await _context.SaveChangesAsync();
                newContract = entity.Entity;
            }

            foreach (var item in contractServiceForEndorsementDtos)
            {
                bool alreadyExists = false;
                if (item.ContractId != 0)
                {
                    alreadyExists = await _context.ContractServiceForEndorsements
                       .AnyAsync(x => x.ContractId == item.ContractId && x.PreviousContractServiceId == item.PreviousContractServiceId
                                   && x.CustomerDivisionId == item.CustomerDivisionId && x.ServiceId == item.ServiceId
                                   && !x.IsApproved && !x.IsDeclined && x.IsConvertedToContractService != true && !x.IsDeleted);
                }

                if (alreadyExists)
                {
                    return CommonResponse.Send(ResponseCodes.FAILURE, null, $"There is already an endorsement request for the contract service");
                }

                //check if this is nenewal and the previous contract has not
                var previouslyRenewal = await _context.ContractServiceForEndorsements
                                                .Include(x => x.EndorsementType)
                                                .Where(x => x.PreviousContractServiceId == item.PreviousContractServiceId && x.EndorsementType.Caption.Contains("retention"))
                                                .FirstOrDefaultAsync();

                if (previouslyRenewal != null)
                {
                    return CommonResponse.Send(ResponseCodes.FAILURE, null, "There has been a retention on this contract service");
                }

                item.CreatedById = id;
                if (createNewContract)
                {
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

                    ////set approval after SBU is completed
                    //var (successful, msg) = await _approvalService.SetUpApprovalsForContractCreationEndorsement(newContract.Id, httpContext);
                    //if (!successful)
                    //{
                    //    return CommonResponse.Send(ResponseCodes.FAILURE, null, msg);
                    //}
                }
                else
                {
                    foreach (var item in entityToSaveList)
                    {
                        var savedEntity = await _endorsementRepo.SaveContractServiceForEndorsement(item);
                        if (savedEntity == null)
                        {
                            return CommonResponse.Send(ResponseCodes.FAILURE, null, "Some system errors occurred");
                        }

                        bool successful = await _endorsementRepo.SetUpApprovalsForContractModificationEndorsement(savedEntity);
                        if (!successful)
                        {
                            return CommonResponse.Send(ResponseCodes.FAILURE, null, "Could not set up approvals for service endorsement.");
                        }
                    }
                }

                await _context.SaveChangesAsync();
                await transaction.CommitAsync();
                return CommonResponse.Send(ResponseCodes.SUCCESS);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                _logger.LogError(ex.StackTrace);
                return CommonResponse.Send(ResponseCodes.FAILURE, null, ex.Message);
            }
        }

        private List<ContractServiceForEndorsementReceivingDto> LinkAdminDirectServiceForEndorsement(List<ContractServiceForEndorsementReceivingDto> contractServices)
                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                     {
            var adminDirectServices = new List<ContractServiceForEndorsementReceivingDto>();
            var nonAdminDirectServices = new List<ContractServiceForEndorsementReceivingDto>();
            var directServices = new List<ContractServiceForEndorsementReceivingDto>();
            var adminServices = new List<ContractServiceForEndorsementReceivingDto>();
            var directSeviceIds = new List<int>();
            var adminDirectServiceIds = _context.ServiceRelationships.Select(x => new AdminDirectObj
            {
                AdminId = (int)x.AdminServiceId,
                DirectId = (int)x.DirectServiceId
            });
            foreach (var item in contractServices)
            {
                if (adminDirectServiceIds.Any(x => x.AdminId == item.ServiceId))
                {
                    return null;
                }
            }

            foreach (var item in contractServices)
            {
                if (adminDirectServiceIds.Any(x => x.DirectId == item.ServiceId))
                {
                    directServices.Add(item);
                }
                else
                {
                    nonAdminDirectServices.Add(item);
                }
            }


            foreach (var item in directServices)
            {
                var adminId = adminDirectServiceIds.FirstOrDefault(x => x.DirectId == item.ServiceId).AdminId;
                var adminContractService = _context.ContractServices.FirstOrDefault(x => x.ContractId == item.ContractId && x.AdminDirectTie == item.AdminDirectTie && x.Version == 0 && x.ServiceId == adminId);
                var amountWithoutVat = adminContractService.UnitPrice * item.Quantity - adminContractService.Discount;
                var amount = 0.0;

                if (adminContractService.Vat > 0)
                {
                    var amountWithVat = amountWithoutVat + (0.075 * amountWithoutVat);
                    amount = amountWithVat.Value;
                }
                else
                {
                    amount = amountWithoutVat.Value;
                }

                adminServices.Add(new ContractServiceForEndorsementReceivingDto
                {
                    ActivationDate = adminContractService.ActivationDate,
                    AdminDirectTie = adminContractService.AdminDirectTie,
                    BillableAmount = amount,
                    VAT = adminContractService.Vat,
                    UnitPrice = adminContractService.UnitPrice,
                    Quantity = item.Quantity,
                    IsApproved = item.IsApproved,
                    IsRequestedForApproval = item.IsRequestedForApproval,
                    BeneficiaryIdentificationType = adminContractService.BeneficiaryIdentificationType,
                    BeneficiaryName = adminContractService.BeneficiaryName,
                    Budget = item.Budget,
                    ContractId = adminContractService.ContractId,
                    CustomerDivisionId = item.CustomerDivisionId,
                    EndorsementTypeId = item.EndorsementTypeId,
                    EndorsementDescription = item.EndorsementDescription,
                    BenificiaryIdentificationNumber = item.BenificiaryIdentificationNumber,
                    BranchId = item.BranchId,
                    ServiceId = adminContractService.ServiceId,
                    PickupLocation = item.PickupLocation,
                    ContractEndDate = adminContractService.ContractEndDate,
                    ContractStartDate = adminContractService.ContractStartDate,
                    FirstInvoiceSendDate = adminContractService.FirstInvoiceSendDate,
                    CreatedById = item.CreatedById,
                    QuoteServiceId = adminContractService.QuoteServiceId,
                    DateForNewContractToTakeEffect = item.DateForNewContractToTakeEffect,
                    Discount = item.Discount,
                    FulfillmentStartDate = adminContractService.FulfillmentStartDate,
                    DocumentUrl = item.DocumentUrl,
                    DropoffDateTime = item.DropoffDateTime,
                    PreviousContractServiceId = adminContractService.Id,
                    UniqueTag = adminContractService.UniqueTag,
                    PaymentCycleInDays = item.PaymentCycleInDays,
                    FulfillmentEndDate = adminContractService.FulfillmentEndDate,
                    InvoicingInterval = (TimeCycle)adminContractService.InvoicingInterval,
                    GroupInvoiceNumber = item.GroupInvoiceNumber,
                    ProblemStatement = item.ProblemStatement,
                    PaymentCycle = (TimeCycle)adminContractService.PaymentCycle,
                    OfficeId = item.OfficeId,
                });
                                                                                                                                                                                                                                                                                                                                                                                    }
            adminDirectServices.AddRange(directServices);
            adminDirectServices.AddRange(adminServices);
            adminDirectServices.AddRange(nonAdminDirectServices);
            return adminDirectServices;

        }

        private bool ValidateAdminAccompaniesDirectService(List<ContractServiceForEndorsementReceivingDto> ContractServices)
        {
            var isValidCount = 0;
            var adminServiceCount = 0;

            foreach (var contractService in ContractServices)
            {
                var directServiceExist = false;
                var adminServiceExist = false;
                var adminDirectService = _context.ServiceRelationships.FirstOrDefault(x => x.DirectServiceId == contractService.ServiceId || x.AdminServiceId == contractService.ServiceId);
                foreach (var item in ContractServices)
                {
                    if (adminDirectService != null)
                    {
                        if (item.ServiceId == adminDirectService?.AdminServiceId)
                        {
                            adminServiceExist = true;
                            adminServiceCount++;
                        }
                        if (item.ServiceId == adminDirectService?.DirectServiceId)
                        {
                            directServiceExist = true;
                        }
                    }
                }
                if (directServiceExist && adminServiceExist)
                {
                    isValidCount++;
                }
            }

            if (isValidCount == adminServiceCount)
            {
                return true;
            }
            return false;
        }
    }
    class AdminDirectObj
    {
        public int AdminId { get; set; }
        public int DirectId { get; set; }
    }
    
}
