using System.Threading.Tasks;
using HaloBiz.DTOs.ApiDTOs;
using HaloBiz.DTOs.ReceivingDTOs;
using Microsoft.AspNetCore.Http;

namespace HaloBiz.MyServices.LAMS
{
    public interface ILeadTypeService
    {
        Task<ApiResponse> AddLeadType(HttpContext context, LeadTypeReceivingDTO leadTypeReceivingDTO);
        Task<ApiResponse> GetAllLeadType();
        Task<ApiResponse> GetLeadTypeById(long id);
        Task<ApiResponse> GetLeadTypeByName(string name);
        Task<ApiResponse> UpdateLeadType(HttpContext context, long id, LeadTypeReceivingDTO leadTypeReceivingDTO);
        Task<ApiResponse> DeleteLeadType(long id);

    }
}