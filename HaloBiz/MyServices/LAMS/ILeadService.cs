using System.Threading.Tasks;
using Halobiz.Common.DTOs.ApiDTOs;
using HaloBiz.DTOs.ReceivingDTOs.LAMS;
using HaloBiz.Helpers;

using Microsoft.AspNetCore.Http;

namespace HaloBiz.MyServices.LAMS
{
    public interface ILeadService
    {
        Task<ApiCommonResponse> AddLead(HttpContext context, LeadReceivingDTO leadReceivingDTO);
        Task<ApiCommonResponse> GetAllLead();
        Task<ApiCommonResponse> GetUserLeads(HttpContext context);
        Task<ApiCommonResponse> GetLeadById(long id);
        Task<ApiCommonResponse> DropLead(HttpContext context, long id, DropLeadReceivingDTO dropLeadReceivingDTO);
        Task<ApiCommonResponse> GetLeadByReferenceNumber(string refNumber);
        Task<ApiCommonResponse> UpdateLead(HttpContext context, long id, LeadReceivingDTO leadReceivingDTO);
        Task<ApiCommonResponse> UpdateLeadStagesStatus(long leadId, LeadStages stage, LeadCaptureReceivingDTO leadCaptureReceivingDTO = null);
        Task<ApiCommonResponse> ConvertLeadToClient(HttpContext context,long leadId);
        Task<ApiCommonResponse> GetAllUnApprovedLeads();
        Task<ApiCommonResponse> SetUpLeadForApproval(HttpContext httpContext, long id);
        Task<ApiCommonResponse> ApproveQuoteService(HttpContext httpContext, long leadId, long quoteServiceId, long sequence);
        Task<ApiCommonResponse> DisapproveQuoteService(HttpContext httpContext, long leadId, long quoteServiceId, long sequence);
    }
}