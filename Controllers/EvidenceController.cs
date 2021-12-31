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
        public async Task<ActionResult> GetEvidence()
        {
            var response = await _EvidenceService.GetAllEvidence();
            if (response.StatusCode >= 400)
                return StatusCode(response.StatusCode, response);
            var Evidence = ((ApiOkResponse)response).Result;
            return Ok(Evidence);
        }
        [HttpGet("caption/{name}")]
        public async Task<ActionResult> GetByCaption(string name)
        {
            var response = await _EvidenceService.GetEvidenceByName(name);
            if (response.StatusCode >= 400)
                return StatusCode(response.StatusCode, response);
            var Evidence = ((ApiOkResponse)response).Result;
            return Ok(Evidence);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult> GetById(long id)
        {
            var response = await _EvidenceService.GetEvidenceById(id);
            if (response.StatusCode >= 400)
                return StatusCode(response.StatusCode, response);
            var Evidence = ((ApiOkResponse)response).Result;
            return Ok(Evidence);
        }

        [HttpPost("")]
        public async Task<ActionResult> AddNewEvidence(EvidenceReceivingDTO EvidenceReceiving)
        {
            var response = await _EvidenceService.AddEvidence(HttpContext, EvidenceReceiving);
            if (response.StatusCode >= 400)
                return StatusCode(response.StatusCode, response);
            var Evidence = ((ApiOkResponse)response).Result;
            return Ok(Evidence);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateById(long id, EvidenceReceivingDTO EvidenceReceiving)
        {
            var response = await _EvidenceService.UpdateEvidence(HttpContext, id, EvidenceReceiving);
            if (response.StatusCode >= 400)
                return StatusCode(response.StatusCode, response);
            var Evidence = ((ApiOkResponse)response).Result;
            return Ok(Evidence);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteById(int id)
        {
            var response = await _EvidenceService.DeleteEvidence(id);
            return StatusCode(response.StatusCode);
        }
    }
}
