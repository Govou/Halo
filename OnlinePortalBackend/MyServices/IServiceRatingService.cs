using System.Threading.Tasks;
using Halobiz.Common.DTOs.ApiDTOs;
using Microsoft.AspNetCore.Http;
using OnlinePortalBackend.DTOs.ApiDTOs;
using OnlinePortalBackend.DTOs.ReceivingDTOs;

namespace OnlinePortalBackend.MyServices
{
    public interface IServiceRatingService
    {
        Task<ApiCommonResponse> AddServiceRating(ServiceRatingReceivingDTO serviceRating);
        Task<ApiCommonResponse> AddAppRating(AppRatingReceivingDTO appRating);
        Task<ApiResponse> FindServiceRatingById(long id);
        Task<ApiResponse> GetReviewHistoryByServiceId(long contractServiceId);
        Task<ApiResponse> FindAllServiceRatings();
        Task<ApiResponse> FindAllAppRatings(int appId);
        Task<ApiResponse> FindAllApplications();
        Task<ApiResponse> GetMyServiceRatings(HttpContext context);
       // Task<ApiResponse> UpdateServiceRating(HttpContext context, long userId, ServiceRatingReceivingDTO controlRoomAlertReceivingDTO);
       // Task<ApiResponse> DeleteServiceRating(long userId);
    }
}