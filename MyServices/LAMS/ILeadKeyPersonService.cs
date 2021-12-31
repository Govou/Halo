using System.Threading.Tasks;
using HaloBiz.DTOs.ApiDTOs;
using HaloBiz.DTOs.ReceivingDTOs.LAMS;

using Microsoft.AspNetCore.Http;

namespace HaloBiz.MyServices.LAMS
{
    public interface ILeadKeyPersonService
    {
        Task<ApiResponse> AddLeadKeyPerson(HttpContext context, LeadKeyPersonReceivingDTO leadKeyPersonReceivingDTO);
        Task<ApiResponse> GetAllLeadKeyPerson();
        Task<ApiResponse> GetLeadKeyPersonById(long id);
        Task<ApiResponse> UpdateLeadKeyPerson(HttpContext context, long id, LeadKeyPersonReceivingDTO leadKeyPersonReceivingDTO);
        Task<ApiResponse> DeleteKeyPerson(long Id);
        Task<ApiResponse> GetAllLeadKeyPersonsByLeadId(long leadId);
    }
}