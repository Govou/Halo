using System.Threading.Tasks;
using HaloBiz.DTOs.ApiDTOs;

namespace HaloBiz.MyServices
{
    public interface IStatesService
    {
        Task<ApiCommonResponse> GetStateById(long id);
        Task<ApiCommonResponse> GetStateByName(string name);
        Task<ApiCommonResponse> GetAllStates();
        Task<ApiCommonResponse> GetAllLgas();
    }
}