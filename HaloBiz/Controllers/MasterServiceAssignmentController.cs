using Halobiz.Common.Auths;
using Halobiz.Common.DTOs.ApiDTOs;
using HaloBiz.DTOs.ReceivingDTOs;
using HaloBiz.MyServices;
using Microsoft.AspNetCore.Authorization;
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
    [ModuleName(HalobizModules.SecuredMobility,71)]

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
           
        }

        [HttpGet("GetAllServiceAssignmentMastersAddedToCartByClientId/{clientId}")]
        public async Task<ApiCommonResponse> GetAllServiceAssignmentMastersAddedToCartByClientId(long clientId)
        {
            return await _masterServiceAssignmentService.GetAllMasterServiceAssignmentsByClientId(clientId);

        }

        [HttpGet("GetAllServiceAssignmentMastersScheduleAddedToCartByClientId/{clientId}")]
        public async Task<ApiCommonResponse> GetAllServiceAssignmentMastersScheduleAddedToCartByClientId(long clientId)
        {
            return await _masterServiceAssignmentService.GetAllMasterServiceAssignmentsForScheduleCartByClientId(clientId);

        }

        [HttpGet("GetAllCompletedTripsByClientId/{clientId}")]
        public async Task<ApiCommonResponse> GetAllCompletedTripsByClientId(long clientId)
        {
            return await _masterServiceAssignmentService.GetAllCompletedTripsByClientId(clientId);

        }

        [HttpGet("GetAllCompletedTripsCountByClientId/{clientId}")]
        public async Task<ApiCommonResponse> GetAllCompletedTripsCountByClientId(long clientId)
        {
            return await _masterServiceAssignmentService.GetAllCompletedTripsCountByClientId(clientId);

        }

        [HttpGet("GetAllScheduledServiceAssignmentMasters")]
        public async Task<ApiCommonResponse> GetAllScheduledServiceAssignmentMasters()
        {
            return await _masterServiceAssignmentService.GetAllScheduledMasterServiceAssignments();

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

        [AllowAnonymous]
        [HttpGet("GetServiceAssignmentMasterById/{id}")]
        public async Task<ApiCommonResponse> GetServiceAssignmentMasterById(long id)
        {
            return await _masterServiceAssignmentService.GetMasterServiceAssignmentById(id);
           
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

        }
        [HttpPost("AddNewAutoServiceAssignmentMaster")] 
        public async Task<ApiCommonResponse> AddNewAutoServiceAssignmentMaster(MasterServiceAssignmentReceivingDTO ReceivingDTO)
        {
             return await _masterServiceAssignmentService.AddMasterAutoServiceAssignment(HttpContext, ReceivingDTO);

        }

        [HttpGet("AllocateResourceForScheduledServiceAssignmentByAssignmentId/{id}")]
        public async Task<ApiCommonResponse> AllocateResourceForScheduledServiceAssignmentByAssignmentId(long id)
        {
            return await _masterServiceAssignmentService.AllocateResourceForScheduledServiceAssignment(HttpContext, id);

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
        }

        [HttpPut("UpdateisAddedToCartStatusById/{id}")]
        public async Task<ApiCommonResponse> UpdateisAddedToCartStatusById(long id)
        {
            return await _masterServiceAssignmentService.UpdateisAddedToCartStatus(id);
        }

        [HttpPut("UpdateisPaidForStatusById/{id}")]
        public async Task<ApiCommonResponse> UpdateisPaidForStatusById(long id)
        {
            return await _masterServiceAssignmentService.UpdateisPaidForStatus(id);
        }


        [HttpDelete("DeleteServiceAssignmentMasterById/{id}")]
        public async Task<ApiCommonResponse> DeleteServiceAssignmentMasterById(int id)
        {
            return await _masterServiceAssignmentService.DeleteMasterServiceAssignment(id);
            //return StatusCode(response.StatusCode);
        }

        [HttpDelete("DeleteServiceAssignmentMasterScheduleById/{id}")]
        public async Task<ApiCommonResponse> DeleteServiceAssignmentMasterScheduleById(int id)
        {
            return await _masterServiceAssignmentService.DeleteMasterServiceAssignmentSchedule(id);
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
        [HttpGet("SendPaidConfirmation/{id}")]
        public async Task<ApiCommonResponse> SendPaidConfirmation(long id)
        {
            return await _invoiceService.SendJourneyConfirmation(id);
        }
        [HttpGet("SendEndOfJourneyConfirmation/{id}")]
        public async Task<ApiCommonResponse> SendEndOfJourneyConfirmation(long id)
        {
            return await _invoiceService.SendJourneyEndNotification(id);
        }
    }
}
