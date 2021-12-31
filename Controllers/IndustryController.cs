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
    public class IndustryController : ControllerBase
    {
        private readonly IIndustryService _industryService;

        public IndustryController(IIndustryService industryService)
        {
            this._industryService = industryService;
        }

        [HttpGet("")]
        public async Task<ApiCommonResponse> GetIndustry()
        {
            var response = await _industryService.GetAllIndustry();
            if (response.StatusCode >= 400)
                return StatusCode(response.StatusCode, response);
            var industry = ((ApiOkResponse)response).Result;
            return Ok(industry);
        }

        [HttpPost("")]
        public async Task<ApiCommonResponse> AddNewIndustry(IndustryReceivingDTO industryReceiving)
        {
            var response = await _industryService.AddIndustry(HttpContext, industryReceiving);
            if (response.StatusCode >= 400)
                return StatusCode(response.StatusCode, response);
            var industry = ((ApiOkResponse)response).Result;
            return Ok(industry);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateById(long id, IndustryReceivingDTO industryReceiving)
        {
            var response = await _industryService.UpdateIndustry(HttpContext, id, industryReceiving);
            if (response.StatusCode >= 400)
                return StatusCode(response.StatusCode, response);
            var industry = ((ApiOkResponse)response).Result;
            return Ok(industry);
        }

        [HttpDelete("{id}")]
        public async Task<ApiCommonResponse> DeleteById(int id)
        {
            var response = await _industryService.DeleteIndustry(id);
            return StatusCode(response.StatusCode);
        }

    }
}