using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using HaloBiz.DTOs.ApiDTOs;
using HaloBiz.DTOs.ReceivingDTOs;
using HaloBiz.DTOs.TransferDTOs;
using HaloBiz.Model;
using HaloBiz.Repository;

namespace HaloBiz.MyServices.Impl
{
    public class ServiceCategoryServiceImpl : IServiceCategoryService
    {
        private readonly IServiceCategoryTaskService _serviceCategoryTaskService;
        private readonly IServicesService _serviceService;
        private readonly IServiceCategoryRepository _serviceCategoryRepo;
        private readonly IMapper _mapper;

        public ServiceCategoryServiceImpl(IServiceCategoryTaskService serviceCategoryTaskService, IServicesService serviceService ,IServiceCategoryRepository serviceCategoryRepo, IMapper mapper)
        {
            this._mapper = mapper;
            this._serviceCategoryTaskService = serviceCategoryTaskService;
            this._serviceService = serviceService;
            this._serviceCategoryRepo = serviceCategoryRepo;
 
        }

        public async Task<ApiResponse> AddServiceCategory(ServiceCategoryReceivingDTO serviceCategoryReceivingDTO)
        {
            var serviceCategory = _mapper.Map<ServiceCategory>(serviceCategoryReceivingDTO);
            var savedserviceCategory = await _serviceCategoryRepo.SaveServiceCategory(serviceCategory);
            if (savedserviceCategory == null)
            {
                return new ApiResponse(500);
            }
            var serviceCategoryTransferDTO = _mapper.Map<ServiceCategoryTransferDTO>(savedserviceCategory);
            return new ApiOkResponse(serviceCategoryTransferDTO);
        }

        public async Task<ApiResponse> GetAllServiceCategory()
        {
            var serviceCategory= await _serviceCategoryRepo.FindAllServiceCategories();
            if (serviceCategory == null)
            {
                return new ApiResponse(404);
            }
            var serviceCategoryTransferDTO = _mapper.Map<IEnumerable<ServiceCategoryTransferDTO>>(serviceCategory);
            return new ApiOkResponse(serviceCategoryTransferDTO);
        }

        public async Task<ApiResponse> GetServiceCategoryById(long id)
        {
            var serviceCategory = await _serviceCategoryRepo.FindServiceCategoryById(id);
            if (serviceCategory == null)
            {
                return new ApiResponse(404);
            }
            var serviceCategoryTransferDTO = _mapper.Map<ServiceCategoryTransferDTO>(serviceCategory);
            return new ApiOkResponse(serviceCategoryTransferDTO);
        }

        public async Task<ApiResponse> GetServiceCategoryByName(string name)
        {
            var serviceCategory = await _serviceCategoryRepo.FindServiceCategoryByName(name);
            if (serviceCategory == null)
            {
                return new ApiResponse(404);
            }
            var serviceCategoryTransferDTO = _mapper.Map<ServiceCategoryTransferDTO>(serviceCategory);
            return new ApiOkResponse(serviceCategoryTransferDTO);
        }

        public async Task<ApiResponse> UpdateServiceCategory(long id, ServiceCategoryReceivingDTO serviceCategoryReceivingDTO)
        {
            var serviceCategoryToUpdate = await _serviceCategoryRepo.FindServiceCategoryById(id);
            if (serviceCategoryToUpdate == null)
            {
                return new ApiResponse(404);
            }
            serviceCategoryToUpdate.Name = serviceCategoryReceivingDTO.Name;
            serviceCategoryToUpdate.Description = serviceCategoryReceivingDTO.Description;
            serviceCategoryToUpdate.ServiceGroupId = serviceCategoryReceivingDTO.ServiceGroupId;
            serviceCategoryToUpdate.DivisionId = serviceCategoryReceivingDTO.DivisionId;
            serviceCategoryToUpdate.OperatingEntityId = serviceCategoryReceivingDTO.OperatingEntityId;
            var updatedServiceCategory = await _serviceCategoryRepo.UpdateServiceCategory(serviceCategoryToUpdate);

            if (updatedServiceCategory == null)
            {
                return new ApiResponse(500);
            }
            var serviceCategoryTransferDTO = _mapper.Map<ServiceCategoryTransferDTO>(updatedServiceCategory);
            return new ApiOkResponse(serviceCategoryTransferDTO);

        }

        public async Task<ApiResponse> DeleteServiceCategory(long id)
        {
            var serviceCategoryToDelete = await _serviceCategoryRepo.FindServiceCategoryById(id);

            if (serviceCategoryToDelete == null)
            {
                return new ApiResponse(404);
            }

            foreach(Services service in serviceCategoryToDelete.Services)
            {
                await _serviceService.DeleteService(service.Id);
            }
            foreach(ServiceCategoryTask serviceCategoryTask in serviceCategoryToDelete.ServiceCategoryTasks)
            {
                await _serviceCategoryTaskService.DeleteServiceCategoryTask(serviceCategoryTask.Id);
            }

            if (!await _serviceCategoryRepo.DeleteServiceCategory(serviceCategoryToDelete))
            {
                return new ApiResponse(500);
            }

            return new ApiOkResponse(true);
        }
    }
}