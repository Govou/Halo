using Halobiz.Common.Auths;
using Halobiz.Common.DTOs.ApiDTOs;
using HaloBiz.DTOs.ReceivingDTOs;
using HaloBiz.MyServices;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace HaloBiz.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    [ModuleName(HalobizModules.Setups,98)]
    public class ServicesController : ControllerBase
    {
        private readonly IServicesService _servicesService;

        public ServicesController(IServicesService servicesService)
        {
            this._servicesService = servicesService;
        }

        [HttpGet("")]
        public async Task<ApiCommonResponse> GetService()
        {
            return await _servicesService.GetAllServices();
        }
        [HttpGet("AllSecuredMobilityServices")]
        public async Task<ApiCommonResponse> AllSecuredMobilityServices()
        {
            return await _servicesService.GetAllSecuredMobilityServices();
        }
        [HttpGet("GetUnpublishedServices")]
        public async Task<ApiCommonResponse> GetUnpublishedService()
        {
            return await _servicesService.GetUnpublishedServices();
        }

        [HttpGet("GetOnlinePortalServices")]
        public async Task<ApiCommonResponse> GetOnlinePortalServices()
        {
            return await _servicesService.GetOnlinePortalServices(); 
        }

        [HttpGet("name/{name}")]
        public async Task<ApiCommonResponse> GetByName(string name)
        {
            return await _servicesService.GetServiceByName(name); 
        }

        [HttpGet("{id}")]
        public async Task<ApiCommonResponse> GetById(long id)
        {
            return await _servicesService.GetServiceById(id); 
        }

        [HttpPost("")]
        public async Task<ApiCommonResponse> AddNew(ServiceReceivingDTO servicesReceivingDTO)
        {
            return await _servicesService.AddService(HttpContext, servicesReceivingDTO); 
        }

        [HttpPut("approve-service/{serviceId}/{sequence}")]
        public async Task<ApiCommonResponse> ApproveServiceById(long serviceId, long sequence)
        {
            return await _servicesService.ApproveService(HttpContext, serviceId, sequence); 
        }

        [HttpPut("disapprove-service/{serviceId}/{sequence}")]
        public async Task<ApiCommonResponse> DisapproveServiceById(long serviceId, long sequence)
        {
            return await _servicesService.DisapproveService(HttpContext, serviceId, sequence); 
        }

        [HttpPut("{id}/request-service-publish")]
        public async Task<ApiCommonResponse> RequestPublishServiceById(long id)
        {
            return await _servicesService.RequestPublishService(HttpContext, id); 
        }

        [HttpPut("{id}")]
        public async Task<ApiCommonResponse> UpdateById(long id, ServiceReceivingDTO servicesReceivingDTO)
        {
            return await _servicesService.UpdateServices(HttpContext, id, servicesReceivingDTO); 
        }



        [HttpDelete("{id}")]
        public async Task<ApiCommonResponse> DeleteById(int id)
        {
            return await _servicesService.DeleteService(id);
         }
        
    }
}