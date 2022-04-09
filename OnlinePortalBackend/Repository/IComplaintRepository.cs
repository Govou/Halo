using OnlinePortalBackend.DTOs.TransferDTOs;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace OnlinePortalBackend.Repository
{
    public interface IComplaintRepository
    {
        Task<IEnumerable<ComplaintTypeDTO>> GetComplainTypes();
        Task<int> CreateComplaint();
        Task<int> GetComplaintOrigin();
        Task<int> GetComplaintSource();
    }
}
