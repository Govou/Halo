using HaloBiz.DTOs.ApiDTOs;
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
    public class BusinessRulesController : ControllerBase
    {
        private readonly IBusinessRuleService _businessRuleService;

        public BusinessRulesController(IBusinessRuleService businessRuleService)
        {
            _businessRuleService = businessRuleService;
        }

        [HttpGet("GetAllRules")]
        public async Task<ApiCommonResponse> GetAllRules()
        {
            var response = await _businessRuleService.GetAllBusinessRules();
            if (response.StatusCode >= 400)
                return StatusCode(response.StatusCode, response);
            var rule = ((ApiOkResponse)response).Result;
            return Ok(rule);
        }

        [HttpGet("GetAllPairableRules")]
        public async Task<ApiCommonResponse> GetAllPairableRules()
        {
            var response = await _businessRuleService.GetAllPairableBusinessRules();
            if (response.StatusCode >= 400)
                return StatusCode(response.StatusCode, response);
            var rule = ((ApiOkResponse)response).Result;
            return Ok(rule);
        }


        [HttpGet("GetAllPairables")]
        public async Task<ApiCommonResponse> GetAllPairables()
        {
            var response = await _businessRuleService.GetAllPairables();
            if (response.StatusCode >= 400)
                return StatusCode(response.StatusCode, response);
            var rule = ((ApiOkResponse)response).Result;
            return Ok(rule);
        }

        [HttpGet("GetAllActivePairables")]
        public async Task<ApiCommonResponse> GetAllActivePairables()
        {
            var response = await _businessRuleService.GetAllActivePairables();
            if (response.StatusCode >= 400)
                return StatusCode(response.StatusCode, response);
            var rule = ((ApiOkResponse)response).Result;
            return Ok(rule);
        }

        [HttpGet("GetRuleById/{id}")]
        public async Task<ApiCommonResponse> GetRuleById(long id)
        {
            var response = await _businessRuleService.GetBusinessRuleById(id);
            if (response.StatusCode >= 400)
                return StatusCode(response.StatusCode, response);
            var rule = ((ApiOkResponse)response).Result;
            return Ok(rule);
        }

        [HttpGet("GetPairableById/{id}")]
        public async Task<ApiCommonResponse> GetPairableById(long id)
        {
            var response = await _businessRuleService.GetPairableById(id);
            if (response.StatusCode >= 400)
                return StatusCode(response.StatusCode, response);
            var rule = ((ApiOkResponse)response).Result;
            return Ok(rule);
        }

        [HttpPost("AddNewRule")]
        public async Task<ApiCommonResponse> AddNewRule(BusinessRuleReceivingDTO ReceivingDTO)
        {
            var response = await _businessRuleService.AddBusinessRule(HttpContext, ReceivingDTO);
            if (response.StatusCode >= 400)
                return StatusCode(response.StatusCode, response);
            var rule = ((ApiOkResponse)response).Result;
            return Ok(rule);
        }


        [HttpPost("AddNewPairable")]
        public async Task<ApiCommonResponse> AddNewPairable(BRPairableReceivingDTO ReceivingDTO)
        {
            var response = await _businessRuleService.AddPairable(HttpContext, ReceivingDTO);
            if (response.StatusCode >= 400)
                return StatusCode(response.StatusCode, response);
            var rule = ((ApiOkResponse)response).Result;
            return Ok(rule);
        }

        [HttpPut("UpdateRuleById/{id}")]
        public async Task<IActionResult> UpdateRuleById(long id, BusinessRuleReceivingDTO ReceivingDTO)
        {
            var response = await _businessRuleService.UpdateBusinessRule(HttpContext, id, ReceivingDTO);
            if (response.StatusCode >= 400)
                return StatusCode(response.StatusCode, response);
            var type = ((ApiOkResponse)response).Result;
            return Ok(type);
        }

        [HttpPut("UpdatePairableById/{id}")]
        public async Task<IActionResult> UpdatePairableById(long id, BRPairableReceivingDTO ReceivingDTO)
        {
            var response = await _businessRuleService.UpdatePairable(HttpContext, id, ReceivingDTO);
            if (response.StatusCode >= 400)
                return StatusCode(response.StatusCode, response);
            var type = ((ApiOkResponse)response).Result;
            return Ok(type);
        }

        [HttpDelete("DeleteRuleById/{id}")]
        public async Task<ApiCommonResponse> DeleteRuleById(int id)
        {
            var response = await _businessRuleService.DeleteBusinessRule(id);
            return StatusCode(response.StatusCode);
        }

        [HttpDelete("DeletePairableById/{id}")]
        public async Task<ApiCommonResponse> DeletePairableById(int id)
        {
            var response = await _businessRuleService.DeletePairable(id);
            return StatusCode(response.StatusCode);
        }
    }
}
