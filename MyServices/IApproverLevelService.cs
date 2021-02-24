using System.Threading.Tasks;
using HaloBiz.DTOs.ApiDTOs;
using HaloBiz.DTOs.ReceivingDTOs;
using Microsoft.AspNetCore.Http;

namespace HaloBiz.MyServices
{
    public interface IApproverLevelService
    {
        Task<ApiResponse> AddApproverLevel(HttpContext context, ApproverLevelReceivingDTO approverLevelReceivingDTO);
        Task<ApiResponse> GetAllApproverLevel();
        Task<ApiResponse> UpdateApproverLevel(HttpContext context, long id, ApproverLevelReceivingDTO approverLevelReceivingDTO);
        Task<ApiResponse> DeleteApproverLevel(long id);
    }
}