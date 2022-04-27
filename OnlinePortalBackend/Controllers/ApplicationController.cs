using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OnlinePortalBackend.DTOs.ApiDTOs;
using OnlinePortalBackend.DTOs.ReceivingDTOs;
using OnlinePortalBackend.MyServices;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace OnlinePortalBackend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ApplicationController : ControllerBase
    {

        private readonly IServiceRatingService _serviceRatingService;

        public ApplicationController(IServiceRatingService serviceRatingService)
        {
            this._serviceRatingService = serviceRatingService;
        }
        [HttpPost("AddAppRating")]
        public async Task<ActionResult> AddAppRating(AppRatingReceivingDTO appRating)
        {
            var response = await _serviceRatingService.AddAppRating(appRating);
            return Ok(response);
        }

        [HttpGet("GetAppRatings")]
        public async Task<ActionResult> GetAppRatings(int appId)
        {
            var response = await _serviceRatingService.FindAllAppRatings(appId);
            if (response.StatusCode >= 400)
                return StatusCode(response.StatusCode, response);
            var serviceRating = ((ApiOkResponse)response).Result;
            return Ok(serviceRating);
        }

        [HttpGet("GetApplications")]
        public async Task<ActionResult> GetApplications()
        {
            var response = await _serviceRatingService.FindAllApplications();
            if (response.StatusCode >= 400)
                return StatusCode(response.StatusCode, response);
            var apps = ((ApiOkResponse)response).Result;
            return Ok(apps);
        }
    }
}
