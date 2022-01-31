using System.Threading.Tasks;
using HaloBiz.DTOs.ApiDTOs;
using HaloBiz.DTOs.ReceivingDTOs;
using Microsoft.AspNetCore.Http;

namespace HaloBiz.MyServices.LAMS
{
    public interface ILeadDivisionService
    {
        Task<ApiCommonResponse> AddLeadDivision(HttpContext context, LeadDivisionReceivingDTO leadDivisionReceivingDTO);
        Task<ApiCommonResponse> GetAllLeadDivision();
        Task<ApiCommonResponse> GetLeadDivisionById(long id);
        Task<ApiCommonResponse> GetLeadDivisionByName(string name);
        Task<ApiCommonResponse> GetLeadDivisionByRCNumber(string rcNumber);
        Task<ApiCommonResponse> UpdateLeadDivision(HttpContext context, long id, LeadDivisionReceivingDTO leadDivisionReceivingDTO);
        Task<ApiCommonResponse> DeleteLeadDivision(long id);

    }
}