using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Halobiz.Common.DTOs.ApiDTOs;
using HaloBiz.DTOs.ReceivingDTOs;
using HaloBiz.DTOs.TransferDTOs;
using HalobizMigrations.Models;
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

        public async Task<ApiCommonResponse> AddServiceCategory(ServiceCategoryReceivingDTO serviceCategoryReceivingDTO)
        {
            var serviceCategory = _mapper.Map<ServiceCategory>(serviceCategoryReceivingDTO);
            var savedserviceCategory = await _serviceCategoryRepo.SaveServiceCategory(serviceCategory);
            if (savedserviceCategory == null)
            {
                return CommonResponse.Send(ResponseCodes.FAILURE, null, "Error! Please check duplicate entry for name and category group");
            }
            var serviceCategoryTransferDTO = _mapper.Map<ServiceCategoryTransferDTO>(savedserviceCategory);
            return CommonResponse.Send(ResponseCodes.SUCCESS,serviceCategoryTransferDTO);
        }

        public async Task<ApiCommonResponse> GetAllServiceCategory()
        {
            var serviceCategory= await _serviceCategoryRepo.FindAllServiceCategories();
            if (serviceCategory == null)
            {
                return CommonResponse.Send(ResponseCodes.NO_DATA_AVAILABLE);;
            }
            var serviceCategoryTransferDTO = _mapper.Map<IEnumerable<ServiceCategoryTransferDTO>>(serviceCategory);
            return CommonResponse.Send(ResponseCodes.SUCCESS,serviceCategoryTransferDTO);
        }

        public async Task<ApiCommonResponse> GetServiceCategoryById(long id)
        {
            var serviceCategory = await _serviceCategoryRepo.FindServiceCategoryById(id);
            if (serviceCategory == null)
            {
                return CommonResponse.Send(ResponseCodes.NO_DATA_AVAILABLE);;
            }
            var serviceCategoryTransferDTO = _mapper.Map<ServiceCategoryTransferDTO>(serviceCategory);
            return CommonResponse.Send(ResponseCodes.SUCCESS,serviceCategoryTransferDTO);
        }

        public async Task<ApiCommonResponse> GetServiceCategoryByName(string name)
        {
            var serviceCategory = await _serviceCategoryRepo.FindServiceCategoryByName(name);
            if (serviceCategory == null)
            {
                return CommonResponse.Send(ResponseCodes.NO_DATA_AVAILABLE);;
            }
            var serviceCategoryTransferDTO = _mapper.Map<ServiceCategoryTransferDTO>(serviceCategory);
            return CommonResponse.Send(ResponseCodes.SUCCESS,serviceCategoryTransferDTO);
        }

        public async Task<ApiCommonResponse> UpdateServiceCategory(long id, ServiceCategoryReceivingDTO serviceCategoryReceivingDTO)
        {
            var serviceCategoryToUpdate = await _serviceCategoryRepo.FindServiceCategoryById(id);
            if (serviceCategoryToUpdate == null)
            {
                return CommonResponse.Send(ResponseCodes.NO_DATA_AVAILABLE);;
            }
            serviceCategoryToUpdate.Name = serviceCategoryReceivingDTO.Name;
            serviceCategoryToUpdate.Description = serviceCategoryReceivingDTO.Description;
            serviceCategoryToUpdate.ServiceGroupId = serviceCategoryReceivingDTO.ServiceGroupId;
            serviceCategoryToUpdate.DivisionId = serviceCategoryReceivingDTO.DivisionId;
            serviceCategoryToUpdate.OperatingEntityId = serviceCategoryReceivingDTO.OperatingEntityId;
            serviceCategoryToUpdate.ImageUrl = serviceCategoryReceivingDTO.ImageUrl;
            var updatedServiceCategory = await _serviceCategoryRepo.UpdateServiceCategory(serviceCategoryToUpdate);

            if (updatedServiceCategory == null)
            {
                return CommonResponse.Send(ResponseCodes.FAILURE, null, "Some system errors occurred");
            }
            var serviceCategoryTransferDTO = _mapper.Map<ServiceCategoryTransferDTO>(updatedServiceCategory);
            return CommonResponse.Send(ResponseCodes.SUCCESS,serviceCategoryTransferDTO);

        }

        public async Task<ApiCommonResponse> DeleteServiceCategory(long id)
        {
            var serviceCategoryToDelete = await _serviceCategoryRepo.FindServiceCategoryById(id);

            if (serviceCategoryToDelete == null)
            {
                return CommonResponse.Send(ResponseCodes.NO_DATA_AVAILABLE);;
            }

            foreach(Service service in serviceCategoryToDelete.Services)
            {
                await _serviceService.DeleteService(service.Id);
            }
            foreach(ServiceCategoryTask serviceCategoryTask in serviceCategoryToDelete.ServiceCategoryTasks)
            {
                await _serviceCategoryTaskService.DeleteServiceCategoryTask(serviceCategoryTask.Id);
            }

            if (!await _serviceCategoryRepo.DeleteServiceCategory(serviceCategoryToDelete))
            {
                return CommonResponse.Send(ResponseCodes.FAILURE, null, "Some system errors occurred");
            }

            return CommonResponse.Send(ResponseCodes.SUCCESS);
        }
    }
}