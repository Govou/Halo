using System.Threading.Tasks;
using Halobiz.Common.Auths;
using Halobiz.Common.DTOs.ApiDTOs;
using HaloBiz.DTOs.ReceivingDTOs;
using HaloBiz.MyServices;
using Microsoft.AspNetCore.Mvc;

namespace HaloBiz.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    [ModuleName(HalobizModules.Setups)]

    public class ServiceCategoryTaskController : ControllerBase
    {
        private readonly IServiceCategoryTaskService _serviceCategoryTaskService;

        public ServiceCategoryTaskController(IServiceCategoryTaskService serviceCategoryTaskService)
        {
            this._serviceCategoryTaskService = serviceCategoryTaskService;
        }

        [HttpGet("")]
        public async Task<ApiCommonResponse> GetServiceCategoryTask()
        {
            return await _serviceCategoryTaskService.GetAllServiceCategoryTasks();
        }

        [HttpGet("name/{name}")]
        public async Task<ApiCommonResponse> GetByCaption(string name)
        {
            return await _serviceCategoryTaskService.GetServiceCategoryTaskByName(name);
        }

        [HttpGet("{id}")]
        public async Task<ApiCommonResponse> GetById(long id)
        {
            return await _serviceCategoryTaskService.GetServiceCategoryTaskById(id);
        }

        [HttpPost("")]
        public async Task<ApiCommonResponse> AddNewServiceCategoryTask(ServiceCategoryTaskReceivingDTO ServiceCategoryTaskReceiving)
        {
            return await _serviceCategoryTaskService.AddServiceCategoryTask(HttpContext, ServiceCategoryTaskReceiving);
        }

        [HttpPut("{id}")]
        public async Task<ApiCommonResponse> UpdateById(long id, ServiceCategoryTaskReceivingDTO ServiceCategoryTaskReceiving)
        {
            return await _serviceCategoryTaskService.UpdateServiceCategoryTask(HttpContext, id, ServiceCategoryTaskReceiving);
        }

        [HttpDelete("{id}")]
        public async Task<ApiCommonResponse> DeleteById(int id)
        {
            return await _serviceCategoryTaskService.DeleteServiceCategoryTask(id);
        }
    }
}