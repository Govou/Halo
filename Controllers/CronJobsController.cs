using HaloBiz.DTOs.ApiDTOs;
using HaloBiz.MyServices;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HaloBiz.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class CronJobsController : ControllerBase
    {
        private readonly ICronJobService _cronJobService;
        public CronJobsController(ICronJobService cronJobService)
        {
            _cronJobService = cronJobService;
        }

        [HttpPost("MigrateNewCustomersToDTRACK")]
        public async Task<ApiCommonResponse> MigrateNewCustomersToDTRACK()
        {
            var response = await _cronJobService.MigrateNewCustomersToDTRACK(HttpContext);
            if (response.StatusCode >= 400)
                return StatusCode(response.StatusCode, response);
            var migratedCustomerCount = ((ApiOkResponse)response).Result;
            return Ok(migratedCustomerCount);
        }

        [HttpPost("PostNewAccountingRecordsToDTRACK")]
        public async Task<ApiCommonResponse> PostNewAccountingRecordsToDTRACK()
        {
            var response = await _cronJobService.PostNewAccountingRecordsToDTRACK(HttpContext);
            if (response.StatusCode >= 400)
                return StatusCode(response.StatusCode, response);
            var accountMasterRecordsMigratedPerCustomer = ((ApiOkResponse)response).Result;
            return Ok(accountMasterRecordsMigratedPerCustomer);
        }
    }
}
