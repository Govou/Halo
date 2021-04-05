using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HaloBiz.DTOs.ApiDTOs;
using HaloBiz.DTOs.ReceivingDTOs;
using HaloBiz.DTOs.ReceivingDTOs.LAMS;
using HaloBiz.DTOs.TransferDTOs.LAMS;
using HaloBiz.MyServices.LAMS;
using Microsoft.AspNetCore.Mvc;
//using Controllers.Models;

namespace Controllers.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class EndorsementController : ControllerBase
    {
        private readonly IContractServiceForEndorsementService _contractServiceForEndorsementService;

        public EndorsementController(IContractServiceForEndorsementService contractServiceForEndorsementService)
        {
            this._contractServiceForEndorsementService = contractServiceForEndorsementService;
        }

        [HttpGet("GetEndorsementDetails/{endorsementId}")]
        public async Task<ActionResult> GetEndorsementDetails(long endorsementId)
        {
            var response = await _contractServiceForEndorsementService.GetEndorsementDetailsById(endorsementId);
            if (response.StatusCode >= 400)
                return StatusCode(response.StatusCode, response);
            var endorsements = ((ApiOkResponse)response).Result;
            return Ok(endorsements);
        }

        [HttpGet("UnApprovedEndorsements")]
        public async Task<ActionResult> GetDropReason()
        {
            var response = await _contractServiceForEndorsementService.GetUnApprovedContractServiceForEndorsement();
            if (response.StatusCode >= 400)
                return StatusCode(response.StatusCode, response);
            var endorsements = ((ApiOkResponse)response).Result;
            return Ok(endorsements);
        }

        [HttpGet("EndorsementPossibleStartDates/{contractServiceId}")]
        public async Task<ActionResult> GetEndorsementPossibleStartDates(long contractServiceId)
        {
            var response = await _contractServiceForEndorsementService.GetAllPossibleEndorsementStartDate(contractServiceId);
            if (response.StatusCode >= 400)
                return StatusCode(response.StatusCode, response);
            var dates = ((ApiOkResponse)response).Result;
            return Ok(dates);
        }

        [HttpPut("ApproveEndorsement/{id}/{sequence}")]
        public async Task<ActionResult> ApproveEndorsement(long id, long sequence)
        {
            var response = await _contractServiceForEndorsementService.ApproveContractServiceForEndorsement(id, sequence, true);
            if (response.StatusCode >= 400)
                return StatusCode(response.StatusCode, response);
            var endorsements = ((ApiOkResponse)response).Result;
            return Ok(endorsements);
        }

        [HttpPut("DeclineEndorsementApproval/{id}/{sequence}")]
        public async Task<ActionResult> DeclineEndorsementApproval(long id, long sequence)
        {
            var response = await _contractServiceForEndorsementService.ApproveContractServiceForEndorsement(id, sequence, false);
            if (response.StatusCode >= 400)
                return StatusCode(response.StatusCode, response);
            var endorsements = ((ApiOkResponse)response).Result;
            return Ok(endorsements);
        }

        [HttpPost("")]
        public async Task<ActionResult> PostEndorsement(ContractServiceForEndorsementReceivingDto dto)
        {
            var response = await _contractServiceForEndorsementService.AddNewContractServiceForEndorsement(HttpContext, dto);
            if (response.StatusCode >= 400)
                return StatusCode(response.StatusCode, response);
            var endorsements = ((ApiOkResponse)response).Result;
            return Ok(endorsements);
        }

        [HttpPost("Retention")]
        public async Task<ActionResult> PostEndorsement(List<ContractServiceForEndorsementReceivingDto> dto)
        {
            var response = await _contractServiceForEndorsementService.AddNewRetentionContractServiceForEndorsement(HttpContext, dto);
            if (response.StatusCode >= 400)
                return StatusCode(response.StatusCode, response);
            var endorsements = ((ApiOkResponse)response).Result;
            return Ok(endorsements);
        }


        [HttpPut("ConverToContract/{contractServiceForEndorsementId}")]
        public async Task<ActionResult> ConvertEndorsementToCOntract(long contractServiceForEndorsementId)
        {
            var response = await _contractServiceForEndorsementService.ConvertContractServiceForEndorsement(HttpContext ,contractServiceForEndorsementId);
            if (response.StatusCode >= 400)
                return StatusCode(response.StatusCode, response);
            var endorsements = ((ApiOkResponse)response).Result;
            return Ok(endorsements);
        }
    }
}