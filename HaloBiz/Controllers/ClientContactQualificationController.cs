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
    [ModuleName(HalobizModules.ClientManagement)]

    public class ClientContactQualificationController : ControllerBase
    {
        private readonly IClientContactQualificationService _clientContactQualificationService;

        public ClientContactQualificationController(IClientContactQualificationService clientContactQualificationService)
        {
            this._clientContactQualificationService = clientContactQualificationService;
        }

        [HttpGet("")]
        public async Task<ApiCommonResponse> GetClientContactQualification()
        {
            return await _clientContactQualificationService.GetAllClientContactQualification();
        }

        [HttpPost("")]
        public async Task<ApiCommonResponse> AddNewClientContactQualification(ClientContactQualificationReceivingDTO clientContactQualificationReceiving)
        {
            return await _clientContactQualificationService.AddClientContactQualification(HttpContext, clientContactQualificationReceiving);
            
        }

        [HttpPut("{id}")]
        public async Task<ApiCommonResponse> UpdateById(long id, ClientContactQualificationReceivingDTO clientContactQualificationReceiving)
        {
            return await _clientContactQualificationService.UpdateClientContactQualification(HttpContext, id, clientContactQualificationReceiving);
            
        }

        [HttpDelete("{id}")]
        public async Task<ApiCommonResponse> DeleteById(int id)
        {
            return await _clientContactQualificationService.DeleteClientContactQualification(id);
        }

    }
}