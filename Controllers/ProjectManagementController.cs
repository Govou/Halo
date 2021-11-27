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
        public async Task<ActionResult> AttachManagerToServiceCategory(ProjectAllocationRecievingDTO projectAllocationRecievingDTO)
        {
            var response = await _projectAllocationService.AddNewManager(HttpContext, projectAllocationRecievingDTO);
            if (response.StatusCode >= 400)
                return StatusCode(response.StatusCode, response);
            var projectManager = ((ApiOkResponse)response).Result;
            return Ok(projectManager);
        }


        [HttpGet("{id}/{email}")]
        public async Task<ActionResult> GetManagersAttachedToServiceCategory(string email,int id)
        {
            var response = await _projectAllocationService.getManagersProjects(email,id);
            if (response.StatusCode >= 400)
                return StatusCode(response.StatusCode, response);
            var responseFeedback = ((ApiOkResponse)response).Result;
            return Ok(responseFeedback);
        }

        [HttpGet("{categoryId}")]
        public async Task<ActionResult> GetServiceCategoryAttachedToASingleManager(int categoryId)
        {
            var response = await _projectAllocationService.getProjectManagers(categoryId);
            if (response.StatusCode >= 400)
                return StatusCode(response.StatusCode, response);
            var responseFeedback = ((ApiOkResponse)response).Result;
            return Ok(responseFeedback);
        }

        [HttpDelete("{id}/{categoryId}/{projectId}")]
        public async Task<ActionResult> DetachManagerFromServiceCategory(int id,int categoryId,long projectId)
        {
            var response = await _projectAllocationService.removeFromCategory(id,categoryId,projectId);
            var responseFeedback = ((ApiOkResponse)response).Result;
            return StatusCode(response.StatusCode, responseFeedback);
        }

        [HttpPost("workspace")]
        public async Task<ActionResult> CreateWorkspace(WorkspaceDTO workspaceDTO)
        {
            var response = await _projectAllocationService.CreateNewWorkspace(HttpContext, workspaceDTO);
            return Ok(response);
        }


        [HttpPost("defaultStatus")]
        public async Task<ActionResult> CreateDefaultStatus(List<DefaultStatusDTO> defaultStatusFlow)
        {
            var response = await _projectAllocationService.createDefaultStatus(HttpContext, defaultStatusFlow);
            return Ok(response);
        }


        [HttpGet("GetAllWorkspaces")]

        public async Task<ActionResult> GetAllWorkspaces()
        {
            var response = await _projectAllocationService.getAllWorkspaces(HttpContext);
            return Ok(response);
        }

        [HttpGet("GetWorkspaceById/{id}")]

        public async Task<ActionResult> GetWorkspaceById(long id)
        {
            var response = await _projectAllocationService.getWorkspaceById(id);
            return Ok(response);
        }

        [HttpGet("GetWorkspaceByCaption/{caption}")]

        public async Task<ActionResult> GetWorkspaceByCaption(string caption)
        {
            var response = await _projectAllocationService.getWorkspaceByCaption(caption);
            return Ok(response);
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

        public async Task<ActionResult> GetAllProjectManagers()
        {
            var response = await _projectAllocationService.getAllProjectManagers();
            return Ok(response);
        }

        [HttpGet("GetDefaultStatus")]

        public async Task<ActionResult> GetDefaultStatus()
        {
            var response = await _projectAllocationService.getAllDefaultStatus();
            return Ok(response);
        }




        [HttpDelete("workspace/{id}")]
        public async Task<ActionResult> DisableWorkspace(int id)
        {
            var response = await _projectAllocationService.disableWorkspace(id);
            var responseFeedback = ((ApiOkResponse)response).Result;
            return StatusCode(response.StatusCode, responseFeedback);
        }

        [HttpPut("workspace/{id}")]
        public async Task<ActionResult> UpdateWorkspace(long id,UpdateWorkspaceDTO workspaceDTO)
        {
            var response = await _projectAllocationService.updateWorkspace(HttpContext, id,workspaceDTO);
            return Ok(response);
        }

        [HttpPut("addMoreProjectCreators/{id}")]
        public async Task<ActionResult> AddMoreProjectCreators(long id, List<AddMoreUserDto> workspaceDTO)
        {
            var response = await _projectAllocationService.addMoreProjectCreators(HttpContext, id, workspaceDTO);
            var responseFeedback = ((ApiOkResponse)response).Result;
            return StatusCode(response.StatusCode, responseFeedback);
        }

        [HttpPut("updateProject/{projectId}")]
        public async Task<ActionResult> updateProject(long projectId, ProjectDTO projectDTO)
        {
            var response = await _projectAllocationService.updateProject(HttpContext, projectId, projectDTO);
            return Ok(response);
        }


        [HttpDelete("RemoveProjectCreator/{workspaceId}/{creatorId}")]
        public async Task<ActionResult> RemoveProjectCreator(long workspaceId,long creatorId)
        {
            var response = await _projectAllocationService.removeFromProjectCreator(workspaceId, creatorId);
            var responseFeedback = ((ApiOkResponse)response).Result;
            return StatusCode(response.StatusCode, responseFeedback);
        }


        [HttpPut("UpdateToPublic/{workspaceId}")]
        public async Task<ActionResult> UpdateToPublic(long workspaceId)
        {
            var response = await _projectAllocationService.updateToPublic(workspaceId);
            var responseFeedback = ((ApiOkResponse)response).Result;
            return StatusCode(response.StatusCode, responseFeedback);
        }

        [HttpDelete("DisablePrivateUser/{workspaceId}/{privateUserId}")]
        public async Task<ActionResult> DisablePrivateUser(long workspaceId, long privateUserId)
        {
            var response = await _projectAllocationService.disablePrivateUser(workspaceId, privateUserId);
            var responseFeedback = ((ApiOkResponse)response).Result;
            return StatusCode(response.StatusCode, responseFeedback);
        }


        [HttpPut("addMorePrivateUsers/{workspaceId}")]
        public async Task<ActionResult> AddMorePrivateUsers(long workspaceId, List<AddMoreUserDto>  addMoreUserDtos )
        {
            var response = await _projectAllocationService.addMorePrivateUser(HttpContext, workspaceId, addMoreUserDtos);
            var responseFeedback = ((ApiOkResponse)response).Result;
            return StatusCode(response.StatusCode, responseFeedback);
        }

        [HttpPut("UpdateStatus/{workspaceId}/{statusFlowId}")]
        public async Task<ActionResult> updateStatus(long workspaceId,long statusFlowId, StatusFlowDTO statusFlowDTO)
        {
            var response = await _projectAllocationService.updateStatus(HttpContext, workspaceId, statusFlowId,statusFlowDTO);
            var responseFeedback = ((ApiOkResponse)response).Result;
            return StatusCode(response.StatusCode, responseFeedback);
        }

        [HttpPut("AddmoreWatchers/{projectId}")]
        public async Task<ActionResult> AddmoreWatchers(long projectId,  List<WatchersDTO> watchersDTOs)
        {
            var response = await _projectAllocationService.addmoreWatchers(HttpContext, projectId, watchersDTOs);
            return Ok(response);
        }


        [HttpPut("AddMoreStatus/{workspaceId}")]
        public async Task<ActionResult> addMoreStatus(long workspaceId, List<StatusFlowDTO> statusFlowDTO)
        {
            var response = await _projectAllocationService.addmoreStatus(HttpContext, workspaceId, statusFlowDTO);
            var responseFeedback = ((ApiOkResponse)response).Result;
            return StatusCode(response.StatusCode, responseFeedback);
        }

        [HttpPut("MoveSequence/{workspaceId}")]
        public async Task<ActionResult> MoveSequence(long workspaceId, List<StatusFlowDTO> statusFlowDTO)
        {
            var response = await _projectAllocationService.moveStatusSequenec(HttpContext, workspaceId, statusFlowDTO);
            var responseFeedback = ((ApiOkResponse)response).Result;
            return StatusCode(response.StatusCode, responseFeedback);
        }

        [HttpPut("ChangeStatusFlowOption/{workspaceId}/{statusOption}")]
        public async Task<ActionResult> ChangeStatusFlowOption(long workspaceId, string statusOption,List<StatusFlowDTO> statusFlowDTOs)
        {
            var response = await _projectAllocationService.updateStatusFlowOpton(HttpContext, workspaceId, statusOption, statusFlowDTOs);
            var responseFeedback = ((ApiOkResponse)response).Result;
            return StatusCode(response.StatusCode, responseFeedback);
        }


        [HttpDelete("DisableStatus/{workspaceId}/{statusId}")]
        public async Task<ActionResult> DisableStatus(long workspaceId, long statusId)
        {
            var response = await _projectAllocationService.disableStatus(workspaceId, statusId);
            var responseFeedback = ((ApiOkResponse)response).Result;
            return StatusCode(response.StatusCode, responseFeedback);
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



        //[HttpGet("GetProjectByWorkspaceId/{workspaceId}")]

        //public async Task<ActionResult> GetProjectByWorkspaceId(long workspaceId)
        //{
        //    var response = await _projectAllocationService.getAllDefaultStatus(workspaceId);
        //    return Ok(response);
        //}

        [HttpGet("GetTaskByCaption/{caption}")]

        public async Task<ActionResult> GetTaskByCaption(string caption)
        {
            var response = await _projectAllocationService.getTaskByCaption(HttpContext, caption);
            return Ok(response);
        }


        [HttpGet("GetAllProjects")]

        public async Task<ActionResult> GetAllProjects()
        {
            var response = await _projectAllocationService.getAllProjects(HttpContext);
            return Ok(response);
        }

        [HttpGet("GetAllTaskByProjectId/{projectId}")]

        public async Task<ActionResult> GetAllTaskByProjectId(long projectId)
        {
            var response = await _projectAllocationService.getTaskByProjectId(HttpContext, projectId);
            return Ok(response);
        }


        [HttpGet("GetAllProjectByWorkspaceId/{workspaceId}")]

        public async Task<ActionResult> GetAllProjectByWorkspaceId(long worksppaceId)
        {
            var response = await _projectAllocationService.getProjectByWorkspaceId(HttpContext, worksppaceId);
            return Ok(response);
        }

        [HttpGet("GetTaskByTaskId/{taskId}")]

        public async Task<ActionResult> GetTaskByTaskId(long taskId)
        {
            var response = await _projectAllocationService.getTaskById(HttpContext, taskId);
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




    }
}
