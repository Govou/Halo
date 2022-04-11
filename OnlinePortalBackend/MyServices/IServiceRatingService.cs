using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using OnlinePortalBackend.DTOs.ApiDTOs;
using OnlinePortalBackend.DTOs.ReceivingDTOs;

namespace OnlinePortalBackend.MyServices
{
    public interface IServiceRatingService
    {
        Task<ApiResponse> AddServiceRating(HttpContext context, ServiceRatingReceivingDTO controlRoomAlertReceivingDTO);
        Task<ApiResponse> FindServiceRatingById(long id);
        Task<ApiResponse> GetReviewHistoryByServiceId(long contractServiceId);
        Task<ApiResponse> FindAllServiceRatings();
        Task<ApiResponse> GetMyServiceRatings(HttpContext context);
       // Task<ApiResponse> UpdateServiceRating(HttpContext context, long userId, ServiceRatingReceivingDTO controlRoomAlertReceivingDTO);
       // Task<ApiResponse> DeleteServiceRating(long userId);
    }
}