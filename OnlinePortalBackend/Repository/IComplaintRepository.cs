using Halobiz.Common.DTOs.ApiDTOs;
using Halobiz.Common.DTOs.TransferDTOs;
using OnlinePortalBackend.DTOs.TransferDTOs;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace OnlinePortalBackend.Repository
{
    public interface IComplaintRepository
    {
        Task<IEnumerable<ComplaintTypeDTO>> GetComplainTypes();
        Task<ApiCommonResponse> CreateComplaint(ComplaintDTO complaintReceivingDTO);
        Task<int> GetComplaintOrigin();
        Task<int> GetComplaintSource();


    }
}
