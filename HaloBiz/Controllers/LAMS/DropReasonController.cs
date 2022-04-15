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
    [ModuleName(HalobizModules.LeadAdministration,15)]

    public class DropReasonController : ControllerBase
    {
        private readonly IDropReasonService _DropReasonService;

        public DropReasonController(IDropReasonService DropReasonService)
        {
            this._DropReasonService = DropReasonService;
        }

        [HttpGet("")]
        public async Task<ApiCommonResponse> GetDropReason()
        {
            return await _DropReasonService.GetAllDropReason();
        }
        [HttpGet("title/{title}")]
        public async Task<ApiCommonResponse> GetByTitle(string title)
        {
            return await _DropReasonService.GetDropReasonByTitle(title);
        }

        [HttpGet("{id}")]
        public async Task<ApiCommonResponse> GetById(long id)
        {
            return await _DropReasonService.GetDropReasonById(id);
        }

        [HttpPost("")]
        public async Task<ApiCommonResponse> AddNewDropReason(DropReasonReceivingDTO DropReasonReceiving)
        {
            return await _DropReasonService.AddDropReason(HttpContext, DropReasonReceiving);
        }

        [HttpPut("{id}")]
        public async Task<ApiCommonResponse> UpdateById(long id, DropReasonReceivingDTO DropReasonReceiving)
        {
            return await _DropReasonService.UpdateDropReason(HttpContext, id, DropReasonReceiving);
        }

        [HttpDelete("{id}")]
        public async Task<ApiCommonResponse> DeleteById(int id)
        {
            return await _DropReasonService.DeleteDropReason(id);
        }
    }
}