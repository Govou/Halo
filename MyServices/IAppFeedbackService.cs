using HaloBiz.DTOs.ApiDTOs;
using HaloBiz.DTOs.ReceivingDTOs;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HaloBiz.MyServices
{
    public interface IAppFeedbackService
    {
        Task<ApiCommonResponse> AddAppFeedback(HttpContext context, AppFeedbackReceivingDTO serviceTypeReceivingDTO);
        Task<ApiCommonResponse> GetAllAppFeedback();
        Task<ApiCommonResponse> GetAppFeedbackById(long id);
        Task<ApiCommonResponse> UpdateAppFeedback(HttpContext context, long id, AppFeedbackReceivingDTO serviceTypeReceivingDTO);
        Task<ApiCommonResponse> DeleteAppFeedback(long id);
    }
}
