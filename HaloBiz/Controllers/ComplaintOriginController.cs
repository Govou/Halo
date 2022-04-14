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
    [ModuleName(HalobizModules.Setups,53)]

    public class ComplaintOriginController : ControllerBase
    {
        private readonly IComplaintOriginService _ComplaintOriginService;

        public ComplaintOriginController(IComplaintOriginService serviceTypeService)
        {
            this._ComplaintOriginService = serviceTypeService;
        }

        [HttpGet("")]
        public async Task<ApiCommonResponse> GetComplaintOrigin()
        {
            return await _ComplaintOriginService.GetAllComplaintOrigin();
        }
        [HttpGet("caption/{name}")]
        public async Task<ApiCommonResponse> GetByCaption(string name)
        {
            return await _ComplaintOriginService.GetComplaintOriginByName(name);
        }

        [HttpGet("{id}")]
        public async Task<ApiCommonResponse> GetById(long id)
        {
            return await _ComplaintOriginService.GetComplaintOriginById(id);
        }

        [HttpPost("")]
        public async Task<ApiCommonResponse> AddNewComplaintOrigin(ComplaintOriginReceivingDTO ComplaintOriginReceiving)
        {
            return await _ComplaintOriginService.AddComplaintOrigin(HttpContext, ComplaintOriginReceiving);
        }

        [HttpPut("{id}")]
        public async Task<ApiCommonResponse> UpdateById(long id, ComplaintOriginReceivingDTO ComplaintOriginReceiving)
        {
            return await _ComplaintOriginService.UpdateComplaintOrigin(HttpContext, id, ComplaintOriginReceiving);
        }

        [HttpDelete("{id}")]
        public async Task<ApiCommonResponse> DeleteById(int id)
        {
            return await _ComplaintOriginService.DeleteComplaintOrigin(id);
        }
    }
}
