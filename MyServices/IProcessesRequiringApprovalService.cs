using System.Threading.Tasks;
using HaloBiz.DTOs.ApiDTOs;
using HaloBiz.DTOs.ReceivingDTOs;
using Microsoft.AspNetCore.Http;

namespace HaloBiz.MyServices
{
    public interface IProcessesRequiringApprovalService
    {
        Task<ApiCommonResponse> AddProcessesRequiringApproval(HttpContext context, ProcessesRequiringApprovalReceivingDTO processesRequiringApprovalReceivingDTO);
        Task<ApiCommonResponse> GetAllProcessesRequiringApproval();
        Task<ApiCommonResponse> UpdateProcessesRequiringApproval(HttpContext context, long id, ProcessesRequiringApprovalReceivingDTO processesRequiringApprovalReceivingDTO);
        Task<ApiCommonResponse> DeleteProcessesRequiringApproval(long id);
    }
}