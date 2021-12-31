using HaloBiz.DTOs.ApiDTOs;
using HaloBiz.DTOs.ReceivingDTOs;
using HaloBiz.MyServices;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace HaloBiz.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
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
            var response = await _servicesService.GetAllServices();
            if (response.StatusCode >= 400)
                return StatusCode(response.StatusCode, response);
            var services = ((ApiOkResponse)response).Result;
            return Ok(services);
        }

        [HttpGet("GetUnpublishedServices")]
        public async Task<ApiCommonResponse> GetUnpublishedService()
        {
            var response = await _servicesService.GetUnpublishedServices();
            if (response.StatusCode >= 400)
                return StatusCode(response.StatusCode, response);
            var services = ((ApiOkResponse)response).Result;
            return Ok(services);
        }

        [HttpGet("GetOnlinePortalServices")]
        public async Task<ApiCommonResponse> GetOnlinePortalServices()
        {
            var response = await _servicesService.GetOnlinePortalServices();
            if (response.StatusCode >= 400)
                return StatusCode(response.StatusCode, response);
            var services = ((ApiOkResponse)response).Result;
            return Ok(services);
        }

        [HttpGet("name/{name}")]
        public async Task<ApiCommonResponse> GetByName(string name)
        {
            var response = await _servicesService.GetServiceByName(name);
            if (response.StatusCode >= 400)
                return StatusCode(response.StatusCode, response);
            var service = ((ApiOkResponse)response).Result;
            return Ok(service);
        }

        [HttpGet("{id}")]
        public async Task<ApiCommonResponse> GetById(long id)
        {
            var response = await _servicesService.GetServiceById(id);
            if (response.StatusCode >= 400)
                return StatusCode(response.StatusCode, response);
            var service = ((ApiOkResponse)response).Result;
            return Ok(service);
        }

        [HttpPost("")]
        public async Task<ApiCommonResponse> AddNew(ServiceReceivingDTO servicesReceivingDTO)
        {
            var response = await _servicesService.AddService(HttpContext, servicesReceivingDTO);
            if (response.StatusCode >= 400)
                return StatusCode(response.StatusCode, response);
            var service = ((ApiOkResponse)response).Result;
            return Ok(service);
        }

        [HttpPut("approve-service/{serviceId}/{sequence}")]
        public async Task<IActionResult> ApproveServiceById(long serviceId, long sequence)
        {
            var response = await _servicesService.ApproveService(HttpContext, serviceId, sequence);
            if (response.StatusCode >= 400)
                return StatusCode(response.StatusCode, response);
            var serviceGroup = ((ApiOkResponse)response).Result;
            return Ok(serviceGroup);
        }

        [HttpPut("disapprove-service/{serviceId}/{sequence}")]
        public async Task<IActionResult> DisapproveServiceById(long serviceId, long sequence)
        {
            var response = await _servicesService.DisapproveService(HttpContext, serviceId, sequence);
            if (response.StatusCode >= 400)
                return StatusCode(response.StatusCode, response);
            var serviceGroup = ((ApiOkResponse)response).Result;
            return Ok(serviceGroup);
        }

        [HttpPut("{id}/request-service-publish")]
        public async Task<IActionResult> RequestPublishServiceById(long id)
        {
            var response = await _servicesService.RequestPublishService(HttpContext, id);
            if (response.StatusCode >= 400)
                return StatusCode(response.StatusCode, response);
            var serviceGroup = ((ApiOkResponse)response).Result;
            return Ok(serviceGroup);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateById(long id, ServiceReceivingDTO servicesReceivingDTO)
        {
            var response = await _servicesService.UpdateServices(HttpContext, id, servicesReceivingDTO);
            if (response.StatusCode >= 400)
                return StatusCode(response.StatusCode, response);
            var serviceGroup = ((ApiOkResponse)response).Result;
            return Ok(serviceGroup);
        }



        [HttpDelete("{id}")]
        public async Task<ApiCommonResponse> DeleteById(int id)
        {
            var response = await _servicesService.DeleteService(id);
            return StatusCode(response.StatusCode);
        }
        
    }
}