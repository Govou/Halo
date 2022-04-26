using Halobiz.Common.DTOs.ApiDTOs;
using System;
using System.Threading.Tasks;

namespace OnlinePortalBackend.MyServices
{
    public interface IInvoiceService
    {
        Task<ApiCommonResponse> GetInvoices(int userId);
        Task<ApiCommonResponse> GetInvoice(string invoiceNumber, DateTime invoiceDate);
    }
}
