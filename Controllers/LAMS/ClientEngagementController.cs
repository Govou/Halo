using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HaloBiz.DTOs.ApiDTOs;
using HaloBiz.DTOs.ReceivingDTOs;
using HaloBiz.DTOs.TransferDTOs.LAMS;
using HaloBiz.MyServices.LAMS;
using Microsoft.AspNetCore.Mvc;
//using Controllers.Models;

namespace Controllers.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class ClientEngagementController : ControllerBase
    {
        private readonly IClientEngagementService _clientEngagementService;

        public ClientEngagementController(IClientEngagementService clientEngagementService)
        {
            this._clientEngagementService = clientEngagementService;
        }

        [HttpGet("")]
        public async Task<ActionResult> GetClientEngagement()
        {
            var response = await _clientEngagementService.GetAllClientEngagement();
            if (response.StatusCode >= 400)
                return StatusCode(response.StatusCode, response);
            var clientEngagement = ((ApiOkResponse)response).Result;
            return Ok(clientEngagement);
        }
        [HttpGet("caption/{name}")]
        public async Task<ActionResult> GetByCaption(string name)
        {
            var response = await _clientEngagementService.GetClientEngagementByName(name);
            if (response.StatusCode >= 400)
                return StatusCode(response.StatusCode, response);
            var clientEngagement = ((ApiOkResponse)response).Result;
            return Ok(clientEngagement);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult> GetById(long id)
        {
            var response = await _clientEngagementService.GetClientEngagementById(id);
            if (response.StatusCode >= 400)
                return StatusCode(response.StatusCode, response);
            var clientEngagement = ((ApiOkResponse)response).Result;
            return Ok(clientEngagement);
        }

        [HttpPost("")]
        public async Task<ActionResult> AddNewClientEngagement(ClientEngagementReceivingDTO clientEngagementReceiving)
        {
            var response = await _clientEngagementService.AddClientEngagement(HttpContext, clientEngagementReceiving);
            if (response.StatusCode >= 400)
                return StatusCode(response.StatusCode, response);
            var clientEngagement = ((ApiOkResponse)response).Result;
            return Ok(clientEngagement);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateById(long id, ClientEngagementReceivingDTO clientEngagementReceivingDTO)
        {
            var response = await _clientEngagementService.UpdateClientEngagement(HttpContext, id, clientEngagementReceivingDTO);
            if (response.StatusCode >= 400)
                return StatusCode(response.StatusCode, response);
            var clientEngagement = ((ApiOkResponse)response).Result;
            return Ok(clientEngagement);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteById(int id)
        {
            var response = await _clientEngagementService.DeleteClientEngagement(id);
            return StatusCode(response.StatusCode);
        }
    }
}