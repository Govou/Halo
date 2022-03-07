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
//using Controllers.Models;


namespace HaloBiz.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class StandardSlaforOperatingEntityController : ControllerBase
    {
        private readonly IStandardSlaforOperatingEntityService _standardSLAForOperatingEntitiesService;

        public StandardSlaforOperatingEntityController(IStandardSlaforOperatingEntityService standardSLAForOperatingEntitiesService)
        {
            this._standardSLAForOperatingEntitiesService = standardSLAForOperatingEntitiesService;
        }

        [HttpGet("")]
        public async Task<ApiCommonResponse> GetStandardSlaforOperatingEntity()
        {
            return await _standardSLAForOperatingEntitiesService.GetAllStandardSlaforOperatingEntity(); 
        }
        [HttpGet("caption/{name}")]
        public async Task<ApiCommonResponse> GetByCaption(string name)
        {
            return await _standardSLAForOperatingEntitiesService.GetStandardSlaforOperatingEntityByName(name); 
        }

        [HttpGet("{id}")]
        public async Task<ApiCommonResponse> GetById(long id)
        {
            return await _standardSLAForOperatingEntitiesService.GetStandardSlaforOperatingEntityById(id); 
        }

        [HttpPost("")]
        public async Task<ApiCommonResponse> AddNewStandardSlaforOperatingEntity(StandardSlaforOperatingEntityReceivingDTO standardSLAForOperatingEntitiesReceiving)
        {
            return await _standardSLAForOperatingEntitiesService.AddStandardSlaforOperatingEntity(HttpContext, standardSLAForOperatingEntitiesReceiving);
        }

        [HttpPut("{id}")]
        public async Task<ApiCommonResponse> UpdateById(long id, StandardSlaforOperatingEntityReceivingDTO standardSLAForOperatingEntitiesReceiving)
        {
            return await _standardSLAForOperatingEntitiesService.UpdateStandardSlaforOperatingEntity(HttpContext, id, standardSLAForOperatingEntitiesReceiving); 
        }

        [HttpDelete("{id}")]
        public async Task<ApiCommonResponse> DeleteById(int id)
        {
            return await _standardSLAForOperatingEntitiesService.DeleteStandardSlaforOperatingEntity(id);
        }
    }
}