using HaloBiz.DTOs.ApiDTOs;
using HaloBiz.DTOs.ReceivingDTOs;
using HaloBiz.MyServices;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace HaloBiz.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class ServiceRelationshipsController : ControllerBase
    {
        private readonly IServiceRelationshipService _servicesService;

        public ServiceRelationshipsController(IServiceRelationshipService servicesService)
        {
            _servicesService = servicesService;
        }

        [HttpGet]
        [Route("FindAllUnmappedDirects")]
        public async Task<ApiCommonResponse> FindAllUnmappedDirects()
        {
            return await _servicesService.FindAllUnmappedDirects();
        }

        [HttpGet]
        [Route("FindAllRelationships")]
        public async Task<ApiCommonResponse> FindAllRelationships()
        {
            return await _servicesService.FindAllRelationships();
        }


        [HttpGet("FindByAdminId/{id}")]
        public async Task<ApiCommonResponse> FindServiceRelationshipByAdminId(long id)
        {
            return await _servicesService.FindServiceRelationshipByAdminId(id);
        }

        [HttpGet("FindByDirectId/{id}")]
        public async Task<ApiCommonResponse> FindServiceRelationshipByDirectId(long id)
        {
            return await _servicesService.FindServiceRelationshipByDirectId(id);
        }

        [HttpGet("FindAllDirectServices")]
        public async Task<ApiCommonResponse> FindAllDirectServices()
        {
            return await _servicesService.FindAllDirectServices();
        }


    }
}