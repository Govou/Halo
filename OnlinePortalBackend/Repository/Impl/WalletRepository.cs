using HalobizMigrations.Data;
using HalobizMigrations.Models;
using HalobizMigrations.Models.OnlinePortal;
using Microsoft.EntityFrameworkCore;
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
        public WalletRepository(HalobizContext context, ILogger<WalletRepository> logger)
        {
            _context = context;
            _logger = logger;
        }
        public async Task<(bool, string)> ActivateWallet(ActivateWalletDTO request)
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

                return (true, "Success");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                await trx.RollbackAsync();
            }

            return (false, "Failed");
        }

        public Task<bool> LoadWallet()
        {
            throw new System.NotImplementedException();
        }

        public Task<bool> SpendWallet()
        {
            throw new System.NotImplementedException();
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
