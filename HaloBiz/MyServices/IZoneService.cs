using System.Threading.Tasks;
using Halobiz.Common.DTOs.ApiDTOs;
using HaloBiz.DTOs.ReceivingDTOs;
using Microsoft.AspNetCore.Http;

namespace HaloBiz.MyServices
{
    public interface  IZoneService
    {
        Task<ApiCommonResponse> AddZone(HttpContext context, ZoneReceivingDTO serviceCategoryTaskReceivingDTO);
        Task<ApiCommonResponse> GetAllZones();
        Task<ApiCommonResponse> GetZoneById(long id);
        Task<ApiCommonResponse> GetZoneByName(string name);
        Task<ApiCommonResponse> UpdateZone(HttpContext context, long id, ZoneReceivingDTO serviceCategoryTaskReceivingDTO);
        Task<ApiCommonResponse> DeleteZone(long id);
    }
}