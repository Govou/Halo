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
    [ModuleName(HalobizModules.Setups,40)]

    public class ApproverLevelController : ControllerBase
    {
        private readonly IApproverLevelService _approverLevelService;

        public ApproverLevelController(IApproverLevelService approverLevelService)
        {
            this._approverLevelService = approverLevelService;
        }

        [HttpGet("")]
        public async Task<ApiCommonResponse> GetApproverLevel()
        {
            return await _approverLevelService.GetAllApproverLevel();
        }

        [HttpPost("")]
        public async Task<ApiCommonResponse> AddNewApproverLevel(ApproverLevelReceivingDTO approverLevelReceiving)
        {
            return await _approverLevelService.AddApproverLevel(HttpContext, approverLevelReceiving);
        }

        [HttpPut("{id}")]
        public async Task<ApiCommonResponse> UpdateById(long id, ApproverLevelReceivingDTO approverLevelReceiving)
        {
            return await _approverLevelService.UpdateApproverLevel(HttpContext, id, approverLevelReceiving);
        }

        [HttpDelete("{id}")]
        public async Task<ApiCommonResponse> DeleteById(int id)
        {
            return await _approverLevelService.DeleteApproverLevel(id);
        }

        [HttpPost("CreateApprovingLevelOffice")]
        public async Task<ApiCommonResponse> CreateApprovingLevelOffice(ApprovingLevelOfficeReceivingDTO model)
        {
            return await _approverLevelService.CreateApprovingLevelOffice(HttpContext, model);
        }

        [HttpGet("GetAllApprovingLevelOffices")]
        public async Task<ApiCommonResponse> GetAllApprovingLevelOffices()
        {
            return await _approverLevelService.GetAllApprovingLevelOffices();
        }

        [HttpDelete("DeleteApprovingLevelOfficeById/{id}")]
        public async Task<ApiCommonResponse> DeleteApprovingLevelOfficeById(int id)
        {
            return await _approverLevelService.DeleteApprovingLevelOffice(id);
        }

        [HttpPut("UpdateApprovingLevelOfficeById/{id}")]
        public async Task<ApiCommonResponse> UpdateApprovingLevelOfficeById(long id, ApprovingLevelOfficeReceivingDTO approverLevelReceiving)
        {
            return await _approverLevelService.UpdateApprovingLevelOffice(HttpContext, id, approverLevelReceiving);
        }

    }
}