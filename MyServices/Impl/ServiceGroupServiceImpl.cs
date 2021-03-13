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
    public class ServiceGroupServiceImpl : IServiceGroupService
    {
        private readonly IServiceCategoryService _serviceCategoryService;
        private readonly IServiceGroupRepository _serviceGroupRepo;
        private readonly IMapper _mapper;

        public ServiceGroupServiceImpl( IServiceCategoryService serviceCategoryService,  IServiceGroupRepository serviceGroupRepo, IMapper mapper)
        {
            this._mapper = mapper;
            this._serviceCategoryService = serviceCategoryService;
            this._serviceGroupRepo = serviceGroupRepo;
 
        }

        public async Task<ApiResponse> AddServiceGroup(ServiceGroupReceivingDTO serviceGroupReceivingDTO)
        {
            var serviceGroup = _mapper.Map<ServiceGroup>(serviceGroupReceivingDTO);
            var savedServiceGroup = await _serviceGroupRepo.SaveServiceGroup(serviceGroup);
            if (savedServiceGroup == null)
            {
                return new ApiResponse(500);
            }
            var serviceGroupTransferDTO = _mapper.Map<ServiceGroupTransferDTO>(savedServiceGroup);
            return new ApiOkResponse(serviceGroupTransferDTO);
        }

        public async Task<ApiResponse> GetAllServiceGroups()
        {
            var serviceGroups = await _serviceGroupRepo.FindAllServiceGroups();
            if (serviceGroups == null)
            {
                return new ApiResponse(404);
            }
            var serviceGroupTransferDTO = _mapper.Map<IEnumerable<ServiceGroupTransferDTO>>(serviceGroups);
            return new ApiOkResponse(serviceGroupTransferDTO);
        }

        public async Task<ApiResponse> GetServiceGroupById(long id)
        {
            var serviceGroup = await _serviceGroupRepo.FindServiceGroupById(id);
            if (serviceGroup == null)
            {
                return new ApiResponse(404);
            }
            var serviceGroupTransferDTO = _mapper.Map<ServiceGroupTransferDTO>(serviceGroup);
            return new ApiOkResponse(serviceGroupTransferDTO);
        }

        public async Task<ApiResponse> GetServiceGroupByName(string name)
        {
            var serviceGroup = await _serviceGroupRepo.FindServiceGroupByName(name);
            if (serviceGroup == null)
            {
                return new ApiResponse(404);
            }
            var serviceGroupTransferDTO = _mapper.Map<ServiceGroupTransferDTO>(serviceGroup);
            return new ApiOkResponse(serviceGroupTransferDTO);
        }

        public async Task<ApiResponse> UpdateServiceGroup(long id, ServiceGroupReceivingDTO serviceGroupReceivingDTO)
        {
            var serviceGroupToUpdate = await _serviceGroupRepo.FindServiceGroupById(id);
            if (serviceGroupToUpdate == null)
            {
                return new ApiResponse(404);
            }
            serviceGroupToUpdate.Name = serviceGroupReceivingDTO.Name;
            serviceGroupToUpdate.Description = serviceGroupReceivingDTO.Description;
            serviceGroupToUpdate.OperatingEntityId = serviceGroupReceivingDTO.OperatingEntityId;
            serviceGroupToUpdate.DivisionId = serviceGroupReceivingDTO.DivisionId;
            var updatedServiceGroup = await _serviceGroupRepo.UpdateServiceGroup(serviceGroupToUpdate);

            if (updatedServiceGroup == null)
            {
                return new ApiResponse(500);
            }
            var serviceGroupTransferDTO = _mapper.Map<ServiceGroupTransferDTO>(updatedServiceGroup);
            return new ApiOkResponse(serviceGroupTransferDTO);


        }

        public async Task<ApiResponse> DeleteServiceGroup(long id)
        {
            var serviceGroupToDelete = await _serviceGroupRepo.FindServiceGroupById(id);
            if (serviceGroupToDelete == null)
            {
                return new ApiResponse(404);
            }

            foreach (ServiceCategory serviceCategory in serviceGroupToDelete.ServiceCategories)
            {
                await _serviceCategoryService.DeleteServiceCategory(serviceCategory.Id);
            }

            if (!await _serviceGroupRepo.DeleteServiceGroup(serviceGroupToDelete))
            {
                return new ApiResponse(500);
            }

            return new ApiOkResponse(true);
        }
    }
}