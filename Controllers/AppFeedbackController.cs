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
    public class AppFeedbackController : ControllerBase
    {
        private readonly IAppFeedbackService _AppFeedbackService;

        public AppFeedbackController(IAppFeedbackService appFeedbackService)
        {
            this._AppFeedbackService = appFeedbackService;
        }

        [HttpGet("")]
        public async Task<ActionResult> GetAppFeedback()
        {
            var response = await _AppFeedbackService.GetAllAppFeedback();
            if (response.StatusCode >= 400)
                return StatusCode(response.StatusCode, response);
            var AppFeedback = ((ApiOkResponse)response).Result;
            return Ok(AppFeedback);
        }

        /*[HttpGet("caption/{name}")]
        public async Task<ActionResult> GetByCaption(string name)
        {
            var response = await _AppFeedbackService.GetAppFeedbackByName(name);
            if (response.StatusCode >= 400)
                return StatusCode(response.StatusCode, response);
            var AppFeedback = ((ApiOkResponse)response).Result;
            return Ok(AppFeedback);
        }*/

        [HttpGet("{id}")]
        public async Task<ActionResult> GetById(long id)
        {
            var response = await _AppFeedbackService.GetAppFeedbackById(id);
            if (response.StatusCode >= 400)
                return StatusCode(response.StatusCode, response);
            var AppFeedback = ((ApiOkResponse)response).Result;
            return Ok(AppFeedback);
        }

        [HttpPost("")]
        public async Task<ActionResult> AddNewAppFeedback(AppFeedbackReceivingDTO AppFeedbackReceiving)
        {
            var response = await _AppFeedbackService.AddAppFeedback(HttpContext, AppFeedbackReceiving);
            if (response.StatusCode >= 400)
                return StatusCode(response.StatusCode, response);
            var AppFeedback = ((ApiOkResponse)response).Result;
            return Ok(AppFeedback);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateById(long id, AppFeedbackReceivingDTO AppFeedbackReceiving)
        {
            var response = await _AppFeedbackService.UpdateAppFeedback(HttpContext, id, AppFeedbackReceiving);
            if (response.StatusCode >= 400)
                return StatusCode(response.StatusCode, response);
            var AppFeedback = ((ApiOkResponse)response).Result;
            return Ok(AppFeedback);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteById(int id)
        {
            var response = await _AppFeedbackService.DeleteAppFeedback(id);
            return StatusCode(response.StatusCode);
        }
    }
}
