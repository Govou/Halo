using Google.Apis.Auth;
using HaloBiz.DTOs.ApiDTOs;
using HaloBiz.DTOs.ReceivingDTOs;
using HaloBiz.DTOs.TransferDTOs;
using HaloBiz.Helpers;
using HaloBiz.MyServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;


namespace HaloBiz.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {

        private readonly IUserProfileService userProfileService;
        private readonly IConfiguration _config;
        private readonly ILogger<AuthController> _logger;

        public AuthController(
            IUserProfileService userProfileService,
            IConfiguration config,
            ILogger<AuthController> logger)
        {
            this._config = config;
            this.userProfileService = userProfileService;
            _logger = logger;
        }

        [AllowAnonymous]
        [HttpPost("Login")]
        public async Task<ActionResult> Login(LoginReceivingDTO loginReceiving)
        {
            try
            {
                GoogleJsonWebSignature.Payload payload;

                try
                {
                    payload = await GoogleJsonWebSignature.ValidateAsync(loginReceiving.IdToken);
                }
                catch (InvalidJwtException invalidJwtException)
                {
                    _logger.LogWarning($"Could not validate Google Id Token [{loginReceiving.IdToken}] => {invalidJwtException.Message}");
                    return StatusCode(404, invalidJwtException.Message);
                }

                if (!payload.EmailVerified)
                {
                    _logger.LogWarning($"Email verification failed. Payload => {JsonConvert.SerializeObject(payload)}");
                    return StatusCode(404, "Email verification failed.");
                }

                var email = payload.Email;

                var response = await userProfileService.FindUserByEmail(email);
                if (response.StatusCode >= 400)
                {
                    _logger.LogWarning($"Could not find user [{email}] => {response.Message}");
                    return StatusCode(response.StatusCode, response);
                }
                    
                var user = ((ApiOkResponse)response).Result;
                var userProfile = (UserProfileTransferDTO)user;

                var jwtToken = GenerateToken(userProfile);
                return Ok(new UserAuthTransferDTO { Token = jwtToken, UserProfile = userProfile });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                _logger.LogError(ex.StackTrace);
                return StatusCode(500, $"An error occured => {ex.Message}");
            }
        }

        [AllowAnonymous]
        [HttpPost("CreateUser")]
        public async Task<ActionResult> CreateProfile(AuthUserProfileReceivingDTO authUserProfileReceivingDTO)
        {
            try
            {
                GoogleJsonWebSignature.Payload payload;

                try
                {
                    payload = await GoogleJsonWebSignature.ValidateAsync(authUserProfileReceivingDTO.IdToken);
                }
                catch (InvalidJwtException invalidJwtException)
                {
                    _logger.LogWarning(JsonConvert.SerializeObject(authUserProfileReceivingDTO));
                    _logger.LogWarning($"Could not validate Google Id Token [{authUserProfileReceivingDTO.IdToken}] => {invalidJwtException.Message}");
                    return StatusCode(404, invalidJwtException.Message);
                }

                if (!payload.EmailVerified)
                {
                    _logger.LogWarning($"Email verification failed. Payload => {JsonConvert.SerializeObject(payload)}");
                    return StatusCode(404, "Email verification failed.");
                }

                var userProfileDTO = authUserProfileReceivingDTO.UserProfile;

                userProfileDTO.RoleId = userProfileDTO.IsSuperAdmin() ? 1 : 2;

                var response = await userProfileService.AddUserProfile(userProfileDTO);

                if (response.StatusCode >= 400)
                {
                    _logger.LogWarning($"Could not create user [{userProfileDTO.Email}] => {response.Message}");
                    return StatusCode(response.StatusCode, response);
                }

                var user = ((ApiOkResponse)response).Result;
                var userProfile = (UserProfileTransferDTO)user;

                var token = GenerateToken(userProfile);
                UserAuthTransferDTO userAuthTransferDTO = new UserAuthTransferDTO()
                {
                    Token = token,
                    UserProfile = userProfile
                };

                return Ok(userAuthTransferDTO);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                _logger.LogError(ex.StackTrace);
                return StatusCode(500, $"An error occured => {ex.Message}");
            }
        }

        private string GenerateToken(UserProfileTransferDTO userProfile)
        {
            List<Claim> claims = new List<Claim>()
            {
                new Claim(ClaimTypes.NameIdentifier, userProfile.Id.ToString()),
                new Claim(ClaimTypes.Email, userProfile.Email),
                new Claim(ClaimTypes.Role, userProfile.Role?.Name ?? string.Empty),
                new Claim("RoleId", userProfile.RoleId.ToString())
            };

            var secret = _config["JWTSecretKey"] ?? _config.GetSection("AppSettings:JWTSecretKey").Value;

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secret));

            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.Now.AddDays(1),
                SigningCredentials = credentials
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }
}
