using HalobizMigrations.Data;
using HalobizMigrations.Models;
using HalobizMigrations.Models.OnlinePortal;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using OnlinePortalBackend.DTOs.ReceivingDTOs;
using System;
using System.Linq;
using System.Threading.Tasks;


namespace OnlinePortalBackend.Repository.Impl
{
    public class WalletRepository : IWalletRepository
    {
        private readonly HalobizContext _context;
        private readonly ILogger<WalletRepository> _logger;
        private readonly IConfiguration _configuration;
        public WalletRepository(HalobizContext context, ILogger<WalletRepository> logger, IConfiguration configuration)
        {
            _context = context;
            _logger = logger;
            _configuration = configuration;
        }
        public async Task<(bool isSuccess, string message)> ActivateWallet(ActivateWalletDTO request)
        {
            var checkExit = _context.WalletMasters.FirstOrDefault(x => x.OnlineProfileId == request.ProfileId);
            if (checkExit != null)
                return (false, "User already has a wallet");

            var profile = _context.OnlineProfiles.FirstOrDefault(x => x.Id == request.ProfileId);
            var createdBy = _context.UserProfiles.FirstOrDefault(x => x.Email.ToLower().Contains("online")).Id;
            var controlAccount = _context.ControlAccounts.FirstOrDefault(x => x.Caption.ToLower().Contains("wallet")).Id;

            using var trx = await _context.Database.BeginTransactionAsync();

            try
            {
                var account = new Account {
                    ControlAccountId = controlAccount,
                    UpdatedAt = DateTime.UtcNow.AddHours(1),
                    CreatedAt = DateTime.UtcNow.AddHours(1),
                    CreatedById = createdBy,
                    IsActive = true,
                    Description = $"Liability Account for {profile.Name.ToUpper()}",
                    Name = profile.Name,
                    IsDebitBalance = true
                };

                await SaveAccount(account);

                var walletUser = new WalletMaster
                {
                    CreatedAt = DateTime.UtcNow.AddHours(1),
                    SecurityAnswer = request.SecurityAnswer,
                    SecurityQuestion = request.SecurityQuestion,
                    UpdatedAt = DateTime.UtcNow.AddHours(1),
                    CreatedById = createdBy,
                    OnlineProfileId = profile.Id,
                    WalletLiabilityAccountId = account.Id,
                    WalletPin = request.WalletPin.ToString()

                };

                _context.WalletMasters.Add(walletUser);
                _context.SaveChanges();

                await trx.CommitAsync();

                return (true, "Success");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                await trx.RollbackAsync();
            }

            return (false, "Activation Failed");
        }


