using System.Threading.Tasks;
using HaloBiz.DTOs.ApiDTOs;
using HaloBiz.DTOs.ReceivingDTOs;
using Microsoft.AspNetCore.Http;

namespace HaloBiz.MyServices
{
    public interface IEngagementTypeService
    {
        Task<ApiCommonResponse> AddEngagementType(HttpContext context, EngagementTypeReceivingDTO engagementTypeReceivingDTO);
        Task<ApiCommonResponse> GetAllEngagementType();
        Task<ApiCommonResponse> UpdateEngagementType(HttpContext context, long id, EngagementTypeReceivingDTO engagementTypeReceivingDTO);
        Task<ApiCommonResponse> DeleteEngagementType(long id);
    }
}