using System.Collections.Generic;
using System.Threading.Tasks;
using Halobiz.Common.Auths;
using Halobiz.Common.DTOs.ApiDTOs;
using HaloBiz.DTOs.ReceivingDTOs;
using HaloBiz.DTOs.TransferDTOs;
using HaloBiz.MyServices;
using Microsoft.AspNetCore.Mvc;

namespace HaloBiz.Controllers.AccountsModel
{
    [Route("api/v1/[controller]")]
    [ApiController]
    [ModuleName(HalobizModules.Finance,6)]

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
            return await _invoiceService.GetAllInvoice();
        }

        [HttpGet("ContractDivision/{contractDivisionId}")]
        public async Task<ApiCommonResponse> GetInvoicesByContractDivisionId(long contractDivisionId)
        {
            return await _invoiceService.GetAllInvoicesByContactserviceId(contractDivisionId);
        }

        [HttpGet("Proforma/ContractDivision/{contractDivisionId}")]
        public async Task<ApiCommonResponse> GetProformaInvoicesByContractDivisionId(long contractDivisionId)
        {
            return await _invoiceService.GetAllProformaInvoicesByContactserviceId(contractDivisionId);
        }

        [HttpGet("GetGroupInvoiceSendDateByContractId/{contractId}")]
        public async Task<ApiCommonResponse> GetGroupInvoiceSendDateByContractId(long contractId)
        {
            return await _invoiceService.GetGroupInvoiceSendDateByContractId(contractId);
        }

        [HttpGet("{id}")]
        public async Task<ApiCommonResponse> GetById(long id)
        {
            return await _invoiceService.GetAllInvoicesById(id);
        }

        [HttpGet("SendInvoice/{invoiceId}")]
        public async Task<ApiCommonResponse> SendInvoice(long invoiceId)
        {
            return await _invoiceService.SendInvoice(invoiceId);
        }


        

        [HttpGet("GetInvoiceDetails/{invoiceId}")]

        public async Task<ApiCommonResponse> SendInvoiceDetails(long invoiceId)
        {
            return await _invoiceService.GetInvoiceDetails(invoiceId);
        }
       


        [HttpPost("AdHocInvoice")]
        public async Task<ApiCommonResponse> AddNewinvoice(InvoiceReceivingDTO invoiceReceivingDTO)
        {
            return await _invoiceService.AddInvoice(HttpContext, invoiceReceivingDTO);
        }

        [HttpPost("GroupAdHocInvoice")]
        public async Task<ApiCommonResponse> AddNewGroupInvoice(GroupInvoiceDto groupInvoiceDto)
        {
            return await _invoiceService.AddGroupInvoice(HttpContext, groupInvoiceDto);
        }


        [HttpPut("{id}")]
        public async Task<ApiCommonResponse> UpdateById(long id, InvoiceReceivingDTO invoiceReceiving)
        {
            return await _invoiceService.UpdateInvoice(HttpContext, id, invoiceReceiving);
        }      
       

        [HttpPut("ConvertToFinalInvoice")]
        public async Task<ApiCommonResponse> ConverToFinal(List<long> invoiceIds)
        {
            return await _invoiceService.ConvertInvoiceToFinalInvoice(invoiceIds, HttpContext);
        }

        [HttpPut("RemoveProformaInvoice/{invoiceId}")]
        public async Task<ApiCommonResponse> RemoveProformaInvoice(long invoiceId)
        {
            return await _invoiceService.RemoveProformaInvoice(invoiceId);
        }

        [HttpDelete("{id}")]
        public async Task<ApiCommonResponse> DeleteById(long id)
        {
            return await _invoiceService.DeleteInvoice(id);
        }

        [HttpPost("PostPeriodicInvoice")]
        public async Task<ApiCommonResponse> SendPeriodicInvoices()
        {
            return await _invoiceService.SendPeriodicInvoices();
        }

        [HttpGet("GetInvoiceDetails")]
        public async Task<ApiCommonResponse> GetInvoiceDetails(string groupInvoiceNumber, string startDate)
        {
            return await _invoiceService.GetInvoiceDetails(groupInvoiceNumber, startDate);
        }
    }
}