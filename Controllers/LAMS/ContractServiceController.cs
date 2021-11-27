using System.Threading.Tasks;
using HaloBiz.DTOs.ApiDTOs;
using HaloBiz.MyServices.LAMS;
using Microsoft.AspNetCore.Mvc;

namespace HaloBiz.Controllers.LAMS
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class ContractServiceController : ControllerBase
    {
        private readonly IContractServiceService _contractServiceService;

        public ContractServiceController(IContractServiceService contractServiceService)
        {
            this._contractServiceService = contractServiceService;
        }



        [HttpGet("ReferenceNumber/{refNo}")]
        public async Task<ActionResult> GetByCaption(string refNo)
        {
            var response = await _contractServiceService.GetContractServiceByReferenceNumber(refNo);
            if (response.StatusCode >= 400)
                return StatusCode(response.StatusCode, response);
            var contract = ((ApiOkResponse)response).Result;
            return Ok(contract);
        }

        [HttpGet("ByTag/{tag}")]
        public async Task<ActionResult> ByTag(string tag)
        {
            var response = await _contractServiceService.GetContractServiceByTag(tag);
            if (response.StatusCode >= 400)
                return StatusCode(response.StatusCode, response);
            var contract = ((ApiOkResponse)response).Result;
            return Ok(contract);
        }

        [HttpGet("GroupInvoiceNumber/{groupInvoiceNumber}")]
        public async Task<ActionResult> GetByGroupInvoiceNumber(string groupInvoiceNumber)
        {
            var response = await _contractServiceService.GetContractServiceByGroupInvoiceNumber(groupInvoiceNumber);
            if (response.StatusCode >= 400)
                return StatusCode(response.StatusCode, response);
            var contract = ((ApiOkResponse)response).Result;
            return Ok(contract);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult> GetById(long id)
        {
            var response = await _contractServiceService.GetContractServiceById(id);
            if (response.StatusCode >= 400)
                return StatusCode(response.StatusCode, response);
            var contract = ((ApiOkResponse)response).Result;
            return Ok(contract);
        }


        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteById(int id)
        {
            var response = await _contractServiceService.DeleteContractService(id);
            return StatusCode(response.StatusCode);
        }
    }
}