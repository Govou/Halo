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
        public async Task<ApiCommonResponse> GetEndorsementDetails(long endorsementId)
        {
            return await _contractServiceForEndorsementService.GetEndorsementDetailsById(endorsementId);
        }

        [HttpGet("UnApprovedEndorsements")]
        public async Task<ApiCommonResponse> GetDropReason()
        {
            return await _contractServiceForEndorsementService.GetUnApprovedContractServiceForEndorsement();
        }

        [HttpGet("GetEndorsementHistory/{contractServiceId}")]
        public async Task<ApiCommonResponse> GetEndorsementHistory(long contractServiceId)
        {
            return await _contractServiceForEndorsementService.GetEndorsementHistory(contractServiceId);
        }

        [HttpGet("EndorsementPossibleStartDates/{contractServiceId}")]
        public async Task<ApiCommonResponse> GetEndorsementPossibleStartDates(long contractServiceId)
        {
            return await _contractServiceForEndorsementService.GetAllPossibleEndorsementStartDate(contractServiceId);
        }

        [HttpPut("ApproveEndorsement/{id}/{sequence}")]
        public async Task<ApiCommonResponse> ApproveEndorsement(long id, long sequence)
        {
            return await _contractServiceForEndorsementService.ApproveContractServiceForEndorsement(id, sequence, true);
        }

        [HttpPut("DeclineEndorsementApproval/{id}/{sequence}")]
        public async Task<ApiCommonResponse> DeclineEndorsementApproval(long id, long sequence)
        {
            return await _contractServiceForEndorsementService.ApproveContractServiceForEndorsement(id, sequence, false);
        }       

        [HttpPost("")]
        public async Task<ApiCommonResponse> PostEndorsement(List<ContractServiceForEndorsementReceivingDto> dto)
        {
            return await _contractServiceForEndorsementService.AddNewRetentionContractServiceForEndorsement(HttpContext, dto);
            
        }


        [HttpPut("ConverToContract/{contractServiceForEndorsementId}")]
        public async Task<ApiCommonResponse> ConvertEndorsementToCOntract(long contractServiceForEndorsementId)
        {
            return await _contractServiceForEndorsementService.ConvertContractServiceForEndorsement(HttpContext ,contractServiceForEndorsementId);
            
        }

        [HttpPut("ConvertDebitCreditNote/{contractServiceForEndorsementId}")]
        public async Task<ApiCommonResponse> ConvertDebitCreditNote(long contractServiceForEndorsementId)
        {
            return await _contractServiceForEndorsementService.ConvertDebitCreditNoteEndorsement(HttpContext, contractServiceForEndorsementId);
            
        }
    }
}