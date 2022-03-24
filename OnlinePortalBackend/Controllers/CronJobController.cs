using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OnlinePortalBackend.DTOs.ApiDTOs;
using OnlinePortalBackend.MyServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OnlinePortalBackend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CronJobController : ControllerBase
    {
        private readonly ICronJobService _cronJobService;
        public CronJobController(ICronJobService cronJobService)
        {
            _cronJobService = cronJobService;
        }

        [HttpPost("RetryPaymentProcessing")]
        public async Task<ActionResult> RetryPaymentProcessing()
        {
            var response = await _cronJobService.RetryPaymentProcessing();
            if (response.StatusCode >= 400)
                return StatusCode(response.StatusCode, response);
            var theResponse = ((ApiOkResponse)response).Result;
            return Ok(theResponse);
        }
    }
}
