using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HaloBiz.DTOs.ApiDTOs;
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
        public async Task<ActionResult> GetTaskFulfillment()
        {
            var response = await _taskFulfillmentService.GetAllTaskFulfillment();
            if (response.StatusCode >= 400)
                return StatusCode(response.StatusCode, response);
            var taskFulfillment = ((ApiOkResponse)response).Result;
            return Ok(taskFulfillment);
        }

        [HttpGet("TaskOwnerPmWidget/{taskMasterId}")]
        public async Task<ActionResult> TaskOwnerPmWidget(long taskMasterId)
        {
            var response = await _taskFulfillmentService.GetPMWidgetStatistics(taskMasterId);
            if (response.StatusCode >= 400)
                return StatusCode(response.StatusCode, response);
            var taskFulfillment = ((ApiOkResponse)response).Result;
            return Ok(taskFulfillment);
        }

        [HttpGet("TaskDeliverableSummary/{userId}")]
        public async Task<ActionResult> GetTAskDeliverableSummaryForUser(long userId)
        {
            var response = await _taskFulfillmentService.GetTaskDeliverableSummary(userId);
            if (response.StatusCode >= 400)
                return StatusCode(response.StatusCode, response);
            var taskSummary = ((ApiOkResponse)response).Result;
            return Ok(taskSummary);
        }
        

        [HttpGet("UnCompletedTaskFulfillmentForTaskOwner/{taskOwnerId}")]
        public async Task<ActionResult> GetUnCompletedTaskFulfillmentForTaskMaster(long taskOwnerId)
        {
            var response = await _taskFulfillmentService. GetAllUnCompletedTaskFulfillmentForTaskOwner(taskOwnerId);
            if (response.StatusCode >= 400)
                return StatusCode(response.StatusCode, response);
            var taskFulfillment = ((ApiOkResponse)response).Result;
            return Ok(taskFulfillment);
        }

        [HttpGet("AllTaskFulfillmentForTaskOwner/{taskOwnerId}")]
        public async Task<ActionResult> GetAllTaskFulfillmentForTaskMaster(long taskOwnerId)
        {
            var response = await _taskFulfillmentService.GetAllTaskFulfillmentForTaskOwner(taskOwnerId);
            if (response.StatusCode >= 400)
                return StatusCode(response.StatusCode, response);
            var taskFulfillment = ((ApiOkResponse)response).Result;
            return Ok(taskFulfillment);
        }

        [HttpGet("caption/{name}")]
        public async Task<ActionResult> GetByCaption(string name)
        {
            var response = await _taskFulfillmentService.GetTaskFulfillmentByName(name);
            if (response.StatusCode >= 400)
                return StatusCode(response.StatusCode, response);
            var taskFulfillment = ((ApiOkResponse)response).Result;
            return Ok(taskFulfillment);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult> GetById(long id)
        {
            var response = await _taskFulfillmentService.GetTaskFulfillmentById(id);
            if (response.StatusCode >= 400)
                return StatusCode(response.StatusCode, response);
            var taskFulfillment = ((ApiOkResponse)response).Result;
            return Ok(taskFulfillment);
        }

        [HttpPost("")]
        public async Task<ActionResult> AddNewTaskFulfillment(TaskFulfillmentReceivingDTO taskFulfillmentReceiving)
        {
            var response = await _taskFulfillmentService.AddTaskFulfillment(HttpContext, taskFulfillmentReceiving);
            if (response.StatusCode >= 400)
                return StatusCode(response.StatusCode, response);
            var taskFulfillment = ((ApiOkResponse)response).Result;
            return Ok(taskFulfillment);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateById(long id, TaskFulfillmentReceivingDTO taskFulfillmentReceivingDTO)
        {
            var response = await _taskFulfillmentService.UpdateTaskFulfillment(HttpContext, id, taskFulfillmentReceivingDTO);
            if (response.StatusCode >= 400)
                return StatusCode(response.StatusCode, response);
            var taskFulfillment = ((ApiOkResponse)response).Result;
            return Ok(taskFulfillment);
        }

        /*[HttpDelete("{id}")]
        public async Task<ActionResult> DeleteById(int id)
        {
            var response = await _taskFulfillmentService.DeleteTaskFulfillment(id);
            return StatusCode(response.StatusCode);
        }*/

        [HttpPut("SetIsPicked/{id}")]
        public async Task<IActionResult> SetIsPicked(long id)
        {
            var response = await _taskFulfillmentService.SetIsPicked(HttpContext, id, true);
            if (response.StatusCode >= 400)
                return StatusCode(response.StatusCode, response);
            var taskFulfillment = ((ApiOkResponse)response).Result;
            return Ok(taskFulfillment);
        }

        [HttpPut("DropPicked/{id}")]
        public async Task<IActionResult> DropPickedTask(long id)
        {
            var response = await _taskFulfillmentService.SetIsPicked(HttpContext, id, false);
            if (response.StatusCode >= 400)
                return StatusCode(response.StatusCode, response);
            var taskFulfillment = ((ApiOkResponse)response).Result;
            return Ok(taskFulfillment);
        }

        [HttpGet("GetTaskFulfillmentsByOperatingEntityHeadId/{id}")]
        public async Task<ActionResult> GetTaskFulfillmentsByOperatingEntityHeadId(long id)
        {
            var response = await _taskFulfillmentService.GetTaskFulfillmentsByOperatingEntityHeadId(id);
            if (response.StatusCode >= 400)
                return StatusCode(response.StatusCode, response);
            var taskFulfillment = ((ApiOkResponse)response).Result;
            return Ok(taskFulfillment);
        }

        [HttpGet("GetTaskFulfillmentDetails/{id}")]
        public async Task<ActionResult> GetTaskFulfillmentDetails(long id)
        {
            var response = await _taskFulfillmentService.GetTaskFulfillmentDetails(id);
            if (response.StatusCode >= 400)
                return StatusCode(response.StatusCode, response);
            var taskFulfillment = ((ApiOkResponse)response).Result;
            return Ok(taskFulfillment);
        }
    }
}