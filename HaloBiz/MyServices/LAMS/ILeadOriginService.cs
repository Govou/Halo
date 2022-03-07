using System.Threading.Tasks;
using Halobiz.Common.DTOs.ApiDTOs;
using HaloBiz.DTOs.ReceivingDTOs;
using Microsoft.AspNetCore.Http;

namespace HaloBiz.MyServices.LAMS
{
    public interface ILeadOriginService
    {
        Task<ApiCommonResponse> AddLeadOrigin(HttpContext context, LeadOriginReceivingDTO leadOriginReceivingDTO);
        Task<ApiCommonResponse> GetAllLeadOrigin();
        Task<ApiCommonResponse> GetLeadOriginById(long id);
        Task<ApiCommonResponse> GetLeadOriginByName(string name);
        Task<ApiCommonResponse> UpdateLeadOrigin(HttpContext context, long id, LeadOriginReceivingDTO leadOriginReceivingDTO);
        Task<ApiCommonResponse> DeleteLeadOrigin(long id);
    }
}