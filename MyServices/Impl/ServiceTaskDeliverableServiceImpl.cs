using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using HaloBiz.DTOs.ApiDTOs;
using HaloBiz.DTOs.ReceivingDTOs;
using HaloBiz.DTOs.TransferDTOs;
using HaloBiz.Helpers;
using HaloBiz.Model;
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

        public async Task<ApiResponse> AddServiceTaskDeliverable(HttpContext context, ServiceTaskDeliverableReceivingDTO serviceTaskDeliverableReceivingDTO)
        {
            var serviceTaskDeliverable = _mapper.Map<ServiceTaskDeliverable>(serviceTaskDeliverableReceivingDTO);
            serviceTaskDeliverable.CreatedById = context.GetLoggedInUserId();
            var savedServiceTaskDeliverable = await _serviceTaskDeliverableRepo.SaveServiceTaskDeliverable(serviceTaskDeliverable);
            if (savedServiceTaskDeliverable == null)
            {
                return new ApiResponse(500);
            }
            var serviceTaskDeliverableTransferDTO = _mapper.Map<ServiceTaskDeliverableTransferDTO>(serviceTaskDeliverable);
            return new ApiOkResponse(serviceTaskDeliverableTransferDTO);
        }

        public async Task<ApiResponse> GetAllServiceTaskDeliverables()
        {
            var serviceTaskDeliverables = await _serviceTaskDeliverableRepo.FindAllServiceTaskDeliverables();
            if (serviceTaskDeliverables == null)
            {
                return new ApiResponse(404);
            }
            var serviceTaskDeliverableTransferDTO = _mapper.Map<IEnumerable<ServiceTaskDeliverableTransferDTO>>(serviceTaskDeliverables);
            return new ApiOkResponse(serviceTaskDeliverableTransferDTO);
        }

        public async Task<ApiResponse> GetServiceTaskDeliverableById(long id)
        {
            var serviceTaskDeliverable = await _serviceTaskDeliverableRepo.FindServiceTaskDeliverableById(id);
            if (serviceTaskDeliverable == null)
            {
                return new ApiResponse(404);
            }
            var serviceTaskDeliverableTransferDTO = _mapper.Map<ServiceTaskDeliverableTransferDTO>(serviceTaskDeliverable);
            return new ApiOkResponse(serviceTaskDeliverableTransferDTO);
        }

        public async Task<ApiResponse> GetServiceTaskDeliverableByName(string name)
        {
            var serviceTaskDeliverable = await _serviceTaskDeliverableRepo.FindServiceTaskDeliverableByName(name);
            if (serviceTaskDeliverable == null)
            {
                return new ApiResponse(404);
            }
            var serviceTaskDeliverableTransferDTOs = _mapper.Map<ServiceTaskDeliverableTransferDTO>(serviceTaskDeliverable);
            return new ApiOkResponse(serviceTaskDeliverableTransferDTOs);
        }

        public async Task<ApiResponse> UpdateServiceTaskDeliverable(HttpContext context, long id, ServiceTaskDeliverableReceivingDTO serviceTaskDeliverableReceivingDTO)
        {
            var serviceTaskDeliverableToUpdate = await _serviceTaskDeliverableRepo.FindServiceTaskDeliverableById(id);
            if (serviceTaskDeliverableToUpdate == null)
            {
                return new ApiResponse(404);
            }
            
            var summary = $"Initial details before change, \n {serviceTaskDeliverableToUpdate.ToString()} \n" ;

            serviceTaskDeliverableToUpdate.Caption = serviceTaskDeliverableReceivingDTO.Caption;
            serviceTaskDeliverableToUpdate.Description = serviceTaskDeliverableReceivingDTO.Description;
            var updatedServiceTaskDeliverable = await _serviceTaskDeliverableRepo.UpdateServiceTaskDeliverable(serviceTaskDeliverableToUpdate);

            summary += $"Details after change, \n {updatedServiceTaskDeliverable.ToString()} \n";

            if (updatedServiceTaskDeliverable == null)
            {
                return new ApiResponse(500);
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

        public async Task<ApiResponse> DeleteServiceTaskDeliverable(long id)
        {
            var serviceTaskDeliverableToDelete = await _serviceTaskDeliverableRepo.FindServiceTaskDeliverableById(id);
            
            if (serviceTaskDeliverableToDelete == null)
            {
                return new ApiResponse(404);
            }

            if (!await _serviceTaskDeliverableRepo.DeleteServiceTaskDeliverable(serviceTaskDeliverableToDelete))
            {
                return new ApiResponse(500);
            }

            return new ApiOkResponse(true);
        }

    }
}