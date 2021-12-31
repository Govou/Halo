using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HaloBiz.DTOs.ApiDTOs;
using HaloBiz.DTOs.ReceivingDTOs;
using HaloBiz.DTOs.TransferDTOs.LAMS;
using HaloBiz.MyServices.LAMS;
using Microsoft.AspNetCore.Mvc;

namespace Controllers.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class ClientBeneficiaryController : ControllerBase
    {
        private readonly IClientBeneficiaryService _clientBeneficiaryService;

        public ClientBeneficiaryController(IClientBeneficiaryService clientBeneficiaryService)
        {
            _clientBeneficiaryService = clientBeneficiaryService;
        }

        [HttpGet("")]
        public async Task<ApiCommonResponse> GetClientBeneficiary()
        {
            return await _clientBeneficiaryService.GetAllClientBeneficiary();
        }
        [HttpGet("code/{code}")]
        public async Task<ApiCommonResponse> GetByCode(string code)
        {
            return await _clientBeneficiaryService.GetClientBeneficiaryByCode(code);
        }

        [HttpGet("{id}")]
        public async Task<ApiCommonResponse> GetById(long id)
        {
            return await _clientBeneficiaryService.GetClientBeneficiaryById(id);
        }

        [HttpPost("")]
        public async Task<ApiCommonResponse> AddNewClientBeneficiary(ClientBeneficiaryReceivingDTO clientBeneficiaryReceiving)
        {
            return await _clientBeneficiaryService.AddClientBeneficiary(HttpContext, clientBeneficiaryReceiving);
        }

        [HttpPut("{id}")]
        public async Task<ApiCommonResponse> UpdateById(long id, ClientBeneficiaryReceivingDTO clientBeneficiaryReceivingDTO)
        {
            return await _clientBeneficiaryService.UpdateClientBeneficiary(HttpContext, id, clientBeneficiaryReceivingDTO);
        }

        [HttpDelete("{id}")]
        public async Task<ApiCommonResponse> DeleteById(int id)
        {
            return await _clientBeneficiaryService.DeleteClientBeneficiary(id);
        }
    }
}