using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HaloBiz.DTOs.ApiDTOs;
using HaloBiz.DTOs.ReceivingDTOs;
using HaloBiz.DTOs.ReceivingDTOs.LAMS;
using HaloBiz.DTOs.TransferDTOs.LAMS;
using HaloBiz.MyServices.LAMS;
using Microsoft.AspNetCore.Mvc;
//using Controllers.Models;

namespace Controllers.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class DeliverableFulfillmentController : ControllerBase
    {
        private readonly IDeliverableFulfillmentService _deliverableFulfillmentService;

        public DeliverableFulfillmentController(IDeliverableFulfillmentService deliverableFulfillmentService)
        {
            this._deliverableFulfillmentService = deliverableFulfillmentService;
        }

        [HttpGet("")]
        public async Task<ActionResult> GetDeliverableFulfillment()
        {
            var response = await _deliverableFulfillmentService.GetAllDeliverableFulfillment();
            if (response.StatusCode >= 400)
                return StatusCode(response.StatusCode, response);
            var deliverableFulfillment = ((ApiOkResponse)response).Result;
            return Ok(deliverableFulfillment);
        }

        [HttpGet("GetAssignedRation/{taskMasterId}")]
        public async Task<ActionResult> GetDeliverableFulfillmentToAssignedRatioFulfillment(long taskMasterId)
        {
            var response = await _deliverableFulfillmentService.DeliverableToAssignedUserRatio(taskMasterId);
            if (response.StatusCode >= 400)
                return StatusCode(response.StatusCode, response);
            var deliverableFulfillment = ((ApiOkResponse)response).Result;
            return Ok(deliverableFulfillment);
        }
        
        [HttpGet("caption/{name}")]
        public async Task<ActionResult> GetByCaption(string name)
        {
            var response = await _deliverableFulfillmentService.GetDeliverableFulfillmentByName(name);
            if (response.StatusCode >= 400)
                return StatusCode(response.StatusCode, response);
            var deliverableFulfillment = ((ApiOkResponse)response).Result;
            return Ok(deliverableFulfillment);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult> GetById(long id)
        {
            var response = await _deliverableFulfillmentService.GetDeliverableFulfillmentById(id);
            if (response.StatusCode >= 400)
                return StatusCode(response.StatusCode, response);
            var deliverableFulfillment = ((ApiOkResponse)response).Result;
            return Ok(deliverableFulfillment);
        }

        [HttpGet("GetDeliverableStat/{userId}")]
        public async Task<ActionResult> GetUserDeliverableStat(long userId)
        {
            var response = await _deliverableFulfillmentService.GetUserDeliverableFulfillmentStat(userId);
            if (response.StatusCode >= 400)
                return StatusCode(response.StatusCode, response);
            var stat = ((ApiOkResponse)response).Result;
            return Ok(stat);
        }

        [HttpGet("GetDeliverableDashboard/{userId}")]
        public async Task<ActionResult> GetUserDeliverableDashboard(long userId)
        {
            var response = await _deliverableFulfillmentService.GetUserDeliverableFulfillmentDashboard(userId);
            if (response.StatusCode >= 400)
                return StatusCode(response.StatusCode, response);
            var stat = ((ApiOkResponse)response).Result;
            return Ok(stat);
        }

        [HttpPost("")]
        public async Task<ActionResult> AddNewDeliverableFulfillment(DeliverableFulfillmentReceivingDTO deliverableFulfillmentReceiving)
        {
            var response = await _deliverableFulfillmentService.AddDeliverableFulfillment(HttpContext, deliverableFulfillmentReceiving);
            if (response.StatusCode >= 400)
                return StatusCode(response.StatusCode, response);
            var deliverableFulfillment = ((ApiOkResponse)response).Result;
            return Ok(deliverableFulfillment);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateById(long id, DeliverableFulfillmentReceivingDTO deliverableFulfillmentReceivingDTO)
        {
            var response = await _deliverableFulfillmentService.UpdateDeliverableFulfillment(HttpContext, id, deliverableFulfillmentReceivingDTO);
            if (response.StatusCode >= 400)
                return StatusCode(response.StatusCode, response);
            var deliverableFulfillment = ((ApiOkResponse)response).Result;
            return Ok(deliverableFulfillment);
        }

        /*[HttpDelete("{id}")]
        public async Task<ActionResult> DeleteById(int id)
        {
            var response = await _deliverableFulfillmentService.DeleteDeliverableFulfillment(id);
            return StatusCode(response.StatusCode);
        }*/

        [HttpPost("ReAssignDeliverableFulfillment/{id}")]
        public async Task<IActionResult> ReAssignDeliverableFulfillment(long id, DeliverableFulfillmentReceivingDTO deliverableFulfillmentReceivingDTO)
        {
            var response = await _deliverableFulfillmentService.ReAssignDeliverableFulfillment(HttpContext, id, deliverableFulfillmentReceivingDTO);
            if (response.StatusCode >= 400)
                return StatusCode(response.StatusCode, response);
            var deliverableFulfillment = ((ApiOkResponse)response).Result;
            return Ok(deliverableFulfillment);
        }

        [HttpPut("SetIsPicked/{id}")]
        public async Task<IActionResult> SetIsPicked(long id)
        {
            var response = await _deliverableFulfillmentService.SetIsPicked(HttpContext, id);
            if (response.StatusCode >= 400)
                return StatusCode(response.StatusCode, response);
            var deliverableFulfillment = ((ApiOkResponse)response).Result;
            return Ok(deliverableFulfillment);
        }

        [HttpPut("SetRequestedForValidation/{id}")]
        public async Task<IActionResult> SetRequestedForValidation(long id, DeliverableFulfillmentApprovalReceivingDTO dto)
        {
            var response = await _deliverableFulfillmentService.SetRequestedForValidation(HttpContext, id, dto);
            if (response.StatusCode >= 400)
                return StatusCode(response.StatusCode, response);
            var deliverableFulfillment = ((ApiOkResponse)response).Result;
            return Ok(deliverableFulfillment);
        }

        [HttpPut("SetDeliveredStatus/{id}")]
        public async Task<IActionResult> SetDeliveredStatus(long id)
        {
            var response = await _deliverableFulfillmentService.SetDeliveredStatus(HttpContext, id);
            if (response.StatusCode >= 400)
                return StatusCode(response.StatusCode, response);
            var deliverableFulfillment = ((ApiOkResponse)response).Result;
            return Ok(deliverableFulfillment);
        }
    }
}