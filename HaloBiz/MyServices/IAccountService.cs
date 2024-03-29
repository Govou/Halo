﻿using Halobiz.Common.DTOs.ApiDTOs;
using HaloBiz.DTOs;
using HaloBiz.DTOs.ReceivingDTOs;
using halobiz_backend.DTOs.ReceivingDTOs;
using HalobizMigrations.Models.Halobiz;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HaloBiz.MyServices
{
    public interface IAccountService
    {
        Task<ApiCommonResponse> AddAccount(HttpContext context, AccountReceivingDTO accountClassReceivingDTO);
        Task<ApiCommonResponse> GetAccountById(long id);
        Task<ApiCommonResponse> SearchForAccountDetails(AccountSearchDTO accountSearchDTO);
        Task<ApiCommonResponse> GetAccountByAlias(string alias);
        Task<ApiCommonResponse> GetAllAccounts();
        Task<ApiCommonResponse> UpdateAccount(long id, AccountReceivingDTO accountClassReceivingDTO);
        Task<ApiCommonResponse> DeleteAccount(long id);
        Task<ApiCommonResponse> GetAllTradeIncomeTaxAccounts();
        Task<ApiCommonResponse> GetCashBookAccounts();
        Task<ApiCommonResponse> GetServiceAccounts();
        Task<ApiCommonResponse> SaveServiceAccounts(ServiceAccountsDTO dto, HttpContext context);
        Task<ApiCommonResponse> GetAccountForService();
    }
}
