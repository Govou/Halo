using System.Threading.Tasks;
using HaloBiz.DTOs.ApiDTOs;
using HaloBiz.DTOs.ReceivingDTOs;
using Microsoft.AspNetCore.Http;

namespace HaloBiz.MyServices
{
    public interface IStandardSLAForOperatingEntitiesService
    {
        Task<ApiResponse> AddStandardSLAForOperatingEntities(HttpContext context, StandardSLAForOperatingEntitiesReceivingDTO standardSLAForOperatingEntitiesReceivingDTO);
        Task<ApiResponse> GetAllStandardSLAForOperatingEntities();
        Task<ApiResponse> GetStandardSLAForOperatingEntitiesById(long id);
        Task<ApiResponse> GetStandardSLAForOperatingEntitiesByName(string name);
        Task<ApiResponse> UpdateStandardSLAForOperatingEntities(HttpContext context, long id, StandardSLAForOperatingEntitiesReceivingDTO standardSLAForOperatingEntitiesReceivingDTO);
        Task<ApiResponse> DeleteStandardSLAForOperatingEntities(long id);
    }
}