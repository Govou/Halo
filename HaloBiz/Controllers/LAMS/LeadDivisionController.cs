using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Halobiz.Common.Auths;
using Halobiz.Common.DTOs.ApiDTOs;
using HaloBiz.DTOs.ReceivingDTOs;
using HaloBiz.DTOs.TransferDTOs.LAMS;
using HaloBiz.MyServices.LAMS;
using Microsoft.AspNetCore.Mvc;
//using Controllers.Models;

namespace Controllers.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    [ModuleName(HalobizModules.LeadAdministration,22)]

    public class LeadDivisionController : ControllerBase
    {
        private readonly ILeadDivisionService _leadDivisionService;

        public LeadDivisionController(ILeadDivisionService leadDivisionService)
        {
            this._leadDivisionService = leadDivisionService;
        }

        [HttpGet("")]
        public async Task<ApiCommonResponse> GetLeadDivision()
        {
            return await _leadDivisionService.GetAllLeadDivision();
        }
        [HttpGet("getbyname/{name}")]
        public async Task<ApiCommonResponse> GetByName(string name)
        {
            return await _leadDivisionService.GetLeadDivisionByName(name);
        }

        [HttpGet("getbyrcnumber/{rcNumber}")]
        public async Task<ApiCommonResponse> GetByReferenceNumber(string rcNumber)
        {
            return await _leadDivisionService.GetLeadDivisionByRCNumber(rcNumber);
        }

        [HttpGet("{id}")]
        public async Task<ApiCommonResponse> GetById(long id)
        {
            return await _leadDivisionService.GetLeadDivisionById(id);
        }

        [HttpPost("")]
        public async Task<ApiCommonResponse> AddNewLeadDivision(LeadDivisionReceivingDTO leadDivisionReceiving)
        {
            return await _leadDivisionService.AddLeadDivision(HttpContext, leadDivisionReceiving);
        }

        [HttpPut("{id}")]
        public async Task<ApiCommonResponse> UpdateById(long id, LeadDivisionReceivingDTO leadDivisionReceivingDTO)
        {
            return await _leadDivisionService.UpdateLeadDivision(HttpContext, id, leadDivisionReceivingDTO);
        }

        [HttpDelete("{id}")]
        public async Task<ApiCommonResponse> DeleteById(int id)
        {
            return await _leadDivisionService.DeleteLeadDivision(id);
        }
    }
}