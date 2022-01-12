using HaloBiz.DTOs.ApiDTOs;
using HaloBiz.DTOs.ProjectManagementDTO;
using HaloBiz.DTOs.ReceivingDTOs;
using HaloBiz.MyServices;
using HalobizMigrations.Models.ProjectManagement;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HaloBiz.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class ProjectManagementController : ControllerBase
    {
        private readonly IProjectAllocationServiceImpl _projectAllocationService;

        public ProjectManagementController(IProjectAllocationServiceImpl projectAllocationService)
        {
            this._projectAllocationService = projectAllocationService;
        }

        [HttpPost("")]
        public async Task<ApiCommonResponse> AttachManagerToServiceCategory(ProjectAllocationRecievingDTO projectAllocationRecievingDTO)
        {
            return await _projectAllocationService.AddNewManager(HttpContext, projectAllocationRecievingDTO);
        }


        [HttpGet("{id}/{email}")]
        public async Task<ApiCommonResponse> GetManagersAttachedToServiceCategory(string email,int id)
        {
            return await _projectAllocationService.getManagersProjects(email,id);
        }

        [HttpGet("{categoryId}")]
        public async Task<ApiCommonResponse> GetServiceCategoryAttachedToASingleManager(int categoryId)
        {
            return await _projectAllocationService.getProjectManagers(categoryId);
        }

        [HttpDelete("{id}/{categoryId}/{projectId}")]
        public async Task<ApiCommonResponse> DetachManagerFromServiceCategory(int id,int categoryId,long projectId)
        {
            return await _projectAllocationService.removeFromCategory(id,categoryId,projectId);
        }

        [HttpPost("workspace")]
        public async Task<ApiCommonResponse> CreateWorkspace(WorkspaceDTO workspaceDTO)
        {
            return await _projectAllocationService.CreateNewWorkspace(HttpContext, workspaceDTO);
        }


        [HttpPost("defaultStatus")]
        public async Task<ApiCommonResponse> CreateDefaultStatus(List<DefaultStatusDTO> defaultStatusFlow)
        {
            return await _projectAllocationService.createDefaultStatus(HttpContext, defaultStatusFlow);
        }


        [HttpGet("GetAllWorkspaces")]

        public async Task<ApiCommonResponse> GetAllWorkspaces()
        {
            return await _projectAllocationService.getAllWorkspaces(HttpContext);
        }

        [HttpGet("GetWorkspaceById/{id}")]

        public async Task<ApiCommonResponse> GetWorkspaceById(long id)
        {
            return await _projectAllocationService.getWorkspaceById(id);
        }

        [HttpGet("GetWorkspaceByCaption/{caption}")]

        public async Task<ApiCommonResponse> GetWorkspaceByCaption(string caption)
        {
            return await _projectAllocationService.getWorkspaceByCaption(caption);
            
        }

        [HttpGet("GetWorkspaceByProjectCreator")]

        public async Task<ApiCommonResponse> GetWorkspaceByProjectCreator()
        {
            return await _projectAllocationService.getWorkByProjectCreatorId(HttpContext);

        }

        [HttpGet("GetWatchersByProjectId/{projectId}")]

        public async Task<ApiCommonResponse> GetWatchersByProjectId(long projectId)
        {
            return await _projectAllocationService.getWatchersByProjectId(HttpContext,projectId);

        }




        [HttpGet("GetAllProjectManagers")]

        public async Task<ApiCommonResponse> GetAllProjectManagers()
        {
            return await _projectAllocationService.getAllProjectManagers();
            
        }

        [HttpGet("GetDefaultStatus")]

        public async Task<ApiCommonResponse> GetDefaultStatus()
        {
            return await _projectAllocationService.getAllDefaultStatus();
            
        }




        [HttpDelete("workspace/{id}")]
        public async Task<ApiCommonResponse> DisableWorkspace(int id)
        {
            return await _projectAllocationService.disableWorkspace(id);
        }

        [HttpPut("workspace/{id}")]
        public async Task<ApiCommonResponse> UpdateWorkspace(long id,UpdateWorkspaceDTO workspaceDTO)
        {
            return await _projectAllocationService.updateWorkspace(HttpContext, id,workspaceDTO);
        }

        [HttpPut("addMoreProjectCreators/{id}")]
        public async Task<ApiCommonResponse> AddMoreProjectCreators(long id, List<AddMoreUserDto> workspaceDTO)
        {
            return await _projectAllocationService.addMoreProjectCreators(HttpContext, id, workspaceDTO);
        }

        [HttpPut("updateProject/{projectId}")]
        public async Task<ApiCommonResponse> updateProject(long projectId, ProjectDTO projectDTO)
        {
            return await _projectAllocationService.updateProject(HttpContext, projectId, projectDTO);

        }


        [HttpDelete("RemoveProjectCreator/{workspaceId}/{creatorId}")]
        public async Task<ApiCommonResponse> RemoveProjectCreator(long workspaceId,long creatorId)
        {
            return await _projectAllocationService.removeFromProjectCreator(workspaceId, creatorId);
        }


        [HttpPut("UpdateToPublic/{workspaceId}")]
        public async Task<ApiCommonResponse> UpdateToPublic(long workspaceId)
        {
            return await _projectAllocationService.updateToPublic(workspaceId);
        }

        [HttpDelete("DisablePrivateUser/{workspaceId}/{privateUserId}")]
        public async Task<ApiCommonResponse> DisablePrivateUser(long workspaceId, long privateUserId)
        {
            return await _projectAllocationService.disablePrivateUser(workspaceId, privateUserId);
        }


        [HttpPut("addMorePrivateUsers/{workspaceId}")]
        public async Task<ApiCommonResponse> AddMorePrivateUsers(long workspaceId, List<AddMoreUserDto>  addMoreUserDtos )
        {
            return await _projectAllocationService.addMorePrivateUser(HttpContext, workspaceId, addMoreUserDtos);
        }

        [HttpPut("UpdateStatus/{workspaceId}/{statusFlowId}")]
        public async Task<ApiCommonResponse> updateStatus(long workspaceId,long statusFlowId, StatusFlowDTO statusFlowDTO)
        {
            return await _projectAllocationService.updateStatus(HttpContext, workspaceId, statusFlowId,statusFlowDTO);
        }

        [HttpPut("AddmoreWatchers/{projectId}")]
        public async Task<ApiCommonResponse> AddmoreWatchers(long projectId,  List<WatchersDTO> watchersDTOs)
        {
            return await _projectAllocationService.addmoreWatchers(HttpContext, projectId, watchersDTOs);

        }


        [HttpPut("AddMoreStatus/{workspaceId}")]
        public async Task<ApiCommonResponse> addMoreStatus(long workspaceId, List<StatusFlowDTO> statusFlowDTO)
        {
            return await _projectAllocationService.addmoreStatus(HttpContext, workspaceId, statusFlowDTO);
        }

        [HttpPut("MoveSequence/{workspaceId}")]
        public async Task<ApiCommonResponse> MoveSequence(long workspaceId, List<StatusFlowDTO> statusFlowDTO)
        {
            return await _projectAllocationService.moveStatusSequenec(HttpContext, workspaceId, statusFlowDTO);
        }

        [HttpPut("ChangeStatusFlowOption/{workspaceId}/{statusOption}")]
        public async Task<ApiCommonResponse> ChangeStatusFlowOption(long workspaceId, string statusOption,List<StatusFlowDTO> statusFlowDTOs)
        {
            return await _projectAllocationService.updateStatusFlowOpton(HttpContext, workspaceId, statusOption, statusFlowDTOs);
        }


        [HttpDelete("DisableStatus/{workspaceId}/{statusId}")]
        public async Task<ApiCommonResponse> DisableStatus(long workspaceId, long statusId)
        {
            return await _projectAllocationService.disableStatus(workspaceId, statusId);
        }

        [HttpDelete("DisableWatcher/{projectId}/{projectWatcherId}")]
        public async Task<ApiCommonResponse> DisableWatcher(long projectId, long projectWatcherId)
        {
            return await _projectAllocationService.removeWatcher(HttpContext, projectId, projectWatcherId);
    
        }


        [HttpPost("CreateProject")]
        public async Task<ApiCommonResponse> CreateProject(ProjectDTO projectDTO)
        {
            return await _projectAllocationService.createProject(HttpContext, projectDTO);

        }

        [HttpPost("CreateTask/{projectId}")]
        public async Task<ApiCommonResponse> CreateTask(long projectId,TaskDTO taskDTO)
        {
            return await _projectAllocationService.createNewTask(HttpContext, projectId, taskDTO);

        }


        [HttpPost("AssignDeliverable/{deliverableId}/{taskId}/{deliverableAssigneeId}")]
        public async Task<ApiCommonResponse> AssignDeliverable(long taskId,long deliverableId,long deliverableAssigneeId,AssignDeliverableDTO assignDeliverableDTO)
        {
            return await _projectAllocationService.AssignDeliverable(HttpContext, taskId,deliverableId,deliverableAssigneeId,assignDeliverableDTO);

        }

        [HttpPost("createDeliverableIllustration/{deliverableId}/{taskId}")]
        public async Task<ApiCommonResponse> createDeliverableIllustration(long taskId, long deliverableId, List<IllustrationsDTO> illustrationsDTOs)
        {
            return await _projectAllocationService.createDeliverableIllustrattions(HttpContext, deliverableId, taskId, illustrationsDTOs);

        }

        [HttpDelete("removeIllustration/{taskId}/{deliverableId}/{illustrationId}")]
        public async Task<ApiCommonResponse> removeIllustration(long taskId, long deliverableId, long illustrationId)
        {
            return await _projectAllocationService.DeleteIllustration(HttpContext, taskId,deliverableId, illustrationId);

        }


        [HttpGet("GetTaskByCaption/{caption}")]

        public async Task<ApiCommonResponse> GetTaskByCaption(string caption)
        {
            return await _projectAllocationService.getTaskByCaption(HttpContext, caption);

        }

        [HttpGet("GetRequirementsByDeliverableId/{deliverableId}")]

        public async Task<ApiCommonResponse> GetRequirementsByDeliverableId(long deliverableId)
        {
            return await _projectAllocationService.getRequirementsByDeliverableId(HttpContext, deliverableId);

        }

        [HttpGet("GetAssignedTask")]

        public async Task<ApiCommonResponse> GetAssignedTask()
        {
            return await _projectAllocationService.getAssignedTask(HttpContext);

        }

        [HttpGet("GetAllProjects")]

        public async Task<ApiCommonResponse> GetAllProjects()
        {
            return await _projectAllocationService.getAllProjects(HttpContext);

        }

        [HttpGet("GetAllWorkspaceForProjectCreator")]

        public async Task<ApiCommonResponse> GetAllWorkspaceForProjectCreator()
        {
            return await _projectAllocationService.getWorkByProjectCreatorId(HttpContext);

        }

        [HttpGet("GetAllPickedTask")]

        public async Task<ApiCommonResponse> GetAllPickedTask()
        {
            return await _projectAllocationService.getAllPickedTask(HttpContext);

        }


        [HttpPost("CreateTaskIllustration/{taskId}")]

        public async Task<ApiCommonResponse> CreateTaskIllustration(List<IllustrationsDTO> illustrations,long taskId)
        {
            return await _projectAllocationService.createTaskIllustration( illustrations,taskId,HttpContext);

        }

        [HttpDelete("removeTaskIllustration/{taskId}/{illustrationId}")]
        public async Task<ApiCommonResponse> removeTaskIllustration(long taskId,long illustrationId)
        {
            return await _projectAllocationService.removeIllustrationById(taskId, illustrationId);

        }

        [HttpDelete("DisableTaskAssignee/{taskId}/{assigneeId}")]
        public async Task<ApiCommonResponse> DisableTaskAssignee(long taskId, long assigneeId)
        {
            return await _projectAllocationService.disableTaskAssignee(HttpContext,taskId, assigneeId);

        }

        [HttpDelete("DisableDeliverable/{taskId}/{deliverableId}")]
        public async Task<ApiCommonResponse> DisableDeliverable(long taskId, long deliverableId)
        {
            return await _projectAllocationService.disableDeliverable(HttpContext, taskId, deliverableId);

        }

        [HttpGet("GetAllTaskIllustrations/{taskId}")]

        public async Task<ApiCommonResponse> GetAllTaskIllustrations(long taskId)
        {
            return await _projectAllocationService.getTaskIllustrationById(taskId);

        }

        [HttpGet("GetAllTaskByProjectId/{projectId}")]

        public async Task<ApiCommonResponse> GetAllTaskByProjectId(long projectId)
        {
            return await _projectAllocationService.getTaskByProjectId(HttpContext, projectId);

        }


        [HttpGet("GetAllProjectByWorkspaceId/{workspaceId}")]

        public async Task<ApiCommonResponse> GetAllProjectByWorkspaceId(long workspaceId)
        {
            return await _projectAllocationService.getProjectByWorkspaceId(HttpContext, workspaceId);

        }

        [HttpGet("GetTaskByTaskId/{taskId}")]

        public async Task<ApiCommonResponse> GetTaskByTaskId(long taskId)
        {
            return await _projectAllocationService.getTaskById(HttpContext, taskId);

        }

        [HttpPut("UpdateTask/{taskId}")]
        public async Task<ApiCommonResponse> UpdateTask(long taskId, TaskDTO taskDTO)
        {
            return await _projectAllocationService.updateTask(HttpContext, taskId, taskDTO);

        }

        [HttpPut("PickUpTask/{taskId}")]
        public async Task<ApiCommonResponse> PickUpTask(long taskId)
        {
            return await _projectAllocationService.pickUptask(taskId,HttpContext);

        }

        [HttpPut("DropTask/{taskId}/{taskOwnershipId}")]
        public async Task<ApiCommonResponse> DropTask(long taskId, long taskOwnershipId)
        {
            return await _projectAllocationService.dropTask(taskId,taskOwnershipId, HttpContext);

        }

        [HttpPut("UpdateDeliverable/{taskId}/{deliverableId}")]
        public async Task<ApiCommonResponse> DropTask(long taskId, long deliverableId,DeliverableDTO deliverableDTO)
        {
            return await _projectAllocationService.updateDeliverable(HttpContext,taskId, deliverableId, deliverableDTO);

        }


        [HttpGet("GetAllTask")]

        public async Task<ApiCommonResponse> GetAllTask()
        {
            return await _projectAllocationService.getAllTask(HttpContext);

        }

        [HttpGet("GetProjectByName/{caption}")]

        public async Task<ApiCommonResponse> GetProjectByName(string caption)
        {
            return await _projectAllocationService.getProjectByProjectName(HttpContext,caption);

        }

        [HttpGet("GetAllDeliverable")]

        public async Task<ApiCommonResponse> GetAllDeliverable()
        {
            return await _projectAllocationService.getAllDeliverables(HttpContext);

        }

        [HttpGet("GetDeliverableByTaskId/{taskId}")]
        public async Task<ApiCommonResponse> GetDeliverableByTaskId(long taskId)
        {
            return await _projectAllocationService.getAllDeliverablesByTaskId(HttpContext, taskId);

        }

        [HttpGet("GetDeliverableById/{Id}")]
        public async Task<ApiCommonResponse> GetDeliverableById(long Id)
        {
            return await _projectAllocationService.getDeliverablesById(HttpContext, Id);

        }


        [HttpGet("GetPrivacyAccessByWorkspaceId/{workspaceId}")]
        public async Task<ApiCommonResponse> GetPrivacyAccessByWorkspaceId(long workspaceId)
        {
            return await _projectAllocationService.getAllPrivacyAccessByWorkspaceId(HttpContext, workspaceId);

        }

        [HttpGet("GetBarChart/{taskId}")]
        public async Task<ActionResult> GetBarChart(long taskId)
        {
            var response = await _projectAllocationService.getBarChartDetails(HttpContext, taskId);
            return Ok(response);
        }

        [HttpPost("PickDeliverable/{deliverableId}/")]
        public async Task<ActionResult> PickDeliverable(List<StatusFlow> statusFlows,long deliverableId)
        {
            var response = await _projectAllocationService.pickDeliverable(HttpContext, statusFlows,deliverableId);
            return Ok(response);
        }


        [HttpPost("AddComments/{deliverableId}/{deliverableAssigneeId}")]
        public async Task<ActionResult> AddComment(CommentsDTO comments, long deliverableId,long deliverableAssigneeId)
        {
            var response = await _projectAllocationService.addComments(HttpContext, deliverableId, deliverableAssigneeId,comments);
            return Ok(response);
        }



        [HttpPost("SelectStatus/{statusId}/{deliverableId}")]
        public async Task<ActionResult> PickDeliverable(long statusId, long deliverableId)
        {
            var response = await _projectAllocationService.selectStatus(HttpContext, statusId, deliverableId);
            return Ok(response);
        }

        //[HttpGet("GetDeliverableStatus")]
        //public async Task<ActionResult> GetDeliverableStatus()
        //{
        //    var response = await _projectAllocationService.getDeliverableStatus(HttpContext);
        //    return Ok(response);
        //}

        [HttpGet("GetWorkspaceStatus")]
        public async Task<ActionResult> GetWorkspaceStatus()
        {
            var response = await _projectAllocationService.getWorkspaceWithStatus(HttpContext);
            return Ok(response);
        }

        [HttpGet("GetAssignedDeliverableStatus/{workspaceId}")]
        public async Task<ActionResult> GetAssignedDeliverableStatus(long workspaceId)
        {
            var response = await _projectAllocationService.getCurrentDeliverableStatus(HttpContext,workspaceId);
            return Ok(response);
        }


        [HttpPost("CreateNewDeliverable/{taskId}")]
        public async Task<ApiCommonResponse> createNewDeliverable(long taskId,DeliverableDTO deliverableDTO)
        {
            return await _projectAllocationService.createNewDeliverable(HttpContext, taskId,deliverableDTO);

        }

        [HttpPost("moveToAnotherStatus/{statusId}/{deliverableId}/{statusCode}")]
        public async Task<ApiCommonResponse> createNewDeliverable(long statusId, long deliverableId,List<StatusFlow> statuses,int statusCode)
        {
            return await _projectAllocationService.moveToAnotherStatus(HttpContext, statuses,statusId,deliverableId,statusCode);

        }


        [HttpDelete("disableUploadedRequirement/{uploadedRequirementId}")]
        public async Task<ApiCommonResponse> createNewDeliverable(long uploadedRequirementId)
        {
            return await _projectAllocationService.disableRequirementUpload(HttpContext, uploadedRequirementId);

        }


        [HttpPost("UploadRequirementsDetails")]
        public async Task<ApiCommonResponse> CreateUploadedRequirement(UploadedRequirement uploadedRequirement)
        {
            return await _projectAllocationService.createUploadedRequirement(HttpContext,uploadedRequirement);

        }


        [HttpPost("CreateNewDeliverableOnTask/{taskId}")]
        public async Task<ApiCommonResponse> CreateNewDeliverableOnTask(long taskId, DeliverableDTO deliverableDTO)
        {
            return await _projectAllocationService.createNewDeliverableFromTask(HttpContext, taskId, deliverableDTO);

        }

        [HttpPost("AddmoreTaskAssignees/{taskId}")]
        public async Task<ApiCommonResponse> AddmoreTaskAssignees(long taskId, List<TaskAssigneeDTO> taskAssigneeDTO)
        {
            return await _projectAllocationService.addMoreTaskAssignees(HttpContext, taskId, taskAssigneeDTO);

        }




    }
}
