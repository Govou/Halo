using System.Threading.Tasks;
using HaloBiz.DTOs.ApiDTOs;
using HaloBiz.DTOs.ReceivingDTOs;
using Microsoft.AspNetCore.Http;

namespace HaloBiz.MyServices
{    
    public interface ISbuproportionService
    {
        Task<ApiCommonResponse> AddSbuproportion(HttpContext context, SbuproportionReceivingDTO sBUProportionReceivingDTO);

        Task<ApiCommonResponse> GetAllSbuproportions();

        Task<ApiCommonResponse> GetSbuproportionById(long id);


        Task<ApiCommonResponse> UpdateSbuproportion(HttpContext context, long id, SbuproportionReceivingDTO sBUProportionReceivingDTO);

        Task<ApiCommonResponse> DeleteSbuproportion(long id);

    }
}