using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using OnlinePortalBackend.DTOs.ApiDTOs;
using OnlinePortalBackend.DTOs.ReceivingDTOs;

namespace OnlinePortalBackend.MyServices
{
    public interface IServiceWishlistService
    {
        Task<ApiResponse> AddServiceWishlist(HttpContext context, ServiceWishlistReceivingDTO controlRoomAlertReceivingDTO);
        Task<ApiResponse> FindServiceWishlistById(long id);
        Task<ApiResponse> FindServiceWishlistsByProspectId(long prospectId);
        Task<ApiResponse> FindAllServiceWishlists();
        Task<ApiResponse> UpdateServiceWishlist(HttpContext context, long userId, ServiceWishlistReceivingDTO controlRoomAlertReceivingDTO);
        Task<ApiResponse> DeleteServiceWishlist(long userId);
    }
}