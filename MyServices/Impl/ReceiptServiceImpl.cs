using System;
using System.Threading.Tasks;
using AutoMapper;
using HalobizMigrations.Data;
using HaloBiz.DTOs.ApiDTOs;
using HaloBiz.DTOs.ReceivingDTOs;
using HaloBiz.DTOs.TransferDTOs;
using HaloBiz.Helpers;
using HalobizMigrations.Models;
using HaloBiz.Repository;
using HaloBiz.Repository.LAMS;
using halobiz_backend.Helpers;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Linq;
using System.Collections.Generic;

namespace HaloBiz.MyServices.Impl
{
    public class ReceiptServiceImpl : IReceiptService
    {
        private readonly ILogger<ReceiptServiceImpl> _logger;
        private readonly IReceiptRepository _receiptRepo;
        private readonly IModificationHistoryRepository _modificationRepo;
        private readonly IMapper _mapper;
        private readonly HalobizContext _context;
        private readonly IInvoiceRepository _invoiceRepo;
        private readonly IAccountRepository _accountRep;
        private readonly IFinancialVoucherTypeRepository _voucherRepo;
        private readonly string WITHOLDING_TAX = "WITHOLDING TAX";
        private readonly string RECEIPTVOUCHERTYPE = "Sales Receipt";
        private long LoggedInUserId;

        public ReceiptServiceImpl(ILogger<ReceiptServiceImpl> logger, IReceiptRepository receiptRepo,
                     IModificationHistoryRepository modificationRepo, IMapper mapper, HalobizContext context,
                        IInvoiceRepository invoiceRepo, IAccountRepository accountRep, IFinancialVoucherTypeRepository voucherRepo)
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

