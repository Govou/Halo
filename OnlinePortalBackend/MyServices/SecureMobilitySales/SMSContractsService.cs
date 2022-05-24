using Halobiz.Common.DTOs.ApiDTOs;
using Halobiz.Common.DTOs.ReceivingDTOs;
using Halobiz.Common.DTOs.TransferDTOs;
using Halobiz.Common.Helpers;
using HalobizMigrations.Models;
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
        public SMSContractsService(ISMSContractsRepository contractsRepo, ISMSInvoiceRepository invoiceRepository, IMailService mailService)
        {
            _contractsRepo = contractsRepo;
            _invoiceRepo = invoiceRepository;
            _mailService = mailService;
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
            //var result = await _invoiceRepo.ReceiptInvoice(request);

            //if (!result.isSuccess)
            //{
            //    return CommonResponse.Send(ResponseCodes.FAILURE); ;
            //}


            await SendReceiptViaEmail(request);
            return CommonResponse.Send(ResponseCodes.SUCCESS, "success");
           // return CommonResponse.Send(ResponseCodes.SUCCESS, result);
        }

        private async Task SendReceiptViaEmail(SMSReceiptReceivingDTO request)
        {

            //    var amountInWords = NumberToWordConverter.ChangeToWordsInMoney(request.InvoiceValue.ToString());
            var amountInWords = NumberToWordConverter.ChangeToWordsInMoney(77918700.20.ToString());

            var sendDetail = new List<SendReceiptDetailDTO>();
            sendDetail.Add(new SendReceiptDetailDTO
            {
                Amount = 30000.ToString(),
                Description = "This is a good service. Trust me",
                Quantity = 3.ToString(),
                ServiceName = "Bull Dog",
                Total = 78730.ToString()
            });

            sendDetail.Add(new SendReceiptDetailDTO
            {
                Amount = 50000.ToString(),
                Description = "This is a good service. Trust me!!!",
                Quantity = 4.ToString(),
                ServiceName = "Plane",
                Total = 87237.00.ToString()
            });

            var result = new SendReceiptDTO
            {
                CustomerName = "Kehinde Guvoeke",
                Amount = 2389238.89.ToString(),
                Date = DateTime.UtcNow.AddHours(1).ToString("dd/MM/yyyy"),
                Email = "guvoekemauton@gmail.com",
                SendReceiptDetailDTOs = sendDetail
            }; //await _invoiceRepo.GetReceiptDetail(request.InvoiceNumber);

            
            var receiptSummaryPartialPage = HTMLGenerator.GenerateReceiptSummary()
                                                                                .Replace("{{Amount}}", result.Amount.ToString())
                                                                                .Replace("{{date}}", DateTime.Now.ToString("dd/MM/yyyy"));



            var receiptDetailPartialPage = string.Empty;

            foreach (var item in result.SendReceiptDetailDTOs)
            {
                receiptDetailPartialPage += HTMLGenerator.GenerateReceiptDetail()
                                                                                 .Replace("{{ServiceName}}", item.ServiceName)
                                                                                 .Replace("{{Quantity}}", item.Quantity)
                                                                                 .Replace("{{ServiceDescription}}", item.Description)
                                                                                 .Replace("{{Amount}}", item.Amount)
                                                                                 .Replace("{{Total}}", item.Total);

            }

            receiptDetailPartialPage += HTMLGenerator.GenerateReceiptDetail()
                                                                                .Replace("{{ServiceName}}", "")
                                                                                .Replace("{{ServiceDescription}}", "Total")
                                                                                .Replace("{{Quantity}}", "")
                                                                                .Replace("{{Amount}}", "")
                                                                                .Replace("{{Total}}", $"{result.Amount}");

            receiptDetailPartialPage += HTMLGenerator.GenerateReceiptDetail()
                                                                                .Replace("{{ServiceName}}", "")
                                                                                .Replace("{{ServiceDescription}}", "VAT")
                                                                                .Replace("{{Quantity}}", "")
                                                                                .Replace("{{Amount}}", "")
                                                                                .Replace("{{Total}}", $"{0}");

            receiptDetailPartialPage += HTMLGenerator.GenerateReceiptDetail()
                                                                                .Replace("{{ServiceName}}", "")
                                                                                .Replace("{{ServiceDescription}}", "Total + VAT")
                                                                                .Replace("{{Quantity}}", "")
                                                                                .Replace("{{Amount}}", "")
                                                                                .Replace("{{Total}}", $"{result.Amount}" );

            var receiptDetailsFooterPartialPage = HTMLGenerator.GenerateReceiptDetailsFooter()
                                                                                           .Replace("{{AmountInWords}}", "Total Amount in words: " + amountInWords);


            var receiptRequest = new ReceiptSendDTO
            {
                AmountInWords = amountInWords,
                Subject = "Receipt",
                ReceiptSummary = receiptSummaryPartialPage,
                CustomerName = result.CustomerName,
                ReceiptItems = receiptDetailPartialPage,
                ReceiptFooter = receiptDetailsFooterPartialPage,
                Email = new string[] { result.Email }
            };


            await _mailService.SendInvoiceReceipt(receiptRequest);
        }
    }
}
