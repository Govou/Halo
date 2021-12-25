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
    public class TypesForServiceAssignmentController : ControllerBase
    {
        private readonly ITypesForServiceAssignmentService _typesForServiceAssignmentService;

        public TypesForServiceAssignmentController(ITypesForServiceAssignmentService typesForServiceAssignmentService)
        {
            _typesForServiceAssignmentService = typesForServiceAssignmentService;
        }

        //PassengerType
        [HttpGet("GetAllPassengerTypes")]
        public async Task<ActionResult> GetAllPassengerTypes()
        {
            var response = await _typesForServiceAssignmentService.GetAllPassengerTypes();
            if (response.StatusCode >= 400)
                return StatusCode(response.StatusCode, response);
            var cType = ((ApiOkResponse)response).Result;
            return Ok(cType);
        }

        [HttpGet("GetPassengerTypeById/{id}")]
        public async Task<ActionResult> GetPassengerTypeById(long id)
        {
            var response = await _typesForServiceAssignmentService.GetPassengerTypeById(id);
            if (response.StatusCode >= 400)
                return StatusCode(response.StatusCode, response);
            var Rank = ((ApiOkResponse)response).Result;
            return Ok(Rank);
        }

        [HttpPost("AddNewPassengerType")]
        public async Task<ActionResult> AddNewPassengerType(PassengerTypesForServiceAssignmentReceivingDTO ReceivingDTO)
        {
            var response = await _typesForServiceAssignmentService.AddPassengerType(HttpContext, ReceivingDTO);

            if (response.StatusCode >= 400)
                return StatusCode(response.StatusCode, response);
            var rank = ((ApiOkResponse)response).Result;
            return Ok(rank);
        }

        [HttpPut("UpdatePassengerTypeById/{id}")]
        public async Task<IActionResult> UpdatePassengerTypeById(long id, PassengerTypesForServiceAssignmentReceivingDTO Receiving)
        {
            var response = await _typesForServiceAssignmentService.UpdatePassengerType(HttpContext, id, Receiving);
            if (response.StatusCode >= 400)
                return StatusCode(response.StatusCode, response);
            var type = ((ApiOkResponse)response).Result;
            return Ok(type);
        }
        [HttpDelete("DeletePassengerTypeById/{id}")]
        public async Task<ActionResult> DeletePassengerTypeById(int id)
        {
            var response = await _typesForServiceAssignmentService.DeletePassengerType(id);
            return StatusCode(response.StatusCode);
        }

        //Release
        [HttpGet("GetAllReleaseTypes")]
        public async Task<ActionResult> GetAllReleaseTypes()
        {
            var response = await _typesForServiceAssignmentService.GetAllReleaseTypes();
            if (response.StatusCode >= 400)
                return StatusCode(response.StatusCode, response);
            var cType = ((ApiOkResponse)response).Result;
            return Ok(cType);
        }

        [HttpGet("GetReleaseTypeById/{id}")]
        public async Task<ActionResult> GetReleaseTypeById(long id)
        {
            var response = await _typesForServiceAssignmentService.GetReleaseTypeById(id);
            if (response.StatusCode >= 400)
                return StatusCode(response.StatusCode, response);
            var Rank = ((ApiOkResponse)response).Result;
            return Ok(Rank);
        }

        [HttpPost("AddNewReleaseType")]
        public async Task<ActionResult> AddNewReleaseType(ReleaseTypesForServiceAssignmentReceivingDTO ReceivingDTO)
        {
            var response = await _typesForServiceAssignmentService.AddReleaseType(HttpContext, ReceivingDTO);

            if (response.StatusCode >= 400)
                return StatusCode(response.StatusCode, response);
            var rank = ((ApiOkResponse)response).Result;
            return Ok(rank);
        }

        [HttpPut("UpdateReleaseTypeById/{id}")]
        public async Task<IActionResult> UpdateReleaseTypeById(long id, ReleaseTypesForServiceAssignmentReceivingDTO Receiving)
        {
            var response = await _typesForServiceAssignmentService.UpdateReleaseType(HttpContext, id, Receiving);
            if (response.StatusCode >= 400)
                return StatusCode(response.StatusCode, response);
            var type = ((ApiOkResponse)response).Result;
            return Ok(type);
        }
        [HttpDelete("DeleteReleaseTypeById/{id}")]
        public async Task<ActionResult> DeleteReleaseTypeById(int id)
        {
            var response = await _typesForServiceAssignmentService.DeleteReleaseType(id);
            return StatusCode(response.StatusCode);
        }

        //Source
        [HttpGet("GetAllSourceTypes")]
        public async Task<ActionResult> GetAllSourceTypes()
        {
            var response = await _typesForServiceAssignmentService.GetAllSourceTypes();
            if (response.StatusCode >= 400)
                return StatusCode(response.StatusCode, response);
            var cType = ((ApiOkResponse)response).Result;
            return Ok(cType);
        }

        [HttpGet("GetSourceTypeById/{id}")]
        public async Task<ActionResult> GetSourceTypeById(long id)
        {
            var response = await _typesForServiceAssignmentService.GetSourceTypeById(id);
            if (response.StatusCode >= 400)
                return StatusCode(response.StatusCode, response);
            var Rank = ((ApiOkResponse)response).Result;
            return Ok(Rank);
        }

        [HttpPost("AddNewSourceType")]
        public async Task<ActionResult> AddNewSourceType(SourceTypesForServiceAssignmentReceivingDTO ReceivingDTO)
        {
            var response = await _typesForServiceAssignmentService.AddSourceType(HttpContext, ReceivingDTO);

            if (response.StatusCode >= 400)
                return StatusCode(response.StatusCode, response);
            var rank = ((ApiOkResponse)response).Result;
            return Ok(rank);
        }

        [HttpPut("UpdateSourceTypeById/{id}")]
        public async Task<IActionResult> UpdateSourceTypeById(long id, SourceTypesForServiceAssignmentReceivingDTO Receiving)
        {
            var response = await _typesForServiceAssignmentService.UpdateSourceType(HttpContext, id, Receiving);
            if (response.StatusCode >= 400)
                return StatusCode(response.StatusCode, response);
            var type = ((ApiOkResponse)response).Result;
            return Ok(type);
        }
        [HttpDelete("DeleteSourceTypeById/{id}")]
        public async Task<ActionResult> DeleteSourceTypeById(int id)
        {
            var response = await _typesForServiceAssignmentService.DeleteSourceType(id);
            return StatusCode(response.StatusCode);
        }

        //Trip
        [HttpGet("GetAllTripTypes")]
        public async Task<ActionResult> GetAllTripTypes()
        {
            var response = await _typesForServiceAssignmentService.GetAllTripTypes();
            if (response.StatusCode >= 400)
                return StatusCode(response.StatusCode, response);
            var cType = ((ApiOkResponse)response).Result;
            return Ok(cType);
        }

        [HttpGet("GetTripTypeById/{id}")]
        public async Task<ActionResult> GetTripTypeById(long id)
        {
            var response = await _typesForServiceAssignmentService.GetTripTypeById(id);
            if (response.StatusCode >= 400)
                return StatusCode(response.StatusCode, response);
            var Rank = ((ApiOkResponse)response).Result;
            return Ok(Rank);
        }

        [HttpPost("AddNewTripType")]
        public async Task<ActionResult> AddNewTripType(TripTypesForServiceAssignmentReceivingDTO ReceivingDTO)
        {
            var response = await _typesForServiceAssignmentService.AddTripType(HttpContext, ReceivingDTO);

            if (response.StatusCode >= 400)
                return StatusCode(response.StatusCode, response);
            var rank = ((ApiOkResponse)response).Result;
            return Ok(rank);
        }

        [HttpPut("UpdateTripTypeById/{id}")]
        public async Task<IActionResult> UpdateTripTypeById(long id, TripTypesForServiceAssignmentReceivingDTO Receiving)
        {
            var response = await _typesForServiceAssignmentService.UpdateTripType(HttpContext, id, Receiving);
            if (response.StatusCode >= 400)
                return StatusCode(response.StatusCode, response);
            var type = ((ApiOkResponse)response).Result;
            return Ok(type);
        }
        [HttpDelete("DeleteTripTypeById/{id}")]
        public async Task<ActionResult> DeleteTripTypeById(int id)
        {
            var response = await _typesForServiceAssignmentService.DeleteTripType(id);
            return StatusCode(response.StatusCode);
        }
    }
}
