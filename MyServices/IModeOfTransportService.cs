using System.Threading.Tasks;
using HaloBiz.DTOs.ApiDTOs;
using HaloBiz.DTOs.ReceivingDTOs;
using Microsoft.AspNetCore.Http;

namespace HaloBiz.MyServices
{
    public interface IModeOfTransportService
    {
        Task<ApiResponse> AddModeOfTransport(HttpContext context, ModeOfTransportReceivingDTO modeOfTransportReceivingDTO);
        Task<ApiResponse> GetAllModeOfTransport();
        Task<ApiResponse> GetModeOfTransportById(long id);
        Task<ApiResponse> GetModeOfTransportByName(string name);
        Task<ApiResponse> UpdateModeOfTransport(HttpContext context, long id, ModeOfTransportReceivingDTO modeOfTransportReceivingDTO);
        Task<ApiResponse> DeleteModeOfTransport(long id);
    }
}