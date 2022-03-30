using Halobiz.Common.DTOs.ApiDTOs;
using OnlinePortalBackend.DTOs.ApiDTOs;
using System.Threading.Tasks;

namespace OnlinePortalBackend.MyServices
{
    public interface IServicesService
    {
        Task<ApiCommonResponse> GetServiceDetails(int serviceId);
    }
}
