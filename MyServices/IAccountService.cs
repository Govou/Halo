using HaloBiz.DTOs.ApiDTOs;
using HaloBiz.DTOs.ReceivingDTOs;
using halobiz_backend.DTOs.ReceivingDTOs;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HaloBiz.MyServices
{
    public interface IAccountService
    {
        Task<ApiResponse> AddAccount(HttpContext context, AccountReceivingDTO accountClassReceivingDTO);
        Task<ApiResponse> GetAccountById(long id);
        Task<ApiResponse> SearchForAccountDetails(AccountSearchDTO accountSearchDTO);
        Task<ApiResponse> GetAccountByAlias(string alias);
        Task<ApiResponse> GetAllAccounts();
        Task<ApiResponse> UpdateAccount(long id, AccountReceivingDTO accountClassReceivingDTO);
        Task<ApiResponse> DeleteAccount(long id);
        Task<ApiResponse> GetAllTradeIncomeTaxAccounts();
    }
}
