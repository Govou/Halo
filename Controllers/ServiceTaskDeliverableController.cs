using System.Threading.Tasks;
using HaloBiz.DTOs.ApiDTOs;
using HaloBiz.DTOs.ReceivingDTOs;
using HaloBiz.MyServices;
using Microsoft.AspNetCore.Mvc;

namespace HaloBiz.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class ServiceTaskDeliverableController : ControllerBase
    {
        private readonly IServiceTaskDeliverableService _serviceTaskDeliverableService;

        public ServiceTaskDeliverableController(IServiceTaskDeliverableService serviceTaskDeliverableService)
        {
            this._serviceTaskDeliverableService = serviceTaskDeliverableService;
        }

        [HttpGet("")]
        public async Task<ActionResult> GetServiceTaskDeliverable()
        {
            var response = await _serviceTaskDeliverableService.GetAllServiceTaskDeliverables();
            if (response.StatusCode >= 400)
                return StatusCode(response.StatusCode, response);
            var serviceTaskDeliverable = ((ApiOkResponse)response).Result;
            return Ok(serviceTaskDeliverable);
        }

        [HttpGet("name/{name}")]
        public async Task<ActionResult> GetByCaption(string name)
        {
            var response = await _serviceTaskDeliverableService.GetServiceTaskDeliverableByName(name);
            if (response.StatusCode >= 400)
                return StatusCode(response.StatusCode, response);
            var ServiceTaskDeliverable = ((ApiOkResponse)response).Result;
            return Ok(ServiceTaskDeliverable);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult> GetById(long id)
        {
            var response = await _serviceTaskDeliverableService.GetServiceTaskDeliverableById(id);
            if (response.StatusCode >= 400)
                return StatusCode(response.StatusCode, response);
            var ServiceTaskDeliverable = ((ApiOkResponse)response).Result;
            return Ok(ServiceTaskDeliverable);
        }

        [HttpPost("")]
        public async Task<ActionResult> AddNewServiceTaskDeliverable(ServiceTaskDeliverableReceivingDTO ServiceTaskDeliverableReceiving)
        {
            var response = await _serviceTaskDeliverableService.AddServiceTaskDeliverable(HttpContext, ServiceTaskDeliverableReceiving);
            if (response.StatusCode >= 400)
                return StatusCode(response.StatusCode, response);
            var serviceTaskDeliverable = ((ApiOkResponse)response).Result;
            return Ok(serviceTaskDeliverable);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateById(long id, ServiceTaskDeliverableReceivingDTO ServiceTaskDeliverableReceiving)
        {
            var response = await _serviceTaskDeliverableService.UpdateServiceTaskDeliverable(HttpContext, id, ServiceTaskDeliverableReceiving);
            if (response.StatusCode >= 400)
                return StatusCode(response.StatusCode, response);
            var serviceTaskDeliverable = ((ApiOkResponse)response).Result;
            return Ok(serviceTaskDeliverable);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteById(int id)
        {
            var response = await _serviceTaskDeliverableService.DeleteServiceTaskDeliverable(id);
            return StatusCode(response.StatusCode);
        }
    }
}