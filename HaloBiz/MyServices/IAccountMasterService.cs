using Halobiz.Common.DTOs.ApiDTOs;
using HaloBiz.DTOs.ReceivingDTOs;
using halobiz_backend.DTOs.QueryParamsDTOs;
using halobiz_backend.DTOs.ReceivingDTOs;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HaloBiz.MyServices
{
    public interface IAccountMasterService
    {
        Task<ApiCommonResponse> AddAccountMaster(HttpContext context, AccountMasterReceivingDTO accountMasterReceivingDTO);
        Task<ApiCommonResponse> GetAccountMasterById(long id);
        Task<ApiCommonResponse> GetAllAccountMasters();
         Task<ApiCommonResponse> QueryAccountMasters(AccountMasterTransactionDateQueryParams query);
        Task<ApiCommonResponse> UpdateAccountMaster(long id, AccountMasterReceivingDTO accountMasterReceivingDTO);
        Task<ApiCommonResponse> DeleteAccountMaster(long id);
        Task<ApiCommonResponse> GetAllAccountMastersByTransactionId(string transactionId);
        Task<ApiCommonResponse> GetAllAccountMastersByCustomerIdAndContractYear(AccountMasterTransactionDateQueryParams searcDto);
        Task<ApiCommonResponse> GetAllAccountMastersByTransactionDate(AccountMasterTransactionDateQueryParams query);
        Task<ApiCommonResponse> PostPeriodicAccountMaster(HttpContext httpContext);
    }
}
