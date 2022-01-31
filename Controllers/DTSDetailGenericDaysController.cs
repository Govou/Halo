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
        public async Task<ApiCommonResponse> GetAllArmedEscortGenerics()
        {
            return await _dTSDetailGenericDaysService.GetAllArmedEscortGenerics();
        }

        [HttpGet("GetArmedEscortGenericsById/{id}")]
        public async Task<ApiCommonResponse> GetArmedEscortGenericsById(long id)
        {
            return await _dTSDetailGenericDaysService.GetArmedEscortGenericById(id);
        }
        [HttpGet("GetArmedEscortGenericsByMasterId/{id}")]
        public async Task<ApiCommonResponse> GetArmedEscortGenericsByMasterId(long id)
        {
            return await _dTSDetailGenericDaysService.GetArmedEscortGenericByMasterId(id);
        }

        [HttpPost("AddNewArmedEscortGeneric")]
        public async Task<ApiCommonResponse> AddNewArmedEscortGeneric(ArmedEscortDTSDetailGenericDaysReceivingDTO ReceivingDTO)
        {
            return await _dTSDetailGenericDaysService.AddArmedEscortGeneric(HttpContext, ReceivingDTO);
        }

        [HttpPut("UpdateArmedEscortGenericById/{id}")]
        public async Task<ApiCommonResponse> UpdateArmedEscortGenericById(long id, ArmedEscortDTSDetailGenericDaysReceivingDTO Receiving)
        {
            return await _dTSDetailGenericDaysService.UpdateArmedEscortGeneric(HttpContext, id, Receiving);
        }
        [HttpDelete("DeleteArmedEscortGenericById/{id}")]
        public async Task<ApiCommonResponse> DeleteArmedEscortMasterById(int id)
        {
            return await _dTSDetailGenericDaysService.DeleteArmedEscortGeneric(id);
        }

        //Commander
        [HttpGet("GetAllCommanderGenerics")]
        public async Task<ApiCommonResponse> GetAllCommanderGenerics()
        {
            return await _dTSDetailGenericDaysService.GetAllCommanderGenerics();
        }

        [HttpGet("GetCommanderGenericsById/{id}")]
        public async Task<ApiCommonResponse> GetCommanderGenericsById(long id)
        {
            return await _dTSDetailGenericDaysService.GetCommanderGenericById(id);
        }
        [HttpGet("GetCommanderGenericsByMasterId/{id}")]
        public async Task<ApiCommonResponse> GetCommanderGenericsByMasterId(long id)
        {
            return await _dTSDetailGenericDaysService.GetCommanderGenericByMasterId(id);
        }

        [HttpPost("AddNewCommanderGeneric")]
        public async Task<ApiCommonResponse> AddNewCommanderGeneric(CommanderDTSDetailGenericDaysReceivingDTO ReceivingDTO)
        {
            return await _dTSDetailGenericDaysService.AddCommanderGeneric(HttpContext, ReceivingDTO);
        }

        [HttpPut("UpdateCommanderGenericById/{id}")]
        public async Task<ApiCommonResponse> UpdateCommanderGenericById(long id, CommanderDTSDetailGenericDaysReceivingDTO Receiving)
        {
            return await _dTSDetailGenericDaysService.UpdateCommanderGeneric(HttpContext, id, Receiving);
        }
        [HttpDelete("DeleteCommanderGenericById/{id}")]
        public async Task<ApiCommonResponse> DeleteCommanderGenericById(int id)
        {
            return await _dTSDetailGenericDaysService.DeleteCommanderGeneric(id);
        }

        //Pilot
        [HttpGet("GetAllPilotGenerics")]
        public async Task<ApiCommonResponse> GetAllPilotGenerics()
        {
            return await _dTSDetailGenericDaysService.GetAllPilotGenerics();
        }

        [HttpGet("GetPilotGenericsById/{id}")]
        public async Task<ApiCommonResponse> GetPilotGenericsById(long id)
        {
            return await _dTSDetailGenericDaysService.GetPilotGenericById(id);
        }
        [HttpGet("GetPilotGenericsByMasterId/{id}")]
        public async Task<ApiCommonResponse> GetPilotGenericsByMasterId(long id)
        {
            return await _dTSDetailGenericDaysService.GetPilotGenericByMasterId(id);
        }

        [HttpPost("AddNewPilotGeneric")]
        public async Task<ApiCommonResponse> AddNewPilotGeneric(PilotDTSDetailGenericDaysReceivingDTO ReceivingDTO)
        {
            return await _dTSDetailGenericDaysService.AddPilotGeneric(HttpContext, ReceivingDTO);
        }

        [HttpPut("UpdatePilotGenericById/{id}")]
        public async Task<ApiCommonResponse> UpdatePilotGenericById(long id, PilotDTSDetailGenericDaysReceivingDTO Receiving)
        {
            return await _dTSDetailGenericDaysService.UpdatePilotGeneric(HttpContext, id, Receiving);
        }
        [HttpDelete("DeletePilotGenericById/{id}")]
        public async Task<ApiCommonResponse> DeletePilotGenericById(int id)
        {
            return await _dTSDetailGenericDaysService.DeletePilotGeneric(id);
        }

        //Vehicle
        [HttpGet("GetAllVehicleGenerics")]
        public async Task<ApiCommonResponse> GetAllVehicleGenerics()
        {
            return await _dTSDetailGenericDaysService.GetAllVehicleGenerics();
        }

        [HttpGet("GetVehicleGenericsById/{id}")]
        public async Task<ApiCommonResponse> GetVehicleGenericsById(long id)
        {
            return await _dTSDetailGenericDaysService.GetVehicleGenericById(id);
        }

        [HttpGet("GetVehicleGenericsByMasterId/{id}")]
        public async Task<ApiCommonResponse> GetVehicleGenericsByMasterId(long id)
        {
            return await _dTSDetailGenericDaysService.GetVehicleGenericByMasterId(id);
        }

        [HttpPost("AddNewVehicleGeneric")]
        public async Task<ApiCommonResponse> AddNewVehicleGeneric(VehicleDTSDetailGenericDaysReceivingDTO ReceivingDTO)
        {
            return await _dTSDetailGenericDaysService.AddVehicleGeneric(HttpContext, ReceivingDTO);
        }

        [HttpPut("UpdateVehicleGenericById/{id}")]
        public async Task<ApiCommonResponse> UpdateVehicleGenericById(long id, VehicleDTSDetailGenericDaysReceivingDTO Receiving)
        {
            return await _dTSDetailGenericDaysService.UpdateVehicleGeneric(HttpContext, id, Receiving);
        }
        [HttpDelete("DeleteVehicleGenericById/{id}")]
        public async Task<ApiCommonResponse> DeleteVehicleGenericById(int id)
        {
            return await _dTSDetailGenericDaysService.DeleteVehicleGeneric(id);
        }
    }
}
