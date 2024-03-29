using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using HalobizMigrations.Data;
using Halobiz.Common.DTOs.ApiDTOs;
using HalobizMigrations.Models;
using Microsoft.AspNetCore.Http;
using Halobiz.Common.Auths.PermissionParts;
using System.Text.RegularExpressions;
using Halobiz.Common.DTOs.ReceivingDTOs.RoleManagement;
using Halobiz.Repository.RoleManagement;
using Halobiz.Common.DTOs.TransferDTOs.RoleManagement;
using Halobiz.Common.Helpers;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using HaloBiz.Model;

namespace Halobiz.Common.MyServices.RoleManagement
{
    public interface IRoleService
    {
        Task<ApiCommonResponse> AddRole(HttpContext context, RoleReceivingDTO RoleReceivingDTO);
        Task<ApiCommonResponse> GetRoleById(long id);
        Task<ApiCommonResponse> GetRoleByName(string name);
        Task<ApiCommonResponse> GetAllRoles();
        Task<ApiCommonResponse> UpdateRole(HttpContext context, long id, RoleReceivingDTO RoleReceivingDTO);
        Task<ApiCommonResponse> DeleteRole(long id);
        ApiCommonResponse GetPermissions();
        ApiCommonResponse GetGroupedPermissions();
        Task<ApiCommonResponse> GetPermissionsOnRole(string name);
        Task<ApiCommonResponse> FindRolesByUser(long userId);
        Task<ApiCommonResponse> GetPermissionsOnUser(long userId);
        Task<(IEnumerable<Permissions>, string[])> GetPermissionEnumsOnUser(long userId);


    }
    public class RoleServiceImpl : IRoleService
    {
        private readonly IMapper _mapper;
        private readonly IRoleRepository _roleRepo;
        private readonly HalobizContext _context;
        private readonly IConfiguration _configuration;

        public RoleServiceImpl(
            IRoleRepository roleRepo,
            HalobizContext dataContext,
            IConfiguration configuration,
            IMapper mapper)
        {
            _mapper = mapper;
            _context = dataContext;
            _roleRepo = roleRepo;
            _configuration = configuration;
        }

