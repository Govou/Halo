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
        Task<ApiCommonResponse> AddRole(HttpContext context, RoleReceivingDTO RoleReceivingDTO);
        Task<ApiCommonResponse> GetRoleById(long id);
        Task<ApiCommonResponse> GetRoleByName(string name);
        Task<ApiCommonResponse> GetAllRoles();
        Task<ApiCommonResponse> UpdateRole(HttpContext context, long id, RoleReceivingDTO RoleReceivingDTO);
        Task<ApiCommonResponse> DeleteRole(long id);
        Task<ApiCommonResponse> GetAllClaims();
        Task<ApiCommonResponse> GetUserRoleClaims(HttpContext context);
        ApiResponse GetPermissions();
        ApiResponse GetGroupedPermissions();
        Task<ApiCommonResponse> GetPermissionsOnRole(string name);
        Task<ApiCommonResponse> FindRolesByUser(long userId);        
        Task<ApiCommonResponse> GetPermissionsOnUser(long userId);
        Task<IEnumerable<Permissions>> GetPermissionEnumsOnUser(long userId);


    }
}
