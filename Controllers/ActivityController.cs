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
    public class ActivityController : ControllerBase
    {
        private readonly IActivityService _ActivityService;

        public ActivityController(IActivityService activityService)
        {
            this._ActivityService = activityService;
        }

        [HttpGet("")]
        public async Task<ApiCommonResponse> GetActivity()
        {
            var response = await _ActivityService.GetAllActivity();
            if (response.StatusCode >= 400)
                return StatusCode(response.StatusCode, response);
            var Activity = ((ApiOkResponse)response).Result;
            return Ok(Activity);
        }
        [HttpGet("caption/{name}")]
        public async Task<ApiCommonResponse> GetByCaption(string name)
        {
            var response = await _ActivityService.GetActivityByName(name);
            if (response.StatusCode >= 400)
                return StatusCode(response.StatusCode, response);
            var Activity = ((ApiOkResponse)response).Result;
            return Ok(Activity);
        }

        [HttpGet("{id}")]
        public async Task<ApiCommonResponse> GetById(long id)
        {
            var response = await _ActivityService.GetActivityById(id);
            if (response.StatusCode >= 400)
                return StatusCode(response.StatusCode, response);
            var Activity = ((ApiOkResponse)response).Result;
            return Ok(Activity);
        }

        [HttpPost("")]
        public async Task<ApiCommonResponse> AddNewActivity(ActivityReceivingDTO ActivityReceiving)
        {
            var response = await _ActivityService.AddActivity(HttpContext, ActivityReceiving);
            if (response.StatusCode >= 400)
                return StatusCode(response.StatusCode, response);
            var Activity = ((ApiOkResponse)response).Result;
            return Ok(Activity);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateById(long id, ActivityReceivingDTO ActivityReceiving)
        {
            var response = await _ActivityService.UpdateActivity(HttpContext, id, ActivityReceiving);
            if (response.StatusCode >= 400)
                return StatusCode(response.StatusCode, response);
            var Activity = ((ApiOkResponse)response).Result;
            return Ok(Activity);
        }

        [HttpDelete("{id}")]
        public async Task<ApiCommonResponse> DeleteById(int id)
        {
            var response = await _ActivityService.DeleteActivity(id);
            return StatusCode(response.StatusCode);
        }
    }
}
