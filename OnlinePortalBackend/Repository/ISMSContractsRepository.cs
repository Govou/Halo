using Halobiz.Common.DTOs.ReceivingDTOs;
using OnlinePortalBackend.Repository.Impl;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace OnlinePortalBackend.Repository
{
    public interface ISMSContractsRepository
    {
        Task<(bool isSuccess, object message)> AddNewContract(SMSContractDTO contractDTO);
        Task<(bool isSuccess, string message, List<InvoiceResult> invoiceResults)> GenerateInvoiceForContract(SMSCreateInvoiceDTO request);
        Task<(bool isSuccess, object message)> AddServiceToContract(SMSContractServiceDTO request);
    }
}
