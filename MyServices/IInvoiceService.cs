using System.Threading.Tasks;
using HaloBiz.DTOs.ApiDTOs;
using HaloBiz.DTOs.ReceivingDTOs;
using Microsoft.AspNetCore.Http;

namespace HaloBiz.MyServices
{
    public interface IInvoiceService
    {
        Task<ApiResponse> AddInvoice(HttpContext context, InvoiceReceivingDTO invoiceReceivingDTO);
        Task<ApiResponse> AddGroupInvoice(HttpContext httpContext, GroupInvoiceDto groupInvoiceDto);
        Task<ApiResponse> DeleteInvoice(long id);
        Task<ApiResponse> GetAllInvoice();
        Task<ApiResponse> GetAllInvoicesById(long id);
        Task<ApiResponse> GetAllInvoicesByContactserviceId(long contractServiceId);
        Task<ApiResponse> GetAllProformaInvoicesByContactserviceId(long contractServiceId);
        Task<ApiResponse> UpdateInvoice(HttpContext context, long id, InvoiceReceivingDTO invoiceReceivingDTO);
        Task<ApiResponse> ConvertProformaInvoiceToFinalInvoice(HttpContext httpContext, long invoiceId);
        Task<ApiResponse> SendPeriodicInvoices();
        Task<ApiResponse> SendInvoice(long invoiceId);
        Task<ApiResponse> GetInvoiceDetails(long invoiceId);
    }
}