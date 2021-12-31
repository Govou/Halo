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
    public class SuspectController : ControllerBase
    {
        private readonly ISuspectService _SuspectService;

        public SuspectController(ISuspectService suspectService)
        {
            this._SuspectService = suspectService;
        }

        [HttpGet("")]
        public async Task<ApiCommonResponse> GetSuspect()
        {
            return await _SuspectService.GetAllSuspect(); 
        }

        [HttpGet("GetUserSuspects")]
        public async Task<ApiCommonResponse> GetUserSuspects()
        {
            return await _SuspectService.GetUserSuspects(HttpContext); 
        }

        /*[HttpGet("caption/{name}")]
        public async Task<ApiCommonResponse> GetByCaption(string name)
        {
            return await _SuspectService.GetSuspectByName(name);
            
                
            var Suspect = ((ApiOkResponse)response).Result;
            return Ok(Suspect);
        }*/

        [HttpGet("{id}")]
        public async Task<ApiCommonResponse> GetById(long id)
        {
            return await _SuspectService.GetSuspectById(id); 
        }

        [HttpPost("ConvertSuspect/{suspectId}")]
        public async Task<ApiCommonResponse> ConvertSuspect(long suspectId)
        {
            return await _SuspectService.ConvertSuspect(HttpContext, suspectId); 
        }

        [HttpPost("")]
        public async Task<ApiCommonResponse> AddNewSuspect(SuspectReceivingDTO SuspectReceiving)
        {
            return await _SuspectService.AddSuspect(HttpContext, SuspectReceiving); 
        }

        [HttpPut("{id}")]
        public async Task<ApiCommonResponse> UpdateById(long id, SuspectReceivingDTO SuspectReceiving)
        {
            return await _SuspectService.UpdateSuspect(HttpContext, id, SuspectReceiving); 
        }

        [HttpDelete("{id}")]
        public async Task<ApiCommonResponse> DeleteById(int id)
        {
            return await _SuspectService.DeleteSuspect(id); 
        }
    }
}
