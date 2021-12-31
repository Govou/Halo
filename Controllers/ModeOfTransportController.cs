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
    public class ModeOfTransportController : ControllerBase
    {
        private readonly IModeOfTransportService _modeOfTransportService;

        public ModeOfTransportController(IModeOfTransportService modeOfTransportService)
        {
            this._modeOfTransportService = modeOfTransportService;
        }

        [HttpGet("")]
        public async Task<ApiCommonResponse> GetModeOfTransport()
        {
            var response = await _modeOfTransportService.GetAllModeOfTransport();
            if (response.StatusCode >= 400)
                return StatusCode(response.StatusCode, response);
            var modeOfTransport = ((ApiOkResponse)response).Result;
            return Ok(modeOfTransport);
        }
        [HttpGet("caption/{name}")]
        public async Task<ApiCommonResponse> GetByCaption(string name)
        {
            var response = await _modeOfTransportService.GetModeOfTransportByName(name);
            if (response.StatusCode >= 400)
                return StatusCode(response.StatusCode, response);
            var modeOfTransport = ((ApiOkResponse)response).Result;
            return Ok(modeOfTransport);
        }

        [HttpGet("{id}")]
        public async Task<ApiCommonResponse> GetById(long id)
        {
            var response = await _modeOfTransportService.GetModeOfTransportById(id);
            if (response.StatusCode >= 400)
                return StatusCode(response.StatusCode, response);
            var modeOfTransport = ((ApiOkResponse)response).Result;
            return Ok(modeOfTransport);
        }

        [HttpPost("")]
        public async Task<ApiCommonResponse> AddNewModeOfTransport(ModeOfTransportReceivingDTO modeOfTransportReceiving)
        {
            var response = await _modeOfTransportService.AddModeOfTransport(HttpContext, modeOfTransportReceiving);
            if (response.StatusCode >= 400)
                return StatusCode(response.StatusCode, response);
            var modeOfTransport = ((ApiOkResponse)response).Result;
            return Ok(modeOfTransport);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateById(long id, ModeOfTransportReceivingDTO modeOfTransportReceiving)
        {
            var response = await _modeOfTransportService.UpdateModeOfTransport(HttpContext, id, modeOfTransportReceiving);
            if (response.StatusCode >= 400)
                return StatusCode(response.StatusCode, response);
            var modeOfTransport = ((ApiOkResponse)response).Result;
            return Ok(modeOfTransport);
        }

        [HttpDelete("{id}")]
        public async Task<ApiCommonResponse> DeleteById(int id)
        {
            var response = await _modeOfTransportService.DeleteModeOfTransport(id);
            return StatusCode(response.StatusCode);
        }
    }
}