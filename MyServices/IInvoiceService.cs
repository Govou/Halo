using System.Threading.Tasks;
using HaloBiz.DTOs.ApiDTOs;
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
        Task<ApiCommonResponse> ConvertProformaInvoiceToFinalInvoice(HttpContext httpContext, long invoiceId);
        Task<ApiCommonResponse> SendPeriodicInvoices();
        Task<ApiCommonResponse> SendInvoice(long invoiceId);
        Task<ApiCommonResponse> GetInvoiceDetails(long invoiceId, bool isAdhocAndGrouped);
        Task<ApiCommonResponse> RemoveProformaInvoice(long invoiceId);
        Task<ApiCommonResponse> GetInvoiceDetails(string groupinvoiceNumber, string startdate);
    }
}