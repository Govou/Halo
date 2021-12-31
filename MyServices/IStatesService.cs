using System.Threading.Tasks;
using HaloBiz.DTOs.ApiDTOs;

namespace HaloBiz.MyServices
{
    public interface IStatesService
    {
        Task<ApiResponse> GetStateById(long id);
        Task<ApiResponse> GetStateByName(string name);
        Task<ApiResponse> GetAllStates();
        Task<ApiResponse> GetAllLgas();
    }
}