        public async Task<(bool isSuccess, string message)> LoadWallet(LoadWalletDTO request)
        {
            var createdBy = _context.UserProfiles.FirstOrDefault(x => x.Email.ToLower().Contains("online")).Id;
            var walletMaster = _context.WalletMasters.FirstOrDefault(x => x.OnlineProfileId == request.ProfileId);
            if (walletMaster == null)
                return (false, "User does not have a wallet");

            var profile = _context.OnlineProfiles.FirstOrDefault(x => x.Id == request.ProfileId);
            var office = _context.Offices.FirstOrDefault(x => x.Name.ToLower().Contains("office")).Id;
            var branchId = _context.Branches.FirstOrDefault(x => x.Name.ToLower().Contains("head")).Id;
            using var trx = await _context.Database.BeginTransactionAsync();

            var balance = 0d;

            try
            {

                var accountMaster = _context.AccountMasters.FirstOrDefault(x => x.CustomerDivisionId == profile.CustomerDivisionId);
                if (accountMaster == null)
                {
                    var voucher = _context.FinanceVoucherTypes.FirstOrDefault(x => x.VoucherType.ToLower() == "wallet topup").Id;
                    accountMaster = new AccountMaster
                    {
                        CreatedAt = DateTime.UtcNow.AddHours(1),
                        UpdatedAt = DateTime.UtcNow.AddHours(1),
                        CreatedById = createdBy,
                        CustomerDivisionId = profile.CustomerDivisionId,
                        VoucherId = voucher,
                        Value = request.Amount,
                        OfficeId = office,
                        BranchId = branchId,
                    };

                    _context.AccountMasters.Add(accountMaster);
                    _context.SaveChanges();
                }

                var debitCashBook = _configuration["WalletDebitCashBookID"] ?? _configuration.GetSection("AppSettings:WalletDebitCashBookID").Value;

                var accountDetail1 = new AccountDetail
                {
                    VoucherId = accountMaster.VoucherId,
                    AccountMasterId = accountMaster.Id,
                    CreatedById = createdBy,
                    CreatedAt = DateTime.UtcNow.AddHours(1),
                    UpdatedAt = DateTime.UtcNow.AddHours(1),
                    BranchId = branchId,
                    OfficeId = office,
                    TransactionDate = DateTime.UtcNow.AddHours(1),
                    Debit = request.Amount,
                    AccountId = int.Parse(debitCashBook),
                    Description = $"Wallet loaded for {profile.Id}",
                };

                var accountDetail2 = new AccountDetail
                {
                    VoucherId = accountMaster.VoucherId,
                    AccountMasterId = accountMaster.Id,
                    CreatedById = createdBy,
                    CreatedAt = DateTime.UtcNow.AddHours(1),
                    UpdatedAt = DateTime.UtcNow.AddHours(1),
                    BranchId = branchId,
                    OfficeId = office,
                    TransactionDate = DateTime.UtcNow.AddHours(1),
                    Credit = request.Amount,
                    AccountId = walletMaster.WalletLiabilityAccountId.Value,
                    Description = $"Wallet loaded for {profile.Id}",
                };

                _context.AccountDetails.Add(accountDetail1);
                _context.SaveChanges();

                _context.AccountDetails.Add(accountDetail2);
                _context.SaveChanges();

                var debitCashAccount = _context.AccountDetails.Include(x => x.AccountMasterId).FirstOrDefault(x => x.AccountId == int.Parse(debitCashBook));

                var acctToDebit = _context.AccountMasters.FirstOrDefault(x => x.Id == debitCashAccount.AccountMasterId);

                acctToDebit.Value = acctToDebit.Value - request.Amount;
                _context.SaveChanges();

                var transactions = _context.WalletDetails.Where(x => x.WalletMasterId == walletMaster.Id);

                var creditTransactions = transactions.Where(x => x.TransactionType == (int)WalletTransactionType.Load);
                var totalCreditAmt = 0d;
                if (creditTransactions.Count() > 0)
                {
                    totalCreditAmt = creditTransactions.Select(x => int.Parse(x.TransactionValue)).Sum();
                }
                var debitTransactions = transactions.Where(x => x.TransactionType == (int)WalletTransactionType.Spend);
                var totalDebitAmt = 0d;
                if (debitTransactions.Count() > 0)
                {
                    totalDebitAmt = debitTransactions.Select(x => int.Parse(x.TransactionValue)).Sum();
                }

                balance = (totalCreditAmt + request.Amount) - totalDebitAmt;
                
                var walletDetail = new WalletDetail
                {
                    CreatedAt = DateTime.UtcNow.AddHours(1),
                    CreatedById = createdBy,
                    UpdatedAt = DateTime.UtcNow.AddHours(1),
                    WalletMasterId = walletMaster.Id,
                    TransactionValue = request.Amount.ToString(),
                    MovingBalance = balance.ToString(),
                    Platform = request.Platform,
                    TransactionType = (int)WalletTransactionType.Load
                };

                _context.WalletDetails.Add(walletDetail);

                walletMaster.WalletBalance = balance.ToString();

                _context.SaveChanges();

                await trx.CommitAsync();

                return (true, "Success");

            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                await trx.RollbackAsync();
                return (false, "Wallet loading Failed");
            }
            
        }

