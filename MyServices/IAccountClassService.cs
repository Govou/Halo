using HaloBiz.DTOs.ApiDTOs;
using HaloBiz.DTOs.ReceivingDTOs;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HaloBiz.MyServices
{
    public interface IAccountClassService
    {
        Task<ApiCommonResponse> AddAccountClass(HttpContext context, AccountClassReceivingDTO accountClassReceivingDTO);
        Task<ApiCommonResponse> GetAccountClassById(long id);
        Task<ApiCommonResponse> GetAccountClassByCaption(string caption);
        Task<ApiCommonResponse> GetAllAccountClasses();
        Task<ApiCommonResponse> UpdateAccountClass(long id, AccountClassReceivingDTO accountClassReceivingDTO);
        Task<ApiCommonResponse> DeleteAccountClass(long id);
        Task<ApiCommonResponse> GetBreakdownOfAccountClass();
    }
}
