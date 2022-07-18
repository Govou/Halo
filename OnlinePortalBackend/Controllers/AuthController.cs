using Halobiz.Common.DTOs.ApiDTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OnlinePortalBackend.MyServices;
using System.Threading.Tasks;

namespace OnlinePortalBackend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [AllowAnonymous]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpGet("GenerateToken")]
        public async Task<ApiCommonResponse> GenerateToken(string email, string password)
        {
            return await _authService.GenerateToken(email, password);
        }

        [HttpGet("GenerateTokenFromRefreshToken")]
        public async Task<ApiCommonResponse> GenerateTokenFromRefreshToken(string refreshToken, string email)
        {
            return await _authService.GenerateTokenFromRefreshToken(email, refreshToken);
        }
    }
}
