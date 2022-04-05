using Halobiz.Common.Auths;
using Halobiz.Common.DTOs.ApiDTOs;
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
    [ModuleName(HalobizModules.CronJobs)]

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
            return await _cronJobService.MigrateNewCustomersToDTRACK(HttpContext);
        }

        [HttpPost("PostNewAccountingRecordsToDTRACK")]
        public async Task<ApiCommonResponse> PostNewAccountingRecordsToDTRACK()
        {
            return await _cronJobService.PostNewAccountingRecordsToDTRACK(HttpContext);
        }
    }
}
