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
    public class TypesForServiceAssignmentController : ControllerBase
    {
        private readonly ITypesForServiceAssignmentService _typesForServiceAssignmentService;

        public TypesForServiceAssignmentController(ITypesForServiceAssignmentService typesForServiceAssignmentService)
        {
            _typesForServiceAssignmentService = typesForServiceAssignmentService;
        }

        //PassengerType
        [HttpGet("GetAllPassengerTypes")]
        public async Task<ApiCommonResponse> GetAllPassengerTypes()
        {
            return await _typesForServiceAssignmentService.GetAllPassengerTypes();
            //if (response.StatusCode >= 400)
            //    return StatusCode(response.StatusCode, response);
            //var cType = ((ApiOkResponse)response).Result;
            //return Ok(cType);
        }

        [HttpGet("GetPassengerTypeById/{id}")]
        public async Task<ApiCommonResponse> GetPassengerTypeById(long id)
        {
            return await _typesForServiceAssignmentService.GetPassengerTypeById(id);
            //if (response.StatusCode >= 400)
            //    return StatusCode(response.StatusCode, response);
            //var Rank = ((ApiOkResponse)response).Result;
            //return Ok(Rank);
        }

        [HttpPost("AddNewPassengerType")]
        public async Task<ApiCommonResponse> AddNewPassengerType(PassengerTypesForServiceAssignmentReceivingDTO ReceivingDTO)
        {
            return await _typesForServiceAssignmentService.AddPassengerType(HttpContext, ReceivingDTO);

            //if (response.StatusCode >= 400)
            //    return StatusCode(response.StatusCode, response);
            //var rank = ((ApiOkResponse)response).Result;
            //return Ok(rank);
        }

        [HttpPut("UpdatePassengerTypeById/{id}")]
        public async Task<ApiCommonResponse> UpdatePassengerTypeById(long id, PassengerTypesForServiceAssignmentReceivingDTO Receiving)
        {
            return await _typesForServiceAssignmentService.UpdatePassengerType(HttpContext, id, Receiving);
            //if (response.StatusCode >= 400)
            //    return StatusCode(response.StatusCode, response);
            //var type = ((ApiOkResponse)response).Result;
            //return Ok(type);
        }
        [HttpDelete("DeletePassengerTypeById/{id}")]
        public async Task<ApiCommonResponse> DeletePassengerTypeById(int id)
        {
            return await _typesForServiceAssignmentService.DeletePassengerType(id);
            //return StatusCode(response.StatusCode);
        }

        //Release
        [HttpGet("GetAllReleaseTypes")]
        public async Task<ApiCommonResponse> GetAllReleaseTypes()
        {
            return await _typesForServiceAssignmentService.GetAllReleaseTypes();
            //if (response.StatusCode >= 400)
            //    return StatusCode(response.StatusCode, response);
            //var cType = ((ApiOkResponse)response).Result;
            //return Ok(cType);
        }

        [HttpGet("GetReleaseTypeById/{id}")]
        public async Task<ApiCommonResponse> GetReleaseTypeById(long id)
        {
            return await _typesForServiceAssignmentService.GetReleaseTypeById(id);
            //if (response.StatusCode >= 400)
            //    return StatusCode(response.StatusCode, response);
            //var Rank = ((ApiOkResponse)response).Result;
            //return Ok(Rank);
        }

        [HttpPost("AddNewReleaseType")]
        public async Task<ApiCommonResponse> AddNewReleaseType(ReleaseTypesForServiceAssignmentReceivingDTO ReceivingDTO)
        {
            return await _typesForServiceAssignmentService.AddReleaseType(HttpContext, ReceivingDTO);

            //if (response.StatusCode >= 400)
            //    return StatusCode(response.StatusCode, response);
            //var rank = ((ApiOkResponse)response).Result;
            //return Ok(rank);
        }

        [HttpPut("UpdateReleaseTypeById/{id}")]
        public async Task<ApiCommonResponse> UpdateReleaseTypeById(long id, ReleaseTypesForServiceAssignmentReceivingDTO Receiving)
        {
            return await _typesForServiceAssignmentService.UpdateReleaseType(HttpContext, id, Receiving);
            //if (response.StatusCode >= 400)
            //    return StatusCode(response.StatusCode, response);
            //var type = ((ApiOkResponse)response).Result;
            //return Ok(type);
        }
        [HttpDelete("DeleteReleaseTypeById/{id}")]
        public async Task<ApiCommonResponse> DeleteReleaseTypeById(int id)
        {
            return await _typesForServiceAssignmentService.DeleteReleaseType(id);
            //return StatusCode(response.StatusCode);
        }

        //Source
        [HttpGet("GetAllSourceTypes")]
        public async Task<ApiCommonResponse> GetAllSourceTypes()
        {
           return await _typesForServiceAssignmentService.GetAllSourceTypes();
            //if (response.StatusCode >= 400)
            //    return StatusCode(response.StatusCode, response);
            //var cType = ((ApiOkResponse)response).Result;
            //return Ok(cType);
        }

        [HttpGet("GetSourceTypeById/{id}")]
        public async Task<ApiCommonResponse> GetSourceTypeById(long id)
        {
            return await _typesForServiceAssignmentService.GetSourceTypeById(id);
            //if (response.StatusCode >= 400)
            //    return StatusCode(response.StatusCode, response);
            //var Rank = ((ApiOkResponse)response).Result;
            //return Ok(Rank);
        }

        [HttpPost("AddNewSourceType")]
        public async Task<ApiCommonResponse> AddNewSourceType(SourceTypesForServiceAssignmentReceivingDTO ReceivingDTO)
        {
            return await _typesForServiceAssignmentService.AddSourceType(HttpContext, ReceivingDTO);

            //if (response.StatusCode >= 400)
            //    return StatusCode(response.StatusCode, response);
            //var rank = ((ApiOkResponse)response).Result;
            //return Ok(rank);
        }

        [HttpPut("UpdateSourceTypeById/{id}")]
        public async Task<ApiCommonResponse> UpdateSourceTypeById(long id, SourceTypesForServiceAssignmentReceivingDTO Receiving)
        {
            return await _typesForServiceAssignmentService.UpdateSourceType(HttpContext, id, Receiving);
            //if (response.StatusCode >= 400)
            //    return StatusCode(response.StatusCode, response);
            //var type = ((ApiOkResponse)response).Result;
            //return Ok(type);
        }
        [HttpDelete("DeleteSourceTypeById/{id}")]
        public async Task<ApiCommonResponse> DeleteSourceTypeById(int id)
        {
            return await _typesForServiceAssignmentService.DeleteSourceType(id);
            //return StatusCode(response.StatusCode);
        }

        //Trip
        [HttpGet("GetAllTripTypes")]
        public async Task<ApiCommonResponse> GetAllTripTypes()
        {
            return await _typesForServiceAssignmentService.GetAllTripTypes();
            //if (response.StatusCode >= 400)
            //    return StatusCode(response.StatusCode, response);
            //var cType = ((ApiOkResponse)response).Result;
            //return Ok(cType);
        }

        [HttpGet("GetTripTypeById/{id}")]
        public async Task<ApiCommonResponse> GetTripTypeById(long id)
        {
            return await _typesForServiceAssignmentService.GetTripTypeById(id);
            //if (response.StatusCode >= 400)
            //    return StatusCode(response.StatusCode, response);
            //var Rank = ((ApiOkResponse)response).Result;
            //return Ok(Rank);
        }

        [HttpPost("AddNewTripType")]
        public async Task<ApiCommonResponse> AddNewTripType(TripTypesForServiceAssignmentReceivingDTO ReceivingDTO)
        {
            return await  _typesForServiceAssignmentService.AddTripType(HttpContext, ReceivingDTO);

            //if (response.StatusCode >= 400)
            //    return StatusCode(response.StatusCode, response);
            //var rank = ((ApiOkResponse)response).Result;
            //return Ok(rank);
        }

        [HttpPut("UpdateTripTypeById/{id}")]
        public async Task<ApiCommonResponse> UpdateTripTypeById(long id, TripTypesForServiceAssignmentReceivingDTO Receiving)
        {
            return await _typesForServiceAssignmentService.UpdateTripType(HttpContext, id, Receiving);
            //if (response.StatusCode >= 400)
            //    return StatusCode(response.StatusCode, response);
            //var type = ((ApiOkResponse)response).Result;
            //return Ok(type);
        }
        [HttpDelete("DeleteTripTypeById/{id}")]
        public async Task<ApiCommonResponse> DeleteTripTypeById(int id)
        {
            return await _typesForServiceAssignmentService.DeleteTripType(id);
            //return StatusCode(response.StatusCode);
        }
    }
}
