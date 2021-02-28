using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using HaloBiz.Data;
using HaloBiz.DTOs.ApiDTOs;
using HaloBiz.DTOs.ReceivingDTOs.RoleManagement;
using HaloBiz.DTOs.TransferDTOs;
using HaloBiz.DTOs.TransferDTOs.RoleManagement;
using HaloBiz.Helpers;
using HaloBiz.Model;
using HaloBiz.Model.RoleManagement;
using HaloBiz.MyServices.RoleManagement;
using HaloBiz.Repository;
using HaloBiz.Repository.RoleManagement;
using Microsoft.AspNetCore.Http;

namespace HaloBiz.MyServices.Impl.RoleManagement
{
    public class RoleServiceImpl : IRoleService
    {
        private readonly IModificationHistoryRepository _historyRepo;
        private readonly IMapper _mapper;
        private readonly IRoleRepository _roleRepo;
        private readonly IUserProfileRepository _userProfileRepo;
        private readonly DataContext _context;

        public RoleServiceImpl(
            IModificationHistoryRepository historyRepo,
            IRoleRepository roleRepo,
            IUserProfileRepository userProfileRepo,
            DataContext dataContext,
            IMapper mapper)
        {
            _mapper = mapper;
            _historyRepo = historyRepo;
            _context = dataContext;
            _roleRepo = roleRepo;
            _userProfileRepo = userProfileRepo;
        }

        public async Task<ApiResponse> AddRole(HttpContext context, RoleReceivingDTO roleReceivingDto)
        {
            var item = await _roleRepo.FindRoleByName(roleReceivingDto.Name);
            if (item != null)
            {
                return new ApiResponse(400, "Role Name Already Exists.");
            }

            var role = _mapper.Map<Role>(roleReceivingDto);
            var savedRole = await _roleRepo.SaveRole(role);
            if (savedRole == null)
            {
                return new ApiResponse(500);
            }
            var roleTransferDto = _mapper.Map<RoleTransferDTO>(role);
            return new ApiOkResponse(roleTransferDto);
        }

        public async Task<ApiResponse> GetAllRoles()
        {
            var roles = await _roleRepo.FindAllRole();
            if (roles == null)
            {
                return new ApiResponse(404);
            }
            var roleTransferDto = _mapper.Map<IEnumerable<RoleTransferDTO>>(roles);
            return new ApiOkResponse(roleTransferDto);
        }

        public async Task<ApiResponse> GetAllClaims()
        {
            var claims = await _roleRepo.FindAllClaims();
            if (claims == null)
            {
                return new ApiResponse(404);
            }
            var claimTransferDto = _mapper.Map<IEnumerable<ClaimTransferDTO>>(claims);
            return new ApiOkResponse(claimTransferDto);
        }

        public async Task<ApiResponse> GetUserRoleClaims(HttpContext context)
        {
            var userId = context.GetLoggedInUserId();
            var user = await _userProfileRepo.FindUserById(userId);
            if (user == null)
            {
                return new ApiResponse(500);
            }

            if (user.Role.Name == ClaimConstants.SuperAdmin)
            {
                var roleClaims = new List<RoleClaim>();
                foreach (ClaimEnum item in Enum.GetValues(typeof(ClaimEnum)))
                {
                    roleClaims.Add(new RoleClaim
                    {
                        CanAdd = true,
                        ClaimEnum = item,
                        CanDelete = true,
                        CanUpdate = true,
                        CanView = true,
                        Description = item.ToString(),
                        Name = item.ToString(),
                        RoleId = user.Role.Id
                    });
                }
                user.Role.RoleClaims = roleClaims;
            }

            var userDto = _mapper.Map<UserProfileTransferDTO>(user);
            return new ApiOkResponse(userDto);
        }

        public async Task<ApiResponse> GetRoleById(long id)
        {
            var role = await _roleRepo.FindRoleById(id);
            if (role == null)
            {
                return new ApiResponse(404);
            }
            var roleTransferDtOs = _mapper.Map<RoleTransferDTO>(role);
            return new ApiOkResponse(roleTransferDtOs);
        }

        public async Task<ApiResponse> GetRoleByName(string name)
        {
            var role = await _roleRepo.FindRoleByName(name);
            if (role == null)
            {
                return new ApiResponse(404);
            }
            var roleTransferDtOs = _mapper.Map<RoleTransferDTO>(role);
            return new ApiOkResponse(roleTransferDtOs);
        }

        public async Task<ApiResponse> UpdateRole(HttpContext context, long id, RoleReceivingDTO roleReceivingDto)
        {
            var roleToUpdate = await _roleRepo.FindRoleById(id);
            if (roleToUpdate == null)
            {
                return new ApiResponse(404);
            }

            if(roleToUpdate.Name != roleReceivingDto.Name)
            {
                var item = await _roleRepo.FindRoleByName(roleReceivingDto.Name);
                if (item != null)
                {
                    return new ApiResponse(400, "Role Name Already Exists.");
                }
            }          

            var summary = $"Initial details before change, \n {roleToUpdate} \n" ;

            roleToUpdate.Name = roleReceivingDto.Name;
            roleToUpdate.Description = roleReceivingDto.Description;

            var updatedRole = await _roleRepo.UpdateRole(roleToUpdate);

            if (updatedRole == null)
            {
                return new ApiResponse(500);
            }

            var roleClaimsDeleted = await _roleRepo.DeleteRoleClaims(updatedRole);
            if (!roleClaimsDeleted)
            {
                return new ApiResponse(500);
            }

            if (roleReceivingDto.RoleClaims.Any())
            {
                var roleClaimsToSave = _mapper.Map<ICollection<RoleClaim>>(roleReceivingDto.RoleClaims);
                foreach (var item in roleClaimsToSave)
                {
                    item.RoleId = updatedRole.Id;
                }
                _context.RoleClaims.AddRange(roleClaimsToSave);
            }       

            summary += $"Details after change, \n {updatedRole.ToString()} \n";
    
            ModificationHistory history = new ModificationHistory(){
                ModelChanged = "Role",
                ChangeSummary = summary,
                ChangedById = context.GetLoggedInUserId(),
                ModifiedModelId = updatedRole.Id
            };

            await _historyRepo.SaveHistory(history);

            var roleTransferDto = _mapper.Map<RoleTransferDTO>(await _roleRepo.FindRoleById(id));
            return new ApiOkResponse(roleTransferDto);

        }

        public async Task<ApiResponse> DeleteRole(long id)
        {
            var roleToDelete = await _roleRepo.FindRoleById(id);
            if (roleToDelete == null)
            {
                return new ApiResponse(404);
            }

            if(roleToDelete.Name == ClaimConstants.SuperAdmin || roleToDelete.Name == ClaimConstants.UnAssigned)
            {
                return new ApiResponse(400);
            }

            var userProfiles = await _userProfileRepo.FindAllUserProfilesAttachedToRole(id);
            if (userProfiles.Any())
            {
                var unAssignedRole = await _roleRepo.FindRoleByName(ClaimConstants.UnAssigned);
                foreach (var userProfile in userProfiles)
                {
                    userProfile.RoleId = unAssignedRole.Id;
                }

                await _userProfileRepo.UpdateUserProfiles(userProfiles);
            }


            if (!await _roleRepo.DeleteRole(roleToDelete))
            {
                return new ApiResponse(500);
            }

            return new ApiOkResponse(true);
        }
    }
}