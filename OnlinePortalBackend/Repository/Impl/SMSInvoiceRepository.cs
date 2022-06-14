using Halobiz.Common.DTOs.ReceivingDTOs;
using Halobiz.Common.DTOs.TransferDTOs;
using HalobizMigrations.Data;
using HalobizMigrations.Models;
using HalobizMigrations.Models.OnlinePortal;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using OnlinePortalBackend.Adapters;
using OnlinePortalBackend.DTOs.ReceivingDTOs;
using OnlinePortalBackend.DTOs.TransferDTOs;
using OnlinePortalBackend.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OnlinePortalBackend.Repository.Impl
{
    public class SMSInvoiceRepository : ISMSInvoiceRepository
    {
        private readonly HalobizContext _context;
        private readonly IConfiguration _configuration;
        private readonly ILogger<SMSInvoiceRepository> _logger;
        private readonly IPaymentAdapter _adapter;


        private readonly string WITHOLDING_TAX = "WITHOLDING TAX";
        private readonly string RECEIPTVOUCHERTYPE = "Sales Receipt";
        private readonly string RETAIL = "RETAIL";

        public SMSInvoiceRepository(HalobizContext context, IConfiguration configuration, ILogger<SMSInvoiceRepository> logger, IPaymentAdapter adapter)
        {
            _context = context;
            _configuration = configuration;
            _logger = logger;
            _adapter = adapter;
        }
        public async Task<(bool isSuccess, SMSInvoiceDTO message)> GetInvoice(int profileId)
        {
            var customerDiv = _context.OnlineProfiles.FirstOrDefault(x => x.Id == profileId);
            if (customerDiv == null)
                return (false,null);

            var customerDivId = customerDiv.CustomerDivisionId;

            var invoice = _context.Invoices.FirstOrDefault(x => x.CustomerDivisionId == customerDivId && x.IsReceiptedStatus == (int)InvoiceStatus.NotReceipted);

            if (invoice != null)
            {
                var invoiceDTO = new SMSInvoiceDTO
                {
                    InvoiceId = invoice.Id,
                    InvoiceNumber = invoice.InvoiceNumber,
                    InvoiceValue = invoice.Value
                };
                return (true, invoiceDTO);
            }
            return (false, null);
        }

        public async Task<(bool isSuccess, object message)> ReceiptInvoice(SMSReceiptReceivingDTO request)
        {

            var receiptReceivingDTO = new ReceiptReceivingDTO
            {
                Caption = request.Caption,
                InvoiceId = request.InvoiceId,
                InvoiceNumber = request.InvoiceNumber,
                InvoiceValue = request.InvoiceValue,
                PaymentGateway = request.PaymentGateway,
                PaymentReference = request.PaymentReference,
                DateAndTimeOfFundsReceived = DateTime.UtcNow.AddHours(1),
                ReceiptValue = request.InvoiceValue,
             
            };

           var LoggedInUserId = (long)_context.UserProfiles.FirstOrDefault(x => x.Email.ToLower().Contains("online.portal")).Id;

            var accountId =  _configuration["AccountId"] ?? _configuration.GetSection("AppSettings:AccountId").Value;
            if (accountId != null)
            {
                receiptReceivingDTO.AccountId = long.Parse(accountId);
            }

                // do special receipting for group invoice.            
                var singleInvoice = await FindInvoiceById(receiptReceivingDTO.InvoiceId);

                var invoicesGrouped = await _context.Invoices
                    .Include(x => x.Receipts)
                    .Include(x => x.CustomerDivision)
                    .Where(x => x.GroupInvoiceNumber == singleInvoice.GroupInvoiceNumber
                            && x.StartDate == singleInvoice.StartDate && !x.IsDeleted && x.AdhocGroupingId == request.AdHocIvoiceGroup)
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
                        return (false, "Some system errors occurred");
                    }
                }

                await trx.CommitAsync();
                return (true, "success");
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

        private async Task<AccountMaster> CreateAccountMaster(Receipt receipt,
                                                    long accountVoucherTypeId,
                                                    Invoice invoice,
                                                    long branchId,
                                                    long officeId
                                                    )
        {
            var LoggedInUserId = (long)_context.UserProfiles.FirstOrDefault(x => x.Email.ToLower().Contains("online.portal")).Id;

            AccountMaster accountMaster = new AccountMaster()
            {
                Description = $"Receipting for {receipt.InvoiceNumber}",
                IntegrationFlag = false,
                VoucherId = accountVoucherTypeId,
                Value = receipt.ReceiptValue,
                TransactionId = receipt.TransactionId ?? "No Transaction Id",
                CreatedById = LoggedInUserId,
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
           var LoggedInUserId = (long)_context.UserProfiles.FirstOrDefault(x => x.Email.ToLower().Contains("online.portal")).Id;

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
                CreatedById = LoggedInUserId,
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
            var LoggedInUserId = (long)_context.UserProfiles.FirstOrDefault(x => x.Email.ToLower().Contains("online.portal")).Id;


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
            var LoggedInUserId = (long)_context.UserProfiles.FirstOrDefault(x => x.Email.ToLower().Contains("online.portal")).Id;

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

        private IQueryable<Account> GetAccountQueriable()
        {
            return _context.Accounts.Include(x => x.AccountDetails).AsQueryable();
        }

        private async Task<FinanceVoucherType> GetFinanceVoucherTypeByName(string name)
        {
            return await _context.FinanceVoucherTypes
                .FirstOrDefaultAsync(x => x.IsDeleted == false && x.VoucherType.ToUpper().Trim() == name);
        }

        public async Task<SendReceiptDTO> GetReceiptDetail(string invoiceNumber)
        {
            var invoice = _context.Invoices.FirstOrDefault(x => x.InvoiceNumber == invoiceNumber);
            var customer = _context.CustomerDivisions.FirstOrDefault(x => x.Id == invoice.CustomerDivisionId);
            var contractServices = _context.ContractServices.Include(x => x.Service).Where(x => x.ContractId == invoice.ContractId);

            var receiptDetails = new List<SendReceiptDetailDTO>();

            foreach (var item in contractServices)
            {
                receiptDetails.Add(new SendReceiptDetailDTO
                {
                    Amount = item.UnitPrice.ToString(),
                    Description = item.Service.Description,
                    ServiceName = item.Service.Name,
                    Quantity = item.Quantity.ToString(),
                    Total = (item.Quantity * item.UnitPrice).ToString(),
                });
            }

            var result = new SendReceiptDTO
            {
                Amount = invoice.Value.ToString(),
                CustomerName = customer.DivisionName,
                SendReceiptDetailDTOs = receiptDetails,
                Email = customer.Email
            };

            return result;
        }

        public async Task<(bool isSuccess, string message)> ReceiptAllInvoicesForContract(SMSReceiptInvoiceForContractDTO request)
        {
            var invoices = _context.Invoices.Where(x => x.ContractId == request.ContractId);
            var inv = invoices.OrderByDescending(x => x.Id).FirstOrDefault();

            var validInvoices = invoices.Where(x => x.AdhocGroupingId == inv.AdhocGroupingId);


            var receiptsRequests = new List<SMSReceiptReceivingDTO>();

            try
            {
                foreach (var item in validInvoices)
                {
                    var receiptInv = new SMSReceiptReceivingDTO
                    {
                        Caption = request.Caption,
                        InvoiceId = item.Id,
                        InvoiceNumber = item.InvoiceNumber,
                        InvoiceValue = item.Value,
                        PaymentGateway = request.PaymentGateway,
                        PaymentReference = request.PaymentReference,
                        AdHocIvoiceGroup = item.AdhocGroupingId
                    };

                    receiptsRequests.Add(receiptInv);
                }

                foreach (var item in receiptsRequests)
                {
                    var result = await ReceiptInvoice(item);

                    if (!result.isSuccess)
                    {
                        throw new Exception("Receipting invoice failed");
                    }
                }

                return (true, "success");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                _logger.LogError(ex.StackTrace);
            }

            return (false, "failed");
        }

        public async Task<bool> PostTransactions(PostTransactionDTO request)
        {
            var vat = Convert.ToDouble(request.Value) * 0.075;
            var invoices = _context.Invoices.Where(x => x.ContractId == request.ContractId);
            var inv = invoices.OrderByDescending(x => x.Id).FirstOrDefault();

            var sessionId = string.Empty;
            if (inv != null)
            {
                sessionId = inv.InvoiceNumber + request.ProfileId + inv.StartDate;
                sessionId = sessionId.Replace('/', '0');
                sessionId = sessionId.Replace('-', '0');
            }
            else
            {
                sessionId = new Random().Next(1_000_000_000).ToString() + new Random().Next(1_000_000_000).ToString();
            }
            var paymentConformation = false;

            if (string.IsNullOrEmpty(request.PaymentGatewayResponseCode))
            {

                var result = await _adapter.VerifyPaymentAsync((PaymentGateway)GeneralHelper.GetPaymentGateway(request.PaymentGateway), request.PaymentReferenceGateway);

                if (result != null)
                {
                    if (result.PaymentSuccessful)
                    {
                        paymentConformation = true;
                    }
                }
            }
          

            _context.OnlineTransactions.Add(new OnlineTransaction
            {
                CreatedAt = DateTime.UtcNow.AddHours(1),
                UpdatedAt = DateTime.UtcNow.AddHours(1),
                VAT = Convert.ToDecimal(vat),
                Value = request.Value - Convert.ToDecimal(vat),
                CreatedById = request.ProfileId,
                TransactionType = request.TransactionType,
                PaymentGateway = request?.PaymentGateway,
                PaymentGatewayResponseDescription = request.PaymentGatewayResponseDescription,
                PaymentGatewayResponseCode = request.PaymentGatewayResponseCode,
                PaymentReferenceInternal = request.PaymentReferenceInternal,
                PaymentReferenceGateway = request.PaymentReferenceGateway,
                TotalValue = request.Value,
                SessionId = sessionId,
                ProfileId = request.ProfileId,
                TransactionSource = request.TransactionSource,
                PaymentFulfilment = true,
                PaymentConfirmation = paymentConformation,
                
            });

            try
            {
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                _logger.LogError(ex.StackTrace);
                return false;
            }
        }

        public async Task<SendReceiptDTO> GetContractServiceDetailsForReceipt(long contractId)
        {
            var receiptDetails = _context.ContractServices.Include(x => x.Service).Where(x => x.ContractId == contractId).Select(x => new SendReceiptDetailDTO
            {
                Amount = x.AdHocInvoicedAmount.ToString(),
                Description = x.Service.Description,
                Quantity = x.Quantity.ToString(),
                ServiceName = x.Service.Name,
                Total = x.AdHocInvoicedAmount.ToString()
            }).AsEnumerable();

            var totalSum = receiptDetails.Select(x => double.Parse(x.Amount)).Sum();

            var customerDivId = _context.Contracts.FirstOrDefault(x => x.Id == contractId).CustomerDivisionId;

            var custDiv = _context.CustomerDivisions.FirstOrDefault(x => x.Id == customerDivId);

            var sendReceipt = new SendReceiptDTO
            {
                Amount = totalSum.ToString(),
                Date = DateTime.UtcNow.AddHours(1).ToString("dd/MM/yyyy"),
                Email = custDiv.Email,
                CustomerName = custDiv.DivisionName,
                SendReceiptDetailDTOs = receiptDetails.ToList()
            };

            return sendReceipt;
        }
    }
}
