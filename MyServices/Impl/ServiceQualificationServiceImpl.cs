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
using HalobizMigrations.Models.Halobiz;

namespace HaloBiz.MyServices.Impl
{
    public class ServiceQualificationServiceImpl : IServiceQualificationService
    {
        private readonly ILogger<ServiceQualificationServiceImpl> _logger;
        private readonly IModificationHistoryRepository _historyRepo;
        private readonly IServiceQualificationRepository _serviceQualificationRepo;
        private readonly IMapper _mapper;

        public ServiceQualificationServiceImpl(IModificationHistoryRepository historyRepo, IServiceQualificationRepository ServiceQualificationRepo, ILogger<ServiceQualificationServiceImpl> logger, IMapper mapper)
        {
            this._mapper = mapper;
            this._historyRepo = historyRepo;
            this._serviceQualificationRepo = ServiceQualificationRepo;
            this._logger = logger;
        }

        public async Task<ApiResponse> AddServiceQualification(HttpContext context, ServiceQualificationReceivingDTO serviceQualificationReceivingDTO)
        {

            var serviceQualification = _mapper.Map<ServiceQualification>(serviceQualificationReceivingDTO);
            serviceQualification.CreatedById = context.GetLoggedInUserId();
            var savedserviceQualification = await _serviceQualificationRepo.SaveServiceQualification(serviceQualification);
            if (savedserviceQualification == null)
            {
                return new ApiResponse(500);
            }
            var serviceQualificationTransferDTO = _mapper.Map<ServiceQualificationTransferDTO>(serviceQualification);
            return new ApiOkResponse(serviceQualificationTransferDTO);
        }

        public async Task<ApiResponse> DeleteServiceQualification(long id)
        {
            var serviceQualificationToDelete = await _serviceQualificationRepo.FindServiceQualificationById(id);
            if (serviceQualificationToDelete == null)
            {
                return new ApiResponse(404);
            }

            if (!await _serviceQualificationRepo.DeleteServiceQualification(serviceQualificationToDelete))
            {
                return new ApiResponse(500);
            }

            return new ApiOkResponse(true);
        }

        public async Task<ApiResponse> GetAllServiceQualification()
        {
            var serviceQualifications = await _serviceQualificationRepo.FindAllServiceQualifications();
            if (serviceQualifications == null)
            {
                return new ApiResponse(404);
            }
            var serviceQualificationTransferDTO = _mapper.Map<IEnumerable<ServiceQualificationTransferDTO>>(serviceQualifications);
            return new ApiOkResponse(serviceQualificationTransferDTO);
        }

        public async Task<ApiResponse> GetServiceQualificationById(long id)
        {
            var serviceQualification = await _serviceQualificationRepo.FindServiceQualificationById(id);
            if (serviceQualification == null)
            {
                return new ApiResponse(404);
            }
            var serviceQualificationTransferDTOs = _mapper.Map<ServiceQualificationTransferDTO>(serviceQualification);
            return new ApiOkResponse(serviceQualificationTransferDTOs);
        }

        /*public async Task<ApiResponse> GetServiceQualificationByName(string name)
        {
            var serviceQualification = await _serviceQualificationRepo.FindServiceQualificationByName(name);
            if (serviceQualification == null)
            {
                return new ApiResponse(404);
            }
            var serviceQualificationTransferDTOs = _mapper.Map<ServiceQualificationTransferDTO>(serviceQualification);
            return new ApiOkResponse(serviceQualificationTransferDTOs);
        }*/

        public async Task<ApiResponse> UpdateServiceQualification(HttpContext context, long id, ServiceQualificationReceivingDTO serviceQualificationReceivingDTO)
        {
            var serviceQualificationToUpdate = await _serviceQualificationRepo.FindServiceQualificationById(id);
            if (serviceQualificationToUpdate == null)
            {
                return new ApiResponse(404);
            }

            var summary = $"Initial details before change, \n {serviceQualificationToUpdate.ToString()} \n";

            serviceQualificationToUpdate.Budget = serviceQualificationReceivingDTO.Budget;
            serviceQualificationToUpdate.QuantityEstimate = serviceQualificationReceivingDTO.QuantityEstimate;
            var updatedserviceQualification = await _serviceQualificationRepo.UpdateServiceQualification(serviceQualificationToUpdate);

            summary += $"Details after change, \n {updatedserviceQualification.ToString()} \n";

            if (updatedserviceQualification == null)
            {
                return new ApiResponse(500);
            }
            ModificationHistory history = new ModificationHistory()
            {
                ModelChanged = "serviceQualification",
                ChangeSummary = summary,
                ChangedById = context.GetLoggedInUserId(),
                ModifiedModelId = updatedserviceQualification.Id
            };
            await _historyRepo.SaveHistory(history);

            var serviceQualificationTransferDTOs = _mapper.Map<ServiceQualificationTransferDTO>(updatedserviceQualification);
            return new ApiOkResponse(serviceQualificationTransferDTOs);
        }
    }
}
