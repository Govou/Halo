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
        public async Task<ActionResult> FindAllUnmappedDirects()
        {
            var response = await _servicesService.FindAllUnmappedDirects();
            if (response.StatusCode >= 400)
                return StatusCode(response.StatusCode, response);
            var services = ((ApiOkResponse)response).Result;
            return Ok(services);
        }

        [HttpGet]
        [Route("FindAllRelationships")]
        public async Task<ActionResult> FindAllRelationships()
        {
            var response = await _servicesService.FindAllRelationships();
            if (response.StatusCode >= 400)
                return StatusCode(response.StatusCode, response);
            var services = ((ApiOkResponse)response).Result;
            return Ok(services);
        }


        [HttpGet("FindByAdminId/{id}")]
        public async Task<ActionResult> FindServiceRelationshipByAdminId(long id)
        {
            var response = await _servicesService.FindServiceRelationshipByAdminId(id);
            if (response.StatusCode >= 400)
                return StatusCode(response.StatusCode, response);
            var service = ((ApiOkResponse)response).Result;
            return Ok(service);
        }

        [HttpGet("FindByDirectId/{id}")]
        public async Task<ActionResult> FindServiceRelationshipByDirectId(long id)
        {
            var response = await _servicesService.FindServiceRelationshipByDirectId(id);
            if (response.StatusCode >= 400)
                return StatusCode(response.StatusCode, response);
            var service = ((ApiOkResponse)response).Result;
            return Ok(service);
        }


    }
}