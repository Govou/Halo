using System.Collections.Generic;
using System.Threading.Tasks;
using Halobiz.Common.DTOs.ApiDTOs;
using HaloBiz.DTOs.ReceivingDTOs;
using Microsoft.AspNetCore.Http;

namespace HaloBiz.MyServices
{
    public interface IInvoiceService
    {
        Task<ApiCommonResponse> AddInvoice(HttpContext context, InvoiceReceivingDTO invoiceReceivingDTO);
        Task<ApiCommonResponse> AddGroupInvoice(HttpContext httpContext, GroupInvoiceDto groupInvoiceDto);
        Task<ApiCommonResponse> DeleteInvoice(long id);
        Task<ApiCommonResponse> GetAllInvoice();
        Task<ApiCommonResponse> GetAllInvoicesById(long id);
        Task<ApiCommonResponse> GetAllInvoicesByContactserviceId(long contractServiceId);
        Task<ApiCommonResponse> GetAllProformaInvoicesByContactserviceId(long contractServiceId);
        Task<ApiCommonResponse> UpdateInvoice(HttpContext context, long id, InvoiceReceivingDTO invoiceReceivingDTO);
        Task<ApiCommonResponse> ConvertInvoiceToFinalInvoice(List<long> invoiceIds, HttpContext httpContext);
        Task<ApiCommonResponse> SendPeriodicInvoices();
        Task<ApiCommonResponse> SendInvoice(long invoiceId);
        Task<ApiCommonResponse> SendJourneyManagementPlan(long serviceAssignmentId);
        Task<ApiCommonResponse> GetJMPDetails(long serviceAssignmentId);
        Task<ApiCommonResponse> GetInvoiceDetails(long invoiceId);
        Task<ApiCommonResponse> RemoveProformaInvoice(long invoiceId);
        Task<ApiCommonResponse> GetGroupInvoiceSendDateByContractId(long contractId);

    }
}