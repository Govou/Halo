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
    public class ClientContactQualificationController : ControllerBase
    {
        private readonly IClientContactQualificationService _clientContactQualificationService;

        public ClientContactQualificationController(IClientContactQualificationService clientContactQualificationService)
        {
            this._clientContactQualificationService = clientContactQualificationService;
        }

        [HttpGet("")]
        public async Task<ActionResult> GetClientContactQualification()
        {
            var response = await _clientContactQualificationService.GetAllClientContactQualification();
            if (response.StatusCode >= 400)
                return StatusCode(response.StatusCode, response);
            var clientContactQualification = ((ApiOkResponse)response).Result;
            return Ok(clientContactQualification);
        }

        [HttpPost("")]
        public async Task<ActionResult> AddNewClientContactQualification(ClientContactQualificationReceivingDTO clientContactQualificationReceiving)
        {
            var response = await _clientContactQualificationService.AddClientContactQualification(HttpContext, clientContactQualificationReceiving);
            if (response.StatusCode >= 400)
                return StatusCode(response.StatusCode, response);
            var clientContactQualification = ((ApiOkResponse)response).Result;
            return Ok(clientContactQualification);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateById(long id, ClientContactQualificationReceivingDTO clientContactQualificationReceiving)
        {
            var response = await _clientContactQualificationService.UpdateClientContactQualification(HttpContext, id, clientContactQualificationReceiving);
            if (response.StatusCode >= 400)
                return StatusCode(response.StatusCode, response);
            var clientContactQualification = ((ApiOkResponse)response).Result;
            return Ok(clientContactQualification);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteById(int id)
        {
            var response = await _clientContactQualificationService.DeleteClientContactQualification(id);
            return StatusCode(response.StatusCode);
        }

    }
}