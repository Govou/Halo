using System.Threading.Tasks;
using HaloBiz.DTOs.ApiDTOs;
using HaloBiz.DTOs.ReceivingDTOs;
using Microsoft.AspNetCore.Http;

namespace HaloBiz.MyServices.LAMS
{
    public interface ILeadDivisionService
    {
        Task<ApiResponse> AddLeadDivision(HttpContext context, LeadDivisionReceivingDTO leadDivisionReceivingDTO);
        Task<ApiResponse> GetAllLeadDivision();
        Task<ApiResponse> GetLeadDivisionById(long id);
        Task<ApiResponse> GetLeadDivisionByName(string name);
        Task<ApiResponse> GetLeadDivisionByRCNumber(string rcNumber);
        Task<ApiResponse> UpdateLeadDivision(HttpContext context, long id, LeadDivisionReceivingDTO leadDivisionReceivingDTO);
        Task<ApiResponse> DeleteLeadDivision(long id);

    }
}