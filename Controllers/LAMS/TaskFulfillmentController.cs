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
            var response = await _taskFulfillmentService.SetIsPicked(HttpContext, id);
            if (response.StatusCode >= 400)
                return StatusCode(response.StatusCode, response);
            var taskFulfillment = ((ApiOkResponse)response).Result;
            return Ok(taskFulfillment);
        }
    }
}