using System.Threading.Tasks;
using HaloBiz.DTOs.ApiDTOs;
using HaloBiz.DTOs.ReceivingDTOs;
using Microsoft.AspNetCore.Http;

namespace HaloBiz.MyServices.LAMS
{
    public interface IEndorsementTypeService
    {
        Task<ApiResponse> AddEndorsementType(HttpContext context, EndorsementTypeReceivingDTO endorsementTypeReceivingDTO);
        Task<ApiResponse> GetAllEndorsementType();
        Task<ApiResponse> GetEndorsementTypeById(long id);
        Task<ApiResponse> GetEndorsementTypeByName(string name);
        Task<ApiResponse> UpdateEndorsementType(HttpContext context, long id, EndorsementTypeReceivingDTO endorsementTypeReceivingDTO);
        Task<ApiResponse> DeleteEndorsementType(long id);

    }
}