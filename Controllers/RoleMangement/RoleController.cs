using HaloBiz.DTOs.ApiDTOs;
using HaloBiz.DTOs.ReceivingDTOs.RoleManagement;
using HaloBiz.DTOs.TransferDTOs.RoleManagement;
using HaloBiz.Helpers;
using HaloBiz.MyServices.RoleManagement;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using HaloBiz.DTOs.TransferDTOs;

namespace HaloBiz.Controllers.RoleMangement
{
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
        public async Task<ActionResult> GetApplicationClaims()
        {
            var response = await _roleService.GetAllClaims();
            if (response.StatusCode >= 400)
                return StatusCode(response.StatusCode, response);

            var claims = ((ApiOkResponse)response).Result as IEnumerable<ClaimTransferDTO>;

            return Ok(claims);
        }

        [HttpGet("GetUserRoleClaims")]
        public async Task<ActionResult> GetUserRoleClaims()
        {
            var response = await _roleService.GetUserRoleClaims(HttpContext);
            if (response.StatusCode >= 400)
                return StatusCode(response.StatusCode, response);

            var claims = ((ApiOkResponse)response).Result as UserProfileTransferDTO;

            return Ok(claims);
        }

        [HttpGet("GetPermissions")]
        public ActionResult GetPermissions()
        {
            var response =  _roleService.GetPermissions();
            if (response.StatusCode >= 400)
                return StatusCode(response.StatusCode, response);

            var claims = ((ApiOkResponse)response).Result;

            return Ok(claims);
        }

        [HttpGet("GetPermissionsOnRole/{name}")]
        public async Task<ActionResult> GetPermissionsOnRole(string name)
        {
            var response = await _roleService.GetPermissionsOnRole(name);
            if (response.StatusCode >= 400)
                return StatusCode(response.StatusCode, response);

            var claims = ((ApiOkResponse)response).Result;

            return Ok(claims);
        }

        [HttpGet("GetPermissionsOnUser/{id}")]
        public async Task<ActionResult> GetPermissionsOnUser(long id)
        {
            var response = await _roleService.GetPermissionsOnUser(id);
            if (response.StatusCode >= 400)
                return StatusCode(response.StatusCode, response);

            var claims = ((ApiOkResponse)response).Result;

            return Ok(claims);
        }

        [HttpGet("GetGroupedPermissions")]
        public ActionResult GetGroupedPermissions()
        {
            var response = _roleService.GetGroupedPermissions();
            if (response.StatusCode >= 400)
                return StatusCode(response.StatusCode, response);

            var claims = ((ApiOkResponse)response).Result;

            return Ok(claims);
        }

        [HttpGet("GetRolesByUser/{id}")]
        public async Task<ActionResult> GetRolesByUser(long id)
        {
            var response = await _roleService.FindRolesByUser(id);
            if (response.StatusCode >= 400)
                return StatusCode(response.StatusCode, response);

            var claims = ((ApiOkResponse)response).Result;

            return Ok(claims);
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
