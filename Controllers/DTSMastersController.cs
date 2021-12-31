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
    public class DTSMastersController : ControllerBase
    {
        private readonly IDTSMastersService _dTSMastersService;

        public DTSMastersController(IDTSMastersService dTSMastersService)
        {
            _dTSMastersService = dTSMastersService;
        }

        //ArmedEscort
        [HttpGet("GetAllArmedEscortMasters")]
        public async Task<ApiCommonResponse> GetAllArmedEscortMasters()
        {
            var response = await _dTSMastersService.GetAllArmedEscortMasters();
            if (response.StatusCode >= 400)
                return StatusCode(response.StatusCode, response);
            var cType = ((ApiOkResponse)response).Result;
            return Ok(cType);
        }

        [HttpGet("GetArmedEscortMasterById/{id}")]
        public async Task<ApiCommonResponse> GetArmedEscortMasterById(long id)
        {
            var response = await _dTSMastersService.GetArmedEscortMasterById(id);
            if (response.StatusCode >= 400)
                return StatusCode(response.StatusCode, response);
            var Rank = ((ApiOkResponse)response).Result;
            return Ok(Rank);
        }

        [HttpPost("AddNewArmedEscortMaster")]
        public async Task<ApiCommonResponse> AddNewArmedEscortMaster(ArmedEscortDTSMastersReceivingDTO ReceivingDTO)
        {
            var response = await _dTSMastersService.AddArmedEscortMaster(HttpContext, ReceivingDTO);

            if (response.StatusCode >= 400)
                return StatusCode(response.StatusCode, response);
            var rank = ((ApiOkResponse)response).Result;
            return Ok(rank);
        }

        [HttpPut("UpdateArmedEscortMasterById/{id}")]
        public async Task<IActionResult> UpdateArmedEscortMasterById(long id, ArmedEscortDTSMastersReceivingDTO Receiving)
        {
            var response = await _dTSMastersService.UpdateArmedEscortMaster(HttpContext, id, Receiving);
            if (response.StatusCode >= 400)
                return StatusCode(response.StatusCode, response);
            var type = ((ApiOkResponse)response).Result;
            return Ok(type);
        }
        [HttpDelete("DeleteArmedEscortMasterById/{id}")]
        public async Task<ApiCommonResponse> DeleteArmedEscortMasterById(int id)
        {
            var response = await _dTSMastersService.DeleteArmedEscortMaster(id);
            return StatusCode(response.StatusCode);
        }

        //Commander
        [HttpGet("GetAllCommanderMasters")]
        public async Task<ApiCommonResponse> GetAllCommanderMasters()
        {
            var response = await _dTSMastersService.GetAllCommanderMasters();
            if (response.StatusCode >= 400)
                return StatusCode(response.StatusCode, response);
            var cType = ((ApiOkResponse)response).Result;
            return Ok(cType);
        }

        [HttpGet("GetCommanderMasterById/{id}")]
        public async Task<ApiCommonResponse> GetCommanderMasterById(long id)
        {
            var response = await _dTSMastersService.GetCommanderMasterById(id);
            if (response.StatusCode >= 400)
                return StatusCode(response.StatusCode, response);
            var Rank = ((ApiOkResponse)response).Result;
            return Ok(Rank);
        }

        [HttpPost("AddNewCommanderMaster")]
        public async Task<ApiCommonResponse> AddNewCommanderMaster(CommanderDTSMastersReceivingDTO ReceivingDTO)
        {
            var response = await _dTSMastersService.AddCommanderMaster(HttpContext, ReceivingDTO);

            if (response.StatusCode >= 400)
                return StatusCode(response.StatusCode, response);
            var rank = ((ApiOkResponse)response).Result;
            return Ok(rank);
        }

        [HttpPut("UpdateCommanderMasterById/{id}")]
        public async Task<IActionResult> UpdateCommanderMasterById(long id, CommanderDTSMastersReceivingDTO Receiving)
        {
            var response = await _dTSMastersService.UpdateCommanderMaster(HttpContext, id, Receiving);
            if (response.StatusCode >= 400)
                return StatusCode(response.StatusCode, response);
            var type = ((ApiOkResponse)response).Result;
            return Ok(type);
        }
        [HttpDelete("DeleteCommanderMasterById/{id}")]
        public async Task<ApiCommonResponse> DeleteCommanderMasterById(int id)
        {
            var response = await _dTSMastersService.DeleteCommanderMaster(id);
            return StatusCode(response.StatusCode);
        }

        //Pilot
        [HttpGet("GetAllPilotMasters")]
        public async Task<ApiCommonResponse> GetAllPilotMasters()
        {
            var response = await _dTSMastersService.GetAllPilotMasters();
            if (response.StatusCode >= 400)
                return StatusCode(response.StatusCode, response);
            var cType = ((ApiOkResponse)response).Result;
            return Ok(cType);
        }

        [HttpGet("GetPilotMasterById/{id}")]
        public async Task<ApiCommonResponse> GetPilotMasterById(long id)
        {
            var response = await _dTSMastersService.GetPilotMasterById(id);
            if (response.StatusCode >= 400)
                return StatusCode(response.StatusCode, response);
            var Rank = ((ApiOkResponse)response).Result;
            return Ok(Rank);
        }

        [HttpPost("AddNewPilotMaster")]
        public async Task<ApiCommonResponse> AddNewPilotMaster(PilotDTSMastersReceivingDTO ReceivingDTO)
        {
            var response = await _dTSMastersService.AddPilotMaster(HttpContext, ReceivingDTO);

            if (response.StatusCode >= 400)
                return StatusCode(response.StatusCode, response);
            var rank = ((ApiOkResponse)response).Result;
            return Ok(rank);
        }

        [HttpPut("UpdatePilotMasterById/{id}")]
        public async Task<IActionResult> UpdatePilotMasterById(long id, PilotDTSMastersReceivingDTO Receiving)
        {
            var response = await _dTSMastersService.UpdatePilotMaster(HttpContext, id, Receiving);
            if (response.StatusCode >= 400)
                return StatusCode(response.StatusCode, response);
            var type = ((ApiOkResponse)response).Result;
            return Ok(type);
        }
        [HttpDelete("DeletePilotMasterById/{id}")]
        public async Task<ApiCommonResponse> DeletePilotMasterById(int id)
        {
            var response = await _dTSMastersService.DeletePilotMaster(id);
            return StatusCode(response.StatusCode);
        }

        //Vehicle
        [HttpGet("GetAllVehicleMasters")]
        public async Task<ApiCommonResponse> GetAllVehicleMasters()
        {
            var response = await _dTSMastersService.GetAllVehicleMasters();
            if (response.StatusCode >= 400)
                return StatusCode(response.StatusCode, response);
            var cType = ((ApiOkResponse)response).Result;
            return Ok(cType);
        }

        [HttpGet("GetVehicleMasterById/{id}")]
        public async Task<ApiCommonResponse> GetVehicleMasterById(long id)
        {
            var response = await _dTSMastersService.GetVehicleMasterById(id);
            if (response.StatusCode >= 400)
                return StatusCode(response.StatusCode, response);
            var Rank = ((ApiOkResponse)response).Result;
            return Ok(Rank);
        }

        [HttpPost("AddNewVehicleMaster")]
        public async Task<ApiCommonResponse> AddNewVehicleMaster(VehicleDTSMastersReceivingDTO ReceivingDTO)
        {
            var response = await _dTSMastersService.AddVehicleMaster(HttpContext, ReceivingDTO);

            if (response.StatusCode >= 400)
                return StatusCode(response.StatusCode, response);
            var rank = ((ApiOkResponse)response).Result;
            return Ok(rank);
        }

        [HttpPut("UpdateVehicleMasterById/{id}")]
        public async Task<IActionResult> UpdateVehicleMasterById(long id, VehicleDTSMastersReceivingDTO Receiving)
        {
            var response = await _dTSMastersService.UpdateVehicleMaster(HttpContext, id, Receiving);
            if (response.StatusCode >= 400)
                return StatusCode(response.StatusCode, response);
            var type = ((ApiOkResponse)response).Result;
            return Ok(type);
        }
        [HttpDelete("DeleteVehicleMasterById/{id}")]
        public async Task<ApiCommonResponse> DeleteVehicleMasterById(int id)
        {
            var response = await _dTSMastersService.DeleteVehicleMaster(id);
            return StatusCode(response.StatusCode);
        }
    }
}
