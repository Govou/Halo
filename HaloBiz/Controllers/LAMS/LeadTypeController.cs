using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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
    public class LeadTypeController : ControllerBase
    {
        private readonly ILeadTypeService _leadTypeService;

        public LeadTypeController(ILeadTypeService leadTypeService)
        {
            this._leadTypeService = leadTypeService;
        }

        [HttpGet("")]
        public async Task<ApiCommonResponse> GetLeadType()
        {
            return await _leadTypeService.GetAllLeadType();
        }
        [HttpGet("caption/{name}")]
        public async Task<ApiCommonResponse> GetByCaption(string name)
        {
            return await _leadTypeService.GetLeadTypeByName(name);
        }

        [HttpGet("{id}")]
        public async Task<ApiCommonResponse> GetById(long id)
        {
            return await _leadTypeService.GetLeadTypeById(id);

        }

        [HttpPost("")]
        public async Task<ApiCommonResponse> AddNewLeadType(LeadTypeReceivingDTO leadTypeReceiving)
        {
            return await _leadTypeService.AddLeadType(HttpContext, leadTypeReceiving);
        }

        [HttpPut("{id}")]
        public async Task<ApiCommonResponse> UpdateById(long id, LeadTypeReceivingDTO leadTypeReceivingDTO)
        {
            return await _leadTypeService.UpdateLeadType(HttpContext, id, leadTypeReceivingDTO);
        }

        [HttpDelete("{id}")]
        public async Task<ApiCommonResponse> DeleteById(int id)
        {
            return await _leadTypeService.DeleteLeadType(id);
        }
    }
}