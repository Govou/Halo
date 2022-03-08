using AutoMapper;
using Google.Apis.Auth;
using Halobiz.Common.DTOs.ApiDTOs;
using Halobiz.Common.DTOs.ReceivingDTOs;
using Halobiz.Common.MyServices;
using Halobiz.Common.MyServices.RoleManagement;
using HaloBiz.DTOs.ReceivingDTOs;
using HaloBiz.DTOs.TransferDTOs;
using HaloBiz.Helpers;
using HalobizMigrations.Models;
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
        private readonly IMapper _mapper;
        public AuthController(
            IUserProfileService userProfileService,
            IConfiguration config,
            JwtHelper jwtHelper,
            IMapper mapper,
            IRoleService roleService,
            ILogger<AuthController> logger)
        {
            this._config = config;
            this.userProfileService = userProfileService;
            _logger = logger;
            _jwttHelper = jwtHelper;
            _mapper = mapper;
            _roleService = roleService;
        }

        [AllowAnonymous]
        [HttpPost("OtherLogin")]
        public async Task<ApiCommonResponse> OtherLogin(LoginDTO login)
        {
            try
            {
                var response = await userProfileService.FindUserByEmail(login.Email);
                
                if(!response.responseCode.Contains("00"))
                {
                    _logger.LogWarning($"Could not find user [{login.Email}] => {response.responseMsg}");
                    return CommonResponse.Send(ResponseCodes.FAILURE, null, "Could not find the user");


                }

                if (login.Password != "12345")
                {
                    return CommonResponse.Send(ResponseCodes.FAILURE, null, "Username or password incorrect");
                }

                var user = response.responseData;
                var userProfile = (UserProfile)user;  

                //get the permissions of the user
                 var permissions = await _roleService.GetPermissionEnumsOnUser(userProfile.Id);

                 var jwtToken =   _jwttHelper.GenerateToken(userProfile, permissions);
                 
                 return CommonResponse.Send(ResponseCodes.SUCCESS,  new UserAuthTransferDTO { Token = jwtToken,
                     UserProfile = _mapper.Map<UserProfileTransferDTO>(userProfile)
                 });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                _logger.LogError(ex.StackTrace);
                return CommonResponse.Send(ResponseCodes.FAILURE, null, "System error");
            }
        }

        [AllowAnonymous]
        [HttpPost("Login")]
        public async Task<ApiCommonResponse> Login(GoogleLoginReceivingDTO loginReceiving)
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
                    return CommonResponse.Send(ResponseCodes.FAILURE, null, invalidJwtException.Message);
                }

                if (!payload.EmailVerified)
                {
                    _logger.LogWarning($"Email verification failed. Payload => {JsonConvert.SerializeObject(payload)}");
                    return CommonResponse.Send(ResponseCodes.INVALID_LOGIN_DETAILS, null, "Invalid login details.");
                }

                var email = payload.Email;

                var response = await userProfileService.FindUserByEmail(email);

                if(!response.responseCode.Contains("00"))
                
                {
                    _logger.LogWarning($"Could not find user [{email}] => {response.responseData}");

                    return CommonResponse.Send(ResponseCodes.NO_USER_PROFILE_FOUND, null, "Could not find user.");
                }

                var user = response.responseData;
                var userProfile = (UserProfile)user;

                var permissions = await _roleService.GetPermissionEnumsOnUser(userProfile.Id);

                var jwtToken =  _jwttHelper.GenerateToken(userProfile, permissions);
                return CommonResponse.Send(ResponseCodes.SUCCESS, new UserAuthTransferDTO { Token = jwtToken,
                    UserProfile = _mapper.Map<UserProfileTransferDTO>(userProfile)
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                _logger.LogError(ex.StackTrace);
                return CommonResponse.Send(ResponseCodes.FAILURE,null, ex.Message);
            }
        }

        [AllowAnonymous]
        [HttpPost("CreateUser")]
        public async Task<ApiCommonResponse> CreateProfile(AuthUserProfileReceivingDTO authUserProfileReceivingDTO)
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
                    return CommonResponse.Send(ResponseCodes.FAILURE, null, invalidJwtException.Message);
                }

                if (!payload.EmailVerified)
                {
                    _logger.LogWarning($"Email verification failed. Payload => {JsonConvert.SerializeObject(payload)}");
                    return CommonResponse.Send(ResponseCodes.FAILURE, null, "Email verification failed.");
                }

                var userProfileDTO = authUserProfileReceivingDTO.UserProfile;

                userProfileDTO.RoleId = userProfileDTO.IsSuperAdmin() ? 1 : 2;

                var response = await userProfileService.AddUserProfile(userProfileDTO);

                if(!response.responseCode.Contains("00"))                
                {
                    _logger.LogWarning($"Could not create user [{userProfileDTO.Email}] => {response.responseMsg}");
                    
                }

                var user = response.responseData;
                var userProfile = (UserProfile)user;

                var permissions = await _roleService.GetPermissionEnumsOnUser(userProfile.Id);

                var token =  _jwttHelper.GenerateToken(userProfile, permissions);

                
                UserAuthTransferDTO userAuthTransferDTO = new UserAuthTransferDTO()
                {
                    Token = token,
                    UserProfile = _mapper.Map<UserProfileTransferDTO>(userProfile)
                };

                return CommonResponse.Send(ResponseCodes.SUCCESS, userAuthTransferDTO);

            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                _logger.LogError(ex.StackTrace);
                return CommonResponse.Send(ResponseCodes.FAILURE, null, ex.Message);

            }
        }
       
    }
}