        public async Task<(bool isSuccess, string message)> SpendWallet(SpendWalletDTO request)
        {
            var createdBy = _context.UserProfiles.FirstOrDefault(x => x.Email.ToLower().Contains("online")).Id;
            var walletMaster = _context.WalletMasters.FirstOrDefault(x => x.OnlineProfileId == request.ProfileId);
            if (walletMaster == null)
                return (false, "User does not have a wallet");

            var profile = _context.OnlineProfiles.FirstOrDefault(x => x.Id == request.ProfileId);
            var office = _context.Offices.FirstOrDefault(x => x.Name.ToLower().Contains("office")).Id;
            var branchId = _context.Branches.FirstOrDefault(x => x.Name.ToLower().Contains("head")).Id;
            using var trx = await _context.Database.BeginTransactionAsync();

            var balance = 0d;

            try
            {

                var accountMaster = _context.AccountMasters.FirstOrDefault(x => x.CustomerDivisionId == profile.CustomerDivisionId);
                if (accountMaster == null)
                {
                    var voucher = _context.FinanceVoucherTypes.FirstOrDefault(x => x.VoucherType.ToLower() == "wallet reduction").Id;
                    accountMaster = new AccountMaster
                    {
                        CreatedAt = DateTime.UtcNow.AddHours(1),
                        UpdatedAt = DateTime.UtcNow.AddHours(1),
                        CreatedById = createdBy,
                        CustomerDivisionId = profile.CustomerDivisionId,
                        VoucherId = voucher,
                        Value = request.Amount,
                        OfficeId = office,
                        BranchId = branchId,
                    };

                    _context.AccountMasters.Add(accountMaster);
                    _context.SaveChanges();
                }

                var creditCashBook = _configuration["WalletCreditCashBookID"] ?? _configuration.GetSection("AppSettings:WalletCreditCashBookID").Value;

                var accountDetail1 = new AccountDetail
                {
                    VoucherId = accountMaster.VoucherId,
                    AccountMasterId = accountMaster.Id,
                    CreatedById = createdBy,
                    CreatedAt = DateTime.UtcNow.AddHours(1),
                    UpdatedAt = DateTime.UtcNow.AddHours(1),
                    BranchId = branchId,
                    OfficeId = office,
                    TransactionDate = DateTime.UtcNow.AddHours(1),
                    Credit = request.Amount,
                    AccountId = int.Parse(creditCashBook),
                    Description = $"Wallet spent for {profile.Id}",
                };

                var accountDetail2 = new AccountDetail
                {
                    VoucherId = accountMaster.VoucherId,
                    AccountMasterId = accountMaster.Id,
                    CreatedById = createdBy,
                    CreatedAt = DateTime.UtcNow.AddHours(1),
                    UpdatedAt = DateTime.UtcNow.AddHours(1),
                    BranchId = branchId,
                    OfficeId = office,
                    TransactionDate = DateTime.UtcNow.AddHours(1),
                    Debit = request.Amount,
                    AccountId = walletMaster.WalletLiabilityAccountId.Value,
                    Description = $"Wallet loaded for {profile.Id}",
                };

                _context.AccountDetails.Add(accountDetail1);
                _context.SaveChanges();

                _context.AccountDetails.Add(accountDetail2);
                _context.SaveChanges();

                var creditCashAccount = _context.AccountDetails.Include(x => x.AccountMasterId).FirstOrDefault(x => x.AccountId == int.Parse(creditCashBook));

                var acctToCredit = _context.AccountMasters.FirstOrDefault(x => x.Id == creditCashAccount.AccountMasterId);

                acctToCredit.Value = acctToCredit.Value + request.Amount;
                _context.SaveChanges();


                var transactions = _context.WalletDetails.Where(x => x.WalletMasterId == walletMaster.Id);
                var creditTransactions = transactions.Where(x => x.TransactionType == (int)WalletTransactionType.Load);
                var totalCreditAmt = 0d;
                if (creditTransactions.Count() > 0)
                {
                    totalCreditAmt = creditTransactions.Select(x => int.Parse(x.TransactionValue)).Sum();
                }
                var debitTransactions = transactions.Where(x => x.TransactionType == (int)WalletTransactionType.Spend);
                var totalDebitAmt = 0d;
                if (debitTransactions.Count() > 0)
                {
                    totalDebitAmt = debitTransactions.Select(x => int.Parse(x.TransactionValue)).Sum();
                }

                balance = totalCreditAmt - (totalDebitAmt + request.Amount);

                var walletDetail = new WalletDetail
                {
                    CreatedAt = DateTime.UtcNow.AddHours(1),
                    CreatedById = createdBy,
                    UpdatedAt = DateTime.UtcNow.AddHours(1),
                    WalletMasterId = walletMaster.Id,
                    TransactionValue = request.Amount.ToString(),
                    MovingBalance = balance.ToString(),
                    TransactionType = (int)WalletTransactionType.Spend
                };

                _context.WalletDetails.Add(walletDetail);

                walletMaster.WalletBalance = balance.ToString();

                _context.SaveChanges();

                await trx.CommitAsync();

                return (true, "Success");

            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                await trx.RollbackAsync();
                return (false, "Wallet loading Failed");
            }
        }

        private async Task<long> SaveAccount(Account account)
        {
            try
            {

                var lastSavedAccount = await _context.Accounts.Where(x => x.ControlAccountId == account.ControlAccountId)
                    .OrderBy(x => x.Id).LastOrDefaultAsync();
                if (lastSavedAccount == null || lastSavedAccount?.AccountNumber < 1000000000)
                {
                    var _controlAccount = await _context.ControlAccounts.Where(x => x.Id == account.ControlAccountId).FirstOrDefaultAsync();

                    account.AccountNumber = _controlAccount.AccountNumber + 1;
                }
                else
                {
                    account.AccountNumber = lastSavedAccount.AccountNumber + 1;
                }

                _context.ChangeTracker.Clear();
                //remove exception throwing
                account.Alias = account.Alias ?? "";

                var savedAccount = await _context.Accounts.AddAsync(account);
                await _context.SaveChangesAsync();
                _context.ChangeTracker.Clear();
                return savedAccount.Entity.Id;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                throw;
            }

        }
    }
}
