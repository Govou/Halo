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
            var response = await _projectAllocationService.AddNewManager(HttpContext, projectAllocationRecievingDTO);
            if (response.StatusCode >= 400)
                return StatusCode(response.StatusCode, response);
            var projectManager = ((ApiOkResponse)response).Result;
            return Ok(projectManager);
        }


        [HttpGet("{id}/{email}")]
        public async Task<ApiCommonResponse> GetManagersAttachedToServiceCategory(string email,int id)
        {
            var response = await _projectAllocationService.getManagersProjects(email,id);
            if (response.StatusCode >= 400)
                return StatusCode(response.StatusCode, response);
            var responseFeedback = ((ApiOkResponse)response).Result;
            return Ok(responseFeedback);
        }

        [HttpGet("{categoryId}")]
        public async Task<ApiCommonResponse> GetServiceCategoryAttachedToASingleManager(int categoryId)
        {
            var response = await _projectAllocationService.getProjectManagers(categoryId);
            if (response.StatusCode >= 400)
                return StatusCode(response.StatusCode, response);
            var responseFeedback = ((ApiOkResponse)response).Result;
            return Ok(responseFeedback);
        }

        [HttpDelete("{id}/{categoryId}/{projectId}")]
        public async Task<ApiCommonResponse> DetachManagerFromServiceCategory(int id,int categoryId,long projectId)
        {
            var response = await _projectAllocationService.removeFromCategory(id,categoryId,projectId);
            var responseFeedback = ((ApiOkResponse)response).Result;
            return StatusCode(response.StatusCode, responseFeedback);
        }

        [HttpPost("workspace")]
        public async Task<ApiCommonResponse> CreateWorkspace(WorkspaceDTO workspaceDTO)
        {
            var response = await _projectAllocationService.CreateNewWorkspace(HttpContext, workspaceDTO);
            return Ok(response);
        }


        [HttpPost("defaultStatus")]
        public async Task<ApiCommonResponse> CreateDefaultStatus(List<DefaultStatusDTO> defaultStatusFlow)
        {
            var response = await _projectAllocationService.createDefaultStatus(HttpContext, defaultStatusFlow);
            return Ok(response);
        }


        [HttpGet("GetAllWorkspaces")]

        public async Task<ApiCommonResponse> GetAllWorkspaces()
        {
            var response = await _projectAllocationService.getAllWorkspaces(HttpContext);
            return Ok(response);
        }

        [HttpGet("GetWorkspaceById/{id}")]

        public async Task<ApiCommonResponse> GetWorkspaceById(long id)
        {
            var response = await _projectAllocationService.getWorkspaceById(id);
            return Ok(response);
        }

        [HttpGet("GetWorkspaceByCaption/{caption}")]

        public async Task<ApiCommonResponse> GetWorkspaceByCaption(string caption)
        {
            var response = await _projectAllocationService.getWorkspaceByCaption(caption);
            return Ok(response);
        }


        [HttpGet("GetAllProjectManagers")]

        public async Task<ApiCommonResponse> GetAllProjectManagers()
        {
            var response = await _projectAllocationService.getAllProjectManagers();
            return Ok(response);
        }

        [HttpGet("GetDefaultStatus")]

        public async Task<ApiCommonResponse> GetDefaultStatus()
        {
            var response = await _projectAllocationService.getAllDefaultStatus();
            return Ok(response);
        }




        [HttpDelete("workspace/{id}")]
        public async Task<ApiCommonResponse> DisableWorkspace(int id)
        {
            var response = await _projectAllocationService.disableWorkspace(id);
            var responseFeedback = ((ApiOkResponse)response).Result;
            return StatusCode(response.StatusCode, responseFeedback);
        }

        [HttpPut("workspace/{id}")]
        public async Task<ApiCommonResponse> UpdateWorkspace(long id,UpdateWorkspaceDTO workspaceDTO)
        {
            var response = await _projectAllocationService.updateWorkspace(HttpContext, id,workspaceDTO);
            var responseFeedback = ((ApiOkResponse)response).Result;
            return StatusCode(response.StatusCode, responseFeedback);
        }

        [HttpPut("addMoreProjectCreators/{id}")]
        public async Task<ApiCommonResponse> AddMoreProjectCreators(long id, List<AddMoreUserDto> workspaceDTO)
        {
            var response = await _projectAllocationService.addMoreProjectCreators(HttpContext, id, workspaceDTO);
            var responseFeedback = ((ApiOkResponse)response).Result;
            return StatusCode(response.StatusCode, responseFeedback);
        }


        [HttpDelete("RemoveProjectCreator/{workspaceId}/{creatorId}")]
        public async Task<ApiCommonResponse> RemoveProjectCreator(long workspaceId,long creatorId)
        {
            var response = await _projectAllocationService.removeFromProjectCreator(workspaceId, creatorId);
            var responseFeedback = ((ApiOkResponse)response).Result;
            return StatusCode(response.StatusCode, responseFeedback);
        }


        [HttpPut("UpdateToPublic/{workspaceId}")]
        public async Task<ApiCommonResponse> UpdateToPublic(long workspaceId)
        {
            var response = await _projectAllocationService.updateToPublic(workspaceId);
            var responseFeedback = ((ApiOkResponse)response).Result;
            return StatusCode(response.StatusCode, responseFeedback);
        }

        [HttpDelete("DisablePrivateUser/{workspaceId}/{privateUserId}")]
        public async Task<ApiCommonResponse> DisablePrivateUser(long workspaceId, long privateUserId)
        {
            var response = await _projectAllocationService.disablePrivateUser(workspaceId, privateUserId);
            var responseFeedback = ((ApiOkResponse)response).Result;
            return StatusCode(response.StatusCode, responseFeedback);
        }


        [HttpPut("addMorePrivateUsers/{workspaceId}")]
        public async Task<ApiCommonResponse> AddMorePrivateUsers(long workspaceId, List<AddMoreUserDto>  addMoreUserDtos )
        {
            var response = await _projectAllocationService.addMorePrivateUser(HttpContext, workspaceId, addMoreUserDtos);
            var responseFeedback = ((ApiOkResponse)response).Result;
            return StatusCode(response.StatusCode, responseFeedback);
        }

        [HttpPut("UpdateStatus/{workspaceId}/{statusFlowId}")]
        public async Task<ApiCommonResponse> updateStatus(long workspaceId,long statusFlowId, StatusFlowDTO statusFlowDTO)
        {
            var response = await _projectAllocationService.updateStatus(HttpContext, workspaceId, statusFlowId,statusFlowDTO);
            var responseFeedback = ((ApiOkResponse)response).Result;
            return StatusCode(response.StatusCode, responseFeedback);
        }

        [HttpPut("AddMoreStatus/{workspaceId}")]
        public async Task<ApiCommonResponse> addMoreStatus(long workspaceId, List<StatusFlowDTO> statusFlowDTO)
        {
            var response = await _projectAllocationService.addmoreStatus(HttpContext, workspaceId, statusFlowDTO);
            var responseFeedback = ((ApiOkResponse)response).Result;
            return StatusCode(response.StatusCode, responseFeedback);
        }

        [HttpPut("MoveSequence/{workspaceId}")]
        public async Task<ApiCommonResponse> MoveSequence(long workspaceId, List<StatusFlowDTO> statusFlowDTO)
        {
            var response = await _projectAllocationService.moveStatusSequenec(HttpContext, workspaceId, statusFlowDTO);
            var responseFeedback = ((ApiOkResponse)response).Result;
            return StatusCode(response.StatusCode, responseFeedback);
        }

        [HttpPut("ChangeStatusFlowOption/{workspaceId}/{statusOption}")]
        public async Task<ApiCommonResponse> ChangeStatusFlowOption(long workspaceId, string statusOption,List<StatusFlowDTO> statusFlowDTOs)
        {
            var response = await _projectAllocationService.updateStatusFlowOpton(HttpContext, workspaceId, statusOption, statusFlowDTOs);
            var responseFeedback = ((ApiOkResponse)response).Result;
            return StatusCode(response.StatusCode, responseFeedback);
        }


        [HttpDelete("DisableStatus/{workspaceId}/{statusId}")]
        public async Task<ApiCommonResponse> DisableStatus(long workspaceId, long statusId)
        {
            var response = await _projectAllocationService.disableStatus(workspaceId, statusId);
            var responseFeedback = ((ApiOkResponse)response).Result;
            return StatusCode(response.StatusCode, responseFeedback);
        }



    }
}
