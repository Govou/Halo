using Halobiz.Common.DTOs.ApiDTOs;
using OnlinePortalBackend.Repository;
using System.Threading.Tasks;
using System;

namespace OnlinePortalBackend.MyServices.Impl
{
    public class InvoiceService : IInvoiceService
    {
        private readonly IInvoiceRepository _invoiceRepository;
        public InvoiceService(IInvoiceRepository invoiceRepository)
        {
            _invoiceRepository = invoiceRepository;
        }

        public async Task<ApiCommonResponse> GetInvoice(string invoiceNumber, DateTime invoiceDate)
        {
            var invoices = await _invoiceRepository.GetInvoice(invoiceNumber, invoiceDate);

            if (invoices == null)
            {
                return CommonResponse.Send(ResponseCodes.NO_DATA_AVAILABLE);
            }
            return CommonResponse.Send(ResponseCodes.SUCCESS, invoices);
        }

        public async Task<ApiCommonResponse> GetInvoices(int userId)
        {
            var invoices = await _invoiceRepository.GetInvoices(userId);

            if (invoices == null)
            {
                return CommonResponse.Send(ResponseCodes.NO_DATA_AVAILABLE);
            }
            return CommonResponse.Send(ResponseCodes.SUCCESS, invoices);

        }
    }
}
