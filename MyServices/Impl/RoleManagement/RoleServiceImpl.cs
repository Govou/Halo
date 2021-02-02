using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
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

        public RoleServiceImpl(
            IModificationHistoryRepository historyRepo, 
            IRoleRepository roleRepo, 
            IMapper mapper)
        {
            this._mapper = mapper;
            this._historyRepo = historyRepo;
            this._roleRepo = roleRepo;
        }

        public async Task<ApiResponse> AddRole(HttpContext context, RoleReceivingDTO roleReceivingDTO)
        {
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
            
            var summary = $"Initial details before change, \n {roleToUpdate.ToString()} \n" ;

            roleToUpdate.Name = roleReceivingDTO.Name;
            roleToUpdate.Description = roleReceivingDTO.Description;

            var roleClaimsDeleted = await _roleRepo.DeleteRoleClaims(roleToUpdate);
            if (!roleClaimsDeleted)
            {
                return new ApiResponse(500);
            }

            var roleClaimsToSave = _mapper.Map<ICollection<RoleClaim>>(roleReceivingDTO.RoleClaims);

            roleToUpdate.RoleClaims = roleClaimsToSave;

            var updatedRole = await _roleRepo.UpdateRole(roleToUpdate);

            summary += $"Details after change, \n {updatedRole.ToString()} \n";

            if (updatedRole == null)
            {
                return new ApiResponse(500);
            }
            ModificationHistory history = new ModificationHistory(){
                ModelChanged = "Role",
                ChangeSummary = summary,
                ChangedById = context.GetLoggedInUserId(),
                ModifiedModelId = updatedRole.Id
            };

            await _historyRepo.SaveHistory(history);

            var roleTransferDTO = _mapper.Map<RoleTransferDTO>(updatedRole);
            return new ApiOkResponse(roleTransferDTO);

        }

        public async Task<ApiResponse> DeleteRole(long id)
        {
            var roleToDelete = await _roleRepo.FindRoleById(id);
            if (roleToDelete == null)
            {
                return new ApiResponse(404);
            }

            if (!await _roleRepo.DeleteRole(roleToDelete))
            {
                return new ApiResponse(500);
            }

            return new ApiOkResponse(true);
        }
    }
}