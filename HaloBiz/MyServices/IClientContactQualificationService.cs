using System.Threading.Tasks;
using Halobiz.Common.DTOs.ApiDTOs;
using HaloBiz.DTOs.ReceivingDTOs;
using Microsoft.AspNetCore.Http;

namespace HaloBiz.MyServices
{
    public interface IClientContactQualificationService
    {
        Task<ApiCommonResponse> AddClientContactQualification(HttpContext context, ClientContactQualificationReceivingDTO clientContactQualificationReceivingDTO);
        Task<ApiCommonResponse> GetAllClientContactQualification();
        Task<ApiCommonResponse> UpdateClientContactQualification(HttpContext context, long id, ClientContactQualificationReceivingDTO clientContactQualificationReceivingDTO);
        Task<ApiCommonResponse> DeleteClientContactQualification(long id);
    }
}