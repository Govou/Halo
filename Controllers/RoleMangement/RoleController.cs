using HaloBiz.DTOs.ApiDTOs;
using HaloBiz.DTOs.ReceivingDTOs.RoleManagement;
using HaloBiz.Helpers;
using HaloBiz.MyServices.RoleManagement;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace HaloBiz.Controllers.RoleMangement
{
    [Authorize(Roles = ClaimConstants.SUPER_ADMIN)]
    [Route("api/v1/[controller]")]
    [ApiController]
    public class RoleController : Controller
    {
        private readonly IRoleService _roleService;
        public RoleController(IRoleService roleService)
        {
            _roleService = roleService;
        }
  
        [HttpGet("")]
        public async Task<ActionResult> GetRoles()
        {
            var response = await _roleService.GetAllRoles();
            if (response.StatusCode >= 400)
                return StatusCode(response.StatusCode, response);
            var role = ((ApiOkResponse)response).Result;
            return Ok(role);
        }

        [HttpGet("GetAllApplicationClaims")]
        public ActionResult GetApplicationClaims()
        {        
            return Ok(ClaimConstants.ApplicationClaims);
        }

        [HttpGet("name/{name}")]
        public async Task<ActionResult> GetByName(string name)
        {
            var response = await _roleService.GetRoleByName(name);
            if (response.StatusCode >= 400)
                return StatusCode(response.StatusCode, response);
            var role = ((ApiOkResponse)response).Result;
            return Ok(role);
        }
     
        [HttpGet("{id}")]
        public async Task<ActionResult> GetById(long id)
        {
            var response = await _roleService.GetRoleById(id);
            if (response.StatusCode >= 400)
                return StatusCode(response.StatusCode, response);
            var role = ((ApiOkResponse)response).Result;
            return Ok(role);
        }
     
        [HttpPost("")]
        public async Task<ActionResult> AddNewRole(RoleReceivingDTO roleReceiving)
        {
            var response = await _roleService.AddRole(HttpContext, roleReceiving);
            if (response.StatusCode >= 400)
                return StatusCode(response.StatusCode, response);
            var role = ((ApiOkResponse)response).Result;
            return Ok(role);
        }

        
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateById(long id, RoleReceivingDTO roleReceiving)
        {
            var response = await _roleService.UpdateRole(HttpContext, id, roleReceiving);
            if (response.StatusCode >= 400)
                return StatusCode(response.StatusCode, response);
            var role = ((ApiOkResponse)response).Result;
            return Ok(role);
        }

        
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteById(int id)
        {
            var response = await _roleService.DeleteRole(id);
            return StatusCode(response.StatusCode);
        }
    }
}
