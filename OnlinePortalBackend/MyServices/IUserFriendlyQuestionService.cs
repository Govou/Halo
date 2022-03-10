using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using OnlinePortalBackend.DTOs.ApiDTOs;
using OnlinePortalBackend.DTOs.ReceivingDTOs;

namespace OnlinePortalBackend.MyServices
{
    public interface IUserFriendlyQuestionService
    {
        Task<ApiResponse> AddUserFriendlyQuestion(HttpContext context, UserFriendlyQuestionReceivingDTO userFriendlyQuestionReceivingDTO);
        Task<ApiResponse> FindUserFriendlyQuestionById(long id);
        Task<ApiResponse> FindAllUserFriendlyQuestions();
        Task<ApiResponse> UpdateUserFriendlyQuestion(HttpContext context, long userId, UserFriendlyQuestionReceivingDTO userFriendlyQuestionReceivingDTO);
        Task<ApiResponse> DeleteUserFriendlyQuestion(long userId);
    }
}