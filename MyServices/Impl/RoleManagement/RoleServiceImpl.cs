using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using HaloBiz.Data;
using HaloBiz.DTOs.ApiDTOs;
using HaloBiz.DTOs.ReceivingDTOs.RoleManagement;
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
        private readonly DataContext _context;

        public RoleServiceImpl(
            IModificationHistoryRepository historyRepo, 
            IRoleRepository roleRepo, 
            DataContext dataContext,
            IMapper mapper)
        {
            this._mapper = mapper;
            this._historyRepo = historyRepo;
            this._context = dataContext;
            this._roleRepo = roleRepo;
        }

        public async Task<ApiResponse> AddRole(HttpContext context, RoleReceivingDTO roleReceivingDTO)
        {
            var item = await _roleRepo.FindRoleByName(roleReceivingDTO.Name);
            if (item != null)
            {
                return new ApiResponse(400, "Role Name Already Exists.");
            }

            var role = _mapper.Map<Role>(roleReceivingDTO);
            var savedRole = await _roleRepo.SaveRole(role);
            if (savedRole == null)
            {
                return new ApiResponse(500);
            }
            var roleTransferDTO = _mapper.Map<RoleTransferDTO>(role);
            return new ApiOkResponse(roleTransferDTO);
        }

        public async Task<ApiResponse> GetAllRoles()
        {
            var roles = await _roleRepo.FindAllRole();
            if (roles == null)
            {
                return new ApiResponse(404);
            }
            var roleTransferDTO = _mapper.Map<IEnumerable<RoleTransferDTO>>(roles);
            return new ApiOkResponse(roleTransferDTO);
        }

        public async Task<ApiResponse> GetAllClaims()
        {
            var claims = await _roleRepo.FindAllClaims();
            if (claims == null)
            {
                return new ApiResponse(404);
            }
            var claimTransferDTO = _mapper.Map<IEnumerable<ClaimTransferDTO>>(claims);
            return new ApiOkResponse(claimTransferDTO);
        }

        public async Task<ApiResponse> GetRoleById(long id)
        {
            var role = await _roleRepo.FindRoleById(id);
            if (role == null)
            {
                return new ApiResponse(404);
            }
            var roleTransferDTOs = _mapper.Map<RoleTransferDTO>(role);
            return new ApiOkResponse(roleTransferDTOs);
        }

        public async Task<ApiResponse> GetRoleByName(string name)
        {
            var role = await _roleRepo.FindRoleByName(name);
            if (role == null)
            {
                return new ApiResponse(404);
            }
            var roleTransferDTOs = _mapper.Map<RoleTransferDTO>(role);
            return new ApiOkResponse(roleTransferDTOs);
        }

        public async Task<ApiResponse> UpdateRole(HttpContext context, long id, RoleReceivingDTO roleReceivingDTO)
        {
            var roleToUpdate = await _roleRepo.FindRoleById(id);
            if (roleToUpdate == null)
            {
                return new ApiResponse(404);
            }

            if(roleToUpdate.Name != roleReceivingDTO.Name)
            {
                var item = await _roleRepo.FindRoleByName(roleReceivingDTO.Name);
                if (item != null)
                {
                    return new ApiResponse(400, "Role Name Already Exists.");
                }
            }          

            var summary = $"Initial details before change, \n {roleToUpdate.ToString()} \n" ;

            roleToUpdate.Name = roleReceivingDTO.Name;
            roleToUpdate.Description = roleReceivingDTO.Description;

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
                       
            var roleClaimsToSave = _mapper.Map<ICollection<RoleClaim>>(roleReceivingDTO.RoleClaims);
            foreach (var item in roleClaimsToSave)
            {
                item.RoleId = updatedRole.Id;
            }
            _context.RoleClaims.AddRange(roleClaimsToSave);

            summary += $"Details after change, \n {updatedRole.ToString()} \n";
    
            ModificationHistory history = new ModificationHistory(){
                ModelChanged = "Role",
                ChangeSummary = summary,
                ChangedById = context.GetLoggedInUserId(),
                ModifiedModelId = updatedRole.Id
            };

            await _historyRepo.SaveHistory(history);

            var roleTransferDTO = _mapper.Map<RoleTransferDTO>(await _roleRepo.FindRoleById(id));
            return new ApiOkResponse(roleTransferDTO);

        }

        public async Task<ApiResponse> DeleteRole(long id)
        {
            var roleToDelete = await _roleRepo.FindRoleById(id);
            if (roleToDelete == null)
            {
                return new ApiResponse(404);
            }

            if(roleToDelete.Name == ClaimConstants.SUPER_ADMIN || roleToDelete.Name == ClaimConstants.UN_ASSIGNED)
            {
                return new ApiResponse(400);
            }

            if (!await _roleRepo.DeleteRole(roleToDelete))
            {
                return new ApiResponse(500);
            }

            return new ApiOkResponse(true);
        }
    }
}