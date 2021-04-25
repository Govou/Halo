using System.Threading.Tasks;
using HaloBiz.DTOs.ApiDTOs;
using HaloBiz.DTOs.ReceivingDTOs.LAMS;
using HaloBiz.Helpers;

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
        Task<ApiResponse> UpdateLeadStagesStatus(long leadId, LeadStages stage, LeadCaptureReceivingDTO leadCaptureReceivingDTO = null);
        Task<ApiResponse> ConvertLeadToClient(HttpContext context,long leadId);
        Task<ApiResponse> GetAllUnApprovedLeads();
        Task<ApiResponse> SetUpLeadForApproval(HttpContext httpContext, long id);
        Task<ApiResponse> ApproveQuoteService(HttpContext httpContext, long leadId, long quoteServiceId, long sequence);
        Task<ApiResponse> DisapproveQuoteService(HttpContext httpContext, long leadId, long quoteServiceId, long sequence);
    }
}