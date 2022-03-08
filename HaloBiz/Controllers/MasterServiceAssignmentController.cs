using Halobiz.Common.DTOs.ApiDTOs;
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
    public class MasterServiceAssignmentController : ControllerBase
    {
        private readonly IMasterServiceAssignmentService _masterServiceAssignmentService;
        private readonly IInvoiceService _invoiceService;

        public MasterServiceAssignmentController(IMasterServiceAssignmentService masterServiceAssignmentService, IInvoiceService invocieService)
        {
            _masterServiceAssignmentService = masterServiceAssignmentService;
            _invoiceService = invocieService;
        }

        [HttpGet("GetAllServiceAssignmentMasters")]
        public async Task<ApiCommonResponse> GetAllServiceAssignmentMasters()
        {
            return await _masterServiceAssignmentService.GetAllMasterServiceAssignments();
            //var response = await _masterServiceAssignmentService.GetAllMasterServiceAssignments();
            //if (response.StatusCode >= 400)
            //    return StatusCode(response.StatusCode, response);
            //var cType = ((ApiOkResponse)response).Result;
            //return Ok(cType);
        }

        [HttpGet("GetAllCustomerDivisions")]
        public async Task<ApiCommonResponse> GetCustomerDivisions()
        {
            return await _masterServiceAssignmentService.GetAllCustomerDivisions();
        }

        [HttpGet("GetAllSecondaryServiceAssignments")]
        public async Task<ApiCommonResponse> GetAllSecondaryServiceAssignments()
        {
            return await _masterServiceAssignmentService.GetAllSecondaryServiceAssignments();
        }


        [HttpGet("GetServiceAssignmentMasterById/{id}")]
        public async Task<ApiCommonResponse> GetServiceAssignmentMasterById(long id)
        {
            return await _masterServiceAssignmentService.GetMasterServiceAssignmentById(id);
            //if (response.StatusCode >= 400)
            //    return StatusCode(response.StatusCode, response);
            //var Rank = ((ApiOkResponse)response).Result;
            //return Ok(Rank);
        }

        [HttpGet("GetSecondaryServiceAssignmentById/{id}")]
        public async Task<ApiCommonResponse> GetSecondaryServiceAssignmentById(long id)
        {
            return await _masterServiceAssignmentService.GetsecondaryServiceAssignmentById(id);
        }

        [HttpPost("AddNewServiceAssignmentMaster")]
        public async Task<ApiCommonResponse> AddNewServiceAssignmentMaster(MasterServiceAssignmentReceivingDTO ReceivingDTO)
        {
             return await _masterServiceAssignmentService.AddMasterServiceAssignment(HttpContext, ReceivingDTO);

            //if (response.StatusCode >= 400)
            //    return StatusCode(response.StatusCode, response);
            //var rank = ((ApiOkResponse)response).Result;
            //return Ok(rank);
        }
        [HttpPost("AddNewAutoServiceAssignmentMaster")]
        public async Task<ApiCommonResponse> AddNewAutoServiceAssignmentMaster(MasterServiceAssignmentReceivingDTO ReceivingDTO)
        {
             return await _masterServiceAssignmentService.AddMasterAutoServiceAssignment(HttpContext, ReceivingDTO);

            //if (response.StatusCode >= 400)
            //    return StatusCode(response.StatusCode, response);
            //var rank = ((ApiOkResponse)response).Result;
            //return Ok(rank);
        }

        [HttpPost("AddNewSecondaryServiceAssignment")]
        public async Task<ApiCommonResponse> AddNewSecondaryServiceAssignment(SecondaryServiceAssignmentReceivingDTO ReceivingDTO)
        {
            return await _masterServiceAssignmentService.AddSecondaryServiceAssignment(HttpContext, ReceivingDTO);
        }

        [HttpPut("UpdateServiceAssignmentMasterById/{id}")]
        public async Task<ApiCommonResponse> UpdateServiceAssignmentMasterById(long id, MasterServiceAssignmentReceivingDTO ReceivingDTO)
        {
            return await _masterServiceAssignmentService.UpdateMasterServiceAssignment(HttpContext, id, ReceivingDTO);
            //if (response.StatusCode >= 400)
            //    return StatusCode(response.StatusCode, response);
            //var type = ((ApiOkResponse)response).Result;
            //return Ok(type);
        }

        [HttpDelete("DeleteServiceAssignmentMasterById/{id}")]
        public async Task<ApiCommonResponse> DeleteServiceAssignmentMasterById(int id)
        {
            return await _masterServiceAssignmentService.DeleteMasterServiceAssignment(id);
            //return StatusCode(response.StatusCode);
        }

        [HttpDelete("DeleteSecondaryServiceAssignmentById/{id}")]
        public async Task<ApiCommonResponse> DeleteSecondaryServiceAssignmentById(int id)
        {
            return await _masterServiceAssignmentService.DeleteSecondaryServiceAssignment(id);
            //return StatusCode(response.StatusCode);
        }

        [HttpGet("GetJMPDetails/{id}")]

        public async Task<ApiCommonResponse> GetJMPDetails(long id)
        {
            return await _invoiceService.GetJMPDetails(id);
        }

        [HttpGet("SendJMP/{id}")]
        public async Task<ApiCommonResponse> SendJMP(long id)
        {
            return await _invoiceService.SendJourneyManagementPlan(id);
        }
    }
}
