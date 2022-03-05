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
    public class ComplaintSourceController : ControllerBase
    {
        private readonly IComplaintSourceService _ComplaintSourceService;

        public ComplaintSourceController(IComplaintSourceService serviceTypeService)
        {
            this._ComplaintSourceService = serviceTypeService;
        }

        [HttpGet("")]
        public async Task<ApiCommonResponse> GetComplaintSource()
        {
            return await _ComplaintSourceService.GetAllComplaintSource();
        }
        [HttpGet("caption/{name}")]
        public async Task<ApiCommonResponse> GetByCaption(string name)
        {
            return await _ComplaintSourceService.GetComplaintSourceByName(name);
        }

        [HttpGet("{id}")]
        public async Task<ApiCommonResponse> GetById(long id)
        {
            return await _ComplaintSourceService.GetComplaintSourceById(id);
        }

        [HttpPost("")]
        public async Task<ApiCommonResponse> AddNewComplaintSource(ComplaintSourceReceivingDTO ComplaintSourceReceiving)
        {
            return await _ComplaintSourceService.AddComplaintSource(HttpContext, ComplaintSourceReceiving);
        }

        [HttpPut("{id}")]
        public async Task<ApiCommonResponse> UpdateById(long id, ComplaintSourceReceivingDTO ComplaintSourceReceiving)
        {
            return await _ComplaintSourceService.UpdateComplaintSource(HttpContext, id, ComplaintSourceReceiving);
        }

        [HttpDelete("{id}")]
        public async Task<ApiCommonResponse> DeleteById(int id)
        {
            return await _ComplaintSourceService.DeleteComplaintSource(id);
        }
    }
}
