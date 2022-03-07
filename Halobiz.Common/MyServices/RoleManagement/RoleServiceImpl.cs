//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Threading.Tasks;
//using AutoMapper;
//using HalobizMigrations.Data;
//using HaloBiz.DTOs.ApiDTOs;
//using HaloBiz.DTOs.ReceivingDTOs.RoleManagement;
//using HaloBiz.DTOs.TransferDTOs;
//using HaloBiz.DTOs.TransferDTOs.RoleManagement;
//using HaloBiz.Helpers;
//using HalobizMigrations.Models;
//using HaloBiz.Repository;
//using HaloBiz.Repository.RoleManagement;
//using Microsoft.AspNetCore.Http;
//using HaloBiz.Model.RoleManagement;
//using HaloBiz.MyServices.RoleManagement;
//using Auth.PermissionParts;
//using System.Text.RegularExpressions;

//namespace HaloBiz.MyServices.Impl.RoleManagement
//{
//    public class RoleServiceImpl : IRoleService
//    {
//        private readonly IModificationHistoryRepository _historyRepo;
//        private readonly IMapper _mapper;
//        private readonly IRoleRepository _roleRepo;
//        private readonly IUserProfileRepository _userProfileRepo;
//        private readonly HalobizContext _context;

//        public RoleServiceImpl(
//            IModificationHistoryRepository historyRepo,
//            IRoleRepository roleRepo,
//            IUserProfileRepository userProfileRepo,
//            HalobizContext dataContext,
//            IMapper mapper)
//        {
//            _mapper = mapper;
//            _historyRepo = historyRepo;
//            _context = dataContext;
//            _roleRepo = roleRepo;
//            _userProfileRepo = userProfileRepo;
//        }

//        public async Task<ApiCommonResponse> AddRole(HttpContext context, RoleReceivingDTO roleReceivingDto)
//        {
//            try
//            {
//                var item = await _roleRepo.FindRoleByName(roleReceivingDto.Name);
//                if (item != null)
//                {
//                    return CommonResponse.Send(ResponseCodes.FAILURE,null, "Role Name Already Exists.");
//                }

//                List<Permissions> initialPermissions = new List<Permissions>();

//                foreach (var permission in roleReceivingDto.Permissions)
//                {
//                    Enum.TryParse<Permissions>(permission.Permission, out Permissions result);
//                    initialPermissions.Add(result);
//                }

//                var role = new RoleTemp(roleReceivingDto.Name, roleReceivingDto.Description, initialPermissions);

//                var savedRole = await _roleRepo.SaveRole(role);
//                if (savedRole == null)
//                {
//                    return CommonResponse.Send(ResponseCodes.FAILURE, null, "Some system errors occurred");
//                }

//                var roleTransferDto = _mapper.Map<RoleTransferDTO>(role);
//                return CommonResponse.Send(ResponseCodes.SUCCESS,roleTransferDto);
//            }
//            catch (Exception ex)
//            {
//                throw;
//            }
//        }

//        public async Task<ApiCommonResponse> FindRolesByUser(long userId)
//        {
//            var roles = await _roleRepo.FindRolesByUser(userId);
//            var roleTransferDto = _mapper.Map<IEnumerable<RoleTransferDTO>>(roles);

//            return CommonResponse.Send(ResponseCodes.SUCCESS,roleTransferDto);
//        }  

//        public async Task<IEnumerable<Permissions>> GetPermissionEnumsOnUser(long userId)
//        {
//            var roles = await _roleRepo.FindRolesByUser(userId);

//            if (!roles.Any())
//                return new List<Permissions>();

//            List<Permissions> permissionsInRole = new List<Permissions>();

//            //get all the permissions for the roles
//            foreach (var role in roles)
//            {
//                RoleTemp roletemp = new RoleTemp { PermissionCode = role.PermissionCode };

//                var thisPermissionInRole = roletemp.PermissionsInRole.ToList();
//                permissionsInRole.AddRange(thisPermissionInRole);
//            }

//            //remove duplicates
//            return permissionsInRole.Distinct().ToList();
//        }

//        public async Task<ApiCommonResponse> GetPermissionsOnUser(long userId)
//        {
//            IEnumerable<PermissionDisplay> allPermissions = PermissionDisplay.GetPermissionsToDisplay(typeof(Permissions));

//            var permissionsInRole = await GetPermissionEnumsOnUser(userId);
//             List<PermissionDisplay> permissions = new List<PermissionDisplay>();

//            foreach (PermissionDisplay permission in allPermissions)
//            {
//                if (permissionsInRole.Contains(permission.Permission))
//                    permissions.Add(permission);
//            }

//            return CommonResponse.Send(ResponseCodes.SUCCESS,permissions);
//        }


//        public async Task<ApiCommonResponse> GetPermissionsOnRole(string name)
//        {
//            var role = await _roleRepo.FindRoleByName(name);
//            if (role == null)
//                return CommonResponse.Send(ResponseCodes.SUCCESS,new List<PermissionDisplay>());

