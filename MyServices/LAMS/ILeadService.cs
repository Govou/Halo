using System.Threading.Tasks;
using HaloBiz.DTOs.ApiDTOs;
using HaloBiz.DTOs.ReceivingDTOs.LAMS;
using HaloBiz.Helpers;
using HaloBiz.Model.LAMS;
using Microsoft.AspNetCore.Http;

namespace HaloBiz.MyServices.LAMS
{
    public interface ILeadService
    {
        Task<ApiResponse> AddLead(HttpContext context, LeadReceivingDTO leadReceivingDTO);
        Task<ApiResponse> GetAllLead();
        Task<ApiResponse> GetLeadById(long id);
        Task<ApiResponse> DropLead(HttpContext context, long id, DropLeadReceivingDTO dropLeadReceivingDTO);
        Task<ApiResponse> GetLeadByReferenceNumber(string refNumber);
        Task<ApiResponse> UpdateLead(HttpContext context, long id, LeadReceivingDTO leadReceivingDTO);
        Task<ApiResponse> UpdateLeadStagesStatus(long leadId, LeadStages stage);
        Task<ApiResponse> ConvertLeadToClient(HttpContext context,long leadId, bool shouldGenerateId);
    }
}