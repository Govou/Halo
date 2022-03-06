using System.Threading.Tasks;
using Halobiz.Common.DTOs.ApiDTOs;
using HaloBiz.DTOs.ReceivingDTOs;
using Microsoft.AspNetCore.Http;

namespace HaloBiz.MyServices
{
    public interface  IRegionService
    {
        Task<ApiCommonResponse> AddRegion(HttpContext context, RegionReceivingDTO serviceCategoryTaskReceivingDTO);
        Task<ApiCommonResponse> GetAllRegions();
        Task<ApiCommonResponse> GetRegionById(long id);
        Task<ApiCommonResponse> GetRegionByName(string name);
        Task<ApiCommonResponse> UpdateRegion(HttpContext context, long id, RegionReceivingDTO serviceCategoryTaskReceivingDTO);
        Task<ApiCommonResponse> DeleteRegion(long id);
    }
}