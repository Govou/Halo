using Halobiz.Common.DTOs.ApiDTOs;
using HaloBiz.DTOs.GenericResponseDTO;
using HaloBiz.DTOs.ProjectManagementDTO;
using HaloBiz.DTOs.ReceivingDTOs;
using HalobizMigrations.Models.ProjectManagement;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HaloBiz.DTOs;
using HaloBiz.DTOs.TransferDTOs;
using HaloBiz.Models;
using Microsoft.AspNetCore.Mvc.Filters;
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
        Task<ApiCommonResponse> updateStatusFlowOptionToDefault(HttpContext httpContext, long workspaceId);
        Task<ApiCommonResponse> addmoreStatus(HttpContext httpContext, long workspaceId, List<StatusFlowDTO> statusFlowDTO);
        Task<ApiCommonResponse> getProjectById(HttpContext httpContext, long projectId);
        Task<ApiCommonResponse> createDefaultStatus(HttpContext httpContext, List<DefaultStatusDTO> defaultStatusDTOs);
        Task<ApiCommonResponse> updateStatusFlowOptionToCustom(HttpContext httpContext, long workspaceId, List<StatusFlowDTO> statusFlowDTOs);
        Task<ApiCommonResponse> removeProject(HttpContext httpContext, long projectId);
        Task<ApiCommonResponse> removeTask(HttpContext httpContext, long taskId, long projectId);
        Task<ApiCommonResponse> removeDeliverable(HttpContext httpContext, long deliverableId, long taskId);
        Task<ApiCommonResponse> updateWorkspace(HttpContext httpContext, long id, UpdateWorkspaceDTO workspaceDTO);
        Task<ApiCommonResponse> addMoreProjectCreators(HttpContext httpContext, long id, List<AddMoreUserDto> projectCreatorDtos);
        Task<ApiCommonResponse> removeFromProjectCreator(long workspaceId, long creatorId);
        Task<ApiCommonResponse> disablePrivateUser(long workspaceId, long privateUserId);
        Task<ApiCommonResponse> getDeliverableIsDeclined(HttpContext httpContext);
        Task<ApiCommonResponse> getAllMilestoneTaskForWatcher(HttpContext httpContext);
        Task<ApiCommonResponse> addMorePrivateUser(HttpContext httpContext, long workspaceId, List<AddMoreUserDto> privateUserid);
        Task<ApiCommonResponse> disableStatus(long workspaceId, long statusId);
        Task<ApiCommonResponse> updateStatus(HttpContext httpContext, long workspaceId, long statusFlowId, StatusFlowDTO statusFlowDTO);
        Task<ApiCommonResponse> updateToPublic(long workspaceId);
        Task<ApiCommonResponse> getAllTaskToDueToday(HttpContext httpContext);
        Task<ApiCommonResponse> getWorkspaceWithProjectWatcher(HttpContext httpContext);
        Task<ApiCommonResponse> getAllTaskFromProjectRevamped(HttpContext httpContext, long projectId);
        Task<ApiCommonResponse> getProjectCountBarChart(HttpContext httpContext);
        Task<ApiCommonResponse> getWorkspaceCountBarChart(HttpContext httpContext);
        Task<ApiCommonResponse> getAllTaskFromProject(HttpContext httpContext, long projectId);
        Task<ApiCommonResponse> moveStatusSequenec(HttpContext httpContext, long workspaceId, List<StatusFlowDTO> statusFlowDTO);
        Task<ApiCommonResponse> createProject(HttpContext httpContext, ProjectDTO projectDTO);
        Task<ApiCommonResponse> getAllProjects(HttpContext httpContext);
        Task<ApiCommonResponse> getTaskOwnershipDTO(HttpContext httpContext);
        Task<ApiCommonResponse> getProjectByProjectName(HttpContext httpContext, string projectName);

        Task<ApiCommonResponse> getWorkByProjectCreatorId(HttpContext httpContext);

        Task<ApiCommonResponse> getWatchersByProjectId(HttpContext httpContext, long projectId);

        Task<ApiCommonResponse> addmoreWatchers(HttpContext httpContext, long projectId, List<WatchersDTO> watchersDTOs);

        Task<ApiCommonResponse> updateProject(HttpContext httpContext, long projectId, ProjectDTO projectDTO);

        Task<ApiCommonResponse> removeWatcher(HttpContext httpContext, long projectId, long projectWatcherId);
        Task<ApiCommonResponse> createNewTask(HttpContext context, long projectId, TaskDTO taskDTO);

        Task<ApiCommonResponse> getTaskByCaption(HttpContext httpContext, string caption);

        Task<ApiCommonResponse> getTaskById(HttpContext httpContext, long taskId);

        Task<ApiCommonResponse> getTaskByProjectId(HttpContext httpContext, long projectId);

        Task<ApiCommonResponse> getAllTask(HttpContext httpContext);

        Task<ApiCommonResponse> getProjectByWorkspaceId(HttpContext httpContext, long workspaceId);

        Task<ApiCommonResponse> updateTask(HttpContext httpContext, long TaskId, TaskDTO taskDTO);
        Task<ApiCommonResponse> createNewDeliverable(HttpContext httpContext, long TaskId, DeliverableDTO deliverableDTO);

        Task<ApiCommonResponse> getAllDeliverables(HttpContext httpContext);

        Task<ApiCommonResponse> getAllDeliverablesByTaskId(HttpContext httpContext, long taskId);

        Task<ApiCommonResponse> getDeliverablesById(HttpContext httpContext, long id);

        Task<ApiCommonResponse> getAllPrivacyAccessByWorkspaceId(HttpContext httpContext, long workspaceId);
        Task<ApiCommonResponse> getRequirementsByDeliverableId(HttpContext httpContext, long deliverableId);
        Task<ApiCommonResponse> getAssignedTask(HttpContext httpContext);
        Task<ApiCommonResponse> getAllProjectCreatorsWorkspace(HttpContext httpContext);
        Task<ApiCommonResponse> pickUptask(long taskId, HttpContext httpContext);
        Task<ApiCommonResponse> dropTask(long taskId, long taskOwnershipId, HttpContext httpContext);
        Task<ApiCommonResponse> getAllPickedTask(HttpContext httpContext);
        Task<ApiCommonResponse> AssignDeliverable(HttpContext context, long taskId, long deliverableId, long assigneDeliverableId, AssignDeliverableDTO assignDeliverableDTO);
        Task<ApiCommonResponse> createDeliverableIllustrattions(HttpContext context, long deliverableId, long taskId,IllustrationsDTO illustrationsDTO);
        Task<ApiCommonResponse> DeleteIllustration(HttpContext context, long taskId, long deliverableId,long illustrationId);
        Task<ApiCommonResponse> createTaskIllustration(IllustrationsDTO illustrationsDTO, long taskId, HttpContext httpContext);
        Task<ApiCommonResponse> getTaskIllustrationById(long taskId);
        Task<ApiCommonResponse> getTaskPieChartData(HttpContext httpContext);
        Task<ApiCommonResponse> getDeliverablesByTaskId(HttpContext httpContext, long taskId);
        Task<ApiCommonResponse> removeIllustrationById(long taskId, long illustrationId);
        Task<ApiCommonResponse> createNewDeliverableFromTask(HttpContext httpContext, long TaskId, DeliverableDTO deliverableDTO);
        Task<ApiCommonResponse> addMoreTaskAssignees(HttpContext context, long taskId, List<TaskAssigneeDTO> taskAssigneeDTO);
        Task<ApiCommonResponse> disableTaskAssignee(HttpContext context, long taskId, long assigneeId);
        Task<ApiCommonResponse> updateDeliverable(HttpContext httpContext, long taskId, long deliverableId, DeliverableDTO deliverableDTO);
        Task<ApiCommonResponse> disableDeliverable(HttpContext httpContext, long taskId, long deliverableId);
        Task<ApiCommonResponse> getBarChartDetails(HttpContext httpContext, long taskId);
        Task<ApiCommonResponse> getAllMilestoneTaskDueTodayForWatcher(HttpContext httpContext);
        Task<ApiCommonResponse> getAssignedDeliverableStatus(HttpContext httpContext, List<DeliverableStatusDTO> deliverableStatusDTOs);
        Task<ApiCommonResponse> getWorkspaceWithStatus(HttpContext httpContext);
        Task<ApiCommonResponse> getCurrentDeliverableStatus(HttpContext httpContext,long deliverableId);
        Task<ApiCommonResponse> createUploadedRequirement(HttpContext httpContext, UploadedRequirement uploadedRequirement);
        Task<ApiCommonResponse> disableRequirementUpload(HttpContext httpContext, long uploadedRequirementId);
        Task<ApiCommonResponse> moveToAnotherStatus(HttpContext httpContext, List<StatusFlow> statuses, long statusId, long deliverableId, int statusCode);
        Task<ApiCommonResponse> pickDeliverable(HttpContext httpContext, long deliverableId);
        Task<ApiCommonResponse> selectStatus(HttpContext httpContext, long statusId, long deliverableId);
        Task<ApiCommonResponse> addComments(HttpContext httpContext, long deliverableId,CommentsDTO comments);
        Task<ApiCommonResponse> disableComment(HttpContext httpContext, long commentId, long deliverableId);
        Task<ApiCommonResponse> saveAmountSpent(HttpContext httpContext, decimal amount, long deliverableId);
        Task<ApiCommonResponse> getDeliverableApprovalList(HttpContext httpContext);
        Task<ApiCommonResponse> pushForApproval(HttpContext httpContext, long deliverableId);
        Task<ApiCommonResponse> reverseApproval(HttpContext httpContext, long deliverableId);
        Task<List<TaskAssigneeDTO>> getAssignees(HttpContext httpContext, long taskId);
        Task<ApiCommonResponse> ApproveDeliverable(HttpContext httpContext, long deliverableId);
        Task<ApiCommonResponse> DeclineDeliverable(HttpContext httpContext, long deliverableId, string declineReason);
        Task<ApiCommonResponse> getDeliverableApproved(HttpContext httpContext);
        Task<ApiCommonResponse> getprojectForWatchers(HttpContext httpContext);
        Task<ApiCommonResponse> getAllWorkspacesRevamped(HttpContext httpContext);
        Task<ApiCommonResponse> getAllDataForWorkspaceSideBar(HttpContext httpContext);
        Task<ApiCommonResponse> getWorkspaceById(HttpContext httpContext, long workspaceId);
        //Task<ApiCommonResponse> CreateDefaultWorkspace();
        // Task<ApiCommonResponse> ResolveQuotesIntoProjects(HttpContext httpContext,long serviceId, string fulfillmentType);
        // //Task<ApiCommonResponse> FetchAmortizationData(int year, int month);
        Task<ApiCommonResponse> FetchAmortizationMaster(int year, int month);

        Task<ApiCommonResponse> FetchAmortizationDetails();
         Task<ApiCommonResponse> RetrieveCustomerDivision();
         Task<ApiCommonResponse> DeleteEvent(string eventId);
         Task<ApiCommonResponse> PushEventToGoogleCalender(CalenderRequestDTO calenderRequestDto,HttpContext httpContext);
         Task<ApiCommonResponse> SendEmail(MailRequest mailRequest, HttpContext htttHttpContext);
         Task<ApiCommonResponse> GetAllConcernedMail(HttpContext httpContext);
         Task<ApiCommonResponse> DisableMail(long emailId);
         Task<ApiCommonResponse> GetAllWatcherProjectsAndTask(HttpContext httpContext);
         Task<ApiCommonResponse> getAllProjectCreatorsWorkspacesRevamped(HttpContext httpContext);
         Task<ApiCommonResponse> getAllDeliverablesRevamp(HttpContext httpContext);
         Task<ApiCommonResponse> DisableProjectComment(long commentId);
         Task<ApiCommonResponse> RetrieveProjectComments(long projectId);
         Task<ApiCommonResponse> GetAllProjectsComment(HttpContext httpContext);
         Task<ApiCommonResponse> MakeProjectComment(ProjectCommentRequest projectCommentRequest, long projectId,
             HttpContext httpContext);

         Task<ApiCommonResponse> DisableTaskComment(long commentId);
         Task<ApiCommonResponse> GetAllTaskComment(HttpContext httpContext);

         Task<ApiCommonResponse> MakeTaskComment(ProjectCommentRequest projectCommentRequest, long taskId,
             HttpContext httpContext);

         Task<ApiCommonResponse> ResolveQuotesIntoProjects(HttpContext httpContext,
             ProjectFulfilmentDto projectFulfilment);

    }
}
