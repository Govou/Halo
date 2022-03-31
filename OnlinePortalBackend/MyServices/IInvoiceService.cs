using Halobiz.Common.DTOs.ApiDTOs;
using System.Threading.Tasks;

namespace OnlinePortalBackend.MyServices
{
    public interface IInvoiceService
    {
        Task<ApiCommonResponse> GetInvoices(int userId, int? contractService, int? contractId, int limit = 10);
    }
}
