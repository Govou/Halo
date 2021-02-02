using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HaloBiz.Data;
using HaloBiz.Model;
using HaloBiz.Model.RoleManagement;
using HaloBiz.Repository.RoleManagement;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace HaloBiz.Repository.Impl.RoleManagement
{
    public class RoleRepositoryImpl : IRoleRepository
    {
        private readonly DataContext _context;
        private readonly ILogger<RoleRepositoryImpl> _logger;
        public RoleRepositoryImpl(DataContext context, ILogger<RoleRepositoryImpl> logger)
        {
            this._logger = logger;
            this._context = context;
        }

        public async Task<Role> SaveRole(Role role)
        {
            var roleEntity = await _context.Roles.AddAsync(role);
            if(await SaveChanges())
            {
                return roleEntity.Entity;
            }
            return null;
        }

        public async Task<Role> FindRoleById(long Id)
        {
            return await _context.Roles.Include(x => x.RoleClaims)
                .FirstOrDefaultAsync( role => role.Id == Id);
        }

        public async Task<Role> FindRoleByName(string name)
        {
            return await _context.Roles.Include(x => x.RoleClaims)
                .FirstOrDefaultAsync( role => role.Name == name);
        }

        public async Task<IEnumerable<Role>> FindAllRole()
        {
            return await _context.Roles.Include(x => x.RoleClaims)
                .ToListAsync();
        }

        public async Task<Role> UpdateRole(Role role)
        {
            var roleEntity =  _context.Roles.Update(role);
            if(await SaveChanges())
            {
                return roleEntity.Entity;
            }
            return null;
        }

        public async Task<bool> DeleteRole(Role role)
        {
            _context.RoleClaims.RemoveRange(role.RoleClaims);
            _context.Roles.Remove(role);
            return await SaveChanges();
        }

        public async Task<bool> DeleteRoleClaims(Role role)
        {
            _context.RoleClaims.RemoveRange(role.RoleClaims);
            return await SaveChanges();
        }

        private async Task<bool> SaveChanges()
        {
           try
           {
               return  await _context.SaveChangesAsync() > 0;
           }
           catch(Exception ex)
           {
               _logger.LogError(ex.Message);
               return false;
           }
        }
    }
}