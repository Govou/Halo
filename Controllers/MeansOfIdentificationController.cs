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
    public class MeansOfIdentificationController : ControllerBase
    {
        private readonly IMeansOfIdentificationService _meansOfIdentificationService;

        public MeansOfIdentificationController(IMeansOfIdentificationService meansOfIdentificationService)
        {
            this._meansOfIdentificationService = meansOfIdentificationService;
        }

        [HttpGet("")]
        public async Task<ActionResult> GetMeansOfIdentification()
        {
            var response = await _meansOfIdentificationService.GetAllMeansOfIdentification();
            if (response.StatusCode >= 400)
                return StatusCode(response.StatusCode, response);
            var meansOfIdentification = ((ApiOkResponse)response).Result;
            return Ok(meansOfIdentification);
        }
        [HttpGet("caption/{name}")]
        public async Task<ActionResult> GetByCaption(string name)
        {
            var response = await _meansOfIdentificationService.GetMeansOfIdentificationByName(name);
            if (response.StatusCode >= 400)
                return StatusCode(response.StatusCode, response);
            var meansOfIdentification = ((ApiOkResponse)response).Result;
            return Ok(meansOfIdentification);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult> GetById(long id)
        {
            var response = await _meansOfIdentificationService.GetMeansOfIdentificationById(id);
            if (response.StatusCode >= 400)
                return StatusCode(response.StatusCode, response);
            var meansOfIdentification = ((ApiOkResponse)response).Result;
            return Ok(meansOfIdentification);
        }

        [HttpPost("")]
        public async Task<ActionResult> AddNewMeansOfIdentification(MeansOfIdentificationReceivingDTO meansOfIdentificationReceiving)
        {
            var response = await _meansOfIdentificationService.AddMeansOfIdentification(HttpContext, meansOfIdentificationReceiving);
            if (response.StatusCode >= 400)
                return StatusCode(response.StatusCode, response);
            var meansOfIdentification = ((ApiOkResponse)response).Result;
            return Ok(meansOfIdentification);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateById(long id, MeansOfIdentificationReceivingDTO meansOfIdentificationReceiving)
        {
            var response = await _meansOfIdentificationService.UpdateMeansOfIdentification(HttpContext, id, meansOfIdentificationReceiving);
            if (response.StatusCode >= 400)
                return StatusCode(response.StatusCode, response);
            var meansOfIdentification = ((ApiOkResponse)response).Result;
            return Ok(meansOfIdentification);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteById(int id)
        {
            var response = await _meansOfIdentificationService.DeleteMeansOfIdentification(id);
            return StatusCode(response.StatusCode);
        }
    }
}