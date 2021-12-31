using System.Threading.Tasks;
using HaloBiz.DTOs.ApiDTOs;
using HaloBiz.DTOs.ReceivingDTOs;
using Microsoft.AspNetCore.Http;

namespace HaloBiz.MyServices
{
    public interface  IRegionService
    {
        Task<ApiResponse> AddRegion(HttpContext context, RegionReceivingDTO serviceCategoryTaskReceivingDTO);
        Task<ApiResponse> GetAllRegions();
        Task<ApiResponse> GetRegionById(long id);
        Task<ApiResponse> GetRegionByName(string name);
        Task<ApiResponse> UpdateRegion(HttpContext context, long id, RegionReceivingDTO serviceCategoryTaskReceivingDTO);
        Task<ApiResponse> DeleteRegion(long id);
    }
}