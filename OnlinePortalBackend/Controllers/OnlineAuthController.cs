using AutoMapper;
using Halobiz.Common.DTOs.ApiDTOs;
using Halobiz.Common.DTOs.ReceivingDTO;
using Halobiz.Common.DTOs.ReceivingDTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using OnlinePortalBackend.Helpers;
using OnlinePortalBackend.MyServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OnlinePortalBackend.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class OnlineAuthController : ControllerBase
    {

       
        private readonly IOnlineAccounts _onlineAccounts;
        public OnlineAuthController(
            IOnlineAccounts onlineAccounts)
        {
            _onlineAccounts = onlineAccounts;
        }

        [AllowAnonymous]
        [HttpGet("SendCode")]
        public async Task<ApiCommonResponse> SendCode(string email)
        {
            return await _onlineAccounts.SendConfirmCodeToClient(email);
        }

        [AllowAnonymous]
        [HttpPost("CreatePassword")]
        public async Task<ApiCommonResponse> AllowAccountCreation(CreatePasswordDTO user)
        {
            return await _onlineAccounts.CreateAccount(user);
        }

        [AllowAnonymous]
        [HttpPost("Login")]
        public async Task<ApiCommonResponse> Login(LoginDTO login)
        {
            return await _onlineAccounts.Login(login);
        }

        [AllowAnonymous]
        [HttpPost("VerifyCode")]
        public async Task<ApiCommonResponse> VerifyCode(CodeVerifyModel model)
        {
            return await _onlineAccounts.VerifyCode(model);
        }     

    }
}
