using System.Threading.Tasks;
using HaloBiz.DTOs.ApiDTOs;
using HaloBiz.DTOs.ReceivingDTOs;
using Microsoft.AspNetCore.Http;

namespace HaloBiz.MyServices
{
    public interface IProcessesRequiringApprovalService
    {
        Task<ApiResponse> AddProcessesRequiringApproval(HttpContext context, ProcessesRequiringApprovalReceivingDTO processesRequiringApprovalReceivingDTO);
        Task<ApiResponse> GetAllProcessesRequiringApproval();
        Task<ApiResponse> UpdateProcessesRequiringApproval(HttpContext context, long id, ProcessesRequiringApprovalReceivingDTO processesRequiringApprovalReceivingDTO);
        Task<ApiResponse> DeleteProcessesRequiringApproval(long id);
    }
}