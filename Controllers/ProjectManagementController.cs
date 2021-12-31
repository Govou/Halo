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

        public async Task<ActionResult> GetWorkspaceByProjectCreator()
        {
            var response = await _projectAllocationService.getWorkByProjectCreatorId(HttpContext);
            return Ok(response);
        }

        [HttpGet("GetWatchersByProjectId/{projectId}")]

        public async Task<ActionResult> GetWatchersByProjectId(long projectId)
        {
            var response = await _projectAllocationService.getWatchersByProjectId(HttpContext,projectId);
            return Ok(response);
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
        public async Task<ActionResult> updateProject(long projectId, ProjectDTO projectDTO)
        {
            var response = await _projectAllocationService.updateProject(HttpContext, projectId, projectDTO);
            return Ok(response);
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
        public async Task<ActionResult> AddmoreWatchers(long projectId,  List<WatchersDTO> watchersDTOs)
        {
            var response = await _projectAllocationService.addmoreWatchers(HttpContext, projectId, watchersDTOs);
            return Ok(response);
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
        public async Task<ActionResult> DisableWatcher(long projectId, long projectWatcherId)
        {
            var response = await _projectAllocationService.removeWatcher(HttpContext, projectId, projectWatcherId);
            return Ok(response);
        }


        [HttpPost("CreateProject")]
        public async Task<ActionResult> CreateProject(ProjectDTO projectDTO)
        {
            var response = await _projectAllocationService.createProject(HttpContext, projectDTO);
            return Ok(response);
        }

        [HttpPost("CreateTask/{projectId}")]
        public async Task<ActionResult> CreateTask(long projectId,TaskDTO taskDTO)
        {
            var response = await _projectAllocationService.createNewTask(HttpContext, projectId, taskDTO);
            return Ok(response);
        }


        [HttpPost("AssignDeliverable/{deliverableId}/{taskId}/{deliverableAssigneeId}")]
        public async Task<ActionResult> AssignDeliverable(long taskId,long deliverableId,long deliverableAssigneeId,AssignDeliverableDTO assignDeliverableDTO)
        {
            var response = await _projectAllocationService.AssignDeliverable(HttpContext, taskId,deliverableId,deliverableAssigneeId,assignDeliverableDTO);
            return Ok(response);
        }

        [HttpPost("createDeliverableIllustration/{deliverableId}/{taskId}")]
        public async Task<ActionResult> createDeliverableIllustration(long taskId, long deliverableId, List<IllustrationsDTO> illustrationsDTOs)
        {
            var response = await _projectAllocationService.createDeliverableIllustrattions(HttpContext, deliverableId, taskId, illustrationsDTOs);
            return Ok(response);
        }

        [HttpDelete("removeIllustration/{taskId}/{deliverableId}/{illustrationId}")]
        public async Task<ActionResult> removeIllustration(long taskId, long deliverableId, long illustrationId)
        {
            var response = await _projectAllocationService.DeleteIllustration(HttpContext, taskId,deliverableId, illustrationId);
            return Ok(response);
        }


        [HttpGet("GetTaskByCaption/{caption}")]

        public async Task<ActionResult> GetTaskByCaption(string caption)
        {
            var response = await _projectAllocationService.getTaskByCaption(HttpContext, caption);
            return Ok(response);
        }

        [HttpGet("GetRequirementsByDeliverableId/{deliverableId}")]

        public async Task<ActionResult> GetRequirementsByDeliverableId(long deliverableId)
        {
            var response = await _projectAllocationService.getRequirementsByDeliverableId(HttpContext, deliverableId);
            return Ok(response);
        }

        [HttpGet("GetAssignedTask")]

        public async Task<ActionResult> GetAssignedTask()
        {
            var response = await _projectAllocationService.getAssignedTask(HttpContext);
            return Ok(response);
        }

        [HttpGet("GetAllProjects")]

        public async Task<ActionResult> GetAllProjects()
        {
            var response = await _projectAllocationService.getAllProjects(HttpContext);
            return Ok(response);
        }

        [HttpGet("GetAllWorkspaceForProjectCreator")]

        public async Task<ActionResult> GetAllWorkspaceForProjectCreator()
        {
            var response = await _projectAllocationService.getWorkByProjectCreatorId(HttpContext);
            return Ok(response);
        }

        [HttpGet("GetAllPickedTask")]

        public async Task<ActionResult> GetAllPickedTask()
        {
            var response = await _projectAllocationService.getAllPickedTask(HttpContext);
            return Ok(response);
        }


        [HttpPost("CreateTaskIllustration/{taskId}")]

        public async Task<ActionResult> CreateTaskIllustration(List<IllustrationsDTO> illustrations,long taskId)
        {
            var response = await _projectAllocationService.createTaskIllustration( illustrations,taskId,HttpContext);
            return Ok(response);
        }

        [HttpDelete("removeTaskIllustration/{taskId}/{illustrationId}")]
        public async Task<ActionResult> removeTaskIllustration(long taskId,long illustrationId)
        {
            var response = await _projectAllocationService.removeIllustrationById(taskId, illustrationId);
            return Ok(response);
        }

        [HttpDelete("DisableTaskAssignee/{taskId}/{assigneeId}")]
        public async Task<ActionResult> DisableTaskAssignee(long taskId, long assigneeId)
        {
            var response = await _projectAllocationService.disableTaskAssignee(HttpContext,taskId, assigneeId);
            return Ok(response);
        }

        [HttpDelete("DisableDeliverable/{taskId}/{deliverableId}")]
        public async Task<ActionResult> DisableDeliverable(long taskId, long deliverableId)
        {
            var response = await _projectAllocationService.disableDeliverable(HttpContext, taskId, deliverableId);
            return Ok(response);
        }

        [HttpGet("GetAllTaskIllustrations/{taskId}")]

        public async Task<ActionResult> GetAllTaskIllustrations(long taskId)
        {
            var response = await _projectAllocationService.getTaskIllustrationById(taskId);
            return Ok(response);
        }

        [HttpGet("GetAllTaskByProjectId/{projectId}")]

        public async Task<ActionResult> GetAllTaskByProjectId(long projectId)
        {
            var response = await _projectAllocationService.getTaskByProjectId(HttpContext, projectId);
            return Ok(response);
        }


        [HttpGet("GetAllProjectByWorkspaceId/{workspaceId}")]

        public async Task<ActionResult> GetAllProjectByWorkspaceId(long workspaceId)
        {
            var response = await _projectAllocationService.getProjectByWorkspaceId(HttpContext, workspaceId);
            return Ok(response);
        }

        [HttpGet("GetTaskByTaskId/{taskId}")]

        public async Task<ActionResult> GetTaskByTaskId(long taskId)
        {
            var response = await _projectAllocationService.getTaskById(HttpContext, taskId);
            return Ok(response);
        }

        [HttpPut("UpdateTask/{taskId}")]
        public async Task<ActionResult> UpdateTask(long taskId, TaskDTO taskDTO)
        {
            var response = await _projectAllocationService.updateTask(HttpContext, taskId, taskDTO);
            return Ok(response);
        }

        [HttpPut("PickUpTask/{taskId}")]
        public async Task<ActionResult> PickUpTask(long taskId)
        {
            var response = await _projectAllocationService.pickUptask(taskId,HttpContext);
            return Ok(response);
        }

        [HttpPut("DropTask/{taskId}/{taskOwnershipId}")]
        public async Task<ActionResult> DropTask(long taskId, long taskOwnershipId)
        {
            var response = await _projectAllocationService.dropTask(taskId,taskOwnershipId, HttpContext);
            return Ok(response);
        }

        [HttpPut("UpdateDeliverable/{taskId}/{deliverableId}")]
        public async Task<ActionResult> DropTask(long taskId, long deliverableId,DeliverableDTO deliverableDTO)
        {
            var response = await _projectAllocationService.updateDeliverable(HttpContext,taskId, deliverableId, deliverableDTO);
            return Ok(response);
        }


        [HttpGet("GetAllTask")]

        public async Task<ActionResult> GetAllTask()
        {
            var response = await _projectAllocationService.getAllTask(HttpContext);
            return Ok(response);
        }

        [HttpGet("GetProjectByName/{caption}")]

        public async Task<ActionResult> GetProjectByName(string caption)
        {
            var response = await _projectAllocationService.getProjectByProjectName(HttpContext,caption);
            return Ok(response);
        }

        [HttpGet("GetAllDeliverable")]

        public async Task<ActionResult> GetAllDeliverable()
        {
            var response = await _projectAllocationService.getAllDeliverables(HttpContext);
            return Ok(response);
        }

        [HttpGet("GetDeliverableByTaskId/{taskId}")]
        public async Task<ActionResult> GetDeliverableByTaskId(long taskId)
        {
            var response = await _projectAllocationService.getAllDeliverablesByTaskId(HttpContext, taskId);
            return Ok(response);
        }

        [HttpGet("GetDeliverableById/{Id}")]
        public async Task<ActionResult> GetDeliverableById(long Id)
        {
            var response = await _projectAllocationService.getDeliverablesById(HttpContext, Id);
            return Ok(response);
        }


        [HttpGet("GetPrivacyAccessByWorkspaceId/{workspaceId}")]
        public async Task<ActionResult> GetPrivacyAccessByWorkspaceId(long workspaceId)
        {
            var response = await _projectAllocationService.getAllPrivacyAccessByWorkspaceId(HttpContext, workspaceId);
            return Ok(response);
        }

        [HttpPost("CreateNewDeliverable/{taskId}")]
        public async Task<ActionResult> createNewDeliverable(long taskId,DeliverableDTO deliverableDTO)
        {
            var response = await _projectAllocationService.createNewDeliverable(HttpContext, taskId,deliverableDTO);
            return Ok(response);
        }

        [HttpPost("CreateNewDeliverableOnTask/{taskId}")]
        public async Task<ActionResult> CreateNewDeliverableOnTask(long taskId, DeliverableDTO deliverableDTO)
        {
            var response = await _projectAllocationService.createNewDeliverableFromTask(HttpContext, taskId, deliverableDTO);
            return Ok(response);
        }

        [HttpPost("AddmoreTaskAssignees/{taskId}")]
        public async Task<ActionResult> AddmoreTaskAssignees(long taskId, List<TaskAssigneeDTO> taskAssigneeDTO)
        {
            var response = await _projectAllocationService.addMoreTaskAssignees(HttpContext, taskId, taskAssigneeDTO);
            return Ok(response);
        }




    }
}
