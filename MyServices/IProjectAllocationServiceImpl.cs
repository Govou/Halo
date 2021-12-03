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
using Task = HalobizMigrations.Models.ProjectManagement.Task;

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

        Task<ApiGenericResponse<Workspace>> updateWorkspace(HttpContext httpContext, long id, UpdateWorkspaceDTO workspaceDTO);
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
        Task<ApiResponse> getProjectByProjectName(HttpContext httpContext, string projectName);

        Task<ApiResponse> getWorkByProjectCreatorId(HttpContext httpContext);

        Task<ApiResponse> getWatchersByProjectId(HttpContext httpContext, long projectId);

        Task<ApiGenericResponse<List<Watcher>>> addmoreWatchers(HttpContext httpContext, long projectId, List<WatchersDTO> watchersDTOs);

        Task<ApiGenericResponse<Project>> updateProject(HttpContext httpContext, long projectId, ProjectDTO projectDTO);

        Task<ApiGenericResponse<List<Watcher>>> removeWatcher(HttpContext httpContext, long projectId, long projectWatcherId);
        Task<ApiGenericResponse<List<TaskSummaryDTO>>> createNewTask(HttpContext context, long projectId, TaskDTO taskDTO);

        Task<ApiGenericResponse<TaskSummaryDTO>> getTaskByCaption(HttpContext httpContext, string caption);

        Task<ApiGenericResponse<TaskSummaryDTO>> getTaskById(HttpContext httpContext, long taskId);

        Task<ApiGenericResponse<List<TaskSummaryDTO>>> getTaskByProjectId(HttpContext httpContext, long projectId);

        Task<ApiGenericResponse<List<TaskSummaryDTO>>> getAllTask(HttpContext httpContext);

        Task<ApiGenericResponse<List<ProjectSummaryDTO>>> getProjectByWorkspaceId(HttpContext httpContext, long workspaceId);

        Task<ApiGenericResponse<TaskSummaryDTO>> updateTask(HttpContext httpContext, long TaskId, TaskDTO taskDTO);
        Task<ApiGenericResponse<List<Deliverable>>> createNewDeliverable(HttpContext httpContext, long TaskId, DeliverableDTO deliverableDTO);

    }
}
