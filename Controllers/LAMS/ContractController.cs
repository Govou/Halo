using System.Threading.Tasks;
using HaloBiz.DTOs.ApiDTOs;
using HaloBiz.MyServices.LAMS;
using Microsoft.AspNetCore.Mvc;

namespace HaloBiz.Controllers.LAMS
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class ContractController : ControllerBase
    {
        private readonly IContractService _contractService;
        private readonly IContractServiceService _contractServiceService;

        public ContractController(IContractService contractService, IContractServiceService contractServiceService)
        {
            this._contractService = contractService;
            this._contractServiceService = contractServiceService;
        }

        [HttpGet("")]
        public async Task<ApiCommonResponse> GetContract()
        {
            var response = await _contractService.GetAllContracts();
            if (response.StatusCode >= 400)
                return StatusCode(response.StatusCode, response);
            var contract = ((ApiOkResponse)response).Result;
            return Ok(contract);
        }

        [HttpGet("ReferenceNumber/{refNo}")]
        public async Task<ApiCommonResponse> GetByCaption(string refNo)
        {
            var response = await _contractService.GetContractByReferenceNumber(refNo);
            if (response.StatusCode >= 400)
                return StatusCode(response.StatusCode, response);
            var contract = ((ApiOkResponse)response).Result;
            return Ok(contract);
        }

        [HttpGet("{id}/ContractService")]
        public async Task<ApiCommonResponse> GetByAllContractServiceForAContract(long id)
        {
            var response = await _contractServiceService.GetAllContractsServcieForAContract(id);
            if (response.StatusCode >= 400)
                return StatusCode(response.StatusCode, response);
            var contract = ((ApiOkResponse)response).Result;
            return Ok(contract);
        }

        [HttpGet("GetContractsByLeadId/{leadId}")]
        public async Task<ApiCommonResponse> GetContractsByLeadId(long leadId)
        {
            var response = await _contractService.GetContractsByLeadId(leadId);
            if (response.StatusCode >= 400)
                return StatusCode(response.StatusCode, response);
            var contract = ((ApiOkResponse)response).Result;
            return Ok(contract);
        }

        [HttpGet("GetContractsByCustomerId/{customerId}")]
        [Microsoft.AspNetCore.Authorization.AllowAnonymous]
        public async Task<ApiCommonResponse> GetContractsByCustomerId(long customerId)
        {
            var response = await _contractService.GetContractsByCustomerId(customerId);
            if (response.StatusCode >= 400)
                return StatusCode(response.StatusCode, response);
            var contract = ((ApiOkResponse)response).Result;
            return Ok(contract);
        }

        [HttpGet("{id}")]
        public async Task<ApiCommonResponse> GetById(long id)
        {
            var response = await _contractService.GetContractById(id);
            if (response.StatusCode >= 400)
                return StatusCode(response.StatusCode, response);
            var contract = ((ApiOkResponse)response).Result;
            return Ok(contract);
        }


        [HttpDelete("{id}")]
        public async Task<ApiCommonResponse> DeleteById(int id)
        {
            var response = await _contractService.DeleteContract(id);
            return StatusCode(response.StatusCode);
        }
    }
}