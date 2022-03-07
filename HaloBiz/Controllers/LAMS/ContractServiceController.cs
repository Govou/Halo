using System.Threading.Tasks;
using Halobiz.Common.DTOs.ApiDTOs;
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
        public async Task<ApiCommonResponse> GetByCaption(string refNo)
        {
            return await _contractServiceService.GetContractServiceByReferenceNumber(refNo);
        }

        [HttpGet("ByTag/{tag}")]
        public async Task<ApiCommonResponse> ByTag(string tag)
        {
            return await _contractServiceService.GetContractServiceByTag(tag);
        }

        [HttpGet("GroupInvoiceNumber/{groupInvoiceNumber}")]
        public async Task<ApiCommonResponse> GetByGroupInvoiceNumber(string groupInvoiceNumber)
        {
            return await _contractServiceService.GetContractServiceByGroupInvoiceNumber(groupInvoiceNumber);
        }

        [HttpGet("{id}")]
        public async Task<ApiCommonResponse> GetById(long id)
        {
            return await _contractServiceService.GetContractServiceById(id);
        }


        [HttpDelete("{id}")]
        public async Task<ApiCommonResponse> DeleteById(int id)
        {
            return await _contractServiceService.DeleteContractService(id);
        }
    }
}