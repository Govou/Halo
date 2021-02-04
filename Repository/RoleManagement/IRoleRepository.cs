using System.Collections.Generic;
using System.Threading.Tasks;
using HaloBiz.Model;
using HaloBiz.Model.RoleManagement;

namespace HaloBiz.Repository.RoleManagement
{
    public interface IRoleRepository
    {
        Task<Role> SaveRole(Role role);
        Task<Role> FindRoleById(long Id);
        Task<Role> FindRoleByName(string name);
        Task<IEnumerable<Role>> FindAllRole();
        Task<Role> UpdateRole(Role role);
        Task<bool> DeleteRole(Role role);
        Task<bool> DeleteRoleClaims(Role role);
    }
}