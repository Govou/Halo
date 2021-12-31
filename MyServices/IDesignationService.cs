using System.Threading.Tasks;
using HaloBiz.DTOs.ApiDTOs;
using HaloBiz.DTOs.ReceivingDTOs;
using Microsoft.AspNetCore.Http;

namespace HaloBiz.MyServices
{
    public interface IDesignationService
    {
        Task<ApiResponse> AddDesignation(HttpContext context, DesignationReceivingDTO designationReceivingDTO);
        Task<ApiResponse> GetAllDesignation();
        Task<ApiResponse> UpdateDesignation(HttpContext context, long id, DesignationReceivingDTO designationReceivingDTO);
        Task<ApiResponse> DeleteDesignation(long id);
    }
}