using System.Threading.Tasks;
using HaloBiz.DTOs.ApiDTOs;
using HaloBiz.DTOs.ReceivingDTOs.LAMS;
using Microsoft.AspNetCore.Http;

namespace HaloBiz.MyServices.LAMS
{
    public interface ILeadDivisionKeyPersonService
    {
        Task<ApiResponse> AddLeadDivisionKeyPerson(HttpContext context, LeadDivisionKeyPersonReceivingDTO LeadDivisionKeyPersonReceivingDTO);
        Task<ApiResponse> GetAllLeadDivisionKeyPerson();
        Task<ApiResponse> GetLeadDivisionKeyPersonById(long id);
        Task<ApiResponse> GetAllLeadDivisionKeyPersonsByLeadDivisionId(long id);
        Task<ApiResponse> UpdateLeadDivisionKeyPerson(HttpContext context, long id, LeadDivisionKeyPersonReceivingDTO LeadDivisionKeyPersonReceivingDTO);
        Task<ApiResponse> DeleteKeyPerson(long Id);
    }
}