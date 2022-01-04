using System.Threading.Tasks;
using HaloBiz.DTOs.ApiDTOs;
using HaloBiz.DTOs.ReceivingDTOs;
using Microsoft.AspNetCore.Http;

namespace HaloBiz.MyServices
{
    public interface IModeOfTransportService
    {
        Task<ApiCommonResponse> AddModeOfTransport(HttpContext context, ModeOfTransportReceivingDTO modeOfTransportReceivingDTO);
        Task<ApiCommonResponse> GetAllModeOfTransport();
        Task<ApiCommonResponse> GetModeOfTransportById(long id);
        Task<ApiCommonResponse> GetModeOfTransportByName(string name);
        Task<ApiCommonResponse> UpdateModeOfTransport(HttpContext context, long id, ModeOfTransportReceivingDTO modeOfTransportReceivingDTO);
        Task<ApiCommonResponse> DeleteModeOfTransport(long id);
    }
}