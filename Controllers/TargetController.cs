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
//using Controllers.Models;

namespace HaloBiz.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class TargetController : ControllerBase
    {
        private readonly ITargetService _targetService;

        public TargetController(ITargetService targetService)
        {
            this._targetService = targetService;
        }

        [HttpGet("")]
        public async Task<ApiCommonResponse> GetTarget()
        {
            return await _targetService.GetAllTarget(); 
        }
        [HttpGet("caption/{name}")]
        public async Task<ApiCommonResponse> GetByCaption(string name)
        {
            return await _targetService.GetTargetByName(name); 
        }

        [HttpGet("{id}")]
        public async Task<ApiCommonResponse> GetById(long id)
        {
            return await _targetService.GetTargetById(id); 
        }

        [HttpPost("")]
        public async Task<ApiCommonResponse> AddNewtarget(TargetReceivingDTO targetReceiving)
        {
            return await _targetService.AddTarget(HttpContext, targetReceiving); 
        }

        [HttpPut("{id}")]
        public async Task<ApiCommonResponse> UpdateById(long id, TargetReceivingDTO targetReceiving)
        {
            return await _targetService.UpdateTarget(HttpContext, id, targetReceiving); 
        }

        [HttpDelete("{id}")]
        public async Task<ApiCommonResponse> DeleteById(int id)
        {
            return await _targetService.DeleteTarget(id);
        }
    }
}