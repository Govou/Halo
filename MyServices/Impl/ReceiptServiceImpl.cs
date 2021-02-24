using System;
using System.Threading.Tasks;
using AutoMapper;
using HaloBiz.Data;
using HaloBiz.DTOs.ApiDTOs;
using HaloBiz.DTOs.ReceivingDTOs;
using HaloBiz.DTOs.TransferDTOs;
using HaloBiz.Helpers;
using HaloBiz.Model.AccountsModel;
using HaloBiz.Repository;
using HaloBiz.Repository.LAMS;
using halobiz_backend.Helpers;
using halobiz_backend.Model.AccountsModel;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace HaloBiz.MyServices.Impl
{
    public class ReceiptServiceImpl : IReceiptService
    {
        private readonly ILogger<ReceiptServiceImpl> _logger;
        private readonly IReceiptRepository _receiptRepo;
        private readonly IModificationHistoryRepository _modificationRepo;
        private readonly IMapper _mapper;
        private readonly DataContext _context;
        private readonly IInvoiceRepository _invoiceRepo;
        private readonly IAccountRepository _accountRep;
        private readonly IFinancialVoucherTypeRepository _voucherRepo;
        private readonly string WITHOLDING_TAX = "WITHOLDING TAX";
        private readonly string RECEIPTVOUCHERTYPE = "RECEIPT";
        private long LoggedInUserId;

        public ReceiptServiceImpl(ILogger<ReceiptServiceImpl> logger, IReceiptRepository receiptRepo,
                     IModificationHistoryRepository modificationRepo, IMapper mapper, DataContext context,
                        IInvoiceRepository invoiceRepo, IAccountRepository accountRep, IFinancialVoucherTypeRepository voucherRepo )
        {
            this._logger = logger;
            this._receiptRepo = receiptRepo;
            this._modificationRepo = modificationRepo;
            this._mapper = mapper;
            this._context = context;
            this._invoiceRepo = invoiceRepo;
            this._accountRep = accountRep;
            this._voucherRepo = voucherRepo;
        }

        public async  Task<ApiResponse> AddReceipt(HttpContext context, ReceiptReceivingDTO receiptReceivingDTO)
        {
            int count = 1;
            using(var transaction = await _context.Database.BeginTransactionAsync())
            {
                try{
                    this.LoggedInUserId = context.GetLoggedInUserId();
                    var receipt = _mapper.Map<Receipt>(receiptReceivingDTO);
                    var invoice = await _invoiceRepo.FindInvoiceById(receipt.InvoiceId);
                    foreach (var item in invoice.Receipts)
                    {
                        count++;
                    }
                    receipt.TransactionId = invoice.TransactionId;
                    receipt.ReceiptNumber = $"{invoice.InvoiceNumber.Replace("INV", "RCP")}/{count}";
                    receipt.InvoiceValueBalanceAfterReceipting = receipt.InvoiceValue - receipt.InvoiceValueBalanceBeforeReceipting ;
                    var savedReceipt = await _receiptRepo.SaveReceipt(receipt);
                    var receiptTransferDTO = _mapper.Map<ReceiptTransferDTO>(invoice);
                    if(receipt.InvoiceValueBalanceAfterReceipting == 0)
                    {
                        invoice.IsReceiptedStatus = InvoiceStatus.CompletelyReceipted;
                        await _invoiceRepo.UpdateInvoice(invoice);
                    }else if(receipt.InvoiceValueBalanceAfterReceipting > 0 
                                && invoice.IsReceiptedStatus == InvoiceStatus.NotReceipted)
                                {
                                    invoice.IsReceiptedStatus = InvoiceStatus.PartlyReceipted;
                                    await _invoiceRepo.UpdateInvoice(invoice);
                                }
                    
                    await PostAccounts( receipt, invoice, receiptReceivingDTO.AccountId);
                    await transaction.CommitAsync();
                    return new ApiOkResponse(receiptTransferDTO);
                }catch(Exception e)
                {
                    _logger.LogError(e.Message);
                    _logger.LogError(e.StackTrace);
                    await transaction.RollbackAsync();
                    return new ApiResponse(500);
                }
            }
           
        }


        private async Task<bool> PostAccounts( Receipt receipt, Invoice invoice,  long bankAccountId )
        {
            var queryable = _accountRep.GetAccountQueriable();
            var witholdingTaxAccount = await queryable
                .FirstOrDefaultAsync(x => x.Name.ToUpper().Trim() == WITHOLDING_TAX && x.IsDeleted == false);
            
            var receiptVoucherType = await _voucherRepo.GetFinanceVoucherTypeByName(this.RECEIPTVOUCHERTYPE);


            var accountMaster = await CreateAccountMaster(receipt, receiptVoucherType.Id ,invoice);
            
            //Post to bank
            await  PostAccountDetail(invoice, receipt , receiptVoucherType.Id, 
                                       false, accountMaster.Id,  bankAccountId, receipt.ReceiptValue - receipt.ValueOfWHT);
            //Post to Task Witholding
            if(receipt.IsTaskWitheld){
                            await PostAccountDetail(invoice, receipt , receiptVoucherType.Id, 
                                       false, accountMaster.Id,  witholdingTaxAccount.Id, receipt.ValueOfWHT);
            }
            //Post to client account 
            await PostAccountDetail(invoice, receipt , receiptVoucherType.Id, 
                                       false, accountMaster.Id,(long)  invoice.CustomerDivision.AccountId, receipt.ReceiptValue);
            return true;
        }

         private async Task<AccountMaster> CreateAccountMaster(Receipt receipt,
                                                        long accountVoucherTypeId,
                                                        Invoice invoice
                                                        )
        {
            AccountMaster accountMaster = new AccountMaster(){
                Description = $"Receipting for {receipt.InvoiceNumber}",
                IntegrationFlag = false,
                VoucherId = accountVoucherTypeId,
                Value = receipt.ReceiptValue,
                TransactionId = receipt.TransactionId,
                CreatedById = this.LoggedInUserId,
                CustomerDivisionId = invoice.CustomerDivisionId
            };     
            var savedAccountMaster = await _context.AccountMasters.AddAsync(accountMaster);
            await _context.SaveChangesAsync();
            return savedAccountMaster.Entity;
        } 

        private async Task<AccountDetail> PostAccountDetail(
                                                    Invoice invoice,
                                                    Receipt receipt ,  
                                                    long accountVoucherTypeId,
                                                    bool isCredit,
                                                    long accountMasterId,
                                                    long accountId,
                                                    double amount
                                                    )
        {

            AccountDetail accountDetail = new AccountDetail(){
                Description = $"Receipt for invoice: {invoice.InvoiceNumber}  deposited by: {receipt.Depositor}",
                IntegrationFlag = false,
                VoucherId = accountVoucherTypeId,
                TransactionId = invoice.TransactionId,
                TransactionDate = DateTime.Now,
                Credit = isCredit ? amount : 0,
                Debit = !isCredit ? amount : 0,
                AccountId = accountId,
                AccountMasterId = accountMasterId,
                CreatedById = this.LoggedInUserId,
            };

            var savedAccountDetails = await _context.AccountDetails.AddAsync(accountDetail);
            await _context.SaveChangesAsync();
            return savedAccountDetails.Entity;
        }
    }
}