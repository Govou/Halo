using System.Threading.Tasks;
using Halobiz.Common.DTOs.ApiDTOs;
using HaloBiz.DTOs.ReceivingDTOs;
using Microsoft.AspNetCore.Http;

namespace HaloBiz.MyServices
{
    public interface IModelService
    {
        Task<ApiCommonResponse> AddModel(HttpContext context, ModelReceivingDTO modelReceivingDTO);
        Task<ApiCommonResponse> GetAllModelByMake(int makeId);
        Task<ApiCommonResponse> DeleteModel(long id);
        Task<ApiCommonResponse> GetModelById(long id);
        Task<ApiCommonResponse> UpdateModel(HttpContext context, long id, ModelReceivingDTO modelReceivingDTO);
    }
}