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
        Task<ApiCommonResponse> AddAccountDetail(HttpContext context, AccountDetailReceivingDTO accountDetailReceivingDTO);
        Task<ApiCommonResponse> GetAccountDetailById(long id);
        Task<ApiCommonResponse> GetAllAccountDetails();
        Task<ApiCommonResponse> UpdateAccountDetail(long id, AccountDetailReceivingDTO accountDetailReceivingDTO);
        Task<ApiCommonResponse> DeleteAccountDetail(long id);
    }
}
