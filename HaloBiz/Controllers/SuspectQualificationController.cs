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
    public class SuspectQualificationController : ControllerBase
    {
        private readonly ISuspectQualificationService _SuspectQualificationService;

        public SuspectQualificationController(ISuspectQualificationService suspectQualificationService)
        {
            this._SuspectQualificationService = suspectQualificationService;
        }

        [HttpGet("")]
        public async Task<ApiCommonResponse> GetSuspectQualification()
        {
            return await _SuspectQualificationService.GetAllSuspectQualification(); 
        }

        [HttpGet("GetUserSuspectQualification")]
        public async Task<ApiCommonResponse> GetUserSuspectQualification()
        {
            return await _SuspectQualificationService.GetUserSuspectQualification(HttpContext); 
        }

        /*[HttpGet("caption/{name}")]
        public async Task<ApiCommonResponse> GetByCaption(string name)
        {
            return await _SuspectQualificationService.GetSuspectQualificationByName(name);
            
                
            var SuspectQualification = ((ApiOkResponse)response).Result;
            return Ok(SuspectQualification);
        }*/

        [HttpGet("{id}")]
        public async Task<ApiCommonResponse> GetById(long id)
        {
            return await _SuspectQualificationService.GetSuspectQualificationById(id); 
        }

        [HttpPost("")]
        public async Task<ApiCommonResponse> AddNewSuspectQualification(SuspectQualificationReceivingDTO SuspectQualificationReceiving)
        {
            return await _SuspectQualificationService.AddSuspectQualification(HttpContext, SuspectQualificationReceiving); 
        }

        [HttpPut("{id}")]
        public async Task<ApiCommonResponse> UpdateById(long id, SuspectQualificationReceivingDTO SuspectQualificationReceiving)
        {
            return await _SuspectQualificationService.UpdateSuspectQualification(HttpContext, id, SuspectQualificationReceiving); 
        }

        [HttpDelete("{id}")]
        public async Task<ApiCommonResponse> DeleteById(int id)
        {
            return await _SuspectQualificationService.DeleteSuspectQualification(id);
        }
    }
}
