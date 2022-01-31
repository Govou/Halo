using System.Threading.Tasks;
using HaloBiz.DTOs.ApiDTOs;
using HaloBiz.DTOs.ReceivingDTOs;
using Microsoft.AspNetCore.Http;

namespace HaloBiz.MyServices
{
    public interface IStandardSlaforOperatingEntityService
    {
        Task<ApiCommonResponse> AddStandardSlaforOperatingEntity(HttpContext context, StandardSlaforOperatingEntityReceivingDTO standardSLAForOperatingEntitiesReceivingDTO);
        Task<ApiCommonResponse> GetAllStandardSlaforOperatingEntity();
        Task<ApiCommonResponse> GetStandardSlaforOperatingEntityById(long id);
        Task<ApiCommonResponse> GetStandardSlaforOperatingEntityByName(string name);
        Task<ApiCommonResponse> UpdateStandardSlaforOperatingEntity(HttpContext context, long id, StandardSlaforOperatingEntityReceivingDTO standardSLAForOperatingEntitiesReceivingDTO);
        Task<ApiCommonResponse> DeleteStandardSlaforOperatingEntity(long id);
    }
}