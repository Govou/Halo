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
    [ModuleName(HalobizModules.Setups)]

    public class AppFeedbackController : ControllerBase
    {
        private readonly IAppFeedbackService _AppFeedbackService;

        public AppFeedbackController(IAppFeedbackService appFeedbackService)
        {
            this._AppFeedbackService = appFeedbackService;
        }

        [HttpGet("")]
        public async Task<ApiCommonResponse> GetAppFeedback()
        {
            return await _AppFeedbackService.GetAllAppFeedback();
        }

        /*[HttpGet("caption/{name}")]
        public async Task<ApiCommonResponse> GetByCaption(string name)
        {
            return await _AppFeedbackService.GetAppFeedbackByName(name);
            
                
            var AppFeedback = ((ApiOkResponse)response).Result;
            return Ok(AppFeedback);
        }*/

        [HttpGet("{id}")]
        public async Task<ApiCommonResponse> GetById(long id)
        {
            return await _AppFeedbackService.GetAppFeedbackById(id);
        }

        [HttpPost("")]
        public async Task<ApiCommonResponse> AddNewAppFeedback(AppFeedbackReceivingDTO AppFeedbackReceiving)
        {
            return await _AppFeedbackService.AddAppFeedback(HttpContext, AppFeedbackReceiving);
        }

        [HttpPut("{id}")]
        public async Task<ApiCommonResponse> UpdateById(long id, AppFeedbackReceivingDTO AppFeedbackReceiving)
        {
            return await _AppFeedbackService.UpdateAppFeedback(HttpContext, id, AppFeedbackReceiving);
        }

        [HttpDelete("{id}")]
        public async Task<ApiCommonResponse> DeleteById(int id)
        {
            return await _AppFeedbackService.DeleteAppFeedback(id);
        }
    }
}
