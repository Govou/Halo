﻿using Halobiz.Common.DTOs.ApiDTOs;
using Halobiz.Common.DTOs.ReceivingDTOs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OnlinePortalBackend.DTOs.ReceivingDTOs;
using OnlinePortalBackend.MyServices.SecureMobilitySales;
using System.Threading.Tasks;

namespace OnlinePortalBackend.Controllers.SecureMobilitySales
{
    [Route("api/[controller]")]
    [ApiController]
    public class SMSAccountController : ControllerBase
    {
        private readonly ISMSAccountService _accountService;
        public SMSAccountController(ISMSAccountService accountService)
        {
            _accountService = accountService;
        }

        [HttpPost("CreateIndividualAccount")]
        public Task<ApiCommonResponse> CreateIndividualAccount(SMSIndividualAccountDTO request)
        {
           return _accountService.CreateIndividualAccount(request);
        }

        [HttpPost("CreateBusinessAccount")]
        public Task<ApiCommonResponse> CreateBusinessAccount(SMSBusinessAccountDTO request)
        {
            return _accountService.CreateBusinessAccount(request);
        }

        [HttpPost("CreateSupplierAccount")]
        public Task<ApiCommonResponse> CreateSupplierAccount(SMSSupplierAccountDTO request)
        {
            return _accountService.CreateSupplierAccount(request);
        }

        [HttpGet("GetCustomerProfile")]
        public Task<ApiCommonResponse> GetCustomerProfile(int profileId)
        {
            return _accountService.GetCustomerProfile(profileId);
        }
    }
}
