using Halobiz.Common.DTOs.ApiDTOs;
using Halobiz.Common.DTOs.ReceivingDTOs;
using System.Threading.Tasks;

namespace OnlinePortalBackend.MyServices.SecureMobilitySales
{
    public interface ISMSContractsService
    {
        Task<ApiCommonResponse> CreateContract(SMSContractDTO contractDTO);
        Task<ApiCommonResponse> AddServiceToContract(SMSContractServiceDTO contractDTO);
        Task<ApiCommonResponse> GetInvoice(int profileId);
        Task<ApiCommonResponse> ReceiptInvoice(SMSReceiptReceivingDTO request);
        Task<ApiCommonResponse> GenerateInvoice(SMSCreateInvoiceDTO request);
        Task<ApiCommonResponse> ReceiptAllInvoicesForContract(SMSReceiptInvoiceForContractDTO request);



    }
}
