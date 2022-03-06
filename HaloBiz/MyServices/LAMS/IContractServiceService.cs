using System.Threading.Tasks;
using Halobiz.Common.DTOs.ApiDTOs;

namespace HaloBiz.MyServices.LAMS
{
    public interface IContractServiceService
    {
        Task<ApiCommonResponse> DeleteContractService(long id);

        Task<ApiCommonResponse> GetAllContractsServcieForAContract(long contractId);
        Task<ApiCommonResponse> GetContractServiceByGroupInvoiceNumber(string refNo);
        Task<ApiCommonResponse> GetContractServiceByTag(string tag);

        Task<ApiCommonResponse> GetContractServiceByReferenceNumber(string refNo);

        Task<ApiCommonResponse> GetContractServiceById(long id);
        Task<ApiCommonResponse> GetAllContractsServcie();
        Task<ApiCommonResponse> GetAllContractsServceByid(long customerDivisionId);

        //Task<ApiCommonResponse> GetContractServiceById(long id);

    }
}