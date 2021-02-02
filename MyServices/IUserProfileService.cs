using System.Threading.Tasks;
using HaloBiz.DTOs.ApiDTOs;
using HaloBiz.DTOs.ReceivingDTO;

namespace HaloBiz.MyServices
{
    public interface IUserProfileService
    {
        Task<ApiResponse> AddUserProfile(UserProfileReceivingDTO userProfileReceivingDTO);
        Task<ApiResponse> FindUserById(long id);
        Task<ApiResponse> FindUserByEmail(string email);
        Task<ApiResponse> FindAllUsers();
        Task<ApiResponse> UpdateUserProfile(long userId, UserProfileReceivingDTO userProfileReceivingDTO);
        Task<ApiResponse> DeleteUserProfile(long userId);
        Task<ApiResponse> UpdateUserRole(long userId, long roleId);
        Task<ApiResponse> AssignUserToSBU(long userId, long SBUId);

    }
}