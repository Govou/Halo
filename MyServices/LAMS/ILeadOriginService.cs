using System.Threading.Tasks;
using HaloBiz.DTOs.ApiDTOs;
using HaloBiz.DTOs.ReceivingDTOs;
using Microsoft.AspNetCore.Http;

namespace HaloBiz.MyServices.LAMS
{
    public interface ILeadOriginService
    {
        Task<ApiResponse> AddLeadOrigin(HttpContext context, LeadOriginReceivingDTO leadOriginReceivingDTO);
        Task<ApiResponse> GetAllLeadOrigin();
        Task<ApiResponse> GetLeadOriginById(long id);
        Task<ApiResponse> GetLeadOriginByName(string name);
        Task<ApiResponse> UpdateLeadOrigin(HttpContext context, long id, LeadOriginReceivingDTO leadOriginReceivingDTO);
        Task<ApiResponse> DeleteLeadOrigin(long id);
    }
}