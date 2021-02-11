using HaloBiz.DTOs.ApiDTOs;
using HaloBiz.DTOs.ReceivingDTOs;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HaloBiz.MyServices
{
    public interface IAccountDetailService
    {
        Task<ApiResponse> AddAccountDetail(HttpContext context, AccountDetailReceivingDTO accountDetailReceivingDTO);
        Task<ApiResponse> GetAccountDetailById(long id);
        Task<ApiResponse> GetAllAccountDetails();
        Task<ApiResponse> UpdateAccountDetail(long id, AccountDetailReceivingDTO accountDetailReceivingDTO);
        Task<ApiResponse> DeleteAccountDetail(long id);
    }
}
