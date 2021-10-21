using HaloBiz.DTOs.ApiDTOs;
using HaloBiz.DTOs.ReceivingDTOs;
using HaloBiz.MyServices;
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
        public async Task<ActionResult> AddNewManager(ProjectAllocationRecievingDTO projectAllocationRecievingDTO)
        {
            var response = await _projectAllocationService.AddNewManager(HttpContext, projectAllocationRecievingDTO);
            if (response.StatusCode >= 400)
                return StatusCode(response.StatusCode, response);
            var projectManager = ((ApiOkResponse)response).Result;
            return Ok(projectManager);
        }


        [HttpGet("{id}/{email}")]
        public async Task<ActionResult> GetManagesFromProject(string email,int id)
        {
            var response = await _projectAllocationService.getManagersProjects(email,id);
            if (response.StatusCode >= 400)
                return StatusCode(response.StatusCode, response);
            var responseFeedback = ((ApiOkResponse)response).Result;
            return Ok(responseFeedback);
        }

        [HttpGet("{categoryId}")]
        public async Task<ActionResult> GetProjectFromManager(int categoryId)
        {
            var response = await _projectAllocationService.getProjectManagers(categoryId);
            if (response.StatusCode >= 400)
                return StatusCode(response.StatusCode, response);
            var responseFeedback = ((ApiOkResponse)response).Result;
            return Ok(responseFeedback);
        }

        [HttpDelete("{id}/{categoryId}/{projectId}")]
        public async Task<ActionResult> DetachFromCategory(int id,int categoryId,long projectId)
        {
            var response = await _projectAllocationService.removeFromCategory(id,categoryId,projectId);
            var responseFeedback = ((ApiOkResponse)response).Result;
            return StatusCode(response.StatusCode, responseFeedback);
        }
    }
}
