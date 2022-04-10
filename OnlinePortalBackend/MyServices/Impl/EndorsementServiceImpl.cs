using AutoMapper;
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
using HaloBiz.Helpers;

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

        public async Task<ApiCommonResponse> EndorsementTopUp(HttpContext context, List<EndorsementDTO> endorsements)
        {
            ApiCommonResponse responseData = new ApiCommonResponse();
            var endorsementDetailDTOs = new List<ContractServiceForEndorsementReceivingDto>();
            var userId = context.GetLoggedInUserId();
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
                var contractService = _context.ContractServices.Include(x => x.Contract).Include(x => x.Service).AsNoTracking().FirstOrDefault(x => x.Id == item.ContractServiceId);
                var endorsementDTO = new ContractServiceForEndorsementReceivingDto
                {
                    UnitPrice = contractService.UnitPrice,
                    Budget = contractService.Budget,
                    Discount = contractService.Discount,
                    ActivationDate = item.DateOfEffect,
                    ContractId = contractService.ContractId,
                    BranchId = contractService.BranchId.Value,
                    ContractService = item.ContractServiceId,
                    BillableAmount = contractService.UnitPrice * item.Quantity,
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
                    Quantity = contractService.Quantity,
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
                    DateForNewContractToTakeEffect = item.DateOfEffect
                };
                endorsementDetailDTOs.Add(endorsementDTO);
            }

          

            try
            {
                //var token = await _apiInterceptor.GetToken();
                //var baseUrl = string.Concat(_HalobizBaseUrl, "Endorsement");


                //var response = await baseUrl.AllowAnyHttpStatus().WithOAuthBearerToken($"{token}")
                //   .PostJsonAsync(endorsementDetailDTOs)?.ReceiveJson();

                //foreach (KeyValuePair<string, object> kvp in (IDictionary<string, object>)response)
                //{
                //    if (kvp.Key.ToString() == "responseCode")
                //    {
                //        responseData.responseCode = kvp.Value.ToString();
                //    }
                //    if (kvp.Key.ToString() == "responseData")
                //    {
                //        responseData.responseData = kvp.Value;
                //    }
                //    if (kvp.Key.ToString() == "responseMsg")
                //    {
                //        responseData.responseMsg = kvp.Value.ToString();
                //    }
                //}
                responseData = await AddNewRetentionContractServiceForEndorsement(endorsementDetailDTOs);
            }
            catch (Exception ex)
            {
                _logger.LogInformation(ex.Message);
                responseData.responseMsg = "An error occured";
            }
            return responseData;
        }

        public async Task<ApiCommonResponse> FetchEndorsements(HttpContext context, int limit = 10)
        {
            var endorsements = await _endorsementRepo.FindEndorsements(context.GetLoggedInUserId(), limit);
            if (endorsements.Count() == 0)
            {
                return CommonResponse.Send(ResponseCodes.NO_DATA_AVAILABLE); ;
            }
            var endorsementDTOs = _mapper.Map<IEnumerable<EndorsementDTO>>(endorsements);
            return CommonResponse.Send(ResponseCodes.SUCCESS, endorsementDTOs);
        }

        public async Task<ApiCommonResponse> TrackEndorsement(HttpContext context, long endorsementId)
        {
            var endorsement = await _endorsementRepo.FindEndorsementById(context.GetLoggedInUserId(), endorsementId);
            var result = string.Empty;
            if (endorsement == null)
            {
                return CommonResponse.Send(ResponseCodes.NO_DATA_AVAILABLE); ;
            }

            if (endorsement.IsDeclined) result = "Declined";
            else if (endorsement.IsApproved) result = "Approved";
            else result = "Pending";

            return CommonResponse.Send(ResponseCodes.SUCCESS, result);
        }

        public async Task<ApiCommonResponse> EndorsementReduction(HttpContext context, List<EndorsementDTO> endorsements)
        {

            ApiCommonResponse responseData = new ApiCommonResponse();
            var endorsementDetailDTOs = new List<ContractServiceForEndorsementReceivingDto>();
            var userId = context.GetLoggedInUserId();
            var topup = _context.EndorsementTypes.FirstOrDefault(x => x.Caption.ToLower().Contains("reduction"))?.Id;

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
                var contractService = _context.ContractServices.Include(x => x.Contract).Include(x => x.Service).FirstOrDefault(x => x.Id == item.ContractServiceId);
                var endorsementDTO = new ContractServiceForEndorsementReceivingDto
                {
                    UnitPrice = contractService.UnitPrice,
                    Budget = contractService.Budget,
                    Discount = contractService.Discount,
                    ActivationDate = item.DateOfEffect,
                    ContractId = contractService.ContractId,
                    BranchId = contractService.BranchId.Value,
                    ContractService = item.ContractServiceId,
                    BillableAmount = contractService.UnitPrice * item.Quantity,
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
                    Quantity = contractService.Quantity,
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
                    DateForNewContractToTakeEffect = item.DateOfEffect
                };
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

        //public Task<ApiCommonResponse> EndorsementCancellation(HttpContext context, int id)
        //{
        //   var endorsement = _context.ContractServiceForEndorsements.FirstOrDefault(x => x.Id == id &&);
        //    if (endorsement == null)
        //    {
        //        return null;
        //    }

        //}

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
            {
                return null;
            }

            DateTime startdate = contractService.ContractStartDate.Value;
            DateTime enddate = contractService.ContractEndDate.Value;
            var day = startdate.Day;
            var month = DateTime.Today.Month;
            var year = DateTime.Today.Year;

            var dateList = new List<DateTime>();
            var months = enddate - DateTime.Today;
            var monthCount = Math.Floor(months.TotalDays / 30);

            for (int i = 1; i < monthCount - 1; i++)
            {
                if (month == 2)
                {
                    dateList.Add(new DateTime(year, month, 29).AddMonths(i));
                }
                else
                {
                    try
                    {
                        dateList.Add(new DateTime(year, month, day).AddMonths(i));
                    }
                    catch (Exception)
                    {
                        dateList.Add(new DateTime(year, month, day - 1).AddMonths(i));
                    }

                }
               
            }

            return dateList;
        }

        private async Task<ApiCommonResponse> AddNewRetentionContractServiceForEndorsement(List<ContractServiceForEndorsementReceivingDto> contractServiceForEndorsementDtos)
        {
            if (!contractServiceForEndorsementDtos.Any())
            {
                return CommonResponse.Send(ResponseCodes.FAILURE, null, "No contract service specified");
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
                    return CommonResponse.Send(ResponseCodes.FAILURE, null, $"There is already an endorsement request for the contract service with id {item.ContractId}");
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
    }
}
