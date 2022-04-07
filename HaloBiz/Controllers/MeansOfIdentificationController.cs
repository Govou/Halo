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
//using Controllers.Models;

namespace HaloBiz.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    [ModuleName(HalobizModules.Setups)]

    public class MeansOfIdentificationController : ControllerBase
    {
        private readonly IMeansOfIdentificationService _meansOfIdentificationService;

        public MeansOfIdentificationController(IMeansOfIdentificationService meansOfIdentificationService)
        {
            this._meansOfIdentificationService = meansOfIdentificationService;
        }

        [HttpGet("")]
        public async Task<ApiCommonResponse> GetMeansOfIdentification()
        {
            return await _meansOfIdentificationService.GetAllMeansOfIdentification();
        }
        [HttpGet("caption/{name}")]
        public async Task<ApiCommonResponse> GetByCaption(string name)
        {
            return await _meansOfIdentificationService.GetMeansOfIdentificationByName(name);
        }

        [HttpGet("{id}")]
        public async Task<ApiCommonResponse> GetById(long id)
        {
            return await _meansOfIdentificationService.GetMeansOfIdentificationById(id);
        }

        [HttpPost("")]
        public async Task<ApiCommonResponse> AddNewMeansOfIdentification(MeansOfIdentificationReceivingDTO meansOfIdentificationReceiving)
        {
            return await _meansOfIdentificationService.AddMeansOfIdentification(HttpContext, meansOfIdentificationReceiving);
            
        }

        [HttpPut("{id}")]
        public async Task<ApiCommonResponse> UpdateById(long id, MeansOfIdentificationReceivingDTO meansOfIdentificationReceiving)
        {
            return await _meansOfIdentificationService.UpdateMeansOfIdentification(HttpContext, id, meansOfIdentificationReceiving);
          
        }

        [HttpDelete("{id}")]
        public async Task<ApiCommonResponse> DeleteById(int id)
        {
            return await _meansOfIdentificationService.DeleteMeansOfIdentification(id);
        }
    }
}