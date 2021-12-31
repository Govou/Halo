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
        Task<ApiCommonResponse> createProject(HttpContext httpContext, ProjectDTO projectDTO);
        Task<ApiCommonResponse> getAllProjects(HttpContext httpContext);
        Task<ApiCommonResponse> getProjectByProjectName(HttpContext httpContext, string projectName);

        Task<ApiCommonResponse> getWorkByProjectCreatorId(HttpContext httpContext);

        Task<ApiCommonResponse> getWatchersByProjectId(HttpContext httpContext, long projectId);

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

        Task<ApiGenericResponse<List<Deliverable>>> getAllDeliverables(HttpContext httpContext);

        Task<ApiGenericResponse<List<DeliverableDTO>>> getAllDeliverablesByTaskId(HttpContext httpContext, long taskId);

        Task<ApiGenericResponse<Deliverable>> getDeliverablesById(HttpContext httpContext, long id);

        Task<ApiGenericResponse<List<PrivacyAccess>>> getAllPrivacyAccessByWorkspaceId(HttpContext httpContext, long workspaceId);
        Task<ApiGenericResponse<List<PMRequirement>>> getRequirementsByDeliverableId(HttpContext httpContext, long deliverableId);
        Task<ApiGenericResponse<List<TaskRevampDTO>>> getAssignedTask(HttpContext httpContext);
        Task<ApiGenericResponse<List<WorkspaceDTO>>> getAllProjectCreatorsWorkspace(HttpContext httpContext);
        Task<ApiGenericResponse<List<Task>>> pickUptask(long taskId, HttpContext httpContext);
        Task<ApiGenericResponse<List<Task>>> dropTask(long taskId, long taskOwnershipId, HttpContext httpContext);
        Task<ApiGenericResponse<List<Task>>> getAllPickedTask(HttpContext httpContext);
        Task<ApiGenericResponse<List<Deliverable>>> AssignDeliverable(HttpContext context, long taskId, long deliverableId, long assigneDeliverableId, AssignDeliverableDTO assignDeliverableDTO);
        Task<ApiGenericResponse<List<Deliverable>>> createDeliverableIllustrattions(HttpContext context, long deliverableId, long taskId, List<IllustrationsDTO> illustrationsDTO);
        Task<ApiGenericResponse<List<Deliverable>>> DeleteIllustration(HttpContext context, long taskId, long deliverableId,long illustrationId);
        Task<ApiGenericResponse<List<PMIllustration>>> createTaskIllustration(List<IllustrationsDTO> illustrationsDTO, long taskId, HttpContext httpContext);
        Task<ApiGenericResponse<List<PMIllustration>>> getTaskIllustrationById(long taskId);
        Task<ApiGenericResponse<List<PMIllustration>>> removeIllustrationById(long taskId, long illustrationId);
        Task<ApiGenericResponse<List<Deliverable>>> createNewDeliverableFromTask(HttpContext httpContext, long TaskId, DeliverableDTO deliverableDTO);
        Task<ApiGenericResponse<IEnumerable<TaskAssignee>>> addMoreTaskAssignees(HttpContext context, long taskId, List<TaskAssigneeDTO> taskAssigneeDTO);
        Task<ApiGenericResponse<List<TaskAssignee>>> disableTaskAssignee(HttpContext context, long taskId, long assigneeId);
        Task<ApiGenericResponse<List<Deliverable>>> updateDeliverable(HttpContext httpContext, long taskId, long deliverableId, DeliverableDTO deliverableDTO);
        Task<ApiGenericResponse<List<Deliverable>>> disableDeliverable(HttpContext httpContext, long taskId, long deliverableId);

    }
}
