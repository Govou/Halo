using System.Threading.Tasks;
using HaloBiz.DTOs.ApiDTOs;

namespace HaloBiz.MyServices.LAMS
{
    public interface IContractServiceService
    {
        Task<ApiResponse> DeleteContractService(long id);

        Task<ApiResponse> GetAllContractsServcieForAContract(long contractId);
        Task<ApiResponse> GetContractServiceByGroupInvoiceNumber(string refNo);
        Task<ApiResponse> GetContractServiceByTag(string tag);

        Task<ApiResponse> GetContractServiceByReferenceNumber(string refNo);

        Task<ApiResponse> GetContractServiceById(long id);
        Task<ApiResponse> GetAllContractsServcie();
    }
}