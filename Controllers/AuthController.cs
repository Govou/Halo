using Google.Apis.Auth;
using HaloBiz.DTOs.ApiDTOs;
using HaloBiz.DTOs.ReceivingDTOs;
using HaloBiz.DTOs.TransferDTOs;
using HaloBiz.Helpers;
using HaloBiz.MyServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
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

        public AuthController(
            IUserProfileService userProfileService,
            IConfiguration config)
        {
            this._config = config;
            this.userProfileService = userProfileService;
        }

        [AllowAnonymous]
        [HttpPost("Login")]
        public async Task<ActionResult> Login(LoginReceivingDTO loginReceiving)
        {
            GoogleJsonWebSignature.Payload payload;

            try
            {
                payload = await GoogleJsonWebSignature.ValidateAsync(loginReceiving.IdToken);
            }
            catch (InvalidJwtException invalidJwtException)
            {
                return StatusCode(404, invalidJwtException.Message);
            }

            if (!payload.EmailVerified)
            {
                return StatusCode(404, "Email verification failed.");
            }

            var email = payload.Email;

            var response = await userProfileService.FindUserByEmail(email);
            if (response.StatusCode >= 400)
                return StatusCode(response.StatusCode, response);
            var user = ((ApiOkResponse)response).Result;
            var userProfile = (UserProfileTransferDTO)user;

            var jwtToken = GenerateToken(userProfile);
            return Ok(new UserAuthTransferDTO { Token = jwtToken, UserProfile = userProfile });
        }

        [AllowAnonymous]
        [HttpPost("CreateUser")]
        public async Task<ActionResult> CreateProfile(AuthUserProfileReceivingDTO authUserProfileReceivingDTO)
        {

            GoogleJsonWebSignature.Payload payload;         

            try
            {
                payload = await GoogleJsonWebSignature.ValidateAsync(authUserProfileReceivingDTO.IdToken);
            }
            catch (InvalidJwtException invalidJwtException)
            {
                return StatusCode(404, invalidJwtException.Message);
            }
                
            if (!payload.EmailVerified)
            {
                return StatusCode(404, "Email verification failed.");
            }

            var userProfileDTO = authUserProfileReceivingDTO.UserProfile;

            userProfileDTO.RoleId = userProfileDTO.IsSuperAdmin() ? 1 : 2;

            var response = await userProfileService.AddUserProfile(userProfileDTO);

            if (response.StatusCode >= 400)
                return StatusCode(response.StatusCode, response);

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

        private string GenerateToken(UserProfileTransferDTO userProfile)
        {        
            List<Claim> claims = new List<Claim>()
            {
                new Claim(ClaimTypes.NameIdentifier, userProfile.Id.ToString()),
                new Claim(ClaimTypes.Email, userProfile.Email),
                new Claim(ClaimTypes.Role, userProfile.Role.Name),
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
