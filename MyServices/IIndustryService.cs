using System.Threading.Tasks;
using HaloBiz.DTOs.ApiDTOs;
using HaloBiz.DTOs.ReceivingDTOs;
using Microsoft.AspNetCore.Http;

namespace HaloBiz.MyServices
{
    public interface IIndustryService
    {
        Task<ApiResponse> AddIndustry(HttpContext context, IndustryReceivingDTO IndustryReceivingDTO);
        Task<ApiResponse> GetAllIndustry();
        Task<ApiResponse> UpdateIndustry(HttpContext context, long id, IndustryReceivingDTO IndustryReceivingDTO);
        Task<ApiResponse> DeleteIndustry(long id);
    }
}