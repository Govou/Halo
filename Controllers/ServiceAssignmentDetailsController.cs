using HaloBiz.DTOs.ApiDTOs;
using HaloBiz.DTOs.ReceivingDTOs;
using HaloBiz.MyServices;
using HaloBiz.MyServices.LAMS;
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
        private readonly IContractServiceService _contractServiceService;

        public ServiceAssignmentDetailsController(IServiceAssignmentDetailsService serviceAssignmentDetailsService, IContractServiceService contractServiceService)
        {
            _serviceAssignmentDetailsService = serviceAssignmentDetailsService;
            _contractServiceService = contractServiceService;
        }

      

        //ArmedEscort
        [HttpGet("GetAllArmedEscortDetails")]
        public async Task<ApiCommonResponse> GetAllArmedEscortDetails()
        {
            return await _serviceAssignmentDetailsService.GetAllArmedEscortDetails();
            //if (response.StatusCode >= 400)
            //    return StatusCode(response.StatusCode, response);
            //var cType = ((ApiOkResponse)response).Result;
            //return Ok(cType);
        }

        [HttpGet("GetAllArmedEscortDetailsByAssignmentId/{id}")]
        public async Task<ApiCommonResponse> GetAllArmedEscortDetailsByAssignmentId(long id)
        {
            return await _serviceAssignmentDetailsService.GetAllArmedEscortDetailsByAssignmentId(id);
            //if (response.StatusCode >= 400)
            //    return StatusCode(response.StatusCode, response);
            //var cType = ((ApiOkResponse)response).Result;
            //return Ok(cType);
        }

        [HttpGet("GetArmedEscortDetailById/{id}")]
        public async Task<ApiCommonResponse> GetArmedEscortDetailById(long id)
        {
            return await _serviceAssignmentDetailsService.GetArmedEscortDetailById(id);
            //if (response.StatusCode >= 400)
            //    return StatusCode(response.StatusCode, response);
            //var Rank = ((ApiOkResponse)response).Result;
            //return Ok(Rank);
        }

        [HttpPost("AddNewArmedEscortDetail")]
        public async Task<ApiCommonResponse> AddNewArmedEscortDetail(ArmedEscortServiceAssignmentDetailsReceivingDTO ReceivingDTO)
        {
            return await _serviceAssignmentDetailsService.AddArmedEscortDetail(HttpContext, ReceivingDTO);

            //if (response.StatusCode >= 400)
            //    return StatusCode(response.StatusCode, response);
            //var rank = ((ApiOkResponse)response).Result;
            //return Ok(rank);
        }

        [HttpPut("UpdateArmedEscortDetailById/{id}")]
        public async Task<ApiCommonResponse> UpdateArmedEscortDetailById(long id, ArmedEscortServiceAssignmentDetailsReceivingDTO Receiving)
        {
            return await _serviceAssignmentDetailsService.UpdateArmedEscortDetail(HttpContext, id, Receiving);
            //if (response.StatusCode >= 400)
            //    return StatusCode(response.StatusCode, response);
            //var type = ((ApiOkResponse)response).Result;
            //return Ok(type);
        }
        [HttpDelete("DeleteArmedEscortDetailById/{id}")]
        public async Task<ApiCommonResponse> DeleteArmedEscortDetailById(int id)
        {
            return await _serviceAssignmentDetailsService.DeleteArmedEscortDetail(id);
            //return StatusCode(response.StatusCode);
        }

        //Commander
        [HttpGet("GetAllCommanderDetails")]
        public async Task<ApiCommonResponse> GetAllCommanderDetails()
        {
            return await _serviceAssignmentDetailsService.GetAllCommanderDetails();
            //if (response.StatusCode >= 400)
            //    return StatusCode(response.StatusCode, response);
            //var cType = ((ApiOkResponse)response).Result;
            //return Ok(cType);
        }

        [HttpGet("GetAllCommanderDetailsByAssignmentId/{id}")]
        public async Task<ApiCommonResponse> GetAllCommanderDetailsByAssignmentId(long id)
        {
            return await _serviceAssignmentDetailsService.GetAllCommanderDetailsByAssignmentId(id);
            //if (response.StatusCode >= 400)
            //    return StatusCode(response.StatusCode, response);
            //var cType = ((ApiOkResponse)response).Result;
            //return Ok(cType);
        }

        [HttpGet("GetCommanderDetailById/{id}")]
        public async Task<ApiCommonResponse> GetCommanderDetailById(long id)
        {
            return await _serviceAssignmentDetailsService.GetCommanderDetailById(id);
            //if (response.StatusCode >= 400)
            //    return StatusCode(response.StatusCode, response);
            //var Rank = ((ApiOkResponse)response).Result;
            //return Ok(Rank);
        }

        [HttpPost("AddNewCommanderDetail")]
        public async Task<ApiCommonResponse> AddNewCommanderDetail(CommanderServiceAssignmentDetailsReceivingDTO ReceivingDTO)
        {
            return await _serviceAssignmentDetailsService.AddCommanderDetail(HttpContext, ReceivingDTO);

            //if (response.StatusCode >= 400)
            //    return StatusCode(response.StatusCode, response);
            //var rank = ((ApiOkResponse)response).Result;
            //return Ok(rank);
        }

        [HttpPut("UpdateCommanderDetailById/{id}")]
        public async Task<ApiCommonResponse> UpdateCommanderDetailById(long id, CommanderServiceAssignmentDetailsReceivingDTO Receiving)
        {
            return await _serviceAssignmentDetailsService.UpdateCommanderDetail(HttpContext, id, Receiving);
            //if (response.StatusCode >= 400)
            //    return StatusCode(response.StatusCode, response);
            //var type = ((ApiOkResponse)response).Result;
            //return Ok(type);
        }
        [HttpDelete("DeleteCommanderDetailById/{id}")]
        public async Task<ApiCommonResponse> DeleteCommanderDetailById(int id)
        {
            return await _serviceAssignmentDetailsService.DeleteCommanderDetail(id);
            //return StatusCode(response.StatusCode);
        }

        //Pilot
        [HttpGet("GetAllPilotDetails")]
        public async Task<ApiCommonResponse> GetAllPilotDetails()
        {
            return await _serviceAssignmentDetailsService.GetAllPilotDetails();
            //if (response.StatusCode >= 400)
            //    return StatusCode(response.StatusCode, response);
            //var cType = ((ApiOkResponse)response).Result;
            //return Ok(cType);
        }

        [HttpGet("GetAllPilotDetailsByAssignmentId/{id}")]
        public async Task<ApiCommonResponse> GetAllPilotDetailsByAssignmentId(long id)
        {
            return await _serviceAssignmentDetailsService.GetAllPilotDetailsByAssignmentId(id);
            //if (response.StatusCode >= 400)
            //    return StatusCode(response.StatusCode, response);
            //var cType = ((ApiOkResponse)response).Result;
            //return Ok(cType);
        }

        [HttpGet("GetPilotDetailById/{id}")]
        public async Task<ApiCommonResponse> GetPilotDetailById(long id)
        {
            return await _serviceAssignmentDetailsService.GetPilotDetailById(id);
            //if (response.StatusCode >= 400)
            //    return StatusCode(response.StatusCode, response);
            //var Rank = ((ApiOkResponse)response).Result;
            //return Ok(Rank);
        }

        [HttpPost("AddNewPilotDetail")]
        public async Task<ApiCommonResponse> AddNewPilotDetail(PilotServiceAssignmentDetailsReceivingDTO ReceivingDTO)
        {
            return await _serviceAssignmentDetailsService.AddPilotDetail(HttpContext, ReceivingDTO);

            //if (response.StatusCode >= 400)
            //    return StatusCode(response.StatusCode, response);
            //var rank = ((ApiOkResponse)response).Result;
            //return Ok(rank);
        }

        [HttpPut("UpdatePilotDetailById/{id}")]
        public async Task<ApiCommonResponse> UpdatePilotDetailById(long id, PilotServiceAssignmentDetailsReceivingDTO Receiving)
        {
            return await _serviceAssignmentDetailsService.UpdatePilotDetail(HttpContext, id, Receiving);
            //if (response.StatusCode >= 400)
            //    return StatusCode(response.StatusCode, response);
            //var type = ((ApiOkResponse)response).Result;
            //return Ok(type);
        }
        [HttpDelete("DeletePilotDetailById/{id}")]
        public async Task<ApiCommonResponse> DeletePilotDetailById(int id)
        {
            return await _serviceAssignmentDetailsService.DeletePilotDetail(id);
            //return StatusCode(response.StatusCode);
        }

        //Vehicle
        [HttpGet("GetAllVehicleDetails")]
        public async Task<ApiCommonResponse> GetAllVehicleDetails()
        {
            return await _serviceAssignmentDetailsService.GetAllVehicleDetails();
            //if (response.StatusCode >= 400)
            //    return StatusCode(response.StatusCode, response);
            //var cType = ((ApiOkResponse)response).Result;
            //return Ok(cType);
        }

        [HttpGet("GetAllVehicleDetailsByAssignmentId/{id}")]
        public async Task<ApiCommonResponse> GetAllVehicleDetailsByAssignmentId(long id)
        {
            return await _serviceAssignmentDetailsService.GetAllVehicleDetailsByAssignmentId(id);
            //if (response.StatusCode >= 400)
            //    return StatusCode(response.StatusCode, response);
            //var cType = ((ApiOkResponse)response).Result;
            //return Ok(cType);
        }

        [HttpGet("GetVehicleDetailById/{id}")]
        public async Task<ApiCommonResponse> GetVehicleDetailById(long id)
        {
            return await _serviceAssignmentDetailsService.GetVehicleDetailById(id);
            //if (response.StatusCode >= 400)
            //    return StatusCode(response.StatusCode, response);
            //var Rank = ((ApiOkResponse)response).Result;
            //return Ok(Rank);
        }

        [HttpPost("AddNewVehicleDetail")]
        public async Task<ApiCommonResponse> AddNewVehicleDetail(VehicleServiceAssignmentDetailsReceivingDTO ReceivingDTO)
        {
            return await _serviceAssignmentDetailsService.AddVehicleDetail(HttpContext, ReceivingDTO);

            //if (response.StatusCode >= 400)
            //    return StatusCode(response.StatusCode, response);
            //var rank = ((ApiOkResponse)response).Result;
            //return Ok(rank);
        }

        [HttpPut("UpdateVehicleDetailById/{id}")]
        public async Task<ApiCommonResponse> UpdateVehicleDetailById(long id, VehicleServiceAssignmentDetailsReceivingDTO Receiving)
        {
            return await _serviceAssignmentDetailsService.UpdateVehicleDetail(HttpContext, id, Receiving);
            //if (response.StatusCode >= 400)
            //    return StatusCode(response.StatusCode, response);
            //var type = ((ApiOkResponse)response).Result;
            //return Ok(type);
        }
        [HttpDelete("DeleteVehicleDetailById/{id}")]
        public async Task<ApiCommonResponse> DeleteVehicleDetailById(int id)
        {
            return await _serviceAssignmentDetailsService.DeleteVehicleDetail(id);
            //return StatusCode(response.StatusCode);
        }

        //Passenger
        [HttpGet("GetAllPassengers")]
        public async Task<ApiCommonResponse> GetAllPassengers()
        {
            return await _serviceAssignmentDetailsService.GetAllPassengers();
            //if (response.StatusCode >= 400)
            //    return StatusCode(response.StatusCode, response);
            //var cType = ((ApiOkResponse)response).Result;
            //return Ok(cType);
        }

        [HttpGet("GetAllPassengersByAssignmentId/{id}")]
        public async Task<ApiCommonResponse> GetAllPassengersByAssignmentId(long id)
        {
            return await _serviceAssignmentDetailsService.GetAllPassengersByAssignmentId(id);
            //if (response.StatusCode >= 400)
            //    return StatusCode(response.StatusCode, response);
            //var cType = ((ApiOkResponse)response).Result;
            //return Ok(cType);
        }

        [HttpGet("GetPassengerById/{id}")]
        public async Task<ApiCommonResponse> GetPassengerById(long id)
        {
            return await _serviceAssignmentDetailsService.GetPassengerById(id);
            //if (response.StatusCode >= 400)
            //    return StatusCode(response.StatusCode, response);
            //var Rank = ((ApiOkResponse)response).Result;
            //return Ok(Rank);
        }

        [HttpPost("AddNewPassenger")]
        public async Task<ApiCommonResponse> AddNewPassenger(PassengerReceivingDTO ReceivingDTO)
        {
            return await _serviceAssignmentDetailsService.AddPassenger(HttpContext, ReceivingDTO);

            //if (response.StatusCode >= 400)
            //    return StatusCode(response.StatusCode, response);
            //var rank = ((ApiOkResponse)response).Result;
            //return Ok(rank);
        }

        [HttpPut("UpdatePassengerById/{id}")]
        public async Task<ApiCommonResponse> UpdatePassengerById(long id, PassengerReceivingDTO Receiving)
        {
            return await _serviceAssignmentDetailsService.UpdatePassenger(HttpContext, id, Receiving);
            //if (response.StatusCode >= 400)
            //    return StatusCode(response.StatusCode, response);
            //var type = ((ApiOkResponse)response).Result;
            //return Ok(type);
        }
        [HttpDelete("DeletePassengerById/{id}")]
        public async Task<ApiCommonResponse> DeletePassengerById(int id)
        {
            return await _serviceAssignmentDetailsService.DeletePassenger(id);
            //return StatusCode(response.StatusCode);
        }
        //ArmedEscort
        [HttpGet("GetAllContractServices")]
        public async Task<ApiCommonResponse> GetAllContractServices()
        {
            return await _contractServiceService.GetAllContractsServcie();
            //if (response.StatusCode >= 400)
            //    return StatusCode(response.StatusCode, response);
            //var cType = ((ApiOkResponse)response).Result;
            //return Ok(cType);
        }
    }
}
