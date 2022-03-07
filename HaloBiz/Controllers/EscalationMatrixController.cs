using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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
    public class EscalationMatrixController : ControllerBase
    {
        private readonly IEscalationMatrixService _EscalationMatrixService;

        public EscalationMatrixController(IEscalationMatrixService serviceTypeService)
        {
            this._EscalationMatrixService = serviceTypeService;
        }

        [HttpGet("")]
        public async Task<ApiCommonResponse> GetEscalationMatrix()
        {
            return await _EscalationMatrixService.GetAllEscalationMatrix();
        }

        [HttpGet("GetHandlers/{complaintTypeId}")]
        public async Task<ApiCommonResponse> GetHandlers(long complaintTypeId)
        {
            return await _EscalationMatrixService.GetHandlers(complaintTypeId);
        }

        /*[HttpGet("caption/{name}")]
        public async Task<ApiCommonResponse> GetByCaption(string name)
        {
            return await _EscalationMatrixService.GetEscalationMatrixByName(name);
            
                
            var EscalationMatrix = ((ApiOkResponse)response).Result;
            return Ok(EscalationMatrix);
        }*/

        [HttpGet("{id}")]
        public async Task<ApiCommonResponse> GetById(long id)
        {
            return await _EscalationMatrixService.GetEscalationMatrixById(id);
        }

        [HttpPost("")]
        public async Task<ApiCommonResponse> AddNewEscalationMatrix(EscalationMatrixReceivingDTO EscalationMatrixReceiving)
        {
            return await _EscalationMatrixService.AddEscalationMatrix(HttpContext, EscalationMatrixReceiving);
        }

        [HttpPut("{id}")]
        public async Task<ApiCommonResponse> UpdateById(long id, EscalationMatrixReceivingDTO EscalationMatrixReceiving)
        {
            return await _EscalationMatrixService.UpdateEscalationMatrix(HttpContext, id, EscalationMatrixReceiving);
        }

        [HttpDelete("{id}")]
        public async Task<ApiCommonResponse> DeleteById(int id)
        {
            return await _EscalationMatrixService.DeleteEscalationMatrix(id);
        }
    }
}
