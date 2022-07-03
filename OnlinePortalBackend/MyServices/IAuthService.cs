using Halobiz.Common.DTOs.ApiDTOs;
using OnlinePortalBackend.DTOs.ReceivingDTOs;
using System.Threading.Tasks;

namespace OnlinePortalBackend.MyServices
{
    public interface IAuthService
    {
        Task<ApiCommonResponse> GenerateToken(string email, string password);
        Task<ApiCommonResponse> GenerateTokenFromRefreshToken(string email, string refreshToken);
    }
}
