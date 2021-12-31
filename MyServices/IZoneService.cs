using System.Threading.Tasks;
using HaloBiz.DTOs.ApiDTOs;
using HaloBiz.DTOs.ReceivingDTOs;
using Microsoft.AspNetCore.Http;

namespace HaloBiz.MyServices
{
    public interface  IZoneService
    {
        Task<ApiResponse> AddZone(HttpContext context, ZoneReceivingDTO serviceCategoryTaskReceivingDTO);
        Task<ApiResponse> GetAllZones();
        Task<ApiResponse> GetZoneById(long id);
        Task<ApiResponse> GetZoneByName(string name);
        Task<ApiResponse> UpdateZone(HttpContext context, long id, ZoneReceivingDTO serviceCategoryTaskReceivingDTO);
        Task<ApiResponse> DeleteZone(long id);
    }
}