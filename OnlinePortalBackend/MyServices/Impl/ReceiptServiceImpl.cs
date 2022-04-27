using AutoMapper;
using Flurl.Http;
using Halobiz.Common.DTOs.ApiDTOs;
using Halobiz.Common.DTOs.ReceivingDTOs;
using HalobizMigrations.Data;
using HalobizMigrations.Models;
using HalobizMigrations.Models.OnlinePortal;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using OnlinePortalBackend.Adapters;
using OnlinePortalBackend.DTOs.TransferDTOs;
using OnlinePortalBackend.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OnlinePortalBackend.MyServices.Impl
{
    public interface IReceiptService
    {
        Task<bool> PostAccounts(long loggedInUserId, Receipt receipt, Invoice invoice, long bankAccountId);
        Task<ApiCommonResponse> AddNewReceipt(ReceiptReceivingDTO receivingDTO);
        Task<ApiCommonResponse> PostPaymentDetails(PaymentDetailsDTO paymentDetails);
    }

    public class ReceiptServiceImpl : IReceiptService
    {
        private readonly HalobizContext _context;
        private readonly IPaymentAdapter _adapter;
        private readonly IConfiguration _configuration;
        private readonly IApiInterceptor _apiInterceptor;
        private readonly ILogger<ReceiptServiceImpl> _logger;
        private readonly IMapper _mapper;

        private readonly string WITHOLDING_TAX = "WITHOLDING TAX";
        private readonly string RECEIPTVOUCHERTYPE = "Sales Receipt";
        private readonly string RETAIL = "RETAIL";

        private string _HalobizBaseUrl;
        private long LoggedInUserId;

        public ReceiptServiceImpl(HalobizContext context, 
            IPaymentAdapter adapter, 
            IConfiguration configuration,
            IApiInterceptor apiInterceptor,
            ILogger<ReceiptServiceImpl> logger,
            IMapper mapper)
        {
            _context = context;
            _adapter = adapter;
            _configuration = configuration;
            _apiInterceptor = apiInterceptor;
            _logger = logger;
            _mapper = mapper;
            _HalobizBaseUrl = _configuration["HalobizBaseUrl"] ?? _configuration.GetSection("AppSettings:HalobizBaseUrl").Value;
        }
        public async Task<bool> PostAccounts(long loggedInUserId, Receipt receipt, Invoice invoice, long bankAccountId)
        {
            LoggedInUserId = loggedInUserId;

            var amount = receipt.ReceiptValue;
            var amountToPost = receipt.ReceiptValue;
            var whtAmount = 0.0;
            if (receipt.IsTaskWitheld)
            {
                var whtPercentage = receipt.ValueOfWht;
                whtAmount = amount * (whtPercentage / 100.0);
                amountToPost = amount - whtAmount;
            }
            var queryable = _context.Accounts.Include(x => x.AccountDetails).AsQueryable();

            var whtControlAccount = await _context.ControlAccounts
                                            .Where(x => x.Caption.ToUpper() == WITHOLDING_TAX && !x.IsDeleted)
                                            .FirstOrDefaultAsync();

            var receiptVoucherType = await _context.FinanceVoucherTypes.FirstOrDefaultAsync(x => x.VoucherType.ToLower() == RECEIPTVOUCHERTYPE.ToLower());

            var branch = await _context.Branches.FirstOrDefaultAsync();
            var office = await _context.Offices.FirstOrDefaultAsync();

            var accountMaster = await CreateAccountMaster(receipt, receiptVoucherType.Id, invoice, branch.Id, office.Id);

            //Post to bank
            await PostAccountDetail(invoice, receipt, receiptVoucherType.Id,
                                       false, accountMaster.Id, bankAccountId, amountToPost, branch.Id, office.Id);
            //Post to Task Witholding
            if (receipt.IsTaskWitheld)
            {

                var retailCustomer = await _context.Customers.FirstOrDefaultAsync(x => x.GroupName == RETAIL);

                long whtAccountId;

                if (invoice.CustomerDivision.CustomerId == retailCustomer.Id)
                {
                    whtAccountId = await GetWHTAccountForRetailClient(whtControlAccount);
                }
                else
                {
                    whtAccountId = await GetWHTAccountForClient(invoice.CustomerDivision, whtControlAccount);
                }

                await PostAccountDetail(invoice, receipt, receiptVoucherType.Id,
                            false, accountMaster.Id, whtAccountId, whtAmount, branch.Id, office.Id);

            }
            //Post to client account 
            await PostAccountDetail(invoice, receipt, receiptVoucherType.Id,
                                       true, accountMaster.Id, (long)invoice.CustomerDivision.ReceivableAccountId, amount, branch.Id, office.Id);
            return true;
        }

        public async Task<ApiCommonResponse> PostPaymentDetails(PaymentDetailsDTO paymentDetails)
        {
            var profileId = _context.OnlineProfiles.FirstOrDefault(x => x.CustomerDivisionId == paymentDetails.userId).Id;
            var inv = _context.Invoices.Include(x => x.ContractService).FirstOrDefault(x => x.InvoiceNumber == paymentDetails.PaymentReferenceInternal);
            _context.OnlineTransactions.Add(new OnlineTransaction
            {
                ConvenienceFee = paymentDetails.ConvenienceFee,
                CreatedAt = DateTime.Now,
                PaymentGatewayResponseCode = paymentDetails.PaymentGatewayResponseCode,
                PaymentGatewayResponseDescription = paymentDetails.PaymentGatewayResponseDescription,
                PaymentConfirmation = paymentDetails.PaymentConfirmation,
                PaymentReferenceGateway = paymentDetails.PaymentReferenceGateway,
                VAT = Convert.ToDecimal(inv.ContractService.Vat.Value) ,
                PaymentGateway = paymentDetails.PaymentGateway,
                Value = paymentDetails.Value,
                TotalValue = paymentDetails.Value + paymentDetails.ConvenienceFee,
                TransactionType = paymentDetails.TransactionType,
                PaymentReferenceInternal = paymentDetails.PaymentReferenceInternal,
                PaymentFulfilment = paymentDetails.PaymentFulfilment,
                TransactionSource = paymentDetails.TransactionSource,
                ProfileId = profileId,
                CreatedById = paymentDetails.CreatedById,
                SessionId = paymentDetails.SessionId,
                UpdatedAt = DateTime.Now,
            });

            try
            {
                _context.SaveChanges();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.StackTrace);
                return CommonResponse.Send(ResponseCodes.FAILURE, null, "Some system errors occurred");
            }
            return CommonResponse.Send(ResponseCodes.SUCCESS);

        }


        private async Task<AccountMaster> CreateAccountMaster(Receipt receipt,
                                                        long accountVoucherTypeId,
                                                        Invoice invoice,
                                                        long branchId,
                                                        long officeId
                                                        )
        {
            AccountMaster accountMaster = new AccountMaster()
            {
                Description = $"Receipting for {receipt.InvoiceNumber}",
                IntegrationFlag = false,
                VoucherId = accountVoucherTypeId,
                Value = receipt.ReceiptValue,
                TransactionId = receipt.TransactionId ?? "No Transaction Id",
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
                                                    Receipt receipt,
                                                    long accountVoucherTypeId,
                                                    bool isCredit,
                                                    long accountMasterId,
                                                    long accountId,
                                                    double amount,
                                                    long branchId,
                                                    long officeId
                                                    )
        {

            AccountDetail accountDetail = new AccountDetail()
            {
                Description = $"Receipt for invoice: {invoice.InvoiceNumber}  deposited by: {receipt.Depositor}",
                IntegrationFlag = false,
                VoucherId = accountVoucherTypeId,
                TransactionId = invoice.TransactionId ?? "No Transaction Id",
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
            try
            {
                await _context.SaveChangesAsync();

            }
            catch (Exception ex)
            {

                throw;
            }
            return savedAccountDetails.Entity;
        }

        private async Task<long> GetWHTAccountForClient(CustomerDivision customerDivision, ControlAccount whtControlAccount)
        {

            string clientWHTAccountName = $"WHT for {customerDivision.DivisionName}";

            Account clientWHTAccount = await _context.Accounts
                .FirstOrDefaultAsync(x => x.ControlAccountId == whtControlAccount.Id && x.Name == clientWHTAccountName);

            long accountId = 0;
            if (clientWHTAccount == null)
            {
                Account account = new Account()
                {
                    Name = clientWHTAccountName,
                    Description = $"WHT Account for {customerDivision.DivisionName}",
                    Alias = customerDivision.DTrackCustomerNumber,
                    IsDebitBalance = true,
                   ControlAccountId = whtControlAccount.Id,
                    CreatedById = LoggedInUserId,
                };
                try
                {
                    var savedAccount = await SaveAccount(account);
                    accountId = savedAccount.Id;
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                
                await _context.SaveChangesAsync();
            }
            else
            {
                accountId = clientWHTAccount.Id;
            }

            return accountId;
        }

        private async Task<long> GetWHTAccountForRetailClient(ControlAccount whtControlAccount)
        {

            string clientWHTAccountName = $"WHT for {RETAIL}";

            Account clientWHTAccount = await _context.Accounts
                .FirstOrDefaultAsync(x => x.ControlAccountId == whtControlAccount.Id && x.Name == clientWHTAccountName);

            long accountId = 0;
            if (clientWHTAccount == null)
            {
                Account account = new Account()
                {
                    Name = clientWHTAccountName,
                    Description = $"WHT Account for {RETAIL}",
                    Alias = "HA_RET",
                    IsDebitBalance = true,
                    ControlAccountId = whtControlAccount.Id,
                    CreatedById = LoggedInUserId
                };
                var savedAccount = await SaveAccount(account);
                accountId = savedAccount.Id;
                await _context.SaveChangesAsync();
            }
            else
            {
                accountId = clientWHTAccount.Id;
            }

            return accountId;
        }

        private async Task<Account> SaveAccount(Account account)
        {
            try
            {
              //  await _context.Database.ExecuteSqlRawAsync("SET IDENTITY_INSERT dbo.Accounts ON");

                var lastSavedAccount = await _context.Accounts.Where(x => x.ControlAccountId == account.ControlAccountId)
                    .OrderBy(x => x.Id).LastOrDefaultAsync();
                if (lastSavedAccount == null || lastSavedAccount.Id < 1000000000)
                {
                    account.ControlAccountId = (long)account.ControlAccountId + 1;
                }
                else
                {
                    account.ControlAccountId = lastSavedAccount.Id + 1;
                }
                var savedAccount = await _context.Accounts.AddAsync(account);
                await _context.SaveChangesAsync();
                return savedAccount.Entity;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                await _context.Database.ExecuteSqlRawAsync("SET IDENTITY_INSERT dbo.Accounts OFF");
            }
        }

   
        public async Task<ApiCommonResponse> AddNewReceipt(ReceiptReceivingDTO receiptReceivingDTO)
        {
            var result = await _adapter.VerifyPaymentAsync((PaymentGateway)receiptReceivingDTO.PaymentGateway, receiptReceivingDTO.PaymentReference);
            if (result == null)
                return CommonResponse.Send(ResponseCodes.FAILURE, "failed");
            if (!result.PaymentSuccessful)
                return CommonResponse.Send(ResponseCodes.FAILURE, "failed");


            LoggedInUserId = (long)_context.UserProfiles.FirstOrDefault(x => x.Email.ToLower().Contains("online.portal")).Id;

            var accountId = _configuration["AccountId"] ?? _configuration.GetSection("AppSettings:AccountId").Value;
            if (accountId != null)
            {
                receiptReceivingDTO.AccountId = long.Parse(accountId);
            }

            if (receiptReceivingDTO.InvoiceNumber.ToUpper().Contains("GINV"))
            {
                // do special receipting for group invoice.            
                var singleInvoice = await FindInvoiceById(receiptReceivingDTO.InvoiceId);

                var invoicesGrouped = await _context.Invoices
                    .Include(x => x.Receipts)
                    .Include(x => x.CustomerDivision)
                    .Where(x => x.GroupInvoiceNumber == singleInvoice.GroupInvoiceNumber
                            && x.StartDate == singleInvoice.StartDate && !x.IsDeleted)
                    .ToListAsync();

                var totalReceiptAmount = receiptReceivingDTO.ReceiptValue;

                using var trx = await _context.Database.BeginTransactionAsync();
                foreach (var invoice in invoicesGrouped)
                {
                    if (invoice.IsReceiptedStatus == (int)InvoiceStatus.CompletelyReceipted) continue;

                    try
                    {
                        var totalAmoutReceipted = invoice.Receipts.Sum(x => x.ReceiptValue);
                        var invoiceValueBeforeReceipting = invoice.Value - totalAmoutReceipted;

                        // var receipt = _mapper.Map<Receipt>(receiptReceivingDTO);
                        var receipt = new Receipt
                        {
                            CreatedAt = DateTime.Now,
                            DateAndTimeOfFundsReceived = DateTime.Now,
                            InvoiceValueBalanceAfterReceipting = receiptReceivingDTO.InvoiceValueBalanceAfterReceipting,
                            UpdatedAt = DateTime.Now,
                            Caption = receiptReceivingDTO.Caption,
                            CreatedById = LoggedInUserId,
                            EvidenceOfPaymentUrl = receiptReceivingDTO.EvidenceOfPaymentUrl,
                            InvoiceId = receiptReceivingDTO.InvoiceId,
                            ValueOfWht = receiptReceivingDTO.ValueOfWHT,
                            ReceiptValue = receiptReceivingDTO.ReceiptValue,
                            InvoiceNumber = receiptReceivingDTO.InvoiceNumber,
                            InvoiceValueBalanceBeforeReceipting = receiptReceivingDTO.InvoiceValueBalanceBeforeReceipting,
                            InvoiceValue = receiptReceivingDTO.InvoiceValue,
                            Depositor = receiptReceivingDTO.Depositor,
                            IsTaskWitheld = receiptReceivingDTO.IsTaskWitheld
                        };

                        receipt.InvoiceId = invoice.Id;
                        receipt.TransactionId = invoice.TransactionId;
                        receipt.ReceiptNumber = $"{invoice.InvoiceNumber.Replace("INV", "RCP")}/{invoice.Receipts.Count + 1}";
                        receipt.InvoiceValueBalanceBeforeReceipting = invoiceValueBeforeReceipting;
                        receipt.CreatedById = LoggedInUserId;

                        if (totalReceiptAmount < invoiceValueBeforeReceipting)
                        {
                            receipt.InvoiceValueBalanceAfterReceipting = receipt.InvoiceValueBalanceBeforeReceipting - totalReceiptAmount;
                            receipt.ReceiptValue = totalReceiptAmount;
                            var savedReceipt = await SaveReceipt(receipt);
                            invoice.IsReceiptedStatus = (int)InvoiceStatus.PartlyReceipted;
                            await UpdateInvoice(invoice);
                            await PostAccounts(receipt, invoice, receiptReceivingDTO.AccountId);
                            break;
                        }
                        else if (totalReceiptAmount == invoiceValueBeforeReceipting)
                        {
                            receipt.InvoiceValueBalanceAfterReceipting = 0;
                            receipt.ReceiptValue = invoiceValueBeforeReceipting;
                            var savedReceipt = await SaveReceipt(receipt);
                            invoice.IsReceiptedStatus = (int)InvoiceStatus.CompletelyReceipted;
                            await UpdateInvoice(invoice);
                            await PostAccounts(receipt, invoice, receiptReceivingDTO.AccountId);
                            break;
                        }
                        else
                        {
                            receipt.InvoiceValueBalanceAfterReceipting = 0;
                            receipt.ReceiptValue = invoiceValueBeforeReceipting;
                            var savedReceipt = await SaveReceipt(receipt);
                            invoice.IsReceiptedStatus = (int)InvoiceStatus.CompletelyReceipted;
                            await UpdateInvoice(invoice);
                            totalReceiptAmount -= invoiceValueBeforeReceipting;
                            await PostAccounts(receipt, invoice, receiptReceivingDTO.AccountId);
                        }
                    }
                    catch (Exception ex)
                    {
                        await trx.RollbackAsync();
                        _logger.LogError(ex.Message);
                        _logger.LogError(ex.StackTrace);
                        return CommonResponse.Send(ResponseCodes.FAILURE, null, "Some system errors occurred");
                    }
                }

                await trx.CommitAsync();
                return CommonResponse.Send(ResponseCodes.SUCCESS);
            }
            else
            {
                int count = 1;
                using (var transaction = await _context.Database.BeginTransactionAsync())
                {
                    try
                    {
                        // var receipt = _mapper.Map<Receipt>(receiptReceivingDTO);
                        var receipt = new Receipt
                        {
                            CreatedAt = DateTime.Now,
                            DateAndTimeOfFundsReceived = DateTime.Now,
                            InvoiceValueBalanceAfterReceipting = receiptReceivingDTO.InvoiceValueBalanceAfterReceipting,
                            UpdatedAt = DateTime.Now,
                            Caption = receiptReceivingDTO.Caption,
                            CreatedById = LoggedInUserId,
                            EvidenceOfPaymentUrl = receiptReceivingDTO.EvidenceOfPaymentUrl,
                            InvoiceId = receiptReceivingDTO.InvoiceId,
                            ValueOfWht = receiptReceivingDTO.ValueOfWHT,
                            ReceiptValue = receiptReceivingDTO.ReceiptValue,
                            InvoiceNumber = receiptReceivingDTO.InvoiceNumber,
                            InvoiceValueBalanceBeforeReceipting = receiptReceivingDTO.InvoiceValueBalanceBeforeReceipting,
                            InvoiceValue = receiptReceivingDTO.InvoiceValue,
                            Depositor = receiptReceivingDTO.Depositor,
                            IsTaskWitheld = receiptReceivingDTO.IsTaskWitheld
                        };

                       
                        var invoice = await FindInvoiceById(receipt.InvoiceId);
                        foreach (var item in invoice.Receipts)
                        {
                            count++;
                        }
                        receipt.TransactionId = invoice.TransactionId;
                        receipt.ReceiptNumber = $"{invoice.InvoiceNumber.Replace("INV", "RCP")}/{count}";
                        receipt.InvoiceValueBalanceAfterReceipting = receipt.InvoiceValueBalanceBeforeReceipting - receipt.ReceiptValue;
                        receipt.CreatedById = LoggedInUserId;
                        var savedReceipt = await SaveReceipt(receipt);
                        var receiptTransferDTO = _mapper.Map<ReceiptTransferDTO>(savedReceipt);

                        if (receipt.InvoiceValueBalanceAfterReceipting == 0)
                        {
                            invoice.IsReceiptedStatus = (int)InvoiceStatus.CompletelyReceipted;
                            await UpdateInvoice(invoice);
                        }
                        else if (receipt.InvoiceValueBalanceAfterReceipting > 0)
                        {
                            invoice.IsReceiptedStatus = (int)InvoiceStatus.PartlyReceipted;
                            await UpdateInvoice(invoice);
                        }

                        await PostAccounts(receipt, invoice, receiptReceivingDTO.AccountId);
                        await transaction.CommitAsync();
                        return CommonResponse.Send(ResponseCodes.SUCCESS, receiptTransferDTO);
                    }
                    catch (Exception e)
                    {
                        _logger.LogError(e.Message);
                        _logger.LogError(e.StackTrace);
                        await transaction.RollbackAsync();
                        return CommonResponse.Send(ResponseCodes.FAILURE, null, "Some system errors occurred");
                    }
                }
            }
        }

        private async Task<Invoice> FindInvoiceById(long Id)
        {
            return await _context.Invoices
            .Include(x => x.CustomerDivision)
             .Include(x => x.Receipts)
             .FirstOrDefaultAsync(x => x.Id == Id && x.IsDeleted == false);
        }

        private async Task<Receipt> SaveReceipt(Receipt receipt)
        {
            var receiptEntity = await _context.Receipts.AddAsync(receipt);
            if (await SaveChanges())
            {
                return receiptEntity.Entity;
            }
            return null;
        }

        private async Task<Invoice> UpdateInvoice(Invoice invoice)
        {
            var invoiceEntity = _context.Invoices.Update(invoice);
            if (await SaveChanges())
            {
                return invoiceEntity.Entity;
            }
            return null;
        }

        private async Task<bool> SaveChanges()
        {
            try
            {
                return await _context.SaveChangesAsync() > 0;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return false;
            }
        }

        private async Task<bool> PostAccounts(Receipt receipt, Invoice invoice, long bankAccountId)
        {
            var amount = receipt.ReceiptValue;
            var amountToPost = receipt.ReceiptValue;
            var whtAmount = 0.0;
            if (receipt.IsTaskWitheld)
            {
                var whtPercentage = receipt.ValueOfWht;
                whtAmount = amount * (whtPercentage / 100.0);
                amountToPost = amount - whtAmount;
            }
            var queryable = GetAccountQueriable();

            /*var witholdingTaxAccount = await queryable
                .FirstOrDefaultAsync(x => x.Name.ToUpper().Trim() == WITHOLDING_TAX && x.IsDeleted == false);*/

            var whtControlAccount = await _context.ControlAccounts
                                            .Where(x => x.Caption.ToUpper() == WITHOLDING_TAX && !x.IsDeleted)
                                            .FirstOrDefaultAsync();

            var receiptVoucherType = await GetFinanceVoucherTypeByName(this.RECEIPTVOUCHERTYPE);

            var branch = await _context.Branches.FirstOrDefaultAsync();
            var office = await _context.Offices.FirstOrDefaultAsync();


            var accountMaster = await CreateAccountMaster(receipt, receiptVoucherType.Id, invoice, branch.Id, office.Id);

            //Post to bank
            await PostAccountDetail(invoice, receipt, receiptVoucherType.Id,
                                       false, accountMaster.Id, bankAccountId, amountToPost, branch.Id, office.Id);
            //Post to Task Witholding
            if (receipt.IsTaskWitheld)
            {

                var retailCustomer = await _context.Customers.FirstOrDefaultAsync(x => x.GroupName == RETAIL);

                long whtAccountId;

                if (invoice.CustomerDivision.CustomerId == retailCustomer.Id)
                {
                    whtAccountId = await GetWHTAccountForRetailClient(whtControlAccount);
                }
                else
                {
                    whtAccountId = await GetWHTAccountForClient(invoice.CustomerDivision, whtControlAccount);
                }

                await PostAccountDetail(invoice, receipt, receiptVoucherType.Id,
                            false, accountMaster.Id, whtAccountId, whtAmount, branch.Id, office.Id);

            }
            var accountIdStr = _configuration["AccountId"] ?? _configuration.GetSection("AppSettings:AccountId").Value;
            long accountIDNum = 0;
            if (accountIdStr != null)
            {
                accountIDNum = long.Parse(accountIdStr);
            }
            var accountId = invoice?.CustomerDivision?.ReceivableAccountId ?? accountIDNum;
            //Post to client account 
            await PostAccountDetail(invoice, receipt, receiptVoucherType.Id,
                                       true, accountMaster.Id, (long)accountId, amount, branch.Id, office.Id);
            return true;
        }

        private IQueryable<Account> GetAccountQueriable()
        {
            return _context.Accounts.Include(x => x.AccountDetails).AsQueryable();
        }

        private async Task<FinanceVoucherType> GetFinanceVoucherTypeByName(string name)
        {
            return await _context.FinanceVoucherTypes
                .FirstOrDefaultAsync(x => x.IsDeleted == false && x.VoucherType.ToUpper().Trim() == name);
        }

       
    }
}
