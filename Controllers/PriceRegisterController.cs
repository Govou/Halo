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
    public class PriceRegisterController : ControllerBase
    {
        private readonly IPriceRegisterService _priceRegisterService;

        public PriceRegisterController(IPriceRegisterService priceRegisterService)
        {
            _priceRegisterService = priceRegisterService;
        }

        [HttpGet("GetAllPriceRegisters")]
        public async Task<ApiCommonResponse> GetAllPriceRegisters()
        {
            var response = await _priceRegisterService.GetAllPriceRegisters();
            if (response.StatusCode >= 400)
                return StatusCode(response.StatusCode, response);
            var priceReg = ((ApiOkResponse)response).Result;
            return Ok(priceReg);
        }

        [HttpGet("GetAllPriceRegistersByRouteId/{routeId}")]
        public async Task<ApiCommonResponse> GetAllPriceRegistersByRouteId(long routeId)
        {
            var response = await _priceRegisterService.GetAllPriceRegistersByRouteId(routeId);
            if (response.StatusCode >= 400)
                return StatusCode(response.StatusCode, response);
            var priceReg = ((ApiOkResponse)response).Result;
            return Ok(priceReg);
        }
        [HttpGet("GetPriceRegisterById/{id}")]
        public async Task<ApiCommonResponse> GetPriceRegisterById(long id)
        {
            var response = await _priceRegisterService.GetPriceRegisterId(id);
            if (response.StatusCode >= 400)
                return StatusCode(response.StatusCode, response);
            var Rank = ((ApiOkResponse)response).Result;
            return Ok(Rank);
        }

        [HttpPost("AddNewPriceRegister")]
        public async Task<ApiCommonResponse> AddNewPriceRegister(PriceRegisterReceivingDTO ReceivingDTO)
        {
            var response = await _priceRegisterService.AddPriceRegister(HttpContext, ReceivingDTO);
            if (response.StatusCode >= 400)
                return StatusCode(response.StatusCode, response);
            var type = ((ApiOkResponse)response).Result;
            return Ok(type);
        }

        [HttpPut("UpdatePriceRegisterById/{id}")]
        public async Task<IActionResult> UpdateTypeById(long id, PriceRegisterReceivingDTO Receiving)
        {
            var response = await _priceRegisterService.UpdatePriceRegister(HttpContext, id, Receiving);
            if (response.StatusCode >= 400)
                return StatusCode(response.StatusCode, response);
            var type = ((ApiOkResponse)response).Result;
            return Ok(type);
        }

        [HttpDelete("DeletePriceRegisterById/{id}")] //{id}
        public async Task<ApiCommonResponse> DeletePriceRegisterById(int id)
        {
            var response = await _priceRegisterService.DeletePriceRegister(id);
            return StatusCode(response.StatusCode);
        }
    }
}
