using System.Threading.Tasks;
using HaloBiz.DTOs.ApiDTOs;

namespace HaloBiz.MyServices.LAMS
{
    public interface IContractService
    {
        Task<ApiResponse> DeleteContract(long id);

        Task<ApiResponse> GetAllContracts();

        Task<ApiResponse> GetContractByReferenceNumber(string refNo);

        Task<ApiResponse> GetContractById(long id);
    }
}