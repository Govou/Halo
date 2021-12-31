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
            return await _ActivityService.GetAllActivity();
        }
        [HttpGet("caption/{name}")]
        public async Task<ApiCommonResponse> GetByCaption(string name)
        {
            return await _ActivityService.GetActivityByName(name);
        }

        [HttpGet("{id}")]
        public async Task<ApiCommonResponse> GetById(long id)
        {
            return await _ActivityService.GetActivityById(id);
        }

        [HttpPost("")]
        public async Task<ApiCommonResponse> AddNewActivity(ActivityReceivingDTO ActivityReceiving)
        {
            return await _ActivityService.AddActivity(HttpContext, ActivityReceiving);
        }

        [HttpPut("{id}")]
        public async Task<ApiCommonResponse> UpdateById(long id, ActivityReceivingDTO ActivityReceiving)
        {
            return await _ActivityService.UpdateActivity(HttpContext, id, ActivityReceiving);
        }

        [HttpDelete("{id}")]
        public async Task<ApiCommonResponse> DeleteById(int id)
        {
            return await _ActivityService.DeleteActivity(id);
        }
    }
}
