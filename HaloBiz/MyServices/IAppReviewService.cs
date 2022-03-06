using Halobiz.Common.DTOs.ApiDTOs;
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
        Task<ApiCommonResponse> AddAppReview(HttpContext context, AppReviewReceivingDTO appReviewReceivingDTO);
        Task<ApiCommonResponse> GetAllAppReview();
        Task<ApiCommonResponse> GetAppReviewById(long id);
        Task<ApiCommonResponse> UpdateAppReview(HttpContext context, long id, AppReviewReceivingDTO appReviewReceivingDTO);
        Task<ApiCommonResponse> DeleteAppReview(long id);
    }
}
