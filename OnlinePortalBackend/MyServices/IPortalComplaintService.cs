using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using OnlinePortalBackend.DTOs.ApiDTOs;
using OnlinePortalBackend.DTOs.ReceivingDTOs;

namespace OnlinePortalBackend.MyServices
{
    public interface IPortalComplaintService
    {
        Task<ApiResponse> AddPortalComplaint(HttpContext context, PortalComplaintReceivingDTO controlRoomAlertReceivingDTO);
        Task<ApiResponse> FindPortalComplaintById(long id);
        Task<ApiResponse> FindAllPortalComplaints();
        Task<ApiResponse> UpdatePortalComplaint(HttpContext context, long userId, PortalComplaintReceivingDTO controlRoomAlertReceivingDTO);
        Task<ApiResponse> DeletePortalComplaint(long userId);
    }
}