using System.Threading.Tasks;
using HaloBiz.DTOs.ApiDTOs;
using HaloBiz.EnumResponse;

namespace HaloBiz.MyServices.Impl
{
    public interface IProjectManagementMailService
    {
        Task<ApiCommonResponse> BuildMail(ProjectManagementOption projectManagementOption, long requestId);
    }
}