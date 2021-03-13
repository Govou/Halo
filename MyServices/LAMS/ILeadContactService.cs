using System.Threading.Tasks;
using HaloBiz.DTOs.ApiDTOs;
using HaloBiz.DTOs.ReceivingDTOs.LAMS;
using HaloBiz.Model.LAMS;
using Microsoft.AspNetCore.Http;

namespace HaloBiz.MyServices.LAMS
{
    public interface ILeadContactService
    {
        Task<ApiResponse> AddLeadContact(HttpContext context, long leadId, LeadContactReceivingDTO leadContactReceivingDTO);
        Task<ApiResponse> GetAllLeadContact();
        Task<ApiResponse> GetLeadContactById(long id);
        Task<ApiResponse> UpdateLeadContact(HttpContext context, long id, LeadContactReceivingDTO leadContactReceivingDTO);
        Task<ApiResponse> DeleteLeadContact(long id);
    }
}