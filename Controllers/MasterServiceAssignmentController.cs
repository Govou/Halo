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
    public class MasterServiceAssignmentController : ControllerBase
    {
        private readonly IMasterServiceAssignmentService _masterServiceAssignmentService;

        public MasterServiceAssignmentController(IMasterServiceAssignmentService masterServiceAssignmentService)
        {
            _masterServiceAssignmentService = masterServiceAssignmentService;
        }

        [HttpGet("GetAllServiceAssignmentMasters")]
        public async Task<ActionResult> GetAllServiceAssignmentMasters()
        {
            var response = await _masterServiceAssignmentService.GetAllMasterServiceAssignments();
            if (response.StatusCode >= 400)
                return StatusCode(response.StatusCode, response);
            var cType = ((ApiOkResponse)response).Result;
            return Ok(cType);
        }

        [HttpGet("GetServiceAssignmentMasterById/{id}")]
        public async Task<ActionResult> GetServiceAssignmentMasterById(long id)
        {
            var response = await _masterServiceAssignmentService.GetMasterServiceAssignmentById(id);
            if (response.StatusCode >= 400)
                return StatusCode(response.StatusCode, response);
            var Rank = ((ApiOkResponse)response).Result;
            return Ok(Rank);
        }

        [HttpPost("AddNewServiceAssignmentMaster")]
        public async Task<ActionResult> AddNewServiceAssignmentMaster(MasterServiceAssignmentReceivingDTO ReceivingDTO)
        {
            var response = await _masterServiceAssignmentService.AddMasterServiceAssignment(HttpContext, ReceivingDTO);

            if (response.StatusCode >= 400)
                return StatusCode(response.StatusCode, response);
            var rank = ((ApiOkResponse)response).Result;
            return Ok(rank);
        }

        [HttpPut("UpdateServiceAssignmentMasterById/{id}")]
        public async Task<IActionResult> UpdateServiceAssignmentMasterById(long id, MasterServiceAssignmentReceivingDTO ReceivingDTO)
        {
            var response = await _masterServiceAssignmentService.UpdateMasterServiceAssignment(HttpContext, id, ReceivingDTO);
            if (response.StatusCode >= 400)
                return StatusCode(response.StatusCode, response);
            var type = ((ApiOkResponse)response).Result;
            return Ok(type);
        }

        [HttpDelete("DeleteServiceAssignmentMasterById/{id}")]
        public async Task<ActionResult> DeleteServiceAssignmentMasterById(int id)
        {
            var response = await _masterServiceAssignmentService.DeleteMasterServiceAssignment(id);
            return StatusCode(response.StatusCode);
        }
    }
}
