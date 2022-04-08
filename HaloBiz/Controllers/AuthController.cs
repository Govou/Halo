using AutoMapper;
using Google.Apis.Auth;
using Halobiz.Common.Auths;
using Halobiz.Common.DTOs.ApiDTOs;
using Halobiz.Common.DTOs.ReceivingDTO;
using Halobiz.Common.DTOs.ReceivingDTOs;
using Halobiz.Common.DTOs.TransferDTOs;
using Halobiz.Common.MyServices;
using Halobiz.Common.MyServices.RoleManagement;
using HaloBiz.DTOs.ReceivingDTOs;
using HaloBiz.DTOs.TransferDTOs;
using HaloBiz.Helpers;
using HaloBiz.MyServices;
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
    [ModuleName(HalobizModules.Setups)]

    public class AuthController : ControllerBase
    {

        private readonly IUserAuthentication _userAuthentication;
        public AuthController(IUserAuthentication userAuthentication)
        {
            _userAuthentication = userAuthentication;           
        }

        [AllowAnonymous]
        [HttpPost("Login")]
        public async Task<ApiCommonResponse> Login(LoginDTO login)
        {
            return await _userAuthentication.Login(login);
        }

        [AllowAnonymous]
        [HttpPost("GoogleLogin")]
        public async Task<ApiCommonResponse> Login(GoogleLoginReceivingDTO loginReceiving)
        {
            return await _userAuthentication.GoogleLogin(loginReceiving);
        }

        [AllowAnonymous]
        [HttpPost("CreateUser")]
        public async Task<ApiCommonResponse> CreateProfile(AuthUserProfileReceivingDTO authUserProfileReceivingDTO)
        {
            return await _userAuthentication.CreateProfile(authUserProfileReceivingDTO);
        }

        [HttpPost("RevokeToken")]
        public async Task<ApiCommonResponse> RevokeToken(RefreshTokenDTO model)
        {
            return await _userAuthentication.RevokeToken(model);
        }

        [HttpPut("UpdatePassword")]
        public async Task<ApiCommonResponse> UpdatePassord(UpdatePassworddDTO request)
        {
            return await _userAuthentication.UpdatePassword(request);
        }

        [HttpPost("CreatePassword")]
        public async Task<ApiCommonResponse> CreatePassword(CreatePasswordDTO request)
        {
            return await _userAuthentication.CreatePassword(request);
        }
    }
}
