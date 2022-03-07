using System.Threading.Tasks;
using Halobiz.Common.DTOs.ApiDTOs;
using HaloBiz.DTOs.ReceivingDTOs;
using Microsoft.AspNetCore.Http;

namespace HaloBiz.MyServices
{
    public interface IDesignationService
    {
        Task<ApiCommonResponse> AddDesignation(HttpContext context, DesignationReceivingDTO designationReceivingDTO);
        Task<ApiCommonResponse> GetAllDesignation();
        Task<ApiCommonResponse> UpdateDesignation(HttpContext context, long id, DesignationReceivingDTO designationReceivingDTO);
        Task<ApiCommonResponse> DeleteDesignation(long id);
    }
}