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

        Task<ApiResponse> AddNewManager(HttpContext context, ProjectAllocationRecievingDTO projectAllocationDTO);
        Task<ApiResponse> getProjectManagers(int serviceCategory);
        Task<ApiResponse> getManagersProjects(string email, int emailId);
        Task<ApiResponse> removeFromCategory(long id,int categoryId, long projectId);
        Task<ApiGenericResponse<Workspace>> CreateNewWorkspace(HttpContext context, WorkspaceDTO workspaceDTO);

        Task<ApiResponse> getAllWorkspaces(HttpContext httpContext);
        Task<ApiResponse> getWorkspaceById(long id);

        Task<ApiResponse> getAllProjectManagers();
        Task<ApiResponse> disableWorkspace(long id);

        Task<ApiResponse> getWorkspaceByCaption(string caption);

        Task<ApiResponse> getDefaultStatus();

        Task<ApiResponse> getAllDefaultStatus();
        Task<ApiResponse> updateStatusFlowOpton(HttpContext httpContext, long workspaceId, string statusOption, List<StatusFlowDTO> statusFlowDTOs);
        Task<ApiResponse> addmoreStatus(HttpContext httpContext, long workspaceId, List<StatusFlowDTO> statusFlowDTO);

        Task<ApiResponse> createDefaultStatus(HttpContext httpContext, List<DefaultStatusDTO> defaultStatusDTOs);

        Task<ApiResponse> updateWorkspace(HttpContext httpContext, long id, UpdateWorkspaceDTO workspaceDTO);
        Task<ApiResponse> addMoreProjectCreators(HttpContext httpContext, long id, List<AddMoreUserDto> projectCreatorDtos);
        Task<ApiResponse> removeFromProjectCreator(long workspaceId, long creatorId);
        Task<ApiResponse> disablePrivateUser(long workspaceId, long privateUserId);
        Task<ApiResponse> addMorePrivateUser(HttpContext httpContext, long workspaceId, List<AddMoreUserDto> privateUserid);
        Task<ApiResponse> disableStatus(long workspaceId, long statusId);
        Task<ApiResponse> updateStatus(HttpContext httpContext, long workspaceId, long statusFlowId, StatusFlowDTO statusFlowDTO);
        Task<ApiResponse> updateToPublic(long workspaceId);

        Task<ApiResponse> createProject(HttpContext httpContext, ProjectDTO projectDTO);
        Task<ApiResponse> moveStatusSequenec(HttpContext httpContext, long workspaceId, List<StatusFlowDTO> statusFlowDTO);

        Task<ApiResponse> getAllProjects(HttpContext httpContext);

    }
}
