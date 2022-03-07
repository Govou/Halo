using System.Threading.Tasks;
using Halobiz.Common.DTOs.ApiDTOs;
using HaloBiz.DTOs.ReceivingDTOs;
using Microsoft.AspNetCore.Http;

namespace HaloBiz.MyServices.LAMS
{
    public interface IOtherLeadCaptureInfoService
    {
        Task<ApiCommonResponse> AddOtherLeadCaptureInfo(HttpContext context, OtherLeadCaptureInfoReceivingDTO otherLeadCaptureInfoReceivingDTO);
        Task<ApiCommonResponse> GetAllOtherLeadCaptureInfo();
        Task<ApiCommonResponse> GetOtherLeadCaptureInfoById(long id);
        Task<ApiCommonResponse> UpdateOtherLeadCaptureInfo(HttpContext context, long id, OtherLeadCaptureInfoReceivingDTO otherLeadCaptureInfoReceivingDTO);
        Task<ApiCommonResponse> DeleteOtherLeadCaptureInfo(long id);
        Task<ApiCommonResponse> GetOtherLeadCaptureInfoByLeadDivisionId(long leadDivisionId);

    }
}