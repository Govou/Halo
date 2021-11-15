﻿using HaloBiz.DTOs.ApiDTOs;
using HaloBiz.DTOs.ReceivingDTOs;
using HaloBiz.MyServices;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HaloBiz.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class ArmedEscortRegistrationController : ControllerBase
    {
        private readonly IArmedEscortRegistrationService _armedEscortService;

        public ArmedEscortRegistrationController(IArmedEscortRegistrationService armedEscortService)
        {
            _armedEscortService = armedEscortService;
        }

        [HttpGet("GetAllArmedEscortProfiless")]
        public async Task<ActionResult> GetAllArmedEscortProfiless()
        {
            var response = await _armedEscortService.GetAllArmedEscorts();
            if (response.StatusCode >= 400)
                return StatusCode(response.StatusCode, response);
            var cRank = ((ApiOkResponse)response).Result;
            return Ok(cRank);
        }

        [HttpGet("GetProfileById/{id}")]
        public async Task<ActionResult> GetProfileById(long id)
        {
            var response = await _armedEscortService.GetArmedEscortById(id);
            if (response.StatusCode >= 400)
                return StatusCode(response.StatusCode, response);
            var Rank = ((ApiOkResponse)response).Result;
            return Ok(Rank);
        }

        [HttpPost("AddNewProfile")]
        public async Task<ActionResult> AddNewProfile(ArmedEscortProfileReceivingDTO ReceivingDTO)
        {
            var response = await _armedEscortService.AddArmedEscort(HttpContext, ReceivingDTO);
            if (response.StatusCode >= 400)
                return StatusCode(response.StatusCode, response);
            var type = ((ApiOkResponse)response).Result;
            return Ok(type);
        }

        [HttpPut("UpdateProfileById/{id}")]
        public async Task<IActionResult> UpdateProfileById(long id, ArmedEscortProfileReceivingDTO Receiving)
        {
            var response = await _armedEscortService.UpdateArmedEscort(HttpContext, id, Receiving);
            if (response.StatusCode >= 400)
                return StatusCode(response.StatusCode, response);
            var type = ((ApiOkResponse)response).Result;
            return Ok(type);
        }

        [HttpDelete("DeleteProfileById/{id}")]
        public async Task<ActionResult> DeleteProfileById(int id)
        {
            var response = await _armedEscortService.DeleteArmedEscort(id);
            return StatusCode(response.StatusCode);
        }
    }
}