using Halobiz.Common.DTOs.ApiDTOs;
using OnlinePortalBackend.DTOs.TransferDTOs;
using System.Threading.Tasks;

namespace OnlinePortalBackend.MyServices
{
    public interface IUtilityService
    {
        Task<ApiCommonResponse> GetStates();
        Task<ApiCommonResponse> GetLocalGovtAreas(int stateId);
        Task<ApiCommonResponse> GetStateById(int id);
        Task<ApiCommonResponse> GetLocalGovtAreaById(int id);
        Task<ApiCommonResponse> GetBusinessTypes();
    }
}
