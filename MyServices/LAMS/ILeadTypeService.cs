using System.Threading.Tasks;
using HaloBiz.DTOs.ApiDTOs;
using HaloBiz.DTOs.ReceivingDTOs;
using Microsoft.AspNetCore.Http;

namespace HaloBiz.MyServices.LAMS
{
    public interface ILeadTypeService
    {
        Task<ApiCommonResponse> AddLeadType(HttpContext context, LeadTypeReceivingDTO leadTypeReceivingDTO);
        Task<ApiCommonResponse> GetAllLeadType();
        Task<ApiCommonResponse> GetLeadTypeById(long id);
        Task<ApiCommonResponse> GetLeadTypeByName(string name);
        Task<ApiCommonResponse> UpdateLeadType(HttpContext context, long id, LeadTypeReceivingDTO leadTypeReceivingDTO);
        Task<ApiCommonResponse> DeleteLeadType(long id);

    }
}