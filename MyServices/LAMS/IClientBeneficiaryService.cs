using System.Threading.Tasks;
using HaloBiz.DTOs.ApiDTOs;
using HaloBiz.DTOs.ReceivingDTOs;
using Microsoft.AspNetCore.Http;

namespace HaloBiz.MyServices.LAMS
{
    public interface IClientBeneficiaryService
    {
        Task<ApiResponse> AddClientBeneficiary(HttpContext context, ClientBeneficiaryReceivingDTO clientBeneficiaryReceivingDTO);
        Task<ApiResponse> GetAllClientBeneficiary();
        Task<ApiResponse> GetClientBeneficiaryById(long id);
        Task<ApiResponse> GetClientBeneficiaryByCode(string code);
        Task<ApiResponse> UpdateClientBeneficiary(HttpContext context, long id, ClientBeneficiaryReceivingDTO clientBeneficiaryReceivingDTO);
        Task<ApiResponse> DeleteClientBeneficiary(long id);

    }
}