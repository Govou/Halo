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



    }
}
