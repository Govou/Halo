using System.Threading.Tasks;
using HaloBiz.DTOs.ApiDTOs;
using HaloBiz.DTOs.ReceivingDTOs;
using Microsoft.AspNetCore.Http;

namespace HaloBiz.MyServices
{
    public interface IClientContactQualificationService
    {
        Task<ApiResponse> AddClientContactQualification(HttpContext context, ClientContactQualificationReceivingDTO clientContactQualificationReceivingDTO);
        Task<ApiResponse> GetAllClientContactQualification();
        Task<ApiResponse> UpdateClientContactQualification(HttpContext context, long id, ClientContactQualificationReceivingDTO clientContactQualificationReceivingDTO);
        Task<ApiResponse> DeleteClientContactQualification(long id);
    }
}