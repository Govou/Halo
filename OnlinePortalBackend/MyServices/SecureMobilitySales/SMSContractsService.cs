using Halobiz.Common.DTOs.ApiDTOs;
using Halobiz.Common.DTOs.ReceivingDTOs;
using HalobizMigrations.Models;
using OnlinePortalBackend.Repository;
using System;
using System.Threading.Tasks;

namespace OnlinePortalBackend.MyServices.SecureMobilitySales
{
    public class SMSContractsService : ISMSContractsService
    {
        private readonly ISMSContractsRepository _contractsRepo;
        private readonly ISMSInvoiceRepository _invoiceRepo;
        public SMSContractsService(ISMSContractsRepository contractsRepo, ISMSInvoiceRepository invoiceRepository)
        {
            _contractsRepo = contractsRepo;
            _invoiceRepo = invoiceRepository;
        }
        public async Task<ApiCommonResponse> CreateContract(SMSContractDTO contractDTO)
        {
           var result = await _contractsRepo.AddNewContract(contractDTO);

            if (!result.isSuccess)
            {
                return CommonResponse.Send(ResponseCodes.FAILURE); ;
            }
            return CommonResponse.Send(ResponseCodes.SUCCESS, result);
        }

        public async Task<ApiCommonResponse> GetInvoice(int profileId)
        {
            var result = await _invoiceRepo.GetInvoice(profileId);

            if (!result.isSuccess)
            {
                return CommonResponse.Send(ResponseCodes.FAILURE); ;
            }
            return CommonResponse.Send(ResponseCodes.SUCCESS, result);
        }

        public async Task<ApiCommonResponse> ReceiptInvoice(SMSReceiptReceivingDTO request)
        {
            var result = await _invoiceRepo.ReceiptInvoice(request);

            if (!result.isSuccess)
            {
                return CommonResponse.Send(ResponseCodes.FAILURE); ;
            }
            return CommonResponse.Send(ResponseCodes.SUCCESS, result);
        }
    }
}