        public async Task<ApiResponse> AddReceipt(HttpContext context, ReceiptReceivingDTO receiptReceivingDTO)
        {
            LoggedInUserId = context.GetLoggedInUserId();
            if (receiptReceivingDTO.InvoiceNumber.ToUpper().Contains("GINV"))
            {
                // do special receipting for group invoice.            
                var singleInvoice = await _invoiceRepo.FindInvoiceById(receiptReceivingDTO.InvoiceId);

                var invoicesGrouped = await _context.Invoices
                    .Include(x => x.Receipts)
                    .Include(x => x.CustomerDivision)
                    .Where(x => x.GroupInvoiceNumber == singleInvoice.GroupInvoiceNumber 
                            && x.StartDate == singleInvoice.StartDate && !x.IsDeleted)
                    .ToListAsync();

                var totalReceiptAmount = receiptReceivingDTO.ReceiptValue;
                foreach (var invoice in invoicesGrouped)
                {
                    if (invoice.IsReceiptedStatus == (int)InvoiceStatus.CompletelyReceipted) continue;
                    using var trx = await _context.Database.BeginTransactionAsync();
                    try
                    {
                        var totalAmoutReceipted = invoice.Receipts.Sum(x => x.ReceiptValue);
                        var invoiceValueBeforeReceipting = invoice.Value - totalAmoutReceipted;

                        var receipt = _mapper.Map<Receipt>(receiptReceivingDTO);
                        receipt.InvoiceId = invoice.Id;
                        receipt.TransactionId = invoice.TransactionId;
                        receipt.ReceiptNumber = $"{invoice.InvoiceNumber.Replace("INV", "RCP")}/{invoice.Receipts.Count + 1}";
                        receipt.InvoiceValueBalanceBeforeReceipting = invoiceValueBeforeReceipting;
                        receipt.CreatedById = context.GetLoggedInUserId();

                        if (totalReceiptAmount < invoiceValueBeforeReceipting)
                        {
                            receipt.InvoiceValueBalanceAfterReceipting = receipt.InvoiceValueBalanceBeforeReceipting - totalReceiptAmount;
                            receipt.ReceiptValue = totalReceiptAmount;
                            var savedReceipt = await _receiptRepo.SaveReceipt(receipt);
                            invoice.IsReceiptedStatus = (int)InvoiceStatus.PartlyReceipted;
                            await _invoiceRepo.UpdateInvoice(invoice);
                            await PostAccounts(receipt, invoice, receiptReceivingDTO.AccountId);
                            await trx.CommitAsync();
                            break;
                        }
                        else if (totalReceiptAmount == invoiceValueBeforeReceipting)
                        {
                            receipt.InvoiceValueBalanceAfterReceipting = 0;
                            receipt.ReceiptValue = invoiceValueBeforeReceipting;
                            var savedReceipt = await _receiptRepo.SaveReceipt(receipt);
                            invoice.IsReceiptedStatus = (int)InvoiceStatus.CompletelyReceipted;
                            await _invoiceRepo.UpdateInvoice(invoice);
                            await PostAccounts(receipt, invoice, receiptReceivingDTO.AccountId);
                            await trx.CommitAsync();
                            break;
                        }
                        else
                        {
                            receipt.InvoiceValueBalanceAfterReceipting = 0;
                            receipt.ReceiptValue = invoiceValueBeforeReceipting;
                            var savedReceipt = await _receiptRepo.SaveReceipt(receipt);
                            invoice.IsReceiptedStatus = (int)InvoiceStatus.CompletelyReceipted;
                            await _invoiceRepo.UpdateInvoice(invoice);
                            totalReceiptAmount -= invoiceValueBeforeReceipting;
                            await PostAccounts(receipt, invoice, receiptReceivingDTO.AccountId);
                            await trx.CommitAsync();
                        }                    
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex.Message);
                        _logger.LogError(ex.StackTrace);
                        await trx.RollbackAsync();
                        return new ApiResponse(500);
                    }                                
                }

                return new ApiOkResponse(true);
            }
            else
            {
                int count = 1;
                using (var transaction = await _context.Database.BeginTransactionAsync())
                {
                    try
                    {
                        this.LoggedInUserId = context.GetLoggedInUserId();
                        var receipt = _mapper.Map<Receipt>(receiptReceivingDTO);
                        var invoice = await _invoiceRepo.FindInvoiceById(receipt.InvoiceId);
                        foreach (var item in invoice.Receipts)
                        {
                            count++;
                        }
                        receipt.TransactionId = invoice.TransactionId;
                        receipt.ReceiptNumber = $"{invoice.InvoiceNumber.Replace("INV", "RCP")}/{count}";
                        receipt.InvoiceValueBalanceAfterReceipting = receipt.InvoiceValueBalanceBeforeReceipting - receipt.ReceiptValue;
                        receipt.CreatedById = context.GetLoggedInUserId();
                        var savedReceipt = await _receiptRepo.SaveReceipt(receipt);
                        var receiptTransferDTO = _mapper.Map<ReceiptTransferDTO>(savedReceipt);

                        if (receipt.InvoiceValueBalanceAfterReceipting == 0)
                        {
                            invoice.IsReceiptedStatus = (int)InvoiceStatus.CompletelyReceipted;
                            await _invoiceRepo.UpdateInvoice(invoice);
                        }
                        else if (receipt.InvoiceValueBalanceAfterReceipting > 0)
                        {
                            invoice.IsReceiptedStatus = (int)InvoiceStatus.PartlyReceipted;
                            await _invoiceRepo.UpdateInvoice(invoice);
                        }

                        await PostAccounts(receipt, invoice, receiptReceivingDTO.AccountId);
                        await transaction.CommitAsync();
                        return new ApiOkResponse(receiptTransferDTO);
                    }
                    catch (Exception e)
                    {
                        _logger.LogError(e.Message);
                        _logger.LogError(e.StackTrace);
                        await transaction.RollbackAsync();
                        return new ApiResponse(500);
                    }
                }
            }
        }

        public async Task<ApiResponse> GetReceiptBreakDown(long invoiceId, double totalReceiptAmount)
        {
            var singleInvoice = await _invoiceRepo.FindInvoiceById(invoiceId);

            var invoicesGrouped = await _context.Invoices.AsNoTracking()
                    .Include(x => x.Receipts)
                    .Where(x => x.GroupInvoiceNumber == singleInvoice.GroupInvoiceNumber
                            && x.StartDate == singleInvoice.StartDate && !x.IsDeleted)
                    .ToListAsync();

            var invoiceTransferDTOS = _mapper.Map<List<InvoiceTransferDTO>>(invoicesGrouped);

            foreach (var invoice in invoiceTransferDTOS)
            {
                if (invoice.IsReceiptedStatus == InvoiceStatus.CompletelyReceipted) 
                {
                    invoice.ToReceiptValue = 0;
                    continue; 
                }

                try
                {
                    var totalAmoutReceipted = invoice.Receipts.Sum(x => x.ReceiptValue);
                    var invoiceValueBeforeReceipting = invoice.Value - totalAmoutReceipted;

                    if (totalReceiptAmount < invoiceValueBeforeReceipting)
                    {
                        invoice.IsReceiptedStatus = InvoiceStatus.PartlyReceipted;
                        invoice.ToReceiptValue = totalReceiptAmount;
                        break;
                    }
                    else if (totalReceiptAmount == invoiceValueBeforeReceipting)
                    {
                        invoice.IsReceiptedStatus = InvoiceStatus.CompletelyReceipted;
                        invoice.ToReceiptValue = totalReceiptAmount;
                        break;
                    }
                    else
                    {
                        invoice.IsReceiptedStatus = InvoiceStatus.CompletelyReceipted;
                        invoice.ToReceiptValue = invoiceValueBeforeReceipting;
                        totalReceiptAmount -= invoiceValueBeforeReceipting;
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex.Message);
                    _logger.LogError(ex.StackTrace);
                    return new ApiResponse(500);
                }
            }
            
            return new ApiOkResponse(invoiceTransferDTOS);
        }

