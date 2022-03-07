using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Halobiz.Common.DTOs.ApiDTOs;
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
        public async Task<ApiCommonResponse> GetDeliverableFulfillment()
        {
            return await _deliverableFulfillmentService.GetAllDeliverableFulfillment();
        }

        [HttpGet("GetAssignedRation/{taskMasterId}")]
        public async Task<ApiCommonResponse> GetDeliverableFulfillmentToAssignedRatioFulfillment(long taskMasterId)
        {
            return await _deliverableFulfillmentService.DeliverableToAssignedUserRatio(taskMasterId);
        }
        
        [HttpGet("caption/{name}")]
        public async Task<ApiCommonResponse> GetByCaption(string name)
        {
            return await _deliverableFulfillmentService.GetDeliverableFulfillmentByName(name);
        }

        [HttpGet("{id}")]
        public async Task<ApiCommonResponse> GetById(long id)
        {
            return await _deliverableFulfillmentService.GetDeliverableFulfillmentById(id);
        }

        [HttpGet("GetDeliverableStat/{userId}")]
        public async Task<ApiCommonResponse> GetUserDeliverableStat(long userId)
        {
            return await _deliverableFulfillmentService.GetUserDeliverableFulfillmentStat(userId);
        }

        [HttpGet("GetDeliverableDashboard/{userId}")]
        public async Task<ApiCommonResponse> GetUserDeliverableDashboard(long userId)
        {
            return await _deliverableFulfillmentService.GetUserDeliverableFulfillmentDashboard(userId);
        }

        [HttpPost("")]
        public async Task<ApiCommonResponse> AddNewDeliverableFulfillment(DeliverableFulfillmentReceivingDTO deliverableFulfillmentReceiving)
        {
            return await _deliverableFulfillmentService.AddDeliverableFulfillment(HttpContext, deliverableFulfillmentReceiving);
        }

        [HttpPut("{id}")]
        public async Task<ApiCommonResponse> UpdateById(long id, DeliverableFulfillmentReceivingDTO deliverableFulfillmentReceivingDTO)
        {
            return await _deliverableFulfillmentService.UpdateDeliverableFulfillment(HttpContext, id, deliverableFulfillmentReceivingDTO);
        }

        /*[HttpDelete("{id}")]
        public async Task<ApiCommonResponse> DeleteById(int id)
        {
            return await _deliverableFulfillmentService.DeleteDeliverableFulfillment(id);
            return StatusCode(response.StatusCode);
        }*/

        [HttpPost("ReAssignDeliverableFulfillment/{id}")]
        public async Task<ApiCommonResponse> ReAssignDeliverableFulfillment(long id, DeliverableFulfillmentReceivingDTO deliverableFulfillmentReceivingDTO)
        {
            return await _deliverableFulfillmentService.ReAssignDeliverableFulfillment(HttpContext, id, deliverableFulfillmentReceivingDTO);
            
        }

        [HttpPut("SetIsPicked/{id}")]
        public async Task<ApiCommonResponse> SetIsPicked(long id)
        {
            return await _deliverableFulfillmentService.SetIsPicked(HttpContext, id);
        }

        [HttpPut("SetRequestedForValidation/{id}")]
        public async Task<ApiCommonResponse> SetRequestedForValidation(long id, DeliverableFulfillmentApprovalReceivingDTO dto)
        {
            return await _deliverableFulfillmentService.SetRequestedForValidation(HttpContext, id, dto);
        }

        [HttpPut("SetDeliveredStatus/{id}")]
        public async Task<ApiCommonResponse> SetDeliveredStatus(long id)
        {
            return await _deliverableFulfillmentService.SetDeliveredStatus(HttpContext, id);
        }
    }
}