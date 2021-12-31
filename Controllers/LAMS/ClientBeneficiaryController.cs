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
        public async Task<ActionResult> GetClientBeneficiary()
        {
            var response = await _clientBeneficiaryService.GetAllClientBeneficiary();
            if (response.StatusCode >= 400)
                return StatusCode(response.StatusCode, response);
            var clientBeneficiary = ((ApiOkResponse)response).Result;
            return Ok(clientBeneficiary);
        }
        [HttpGet("code/{code}")]
        public async Task<ActionResult> GetByCode(string code)
        {
            var response = await _clientBeneficiaryService.GetClientBeneficiaryByCode(code);
            if (response.StatusCode >= 400)
                return StatusCode(response.StatusCode, response);
            var clientBeneficiary = ((ApiOkResponse)response).Result;
            return Ok(clientBeneficiary);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult> GetById(long id)
        {
            var response = await _clientBeneficiaryService.GetClientBeneficiaryById(id);
            if (response.StatusCode >= 400)
                return StatusCode(response.StatusCode, response);
            var clientBeneficiary = ((ApiOkResponse)response).Result;
            return Ok(clientBeneficiary);
        }

        [HttpPost("")]
        public async Task<ActionResult> AddNewClientBeneficiary(ClientBeneficiaryReceivingDTO clientBeneficiaryReceiving)
        {
            var response = await _clientBeneficiaryService.AddClientBeneficiary(HttpContext, clientBeneficiaryReceiving);
            if (response.StatusCode >= 400)
                return StatusCode(response.StatusCode, response);
            var clientBeneficiary = ((ApiOkResponse)response).Result;
            return Ok(clientBeneficiary);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateById(long id, ClientBeneficiaryReceivingDTO clientBeneficiaryReceivingDTO)
        {
            var response = await _clientBeneficiaryService.UpdateClientBeneficiary(HttpContext, id, clientBeneficiaryReceivingDTO);
            if (response.StatusCode >= 400)
                return StatusCode(response.StatusCode, response);
            var clientBeneficiary = ((ApiOkResponse)response).Result;
            return Ok(clientBeneficiary);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteById(int id)
        {
            var response = await _clientBeneficiaryService.DeleteClientBeneficiary(id);
            return StatusCode(response.StatusCode);
        }
    }
}