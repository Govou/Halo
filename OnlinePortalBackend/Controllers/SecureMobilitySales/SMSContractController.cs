using Halobiz.Common.DTOs.ApiDTOs;
using Halobiz.Common.DTOs.ReceivingDTOs;
using Microsoft.AspNetCore.Mvc;
using OnlinePortalBackend.MyServices.SecureMobilitySales;
using System.Threading.Tasks;

namespace OnlinePortalBackend.Controllers.SecureMobilitySales
{
    [Route("api/[controller]")]
    [ApiController]
    public class SMSContractController : ControllerBase
    {
        private readonly ISMSContractsService _contractsService;
        public SMSContractController(ISMSContractsService contractsService)
        {
            _contractsService = contractsService;
        }

        [HttpPost("CreateNewContract")]
        public async Task<ApiCommonResponse> CreateContract(SMSContractDTO request)
        {
            return await _contractsService.CreateContract(request);
        }

        [HttpGet("GetInvoice")]
        public async Task<ApiCommonResponse> GetInvoice(int profileId)
        {
            return await _contractsService.GetInvoice(profileId);
        }

        [HttpPost("GenerateInvoice")]
        public async Task<ApiCommonResponse> GenerateInvoice(SMSCreateInvoiceDTO request)
        {
            return await _contractsService.GenerateInvoice(request);
        }

        [HttpPost("ReceiptInvoice")]
        public async Task<ApiCommonResponse> ReceiptInvoice(SMSReceiptReceivingDTO request)
        {
            return await _contractsService.ReceiptInvoice(request);
        }
    }
}
