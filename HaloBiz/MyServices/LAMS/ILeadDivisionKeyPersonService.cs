using System.Threading.Tasks;
using Halobiz.Common.DTOs.ApiDTOs;
using HaloBiz.DTOs.ReceivingDTOs.LAMS;
using Microsoft.AspNetCore.Http;

namespace HaloBiz.MyServices.LAMS
{
    public interface ILeadDivisionKeyPersonService
    {
        Task<ApiCommonResponse> AddLeadDivisionKeyPerson(HttpContext context, LeadDivisionKeyPersonReceivingDTO LeadDivisionKeyPersonReceivingDTO);
        Task<ApiCommonResponse> GetAllLeadDivisionKeyPerson();
        Task<ApiCommonResponse> GetLeadDivisionKeyPersonById(long id);
        Task<ApiCommonResponse> GetAllLeadDivisionKeyPersonsByLeadDivisionId(long id);
        Task<ApiCommonResponse> UpdateLeadDivisionKeyPerson(HttpContext context, long id, LeadDivisionKeyPersonReceivingDTO LeadDivisionKeyPersonReceivingDTO);
        Task<ApiCommonResponse> DeleteKeyPerson(long Id);
    }
}