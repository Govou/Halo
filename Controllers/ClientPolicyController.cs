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
    public class ClientPolicyController : ControllerBase
    {
        private readonly IClientPolicyService _ClientPolicyService;

        public ClientPolicyController(IClientPolicyService serviceTypeService)
        {
            this._ClientPolicyService = serviceTypeService;
        }

        [HttpGet("")]
        public async Task<ApiCommonResponse> GetClientPolicies()
        {
            var response = await _ClientPolicyService.GetAllClientPolicies();
            if (response.StatusCode >= 400)
                return StatusCode(response.StatusCode, response);
            var ClientPolicy = ((ApiOkResponse)response).Result;
            return Ok(ClientPolicy);
        }

        /*[HttpGet("caption/{name}")]
        public async Task<ApiCommonResponse> GetByCaption(string name)
        {
            var response = await _ClientPolicyService.GetClientPolicyByName(name);
            if (response.StatusCode >= 400)
                return StatusCode(response.StatusCode, response);
            var ClientPolicy = ((ApiOkResponse)response).Result;
            return Ok(ClientPolicy);
        }*/

        [HttpGet("{id}")]
        public async Task<ApiCommonResponse> GetById(long id)
        {
            var response = await _ClientPolicyService.GetClientPolicyById(id);
            if (response.StatusCode >= 400)
                return StatusCode(response.StatusCode, response);
            var ClientPolicy = ((ApiOkResponse)response).Result;
            return Ok(ClientPolicy);
        }

        [HttpGet("GetByContractId/{contractId}")]
        public async Task<ApiCommonResponse> FindClientPolicyByContractId(long contractId)
        {
            var response = await _ClientPolicyService.FindClientPolicyByContractId(contractId);
            if (response.StatusCode >= 400)
                return StatusCode(response.StatusCode, response);
            var ClientPolicy = ((ApiOkResponse)response).Result;
            return Ok(ClientPolicy);
        }

        [HttpGet("GetByContractServiceId/{contractServiceId}")]
        public async Task<ApiCommonResponse> FindClientPolicyByContractServiceId(long contractServiceId)
        {
            var response = await _ClientPolicyService.FindClientPolicyByContractServiceId(contractServiceId);
            if (response.StatusCode >= 400)
                return StatusCode(response.StatusCode, response);
            var ClientPolicy = ((ApiOkResponse)response).Result;
            return Ok(ClientPolicy);
        }

        [HttpPost("")]
        public async Task<ApiCommonResponse> AddNewClientPolicy(ClientPolicyReceivingDTO ClientPolicyReceiving)
        {
            var response = await _ClientPolicyService.AddClientPolicy(HttpContext, ClientPolicyReceiving);
            if (response.StatusCode >= 400)
                return StatusCode(response.StatusCode, response);
            var ClientPolicy = ((ApiOkResponse)response).Result;
            return Ok(ClientPolicy);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateById(long id, ClientPolicyReceivingDTO ClientPolicyReceiving)
        {
            var response = await _ClientPolicyService.UpdateClientPolicy(HttpContext, id, ClientPolicyReceiving);
            if (response.StatusCode >= 400)
                return StatusCode(response.StatusCode, response);
            var ClientPolicy = ((ApiOkResponse)response).Result;
            return Ok(ClientPolicy);
        }

        [HttpDelete("{id}")]
        public async Task<ApiCommonResponse> DeleteById(int id)
        {
            var response = await _ClientPolicyService.DeleteClientPolicy(id);
            return StatusCode(response.StatusCode);
        }
    }
}
