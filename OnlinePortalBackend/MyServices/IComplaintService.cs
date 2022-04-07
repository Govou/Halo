using Halobiz.Common.DTOs.ApiDTOs;
using Halobiz.Common.DTOs.TransferDTOs;
using System.Threading.Tasks;

namespace OnlinePortalBackend.MyServices
{
    public interface IComplaintService
    {
        Task<ApiCommonResponse> GetComplainType();
        Task<ApiCommonResponse> CreateComplaint(ComplaintDTO complaint);
    }
}
