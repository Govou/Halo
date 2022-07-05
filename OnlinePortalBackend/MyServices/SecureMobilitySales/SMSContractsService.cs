using Halobiz.Common.DTOs.ApiDTOs;
using Halobiz.Common.DTOs.ReceivingDTOs;
using Halobiz.Common.DTOs.TransferDTOs;
using Halobiz.Common.Helpers;
using HalobizMigrations.Models;
using OnlinePortalBackend.DTOs.ReceivingDTOs;
using OnlinePortalBackend.DTOs.TransferDTOs;
using OnlinePortalBackend.Helpers;
using OnlinePortalBackend.Repository;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace OnlinePortalBackend.MyServices.SecureMobilitySales
{
    public class SMSContractsService : ISMSContractsService
    {
        private readonly ISMSContractsRepository _contractsRepo;
        private readonly ISMSInvoiceRepository _invoiceRepo;
        private readonly IMailService _mailService;
        private readonly ISupplierRepository _supplierRepo;
        public SMSContractsService(ISMSContractsRepository contractsRepo, ISMSInvoiceRepository invoiceRepository, IMailService mailService, ISupplierRepository supplierRepository)
        {
            _contractsRepo = contractsRepo;
            _invoiceRepo = invoiceRepository;
            _mailService = mailService;
            _supplierRepo = supplierRepository;
        }

        public async Task<ApiCommonResponse> AddServiceToContract(SMSContractServiceDTO request)
        {
            var result = await _contractsRepo.AddServiceToContract(request);

            if (!result.isSuccess)
            {
                return CommonResponse.Send(ResponseCodes.FAILURE); 
            }
            return CommonResponse.Send(ResponseCodes.SUCCESS, result);
        }

 

        public async Task<ApiCommonResponse> CreateContract(SMSContractDTO contractDTO)
        {
            try
            {
                var result = await _contractsRepo.AddNewContract(contractDTO);

                if (!result.isSuccess)
                {
                    return CommonResponse.Send(ResponseCodes.FAILURE); 
                }
                return CommonResponse.Send(ResponseCodes.SUCCESS, result);
            }
            catch (Exception)
            {
                return CommonResponse.Send(ResponseCodes.FAILURE);
            }
          
        }

        public async Task<ApiCommonResponse> GenerateInvoice(SMSCreateInvoiceDTO request)
        {
            var result = await _contractsRepo.GenerateInvoiceForContract(request);

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

        public async Task<ApiCommonResponse> PostSupplierTransactions(PostTransactionDTO request)
        {
            var result = await _supplierRepo.PostTransactionForBooking(request);

            if (!result)
            {
                return CommonResponse.Send(ResponseCodes.FAILURE, null, "failed"); ;
            }
            return CommonResponse.Send(ResponseCodes.SUCCESS, null, "success");
        }

        public async Task<ApiCommonResponse> PostTransactions(PostTransactionDTO request)
        {
            var result = await _invoiceRepo.PostTransactions(request);

            if (!result)
            {
                return CommonResponse.Send(ResponseCodes.FAILURE); ;
            }
            return CommonResponse.Send(ResponseCodes.SUCCESS, result);
        }

        public async Task<ApiCommonResponse> ReceiptAllInvoicesForContract(SMSReceiptInvoiceForContractDTO request)
        {

            var result = await _invoiceRepo.ReceiptAllInvoicesForContract(request);

            if (!result.isSuccess)
            {
                return CommonResponse.Send(ResponseCodes.FAILURE); ;
            }

            var receipt = await _invoiceRepo.GetContractServiceDetailsForReceipt(request.ContractId, request.ContractServices);
          //  var receipt = new SendReceiptDTO();
            await SendReceiptViaEmail(receipt);

            return CommonResponse.Send(ResponseCodes.SUCCESS, result);
          //  return CommonResponse.Send(ResponseCodes.SUCCESS, null);
        }

        public async Task<ApiCommonResponse> ReceiptInvoice(SMSReceiptReceivingDTO request)
        {
            //var result = await _invoiceRepo.ReceiptInvoice(request);

            //if (!result.isSuccess)
            //{
            //    return CommonResponse.Send(ResponseCodes.FAILURE);
            //}


         //   await SendReceiptViaEmail(request);
            return CommonResponse.Send(ResponseCodes.SUCCESS, "success");
           // return CommonResponse.Send(ResponseCodes.SUCCESS, result);
        }

        public async Task<ApiCommonResponse> RemoveServiceFromContract(SMSContractServiceRemovalDTO request)
        {
            var result = await _contractsRepo.RemoveServiceFromContract(request);

            if (!result.isSuccess)
            {
                return CommonResponse.Send(ResponseCodes.FAILURE);
            }
            return CommonResponse.Send(ResponseCodes.SUCCESS, result);
        }

        private async Task SendReceiptViaEmail(SendReceiptDTO request)
        {
            //var sendReceiptDetail1 = new SendReceiptDetailDTO
            //{
            //    Amount = "7000",
            //    Description = "Secure mobility test",
            //    Quantity = "2",
            //    ServiceName = "Pilot service",
            //    Total = "43000"
            //};

            //var sendReceiptDetail2 = new SendReceiptDetailDTO
            //{
            //    Amount = "7000",
            //    Description = "Secure mobility test",
            //    Quantity = "2",
            //    ServiceName = "Pilot service",
            //    Total = "43000"
            //};

            //var sendReceiptDetails = new List<SendReceiptDetailDTO>();
            //sendReceiptDetails.Add(sendReceiptDetail1);
            //sendReceiptDetails.Add(sendReceiptDetail2);

            //var acct = new SendReceiptDTO
            //{
            //    Amount = "350000",
            //    CustomerName = "Abolade Samuel",
            //    Email = "guvoekemauton@gmail.com",
            //    Date = DateTime.Now.ToString(),
            //    SendReceiptDetailDTOs = sendReceiptDetails
            //};

            //var amountInWords = NumberToWordConverter.ChangeToWordsInMoney("35000");

            //request = acct;

            var amountInWords = NumberToWordConverter_v2.ConvertMoneyToWords(request.Amount);

            var VAT = 0.075 * double.Parse(request.Amount);
            var amount = double.Parse(request.Amount) - VAT;

            var receiptSummaryPartialPage = HTMLGenerator.GenerateReceiptSummary()
                                                                                .Replace("{{Amount}}", request.Amount.ToString())
                                                                                .Replace("{{date}}", DateTime.Now.ToString("dd/MM/yyyy"));



            var receiptDetailPartialPage = string.Empty;

            foreach (var item in request.SendReceiptDetailDTOs)
            {
                receiptDetailPartialPage += HTMLGenerator.GenerateReceiptDetail()
                                                                                 .Replace("{{ServiceName}}", item.ServiceName)
                                                                                 .Replace("{{Quantity}}", item.Quantity)
                                                                                 .Replace("{{ServiceDescription}}", item.Description)
                                                                                 .Replace("{{Amount}}", item.Amount)
                                                                                 .Replace("{{Total}}", item.Total);

            }



            receiptDetailPartialPage += HTMLGenerator.GenerateReceiptTotal()
                                                                                .Replace("{{ServiceName}}", "")
                                                                                .Replace("{{ServiceDescription}}", "Total")
                                                                                .Replace("{{Quantity}}", "")
                                                                                .Replace("{{Amount}}", "")
                                                                                .Replace("{{total}}", $"{amount.ToString("0.00")}");

            receiptDetailPartialPage += HTMLGenerator.GenerateReceiptVAT()
                                                                                .Replace("{{ServiceName}}", "")
                                                                                .Replace("{{ServiceDescription}}", "VAT")
                                                                                .Replace("{{Quantity}}", "")
                                                                                .Replace("{{Amount}}", "")
                                                                                .Replace("{{vat}}", $"{VAT.ToString("0.00")}");

            receiptDetailPartialPage += HTMLGenerator.GenerateReceiptTotal_VAT()
                                                                                .Replace("{{ServiceName}}", "")
                                                                                .Replace("{{ServiceDescription}}", "Total + VAT")
                                                                                .Replace("{{Quantity}}", "")
                                                                                .Replace("{{Amount}}", "")
                                                                                .Replace("{{total_vat}}", $"{double.Parse(request.Amount).ToString("0.00")}");

            var receiptDetailsFooterPartialPage = HTMLGenerator.GenerateReceiptDetailsFooter()
                                                                                           .Replace("{{AmountInWords}}", "Total Amount in words: " + amountInWords);


            var receiptRequest = new ReceiptSendDTO
            {
                AmountInWords = amountInWords,
                Subject = "Receipt",
                ReceiptSummary = receiptSummaryPartialPage,
                CustomerName = request.CustomerName,
                ReceiptItems = receiptDetailPartialPage,
                ReceiptFooter = receiptDetailsFooterPartialPage,
                Email = new string[] { request.Email }
            };


            await _mailService.SendInvoiceReceipt(receiptRequest);
        }
    }
}
