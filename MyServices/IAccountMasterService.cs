using HaloBiz.DTOs.ApiDTOs;
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
        Task<ApiResponse> AddAccountMaster(HttpContext context, AccountMasterReceivingDTO accountMasterReceivingDTO);
        Task<ApiResponse> GetAccountMasterById(long id);
        Task<ApiResponse> GetAllAccountMasters();
         Task<ApiResponse> QueryAccountMasters(AccountMasterTransactionDateQueryParams query);
        Task<ApiResponse> UpdateAccountMaster(long id, AccountMasterReceivingDTO accountMasterReceivingDTO);
        Task<ApiResponse> DeleteAccountMaster(long id);
        Task<ApiResponse> GetAllAccountMastersByTransactionId(string transactionId);
        Task<ApiResponse> GetAllAccountMastersByCustomerIdAndContractYear(AccountMasterTransactionDateQueryParams searcDto);
        Task<ApiResponse> GetAllAccountMastersByTransactionDate(AccountMasterTransactionDateQueryParams query);
    }
}
