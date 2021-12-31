using System.Threading.Tasks;
using HaloBiz.DTOs.ApiDTOs;
using HaloBiz.DTOs.ReceivingDTOs;
using HaloBiz.MyServices;
using Microsoft.AspNetCore.Mvc;

namespace HaloBiz.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class ServiceCategoryTaskController : ControllerBase
    {
        private readonly IServiceCategoryTaskService _serviceCategoryTaskService;

        public ServiceCategoryTaskController(IServiceCategoryTaskService serviceCategoryTaskService)
        {
            this._serviceCategoryTaskService = serviceCategoryTaskService;
        }

        [HttpGet("")]
        public async Task<ActionResult> GetServiceCategoryTask()
        {
            var response = await _serviceCategoryTaskService.GetAllServiceCategoryTasks();
            if (response.StatusCode >= 400)
                return StatusCode(response.StatusCode, response);
            var serviceCategoryTask = ((ApiOkResponse)response).Result;
            return Ok(serviceCategoryTask);
        }

        [HttpGet("name/{name}")]
        public async Task<ActionResult> GetByCaption(string name)
        {
            var response = await _serviceCategoryTaskService.GetServiceCategoryTaskByName(name);
            if (response.StatusCode >= 400)
                return StatusCode(response.StatusCode, response);
            var ServiceCategoryTask = ((ApiOkResponse)response).Result;
            return Ok(ServiceCategoryTask);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult> GetById(long id)
        {
            var response = await _serviceCategoryTaskService.GetServiceCategoryTaskById(id);
            if (response.StatusCode >= 400)
                return StatusCode(response.StatusCode, response);
            var ServiceCategoryTask = ((ApiOkResponse)response).Result;
            return Ok(ServiceCategoryTask);
        }

        [HttpPost("")]
        public async Task<ActionResult> AddNewServiceCategoryTask(ServiceCategoryTaskReceivingDTO ServiceCategoryTaskReceiving)
        {
            var response = await _serviceCategoryTaskService.AddServiceCategoryTask(HttpContext, ServiceCategoryTaskReceiving);
            if (response.StatusCode >= 400)
                return StatusCode(response.StatusCode, response);
            var serviceCategoryTask = ((ApiOkResponse)response).Result;
            return Ok(serviceCategoryTask);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateById(long id, ServiceCategoryTaskReceivingDTO ServiceCategoryTaskReceiving)
        {
            var response = await _serviceCategoryTaskService.UpdateServiceCategoryTask(HttpContext, id, ServiceCategoryTaskReceiving);
            if (response.StatusCode >= 400)
                return StatusCode(response.StatusCode, response);
            var serviceCategoryTask = ((ApiOkResponse)response).Result;
            return Ok(serviceCategoryTask);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteById(int id)
        {
            var response = await _serviceCategoryTaskService.DeleteServiceCategoryTask(id);
            return StatusCode(response.StatusCode);
        }
    }
}