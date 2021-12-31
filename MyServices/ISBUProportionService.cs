using System.Threading.Tasks;
using HaloBiz.DTOs.ApiDTOs;
using HaloBiz.DTOs.ReceivingDTOs;
using Microsoft.AspNetCore.Http;

namespace HaloBiz.MyServices
{    
    public interface ISbuproportionService
    {
        Task<ApiResponse> AddSbuproportion(HttpContext context, SbuproportionReceivingDTO sBUProportionReceivingDTO);

        Task<ApiResponse> GetAllSbuproportions();

        Task<ApiResponse> GetSbuproportionById(long id);


        Task<ApiResponse> UpdateSbuproportion(HttpContext context, long id, SbuproportionReceivingDTO sBUProportionReceivingDTO);

        Task<ApiResponse> DeleteSbuproportion(long id);

    }
}