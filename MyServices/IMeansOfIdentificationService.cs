using System.Threading.Tasks;
using HaloBiz.DTOs.ApiDTOs;
using HaloBiz.DTOs.ReceivingDTOs;
using Microsoft.AspNetCore.Http;

namespace HaloBiz.MyServices
{
    public interface IMeansOfIdentificationService
    {
        Task<ApiResponse> AddMeansOfIdentification(HttpContext context, MeansOfIdentificationReceivingDTO MeansOfIdentificationReceivingDTO);
        Task<ApiResponse> GetAllMeansOfIdentification();
        Task<ApiResponse> GetMeansOfIdentificationById(long id);
        Task<ApiResponse> GetMeansOfIdentificationByName(string name);
        Task<ApiResponse> UpdateMeansOfIdentification(HttpContext context, long id, MeansOfIdentificationReceivingDTO MeansOfIdentificationReceivingDTO);
        Task<ApiResponse> DeleteMeansOfIdentification(long id);
    }
}