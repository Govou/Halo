using Halobiz.Common.DTOs.ApiDTOs;
using HaloBiz.DTOs.ReceivingDTOs;
using HaloBiz.DTOs.TransferDTOs;
using HaloBiz.MyServices;
using HaloBiz.Repository;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HaloBiz.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]

    public class GenerateGroupInvoiceNumberController : ControllerBase
    {
        private readonly IGroupInvoiceTrackerRepository _trackerRepo;

        public GenerateGroupInvoiceNumberController(IGroupInvoiceTrackerRepository trackerRepo)
        {
            this._trackerRepo = trackerRepo;
        }

        [HttpGet("")]
        public async Task<ApiCommonResponse> GetDivisions()
        {
            return await _trackerRepo.GenerateGroupInvoiceNumber();
        }
        
    }
}