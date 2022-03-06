using System.Threading.Tasks;
using Halobiz.Common.DTOs.ApiDTOs;
using HaloBiz.DTOs.ReceivingDTOs.LAMS;

using Microsoft.AspNetCore.Http;

namespace HaloBiz.MyServices.LAMS
{
    public interface ILeadKeyPersonService
    {
        Task<ApiCommonResponse> AddLeadKeyPerson(HttpContext context, LeadKeyPersonReceivingDTO leadKeyPersonReceivingDTO);
        Task<ApiCommonResponse> GetAllLeadKeyPerson();
        Task<ApiCommonResponse> GetLeadKeyPersonById(long id);
        Task<ApiCommonResponse> UpdateLeadKeyPerson(HttpContext context, long id, LeadKeyPersonReceivingDTO leadKeyPersonReceivingDTO);
        Task<ApiCommonResponse> DeleteKeyPerson(long Id);
        Task<ApiCommonResponse> GetAllLeadKeyPersonsByLeadId(long leadId);
    }
}