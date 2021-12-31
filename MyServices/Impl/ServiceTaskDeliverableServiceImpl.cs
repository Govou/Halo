using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using HaloBiz.DTOs.ApiDTOs;
using HaloBiz.DTOs.ReceivingDTOs;
using HaloBiz.DTOs.TransferDTOs;
using HaloBiz.Helpers;
using HalobizMigrations.Models;
using HaloBiz.Repository;
using Microsoft.AspNetCore.Http;

namespace HaloBiz.MyServices.Impl
{
    public class ServiceTaskDeliverableServiceImpl : IServiceTaskDeliverableService
    {
        private readonly IModificationHistoryRepository _historyRepo;
        private readonly IServiceTaskDeliverableRepository _serviceTaskDeliverableRepo;
        private readonly IMapper _mapper;

        public ServiceTaskDeliverableServiceImpl(IModificationHistoryRepository historyRepo, IMapper mapper, IServiceTaskDeliverableRepository _serviceTaskDeliverableRepo)
        {
            this._mapper = mapper;
            this._historyRepo = historyRepo;
            this._serviceTaskDeliverableRepo = _serviceTaskDeliverableRepo;
        }

        public async Task<ApiCommonResponse> AddServiceTaskDeliverable(HttpContext context, ServiceTaskDeliverableReceivingDTO serviceTaskDeliverableReceivingDTO)
        {
            var serviceTaskDeliverable = _mapper.Map<ServiceTaskDeliverable>(serviceTaskDeliverableReceivingDTO);
            serviceTaskDeliverable.CreatedById = context.GetLoggedInUserId();
            var savedServiceTaskDeliverable = await _serviceTaskDeliverableRepo.SaveServiceTaskDeliverable(serviceTaskDeliverable);
            if (savedServiceTaskDeliverable == null)
            {
                return CommonResponse.Send(ResponseCodes.FAILURE, null, "Some system errors occurred");
            }
            var serviceTaskDeliverableTransferDTO = _mapper.Map<ServiceTaskDeliverableTransferDTO>(serviceTaskDeliverable);
            return new ApiOkResponse(serviceTaskDeliverableTransferDTO);
        }

        public async Task<ApiCommonResponse> GetAllServiceTaskDeliverables()
        {
            var serviceTaskDeliverables = await _serviceTaskDeliverableRepo.FindAllServiceTaskDeliverables();
            if (serviceTaskDeliverables == null)
            {
                return CommonResponse.Send(ResponseCodes.NO_DATA_AVAILABLE);;
            }
            var serviceTaskDeliverableTransferDTO = _mapper.Map<IEnumerable<ServiceTaskDeliverableTransferDTO>>(serviceTaskDeliverables);
            return new ApiOkResponse(serviceTaskDeliverableTransferDTO);
        }

        public async Task<ApiCommonResponse> GetServiceTaskDeliverableById(long id)
        {
            var serviceTaskDeliverable = await _serviceTaskDeliverableRepo.FindServiceTaskDeliverableById(id);
            if (serviceTaskDeliverable == null)
            {
                return CommonResponse.Send(ResponseCodes.NO_DATA_AVAILABLE);;
            }
            var serviceTaskDeliverableTransferDTO = _mapper.Map<ServiceTaskDeliverableTransferDTO>(serviceTaskDeliverable);
            return new ApiOkResponse(serviceTaskDeliverableTransferDTO);
        }

        public async Task<ApiCommonResponse> GetServiceTaskDeliverableByName(string name)
        {
            var serviceTaskDeliverable = await _serviceTaskDeliverableRepo.FindServiceTaskDeliverableByName(name);
            if (serviceTaskDeliverable == null)
            {
                return CommonResponse.Send(ResponseCodes.NO_DATA_AVAILABLE);;
            }
            var serviceTaskDeliverableTransferDTOs = _mapper.Map<ServiceTaskDeliverableTransferDTO>(serviceTaskDeliverable);
            return new ApiOkResponse(serviceTaskDeliverableTransferDTOs);
        }

        public async Task<ApiCommonResponse> UpdateServiceTaskDeliverable(HttpContext context, long id, ServiceTaskDeliverableReceivingDTO serviceTaskDeliverableReceivingDTO)
        {
            var serviceTaskDeliverableToUpdate = await _serviceTaskDeliverableRepo.FindServiceTaskDeliverableById(id);
            if (serviceTaskDeliverableToUpdate == null)
            {
                return CommonResponse.Send(ResponseCodes.NO_DATA_AVAILABLE);;
            }
            
            var summary = $"Initial details before change, \n {serviceTaskDeliverableToUpdate.ToString()} \n" ;

            serviceTaskDeliverableToUpdate.Caption = serviceTaskDeliverableReceivingDTO.Caption;
            serviceTaskDeliverableToUpdate.Description = serviceTaskDeliverableReceivingDTO.Description;
            var updatedServiceTaskDeliverable = await _serviceTaskDeliverableRepo.UpdateServiceTaskDeliverable(serviceTaskDeliverableToUpdate);

            summary += $"Details after change, \n {updatedServiceTaskDeliverable.ToString()} \n";

            if (updatedServiceTaskDeliverable == null)
            {
                return CommonResponse.Send(ResponseCodes.FAILURE, null, "Some system errors occurred");
            }
            ModificationHistory history = new ModificationHistory(){
                ModelChanged = "ServiceTaskDeliverable",
                ChangeSummary = summary,
                ChangedById = context.GetLoggedInUserId(),
                ModifiedModelId = updatedServiceTaskDeliverable.Id
            };

            await _historyRepo.SaveHistory(history);

            var serviceTaskDeliverableTransferDTOs = _mapper.Map<ServiceTaskDeliverableTransferDTO>(updatedServiceTaskDeliverable);
            return new ApiOkResponse(serviceTaskDeliverableTransferDTOs);

        }

        public async Task<ApiCommonResponse> DeleteServiceTaskDeliverable(long id)
        {
            var serviceTaskDeliverableToDelete = await _serviceTaskDeliverableRepo.FindServiceTaskDeliverableById(id);
            
            if (serviceTaskDeliverableToDelete == null)
            {
                return CommonResponse.Send(ResponseCodes.NO_DATA_AVAILABLE);;
            }

            if (!await _serviceTaskDeliverableRepo.DeleteServiceTaskDeliverable(serviceTaskDeliverableToDelete))
            {
                return CommonResponse.Send(ResponseCodes.FAILURE, null, "Some system errors occurred");
            }

            return CommonResponse.Send(ResponseCodes.SUCCESS);
        }

    }
}