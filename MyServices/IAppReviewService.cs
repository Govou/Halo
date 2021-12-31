using HaloBiz.DTOs.ApiDTOs;
using HaloBiz.DTOs.ReceivingDTOs;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HaloBiz.MyServices
{
    public interface IAppReviewService
    {
        Task<ApiResponse> AddAppReview(HttpContext context, AppReviewReceivingDTO appReviewReceivingDTO);
        Task<ApiResponse> GetAllAppReview();
        Task<ApiResponse> GetAppReviewById(long id);
        Task<ApiResponse> UpdateAppReview(HttpContext context, long id, AppReviewReceivingDTO appReviewReceivingDTO);
        Task<ApiResponse> DeleteAppReview(long id);
    }
}
