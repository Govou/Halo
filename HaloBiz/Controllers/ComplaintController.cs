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
using Microsoft.Extensions.Configuration;

namespace HaloBiz.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    [ModuleName(HalobizModules.ComplaintManagement,51)]

    public class ComplaintController : ControllerBase
    {
        private readonly IComplaintService _ComplaintService;
        private readonly IConfiguration _configuration;

        public ComplaintController(IComplaintService serviceTypeService, IConfiguration configuration)
        {
            this._ComplaintService = serviceTypeService;
            _configuration = configuration;
        }

        [HttpGet("")]
        public async Task<ApiCommonResponse> GetComplaint()
        {
            return await _ComplaintService.GetAllComplaint();
        }

        [HttpGet("GetComplaintsStats")]
        public async Task<ApiCommonResponse> GetComplaintsStats()
        {
            return await _ComplaintService.GetComplaintsStats(HttpContext);
        }

        /*[HttpGet("caption/{name}")]
        public async Task<ApiCommonResponse> GetByCaption(string name)
        {
            return await _ComplaintService.GetComplaintByName(name);
            
                
            var Complaint = ((ApiOkResponse)response).Result;
            return Ok(Complaint);
        }*/

        [HttpGet("{id}")]
        public async Task<ApiCommonResponse> GetById(long id)
        {
            return await _ComplaintService.GetComplaintById(id);
        }

        [HttpPost("")]
        public async Task<ApiCommonResponse> AddNewComplaint(ComplaintReceivingDTO ComplaintReceiving)
        {
            string applicationBaseUrl = _configuration.GetSection("AppSettings:ApplicationURL").Value;
            return await _ComplaintService.AddComplaint(HttpContext, ComplaintReceiving, applicationBaseUrl);
        }

        [HttpPut("{id}")]
        public async Task<ApiCommonResponse> UpdateById(long id, ComplaintReceivingDTO ComplaintReceiving)
        {
            return await _ComplaintService.UpdateComplaint(HttpContext, id, ComplaintReceiving);
        }

        [HttpDelete("{id}")]
        public async Task<ApiCommonResponse> DeleteById(int id)
        {
            return await _ComplaintService.DeleteComplaint(id);
        }
    }
}
