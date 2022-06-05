using Halobiz.Common.Auths;
using Halobiz.Common.DTOs.ApiDTOs;
using Microsoft.AspNetCore.Mvc;

namespace HaloBiz.Controllers.AccountsModel
{
    [Route("api/v1/[controller]")]
    [ApiController]
    [ModuleName(HalobizModules.Finance, 127)]
    public class AdvancePaymentController : Controller
    {
        [HttpGet("")]
        public async Task<ApiCommonResponse> GetAdvancePayments()
        {
                return await _invoiceService.GetAllInvoice();
        }


        [HttpGet("{id}")]
        public async Task<ApiCommonResponse> GetByCustomerId(long id)
        {
            return await _invoiceService.GetAllInvoicesById(id);
        }
        [HttpPut("{id}")]
        public async Task<ApiCommonResponse> Update(long id)
        {
            return await _invoiceService.SendInvoice(invoiceId);
        }

        [HttpPut("{id}")]
        public async Task<ApiCommonResponse> Post(long id)
        {
            return await _invoiceService.SendInvoice(invoiceId);
        }
    }
}
