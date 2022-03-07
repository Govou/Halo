using System.Threading.Tasks;
using Halobiz.Common.DTOs.ApiDTOs;
using HaloBiz.DTOs.ReceivingDTOs;
using Microsoft.AspNetCore.Http;

namespace HaloBiz.MyServices
{
    public interface IMeansOfIdentificationService
    {
        Task<ApiCommonResponse> AddMeansOfIdentification(HttpContext context, MeansOfIdentificationReceivingDTO MeansOfIdentificationReceivingDTO);
        Task<ApiCommonResponse> GetAllMeansOfIdentification();
        Task<ApiCommonResponse> GetMeansOfIdentificationById(long id);
        Task<ApiCommonResponse> GetMeansOfIdentificationByName(string name);
        Task<ApiCommonResponse> UpdateMeansOfIdentification(HttpContext context, long id, MeansOfIdentificationReceivingDTO MeansOfIdentificationReceivingDTO);
        Task<ApiCommonResponse> DeleteMeansOfIdentification(long id);
    }
}