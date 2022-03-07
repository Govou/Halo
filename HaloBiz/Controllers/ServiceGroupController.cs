using Halobiz.Common.DTOs.ApiDTOs;
using HaloBiz.DTOs.ReceivingDTOs;
using HaloBiz.MyServices;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;


namespace HaloBiz.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class ServiceGroupController : ControllerBase
    {
        private readonly IServiceGroupService _serviceGroupService;

        public ServiceGroupController(IServiceGroupService serviceGroupService)
        {
            this._serviceGroupService = serviceGroupService;
        }

        [HttpGet("")]
        public async Task<ApiCommonResponse> GetServiceGroup()
        {
            return await _serviceGroupService.GetAllServiceGroups();
        }

        [HttpGet("name/{name}")]
        public async Task<ApiCommonResponse> GetByName(string name)
        {
            return await _serviceGroupService.GetServiceGroupByName(name);
        }

        [HttpGet("{id}")]
        public async Task<ApiCommonResponse> GetById(long id)
        {
            return await _serviceGroupService.GetServiceGroupById(id);
        }

        [HttpPost("")]
        public async Task<ApiCommonResponse> AddNew(ServiceGroupReceivingDTO serviceGroupReceivingDTO)
        {
            return await _serviceGroupService.AddServiceGroup(serviceGroupReceivingDTO);
        }

        [HttpPut("{id}")]
        public async Task<ApiCommonResponse> UpdateById(long id, ServiceGroupReceivingDTO serviceGroupReceivingDTO)
        {
            return await _serviceGroupService.UpdateServiceGroup(id, serviceGroupReceivingDTO);
        }

        [HttpDelete("{id}")]
        public async Task<ApiCommonResponse> DeleteById(int id)
        {
            return await _serviceGroupService.DeleteServiceGroup(id);
        }
    }
}