        public async Task<ApiCommonResponse> AddRole(HttpContext context, RoleReceivingDTO roleReceivingDto)
        {
            try
            {
                var item = await _roleRepo.FindRoleByName(roleReceivingDto.Name);
                if (item != null)
                {
                    return CommonResponse.Send(ResponseCodes.FAILURE, null, "Role Name Already Exists.");
                }

                List<Permissions> initialPermissions = new List<Permissions>();

                foreach (var permission in roleReceivingDto.Permissions)
                {
                    Enum.TryParse<Permissions>(permission.Permission, out Permissions result);
                    initialPermissions.Add(result);
                }

                var role = new RoleTemp(roleReceivingDto.Name, roleReceivingDto.Description, initialPermissions);

                var savedRole = await _roleRepo.SaveRole(role);
                if (savedRole == null)
                {
                    return CommonResponse.Send(ResponseCodes.FAILURE, null, "Some system errors occurred");
                }

                //var roleTransferDto = _mapper.Map<RoleTransferDTO>(role);
                return CommonResponse.Send(ResponseCodes.SUCCESS, role);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<ApiCommonResponse> FindRolesByUser(long userId)
        {
            var roles = await _roleRepo.FindRolesByUser(userId);
            //var roleTransferDto = _mapper.Map<IEnumerable<RoleTransferDTO>>(roles);
            return CommonResponse.Send(ResponseCodes.SUCCESS, roles);
        }

        public async Task<(IEnumerable<Permissions>,string[])> GetPermissionEnumsOnUser(long userId)
        {
            var roles = await _roleRepo.FindRolesByUser(userId);

            if (!roles.Any())
                return (new List<Permissions>(), new string[] { });

            var rolesList = roles.Select(x => x.Name).ToArray();

            List<Permissions> permissionsInRole = new List<Permissions>();

            //get all the permissions for the roles
            foreach (var role in roles)
            {
                RoleTemp roletemp = new RoleTemp { PermissionCode = role.PermissionCode };

                var thisPermissionInRole = roletemp.PermissionsInRole.ToList();
                permissionsInRole.AddRange(thisPermissionInRole);
            }

            //remove duplicates
            return (permissionsInRole.Distinct().ToList(), rolesList);
        }

        public async Task<ApiCommonResponse> GetPermissionsOnUser(long userId)
        {
            IEnumerable<PermissionDisplay> allPermissions = PermissionDisplay.GetPermissionsToDisplay(typeof(Permissions));

            var (permissionsInRole, roleList) = await GetPermissionEnumsOnUser(userId);
            List<PermissionDisplay> permissions = new List<PermissionDisplay>();

            foreach (PermissionDisplay permission in allPermissions)
            {
                if (permissionsInRole.Contains(permission.Permission))
                    permissions.Add(permission);
            }

            return CommonResponse.Send(ResponseCodes.SUCCESS, permissions);
        }


        public async Task<ApiCommonResponse> GetPermissionsOnRole(string name)
        {
            var role = await _roleRepo.FindRoleByName(name);
            if (role == null)
                return CommonResponse.Send(ResponseCodes.SUCCESS, new List<PermissionDisplay>());

            IEnumerable<PermissionDisplay> allPermissions = PermissionDisplay.GetPermissionsToDisplay(typeof(Permissions));
            List<PermissionDisplay> permissions = new List<PermissionDisplay>();

            var roletemp = new RoleTemp { PermissionCode = role.PermissionCode };

            var permissionsInRole = roletemp.PermissionsInRole.ToList();
            foreach (PermissionDisplay permission in allPermissions)
            {
                if (permissionsInRole.Contains(permission.Permission))
                    permissions.Add(permission);
            }

            return CommonResponse.Send(ResponseCodes.SUCCESS, permissions);
        }

        public async Task<ApiCommonResponse> UpdateRole(HttpContext context, long id, RoleReceivingDTO roleReceivingDto)
        {
            try
            {
                var role = await _context.Roles.Where(x => x.Id == id).FirstOrDefaultAsync() ;
                if (role == null)
                {
                    return CommonResponse.Send(ResponseCodes.FAILURE, null, $"Role {roleReceivingDto.Name} does not exists.");
                }

                //check if this is the admin role
                var adminInfo = _configuration.GetSection("AdminRoleInformation")?.Get<AdminRole>();
                if (adminInfo.RoleName.ToLower() == roleReceivingDto.Name.ToLower())
                {
                    return CommonResponse.Send(ResponseCodes.FAILURE, null, $"You cannot edit this super role '{adminInfo.RoleName}'");
                }

                List<Permissions> initialPermissions = new List<Permissions>();

                foreach (var permission in roleReceivingDto.Permissions)
                {
                    Enum.TryParse<Permissions>(permission.Permission, out Permissions result);
                    initialPermissions.Add(result);
                }

                var roletemp = new RoleTemp(roleReceivingDto.Name, roleReceivingDto.Description, initialPermissions);

                //alter some properties of the role
                role.PermissionCode = roletemp.PermissionCode;
                role.Name = roleReceivingDto.Name;
                role.Description = roleReceivingDto.Description;

                await _context.SaveChangesAsync();

                //var roleTransferDto = _mapper.Map<RoleTransferDTO>(role);
                return CommonResponse.Send(ResponseCodes.SUCCESS, role);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<ApiCommonResponse> GetAllRoles()
        {
            var roles = await _roleRepo.FindAllRole();
            if (roles == null)
            {
                return CommonResponse.Send(ResponseCodes.NO_DATA_AVAILABLE); ;
            }

            //var roleTransfer = _mapper.Map<IEnumerable<RoleTransferDTO>>(roles);
            return CommonResponse.Send(ResponseCodes.SUCCESS, roles);
        }




        public ApiCommonResponse GetPermissions()
        {
            var result = PermissionDisplay.GetPermissionsToDisplay(typeof(Permissions));
            return CommonResponse.Send(ResponseCodes.SUCCESS, result);

        }

        public ApiCommonResponse GetGroupedPermissions()
        {
            var result = PermissionDisplay.GetPermissionsToDisplay(typeof(Permissions));

            //group according the controller id
            var grouped = from e in result
                          group e by  e.ControllerModule into g
                          select new
                          {
                              GroupName = SplitControllerModule(g.Key).Item1,
                              SplitName = SplitCamelCase(SplitControllerModule(g.Key).Item1),
                              Module = SplitCamelCase(SplitControllerModule(g.Key).Item2),
                              PermissionList = g.Select(x => new PermissionDisplay
                              (
                                  x.Controller,
                                  x.Action,
                                  x.Description,
                                  SplitCamelCase(x.Module),
                                  x.Permission
                              )).ToList()
                          };

            return CommonResponse.Send(ResponseCodes.SUCCESS, grouped);

        }

        string SplitCamelCase(string source)
        {
            return string.Join(" ", Regex.Split(source, @"(?<!^)(?=[A-Z](?![A-Z]|$))"));
        }

        (string,string) SplitControllerModule(string source)
        {
            var split = source.Split('_');
            return (split[0], split[1]); 
        }


        public async Task<ApiCommonResponse> GetRoleById(long id)
        {
            var role = await _roleRepo.FindRoleById(id);
            if (role == null)
            {
                return CommonResponse.Send(ResponseCodes.NO_DATA_AVAILABLE); ;
            }
           // var roleTransferDtOs = _mapper.Map<RoleTransferDTO>(role);
            return CommonResponse.Send(ResponseCodes.SUCCESS, role);
        }


        public async Task<ApiCommonResponse> GetRoleByName(string name)
        {
            var role = await _roleRepo.FindRoleByName(name);
            if (role == null)
            {
                return CommonResponse.Send(ResponseCodes.NO_DATA_AVAILABLE); ;
            }
           // var roleTransferDtOs = Mapping.Mapper.Map<RoleTransferDTO>(role);
            return CommonResponse.Send(ResponseCodes.SUCCESS, role);
        }



        public async Task<ApiCommonResponse> DeleteRole(long id)
        {
            var roleToDelete = await _roleRepo.FindRoleById(id);
            if (roleToDelete == null)
            {
                return CommonResponse.Send(ResponseCodes.NO_DATA_AVAILABLE); ;
            }




            if (!await _roleRepo.DeleteRole(roleToDelete))
            {
                return CommonResponse.Send(ResponseCodes.FAILURE, null, "Some system errors occurred");
            }

            return CommonResponse.Send(ResponseCodes.SUCCESS);

        }
    }
}