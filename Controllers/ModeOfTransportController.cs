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
            return await _modeOfTransportService.GetAllModeOfTransport();
        }
        [HttpGet("caption/{name}")]
        public async Task<ApiCommonResponse> GetByCaption(string name)
        {
            return await _modeOfTransportService.GetModeOfTransportByName(name);
        }

        [HttpGet("{id}")]
        public async Task<ApiCommonResponse> GetById(long id)
        {
            return await _modeOfTransportService.GetModeOfTransportById(id);
        }

        [HttpPost("")]
        public async Task<ApiCommonResponse> AddNewModeOfTransport(ModeOfTransportReceivingDTO modeOfTransportReceiving)
        {
            return await _modeOfTransportService.AddModeOfTransport(HttpContext, modeOfTransportReceiving);
        }

        [HttpPut("{id}")]
        public async Task<ApiCommonResponse> UpdateById(long id, ModeOfTransportReceivingDTO modeOfTransportReceiving)
        {
            return await _modeOfTransportService.UpdateModeOfTransport(HttpContext, id, modeOfTransportReceiving);
        }

        [HttpDelete("{id}")]
        public async Task<ApiCommonResponse> DeleteById(int id)
        {
            return await _modeOfTransportService.DeleteModeOfTransport(id);
        }
    }
}