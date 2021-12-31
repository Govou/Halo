using Auth.PermissionParts;
using HaloBiz.DTOs.ApiDTOs;
using HaloBiz.DTOs.ReceivingDTOs.RoleManagement;
using Microsoft.AspNetCore.Http;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HaloBiz.MyServices.RoleManagement
{
    public interface IRoleService
    {
        Task<ApiResponse> AddRole(HttpContext context, RoleReceivingDTO RoleReceivingDTO);
        Task<ApiResponse> GetRoleById(long id);
        Task<ApiResponse> GetRoleByName(string name);
        Task<ApiResponse> GetAllRoles();
        Task<ApiResponse> UpdateRole(HttpContext context, long id, RoleReceivingDTO RoleReceivingDTO);
        Task<ApiResponse> DeleteRole(long id);
        Task<ApiResponse> GetAllClaims();
        Task<ApiResponse> GetUserRoleClaims(HttpContext context);
        ApiResponse GetPermissions();
        ApiResponse GetGroupedPermissions();
        Task<ApiResponse> GetPermissionsOnRole(string name);
        Task<ApiResponse> FindRolesByUser(long userId);        
        Task<ApiResponse> GetPermissionsOnUser(long userId);
        Task<IEnumerable<Permissions>> GetPermissionEnumsOnUser(long userId);


    }
}
