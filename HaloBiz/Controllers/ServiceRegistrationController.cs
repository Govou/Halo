using HaloBiz.DTOs;
using Halobiz.Common.DTOs.ApiDTOs;
using HaloBiz.DTOs.ReceivingDTOs;
using HaloBiz.MyServices;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Halobiz.Common.Auths;

namespace HaloBiz.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    [ModuleName(HalobizModules.Setups)]

    public class ServiceRegistrationController : ControllerBase
    {
        private readonly IServiceRegistrationService _serviceRegistration;
        private readonly IServicesService _servicesService;


        public ServiceRegistrationController(IServiceRegistrationService serviceRegistration, IServicesService servicesService)
        {
            _serviceRegistration = serviceRegistration;
            _servicesService = servicesService;
        }


        [HttpGet("GetAllRegisteredServices")]
        public async Task<ApiCommonResponse> GetAllRegisteredServices()
        {
            return await _serviceRegistration.GetAllServiceRegs();
        }
        [HttpGet("AllSecuredMobilityServices")]
        public async Task<ApiCommonResponse> AllSecuredMobilityServices()
        {
            return await _servicesService.GetAllSecuredMobilityServices();
        }
        [HttpGet("GetAllArmedEscortResourceRequired")]
        public async Task<ApiCommonResponse> GetAllArmedEscortResourceRequired()
        {
            return await _serviceRegistration.GetAllArmedEscortResourceRequired();
        }
        [HttpGet("GetAllPilotResourceRequired")] 
        public async Task<ApiCommonResponse> GetAllPilotResourceRequired()
        {
            return await _serviceRegistration.GetAllPilotResourceRequired();
        }
        [HttpGet("GetAllCommanderResourceRequired")]
        public async Task<ApiCommonResponse> GetAllCommanderResourceRequired()
        {
            return await _serviceRegistration.GetAllCommanderResourceRequired();
        }
        [HttpGet("GetAllVehicleResourceRequired")]
        public async Task<ApiCommonResponse> GetAllVehicleResourceRequired()
        {
            return await _serviceRegistration.GetAllVehicleResourceRequired();
        }

        [HttpGet("GetRegServiceById/{id}")]
        public async Task<ApiCommonResponse> GetRegServiceById(long id)
        {
            return await _serviceRegistration.GetServiceRegById(id);
        }
        [HttpGet("GetArmedEscortResourceRequiredById/{id}")]
        public async Task<ApiCommonResponse> GetArmedEscortResourceRequiredById(long id)
        {
            return await _serviceRegistration.GetArmedEscortResourceById(id);
        }
        [HttpGet("GetPilotResourceRequiredById/{id}")] 
        public async Task<ApiCommonResponse> GetPilotResourceRequiredById(long id)
        {
            return await _serviceRegistration.GetPilotResourceById(id);
        }
        [HttpGet("GetCommanderResourceRequiredById/{id}")]
        public async Task<ApiCommonResponse> GetCommanderResourceRequiredById(long id)
        {
            return await _serviceRegistration.GetCommanderResourceById(id);
        }
        [HttpGet("GetVehicleResourceRequiredById/{id}")]
        public async Task<ApiCommonResponse> GetVehicleResourceRequiredById(long id)
        {
            return await _serviceRegistration.GetVehicleResourceById(id);
        }

        [HttpPost("AddNewServiceReg")]
        public async Task<ApiCommonResponse> AddNewServiceReg(ServiceRegistrationReceivingDTO ReceivingDTO)
        {
            return await _serviceRegistration.AddServiceReg(HttpContext, ReceivingDTO);
        }

        [HttpPost("AddNewResourceRequired")]
        public async Task<ApiCommonResponse> AddNewResourceRequiredTypes(AllResourceTypesPerServiceRegReceivingDTO ReceivingDTO)
        {
            return await _serviceRegistration.AddResourceRequired(HttpContext, ReceivingDTO);
        }

        [HttpPut("UpdateRegServiceById/{id}")]
        public async Task<ApiCommonResponse> UpdateRegServiceById(long id, ServiceRegistrationReceivingDTO ReceivingDTO)
        {
            return await _serviceRegistration.UpdateServiceReg(HttpContext, id, ReceivingDTO);
        }

        [HttpDelete("DeleteRegServiceById/{id}")]
        public async Task<ApiCommonResponse> DeleteRegServiceById(int id)
        {
            return await _serviceRegistration.DeleteServiceReg(id);
        }
        [HttpDelete("DeleteArmedEscortResourceRequiredById/{id}")]
        public async Task<ApiCommonResponse> DeleteArmedEscortResourceRequiredById(int id)
        {
            return await _serviceRegistration.DeleteArmedEscortResource(id);
        }
        [HttpDelete("DeletePilotResourceRequiredById/{id}")] 
        public async Task<ApiCommonResponse> DeletePilotResourceRequiredById(int id)
        {
            return await _serviceRegistration.DeletePilotResource(id);
        }
        [HttpDelete("DeleteCommanderResourceRequiredById/{id}")]
        public async Task<ApiCommonResponse> DeleteCommanderResourceRequiredById(int id)
        {
            return await _serviceRegistration.DeleteCommanderResource(id);
        }
        [HttpDelete("DeleteVehicleResourceRequiredById/{id}")]
        public async Task<ApiCommonResponse> DeleteVehicleResourceRequiredById(int id)
        {
            return await _serviceRegistration.DeleteVehicleResource(id);
        }


    }
}
