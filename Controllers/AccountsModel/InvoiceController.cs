using System.Threading.Tasks;
using HaloBiz.DTOs.ApiDTOs;
using HaloBiz.DTOs.ReceivingDTOs;
using HaloBiz.DTOs.TransferDTOs;
using HaloBiz.MyServices;
using Microsoft.AspNetCore.Mvc;

namespace HaloBiz.Controllers.AccountsModel
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class InvoiceController : ControllerBase
    {
        private readonly IInvoiceService _invoiceService;

        public InvoiceController(IInvoiceService invocieService)
        {
            this._invoiceService = invocieService;
        }

        [HttpGet("")]
        public async Task<ActionResult> GetInvoices()
        {
            var response = await _invoiceService.GetAllInvoice();
            if (response.StatusCode >= 400)
                return StatusCode(response.StatusCode, response);
            var invoice = ((ApiOkResponse)response).Result;
            return Ok(invoice);
        }

        [HttpGet("ContractDivision/{contractDivisionId}")]
        public async Task<ActionResult> GetInvoicesByContractDivisionId(long contractDivisionId)
        {
            var response = await _invoiceService.GetAllInvoicesByContactserviceId(contractDivisionId);
            if (response.StatusCode >= 400)
                return StatusCode(response.StatusCode, response);
            var invoices = ((ApiOkResponse)response).Result;
            return Ok(invoices);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult> GetById(long id)
        {
            var response = await _invoiceService.GetAllInvoicesById(id);
            if (response.StatusCode >= 400)
                return StatusCode(response.StatusCode, response);
            var invoice = ((ApiOkResponse)response).Result;
            return Ok(invoice);
        }

        [HttpPost("AdHocInvoice")]
        public async Task<ActionResult> AddNewinvoice(InvoiceReceivingDTO invoiceReceivingDTO)
        {
            var response = await _invoiceService.AddInvoice(HttpContext, invoiceReceivingDTO);
            if (response.StatusCode >= 400)
                return StatusCode(response.StatusCode, response);
            var invoice = ((ApiOkResponse)response).Result;
            return Ok(invoice);
        }

        [HttpPost("GroupAdHocInvoice")]
        public async Task<ActionResult> AddNewGroupInvoice(GroupInvoiceDto groupInvoiceDto)
        {
            var response = await _invoiceService.AddGroupInvoice(HttpContext, groupInvoiceDto);
            if (response.StatusCode >= 400)
                return StatusCode(response.StatusCode, response);
            var invoice = ((ApiOkResponse)response).Result;
            return Ok(invoice);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateById(long id, InvoiceReceivingDTO invoiceReceiving)
        {
            var response = await _invoiceService.UpdateInvoice(HttpContext, id, invoiceReceiving);
            if (response.StatusCode >= 400)
                return StatusCode(response.StatusCode, response);
            var invoice = ((ApiOkResponse)response).Result;
            return Ok(invoice);
        }

        [HttpPut("ConvertToFinalInvoice/{invoiceId}")]
        public async Task<IActionResult> ConverToFinal(long invoiceId)
        {
            var response = await _invoiceService.ConvertProformaInvoiceToFinalInvoice(HttpContext ,invoiceId);
            if (response.StatusCode >= 400)
                return StatusCode(response.StatusCode, response);
            var invoice = ((ApiOkResponse)response).Result;
            return Ok(invoice);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteById(long id)
        {
            var response = await _invoiceService.DeleteInvoice(id);
            return StatusCode(response.StatusCode);
        }
    }
}