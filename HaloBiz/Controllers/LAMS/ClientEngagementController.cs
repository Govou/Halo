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
        public async Task<ApiCommonResponse> GetClientEngagement()
        {
            return await _clientEngagementService.GetAllClientEngagement();
        }
        [HttpGet("caption/{name}")]
        public async Task<ApiCommonResponse> GetByCaption(string name)
        {
            return await _clientEngagementService.GetClientEngagementByName(name);
        }

        [HttpGet("{id}")]
        public async Task<ApiCommonResponse> GetById(long id)
        {
            return await _clientEngagementService.GetClientEngagementById(id);
        }

        [HttpPost("")]
        public async Task<ApiCommonResponse> AddNewClientEngagement(ClientEngagementReceivingDTO clientEngagementReceiving)
        {
            return await _clientEngagementService.AddClientEngagement(HttpContext, clientEngagementReceiving);
        }

        [HttpPut("{id}")]
        public async Task<ApiCommonResponse> UpdateById(long id, ClientEngagementReceivingDTO clientEngagementReceivingDTO)
        {
            return await _clientEngagementService.UpdateClientEngagement(HttpContext, id, clientEngagementReceivingDTO);
        }

        [HttpDelete("{id}")]
        public async Task<ApiCommonResponse> DeleteById(int id)
        {
            return await _clientEngagementService.DeleteClientEngagement(id);
        }
    }
}