using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Halobiz.Common.Auths;
using Halobiz.Common.DTOs.ApiDTOs;
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
    [ModuleName(HalobizModules.Setups)]

    public class IndustryController : ControllerBase
    {
        private readonly IIndustryService _industryService;

        public IndustryController(IIndustryService industryService)
        {
            this._industryService = industryService;
        }

        [HttpGet("")]
        public async Task<ApiCommonResponse> GetIndustry()
        {
            return await _industryService.GetAllIndustry();
        }

        [HttpPost("")]
        public async Task<ApiCommonResponse> AddNewIndustry(IndustryReceivingDTO industryReceiving)
        {
            return await _industryService.AddIndustry(HttpContext, industryReceiving);
        }

        [HttpPut("{id}")]
        public async Task<ApiCommonResponse> UpdateById(long id, IndustryReceivingDTO industryReceiving)
        {
            return await _industryService.UpdateIndustry(HttpContext, id, industryReceiving);
        }

        [HttpDelete("{id}")]
        public async Task<ApiCommonResponse> DeleteById(int id)
        {
            return await _industryService.DeleteIndustry(id);
        }

    }
}