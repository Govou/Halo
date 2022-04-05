using Halobiz.Common.Auths;
using Halobiz.Common.DTOs.ApiDTOs;
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
    [ModuleName(HalobizModules.Setups)]

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
            return await _businessRuleService.GetAllBusinessRules();
        }

        [HttpGet("GetAllPairableRules")]
        public async Task<ApiCommonResponse> GetAllPairableRules()
        {
            return await _businessRuleService.GetAllPairableBusinessRules();
        }


        [HttpGet("GetAllPairables")]
        public async Task<ApiCommonResponse> GetAllPairables()
        {
            return await _businessRuleService.GetAllPairables();
        }

        [HttpGet("GetAllActivePairables")]
        public async Task<ApiCommonResponse> GetAllActivePairables()
        {
            return await _businessRuleService.GetAllActivePairables();
        }

        [HttpGet("GetRuleById/{id}")]
        public async Task<ApiCommonResponse> GetRuleById(long id)
        {
            return await _businessRuleService.GetBusinessRuleById(id);
        }

        [HttpGet("GetPairableById/{id}")]
        public async Task<ApiCommonResponse> GetPairableById(long id)
        {
            return await _businessRuleService.GetPairableById(id);
        }

        [HttpPost("AddNewRule")]
        public async Task<ApiCommonResponse> AddNewRule(BusinessRuleReceivingDTO ReceivingDTO)
        {
            return await _businessRuleService.AddBusinessRule(HttpContext, ReceivingDTO);
        }


        [HttpPost("AddNewPairable")]
        public async Task<ApiCommonResponse> AddNewPairable(BRPairableReceivingDTO ReceivingDTO)
        {
            return await _businessRuleService.AddPairable(HttpContext, ReceivingDTO);
        }

        [HttpPut("UpdateRuleById/{id}")]
        public async Task<ApiCommonResponse> UpdateRuleById(long id, BusinessRuleReceivingDTO ReceivingDTO)
        {
            return await _businessRuleService.UpdateBusinessRule(HttpContext, id, ReceivingDTO);
        }

        [HttpPut("UpdatePairableById/{id}")]
        public async Task<ApiCommonResponse> UpdatePairableById(long id, BRPairableReceivingDTO ReceivingDTO)
        {
            return await _businessRuleService.UpdatePairable(HttpContext, id, ReceivingDTO);
        }

        [HttpDelete("DeleteRuleById/{id}")]
        public async Task<ApiCommonResponse> DeleteRuleById(int id)
        {
            return await _businessRuleService.DeleteBusinessRule(id);
        }

        [HttpDelete("DeletePairableById/{id}")]
        public async Task<ApiCommonResponse> DeletePairableById(int id)
        {
            return await _businessRuleService.DeletePairable(id);
        }
    }
}
