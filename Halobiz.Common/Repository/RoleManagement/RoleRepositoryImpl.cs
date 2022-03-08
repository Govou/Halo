using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HalobizMigrations.Data;
using HalobizMigrations.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Halobiz.Repository.RoleManagement
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
        Task<IEnumerable<Role>> FindRolesByUser(long userId);
    }

    public class RoleRepositoryImpl : IRoleRepository
    {
        private readonly HalobizContext _context;
        private readonly ILogger<RoleRepositoryImpl> _logger;
        public RoleRepositoryImpl(HalobizContext context, ILogger<RoleRepositoryImpl> logger)
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
            return await _context.Roles.Where(x => x.IsDeleted == false)
                .FirstOrDefaultAsync( role => role.Id == Id);
        }

        public async Task<Role> FindRoleByName(string name)
        {
            return await _context.Roles.Where(x => x.IsDeleted == false && x.Name == name)
                .FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<Role>> FindRolesByUser(long userId)
        {
            var userRole =  await _context.UserRoles.Where(x => x.IsDeleted == false && x.UserId==userId)
                .Include(x => x.Role)
                .ToListAsync();

            List<Role> roles = new List<Role>();
            foreach (var item in userRole)
            {
                roles.Add(item.Role);
            }

            return roles;
        }

        public async Task<IEnumerable<Role>> FindAllRole()
        {
            return await _context.Roles.Where(x => x.IsDeleted == false)
                .ToListAsync();
        }

        public async Task<IEnumerable<Claim>> FindAllClaims()
        {
            return await _context.Claims.Where(x => x.IsDeleted == false).ToListAsync();
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
            await DeleteRoleClaims(role);

            role.IsDeleted = true;
            _context.Roles.Update(role);       
            return await SaveChanges();
        }

        public async Task<bool> DeleteRoleClaims(Role role)
        {
           
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