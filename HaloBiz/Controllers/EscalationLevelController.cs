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

namespace HaloBiz.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    [ModuleName(HalobizModules.Setups,63)]

    public class EscalationLevelController : ControllerBase
    {
        private readonly IEscalationLevelService _EscalationLevelService;

        public EscalationLevelController(IEscalationLevelService serviceTypeService)
        {
            this._EscalationLevelService = serviceTypeService;
        }

        [HttpGet("")]
        public async Task<ApiCommonResponse> GetEscalationLevel()
        {
            return await _EscalationLevelService.GetAllEscalationLevel();
        }
        [HttpGet("caption/{name}")]
        public async Task<ApiCommonResponse> GetByCaption(string name)
        {
            return await _EscalationLevelService.GetEscalationLevelByName(name);
        }

        [HttpGet("{id}")]
        public async Task<ApiCommonResponse> GetById(long id)
        {
            return await _EscalationLevelService.GetEscalationLevelById(id);
        }

        [HttpPost("")]
        public async Task<ApiCommonResponse> AddNewEscalationLevel(EscalationLevelReceivingDTO EscalationLevelReceiving)
        {
            return await _EscalationLevelService.AddEscalationLevel(HttpContext, EscalationLevelReceiving);
        }

        [HttpPut("{id}")]
        public async Task<ApiCommonResponse> UpdateById(long id, EscalationLevelReceivingDTO EscalationLevelReceiving)
        {
            return await _EscalationLevelService.UpdateEscalationLevel(HttpContext, id, EscalationLevelReceiving);
        }

        [HttpDelete("{id}")]
        public async Task<ApiCommonResponse> DeleteById(int id)
        {
            return await _EscalationLevelService.DeleteEscalationLevel(id);
        }
    }
}
