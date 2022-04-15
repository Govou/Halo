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
    [ModuleName(HalobizModules.Setups,99)]
    public class ServiceTaskDeliverableController : ControllerBase
    {
        private readonly IServiceTaskDeliverableService _serviceTaskDeliverableService;

        public ServiceTaskDeliverableController(IServiceTaskDeliverableService serviceTaskDeliverableService)
        {
            this._serviceTaskDeliverableService = serviceTaskDeliverableService;
        }

        [HttpGet("")]
        public async Task<ApiCommonResponse> GetServiceTaskDeliverable()
        {
            return await _serviceTaskDeliverableService.GetAllServiceTaskDeliverables(); 
        }

        [HttpGet("name/{name}")]
        public async Task<ApiCommonResponse> GetByCaption(string name)
        {
            return await _serviceTaskDeliverableService.GetServiceTaskDeliverableByName(name); 
        }

        [HttpGet("{id}")]
        public async Task<ApiCommonResponse> GetById(long id)
        {
            return await _serviceTaskDeliverableService.GetServiceTaskDeliverableById(id); 
        }

        [HttpPost("")]
        public async Task<ApiCommonResponse> AddNewServiceTaskDeliverable(ServiceTaskDeliverableReceivingDTO ServiceTaskDeliverableReceiving)
        {
            return await _serviceTaskDeliverableService.AddServiceTaskDeliverable(HttpContext, ServiceTaskDeliverableReceiving);
            
        }

        [HttpPut("{id}")]
        public async Task<ApiCommonResponse> UpdateById(long id, ServiceTaskDeliverableReceivingDTO ServiceTaskDeliverableReceiving)
        {
            return await _serviceTaskDeliverableService.UpdateServiceTaskDeliverable(HttpContext, id, ServiceTaskDeliverableReceiving);
             
        }

        [HttpDelete("{id}")]
        public async Task<ApiCommonResponse> DeleteById(int id)
        {
            return await _serviceTaskDeliverableService.DeleteServiceTaskDeliverable(id);
        }
    }
}