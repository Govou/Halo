using Halobiz.Common.DTOs.ApiDTOs;
using System.Threading.Tasks;

namespace OnlinePortalBackend.MyServices
{
    public interface ICustomerInfoService
    {
        Task<ApiCommonResponse> FetchContractInfos(int customerId);
    }
}
