using System.Threading.Tasks;
using HaloBiz.DTOs.ApiDTOs;
using HaloBiz.DTOs.ReceivingDTOs;
using Microsoft.AspNetCore.Http;

namespace HaloBiz.MyServices.LAMS
{
    public interface IEndorsementTypeService
    {
        Task<ApiCommonResponse> AddEndorsementType(HttpContext context, EndorsementTypeReceivingDTO endorsementTypeReceivingDTO);
        Task<ApiCommonResponse> GetAllEndorsementType();
        Task<ApiCommonResponse> GetEndorsementTypeById(long id);
        Task<ApiCommonResponse> GetEndorsementTypeByName(string name);
        Task<ApiCommonResponse> UpdateEndorsementType(HttpContext context, long id, EndorsementTypeReceivingDTO endorsementTypeReceivingDTO);
        Task<ApiCommonResponse> DeleteEndorsementType(long id);

    }
}