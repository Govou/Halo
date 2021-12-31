using AutoMapper;
using HaloBiz.DTOs.ApiDTOs;
using HaloBiz.DTOs.ReceivingDTOs;
using HaloBiz.DTOs.TransferDTOs;
using HaloBiz.Helpers;
using HalobizMigrations.Models;
using HaloBiz.Repository;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HalobizMigrations.Data;
using Microsoft.EntityFrameworkCore;

namespace HaloBiz.MyServices.Impl
{
    public class ServicePricingServiceImpl : IServicePricingService
    {
        private readonly ILogger<ServicePricingServiceImpl> _logger;
        private readonly IModificationHistoryRepository _historyRepo;
        private readonly IServicePricingRepository _servicePricingRepo;
        private readonly HalobizContext _context;
        private readonly IMapper _mapper;

        public ServicePricingServiceImpl(IModificationHistoryRepository historyRepo,
            IServicePricingRepository ServicePricingRepo,
            HalobizContext context,
            ILogger<ServicePricingServiceImpl> logger, 
            IMapper mapper)
        {
            this._mapper = mapper;
            this._historyRepo = historyRepo;
            this._servicePricingRepo = ServicePricingRepo;
            _context = context;
            this._logger = logger;
        }

        public async Task<ApiResponse> AddServicePricing(HttpContext context, ServicePricingReceivingDTO servicePricingReceivingDTO)
        {
            var servicePricingInDb = await _context.ServicePricings.Where(x => x.ServiceId == servicePricingReceivingDTO.ServiceId && x.BranchId == servicePricingReceivingDTO.BranchId)
                .ToListAsync();

            if(servicePricingInDb.Count > 0)
            {
                return new ApiResponse(400, "Service Pricing already configured.");
            }

            var servicePricing = _mapper.Map<ServicePricing>(servicePricingReceivingDTO);
            servicePricing.CreatedById = context.GetLoggedInUserId();
            var savedservicePricing = await _servicePricingRepo.SaveServicePricing(servicePricing);
            if (savedservicePricing == null)
            {
                return new ApiResponse(500);
            }
            var servicePricingTransferDTO = _mapper.Map<ServicePricingTransferDTO>(servicePricing);
            return new ApiOkResponse(servicePricingTransferDTO);
        }

        public async Task<ApiResponse> DeleteServicePricing(long id)
        {
            var servicePricingToDelete = await _servicePricingRepo.FindServicePricingById(id);
            if (servicePricingToDelete == null)
            {
                return new ApiResponse(404);
            }

            if (!await _servicePricingRepo.DeleteServicePricing(servicePricingToDelete))
            {
                return new ApiResponse(500);
            }

            return new ApiOkResponse(true);
        }

        public async Task<ApiResponse> GetAllServicePricing()
        {
            var servicePricings = await _servicePricingRepo.FindAllServicePricings();
            if (servicePricings == null)
            {
                return new ApiResponse(404);
            }
            var servicePricingTransferDTO = _mapper.Map<IEnumerable<ServicePricingTransferDTO>>(servicePricings);
            return new ApiOkResponse(servicePricingTransferDTO);
        }

        public async Task<ApiResponse> GetServicePricingById(long id)
        {
            var servicePricing = await _servicePricingRepo.FindServicePricingById(id);
            if (servicePricing == null)
            {
                return new ApiResponse(404);
            }
            var servicePricingTransferDTOs = _mapper.Map<ServicePricingTransferDTO>(servicePricing);
            return new ApiOkResponse(servicePricingTransferDTOs);
        }

        public async Task<ApiResponse> GetServicePricingByServiceId(long serviceId)
        {
            var servicePricings = await _servicePricingRepo.FindServicePricingByServiceId(serviceId);
            if (servicePricings == null)
            {
                return new ApiResponse(404);
            }
            var servicePricingTransferDTOs = _mapper.Map<IEnumerable<ServicePricingTransferDTO>>(servicePricings);
            return new ApiOkResponse(servicePricingTransferDTOs);
        }

        public async Task<ApiResponse> GetServicePricingByBranchId(long branchId)
        {
            var servicePricings = await _servicePricingRepo.FindServicePricingByBranchId(branchId);
            if (servicePricings == null)
            {
                return new ApiResponse(404);
            }
            var servicePricingTransferDTOs = _mapper.Map<IEnumerable<ServicePricingTransferDTO>>(servicePricings);
            return new ApiOkResponse(servicePricingTransferDTOs);
        }

        public async Task<ApiResponse> UpdateServicePricing(HttpContext context, long id, ServicePricingReceivingDTO servicePricingReceivingDTO)
        {
            var servicePricingToUpdate = await _servicePricingRepo.FindServicePricingById(id);
            if (servicePricingToUpdate == null)
            {
                return new ApiResponse(404);
            }

            var servicePricingInDb = await _context.ServicePricings.Where(x => x.ServiceId == servicePricingReceivingDTO.ServiceId && x.BranchId == servicePricingReceivingDTO.BranchId)
                .ToListAsync();

            if (servicePricingInDb.Count > 0)
            {
                return new ApiResponse(400, "Service Pricing already configured.");
            }
            
            var summary = $"Initial details before change, \n {servicePricingToUpdate.ToString()} \n";

            servicePricingToUpdate.BranchId = servicePricingReceivingDTO.BranchId;
            servicePricingToUpdate.MaximumAmount = servicePricingReceivingDTO.MaximumAmount;
            servicePricingToUpdate.MinimumAmount = servicePricingReceivingDTO.MinimumAmount;
            servicePricingToUpdate.ServiceId = servicePricingReceivingDTO.ServiceId;

            var updatedservicePricing = await _servicePricingRepo.UpdateServicePricing(servicePricingToUpdate);

            summary += $"Details after change, \n {updatedservicePricing.ToString()} \n";

            if (updatedservicePricing == null)
            {
                return new ApiResponse(500);
            }
            ModificationHistory history = new ModificationHistory()
            {
                ModelChanged = "ServicePricing",
                ChangeSummary = summary,
                ChangedById = context.GetLoggedInUserId(),
                ModifiedModelId = updatedservicePricing.Id
            };
            await _historyRepo.SaveHistory(history);

            var servicePricingTransferDTOs = _mapper.Map<ServicePricingTransferDTO>(updatedservicePricing);
            return new ApiOkResponse(servicePricingTransferDTOs);
        }
    }
}
