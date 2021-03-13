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
    public class ServiceCategoryTaskServiceImpl : IServiceCategoryTaskService
    {
        private readonly IModificationHistoryRepository _historyRepo;
        private readonly IServiceCategoryTaskRepository _serviceCategoryTaskRepo;
        private readonly IServiceTaskDeliverableRepository _serviceTaskDeliverableRepo;
        private readonly IMapper _mapper;

        public ServiceCategoryTaskServiceImpl(IModificationHistoryRepository historyRepo, IMapper mapper, IServiceCategoryTaskRepository _serviceCategoryTaskRepo, IServiceTaskDeliverableRepository serviceTaskDeliverableRepo)
        {
            this._mapper = mapper;
            this._historyRepo = historyRepo;
            this._serviceCategoryTaskRepo = _serviceCategoryTaskRepo;
            this._serviceTaskDeliverableRepo = serviceTaskDeliverableRepo;
        }

        public async Task<ApiResponse> AddServiceCategoryTask(HttpContext context, ServiceCategoryTaskReceivingDTO serviceCategoryTaskReceivingDTO)
        {
            var serviceCategoryTask = _mapper.Map<ServiceCategoryTask>(serviceCategoryTaskReceivingDTO);
            serviceCategoryTask.CreatedById = context.GetLoggedInUserId();
            var savedServiceCategoryTask = await _serviceCategoryTaskRepo.SaveServiceCategoryTask(serviceCategoryTask);
            if (savedServiceCategoryTask == null)
            {
                return new ApiResponse(500);
            }
            var serviceCategoryTaskTransferDTO = _mapper.Map<ServiceCategoryTaskTransferDTO>(serviceCategoryTask);
            return new ApiOkResponse(serviceCategoryTaskTransferDTO);
        }

        public async Task<ApiResponse> GetAllServiceCategoryTasks()
        {
            var serviceCategoryTasks = await _serviceCategoryTaskRepo.FindAllServiceCategoryTasks();
            if (serviceCategoryTasks == null)
            {
                return new ApiResponse(404);
            }
            var serviceCategoryTaskTransferDTO = _mapper.Map<IEnumerable<ServiceCategoryTaskTransferDTO>>(serviceCategoryTasks);
            return new ApiOkResponse(serviceCategoryTaskTransferDTO);
        }

        public async Task<ApiResponse> GetServiceCategoryTaskById(long id)
        {
            var serviceCategoryTask = await _serviceCategoryTaskRepo.FindServiceCategoryTaskById(id);
            if (serviceCategoryTask == null)
            {
                return new ApiResponse(404);
            }
            var serviceCategoryTaskTransferDTO = _mapper.Map<ServiceCategoryTaskTransferDTO>(serviceCategoryTask);
            return new ApiOkResponse(serviceCategoryTaskTransferDTO);
        }

        public async Task<ApiResponse> GetServiceCategoryTaskByName(string name)
        {
            var serviceCategoryTask = await _serviceCategoryTaskRepo.FindServiceCategoryTaskByName(name);
            if (serviceCategoryTask == null)
            {
                return new ApiResponse(404);
            }
            var serviceCategoryTaskTransferDTOs = _mapper.Map<ServiceCategoryTaskTransferDTO>(serviceCategoryTask);
            return new ApiOkResponse(serviceCategoryTaskTransferDTOs);
        }

        public async Task<ApiResponse> UpdateServiceCategoryTask(HttpContext context, long id, ServiceCategoryTaskReceivingDTO serviceCategoryTaskReceivingDTO)
        {
            var serviceCategoryTaskToUpdate = await _serviceCategoryTaskRepo.FindServiceCategoryTaskById(id);
            if (serviceCategoryTaskToUpdate == null)
            {
                return new ApiResponse(404);
            }
            
            var summary = $"Initial details before change, \n {serviceCategoryTaskToUpdate.ToString()} \n" ;

            serviceCategoryTaskToUpdate.Caption = serviceCategoryTaskReceivingDTO.Caption;
            serviceCategoryTaskToUpdate.Description = serviceCategoryTaskReceivingDTO.Description;
            var updatedServiceCategoryTask = await _serviceCategoryTaskRepo.UpdateServiceCategoryTask(serviceCategoryTaskToUpdate);

            summary += $"Details after change, \n {updatedServiceCategoryTask.ToString()} \n";

            if (updatedServiceCategoryTask == null)
            {
                return new ApiResponse(500);
            }
            ModificationHistory history = new ModificationHistory(){
                ModelChanged = "ServiceCategoryTask",
                ChangeSummary = summary,
                ChangedById = context.GetLoggedInUserId(),
                ModifiedModelId = updatedServiceCategoryTask.Id
            };

            await _historyRepo.SaveHistory(history);

            var serviceCategoryTaskTransferDTOs = _mapper.Map<ServiceCategoryTaskTransferDTO>(updatedServiceCategoryTask);
            return new ApiOkResponse(serviceCategoryTaskTransferDTOs);

        }

        public async Task<ApiResponse> DeleteServiceCategoryTask(long id)
        {
            var serviceCategoryTaskToDelete = await _serviceCategoryTaskRepo.FindServiceCategoryTaskById(id);

            if (serviceCategoryTaskToDelete == null)
            {
                return new ApiResponse(404);
            }

            await _serviceTaskDeliverableRepo.DeleteServiceTaskDeliverableRange(serviceCategoryTaskToDelete.ServiceTaskDeliverable);

            if (!await _serviceCategoryTaskRepo.DeleteServiceCategoryTask(serviceCategoryTaskToDelete))
            {
                return new ApiResponse(500);
            }

            return new ApiOkResponse(true);
        }

       
    }
}