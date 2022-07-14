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
using OnlinePortalBackend.Helpers;
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
        private readonly ICustomerInfoRepository _customerInfoRepository;

        private readonly string RETAIL_VAT_ACCOUNT = "RETAIL VAT ACCOUNT";
        private readonly string VatControlAccount = "VAT";


        public WalletRepository(HalobizContext context, ILogger<WalletRepository> logger, IConfiguration configuration, ICustomerInfoRepository customerInfoRepository)
        {
            _context = context;
            _logger = logger;
            _customerInfoRepository = customerInfoRepository;
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

            var alias = string.Empty;
            var custDiv = _context.CustomerDivisions.FirstOrDefault(x => x.Id == profile.CustomerDivisionId);

            if (custDiv != null)
            {
                alias = custDiv.DTrackCustomerNumber;
            }

            using var trx = await _context.Database.BeginTransactionAsync();
           
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
                    Alias = string.IsNullOrEmpty(alias) ? await _customerInfoRepository.GetDtrackCustomerNumber(new CustomerDivision { DivisionName = profile.Name }) : alias
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
                var payDesc = $"Topup of {profile.Name}'s wallet with reference number {transactionId} on {GeneralHelper.ConvertDateToLongString(DateTime.UtcNow.AddHours(1))}";
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
                    TransactionId = transactionId,
                    Description = payDesc
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
                    Description = payDesc,
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
                    Description = payDesc,
                    TransactionId = transactionId,
                };

                _context.AccountDetails.Add(accountDetail1);
                _context.SaveChanges();

                _context.AccountDetails.Add(accountDetail2);
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

            var customerDiv = _context.OnlineProfiles.FirstOrDefault(x => x.Id == request.ProfileId).CustomerDivisionId;

            if (customerDiv != null)
            {
                var totalBookingAmt = 0d;
                var scheduledBookingConService = _context.MasterServiceAssignments.Where(x => x.CustomerDivisionId == customerDiv && x.IsDeleted == false && x.IsPaidFor == false && x.IsScheduled == true && x.IsAddedToCart == true && x.PickoffTime > DateTime.UtcNow.AddHours(1)).Select(x => x.ContractServiceId).AsEnumerable();
                foreach (var item in scheduledBookingConService)
                {
                    totalBookingAmt += _context.ContractServices.FirstOrDefault(x => x.Id == item.Value).AdHocInvoicedAmount;
                }

                if (double.Parse(new AesCryptoUtil().Decrypt(walletMaster.WalletBalance)) < request.Amount + totalBookingAmt)
                    return (false, new SpendWalletResponseDTO { Message = "Insufficient funds in wallet" });
            }

            var profile = _context.OnlineProfiles.FirstOrDefault(x => x.Id == request.ProfileId);
            var office = _context.Offices.FirstOrDefault(x => x.Name.ToLower().Contains("office")).Id;
            var branchId = _context.Branches.FirstOrDefault(x => x.Name.ToLower().Contains("hq")).Id;
            using var trx = await _context.Database.BeginTransactionAsync();

            var balance = 0d;

            try
            {

                var transactionId = "SM" + new Random().Next(100_000_000, 1_000_000_000);

                var voucher = _configuration["WalletReductionVoucherTypeID"] ?? _configuration.GetSection("AppSettings:WalletReductionVoucherTypeID").Value;

                var payDesc = $"Spend from {profile.Name}'s wallet with reference number {transactionId} on {GeneralHelper.ConvertDateToLongString(DateTime.UtcNow.AddHours(1))}";
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
                    TransactionId = transactionId,
                    Description = payDesc
                };

                _context.AccountMasters.Add(accountMaster);
                _context.SaveChanges();

                var customerDivision = _context.CustomerDivisions.FirstOrDefault(x => x.Id == customerDiv);

                var conService = _context.ContractServices.Include(x => x.Service).FirstOrDefault(x => x.Id == request.ContractServiceId);

                var serviceAccountId =  await GetServiceIncomeAccountForClient(customerDivision, conService.Service);

                var vatAccountId = await GetRetailVATAccount(customerDivision);

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
                    Credit = request.Amount - (request.Amount * 0.075),
                    AccountId = serviceAccountId,
                    Description = payDesc,
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
                    Credit = request.Amount * 0.075,
                    AccountId = vatAccountId,
                    Description = payDesc,
                    TransactionId = transactionId,

                };

                var accountDetail3 = new AccountDetail
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
                    Description = payDesc,
                    TransactionId = transactionId,
                };

                _context.AccountDetails.Add(accountDetail1);
                _context.SaveChanges();

                _context.AccountDetails.Add(accountDetail2);
                _context.SaveChanges();

                _context.AccountDetails.Add(accountDetail3);
                _context.SaveChanges();

                //var creditCashBk = int.Parse(creditCashBook);
                //var creditCashAccount = _context.AccountDetails.Include(x => x.AccountMaster).FirstOrDefault(x => x.AccountId == creditCashBk);

                //var acctToCredit = _context.AccountMasters.FirstOrDefault(x => x.Id == creditCashAccount.AccountMasterId);

                //acctToCredit.Value = acctToCredit.Value + request.Amount;
                //_context.SaveChanges();

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
                    TransactionValue = new AesCryptoUtil().Encrypt(request.Amount.ToString()),
                    MovingBalance = new AesCryptoUtil().Encrypt(balance.ToString()),
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

        private async Task<long> GetServiceIncomeAccountForClient(CustomerDivision customerDivision, Service service)
        {
            string serviceClientIncomeAccountName = $"{service.Name} Income for {customerDivision.DivisionName}";

            var createdBy = _context.UserProfiles.FirstOrDefault(x => x.Email.ToLower().Contains("online")).Id;

            Account serviceClientIncomeAccount = await _context.Accounts
                .FirstOrDefaultAsync(x => x.ControlAccountId == (long)service.ControlAccountId && x.Name == serviceClientIncomeAccountName);

            long accountId = 0;
            if (serviceClientIncomeAccount == null)
            {
                Account account = new Account()
                {
                    Name = serviceClientIncomeAccountName,
                    Description = $"{service.Name} Income Account for {customerDivision.DivisionName}",
                    Alias = String.IsNullOrEmpty(customerDivision.DTrackCustomerNumber) ? await _customerInfoRepository.GetDtrackCustomerNumber(customerDivision) : customerDivision.DTrackCustomerNumber,
                    IsDebitBalance = true,
                    ControlAccountId = (long)service.ControlAccountId,
                    CreatedById = createdBy
                };
                var savedAccount = await SaveAccount(account);
                accountId = savedAccount;
                await _context.SaveChangesAsync();
            }
            else
            {
                accountId = serviceClientIncomeAccount.Id;
            }

            return accountId;
        }

        private async Task<long> GetRetailVATAccount(CustomerDivision customerDivision)
        {
            var createdBy = _context.UserProfiles.FirstOrDefault(x => x.Email.ToLower().Contains("online")).Id;

            Account vatAccount = await _context.Accounts.FirstOrDefaultAsync(x => x.Name == RETAIL_VAT_ACCOUNT);
            long accountId = 0;
            if (vatAccount == null)
            {
                ControlAccount controlAccount = await _context.ControlAccounts
                        .FirstOrDefaultAsync(x => x.Caption == VatControlAccount);

                Account account = new Account()
                {
                    Name = RETAIL_VAT_ACCOUNT,
                    Description = $"VAT Account of Retail Clients",
                    Alias = "HA_RET",
                    IsDebitBalance = true,
                    ControlAccountId = controlAccount.Id,
                    CreatedById = createdBy
                };
                var savedAccount = await SaveAccount(account);

                customerDivision.VatAccountId = savedAccount;
                _context.CustomerDivisions.Update(customerDivision);
                await _context.SaveChangesAsync();
                accountId = savedAccount;
            }
            else
            {
                customerDivision.VatAccountId = vatAccount.Id;
                _context.CustomerDivisions.Update(customerDivision);
                await _context.SaveChangesAsync();
                accountId = vatAccount.Id;
            }

            return accountId;

        }

        public async Task<WalletTransactionStatistics> GetWalletTransactionStatistics(int profileId)
        {

            var walletmaster = _context.WalletMasters.FirstOrDefault(x => x.OnlineProfileId == profileId);
            if (walletmaster == null)
                return null;

            var stat = new WalletTransactionStatistics();

            var walletTrx = _context.WalletDetails.Where(x => x.WalletMasterId == walletmaster.Id).ToList();

            stat.TotalTransactions = walletTrx.Count();
            stat.TopUpCount = walletTrx.Where(x => x.TransactionType == (int)WalletTransactionType.Load).Select( x=> x.TransactionValue).Count();
            stat.SpendCount = walletTrx.Where(x => x.TransactionType == (int)WalletTransactionType.Spend).Select( x=> x.TransactionValue).Count();

            if (stat.TotalTransactions > 0)
            {
                stat.TopUpPercent = (stat.TopUpCount / stat.TotalTransactions) * 100;
                stat.TopUpPercent = (stat.SpendCount / stat.TotalTransactions) * 100;
            }
            
            return stat;
        }

        public async Task<bool> CheckWalletFundedEnough(long profileId, double amount)
        {
            var walletMaster = _context.WalletMasters.FirstOrDefault(x => x.OnlineProfileId == profileId);

            if (walletMaster == null)
            {
                return false;
            }

            var balance = double.Parse(new AesCryptoUtil().Decrypt(walletMaster.WalletBalance));

            if (balance < amount)
            {
                return false;
            }

            var custDivId = _context.OnlineProfiles.FirstOrDefault(x => x.Id == profileId).CustomerDivisionId;

            var pendindingTrips = _context.MasterServiceAssignments.Where(x => x.CustomerDivisionId == custDivId && x.AssignmentStatus == "Open" && x.IsScheduled == true && x.IsPaidFor == false);

            if (pendindingTrips.Count() > 0)
            {
                var amountPending = 0d;
                foreach (var item in pendindingTrips)
                {
                    var amt = _context.ContractServices.FirstOrDefault(x => x.Id == item.ContractServiceId).BillableAmount.Value;
                    amountPending += amt;
                }

                if ((amountPending + amount) > balance)
                {
                    return false;
                }
                return true;
            }

            return true;
        }
    }
}
