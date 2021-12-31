using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HaloBiz.DTOs.ApiDTOs;
using HaloBiz.DTOs.ReceivingDTOs;
using HaloBiz.DTOs.TransferDTOs.LAMS;
using HaloBiz.MyServices.LAMS;
using Microsoft.AspNetCore.Mvc;

namespace Controllers.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class EndorsementTypeController : ControllerBase
    {
        private readonly IEndorsementTypeService _endorsementTypeService;

        public EndorsementTypeController(IEndorsementTypeService endorsementTypeService)
        {
            _endorsementTypeService = endorsementTypeService;
        }

        [HttpGet("")]
        public async Task<ApiCommonResponse> GetEndorsementType()
        {
            var response = await _endorsementTypeService.GetAllEndorsementType();
            if (response.StatusCode >= 400)
                return StatusCode(response.StatusCode, response);
            var endorsementType = ((ApiOkResponse)response).Result;
            return Ok(endorsementType);
        }
        [HttpGet("caption/{name}")]
        public async Task<ApiCommonResponse> GetByCaption(string name)
        {
            var response = await _endorsementTypeService.GetEndorsementTypeByName(name);
            if (response.StatusCode >= 400)
                return StatusCode(response.StatusCode, response);
            var endorsementType = ((ApiOkResponse)response).Result;
            return Ok(endorsementType);
        }

        [HttpGet("{id}")]
        public async Task<ApiCommonResponse> GetById(long id)
        {
            var response = await _endorsementTypeService.GetEndorsementTypeById(id);
            if (response.StatusCode >= 400)
                return StatusCode(response.StatusCode, response);
            var endorsementType = ((ApiOkResponse)response).Result;
            return Ok(endorsementType);
        }

        [HttpPost("")]
        public async Task<ApiCommonResponse> AddNewEndorsementType(EndorsementTypeReceivingDTO endorsementTypeReceiving)
        {
            var response = await _endorsementTypeService.AddEndorsementType(HttpContext, endorsementTypeReceiving);
            if (response.StatusCode >= 400)
                return StatusCode(response.StatusCode, response);
            var endorsementType = ((ApiOkResponse)response).Result;
            return Ok(endorsementType);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateById(long id, EndorsementTypeReceivingDTO endorsementTypeReceivingDTO)
        {
            var response = await _endorsementTypeService.UpdateEndorsementType(HttpContext, id, endorsementTypeReceivingDTO);
            if (response.StatusCode >= 400)
                return StatusCode(response.StatusCode, response);
            var endorsementType = ((ApiOkResponse)response).Result;
            return Ok(endorsementType);
        }

        [HttpDelete("{id}")]
        public async Task<ApiCommonResponse> DeleteById(int id)
        {
            var response = await _endorsementTypeService.DeleteEndorsementType(id);
            return StatusCode(response.StatusCode);
        }
    }
}