using HalobizMigrations.Data;
using HalobizMigrations.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using OnlinePortalBackend.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OnlinePortalBackend.MyServices.Impl
{
    public class ReceiptServiceImpl : IReceiptService
    {
        private readonly HalobizContext _context;

        private readonly string WITHOLDING_TAX = "WITHOLDING TAX";
        private readonly string RECEIPTVOUCHERTYPE = "Sales Receipt";
        private readonly string RETAIL = "RETAIL";

        private long LoggedInUserId;

        public ReceiptServiceImpl(HalobizContext context)
        {
            _context = context;
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
            await _context.SaveChangesAsync();
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
                await _context.Database.ExecuteSqlRawAsync("SET IDENTITY_INSERT dbo.Accounts ON");

                var lastSavedAccount = await _context.Accounts.Where(x => x.ControlAccountId == account.ControlAccountId)
                    .OrderBy(x => x.Id).LastOrDefaultAsync();
                if (lastSavedAccount == null || lastSavedAccount.Id < 1000000000)
                {
                    account.Id = (long)account.ControlAccountId + 1;
                }
                else
                {
                    account.Id = lastSavedAccount.Id + 1;
                }
                var savedAccount = await _context.Accounts.AddAsync(account);
                await _context.SaveChangesAsync();
                return savedAccount.Entity;
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                await _context.Database.ExecuteSqlRawAsync("SET IDENTITY_INSERT dbo.Accounts OFF");
            }
        }
    }
}
