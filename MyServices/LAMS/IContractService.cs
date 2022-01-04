using System.Threading.Tasks;
using HaloBiz.DTOs.ApiDTOs;

namespace HaloBiz.MyServices.LAMS
{
    public interface IContractService
    {
        Task<ApiCommonResponse> DeleteContract(long id);

        Task<ApiCommonResponse> GetAllContracts();

        Task<ApiCommonResponse> GetContractByReferenceNumber(string refNo);

        Task<ApiCommonResponse> GetContractById(long id);
        Task<ApiCommonResponse> GetContractsByLeadId(long leadId);
        Task<ApiCommonResponse> GetContractsByCustomerId(long leadId);
    }
}