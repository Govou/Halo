using Halobiz.Common.Auths;
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
    [ModuleName(HalobizModules.SecuredMobility,61)]

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
            return await _dTSMastersService.GetAllArmedEscortMasters();
        }

        [HttpGet("GetArmedEscortMasterById/{id}")]
        public async Task<ApiCommonResponse> GetArmedEscortMasterById(long id)
        {
            return await _dTSMastersService.GetArmedEscortMasterById(id);
        }

        [HttpPost("AddNewArmedEscortMaster")]
        public async Task<ApiCommonResponse> AddNewArmedEscortMaster(ArmedEscortDTSMastersReceivingDTO ReceivingDTO)
        {
            return await _dTSMastersService.AddArmedEscortMaster(HttpContext, ReceivingDTO);
        }

        [HttpPut("UpdateArmedEscortMasterById/{id}")]
        public async Task<ApiCommonResponse> UpdateArmedEscortMasterById(long id, ArmedEscortDTSMastersReceivingDTO Receiving)
        {
            return await _dTSMastersService.UpdateArmedEscortMaster(HttpContext, id, Receiving);
        }
        [HttpDelete("DeleteArmedEscortMasterById/{id}")]
        public async Task<ApiCommonResponse> DeleteArmedEscortMasterById(int id)
        {
            return await _dTSMastersService.DeleteArmedEscortMaster(id);
        }

        //Commander
        [HttpGet("GetAllCommanderMasters")]
        public async Task<ApiCommonResponse> GetAllCommanderMasters()
        {
            return await _dTSMastersService.GetAllCommanderMasters();
        }

        [HttpGet("GetCommanderMasterById/{id}")]
        public async Task<ApiCommonResponse> GetCommanderMasterById(long id)
        {
            return await _dTSMastersService.GetCommanderMasterById(id);
        }

        [HttpPost("AddNewCommanderMaster")]
        public async Task<ApiCommonResponse> AddNewCommanderMaster(CommanderDTSMastersReceivingDTO ReceivingDTO)
        {
            return await _dTSMastersService.AddCommanderMaster(HttpContext, ReceivingDTO);
        }

        [HttpPut("UpdateCommanderMasterById/{id}")]
        public async Task<ApiCommonResponse> UpdateCommanderMasterById(long id, CommanderDTSMastersReceivingDTO Receiving)
        {
            return await _dTSMastersService.UpdateCommanderMaster(HttpContext, id, Receiving);
        }
        [HttpDelete("DeleteCommanderMasterById/{id}")]
        public async Task<ApiCommonResponse> DeleteCommanderMasterById(int id)
        {
            return await _dTSMastersService.DeleteCommanderMaster(id);
        }

        //Pilot
        [HttpGet("GetAllPilotMasters")]
        public async Task<ApiCommonResponse> GetAllPilotMasters()
        {
            return await _dTSMastersService.GetAllPilotMasters();
        }

        [HttpGet("GetPilotMasterById/{id}")]
        public async Task<ApiCommonResponse> GetPilotMasterById(long id)
        {
            return await _dTSMastersService.GetPilotMasterById(id);
        }

        [HttpPost("AddNewPilotMaster")]
        public async Task<ApiCommonResponse> AddNewPilotMaster(PilotDTSMastersReceivingDTO ReceivingDTO)
        {
            return await _dTSMastersService.AddPilotMaster(HttpContext, ReceivingDTO);
        }

        [HttpPut("UpdatePilotMasterById/{id}")]
        public async Task<ApiCommonResponse> UpdatePilotMasterById(long id, PilotDTSMastersReceivingDTO Receiving)
        {
            return await _dTSMastersService.UpdatePilotMaster(HttpContext, id, Receiving);
        }
        [HttpDelete("DeletePilotMasterById/{id}")]
        public async Task<ApiCommonResponse> DeletePilotMasterById(int id)
        {
            return await _dTSMastersService.DeletePilotMaster(id);
        }

        //Vehicle
        [HttpGet("GetAllVehicleMasters")]
        public async Task<ApiCommonResponse> GetAllVehicleMasters()
        {
            return await _dTSMastersService.GetAllVehicleMasters();
        }

        [HttpGet("GetVehicleMasterById/{id}")]
        public async Task<ApiCommonResponse> GetVehicleMasterById(long id)
        {
            return await _dTSMastersService.GetVehicleMasterById(id);
        }

        [HttpPost("AddNewVehicleMaster")]
        public async Task<ApiCommonResponse> AddNewVehicleMaster(VehicleDTSMastersReceivingDTO ReceivingDTO)
        {
            return await _dTSMastersService.AddVehicleMaster(HttpContext, ReceivingDTO);
        }

        [HttpPut("UpdateVehicleMasterById/{id}")]
        public async Task<ApiCommonResponse> UpdateVehicleMasterById(long id, VehicleDTSMastersReceivingDTO Receiving)
        {
            return await _dTSMastersService.UpdateVehicleMaster(HttpContext, id, Receiving);
        }
        [HttpDelete("DeleteVehicleMasterById/{id}")]
        public async Task<ApiCommonResponse> DeleteVehicleMasterById(int id)
        {
            return await _dTSMastersService.DeleteVehicleMaster(id);
        }
    }
}
