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
        public async Task<ActionResult> GetAllRules()
        {
            var response = await _businessRuleService.GetAllBusinessRules();
            if (response.StatusCode >= 400)
                return StatusCode(response.StatusCode, response);
            var rule = ((ApiOkResponse)response).Result;
            return Ok(rule);
        }

        [HttpGet("GetRuleById/{id}")]
        public async Task<ActionResult> GetRuleById(long id)
        {
            var response = await _businessRuleService.GetBusinessRuleById(id);
            if (response.StatusCode >= 400)
                return StatusCode(response.StatusCode, response);
            var rule = ((ApiOkResponse)response).Result;
            return Ok(rule);
        }

        [HttpPost("AddNewRule")]
        public async Task<ActionResult> AddNewRule(BusinessRuleReceivingDTO ReceivingDTO)
        {
            var response = await _businessRuleService.AddBusinessRule(HttpContext, ReceivingDTO);
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

        [HttpDelete("DeleteRuleById/{id}")]
        public async Task<ActionResult> DeleteRuleById(int id)
        {
            var response = await _businessRuleService.DeleteBusinessRule(id);
            return StatusCode(response.StatusCode);
        }
    }
}
