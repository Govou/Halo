using System.Collections.Generic;
using System.Threading.Tasks;
using HaloBiz.DTOs.ApiDTOs;
using HaloBiz.DTOs.ReceivingDTO;
using HaloBiz.DTOs.ReceivingDTOs.RoleManagement;
using Microsoft.AspNetCore.Http;

namespace HaloBiz.MyServices
{
    public interface IUserProfileService
    {
        Task<ApiResponse> AddUserProfile(UserProfileReceivingDTO userProfileReceivingDTO);
        Task<ApiResponse> FindUserById(long id);
        Task<ApiResponse> FindUserByEmail(string email);
        Task<ApiResponse> FindAllUsers();
        Task<ApiResponse> FindAllUsersNotInAnSBU(long sbuId);
        Task<ApiResponse> UpdateUserProfile(long userId, UserProfileReceivingDTO userProfileReceivingDTO);
        Task<ApiResponse> DeleteUserProfile(long userId);
        Task<ApiResponse> UpdateUserRole(HttpContext context, long userId, List<RoleReceivingDTO> roles);
        Task<ApiResponse> AssignUserToSBU(long userId, long SBUId);
        Task<ApiResponse> DetachUserFromSBU(long id);
        Task<ApiResponse> FetchAllUserProfilesWithEscalationLevelConfiguration();
    }
}