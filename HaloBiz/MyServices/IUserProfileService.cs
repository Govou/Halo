using System.Collections.Generic;
using System.Threading.Tasks;
using Halobiz.Common.DTOs.ApiDTOs;
using Halobiz.Common.DTOs.ReceivingDTOs.RoleManagement;
using HaloBiz.DTOs.ReceivingDTO;
using HaloBiz.DTOs.ReceivingDTOs.RoleManagement;
using Microsoft.AspNetCore.Http;

namespace HaloBiz.MyServices
{
    public interface IUserProfileService
    {
        Task<ApiCommonResponse> AddUserProfile(UserProfileReceivingDTO userProfileReceivingDTO);
        Task<ApiCommonResponse> FindUserById(long id);
        Task<ApiCommonResponse> FindUserByEmail(string email);
        Task<ApiCommonResponse> FindAllUsers();
        Task<ApiCommonResponse> FindAllUsersNotInAnSBU(long sbuId);
        Task<ApiCommonResponse> UpdateUserProfile(long userId, UserProfileReceivingDTO userProfileReceivingDTO);
        Task<ApiCommonResponse> DeleteUserProfile(long userId);
        Task<ApiCommonResponse> UpdateUserRole(HttpContext context, long userId, List<RoleReceivingDTO> roles);
        Task<ApiCommonResponse> AssignUserToSBU(long userId, long SBUId);
        Task<ApiCommonResponse> DetachUserFromSBU(long id);
        Task<ApiCommonResponse> FetchAllUserProfilesWithEscalationLevelConfiguration();
    }
}