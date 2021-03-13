using System.Threading.Tasks;
using HaloBiz.DTOs.ApiDTOs;
using HaloBiz.DTOs.ReceivingDTOs;
using Microsoft.AspNetCore.Http;

namespace HaloBiz.MyServices
{    
    public interface ISBUProportionService
    {
        Task<ApiResponse> AddSBUProportion(HttpContext context, SBUProportionReceivingDTO sBUProportionReceivingDTO);

        Task<ApiResponse> GetAllSBUProportions();

        Task<ApiResponse> GetSBUProportionById(long id);


        Task<ApiResponse> UpdateSBUProportion(HttpContext context, long id, SBUProportionReceivingDTO sBUProportionReceivingDTO);

        Task<ApiResponse> DeleteSBUProportion(long id);

    }
}