//            IEnumerable<PermissionDisplay> allPermissions = PermissionDisplay.GetPermissionsToDisplay(typeof(Permissions));
//            List<PermissionDisplay> permissions = new List<PermissionDisplay>();

//            var roletemp = new RoleTemp { PermissionCode = role.PermissionCode };

//            var permissionsInRole = roletemp.PermissionsInRole.ToList();
//            foreach (PermissionDisplay permission in allPermissions)
//            {
//                if (permissionsInRole.Contains(permission.Permission))
//                    permissions.Add(permission);
//            }

//            return CommonResponse.Send(ResponseCodes.SUCCESS,permissions);
//        }

//        public async Task<ApiCommonResponse> UpdateRole(HttpContext context, long id, RoleReceivingDTO roleReceivingDto)
//        {
//            try
//            {
//                var role = await _roleRepo.FindRoleByName(roleReceivingDto.Name);
//                if (role == null)
//                {
//                    return CommonResponse.Send(ResponseCodes.FAILURE,null, $"Role {roleReceivingDto.Name} does not exists.");
//                }

//                List<Permissions> initialPermissions = new List<Permissions>();

//                foreach (var permission in roleReceivingDto.Permissions)
//                {
//                    Enum.TryParse<Permissions>(permission.Permission, out Permissions result);
//                    initialPermissions.Add(result);
//                }

//                var roletemp = new RoleTemp(roleReceivingDto.Name, roleReceivingDto.Description, initialPermissions);

//                //alter some properties of the role
//                role.PermissionCode = roletemp.PermissionCode;
//                role.Name = roleReceivingDto.Name;
//                role.Description = roleReceivingDto.Description;

//                await _context.SaveChangesAsync();

//                var roleTransferDto = _mapper.Map<RoleTransferDTO>(role);
//                return CommonResponse.Send(ResponseCodes.SUCCESS,roleTransferDto);
//            }
//            catch (Exception ex)
//            {
//                throw;
//            }
//        }

//        public async Task<ApiCommonResponse> GetAllRoles()
//        {
//            var roles = await _roleRepo.FindAllRole();
//            if (roles == null)
//            {
//                return CommonResponse.Send(ResponseCodes.NO_DATA_AVAILABLE);;
//            }           

//           var roleTransfer = _mapper.Map<IEnumerable<RoleTransferDTO>>(roles);
//            return CommonResponse.Send(ResponseCodes.SUCCESS,roleTransfer);
//        }

       
//        public async Task<ApiCommonResponse> GetAllClaims()
//        {
//            var claims = await _roleRepo.FindAllClaims();
//            if (claims == null)
//            {
//                return CommonResponse.Send(ResponseCodes.NO_DATA_AVAILABLE);;
//            }
//            var claimTransferDto = _mapper.Map<IEnumerable<ClaimTransferDTO>>(claims);
//            return CommonResponse.Send(ResponseCodes.SUCCESS,claimTransferDto);
//        }

//        public ApiCommonResponse GetPermissions()
//        {
//            var result =  PermissionDisplay.GetPermissionsToDisplay(typeof(Permissions));
//            return CommonResponse.Send(ResponseCodes.SUCCESS,result);

//        } 

//        public ApiCommonResponse GetGroupedPermissions()
//        {
//            var result = PermissionDisplay.GetPermissionsToDisplay(typeof(Permissions));

//            //group according the controller id
//            var grouped = from e in result
//                           group e by e.Controller into g
//                           select new
//                           {
//                               GroupName = g.Key,
//                               SplitName = SplitCamelCase(g.Key),
//                               PermissionList = g.Select(x => new PermissionDisplay
//                               (
//                                   x.Controller,
//                                    x.Action,
//                                   x.Description,
//                                   x.Permission

//                               )).ToList()
//                           };

//            return CommonResponse.Send(ResponseCodes.SUCCESS,grouped);

//        }

//        string SplitCamelCase(string source)
//        {
//            return string.Join(" ", Regex.Split(source, @"(?<!^)(?=[A-Z](?![A-Z]|$))"));
//        }

//        public async Task<ApiCommonResponse> GetUserRoleClaims(HttpContext context)
//        {
//            var userId = context.GetLoggedInUserId();
//            var user = await _userProfileRepo.FindUserById(userId);
//            if (user == null)
//            {
//                return CommonResponse.Send(ResponseCodes.FAILURE, null, "Some system errors occurred");
//            }

//            if (user.Role.Name == ClaimConstants.SuperAdmin)
//            {
//                var roleClaims = new List<RoleClaim>();
//                foreach (ClaimEnum item in Enum.GetValues(typeof(ClaimEnum)))
//                {
//                    roleClaims.Add(new RoleClaim
//                    {
//                        CanAdd = true,
//                        ClaimEnum = (int)item,
//                        CanDelete = true,
//                        CanUpdate = true,
//                        CanView = true,
//                        Description = item.ToString(),
//                        Name = item.ToString(),
//                        RoleId = user.Role.Id
//                    });
//                }
//                //user.Role.RoleClaims = roleClaims;
//            }

