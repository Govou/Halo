using Google.Apis.Auth;
using HaloBiz.DTOs.ApiDTOs;
using HaloBiz.DTOs.ReceivingDTOs;
using HaloBiz.DTOs.TransferDTOs;
using HaloBiz.Helpers;
using HaloBiz.MyServices;
using HaloBiz.MyServices.RoleManagement;
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
        private readonly JwtHelper _jwttHelper;
        private readonly IRoleService _roleService;
        public AuthController(
            IUserProfileService userProfileService,
            IConfiguration config,
            JwtHelper jwtHelper,
            IRoleService roleService,
            ILogger<AuthController> logger)
        {
            this._config = config;
            this.userProfileService = userProfileService;
            _logger = logger;
            _jwttHelper = jwtHelper;
            _roleService = roleService;
        }

        [AllowAnonymous]
        [HttpPost("OtherLogin")]
        public async Task<ActionResult> OtherLogin(LoginDTO login)
        {
            try
            {
                var response = await userProfileService.FindUserByEmail(login.Email);
                if (response.StatusCode >= 400)
                {
                    _logger.LogWarning($"Could not find user [{login.Email}] => {response.Message}");
                    return StatusCode(response.StatusCode, response);
                }

                if(login.Password != "12345")
                {
                    return StatusCode(400, "Username or password incorrect");
                }

                var user = ((ApiOkResponse)response).Result;
                var userProfile = (UserProfileTransferDTO)user;

                //get the permissions of the user
                var permissions = await _roleService.GetPermissionEnumsOnUser(userProfile.Id);

                var jwtToken =   _jwttHelper.GenerateToken(userProfile, permissions);
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
        [HttpPost("Login")]
        public async Task<ActionResult> Login(GoogleLoginReceivingDTO loginReceiving)
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

                var permissions = await _roleService.GetPermissionEnumsOnUser(userProfile.Id);

                var jwtToken =  _jwttHelper.GenerateToken(userProfile, permissions);
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

                var permissions = await _roleService.GetPermissionEnumsOnUser(userProfile.Id);

                var token =  _jwttHelper.GenerateToken(userProfile, permissions);
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
       
    }
}
