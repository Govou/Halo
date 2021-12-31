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
    public class AppReviewController : ControllerBase
    {
        private readonly IAppReviewService _AppReviewService;

        public AppReviewController(IAppReviewService appReviewService)
        {
            this._AppReviewService = appReviewService;
        }

        [HttpGet("")]
        public async Task<ActionResult> GetAppReview()
        {
            var response = await _AppReviewService.GetAllAppReview();
            if (response.StatusCode >= 400)
                return StatusCode(response.StatusCode, response);
            var AppReview = ((ApiOkResponse)response).Result;
            return Ok(AppReview);
        }
        
        /*[HttpGet("caption/{name}")]
        public async Task<ActionResult> GetByCaption(string name)
        {
            var response = await _AppReviewService.GetAppReviewByName(name);
            if (response.StatusCode >= 400)
                return StatusCode(response.StatusCode, response);
            var AppReview = ((ApiOkResponse)response).Result;
            return Ok(AppReview);
        }*/

        [HttpGet("{id}")]
        public async Task<ActionResult> GetById(long id)
        {
            var response = await _AppReviewService.GetAppReviewById(id);
            if (response.StatusCode >= 400)
                return StatusCode(response.StatusCode, response);
            var AppReview = ((ApiOkResponse)response).Result;
            return Ok(AppReview);
        }

        [HttpPost("")]
        public async Task<ActionResult> AddNewAppReview(AppReviewReceivingDTO AppReviewReceiving)
        {
            var response = await _AppReviewService.AddAppReview(HttpContext, AppReviewReceiving);
            if (response.StatusCode >= 400)
                return StatusCode(response.StatusCode, response);
            var AppReview = ((ApiOkResponse)response).Result;
            return Ok(AppReview);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateById(long id, AppReviewReceivingDTO AppReviewReceiving)
        {
            var response = await _AppReviewService.UpdateAppReview(HttpContext, id, AppReviewReceiving);
            if (response.StatusCode >= 400)
                return StatusCode(response.StatusCode, response);
            var AppReview = ((ApiOkResponse)response).Result;
            return Ok(AppReview);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteById(int id)
        {
            var response = await _AppReviewService.DeleteAppReview(id);
            return StatusCode(response.StatusCode);
        }
    }
}
