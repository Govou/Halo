using System.Threading.Tasks;
using HaloBiz.DTOs.ApiDTOs;
using HaloBiz.DTOs.ReceivingDTOs.LAMS;
using Microsoft.AspNetCore.Http;

namespace HaloBiz.MyServices.LAMS
{
    public interface ILeadContactService
    {
        Task<ApiResponse> AddLeadContact(HttpContext context, LeadContactReceivingDTO leadContactReceivingDTO);
        Task<ApiResponse> GetAllLeadContact();
        Task<ApiResponse> GetLeadContactById(long id);
        Task<ApiResponse> UpdateLeadContact(HttpContext context, long id, LeadContactReceivingDTO leadContactReceivingDTO);
    }
}