using Halobiz.Common.DTOs.ReceivingDTOs;
using Halobiz.Common.Utility;
using HalobizMigrations.Data;
using HalobizMigrations.Models;
using HalobizMigrations.Models.OnlinePortal;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using OnlinePortalBackend.DTOs.ReceivingDTOs;
using OnlinePortalBackend.DTOs.TransferDTOs;
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
            var controlAccount = _configuration["WalletControlAccountID"] ?? _configuration.GetSection("AppSettings:WalletControlAccountID").Value;
            var controlAccountId = int.Parse(controlAccount);

            using var trx = await _context.Database.BeginTransactionAsync();
            string initials = GetInitials(profile.Name);
            var acctTrx = _context.Accounts.LastOrDefault(x => x.Alias.StartsWith("W"));
            long acctTrxId = 0;
            if (acctTrx == null)
                acctTrxId = 1;
            else
                acctTrxId = acctTrx.Id + 1;

            try
            {
                var account = new Account {
                    ControlAccountId = controlAccountId,
                    UpdatedAt = DateTime.UtcNow.AddHours(1),
                    CreatedAt = DateTime.UtcNow.AddHours(1),
                    CreatedById = createdBy,
                    IsActive = true,
                    Description = $"Liability Account for {profile.Name.ToUpper()}",
                    Name = profile.Name,
                    IsDebitBalance = false,
                    Alias = "W_" + $"{initials}_" +  $"0{acctTrxId}"
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
                    WalletPin = new AesCryptoUtil().Encrypt(request.WalletPin.ToString()),
                    WalletBalance = new AesCryptoUtil().Encrypt(0.ToString()),
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

        public async Task<(bool isSuccess, bool status)> GetWalletActivationStatus(int profileId)
        {
            var profile = _context.WalletMasters.FirstOrDefault(x => x.OnlineProfileId == profileId);
            if (profile == null)
                return (true, false);

            return(true, true);
        }

        public async Task<(bool isSuccess, object message)> GetWalletBalance(int profileId)
        {
           var balance = _context.WalletMasters.FirstOrDefault(x => x.OnlineProfileId == profileId);
            if (balance == null)
            {
                return (false, "User does not exist");
            }

            return (true, new AesCryptoUtil().Decrypt(balance.WalletBalance));

        }

        public async Task<WalletTransactionHistoryDTO> GetWalletTransactionHistory(int propfileId)
        {
            var walletTrxActivity = _context.OnlineTransactions.Where(x => x.ProfileId == propfileId && x.TransactionType.ToLower().Contains("wallet")).Select(x => new WalletTransactionActivity
            {
                Amount = x.TotalValue,
                Description = x.TransactionType,
                Platform = "Secure Mobility",
                Status = String.IsNullOrEmpty(x.PaymentReferenceInternal) ? "Success" : "Fail",
                TransactionDate = DateTime.UtcNow.AddHours(1)
            });

            if (walletTrxActivity.Count() < 1)
            {
                return null;
            }

            var trxHistory = new WalletTransactionHistoryDTO
            {
                ProfileId = propfileId,
                TotalSpend = walletTrxActivity.Where(x => x.Description.ToLower().Contains("spend")).Select(x => x.Amount).Sum(),
                TotalTopup = walletTrxActivity.Where(x => x.Description.ToLower().Contains("topup")).Select(x => x.Amount).Sum(),
                Balance = walletTrxActivity.Where(x => x.Description.ToLower().Contains("topup")).Select(x => x.Amount).Sum() - walletTrxActivity.Where(x => x.Description.ToLower().Contains("spend")).Select(x => x.Amount).Sum(),
                TransactionHistory = walletTrxActivity
            };

            return trxHistory;
        }

        public async Task<(bool isSuccess, string message)> LoadWallet(LoadWalletDTO request)
        {
            var createdBy = _context.UserProfiles.FirstOrDefault(x => x.Email.ToLower().Contains("online")).Id;
            var walletMaster = _context.WalletMasters.FirstOrDefault(x => x.OnlineProfileId == request.ProfileId);
            if (walletMaster == null)
                return (false, "User does not have a wallet");

            var profile = _context.OnlineProfiles.FirstOrDefault(x => x.Id == request.ProfileId);
            var office = _context.Offices.FirstOrDefault(x => x.Name.ToLower().Contains("office")).Id;
            var branchId = _context.Branches.FirstOrDefault(x => x.Name.ToLower().Contains("hq")).Id;
            using var trx = await _context.Database.BeginTransactionAsync();

            var balance = 0d;

            try
            {
                var transactionId = "SM" + new Random().Next(100_000_000, 1_000_000_000);

                var voucher = _configuration["WalletTopupVoucherTypeID"] ?? _configuration.GetSection("AppSettings:WalletTopupVoucherTypeID").Value;
                var accountMaster = new AccountMaster
                {
                    CreatedAt = DateTime.UtcNow.AddHours(1),
                    UpdatedAt = DateTime.UtcNow.AddHours(1),
                    CreatedById = createdBy,
                    CustomerDivisionId = profile.CustomerDivisionId,
                    VoucherId = long.Parse(voucher),
                    Value = request.Amount,
                    OfficeId = office,
                    BranchId = branchId,
                    TransactionId = transactionId
                };

                _context.AccountMasters.Add(accountMaster);
                _context.SaveChanges();


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
                    Description = $"Topup of {profile.Name}'s wallet with reference number {transactionId} on {ConvertDateToLongString(DateTime.UtcNow.AddHours(1))}",
                   TransactionId = transactionId,
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
                    Description = $"Topup of {profile.Name}'s wallet with reference number {transactionId} on {ConvertDateToLongString(DateTime.UtcNow.AddHours(1))}",
                    TransactionId = transactionId,
                };

                _context.AccountDetails.Add(accountDetail1);
                _context.SaveChanges();

                _context.AccountDetails.Add(accountDetail2);
                _context.SaveChanges();

                var debitCashAccount = _context.AccountDetails.Include(x => x.AccountMaster).FirstOrDefault(x => x.AccountId == int.Parse(debitCashBook));

                var acctToDebit = _context.AccountMasters.FirstOrDefault(x => x.Id == debitCashAccount.AccountMasterId);

                acctToDebit.Value = acctToDebit.Value - request.Amount;
                _context.SaveChanges();

                var transactions = _context.WalletDetails.Where(x => x.WalletMasterId == walletMaster.Id);

                var creditTransactions = transactions.Where(x => x.TransactionType == (int)WalletTransactionType.Load).ToList();
                var totalCreditAmt = 0d;
                if (creditTransactions.Count() > 0)
                {
                    var trxs = creditTransactions.Select(x => x.TransactionValue);
                    foreach (var item in trxs)
                    {
                        totalCreditAmt +=  double.Parse(new AesCryptoUtil().Decrypt(item));
                    }
                }
                var debitTransactions = transactions.Where(x => x.TransactionType == (int)WalletTransactionType.Spend);
                var totalDebitAmt = 0d;
                if (debitTransactions.Count() > 0)
                {
                    var trxs = debitTransactions.Select(x => x.TransactionValue);
                    foreach (var item in trxs)
                    {
                        totalDebitAmt += double.Parse(new AesCryptoUtil().Decrypt(item));
                    }
                }

                balance = (totalCreditAmt + request.Amount) - totalDebitAmt;
                
                var walletDetail = new WalletDetail
                {
                    CreatedAt = DateTime.UtcNow.AddHours(1),
                    CreatedById = createdBy,
                    UpdatedAt = DateTime.UtcNow.AddHours(1),
                    WalletMasterId = walletMaster.Id,
                    TransactionValue = new AesCryptoUtil().Encrypt(request.Amount.ToString()),
                    MovingBalance = new AesCryptoUtil().Encrypt(balance.ToString()),
                    Platform = request.Platform,
                    TransactionType = (int)WalletTransactionType.Load,
                    AccountMasterId = accountMaster.Id,
                };

                _context.WalletDetails.Add(walletDetail);

                walletMaster.WalletBalance = new AesCryptoUtil().Encrypt(balance.ToString());

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

        public async Task<(bool isSuccess, SpendWalletResponseDTO message)> SpendWallet(SpendWalletDTO request)
        {
            var createdBy = _context.UserProfiles.FirstOrDefault(x => x.Email.ToLower().Contains("online")).Id;
            var walletMaster = _context.WalletMasters.FirstOrDefault(x => x.OnlineProfileId == request.ProfileId);
            if (walletMaster == null)
                return (false, new SpendWalletResponseDTO { Message = "User does not have a wallet" });

            if (double.Parse(new AesCryptoUtil().Decrypt(walletMaster.WalletBalance)) < request.Amount)
                return (false, new SpendWalletResponseDTO { Message = "Insufficient funds in wallet" });
            

            var profile = _context.OnlineProfiles.FirstOrDefault(x => x.Id == request.ProfileId);
            var office = _context.Offices.FirstOrDefault(x => x.Name.ToLower().Contains("office")).Id;
            var branchId = _context.Branches.FirstOrDefault(x => x.Name.ToLower().Contains("hq")).Id;
            using var trx = await _context.Database.BeginTransactionAsync();

            var balance = 0d;

            try
            {

                var transactionId = "SM" + new Random().Next(100_000_000, 1_000_000_000);

                var voucher = _configuration["WalletReductionVoucherTypeID"] ?? _configuration.GetSection("AppSettings:WalletReductionVoucherTypeID").Value;
                var accountMaster = new AccountMaster
                {
                    CreatedAt = DateTime.UtcNow.AddHours(1),
                    UpdatedAt = DateTime.UtcNow.AddHours(1),
                    CreatedById = createdBy,
                    CustomerDivisionId = profile.CustomerDivisionId,
                    VoucherId = long.Parse(voucher),
                    Value = request.Amount,
                    OfficeId = office,
                    BranchId = branchId,
                    TransactionId = transactionId
                };

                _context.AccountMasters.Add(accountMaster);
                _context.SaveChanges();
                

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
                    Description = $"Spend from {profile.Name}'s wallet with reference number {transactionId} on {ConvertDateToLongString(DateTime.UtcNow.AddHours(1))}",
                    TransactionId = transactionId,
                    
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
                    Description = $"Spend from {profile.Name}'s wallet with reference number {transactionId} on {ConvertDateToLongString(DateTime.UtcNow.AddHours(1))}",
                    TransactionId = transactionId,
                };

                _context.AccountDetails.Add(accountDetail1);
                _context.SaveChanges();

                _context.AccountDetails.Add(accountDetail2);
                _context.SaveChanges();

                var creditCashAccount = _context.AccountDetails.Include(x => x.AccountMaster).FirstOrDefault(x => x.AccountId == int.Parse(creditCashBook));

                var acctToCredit = _context.AccountMasters.FirstOrDefault(x => x.Id == creditCashAccount.AccountMasterId);

                acctToCredit.Value = acctToCredit.Value + request.Amount;
                _context.SaveChanges();


                var transactions = _context.WalletDetails.Where(x => x.WalletMasterId == walletMaster.Id);
                var creditTransactions = transactions.Where(x => x.TransactionType == (int)WalletTransactionType.Load);
                var totalCreditAmt = 0d;
                if (creditTransactions.Count() > 0)
                {
                    var trxs = creditTransactions.Select(x => x.TransactionValue);
                    foreach (var item in trxs)
                    {
                        totalCreditAmt += double.Parse(new AesCryptoUtil().Decrypt(item));
                    }
                }
                var debitTransactions = transactions.Where(x => x.TransactionType == (int)WalletTransactionType.Spend);
                var totalDebitAmt = 0d;
                if (debitTransactions.Count() > 0)
                {
                    var trxs = debitTransactions.Select(x => x.TransactionValue);
                    foreach (var item in trxs)
                    {
                        totalDebitAmt += double.Parse(new AesCryptoUtil().Decrypt(item));
                    }
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
                    TransactionType = (int)WalletTransactionType.Spend,
                    AccountMasterId = accountMaster.Id,
                };

                _context.WalletDetails.Add(walletDetail);

                walletMaster.WalletBalance = new AesCryptoUtil().Encrypt(balance.ToString());

                _context.SaveChanges();

                await trx.CommitAsync();

                return (true, new SpendWalletResponseDTO { TransactionReference = transactionId, Message = "Success" });

            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                await trx.RollbackAsync();
                return (false, new SpendWalletResponseDTO { Message = "Wallet spending Failed" });
            }
        }

        public async Task<bool> WalletLogin(WalletLoginDTO request)
        {
            var encryptedPIN = new AesCryptoUtil().Encrypt(request.PIN);
            return _context.WalletMasters.Any(x => x.OnlineProfileId == request.ProfileId && x.WalletPin == encryptedPIN);
        }

        private object ConvertDateToLongString(DateTime dateTime)
        {
            var dateStr = dateTime.ToString("dd/MM/yyyy/HH/mm/ss");
            dateStr = dateStr.Replace("/", "");
            return dateStr;
        }

        private string GetInitials(string name)
        {
            if (string.IsNullOrEmpty(name)) return name;

            var nameArr = name.Split(' ');

            var initials = string.Empty;

            foreach (var item in nameArr)
            {
                initials += item[0].ToString().ToUpper();
            }
            return initials;
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
