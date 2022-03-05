using System.Threading.Tasks;
using HaloBiz.DTOs.ApiDTOs;
using HaloBiz.DTOs.ReceivingDTOs;
using Microsoft.AspNetCore.Http;

namespace HaloBiz.MyServices.LAMS
{
    public interface IDropReasonService
    {
        Task<ApiCommonResponse> AddDropReason(HttpContext context, DropReasonReceivingDTO DropReasonReceivingDTO);
        Task<ApiCommonResponse> GetAllDropReason();
        Task<ApiCommonResponse> GetDropReasonById(long id);
        Task<ApiCommonResponse> GetDropReasonByTitle(string name);
        Task<ApiCommonResponse> UpdateDropReason(HttpContext context, long id, DropReasonReceivingDTO DropReasonReceivingDTO);
        Task<ApiCommonResponse> DeleteDropReason(long id);

    }
}