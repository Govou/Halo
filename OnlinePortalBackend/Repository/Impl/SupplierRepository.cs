using HalobizMigrations.Data;
using HalobizMigrations.Models;
using HalobizMigrations.Models.OnlinePortal;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using OnlinePortalBackend.DTOs.ReceivingDTOs;
using OnlinePortalBackend.Helpers;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace OnlinePortalBackend.Repository.Impl
{
    public class SupplierRepository : ISupplierRepository
    {
        private readonly HalobizContext _context;
        private readonly ILogger<SupplierRepository> _logger;
        private readonly IConfiguration _configuration;
        public SupplierRepository(HalobizContext context, ILogger<SupplierRepository> logger, IConfiguration configuration)
        {
            _context = context;
            _logger = logger;
            _configuration = configuration;
        }
        public async Task<bool> AddNewAsset(AssetAdditionDTO request)
        {
            var createdBy = _context.UserProfiles.FirstOrDefault(x => x.Email.Contains("online")).Id;
            _context.SupplierServices.Add(new SupplierService
            {
                LeftViewImage = request.LeftViewImage,
                AveragePrice = request.AveragePrice,
                CreatedAt = DateTime.UtcNow.AddHours(1),
                UpdatedAt = DateTime.UtcNow.AddHours(1),
                IsAvailable = request.IsAvailable,
                Model = request.Model,
                Description = request.Description,
                FrontViewImage = request.FrontViewImage,
                InteriorViewImage = request.InteriorViewImage,
                UnitCostPrice = request.UnitCostPrice,
                SerialNumber = request.SerialNumber,
                RearViewImage = request.RearViewImage,
                ModelNumber = request.ModelNumber,
                ImageUrl = request.ImageUrl,
                TrackerId = request.TrackerId,
                ReferenceNumber1 = request.ReferenceNumber1,
                Make = request.Make,
                ReferenceNumber2 = request.ReferenceNumber2,
                ReferenceNumber3 = request.ReferenceNumber3,
                TopViewImage = request.TopViewImage,
                IdentificationNumber = request.IdentificationNumber,
                StandardDiscount = request.StandardDiscount,
                SupplierId = request.SupplierId,
                IsDeleted = false,
                ServiceName = request.ServiceName,
                RightViewImage = request.RightViewImage,
                CreatedById = createdBy
            });
            try
            {
                var result = _context.SaveChanges();

                if (result > 0)
                    return true;

                return false;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                _logger.LogError(ex.StackTrace);
            }
          
            return false;
        }

        public async Task<bool> PostTransactionForBooking(PostTransactionDTO request)
        {
            var transactionId = "SMP" + new Random().Next(100_000_000, 1_000_000_000);
            var createdBy = _configuration["OnlineUserId"] ?? _configuration.GetSection("AppSettings:OnlineUserId").Value;
            var office = _configuration["OnlineOfficeId"] ?? _configuration.GetSection("AppSettings:OnlineOfficeId").Value;
            var branch = _configuration["OnlineBranchId"] ?? _configuration.GetSection("AppSettings:OnlineBranchId").Value;

            var profile = _context.OnlineProfiles.FirstOrDefault(p => p.SupplierId == request.ProfileId);


            var voucher = _configuration["WalletTopupVoucherTypeID"] ?? _configuration.GetSection("AppSettings:WalletTopupVoucherTypeID").Value;
            var accountMaster = new AccountMaster
            {
                CreatedAt = DateTime.UtcNow.AddHours(1),
                UpdatedAt = DateTime.UtcNow.AddHours(1),
                CreatedById = long.Parse(createdBy),
                CustomerDivisionId = profile.CustomerDivisionId,
                VoucherId = long.Parse(voucher),
                Value = Convert.ToDouble(request.Value),
                OfficeId = long.Parse(office),
                BranchId = long.Parse(branch),
                TransactionId = transactionId
            };

            _context.AccountMasters.Add(accountMaster);
            _context.SaveChanges();


            var debitCashBook = _configuration["SupplierDebitCashBookID"] ?? _configuration.GetSection("AppSettings:SupplierDebitCashBookID").Value;
            var creditCashBook = _configuration["SupplierCreditCashBookID"] ?? _configuration.GetSection("AppSettings:SupplierCreditCashBookID").Value;

            var accountDetail1 = new AccountDetail
            {
                VoucherId = accountMaster.VoucherId,
                AccountMasterId = accountMaster.Id,
                CreatedById = long.Parse(createdBy),
                CreatedAt = DateTime.UtcNow.AddHours(1),
                UpdatedAt = DateTime.UtcNow.AddHours(1),
                BranchId = long.Parse(branch),
                OfficeId = long.Parse(office),
                TransactionDate = DateTime.UtcNow.AddHours(1),
                Debit = Convert.ToDouble(request.Value),
                AccountId = int.Parse(debitCashBook),
                Description = $"Topup of {profile.Name}'s wallet with reference number {transactionId} on {GeneralHelper.ConvertDateToLongString(DateTime.UtcNow.AddHours(1))}",
                TransactionId = transactionId,
            };

            var accountDetail2 = new AccountDetail
            {
                VoucherId = accountMaster.VoucherId,
                AccountMasterId = accountMaster.Id,
                CreatedById = long.Parse(createdBy),
                CreatedAt = DateTime.UtcNow.AddHours(1),
                UpdatedAt = DateTime.UtcNow.AddHours(1),
                BranchId = long.Parse(branch),
                OfficeId = long.Parse(office),
                TransactionDate = DateTime.UtcNow.AddHours(1),
                Credit = Convert.ToDouble(request.Value),
                AccountId = int.Parse(creditCashBook),
                Description = $"Topup of {profile.Name}'s wallet with reference number {transactionId} on {GeneralHelper.ConvertDateToLongString(DateTime.UtcNow.AddHours(1))}",
                TransactionId = transactionId,
            };

            _context.AccountDetails.Add(accountDetail1);
            _context.SaveChanges();

            _context.AccountDetails.Add(accountDetail2);
            _context.SaveChanges();

            var transaction = new OnlineTransaction
            {
                CreatedAt = DateTime.UtcNow.AddHours(1),
                CreatedById = long.Parse(createdBy),
                PaymentConfirmation = true,
                PaymentFulfilment = true,
                VAT = request.Value * Convert.ToDecimal(0.075),
                Value = request.Value - (request.Value * Convert.ToDecimal(0.075)),
                UpdatedAt = DateTime.UtcNow.AddHours(1),
                TotalValue = request.Value,
                TransactionSource = "Web",
                TransactionType = "Secure mobility supplier",
                PaymentReferenceGateway = request.PaymentReferenceGateway,
                SessionId = "SMP" + new Random().Next(100_000_000, 1_000_000_000).ToString() + new Random().Next(100_000_000, 1_000_000_000).ToString(),
                ProfileId = request.ProfileId,
                PaymentReferenceInternal = "SMP" + new Random().Next(100_000_000, 1_000_000_000).ToString(),
                PaymentGatewayResponseCode = "00",
                PaymentGatewayResponseDescription = "Approved",
                PaymentGateway = request.PaymentGateway,
            };


            try
            {
                _context.OnlineTransactions.Add(transaction);
                _context.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                _logger.LogError(ex.StackTrace);
                return false;
            }
            return false;
           
        }
    }
}
