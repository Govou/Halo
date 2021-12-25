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
    public class ServiceAssignmentDetailsController : ControllerBase
    {
       
        private readonly IServiceAssignmentDetailsService _serviceAssignmentDetailsService;

        public ServiceAssignmentDetailsController(IServiceAssignmentDetailsService serviceAssignmentDetailsService)
        {
            _serviceAssignmentDetailsService = serviceAssignmentDetailsService;
        }

      

        //ArmedEscort
        [HttpGet("GetAllArmedEscortDetails")]
        public async Task<ActionResult> GetAllArmedEscortDetails()
        {
            var response = await _serviceAssignmentDetailsService.GetAllArmedEscortDetails();
            if (response.StatusCode >= 400)
                return StatusCode(response.StatusCode, response);
            var cType = ((ApiOkResponse)response).Result;
            return Ok(cType);
        }

        [HttpGet("GetAllArmedEscortDetailsByAssignmentId/{id}")]
        public async Task<ActionResult> GetAllArmedEscortDetailsByAssignmentId(long id)
        {
            var response = await _serviceAssignmentDetailsService.GetAllArmedEscortDetailsByAssignmentId(id);
            if (response.StatusCode >= 400)
                return StatusCode(response.StatusCode, response);
            var cType = ((ApiOkResponse)response).Result;
            return Ok(cType);
        }

        [HttpGet("GetArmedEscortDetailById/{id}")]
        public async Task<ActionResult> GetArmedEscortDetailById(long id)
        {
            var response = await _serviceAssignmentDetailsService.GetArmedEscortDetailById(id);
            if (response.StatusCode >= 400)
                return StatusCode(response.StatusCode, response);
            var Rank = ((ApiOkResponse)response).Result;
            return Ok(Rank);
        }

        [HttpPost("AddNewArmedEscortDetail")]
        public async Task<ActionResult> AddNewArmedEscortDetail(ArmedEscortServiceAssignmentDetailsReceivingDTO ReceivingDTO)
        {
            var response = await _serviceAssignmentDetailsService.AddArmedEscortDetail(HttpContext, ReceivingDTO);

            if (response.StatusCode >= 400)
                return StatusCode(response.StatusCode, response);
            var rank = ((ApiOkResponse)response).Result;
            return Ok(rank);
        }

        [HttpPut("UpdateArmedEscortDetailById/{id}")]
        public async Task<IActionResult> UpdateArmedEscortDetailById(long id, ArmedEscortServiceAssignmentDetailsReceivingDTO Receiving)
        {
            var response = await _serviceAssignmentDetailsService.UpdateArmedEscortDetail(HttpContext, id, Receiving);
            if (response.StatusCode >= 400)
                return StatusCode(response.StatusCode, response);
            var type = ((ApiOkResponse)response).Result;
            return Ok(type);
        }
        [HttpDelete("DeleteArmedEscortDetailById/{id}")]
        public async Task<ActionResult> DeleteArmedEscortDetailById(int id)
        {
            var response = await _serviceAssignmentDetailsService.DeleteArmedEscortDetail(id);
            return StatusCode(response.StatusCode);
        }

        //Commander
        [HttpGet("GetAllCommanderDetails")]
        public async Task<ActionResult> GetAllCommanderDetails()
        {
            var response = await _serviceAssignmentDetailsService.GetAllCommanderDetails();
            if (response.StatusCode >= 400)
                return StatusCode(response.StatusCode, response);
            var cType = ((ApiOkResponse)response).Result;
            return Ok(cType);
        }

        [HttpGet("GetAllCommanderDetailsByAssignmentId/{id}")]
        public async Task<ActionResult> GetAllCommanderDetailsByAssignmentId(long id)
        {
            var response = await _serviceAssignmentDetailsService.GetAllCommanderDetailsByAssignmentId(id);
            if (response.StatusCode >= 400)
                return StatusCode(response.StatusCode, response);
            var cType = ((ApiOkResponse)response).Result;
            return Ok(cType);
        }

        [HttpGet("GetCommanderDetailById/{id}")]
        public async Task<ActionResult> GetCommanderDetailById(long id)
        {
            var response = await _serviceAssignmentDetailsService.GetCommanderDetailById(id);
            if (response.StatusCode >= 400)
                return StatusCode(response.StatusCode, response);
            var Rank = ((ApiOkResponse)response).Result;
            return Ok(Rank);
        }

        [HttpPost("AddNewCommanderDetail")]
        public async Task<ActionResult> AddNewCommanderDetail(CommanderServiceAssignmentDetailsReceivingDTO ReceivingDTO)
        {
            var response = await _serviceAssignmentDetailsService.AddCommanderDetail(HttpContext, ReceivingDTO);

            if (response.StatusCode >= 400)
                return StatusCode(response.StatusCode, response);
            var rank = ((ApiOkResponse)response).Result;
            return Ok(rank);
        }

        [HttpPut("UpdateCommanderDetailById/{id}")]
        public async Task<IActionResult> UpdateCommanderDetailById(long id, CommanderServiceAssignmentDetailsReceivingDTO Receiving)
        {
            var response = await _serviceAssignmentDetailsService.UpdateCommanderDetail(HttpContext, id, Receiving);
            if (response.StatusCode >= 400)
                return StatusCode(response.StatusCode, response);
            var type = ((ApiOkResponse)response).Result;
            return Ok(type);
        }
        [HttpDelete("DeleteCommanderDetailById/{id}")]
        public async Task<ActionResult> DeleteCommanderDetailById(int id)
        {
            var response = await _serviceAssignmentDetailsService.DeleteCommanderDetail(id);
            return StatusCode(response.StatusCode);
        }

        //Pilot
        [HttpGet("GetAllPilotDetails")]
        public async Task<ActionResult> GetAllPilotDetails()
        {
            var response = await _serviceAssignmentDetailsService.GetAllPilotDetails();
            if (response.StatusCode >= 400)
                return StatusCode(response.StatusCode, response);
            var cType = ((ApiOkResponse)response).Result;
            return Ok(cType);
        }

        [HttpGet("GetAllPilotDetailsByAssignmentId/{id}")]
        public async Task<ActionResult> GetAllPilotDetailsByAssignmentId(long id)
        {
            var response = await _serviceAssignmentDetailsService.GetAllPilotDetailsByAssignmentId(id);
            if (response.StatusCode >= 400)
                return StatusCode(response.StatusCode, response);
            var cType = ((ApiOkResponse)response).Result;
            return Ok(cType);
        }

        [HttpGet("GetPilotDetailById/{id}")]
        public async Task<ActionResult> GetPilotDetailById(long id)
        {
            var response = await _serviceAssignmentDetailsService.GetPilotDetailById(id);
            if (response.StatusCode >= 400)
                return StatusCode(response.StatusCode, response);
            var Rank = ((ApiOkResponse)response).Result;
            return Ok(Rank);
        }

        [HttpPost("AddNewPilotDetail")]
        public async Task<ActionResult> AddNewPilotDetail(PilotServiceAssignmentDetailsReceivingDTO ReceivingDTO)
        {
            var response = await _serviceAssignmentDetailsService.AddPilotDetail(HttpContext, ReceivingDTO);

            if (response.StatusCode >= 400)
                return StatusCode(response.StatusCode, response);
            var rank = ((ApiOkResponse)response).Result;
            return Ok(rank);
        }

        [HttpPut("UpdatePilotDetailById/{id}")]
        public async Task<IActionResult> UpdatePilotDetailById(long id, PilotServiceAssignmentDetailsReceivingDTO Receiving)
        {
            var response = await _serviceAssignmentDetailsService.UpdatePilotDetail(HttpContext, id, Receiving);
            if (response.StatusCode >= 400)
                return StatusCode(response.StatusCode, response);
            var type = ((ApiOkResponse)response).Result;
            return Ok(type);
        }
        [HttpDelete("DeletePilotDetailById/{id}")]
        public async Task<ActionResult> DeletePilotDetailById(int id)
        {
            var response = await _serviceAssignmentDetailsService.DeletePilotDetail(id);
            return StatusCode(response.StatusCode);
        }

        //Vehicle
        [HttpGet("GetAllVehicleDetails")]
        public async Task<ActionResult> GetAllVehicleDetails()
        {
            var response = await _serviceAssignmentDetailsService.GetAllVehicleDetails();
            if (response.StatusCode >= 400)
                return StatusCode(response.StatusCode, response);
            var cType = ((ApiOkResponse)response).Result;
            return Ok(cType);
        }

        [HttpGet("GetAllVehicleDetailsByAssignmentId/{id}")]
        public async Task<ActionResult> GetAllVehicleDetailsByAssignmentId(long id)
        {
            var response = await _serviceAssignmentDetailsService.GetAllVehicleDetailsByAssignmentId(id);
            if (response.StatusCode >= 400)
                return StatusCode(response.StatusCode, response);
            var cType = ((ApiOkResponse)response).Result;
            return Ok(cType);
        }

        [HttpGet("GetVehicleDetailById/{id}")]
        public async Task<ActionResult> GetVehicleDetailById(long id)
        {
            var response = await _serviceAssignmentDetailsService.GetVehicleDetailById(id);
            if (response.StatusCode >= 400)
                return StatusCode(response.StatusCode, response);
            var Rank = ((ApiOkResponse)response).Result;
            return Ok(Rank);
        }

        [HttpPost("AddNewVehicleDetail")]
        public async Task<ActionResult> AddNewVehicleDetail(VehicleServiceAssignmentDetailsReceivingDTO ReceivingDTO)
        {
            var response = await _serviceAssignmentDetailsService.AddVehicleDetail(HttpContext, ReceivingDTO);

            if (response.StatusCode >= 400)
                return StatusCode(response.StatusCode, response);
            var rank = ((ApiOkResponse)response).Result;
            return Ok(rank);
        }

        [HttpPut("UpdateVehicleDetailById/{id}")]
        public async Task<IActionResult> UpdateVehicleDetailById(long id, VehicleServiceAssignmentDetailsReceivingDTO Receiving)
        {
            var response = await _serviceAssignmentDetailsService.UpdateVehicleDetail(HttpContext, id, Receiving);
            if (response.StatusCode >= 400)
                return StatusCode(response.StatusCode, response);
            var type = ((ApiOkResponse)response).Result;
            return Ok(type);
        }
        [HttpDelete("DeleteVehicleDetailById/{id}")]
        public async Task<ActionResult> DeleteVehicleDetailById(int id)
        {
            var response = await _serviceAssignmentDetailsService.DeleteVehicleDetail(id);
            return StatusCode(response.StatusCode);
        }

        //Passenger
        [HttpGet("GetAllPassengers")]
        public async Task<ActionResult> GetAllPassengers()
        {
            var response = await _serviceAssignmentDetailsService.GetAllPassengers();
            if (response.StatusCode >= 400)
                return StatusCode(response.StatusCode, response);
            var cType = ((ApiOkResponse)response).Result;
            return Ok(cType);
        }

        [HttpGet("GetAllPassengersByAssignmentId/{id}")]
        public async Task<ActionResult> GetAllPassengersByAssignmentId(long id)
        {
            var response = await _serviceAssignmentDetailsService.GetAllPassengersByAssignmentId(id);
            if (response.StatusCode >= 400)
                return StatusCode(response.StatusCode, response);
            var cType = ((ApiOkResponse)response).Result;
            return Ok(cType);
        }

        [HttpGet("GetPassengerById/{id}")]
        public async Task<ActionResult> GetPassengerById(long id)
        {
            var response = await _serviceAssignmentDetailsService.GetPassengerById(id);
            if (response.StatusCode >= 400)
                return StatusCode(response.StatusCode, response);
            var Rank = ((ApiOkResponse)response).Result;
            return Ok(Rank);
        }

        [HttpPost("AddNewPassenger")]
        public async Task<ActionResult> AddNewPassenger(PassengerReceivingDTO ReceivingDTO)
        {
            var response = await _serviceAssignmentDetailsService.AddPassenger(HttpContext, ReceivingDTO);

            if (response.StatusCode >= 400)
                return StatusCode(response.StatusCode, response);
            var rank = ((ApiOkResponse)response).Result;
            return Ok(rank);
        }

        [HttpPut("UpdatePassengerById/{id}")]
        public async Task<IActionResult> UpdatePassengerById(long id, PassengerReceivingDTO Receiving)
        {
            var response = await _serviceAssignmentDetailsService.UpdatePassenger(HttpContext, id, Receiving);
            if (response.StatusCode >= 400)
                return StatusCode(response.StatusCode, response);
            var type = ((ApiOkResponse)response).Result;
            return Ok(type);
        }
        [HttpDelete("DeletePassengerById/{id}")]
        public async Task<ActionResult> DeletePassengerById(int id)
        {
            var response = await _serviceAssignmentDetailsService.DeletePassenger(id);
            return StatusCode(response.StatusCode);
        }
    }
}
