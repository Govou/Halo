using Halobiz.Common.DTOs.ReceivingDTOs;
using Halobiz.Common.DTOs.TransferDTOs;
using OnlinePortalBackend.DTOs.TransferDTOs;
using System.Threading.Tasks;

namespace OnlinePortalBackend.Repository
{
    public interface ISMSInvoiceRepository
    {
        Task<(bool isSuccess, SMSInvoiceDTO message)> GetInvoice(int profileId);
        Task<(bool isSuccess, object message)> ReceiptInvoice(SMSReceiptReceivingDTO request);
        Task<SendReceiptDTO> GetReceiptDetail(string invoiceNumber);
        Task<(bool isSuccess, string message)> ReceiptAllInvoicesForContract(SMSReceiptInvoiceForContractDTO request);
    }
}
