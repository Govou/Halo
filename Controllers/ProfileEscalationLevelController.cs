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

namespace HaloBiz.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class ProfileEscalationLevelController : ControllerBase
    {
        private readonly IProfileEscalationLevelService _ProfileEscalationLevelService;

        public ProfileEscalationLevelController(IProfileEscalationLevelService serviceTypeService)
        {
            this._ProfileEscalationLevelService = serviceTypeService;
        }

        [HttpGet("")]
        public async Task<ApiCommonResponse> GetProfileEscalationLevel()
        {
            return await _ProfileEscalationLevelService.GetAllProfileEscalationLevel();
        }

        [HttpGet("GetAllHandlers")]
        public async Task<ApiCommonResponse> GetAllHandlers()
        {
            return await _ProfileEscalationLevelService.GetAllHandlerProfileEscalationLevel();
        }
        /*[HttpGet("caption/{name}")]
        public async Task<ApiCommonResponse> GetByCaption(string name)
        {
            return await _ProfileEscalationLevelService.GetProfileEscalationLevelByName(name);
            
                
            var ProfileEscalationLevel = ((ApiOkResponse)response).Result;
            return Ok(ProfileEscalationLevel);
        }*/

        [HttpGet("{id}")]
        public async Task<ApiCommonResponse> GetById(long id)
        {
            return await _ProfileEscalationLevelService.GetProfileEscalationLevelById(id);
        }

        [HttpPost("")]
        public async Task<ApiCommonResponse> AddNewProfileEscalationLevel(ProfileEscalationLevelReceivingDTO ProfileEscalationLevelReceiving)
        {
            return await _ProfileEscalationLevelService.AddProfileEscalationLevel(HttpContext, ProfileEscalationLevelReceiving);
            
        }

        [HttpPut("{id}")]
        public async Task<ApiCommonResponse> UpdateById(long id, ProfileEscalationLevelReceivingDTO ProfileEscalationLevelReceiving)
        {
            return await _ProfileEscalationLevelService.UpdateProfileEscalationLevel(HttpContext, id, ProfileEscalationLevelReceiving);
          
        }

        [HttpDelete("{id}")]
        public async Task<ApiCommonResponse> DeleteById(int id)
        {
            return await _ProfileEscalationLevelService.DeleteProfileEscalationLevel(id);
        }
    }
}
