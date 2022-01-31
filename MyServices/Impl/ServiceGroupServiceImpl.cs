using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using HaloBiz.DTOs.ApiDTOs;
using HaloBiz.DTOs.ReceivingDTOs;
using HaloBiz.DTOs.TransferDTOs;
using HalobizMigrations.Models;
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

        public async Task<ApiCommonResponse> AddServiceGroup(ServiceGroupReceivingDTO serviceGroupReceivingDTO)
        {
            var serviceGroup = _mapper.Map<ServiceGroup>(serviceGroupReceivingDTO);
            var savedServiceGroup = await _serviceGroupRepo.SaveServiceGroup(serviceGroup);
            if (savedServiceGroup == null)
            {
                return CommonResponse.Send(ResponseCodes.FAILURE, null, "Some system errors occurred");
            }
            var serviceGroupTransferDTO = _mapper.Map<ServiceGroupTransferDTO>(savedServiceGroup);
            return CommonResponse.Send(ResponseCodes.SUCCESS,serviceGroupTransferDTO);
        }

        public async Task<ApiCommonResponse> GetAllServiceGroups()
        {
            var serviceGroups = await _serviceGroupRepo.FindAllServiceGroups();
            if (serviceGroups == null)
            {
                return CommonResponse.Send(ResponseCodes.NO_DATA_AVAILABLE);;
            }
            var serviceGroupTransferDTO = _mapper.Map<IEnumerable<ServiceGroupTransferDTO>>(serviceGroups);
            return CommonResponse.Send(ResponseCodes.SUCCESS,serviceGroupTransferDTO);
        }

        public async Task<ApiCommonResponse> GetServiceGroupById(long id)
        {
            var serviceGroup = await _serviceGroupRepo.FindServiceGroupById(id);
            if (serviceGroup == null)
            {
                return CommonResponse.Send(ResponseCodes.NO_DATA_AVAILABLE);;
            }
            var serviceGroupTransferDTO = _mapper.Map<ServiceGroupTransferDTO>(serviceGroup);
            return CommonResponse.Send(ResponseCodes.SUCCESS,serviceGroupTransferDTO);
        }

        public async Task<ApiCommonResponse> GetServiceGroupByName(string name)
        {
            var serviceGroup = await _serviceGroupRepo.FindServiceGroupByName(name);
            if (serviceGroup == null)
            {
                return CommonResponse.Send(ResponseCodes.NO_DATA_AVAILABLE);;
            }
            var serviceGroupTransferDTO = _mapper.Map<ServiceGroupTransferDTO>(serviceGroup);
            return CommonResponse.Send(ResponseCodes.SUCCESS,serviceGroupTransferDTO);
        }

        public async Task<ApiCommonResponse> UpdateServiceGroup(long id, ServiceGroupReceivingDTO serviceGroupReceivingDTO)
        {
            var serviceGroupToUpdate = await _serviceGroupRepo.FindServiceGroupById(id);
            if (serviceGroupToUpdate == null)
            {
                return CommonResponse.Send(ResponseCodes.NO_DATA_AVAILABLE);;
            }
            serviceGroupToUpdate.Name = serviceGroupReceivingDTO.Name;
            serviceGroupToUpdate.Description = serviceGroupReceivingDTO.Description;
            serviceGroupToUpdate.OperatingEntityId = serviceGroupReceivingDTO.OperatingEntityId;
            serviceGroupToUpdate.DivisionId = serviceGroupReceivingDTO.DivisionId;
            var updatedServiceGroup = await _serviceGroupRepo.UpdateServiceGroup(serviceGroupToUpdate);

            if (updatedServiceGroup == null)
            {
                return CommonResponse.Send(ResponseCodes.FAILURE, null, "Some system errors occurred");
            }
            var serviceGroupTransferDTO = _mapper.Map<ServiceGroupTransferDTO>(updatedServiceGroup);
            return CommonResponse.Send(ResponseCodes.SUCCESS,serviceGroupTransferDTO);


        }

        public async Task<ApiCommonResponse> DeleteServiceGroup(long id)
        {
            var serviceGroupToDelete = await _serviceGroupRepo.FindServiceGroupById(id);
            if (serviceGroupToDelete == null)
            {
                return CommonResponse.Send(ResponseCodes.NO_DATA_AVAILABLE);;
            }

            foreach (ServiceCategory serviceCategory in serviceGroupToDelete.ServiceCategories)
            {
                await _serviceCategoryService.DeleteServiceCategory(serviceCategory.Id);
            }

            if (!await _serviceGroupRepo.DeleteServiceGroup(serviceGroupToDelete))
            {
                return CommonResponse.Send(ResponseCodes.FAILURE, null, "Some system errors occurred");
            }

            return CommonResponse.Send(ResponseCodes.SUCCESS);
        }
    }
}