using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Halobiz.Common.DTOs.ApiDTOs;
using HaloBiz.DTOs.ReceivingDTOs;
using HaloBiz.DTOs.TransferDTOs;
using HaloBiz.Helpers;
using HalobizMigrations.Models;
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

        public async Task<ApiCommonResponse> AddServiceCategoryTask(HttpContext context, ServiceCategoryTaskReceivingDTO serviceCategoryTaskReceivingDTO)
        {
            var serviceCategoryTask = _mapper.Map<ServiceCategoryTask>(serviceCategoryTaskReceivingDTO);
            serviceCategoryTask.CreatedById = context.GetLoggedInUserId();
            var savedServiceCategoryTask = await _serviceCategoryTaskRepo.SaveServiceCategoryTask(serviceCategoryTask);
            if (savedServiceCategoryTask == null)
            {
                return CommonResponse.Send(ResponseCodes.FAILURE, null, "Some system errors occurred");
            }
            var serviceCategoryTaskTransferDTO = _mapper.Map<ServiceCategoryTaskTransferDTO>(serviceCategoryTask);
            return CommonResponse.Send(ResponseCodes.SUCCESS,serviceCategoryTaskTransferDTO);
        }

        public async Task<ApiCommonResponse> GetAllServiceCategoryTasks()
        {
            var serviceCategoryTasks = await _serviceCategoryTaskRepo.FindAllServiceCategoryTasks();
            if (serviceCategoryTasks == null)
            {
                return CommonResponse.Send(ResponseCodes.NO_DATA_AVAILABLE);;
            }
            var serviceCategoryTaskTransferDTO = _mapper.Map<IEnumerable<ServiceCategoryTaskTransferDTO>>(serviceCategoryTasks);
            return CommonResponse.Send(ResponseCodes.SUCCESS,serviceCategoryTaskTransferDTO);
        }

        public async Task<ApiCommonResponse> GetServiceCategoryTaskById(long id)
        {
            var serviceCategoryTask = await _serviceCategoryTaskRepo.FindServiceCategoryTaskById(id);
            if (serviceCategoryTask == null)
            {
                return CommonResponse.Send(ResponseCodes.NO_DATA_AVAILABLE);;
            }
            var serviceCategoryTaskTransferDTO = _mapper.Map<ServiceCategoryTaskTransferDTO>(serviceCategoryTask);
            return CommonResponse.Send(ResponseCodes.SUCCESS,serviceCategoryTaskTransferDTO);
        }

        public async Task<ApiCommonResponse> GetServiceCategoryTaskByName(string name)
        {
            var serviceCategoryTask = await _serviceCategoryTaskRepo.FindServiceCategoryTaskByName(name);
            if (serviceCategoryTask == null)
            {
                return CommonResponse.Send(ResponseCodes.NO_DATA_AVAILABLE);;
            }
            var serviceCategoryTaskTransferDTOs = _mapper.Map<ServiceCategoryTaskTransferDTO>(serviceCategoryTask);
            return CommonResponse.Send(ResponseCodes.SUCCESS,serviceCategoryTaskTransferDTOs);
        }

        public async Task<ApiCommonResponse> UpdateServiceCategoryTask(HttpContext context, long id, ServiceCategoryTaskReceivingDTO serviceCategoryTaskReceivingDTO)
        {
            var serviceCategoryTaskToUpdate = await _serviceCategoryTaskRepo.FindServiceCategoryTaskById(id);
            if (serviceCategoryTaskToUpdate == null)
            {
                return CommonResponse.Send(ResponseCodes.NO_DATA_AVAILABLE);;
            }
            
            var summary = $"Initial details before change, \n {serviceCategoryTaskToUpdate.ToString()} \n" ;

            serviceCategoryTaskToUpdate.Caption = serviceCategoryTaskReceivingDTO.Caption;
            serviceCategoryTaskToUpdate.Description = serviceCategoryTaskReceivingDTO.Description;
            var updatedServiceCategoryTask = await _serviceCategoryTaskRepo.UpdateServiceCategoryTask(serviceCategoryTaskToUpdate);

            summary += $"Details after change, \n {updatedServiceCategoryTask.ToString()} \n";

            if (updatedServiceCategoryTask == null)
            {
                return CommonResponse.Send(ResponseCodes.FAILURE, null, "Some system errors occurred");
            }
            ModificationHistory history = new ModificationHistory(){
                ModelChanged = "ServiceCategoryTask",
                ChangeSummary = summary,
                ChangedById = context.GetLoggedInUserId(),
                ModifiedModelId = updatedServiceCategoryTask.Id
            };

            await _historyRepo.SaveHistory(history);

            var serviceCategoryTaskTransferDTOs = _mapper.Map<ServiceCategoryTaskTransferDTO>(updatedServiceCategoryTask);
            return CommonResponse.Send(ResponseCodes.SUCCESS,serviceCategoryTaskTransferDTOs);

        }

        public async Task<ApiCommonResponse> DeleteServiceCategoryTask(long id)
        {
            var serviceCategoryTaskToDelete = await _serviceCategoryTaskRepo.FindServiceCategoryTaskById(id);

            if (serviceCategoryTaskToDelete == null)
            {
                return CommonResponse.Send(ResponseCodes.NO_DATA_AVAILABLE);;
            }

            await _serviceTaskDeliverableRepo.DeleteServiceTaskDeliverableRange(serviceCategoryTaskToDelete.ServiceTaskDeliverables);

            if (!await _serviceCategoryTaskRepo.DeleteServiceCategoryTask(serviceCategoryTaskToDelete))
            {
                return CommonResponse.Send(ResponseCodes.FAILURE, null, "Some system errors occurred");
            }

            return CommonResponse.Send(ResponseCodes.SUCCESS);
        }

       
    }
}