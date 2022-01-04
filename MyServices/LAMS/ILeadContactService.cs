using System.Threading.Tasks;
using HaloBiz.DTOs.ApiDTOs;
using HaloBiz.DTOs.ReceivingDTOs.LAMS;

using Microsoft.AspNetCore.Http;

namespace HaloBiz.MyServices.LAMS
{
    public interface ILeadContactService
    {
        Task<ApiCommonResponse> AddLeadContact(HttpContext context, long leadId, LeadContactReceivingDTO leadContactReceivingDTO);
        Task<ApiCommonResponse> GetAllLeadContact();
        Task<ApiCommonResponse> GetLeadContactById(long id);
        Task<ApiCommonResponse> UpdateLeadContact(HttpContext context, long id, LeadContactReceivingDTO leadContactReceivingDTO);
        Task<ApiCommonResponse> DeleteLeadContact(long id);
    }
}