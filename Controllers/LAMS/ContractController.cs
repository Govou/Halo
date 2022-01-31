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
            return await _contractService.GetAllContracts();
        }

        [HttpGet("ReferenceNumber/{refNo}")]
        public async Task<ApiCommonResponse> GetByCaption(string refNo)
        {
            return await _contractService.GetContractByReferenceNumber(refNo);
        }

        [HttpGet("{id}/ContractService")]
        public async Task<ApiCommonResponse> GetByAllContractServiceForAContract(long id)
        {
            return await _contractServiceService.GetAllContractsServcieForAContract(id);
        }

        [HttpGet("GetContractsByLeadId/{leadId}")]
        public async Task<ApiCommonResponse> GetContractsByLeadId(long leadId)
        {
            return await _contractService.GetContractsByLeadId(leadId);
        }

        [HttpGet("GetContractsByCustomerId/{customerId}")]
        [Microsoft.AspNetCore.Authorization.AllowAnonymous]
        public async Task<ApiCommonResponse> GetContractsByCustomerId(long customerId)
        {
            return await _contractService.GetContractsByCustomerId(customerId);
        }

        [HttpGet("{id}")]
        public async Task<ApiCommonResponse> GetById(long id)
        {
            return await _contractService.GetContractById(id);
        }


        [HttpDelete("{id}")]
        public async Task<ApiCommonResponse> DeleteById(int id)
        {
            return await _contractService.DeleteContract(id);
        }
    }
}