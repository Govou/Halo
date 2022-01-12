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
            return await _ClientPolicyService.GetAllClientPolicies();

        }

        /*[HttpGet("caption/{name}")]
        public async Task<ApiCommonResponse> GetByCaption(string name)
        {
            return await _ClientPolicyService.GetClientPolicyByName(name);
            
                
            var ClientPolicy = ((ApiOkResponse)response).Result;
            return Ok(ClientPolicy);
        }*/

        [HttpGet("{id}")]
        public async Task<ApiCommonResponse> GetById(long id)
        {
            return await _ClientPolicyService.GetClientPolicyById(id);

        }

      

        [HttpGet("GetByContractServiceId/{contractServiceId}")]
        public async Task<ApiCommonResponse> FindClientPolicyByContractServiceId(long contractServiceId)
        {
            return await _ClientPolicyService.FindClientPolicyByContractServiceId(contractServiceId);
        }

        [HttpPost]
        public async Task<ApiCommonResponse> AddNewClientPolicy(List<ClientPolicyReceivingDTO> ClientPolicyReceiving)
        {
            return await _ClientPolicyService.AddClientPolicy(HttpContext, ClientPolicyReceiving);
        }

        [HttpPut]
        public async Task<ApiCommonResponse> UpdatePolicies( List<ClientPolicyReceivingDTO> ClientPolicyReceiving)
        {
            return await _ClientPolicyService.UpdateClientPolicy(HttpContext, ClientPolicyReceiving);
        }

        [HttpDelete("{id}")]
        public async Task<ApiCommonResponse> DeleteById(int id)
        {
            return await _ClientPolicyService.DeleteClientPolicy(id);
        }
    }
}
