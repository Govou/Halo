using HaloBiz.DTOs.ApiDTOs;
using HaloBiz.DTOs.GenericResponseDTO;
using HaloBiz.DTOs.ProjectManagementDTO;
using HaloBiz.DTOs.ReceivingDTOs;
using HalobizMigrations.Models.ProjectManagement;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HaloBiz.MyServices
{
    public interface IProjectAllocationServiceImpl
    {

        Task<ApiCommonResponse> AddNewManager(HttpContext context, ProjectAllocationRecievingDTO projectAllocationDTO);
        Task<ApiCommonResponse> getProjectManagers(int serviceCategory);
        Task<ApiCommonResponse> getManagersProjects(string email, int emailId);
        Task<ApiCommonResponse> removeFromCategory(long id,int categoryId, long projectId);
        Task<ApiCommonResponse> CreateNewWorkspace(HttpContext context, WorkspaceDTO workspaceDTO);

        Task<ApiCommonResponse> getAllWorkspaces(HttpContext httpContext);
        Task<ApiCommonResponse> getWorkspaceById(long id);

        Task<ApiCommonResponse> getAllProjectManagers();
        Task<ApiCommonResponse> disableWorkspace(long id);

        Task<ApiCommonResponse> getWorkspaceByCaption(string caption);

        Task<ApiCommonResponse> getDefaultStatus();

        Task<ApiCommonResponse> getAllDefaultStatus();
        Task<ApiCommonResponse> updateStatusFlowOpton(HttpContext httpContext, long workspaceId, string statusOption, List<StatusFlowDTO> statusFlowDTOs);
        Task<ApiCommonResponse> addmoreStatus(HttpContext httpContext, long workspaceId, List<StatusFlowDTO> statusFlowDTO);

        Task<ApiCommonResponse> createDefaultStatus(HttpContext httpContext, List<DefaultStatusDTO> defaultStatusDTOs);

        Task<ApiCommonResponse> updateWorkspace(HttpContext httpContext, long id, UpdateWorkspaceDTO workspaceDTO);
        Task<ApiCommonResponse> addMoreProjectCreators(HttpContext httpContext, long id, List<AddMoreUserDto> projectCreatorDtos);
        Task<ApiCommonResponse> removeFromProjectCreator(long workspaceId, long creatorId);
        Task<ApiCommonResponse> disablePrivateUser(long workspaceId, long privateUserId);
        Task<ApiCommonResponse> addMorePrivateUser(HttpContext httpContext, long workspaceId, List<AddMoreUserDto> privateUserid);
        Task<ApiCommonResponse> disableStatus(long workspaceId, long statusId);
        Task<ApiCommonResponse> updateStatus(HttpContext httpContext, long workspaceId, long statusFlowId, StatusFlowDTO statusFlowDTO);
        Task<ApiCommonResponse> updateToPublic(long workspaceId);
        Task<ApiCommonResponse> moveStatusSequenec(HttpContext httpContext, long workspaceId, List<StatusFlowDTO> statusFlowDTO);

    }
}
