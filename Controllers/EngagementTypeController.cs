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
    public class EngagementTypeController : ControllerBase
    {
        private readonly IEngagementTypeService _engagementTypeService;

        public EngagementTypeController(IEngagementTypeService engagementTypeService)
        {
            this._engagementTypeService = engagementTypeService;
        }

        [HttpGet("")]
        public async Task<ApiCommonResponse> GetEngagementType()
        {
            return await _engagementTypeService.GetAllEngagementType();
        }

        [HttpPost("")]
        public async Task<ApiCommonResponse> AddNewEngagementType(EngagementTypeReceivingDTO engagementTypeReceiving)
        {
            return await _engagementTypeService.AddEngagementType(HttpContext, engagementTypeReceiving);
        }

        [HttpPut("{id}")]
        public async Task<ApiCommonResponse> UpdateById(long id, EngagementTypeReceivingDTO engagementTypeReceiving)
        {
            return await _engagementTypeService.UpdateEngagementType(HttpContext, id, engagementTypeReceiving);
        }

        [HttpDelete("{id}")]
        public async Task<ApiCommonResponse> DeleteById(int id)
        {
            return await _engagementTypeService.DeleteEngagementType(id);
        }

    }
}