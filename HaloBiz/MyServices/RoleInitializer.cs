using Halobiz.Common.Auths.PermissionParts;
using Halobiz.Common.DTOs.ReceivingDTOs.RoleManagement;
using HaloBiz.Model;
using HalobizMigrations.Data;
using HalobizMigrations.Models;
using HalobizMigrations.Models.Halobiz;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace HaloBiz.MyServices
{
    public static class RoleInitializer
    {
      

        public static async Task AddAdminRole(this IServiceProvider serviceProvider)
        {
            using (var scope = serviceProvider.CreateScope())
            {
                var services = scope.ServiceProvider;
                var configuration = services.GetRequiredService<IConfiguration>();

                try
                {
                    var adminInfo = configuration.GetSection("AdminRoleInformation")?.Get<AdminRole>();
                    using (var context = services.GetRequiredService<HalobizContext>())
                    {
                        //get seeder profile
                        var seeder = await context.UserProfiles.Where(x => x.Email.ToLower().Contains("seeder")).FirstOrDefaultAsync();
                        if (seeder == null)
                            throw new Exception("No seeder profile created");

                        //check if the role exist
                        var adminRole = await context.Roles.Where(x => x.Name == adminInfo.RoleName).FirstOrDefaultAsync();
                        if (adminRole == null)
                        {
                            List<Permissions> initialPermissions = new List<Permissions>() { Permissions.NotSet };
                            var role = new RoleTemp(adminInfo.RoleName, adminInfo.RoleDescription, initialPermissions);
                            var roleEntity = await context.Roles.AddAsync(role);
                            await context.SaveChangesAsync();
                            adminRole = roleEntity.Entity;
                        }

                        //check if users here have been assigned the this role
                        foreach (var email in adminInfo.RoleAssignees)
                        {
                            var userprofile = await context.UserProfiles.Where(x => x.Email.ToLower() == email).FirstOrDefaultAsync();
                            if (userprofile == null)
                                continue;

                            //check if this user is assigned the admin role, else assign him
                            if(!context.UserRoles.Any(x=>x.UserId==userprofile.Id && x.RoleId == adminRole.Id))
                            {
                                //create the user with this user
                                await context.UserRoles.AddAsync(new UserRole
                                {
                                    RoleId = adminRole.Id,
                                    UserId = userprofile.Id,
                                    CreatedAt = DateTime.Now,
                                    UpdatedAt = DateTime.Now,
                                    CreatedById = seeder.Id
                                });

                                await context.SaveChangesAsync();
                            }
                        }

                    }
                }
                catch (Exception ex)
                {
                    throw;
                }
            }
        }
    }   
}
