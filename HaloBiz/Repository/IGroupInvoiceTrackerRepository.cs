using Halobiz.Common.DTOs.ApiDTOs;
using System.Threading.Tasks;

namespace HaloBiz.Repository
{
    public interface IGroupInvoiceTrackerRepository
    {
        Task<ApiCommonResponse> GenerateGroupInvoiceNumber();
    }
}