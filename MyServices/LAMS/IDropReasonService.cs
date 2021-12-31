using System.Threading.Tasks;
using HaloBiz.DTOs.ApiDTOs;
using HaloBiz.DTOs.ReceivingDTOs;
using Microsoft.AspNetCore.Http;

namespace HaloBiz.MyServices.LAMS
{
    public interface IDropReasonService
    {
        Task<ApiResponse> AddDropReason(HttpContext context, DropReasonReceivingDTO DropReasonReceivingDTO);
        Task<ApiResponse> GetAllDropReason();
        Task<ApiResponse> GetDropReasonById(long id);
        Task<ApiResponse> GetDropReasonByTitle(string name);
        Task<ApiResponse> UpdateDropReason(HttpContext context, long id, DropReasonReceivingDTO DropReasonReceivingDTO);
        Task<ApiResponse> DeleteDropReason(long id);

    }
}