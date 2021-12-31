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
    public class ComplaintSourceController : ControllerBase
    {
        private readonly IComplaintSourceService _ComplaintSourceService;

        public ComplaintSourceController(IComplaintSourceService serviceTypeService)
        {
            this._ComplaintSourceService = serviceTypeService;
        }

        [HttpGet("")]
        public async Task<ApiCommonResponse> GetComplaintSource()
        {
            var response = await _ComplaintSourceService.GetAllComplaintSource();
            if (response.StatusCode >= 400)
                return StatusCode(response.StatusCode, response);
            var ComplaintSource = ((ApiOkResponse)response).Result;
            return Ok(ComplaintSource);
        }
        [HttpGet("caption/{name}")]
        public async Task<ApiCommonResponse> GetByCaption(string name)
        {
            var response = await _ComplaintSourceService.GetComplaintSourceByName(name);
            if (response.StatusCode >= 400)
                return StatusCode(response.StatusCode, response);
            var ComplaintSource = ((ApiOkResponse)response).Result;
            return Ok(ComplaintSource);
        }

        [HttpGet("{id}")]
        public async Task<ApiCommonResponse> GetById(long id)
        {
            var response = await _ComplaintSourceService.GetComplaintSourceById(id);
            if (response.StatusCode >= 400)
                return StatusCode(response.StatusCode, response);
            var ComplaintSource = ((ApiOkResponse)response).Result;
            return Ok(ComplaintSource);
        }

        [HttpPost("")]
        public async Task<ApiCommonResponse> AddNewComplaintSource(ComplaintSourceReceivingDTO ComplaintSourceReceiving)
        {
            var response = await _ComplaintSourceService.AddComplaintSource(HttpContext, ComplaintSourceReceiving);
            if (response.StatusCode >= 400)
                return StatusCode(response.StatusCode, response);
            var ComplaintSource = ((ApiOkResponse)response).Result;
            return Ok(ComplaintSource);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateById(long id, ComplaintSourceReceivingDTO ComplaintSourceReceiving)
        {
            var response = await _ComplaintSourceService.UpdateComplaintSource(HttpContext, id, ComplaintSourceReceiving);
            if (response.StatusCode >= 400)
                return StatusCode(response.StatusCode, response);
            var ComplaintSource = ((ApiOkResponse)response).Result;
            return Ok(ComplaintSource);
        }

        [HttpDelete("{id}")]
        public async Task<ApiCommonResponse> DeleteById(int id)
        {
            var response = await _ComplaintSourceService.DeleteComplaintSource(id);
            return StatusCode(response.StatusCode);
        }
    }
}