        private async Task<bool> PostAccounts( Receipt receipt, Invoice invoice,  long bankAccountId )
        {
            var amount = receipt.ReceiptValue;
            var amountToPost = receipt.ReceiptValue;
            var whtAmount = 0.0;
            if(receipt.IsTaskWitheld)
            {
                var whtPercentage = receipt.ValueOfWht;
                whtAmount = amount * (whtPercentage / 100.0);
                amountToPost = amount - whtAmount;
            }
            var queryable = _accountRep.GetAccountQueriable();
            var witholdingTaxAccount = await queryable
                .FirstOrDefaultAsync(x => x.Name.ToUpper().Trim() == WITHOLDING_TAX && x.IsDeleted == false);
            
            var receiptVoucherType = await _voucherRepo.GetFinanceVoucherTypeByName(this.RECEIPTVOUCHERTYPE);

            var branch = await _context.Branches.FirstOrDefaultAsync();
            var office = await _context.Offices.FirstOrDefaultAsync();


            var accountMaster = await CreateAccountMaster(receipt, receiptVoucherType.Id ,invoice, branch.Id, office.Id);
            
            //Post to bank
            await  PostAccountDetail(invoice, receipt , receiptVoucherType.Id, 
                                       false, accountMaster.Id,  bankAccountId, amountToPost, branch.Id, office.Id);
            //Post to Task Witholding
            if(receipt.IsTaskWitheld){
                            await PostAccountDetail(invoice, receipt , receiptVoucherType.Id, 
                                       false, accountMaster.Id,  witholdingTaxAccount.Id, whtAmount, branch.Id, office.Id);
            }
            //Post to client account 
            await PostAccountDetail(invoice, receipt , receiptVoucherType.Id, 
                                       true, accountMaster.Id,(long)  invoice.CustomerDivision.ReceivableAccountId, amount, branch.Id, office.Id);
            return true;
        }

        private async Task<AccountMaster> CreateAccountMaster(Receipt receipt,
                                                        long accountVoucherTypeId,
                                                        Invoice invoice,
                                                        long branchId,
                                                        long officeId
                                                        )
        {
            AccountMaster accountMaster = new AccountMaster(){
                Description = $"Receipting for {receipt.InvoiceNumber}",
                IntegrationFlag = false,
                VoucherId = accountVoucherTypeId,
                Value = receipt.ReceiptValue,
                TransactionId = receipt.TransactionId?? "No Transaction Id",
                CreatedById = this.LoggedInUserId,
                CustomerDivisionId = invoice.CustomerDivisionId,
                BranchId = branchId,
                OfficeId = officeId
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
                                                    double amount,
                                                    long branchId,
                                                    long officeId
                                                    )
        {

            AccountDetail accountDetail = new AccountDetail(){
                Description = $"Receipt for invoice: {invoice.InvoiceNumber}  deposited by: {receipt.Depositor}",
                IntegrationFlag = false,
                VoucherId = accountVoucherTypeId,
                TransactionId = invoice.TransactionId?? "No Transaction Id",
                TransactionDate = DateTime.Now,
                Credit = isCredit ? amount : 0,
                Debit = !isCredit ? amount : 0,
                AccountId = accountId,
                AccountMasterId = accountMasterId,
                CreatedById = this.LoggedInUserId,
                BranchId = branchId,
                OfficeId = officeId
                
            };

            var savedAccountDetails = await _context.AccountDetails.AddAsync(accountDetail);
            await _context.SaveChangesAsync();
            return savedAccountDetails.Entity;
        }
    }
}