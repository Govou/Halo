using System.Threading.Tasks;
using Halobiz.Common.DTOs.ApiDTOs;
using HaloBiz.DTOs.ReceivingDTOs;
using Microsoft.AspNetCore.Http;

namespace HaloBiz.MyServices.LAMS
{
    public interface IClientBeneficiaryService
    {
        Task<ApiCommonResponse> AddClientBeneficiary(HttpContext context, ClientBeneficiaryReceivingDTO clientBeneficiaryReceivingDTO);
        Task<ApiCommonResponse> GetAllClientBeneficiary();
        Task<ApiCommonResponse> GetClientBeneficiaryById(long id);
        Task<ApiCommonResponse> GetClientBeneficiaryByCode(string code);
        Task<ApiCommonResponse> UpdateClientBeneficiary(HttpContext context, long id, ClientBeneficiaryReceivingDTO clientBeneficiaryReceivingDTO);
        Task<ApiCommonResponse> DeleteClientBeneficiary(long id);

    }
}