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
//using Controllers.Models;

namespace HaloBiz.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class EngagementTypeController : ControllerBase
    {
        private readonly IEngagementTypeService _engagementTypeService;

        public EngagementTypeController(IEngagementTypeService engagementTypeService)
        {
            this._engagementTypeService = engagementTypeService;
        }

        [HttpGet("")]
        public async Task<ApiCommonResponse> GetEngagementType()
        {
            var response = await _engagementTypeService.GetAllEngagementType();
            if (response.StatusCode >= 400)
                return StatusCode(response.StatusCode, response);
            var engagementType = ((ApiOkResponse)response).Result;
            return Ok(engagementType);
        }

        [HttpPost("")]
        public async Task<ApiCommonResponse> AddNewEngagementType(EngagementTypeReceivingDTO engagementTypeReceiving)
        {
            var response = await _engagementTypeService.AddEngagementType(HttpContext, engagementTypeReceiving);
            if (response.StatusCode >= 400)
                return StatusCode(response.StatusCode, response);
            var engagementType = ((ApiOkResponse)response).Result;
            return Ok(engagementType);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateById(long id, EngagementTypeReceivingDTO engagementTypeReceiving)
        {
            var response = await _engagementTypeService.UpdateEngagementType(HttpContext, id, engagementTypeReceiving);
            if (response.StatusCode >= 400)
                return StatusCode(response.StatusCode, response);
            var engagementType = ((ApiOkResponse)response).Result;
            return Ok(engagementType);
        }

        [HttpDelete("{id}")]
        public async Task<ApiCommonResponse> DeleteById(int id)
        {
            var response = await _engagementTypeService.DeleteEngagementType(id);
            return StatusCode(response.StatusCode);
        }

    }
}