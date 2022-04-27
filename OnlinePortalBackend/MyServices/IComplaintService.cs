using Halobiz.Common.DTOs.ApiDTOs;
using Halobiz.Common.DTOs.TransferDTOs;
using OnlinePortalBackend.DTOs.TransferDTOs;
using System.Threading.Tasks;

namespace OnlinePortalBackend.MyServices
{
    public interface IComplaintService
    {
        Task<ApiCommonResponse> GetComplainType();
        Task<ApiCommonResponse> CreateComplaint(ComplaintDTO complaint);
        Task<ApiCommonResponse> TrackComplaint(ComplaintTrackingDTO model);
        Task<ApiCommonResponse> GetAllComplaints(int userId);
        Task<ApiCommonResponse> GetResolvedComplaintsPercentage(int userId); 
    }
}
