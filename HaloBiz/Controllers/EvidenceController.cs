using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HaloBiz.DTOs.ApiDTOs;
using HaloBiz.DTOs.ReceivingDTOs;
using HaloBiz.DTOs.TransferDTOs.LAMS;
using HaloBiz.MyServices;
using HaloBiz.MyServices.LAMS;
using Microsoft.AspNetCore.Mvc;

namespace HaloBiz.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class EvidenceController : ControllerBase
    {
        private readonly IEvidenceService _EvidenceService;

        public EvidenceController(IEvidenceService serviceTypeService)
        {
            this._EvidenceService = serviceTypeService;
        }

        [HttpGet("")]
        public async Task<ApiCommonResponse> GetEvidence()
        {
            return await _EvidenceService.GetAllEvidence();
        }
        [HttpGet("caption/{name}")]
        public async Task<ApiCommonResponse> GetByCaption(string name)
        {
            return await _EvidenceService.GetEvidenceByName(name);
        }

        [HttpGet("{id}")]
        public async Task<ApiCommonResponse> GetById(long id)
        {
            return await _EvidenceService.GetEvidenceById(id);
        }

        [HttpPost("")]
        public async Task<ApiCommonResponse> AddNewEvidence(EvidenceReceivingDTO EvidenceReceiving)
        {
            return await _EvidenceService.AddEvidence(HttpContext, EvidenceReceiving);
        }

        [HttpPut("{id}")]
        public async Task<ApiCommonResponse> UpdateById(long id, EvidenceReceivingDTO EvidenceReceiving)
        {
            return await _EvidenceService.UpdateEvidence(HttpContext, id, EvidenceReceiving);
        }

        [HttpDelete("{id}")]
        public async Task<ApiCommonResponse> DeleteById(int id)
        {
            return await _EvidenceService.DeleteEvidence(id);
        }
    }
}
