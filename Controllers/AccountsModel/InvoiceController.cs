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
        public async Task<ApiCommonResponse> GetInvoices()
        {
            var response = await _invoiceService.GetAllInvoice();
            if (response.StatusCode >= 400)
                return StatusCode(response.StatusCode, response);
            var invoice = ((ApiOkResponse)response).Result;
            return Ok(invoice);
        }

        [HttpGet("ContractDivision/{contractDivisionId}")]
        public async Task<ApiCommonResponse> GetInvoicesByContractDivisionId(long contractDivisionId)
        {
            var response = await _invoiceService.GetAllInvoicesByContactserviceId(contractDivisionId);
            if (response.StatusCode >= 400)
                return StatusCode(response.StatusCode, response);
            var invoices = ((ApiOkResponse)response).Result;
            return Ok(invoices);
        }

        [HttpGet("Proforma/ContractDivision/{contractDivisionId}")]
        public async Task<ApiCommonResponse> GetProformaInvoicesByContractDivisionId(long contractDivisionId)
        {
            var response = await _invoiceService.GetAllProformaInvoicesByContactserviceId(contractDivisionId);
            if (response.StatusCode >= 400)
                return StatusCode(response.StatusCode, response);
            var invoices = ((ApiOkResponse)response).Result;
            return Ok(invoices);
        }

        [HttpGet("{id}")]
        public async Task<ApiCommonResponse> GetById(long id)
        {
            var response = await _invoiceService.GetAllInvoicesById(id);
            if (response.StatusCode >= 400)
                return StatusCode(response.StatusCode, response);
            var invoice = ((ApiOkResponse)response).Result;
            return Ok(invoice);
        }

        [HttpGet("SendInvoice/{invoiceId}")]
        public async Task<ApiCommonResponse> SendInvoice(long invoiceId)
        {
            var response = await _invoiceService.SendInvoice(invoiceId);
            if (response.StatusCode >= 400)
                return StatusCode(response.StatusCode, response);
            var invoice = ((ApiOkResponse)response).Result;
            return Ok(invoice);
        }

        [HttpGet("GetInvoiceDetails/{invoiceId}")]
        [HttpGet("GetInvoiceDetails/{invoiceId}/{isAdhocAndGrouped}")]

        public async Task<ApiCommonResponse> SendInvoiceDetails(long invoiceId, bool isAdhocAndGrouped = false)
        {
            var response = await _invoiceService.GetInvoiceDetails(invoiceId, isAdhocAndGrouped);
            if (response.StatusCode >= 400)
                return StatusCode(response.StatusCode, response);
            var invoice = ((ApiOkResponse)response).Result;
            return Ok(invoice);
        }

        //[HttpGet("GetInvoiceDetails/{groupinvoicenumber}/{startdate}")]
        //public async Task<ApiCommonResponse> GetInvoiceDetails(string groupinvoicenumber, string startdate)
        //{
        //    return await _invoiceService.GetInvoiceDetails(groupinvoicenumber, startdate);
        //}


        [HttpPost("AdHocInvoice")]
        public async Task<ApiCommonResponse> AddNewinvoice(InvoiceReceivingDTO invoiceReceivingDTO)
        {
            var response = await _invoiceService.AddInvoice(HttpContext, invoiceReceivingDTO);
            if (response.StatusCode >= 400)
                return StatusCode(response.StatusCode, response);
            var invoice = ((ApiOkResponse)response).Result;
            return Ok(invoice);
        }

        [HttpPost("GroupAdHocInvoice")]
        public async Task<ApiCommonResponse> AddNewGroupInvoice(GroupInvoiceDto groupInvoiceDto)
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

        [HttpPut("RemoveProformaInvoice/{invoiceId}")]
        public async Task<ApiCommonResponse> RemoveProformaInvoice(long invoiceId)
        {
            return await _invoiceService.RemoveProformaInvoice(invoiceId);
        }

        [HttpDelete("{id}")]
        public async Task<ApiCommonResponse> DeleteById(long id)
        {
            var response = await _invoiceService.DeleteInvoice(id);
            return StatusCode(response.StatusCode);
        }

        [HttpPost("PostPeriodicInvoice")]
        public async Task<ApiCommonResponse> SendPeriodicInvoices()
        {
            var response = await _invoiceService.SendPeriodicInvoices();
             if (response.StatusCode >= 400)
                return StatusCode(response.StatusCode, response);
            var res = ((ApiOkResponse)response).Result;
            return Ok(res);
        }
    }
}