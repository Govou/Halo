using AutoMapper;
using HaloBiz.DTOs.ApiDTOs;
using HaloBiz.DTOs.ReceivingDTOs;
using HaloBiz.DTOs.TransferDTOs;
using HaloBiz.Helpers;
using HaloBiz.Model;
using HaloBiz.Repository;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HaloBiz.MyServices.Impl
{
    public class ServiceTypeServiceImpl : IServiceTypeService
    {
        private readonly ILogger<ServiceTypeServiceImpl> _logger;
        private readonly IModificationHistoryRepository _historyRepo;
        private readonly IServiceTypeRepository _serviceTypeRepo;
        private readonly IMapper _mapper;

        public ServiceTypeServiceImpl(IModificationHistoryRepository historyRepo, IServiceTypeRepository ServiceTypeRepo, ILogger<ServiceTypeServiceImpl> logger, IMapper mapper)
        {
            this._mapper = mapper;
            this._historyRepo = historyRepo;
            this._serviceTypeRepo = ServiceTypeRepo;
            this._logger = logger;
        }

        public async Task<ApiResponse> AddServiceType(HttpContext context, ServiceTypeReceivingDTO serviceTypeReceivingDTO)
        {

            var serviceType = _mapper.Map<ServiceType>(serviceTypeReceivingDTO);
            serviceType.CreatedById = context.GetLoggedInUserId();
            var savedserviceType = await _serviceTypeRepo.SaveServiceType(serviceType);
            if (savedserviceType == null)
            {
                return new ApiResponse(500);
            }
            var serviceTypeTransferDTO = _mapper.Map<ServiceTypeTransferDTO>(serviceType);
            return new ApiOkResponse(serviceTypeTransferDTO);
        }

        public async Task<ApiResponse> DeleteServiceType(long id)
        {
            var serviceTypeToDelete = await _serviceTypeRepo.FindServiceTypeById(id);
            if (serviceTypeToDelete == null)
            {
                return new ApiResponse(404);
            }

            if (!await _serviceTypeRepo.DeleteServiceType(serviceTypeToDelete))
            {
                return new ApiResponse(500);
            }

            return new ApiOkResponse(true);
        }

        public async Task<ApiResponse> GetAllServiceType()
        {
            var serviceTypes = await _serviceTypeRepo.FindAllServiceTypes();
            if (serviceTypes == null)
            {
                return new ApiResponse(404);
            }
            var serviceTypeTransferDTO = _mapper.Map<IEnumerable<ServiceTypeTransferDTO>>(serviceTypes);
            return new ApiOkResponse(serviceTypeTransferDTO);
        }

        public async Task<ApiResponse> GetServiceTypeById(long id)
        {
            var serviceType = await _serviceTypeRepo.FindServiceTypeById(id);
            if (serviceType == null)
            {
                return new ApiResponse(404);
            }
            var serviceTypeTransferDTOs = _mapper.Map<ServiceTypeTransferDTO>(serviceType);
            return new ApiOkResponse(serviceTypeTransferDTOs);
        }

        public async Task<ApiResponse> GetServiceTypeByName(string name)
        {
            var serviceType = await _serviceTypeRepo.FindServiceTypeByName(name);
            if (serviceType == null)
            {
                return new ApiResponse(404);
            }
            var serviceTypeTransferDTOs = _mapper.Map<ServiceTypeTransferDTO>(serviceType);
            return new ApiOkResponse(serviceTypeTransferDTOs);
        }

        public async Task<ApiResponse> UpdateServiceType(HttpContext context, long id, ServiceTypeReceivingDTO serviceTypeReceivingDTO)
        {
            var serviceTypeToUpdate = await _serviceTypeRepo.FindServiceTypeById(id);
            if (serviceTypeToUpdate == null)
            {
                return new ApiResponse(404);
            }

            var summary = $"Initial details before change, \n {serviceTypeToUpdate.ToString()} \n";

            serviceTypeToUpdate.Caption = serviceTypeReceivingDTO.Caption;
            serviceTypeToUpdate.Description = serviceTypeReceivingDTO.Description;
            var updatedserviceType = await _serviceTypeRepo.UpdateServiceType(serviceTypeToUpdate);

            summary += $"Details after change, \n {updatedserviceType.ToString()} \n";

            if (updatedserviceType == null)
            {
                return new ApiResponse(500);
            }
            ModificationHistory history = new ModificationHistory()
            {
                ModelChanged = "serviceType",
                ChangeSummary = summary,
                ChangedById = context.GetLoggedInUserId(),
                ModifiedModelId = updatedserviceType.Id
            };
            await _historyRepo.SaveHistory(history);

            var serviceTypeTransferDTOs = _mapper.Map<ServiceTypeTransferDTO>(updatedserviceType);
            return new ApiOkResponse(serviceTypeTransferDTOs);
        }
    }
}
