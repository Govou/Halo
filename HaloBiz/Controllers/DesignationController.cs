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
    [ModuleName(HalobizModules.Setups,58)]

    public class DesignationController : ControllerBase
    {
        private readonly IDesignationService _designationService;

        public DesignationController(IDesignationService designationService)
        {
            this._designationService = designationService;
        }

        [HttpGet("")]
        public async Task<ApiCommonResponse> GetDesignation()
        {
            return await _designationService.GetAllDesignation();
        }

        [HttpPost("")]
        public async Task<ApiCommonResponse> AddNewDesignation(DesignationReceivingDTO designationReceiving)
        {
            return await _designationService.AddDesignation(HttpContext, designationReceiving);
        }

        [HttpPut("{id}")]
        public async Task<ApiCommonResponse> UpdateById(long id, DesignationReceivingDTO designationReceiving)
        {
            return await _designationService.UpdateDesignation(HttpContext, id, designationReceiving);
        }

        [HttpDelete("{id}")]
        public async Task<ApiCommonResponse> DeleteById(int id)
        {
            return await _designationService.DeleteDesignation(id);
        }

    }
}