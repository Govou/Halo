using System.Threading.Tasks;
using HaloBiz.DTOs.ApiDTOs;
using HaloBiz.DTOs.ReceivingDTOs;
using Microsoft.AspNetCore.Http;

namespace HaloBiz.MyServices
{
    public interface IApproverLevelService
    {
        Task<ApiCommonResponse> AddApproverLevel(HttpContext context, ApproverLevelReceivingDTO approverLevelReceivingDTO);
        Task<ApiCommonResponse> GetAllApproverLevel();
        Task<ApiCommonResponse> UpdateApproverLevel(HttpContext context, long id, ApproverLevelReceivingDTO approverLevelReceivingDTO);
        Task<ApiCommonResponse> DeleteApproverLevel(long id);
    }
}