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

        public async Task<ApiCommonResponse> AddServiceType(HttpContext context, ServiceTypeReceivingDTO serviceTypeReceivingDTO)
        {

            var serviceType = _mapper.Map<ServiceType>(serviceTypeReceivingDTO);
            serviceType.CreatedById = context.GetLoggedInUserId();
            var savedserviceType = await _serviceTypeRepo.SaveServiceType(serviceType);
            if (savedserviceType == null)
            {
                return CommonResponse.Send(ResponseCodes.FAILURE, null, "Some system errors occurred");
            }
            var serviceTypeTransferDTO = _mapper.Map<ServiceTypeTransferDTO>(serviceType);
            return CommonResponse.Send(ResponseCodes.SUCCESS,serviceTypeTransferDTO);
        }

        public async Task<ApiCommonResponse> DeleteServiceType(long id)
        {
            var serviceTypeToDelete = await _serviceTypeRepo.FindServiceTypeById(id);
            if (serviceTypeToDelete == null)
            {
                return CommonResponse.Send(ResponseCodes.NO_DATA_AVAILABLE);;
            }

            if (!await _serviceTypeRepo.DeleteServiceType(serviceTypeToDelete))
            {
                return CommonResponse.Send(ResponseCodes.FAILURE, null, "Some system errors occurred");
            }

            return CommonResponse.Send(ResponseCodes.SUCCESS);
        }

        public async Task<ApiCommonResponse> GetAllServiceType()
        {
            var serviceTypes = await _serviceTypeRepo.FindAllServiceTypes();
            if (serviceTypes == null)
            {
                return CommonResponse.Send(ResponseCodes.NO_DATA_AVAILABLE);;
            }
            var serviceTypeTransferDTO = _mapper.Map<IEnumerable<ServiceTypeTransferDTO>>(serviceTypes);
            return CommonResponse.Send(ResponseCodes.SUCCESS,serviceTypeTransferDTO);
        }

        public async Task<ApiCommonResponse> GetServiceTypeById(long id)
        {
            var serviceType = await _serviceTypeRepo.FindServiceTypeById(id);
            if (serviceType == null)
            {
                return CommonResponse.Send(ResponseCodes.NO_DATA_AVAILABLE);;
            }
            var serviceTypeTransferDTOs = _mapper.Map<ServiceTypeTransferDTO>(serviceType);
            return CommonResponse.Send(ResponseCodes.SUCCESS,serviceTypeTransferDTOs);
        }

        public async Task<ApiCommonResponse> GetServiceTypeByName(string name)
        {
            var serviceType = await _serviceTypeRepo.FindServiceTypeByName(name);
            if (serviceType == null)
            {
                return CommonResponse.Send(ResponseCodes.NO_DATA_AVAILABLE);;
            }
            var serviceTypeTransferDTOs = _mapper.Map<ServiceTypeTransferDTO>(serviceType);
            return CommonResponse.Send(ResponseCodes.SUCCESS,serviceTypeTransferDTOs);
        }

        public async Task<ApiCommonResponse> UpdateServiceType(HttpContext context, long id, ServiceTypeReceivingDTO serviceTypeReceivingDTO)
        {
            var serviceTypeToUpdate = await _serviceTypeRepo.FindServiceTypeById(id);
            if (serviceTypeToUpdate == null)
            {
                return CommonResponse.Send(ResponseCodes.NO_DATA_AVAILABLE);;
            }

            var summary = $"Initial details before change, \n {serviceTypeToUpdate.ToString()} \n";

            serviceTypeToUpdate.Caption = serviceTypeReceivingDTO.Caption;
            serviceTypeToUpdate.Description = serviceTypeReceivingDTO.Description;
            var updatedserviceType = await _serviceTypeRepo.UpdateServiceType(serviceTypeToUpdate);

            summary += $"Details after change, \n {updatedserviceType.ToString()} \n";

            if (updatedserviceType == null)
            {
                return CommonResponse.Send(ResponseCodes.FAILURE, null, "Some system errors occurred");
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
            return CommonResponse.Send(ResponseCodes.SUCCESS,serviceTypeTransferDTOs);
        }
    }
}
