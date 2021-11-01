using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using HaloBiz.DTOs.ApiDTOs;
using HaloBiz.DTOs.ReceivingDTOs;
using HaloBiz.DTOs.TransferDTOs.LAMS;
using HaloBiz.Helpers;
using HalobizMigrations.Models;

using HaloBiz.MyServices.LAMS;
using HaloBiz.Repository;
using HaloBiz.Repository.LAMS;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace HaloBiz.MyServices.Impl.LAMS
{
    public class QuoteServiceServiceImpl : IQuoteServiceService
    {
        private readonly ILogger<QuoteServiceServiceImpl> _logger;
        private readonly IModificationHistoryRepository _historyRepo;
        private readonly IQuoteServiceRepository _quoteServiceRepo;
        private readonly IMapper _mapper;

        public QuoteServiceServiceImpl(IModificationHistoryRepository historyRepo, IQuoteServiceRepository quoteServiceRepo, ILogger<QuoteServiceServiceImpl> logger, IMapper mapper)
        {
            this._mapper = mapper;
            this._historyRepo = historyRepo;
            this._quoteServiceRepo = quoteServiceRepo;
            this._logger = logger;
        }

        public async Task<ApiResponse> AddQuoteService(HttpContext context, QuoteServiceReceivingDTO quoteServiceReceivingDTO)
        {
            var quoteService = _mapper.Map<QuoteService>(quoteServiceReceivingDTO);
            quoteService.CreatedById = context.GetLoggedInUserId();
            var savedQuoteService = await _quoteServiceRepo.SaveQuoteService(quoteService);
            if (savedQuoteService == null)
            {
                return new ApiResponse(500);
            }
            var quoteServiceTransferDTO = _mapper.Map<QuoteServiceTransferDTO>(savedQuoteService);
            return new ApiOkResponse(quoteServiceTransferDTO);
        }

        public async Task<ApiResponse> GetAllQuoteService()
        {
            var quoteService = await _quoteServiceRepo.FindAllQuoteService();
            if (quoteService == null)
            {
                return new ApiResponse(404);
            }
            var quoteServiceTransferDTO = _mapper.Map<IEnumerable<QuoteServiceTransferDTO>>(quoteService);
            return new ApiOkResponse(quoteServiceTransferDTO);
        }

        public async Task<ApiResponse> GetQuoteServiceById(long id)
        {
            var quoteService = await _quoteServiceRepo.FindQuoteServiceById(id);
            if (quoteService == null)
            {
                return new ApiResponse(404);
            }
            var quoteServiceTransferDTOs = _mapper.Map<QuoteServiceTransferDTO>(quoteService);
            return new ApiOkResponse(quoteServiceTransferDTOs);
        }

        public async Task<ApiResponse> GetQuoteServiceByTag(string tag)
        {
            var quoteService = await _quoteServiceRepo.FindQuoteServiceByTag(tag);
            if (quoteService == null)
            {
                return new ApiResponse(404);
            }
            var quoteServiceTransferDTOs = _mapper.Map<QuoteServiceTransferDTO>(quoteService);
            return new ApiOkResponse(quoteServiceTransferDTOs);
        }

        public async Task<ApiResponse> UpdateQuoteServicesByQuoteId(HttpContext context, long quoteId, IEnumerable<QuoteServiceReceivingDTO> quoteServices)
        {
            var userId = context.GetLoggedInUserId();
            var _quoteServices = _mapper.Map<IEnumerable<QuoteService>>(quoteServices);
            var response = await _quoteServiceRepo.UpdateQuoteServicesByQuoteId(quoteId, _quoteServices);
            return new ApiOkResponse(response);

        }
        public async Task<ApiResponse> UpdateQuoteService(HttpContext context, long id, QuoteServiceReceivingDTO quoteServiceReceivingDTO)
        {
            var quoteServiceToUpdate = await _quoteServiceRepo.FindQuoteServiceById(id);
            if (quoteServiceToUpdate == null)
            {
                return new ApiResponse(404);
            }
            
            var summary = $"Initial details before change, \n {quoteServiceToUpdate.ToString()} \n" ;

            quoteServiceToUpdate.UnitPrice = quoteServiceReceivingDTO.UnitPrice;
            quoteServiceToUpdate.Quantity = quoteServiceReceivingDTO.Quantity;
            quoteServiceToUpdate.Discount = quoteServiceReceivingDTO.Discount;
            quoteServiceToUpdate.Vat = quoteServiceReceivingDTO.VAT;
            quoteServiceToUpdate.BillableAmount = quoteServiceReceivingDTO.BillableAmount;
            quoteServiceToUpdate.Budget = quoteServiceReceivingDTO.Budget;
            quoteServiceToUpdate.ContractStartDate = quoteServiceReceivingDTO.ContractStartDate;
            quoteServiceToUpdate.ContractEndDate = quoteServiceReceivingDTO.ContractEndDate;
            quoteServiceToUpdate.PaymentCycle = (int)quoteServiceReceivingDTO.PaymentCycle;
            quoteServiceToUpdate.PaymentCycleInDays = quoteServiceReceivingDTO.PaymentCycleInDays;
            quoteServiceToUpdate.FirstInvoiceSendDate = quoteServiceReceivingDTO.FirstInvoiceSendDate;
            quoteServiceToUpdate.InvoicingInterval = (int)quoteServiceReceivingDTO.InvoicingInterval;
            quoteServiceToUpdate.ProblemStatement = quoteServiceReceivingDTO.ProblemStatement;
            quoteServiceToUpdate.ActivationDate = quoteServiceReceivingDTO.ActivationDate;
            quoteServiceToUpdate.FulfillmentStartDate = quoteServiceReceivingDTO.FulfillmentStartDate;
            quoteServiceToUpdate.FulfillmentEndDate = quoteServiceReceivingDTO.FulfillmentEndDate;
            quoteServiceToUpdate.TentativeDateForSiteSurvey = quoteServiceReceivingDTO.TentativeDateForSiteSurvey;
            quoteServiceToUpdate.PickupDateTime = quoteServiceReceivingDTO.PickupDateTime;
            quoteServiceToUpdate.DropoffDateTime = quoteServiceReceivingDTO.DropoffDateTime;
            quoteServiceToUpdate.PickupLocation = quoteServiceReceivingDTO.PickupLocation;
            quoteServiceToUpdate.Dropofflocation = quoteServiceReceivingDTO.Dropofflocation;
            quoteServiceToUpdate.BeneficiaryName = quoteServiceReceivingDTO.BeneficiaryName;
            quoteServiceToUpdate.BeneficiaryIdentificationType = quoteServiceReceivingDTO.BeneficiaryIdentificationType;
            quoteServiceToUpdate.BenificiaryIdentificationNumber = quoteServiceReceivingDTO.BenificiaryIdentificationNumber;
            quoteServiceToUpdate.TentativeProofOfConceptStartDate = quoteServiceReceivingDTO.TentativeProofOfConceptStartDate;
            quoteServiceToUpdate.TentativeProofOfConceptEndDate = quoteServiceReceivingDTO.TentativeProofOfConceptEndDate;
            quoteServiceToUpdate.TentativeFulfillmentDate = quoteServiceReceivingDTO.TentativeFulfillmentDate;
            quoteServiceToUpdate.ProgramCommencementDate = quoteServiceReceivingDTO.ProgramCommencementDate;
            quoteServiceToUpdate.ProgramDuration = quoteServiceReceivingDTO.ProgramDuration;
            quoteServiceToUpdate.ProgramEndDate = quoteServiceReceivingDTO.ProgramEndDate;
            quoteServiceToUpdate.TentativeDateOfSiteVisit = quoteServiceReceivingDTO.TentativeDateOfSiteVisit;
            quoteServiceToUpdate.IsConvertedToContractService = quoteServiceReceivingDTO.IsConvertedToContractService;

            quoteServiceToUpdate.ServiceId = quoteServiceReceivingDTO.ServiceId;

            var updatedQuoteService = await _quoteServiceRepo.UpdateQuoteService(quoteServiceToUpdate);

            summary += $"Details after change, \n {updatedQuoteService.ToString()} \n";

            if (updatedQuoteService == null)
            {
                return new ApiResponse(500);
            }
            ModificationHistory history = new ModificationHistory(){
                ModelChanged = "QuoteService",
                ChangeSummary = summary,
                ChangedById = context.GetLoggedInUserId(),
                ModifiedModelId = updatedQuoteService.Id
            };

            await _historyRepo.SaveHistory(history);

            var quoteServiceTransferDTOs = _mapper.Map<QuoteServiceTransferDTO>(updatedQuoteService);
            return new ApiOkResponse(quoteServiceTransferDTOs);

        }

        public async Task<ApiResponse> DeleteQuoteService(long id)
        {
            var quoteServiceToDelete = await _quoteServiceRepo.FindQuoteServiceById(id);
            if (quoteServiceToDelete == null)
            {
                return new ApiResponse(404);
            }

            if (!await _quoteServiceRepo.DeleteQuoteService(quoteServiceToDelete))
            {
                return new ApiResponse(500);
            }

            return new ApiOkResponse(true);
        }
    }
}