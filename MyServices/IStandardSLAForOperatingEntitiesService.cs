using System.Threading.Tasks;
using HaloBiz.DTOs.ApiDTOs;
using HaloBiz.DTOs.ReceivingDTOs;
using Microsoft.AspNetCore.Http;

namespace HaloBiz.MyServices
{
    public interface IStandardSlaforOperatingEntityService
    {
        Task<ApiResponse> AddStandardSlaforOperatingEntity(HttpContext context, StandardSlaforOperatingEntityReceivingDTO standardSLAForOperatingEntitiesReceivingDTO);
        Task<ApiResponse> GetAllStandardSlaforOperatingEntity();
        Task<ApiResponse> GetStandardSlaforOperatingEntityById(long id);
        Task<ApiResponse> GetStandardSlaforOperatingEntityByName(string name);
        Task<ApiResponse> UpdateStandardSlaforOperatingEntity(HttpContext context, long id, StandardSlaforOperatingEntityReceivingDTO standardSLAForOperatingEntitiesReceivingDTO);
        Task<ApiResponse> DeleteStandardSlaforOperatingEntity(long id);
    }
}