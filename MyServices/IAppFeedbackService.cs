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
        Task<ApiResponse> AddAppFeedback(HttpContext context, AppFeedbackReceivingDTO serviceTypeReceivingDTO);
        Task<ApiResponse> GetAllAppFeedback();
        Task<ApiResponse> GetAppFeedbackById(long id);
        Task<ApiResponse> UpdateAppFeedback(HttpContext context, long id, AppFeedbackReceivingDTO serviceTypeReceivingDTO);
        Task<ApiResponse> DeleteAppFeedback(long id);
    }
}
