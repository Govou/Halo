using System.Collections.Generic;
using System.Threading.Tasks;
using HalobizMigrations.Models;

namespace HaloBiz.Repository.RoleManagement
{
    public interface IRoleRepository
    {
        Task<Role> SaveRole(Role role);
        Task<Role> FindRoleById(long Id);
        Task<Role> FindRoleByName(string name);
        Task<IEnumerable<Role>> FindAllRole();
        Task<IEnumerable<Claim>> FindAllClaims();
        Task<Role> UpdateRole(Role role);
        Task<bool> DeleteRole(Role role);
        Task<bool> DeleteRoleClaims(Role role);
    }
}