using System.Threading.Tasks;
using HaloBiz.DTOs.ApiDTOs;
using HaloBiz.DTOs.ReceivingDTOs;

namespace HaloBiz.MyServices
{
    public interface IServiceGroupService
    {
        Task<ApiCommonResponse> AddServiceGroup(ServiceGroupReceivingDTO serviceGroupReceivingDTO);
        Task<ApiCommonResponse> GetAllServiceGroups();
        Task<ApiCommonResponse> GetServiceGroupById(long id);
        Task<ApiCommonResponse> GetServiceGroupByName(string name);
        Task<ApiCommonResponse> UpdateServiceGroup(long id, ServiceGroupReceivingDTO serviceGroupReceivingDTO);
        Task<ApiCommonResponse> DeleteServiceGroup(long id);
    }
}