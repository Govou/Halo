using System.Threading.Tasks;
using Halobiz.Common.DTOs.ApiDTOs;
using HaloBiz.DTOs.ReceivingDTOs;
using Microsoft.AspNetCore.Http;

namespace HaloBiz.MyServices
{
    public interface IIndustryService
    {
        Task<ApiCommonResponse> AddIndustry(HttpContext context, IndustryReceivingDTO IndustryReceivingDTO);
        Task<ApiCommonResponse> GetAllIndustry();
        Task<ApiCommonResponse> UpdateIndustry(HttpContext context, long id, IndustryReceivingDTO IndustryReceivingDTO);
        Task<ApiCommonResponse> DeleteIndustry(long id);
    }
}