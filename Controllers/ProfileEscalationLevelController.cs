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
            var response = await _ProfileEscalationLevelService.GetAllProfileEscalationLevel();
            if (response.StatusCode >= 400)
                return StatusCode(response.StatusCode, response);
            var ProfileEscalationLevel = ((ApiOkResponse)response).Result;
            return Ok(ProfileEscalationLevel);
        }

        [HttpGet("GetAllHandlers")]
        public async Task<ApiCommonResponse> GetAllHandlers()
        {
            var response = await _ProfileEscalationLevelService.GetAllHandlerProfileEscalationLevel();
            if (response.StatusCode >= 400)
                return StatusCode(response.StatusCode, response);
            var ProfileEscalationLevel = ((ApiOkResponse)response).Result;
            return Ok(ProfileEscalationLevel);
        }
        /*[HttpGet("caption/{name}")]
        public async Task<ApiCommonResponse> GetByCaption(string name)
        {
            var response = await _ProfileEscalationLevelService.GetProfileEscalationLevelByName(name);
            if (response.StatusCode >= 400)
                return StatusCode(response.StatusCode, response);
            var ProfileEscalationLevel = ((ApiOkResponse)response).Result;
            return Ok(ProfileEscalationLevel);
        }*/

        [HttpGet("{id}")]
        public async Task<ApiCommonResponse> GetById(long id)
        {
            var response = await _ProfileEscalationLevelService.GetProfileEscalationLevelById(id);
            if (response.StatusCode >= 400)
                return StatusCode(response.StatusCode, response);
            var ProfileEscalationLevel = ((ApiOkResponse)response).Result;
            return Ok(ProfileEscalationLevel);
        }

        [HttpPost("")]
        public async Task<ApiCommonResponse> AddNewProfileEscalationLevel(ProfileEscalationLevelReceivingDTO ProfileEscalationLevelReceiving)
        {
            var response = await _ProfileEscalationLevelService.AddProfileEscalationLevel(HttpContext, ProfileEscalationLevelReceiving);
            if (response.StatusCode >= 400)
                return StatusCode(response.StatusCode, response);
            var ProfileEscalationLevel = ((ApiOkResponse)response).Result;
            return Ok(ProfileEscalationLevel);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateById(long id, ProfileEscalationLevelReceivingDTO ProfileEscalationLevelReceiving)
        {
            var response = await _ProfileEscalationLevelService.UpdateProfileEscalationLevel(HttpContext, id, ProfileEscalationLevelReceiving);
            if (response.StatusCode >= 400)
                return StatusCode(response.StatusCode, response);
            var ProfileEscalationLevel = ((ApiOkResponse)response).Result;
            return Ok(ProfileEscalationLevel);
        }

        [HttpDelete("{id}")]
        public async Task<ApiCommonResponse> DeleteById(int id)
        {
            var response = await _ProfileEscalationLevelService.DeleteProfileEscalationLevel(id);
            return StatusCode(response.StatusCode);
        }
    }
}
