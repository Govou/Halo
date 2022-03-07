using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Halobiz.Common.DTOs.ApiDTOs;
using HaloBiz.DTOs.ReceivingDTOs;
using HaloBiz.DTOs.TransferDTOs.LAMS;
using HaloBiz.MyServices.LAMS;
using Microsoft.AspNetCore.Mvc;
//using Controllers.Models;

namespace Controllers.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class TaskFulfillmentController : ControllerBase
    {
        private readonly ITaskFulfillmentService _taskFulfillmentService;

        public TaskFulfillmentController(ITaskFulfillmentService taskFulfillmentService)
        {
            this._taskFulfillmentService = taskFulfillmentService;
        }

        [HttpGet("")]
        public async Task<ApiCommonResponse> GetTaskFulfillment()
        {
            return await _taskFulfillmentService.GetAllTaskFulfillment();
        }

        [HttpGet("TaskOwnerPmWidget/{taskMasterId}")]
        public async Task<ApiCommonResponse> TaskOwnerPmWidget(long taskMasterId)
        {
            return await _taskFulfillmentService.GetPMWidgetStatistics(taskMasterId);
        }

        [HttpGet("TaskDeliverableSummary/{userId}")]
        public async Task<ApiCommonResponse> GetTAskDeliverableSummaryForUser(long userId)
        {
            return await _taskFulfillmentService.GetTaskDeliverableSummary(userId);
        }
        

        [HttpGet("UnCompletedTaskFulfillmentForTaskOwner/{taskOwnerId}")]
        public async Task<ApiCommonResponse> GetUnCompletedTaskFulfillmentForTaskMaster(long taskOwnerId)
        {
            return await _taskFulfillmentService. GetAllUnCompletedTaskFulfillmentForTaskOwner(taskOwnerId);
        }

        [HttpGet("AllTaskFulfillmentForTaskOwner/{taskOwnerId}")]
        public async Task<ApiCommonResponse> GetAllTaskFulfillmentForTaskMaster(long taskOwnerId)
        {
            return await _taskFulfillmentService.GetAllTaskFulfillmentForTaskOwner(taskOwnerId);
        }

        [HttpGet("caption/{name}")]
        public async Task<ApiCommonResponse> GetByCaption(string name)
        {
            return await _taskFulfillmentService.GetTaskFulfillmentByName(name);
        }

        [HttpGet("{id}")]
        public async Task<ApiCommonResponse> GetById(long id)
        {
            return await _taskFulfillmentService.GetTaskFulfillmentById(id);
        }

        [HttpPost("")]
        public async Task<ApiCommonResponse> AddNewTaskFulfillment(TaskFulfillmentReceivingDTO taskFulfillmentReceiving)
        {
            return await _taskFulfillmentService.AddTaskFulfillment(HttpContext, taskFulfillmentReceiving);
        }

        [HttpPut("{id}")]
        public async Task<ApiCommonResponse> UpdateById(long id, TaskFulfillmentReceivingDTO taskFulfillmentReceivingDTO)
        {
            return await _taskFulfillmentService.UpdateTaskFulfillment(HttpContext, id, taskFulfillmentReceivingDTO);
        }

        /*[HttpDelete("{id}")]
        public async Task<ApiCommonResponse> DeleteById(int id)
        {
            return await _taskFulfillmentService.DeleteTaskFulfillment(id);
            return StatusCode(response.StatusCode);
        }*/

        [HttpPut("SetIsPicked/{id}")]
        public async Task<ApiCommonResponse> SetIsPicked(long id)
        {
            return await _taskFulfillmentService.SetIsPicked(HttpContext, id, true);
        }

        [HttpPut("DropPicked/{id}")]
        public async Task<ApiCommonResponse> DropPickedTask(long id)
        {
            return await _taskFulfillmentService.SetIsPicked(HttpContext, id, false);
        }

        [HttpGet("GetTaskFulfillmentsByOperatingEntityHeadId/{id}")]
        public async Task<ApiCommonResponse> GetTaskFulfillmentsByOperatingEntityHeadId(long id)
        {
            return await _taskFulfillmentService.GetTaskFulfillmentsByOperatingEntityHeadId(id);
        }

        [HttpGet("GetTaskFulfillmentDetails/{id}")]
        public async Task<ApiCommonResponse> GetTaskFulfillmentDetails(long id)
        {
            return await _taskFulfillmentService.GetTaskFulfillmentDetails(id);
        }
    }
}