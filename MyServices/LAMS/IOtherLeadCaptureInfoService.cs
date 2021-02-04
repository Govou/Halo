using System.Threading.Tasks;
using HaloBiz.DTOs.ApiDTOs;
using HaloBiz.DTOs.ReceivingDTOs;
using Microsoft.AspNetCore.Http;

namespace HaloBiz.MyServices.LAMS
{
    public interface IOtherLeadCaptureInfoService
    {
        Task<ApiResponse> AddOtherLeadCaptureInfo(HttpContext context, OtherLeadCaptureInfoReceivingDTO otherLeadCaptureInfoReceivingDTO);
        Task<ApiResponse> GetAllOtherLeadCaptureInfo();
        Task<ApiResponse> GetOtherLeadCaptureInfoById(long id);
        Task<ApiResponse> UpdateOtherLeadCaptureInfo(HttpContext context, long id, OtherLeadCaptureInfoReceivingDTO otherLeadCaptureInfoReceivingDTO);
        Task<ApiResponse> DeleteOtherLeadCaptureInfo(long id);

    }
}