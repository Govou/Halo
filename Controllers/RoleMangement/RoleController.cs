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
        public async Task<ApiCommonResponse> GetRoles()
        {
            return await _roleService.GetAllRoles();
        }

        [HttpGet("GetAllApplicationClaims")]
        public async Task<ApiCommonResponse> GetApplicationClaims()
        {
            return await _roleService.GetAllClaims();
        }

        [HttpGet("GetUserRoleClaims")]
        public async Task<ApiCommonResponse> GetUserRoleClaims()
        {
            return await _roleService.GetUserRoleClaims(HttpContext);
        }

        [HttpGet("GetPermissions")]
        public ApiCommonResponse GetPermissions()
        {
            return _roleService.GetPermissions();
        }

        [HttpGet("GetPermissionsOnRole/{name}")]
        public async Task<ApiCommonResponse> GetPermissionsOnRole(string name)
        {
            return await _roleService.GetPermissionsOnRole(name);
        }

        [HttpGet("GetPermissionsOnUser/{id}")]
        public async Task<ApiCommonResponse> GetPermissionsOnUser(long id)
        {
            return await _roleService.GetPermissionsOnUser(id);
        }

        [HttpGet("GetGroupedPermissions")]
        public ApiCommonResponse GetGroupedPermissions()
        {
            return _roleService.GetGroupedPermissions();
        }

        [HttpGet("GetRolesByUser/{id}")]
        public async Task<ApiCommonResponse> GetRolesByUser(long id)
        {
            return await _roleService.FindRolesByUser(id);
        }

        [HttpGet("name/{name}")]
        public async Task<ApiCommonResponse> GetByName(string name)
        {
            return await _roleService.GetRoleByName(name);
        }
     
        [HttpGet("{id}")]
        public async Task<ApiCommonResponse> GetById(long id)
        {
            return await _roleService.GetRoleById(id);
        }
     
        [HttpPost("")]
        public async Task<ApiCommonResponse> AddNewRole(RoleReceivingDTO roleReceiving)
        {
            return await _roleService.AddRole(HttpContext, roleReceiving);
        }

        
        [HttpPut("{id}")]
        public async Task<ApiCommonResponse> UpdateById(long id, RoleReceivingDTO roleReceiving)
        {
            return await _roleService.UpdateRole(HttpContext, id, roleReceiving);
        }

        
        [HttpDelete("{id}")]
        public async Task<ApiCommonResponse> DeleteById(int id)
        {
            return await _roleService.DeleteRole(id);
        }
    }
}
