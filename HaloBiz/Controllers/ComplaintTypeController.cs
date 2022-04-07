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
    [ModuleName(HalobizModules.Setups)]

    public class ComplaintTypeController : ControllerBase
    {
        private readonly IComplaintTypeService _ComplaintTypeService;

        public ComplaintTypeController(IComplaintTypeService serviceTypeService)
        {
            this._ComplaintTypeService = serviceTypeService;
        }

        [HttpGet("")]
        public async Task<ApiCommonResponse> GetComplaintType()
        {
            return await _ComplaintTypeService.GetAllComplaintType();
        }
        [HttpGet("caption/{name}")]
        public async Task<ApiCommonResponse> GetByCaption(string name)
        {
            return await _ComplaintTypeService.GetComplaintTypeByName(name);
        }

        [HttpGet("{id}")]
        public async Task<ApiCommonResponse> GetById(long id)
        {
            return await _ComplaintTypeService.GetComplaintTypeById(id);
        }

        [HttpPost("")]
        public async Task<ApiCommonResponse> AddNewComplaintType(ComplaintTypeReceivingDTO ComplaintTypeReceiving)
        {
            return await _ComplaintTypeService.AddComplaintType(HttpContext, ComplaintTypeReceiving);
        }

        [HttpPut("{id}")]
        public async Task<ApiCommonResponse> UpdateById(long id, ComplaintTypeReceivingDTO ComplaintTypeReceiving)
        {
            return await _ComplaintTypeService.UpdateComplaintType(HttpContext, id, ComplaintTypeReceiving);
        }

        [HttpDelete("{id}")]
        public async Task<ApiCommonResponse> DeleteById(int id)
        {
            return await _ComplaintTypeService.DeleteComplaintType(id);
        }
    }
}
