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
    [ModuleName(HalobizModules.Setups,62)]

    public class EngagementReasonController : ControllerBase
    {
        private readonly IEngagementReasonService _engagementReasonService;

        public EngagementReasonController(IEngagementReasonService engagementReasonService)
        {
            this._engagementReasonService = engagementReasonService;
        }

        [HttpGet("")]
        public async Task<ApiCommonResponse> GetEngagementReason()
        {
            return await _engagementReasonService.GetAllEngagementReason();
        }

        [HttpPost("")]
        public async Task<ApiCommonResponse> AddNewEngagementReason(EngagementReasonReceivingDTO engagementReasonReceiving)
        {
            return await _engagementReasonService.AddEngagementReason(HttpContext, engagementReasonReceiving);
        }

        [HttpPut("{id}")]
        public async Task<ApiCommonResponse> UpdateById(long id, EngagementReasonReceivingDTO engagementReasonReceiving)
        {
            return await _engagementReasonService.UpdateEngagementReason(HttpContext, id, engagementReasonReceiving);
        }

        [HttpDelete("{id}")]
        public async Task<ApiCommonResponse> DeleteById(int id)
        {
            return await _engagementReasonService.DeleteEngagementReason(id);
        }

    }
}