//            var userDto = _mapper.Map<UserProfileTransferDTO>(user);
//            return CommonResponse.Send(ResponseCodes.SUCCESS,userDto);
//        }

//        public async Task<ApiCommonResponse> GetRoleById(long id)
//        {
//            var role = await _roleRepo.FindRoleById(id);
//            if (role == null)
//            {
//                return CommonResponse.Send(ResponseCodes.NO_DATA_AVAILABLE);;
//            }
//            var roleTransferDtOs = _mapper.Map<RoleTransferDTO>(role);
//            return CommonResponse.Send(ResponseCodes.SUCCESS,roleTransferDtOs);
//        }

//        public async Task<ApiCommonResponse> GetRoleByName(string name)
//        {
//            var role = await _roleRepo.FindRoleByName(name);
//            if (role == null)
//            {
//                return CommonResponse.Send(ResponseCodes.NO_DATA_AVAILABLE);;
//            }
//            var roleTransferDtOs = _mapper.Map<RoleTransferDTO>(role);
//            return CommonResponse.Send(ResponseCodes.SUCCESS,roleTransferDtOs);
//        }

//        //public async Task<ApiCommonResponse> UpdateRole(HttpContext context, long id, RoleReceivingDTO roleReceivingDto)
//        //{
//        //    var roleToUpdate = await _roleRepo.FindRoleById(id);
//        //    if (roleToUpdate == null)
//        //    {
//        //        return CommonResponse.Send(ResponseCodes.NO_DATA_AVAILABLE);;
//        //    }

//        //    if(roleToUpdate.Name != roleReceivingDto.Name)
//        //    {
//        //        var item = await _roleRepo.FindRoleByName(roleReceivingDto.Name);
//        //        if (item != null)
//        //        {
//        //            return CommonResponse.Send(ResponseCodes.FAILURE,null, "Role Name Already Exists.");
//        //        }
//        //    }          

//        //    var summary = $"Initial details before change, \n {roleToUpdate} \n" ;

//        //    roleToUpdate.Name = roleReceivingDto.Name;
//        //    roleToUpdate.Description = roleReceivingDto.Description;

//        //    var updatedRole = await _roleRepo.UpdateRole(roleToUpdate);

//        //    if (updatedRole == null)
//        //    {
//        //        return CommonResponse.Send(ResponseCodes.FAILURE, null, "Some system errors occurred");
//        //    }

//        //    var roleClaimsDeleted = await _roleRepo.DeleteRoleClaims(updatedRole);
//        //    if (!roleClaimsDeleted)
//        //    {
//        //        return CommonResponse.Send(ResponseCodes.FAILURE, null, "Some system errors occurred");
//        //    }

//        //    //if (roleReceivingDto.RoleClaims.Any())
//        //    //{
//        //    //    var roleClaimsToSave = _mapper.Map<ICollection<RoleClaim>>(roleReceivingDto.RoleClaims);
//        //    //    foreach (var item in roleClaimsToSave)
//        //    //    {
//        //    //        item.RoleId = updatedRole.Id;
//        //    //    }
//        //    //    _context.RoleClaims.AddRange(roleClaimsToSave);
//        //    //}       

//        //    summary += $"Details after change, \n {updatedRole.ToString()} \n";
    
//        //    ModificationHistory history = new ModificationHistory(){
//        //        ModelChanged = "Role",
//        //        ChangeSummary = summary,
//        //        ChangedById = context.GetLoggedInUserId(),
//        //        ModifiedModelId = updatedRole.Id
//        //    };

//        //    await _historyRepo.SaveHistory(history);

//        //    var roleTransferDto = _mapper.Map<RoleTransferDTO>(await _roleRepo.FindRoleById(id));
//        //    return CommonResponse.Send(ResponseCodes.SUCCESS,roleTransferDto);

//        //}

//        public async Task<ApiCommonResponse> DeleteRole(long id)
//        {
//            var roleToDelete = await _roleRepo.FindRoleById(id);
//            if (roleToDelete == null)
//            {
//                return CommonResponse.Send(ResponseCodes.NO_DATA_AVAILABLE);;
//            }

//            if(roleToDelete.Name == ClaimConstants.SuperAdmin || roleToDelete.Name == ClaimConstants.UnAssigned)
//            {
//                return CommonResponse.Send(ResponseCodes.FAILURE);
//            }

//            var userProfiles = await _userProfileRepo.FindAllUserProfilesAttachedToRole(id);
//            if (userProfiles.Any())
//            {
//                var unAssignedRole = await _roleRepo.FindRoleByName(ClaimConstants.UnAssigned);
//                foreach (var userProfile in userProfiles)
//                {
//                    userProfile.RoleId = unAssignedRole.Id;
//                }

//                await _userProfileRepo.UpdateUserProfiles(userProfiles);
//            }


//            if (!await _roleRepo.DeleteRole(roleToDelete))
//            {
//                return CommonResponse.Send(ResponseCodes.FAILURE, null, "Some system errors occurred");
//            }

//            return CommonResponse.Send(ResponseCodes.SUCCESS);
//        }
//    }
//}