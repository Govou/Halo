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
    public class DTSDetailGenericDaysController : ControllerBase
    {
        private readonly IDTSDetailGenericDaysService _dTSDetailGenericDaysService;
        public DTSDetailGenericDaysController(IDTSDetailGenericDaysService dTSDetailGenericDaysService)
        {
            _dTSDetailGenericDaysService = dTSDetailGenericDaysService;
        }

        //ArmedEscort
        [HttpGet("GetAllArmedEscortGenerics")]
        public async Task<ActionResult> GetAllArmedEscortGenerics()
        {
            var response = await _dTSDetailGenericDaysService.GetAllArmedEscortGenerics();
            if (response.StatusCode >= 400)
                return StatusCode(response.StatusCode, response);
            var cType = ((ApiOkResponse)response).Result;
            return Ok(cType);
        }

        [HttpGet("GetArmedEscortGenericsById/{id}")]
        public async Task<ActionResult> GetArmedEscortGenericsById(long id)
        {
            var response = await _dTSDetailGenericDaysService.GetArmedEscortGenericById(id);
            if (response.StatusCode >= 400)
                return StatusCode(response.StatusCode, response);
            var Rank = ((ApiOkResponse)response).Result;
            return Ok(Rank);
        }
        [HttpGet("GetArmedEscortGenericsByMasterId/{id}")]
        public async Task<ActionResult> GetArmedEscortGenericsByMasterId(long id)
        {
            var response = await _dTSDetailGenericDaysService.GetArmedEscortGenericByMasterId(id);
            if (response.StatusCode >= 400)
                return StatusCode(response.StatusCode, response);
            var Rank = ((ApiOkResponse)response).Result;
            return Ok(Rank);
        }

        [HttpPost("AddNewArmedEscortGeneric")]
        public async Task<ActionResult> AddNewArmedEscortGeneric(ArmedEscortDTSDetailGenericDaysReceivingDTO ReceivingDTO)
        {
            var response = await _dTSDetailGenericDaysService.AddArmedEscortGeneric(HttpContext, ReceivingDTO);

            if (response.StatusCode >= 400)
                return StatusCode(response.StatusCode, response);
            var rank = ((ApiOkResponse)response).Result;
            return Ok(rank);
        }

        [HttpPut("UpdateArmedEscortGenericById/{id}")]
        public async Task<IActionResult> UpdateArmedEscortGenericById(long id, ArmedEscortDTSDetailGenericDaysReceivingDTO Receiving)
        {
            var response = await _dTSDetailGenericDaysService.UpdateArmedEscortGeneric(HttpContext, id, Receiving);
            if (response.StatusCode >= 400)
                return StatusCode(response.StatusCode, response);
            var type = ((ApiOkResponse)response).Result;
            return Ok(type);
        }
        [HttpDelete("DeleteArmedEscortGenericById/{id}")]
        public async Task<ActionResult> DeleteArmedEscortMasterById(int id)
        {
            var response = await _dTSDetailGenericDaysService.DeleteArmedEscortGeneric(id);
            return StatusCode(response.StatusCode);
        }

        //Commander
        [HttpGet("GetAllCommanderGenerics")]
        public async Task<ActionResult> GetAllCommanderGenerics()
        {
            var response = await _dTSDetailGenericDaysService.GetAllCommanderGenerics();
            if (response.StatusCode >= 400)
                return StatusCode(response.StatusCode, response);
            var cType = ((ApiOkResponse)response).Result;
            return Ok(cType);
        }

        [HttpGet("GetCommanderGenericsById/{id}")]
        public async Task<ActionResult> GetCommanderGenericsById(long id)
        {
            var response = await _dTSDetailGenericDaysService.GetCommanderGenericById(id);
            if (response.StatusCode >= 400)
                return StatusCode(response.StatusCode, response);
            var Rank = ((ApiOkResponse)response).Result;
            return Ok(Rank);
        }
        [HttpGet("GetCommanderGenericsByMasterId/{id}")]
        public async Task<ActionResult> GetCommanderGenericsByMasterId(long id)
        {
            var response = await _dTSDetailGenericDaysService.GetCommanderGenericByMasterId(id);
            if (response.StatusCode >= 400)
                return StatusCode(response.StatusCode, response);
            var Rank = ((ApiOkResponse)response).Result;
            return Ok(Rank);
        }

        [HttpPost("AddNewCommanderGeneric")]
        public async Task<ActionResult> AddNewCommanderGeneric(CommanderDTSDetailGenericDaysReceivingDTO ReceivingDTO)
        {
            var response = await _dTSDetailGenericDaysService.AddCommanderGeneric(HttpContext, ReceivingDTO);

            if (response.StatusCode >= 400)
                return StatusCode(response.StatusCode, response);
            var rank = ((ApiOkResponse)response).Result;
            return Ok(rank);
        }

        [HttpPut("UpdateCommanderGenericById/{id}")]
        public async Task<IActionResult> UpdateCommanderGenericById(long id, CommanderDTSDetailGenericDaysReceivingDTO Receiving)
        {
            var response = await _dTSDetailGenericDaysService.UpdateCommanderGeneric(HttpContext, id, Receiving);
            if (response.StatusCode >= 400)
                return StatusCode(response.StatusCode, response);
            var type = ((ApiOkResponse)response).Result;
            return Ok(type);
        }
        [HttpDelete("DeleteCommanderGenericById/{id}")]
        public async Task<ActionResult> DeleteCommanderGenericById(int id)
        {
            var response = await _dTSDetailGenericDaysService.DeleteCommanderGeneric(id);
            return StatusCode(response.StatusCode);
        }

        //Pilot
        [HttpGet("GetAllPilotGenerics")]
        public async Task<ActionResult> GetAllPilotGenerics()
        {
            var response = await _dTSDetailGenericDaysService.GetAllPilotGenerics();
            if (response.StatusCode >= 400)
                return StatusCode(response.StatusCode, response);
            var cType = ((ApiOkResponse)response).Result;
            return Ok(cType);
        }

        [HttpGet("GetPilotGenericsById/{id}")]
        public async Task<ActionResult> GetPilotGenericsById(long id)
        {
            var response = await _dTSDetailGenericDaysService.GetPilotGenericById(id);
            if (response.StatusCode >= 400)
                return StatusCode(response.StatusCode, response);
            var Rank = ((ApiOkResponse)response).Result;
            return Ok(Rank);
        }
        [HttpGet("GetPilotGenericsByMasterId/{id}")]
        public async Task<ActionResult> GetPilotGenericsByMasterId(long id)
        {
            var response = await _dTSDetailGenericDaysService.GetPilotGenericByMasterId(id);
            if (response.StatusCode >= 400)
                return StatusCode(response.StatusCode, response);
            var Rank = ((ApiOkResponse)response).Result;
            return Ok(Rank);
        }

        [HttpPost("AddNewPilotGeneric")]
        public async Task<ActionResult> AddNewPilotGeneric(PilotDTSDetailGenericDaysReceivingDTO ReceivingDTO)
        {
            var response = await _dTSDetailGenericDaysService.AddPilotGeneric(HttpContext, ReceivingDTO);

            if (response.StatusCode >= 400)
                return StatusCode(response.StatusCode, response);
            var rank = ((ApiOkResponse)response).Result;
            return Ok(rank);
        }

        [HttpPut("UpdatePilotGenericById/{id}")]
        public async Task<IActionResult> UpdatePilotGenericById(long id, PilotDTSDetailGenericDaysReceivingDTO Receiving)
        {
            var response = await _dTSDetailGenericDaysService.UpdatePilotGeneric(HttpContext, id, Receiving);
            if (response.StatusCode >= 400)
                return StatusCode(response.StatusCode, response);
            var type = ((ApiOkResponse)response).Result;
            return Ok(type);
        }
        [HttpDelete("DeletePilotGenericById/{id}")]
        public async Task<ActionResult> DeletePilotGenericById(int id)
        {
            var response = await _dTSDetailGenericDaysService.DeletePilotGeneric(id);
            return StatusCode(response.StatusCode);
        }

        //Vehicle
        [HttpGet("GetAllVehicleGenerics")]
        public async Task<ActionResult> GetAllVehicleGenerics()
        {
            var response = await _dTSDetailGenericDaysService.GetAllVehicleGenerics();
            if (response.StatusCode >= 400)
                return StatusCode(response.StatusCode, response);
            var cType = ((ApiOkResponse)response).Result;
            return Ok(cType);
        }

        [HttpGet("GetVehicleGenericsById/{id}")]
        public async Task<ActionResult> GetVehicleGenericsById(long id)
        {
            var response = await _dTSDetailGenericDaysService.GetVehicleGenericById(id);
            if (response.StatusCode >= 400)
                return StatusCode(response.StatusCode, response);
            var Rank = ((ApiOkResponse)response).Result;
            return Ok(Rank);
        }

        [HttpGet("GetVehicleGenericsByMasterId/{id}")]
        public async Task<ActionResult> GetVehicleGenericsByMasterId(long id)
        {
            var response = await _dTSDetailGenericDaysService.GetVehicleGenericByMasterId(id);
            if (response.StatusCode >= 400)
                return StatusCode(response.StatusCode, response);
            var Rank = ((ApiOkResponse)response).Result;
            return Ok(Rank);
        }

        [HttpPost("AddNewVehicleGeneric")]
        public async Task<ActionResult> AddNewVehicleGeneric(VehicleDTSDetailGenericDaysReceivingDTO ReceivingDTO)
        {
            var response = await _dTSDetailGenericDaysService.AddVehicleGeneric(HttpContext, ReceivingDTO);

            if (response.StatusCode >= 400)
                return StatusCode(response.StatusCode, response);
            var rank = ((ApiOkResponse)response).Result;
            return Ok(rank);
        }

        [HttpPut("UpdateVehicleGenericById/{id}")]
        public async Task<IActionResult> UpdateVehicleGenericById(long id, VehicleDTSDetailGenericDaysReceivingDTO Receiving)
        {
            var response = await _dTSDetailGenericDaysService.UpdateVehicleGeneric(HttpContext, id, Receiving);
            if (response.StatusCode >= 400)
                return StatusCode(response.StatusCode, response);
            var type = ((ApiOkResponse)response).Result;
            return Ok(type);
        }
        [HttpDelete("DeleteVehicleGenericById/{id}")]
        public async Task<ActionResult> DeleteVehicleGenericById(int id)
        {
            var response = await _dTSDetailGenericDaysService.DeleteVehicleGeneric(id);
            return StatusCode(response.StatusCode);
        }
    }
}
