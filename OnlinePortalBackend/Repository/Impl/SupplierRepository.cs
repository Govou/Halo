using HalobizMigrations.Data;
using HalobizMigrations.Models;
using HalobizMigrations.Models.OnlinePortal;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using OnlinePortalBackend.Adapters;
using OnlinePortalBackend.DTOs.AdapterDTOs;
using OnlinePortalBackend.DTOs.ReceivingDTOs;
using OnlinePortalBackend.DTOs.TransferDTOs;
using OnlinePortalBackend.Helpers;
using OnlinePortalBackend.MyServices.SecureMobilitySales;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OnlinePortalBackend.Repository.Impl
{
    public class SupplierRepository : ISupplierRepository
    {
        private readonly HalobizContext _context;
        private readonly ILogger<SupplierRepository> _logger;
        private readonly IConfiguration _configuration;
        private readonly IPaymentAdapter _adapter;
        private readonly ISMSAccountRepository _accountRepository;
        public SupplierRepository(HalobizContext context, ILogger<SupplierRepository> logger, IConfiguration configuration, IPaymentAdapter adapter, ISMSAccountRepository accountRepository)
        {
            _context = context;
            _logger = logger;
            _configuration = configuration;
            _adapter = adapter;
            _accountRepository = accountRepository;
        }
        public async Task<bool> AddNewAsset(AssetAdditionDTO request)
        {
            var profileId = int.Parse(request.ProfileId);
            var profile = _context.OnlineProfiles.FirstOrDefault(x => x.Id == profileId);
  
            var createdBy = _context.UserProfiles.FirstOrDefault(x => x.Email.Contains("online")).Id;

            using (var transaction = await _context.Database.BeginTransactionAsync())
            {
                var supplierService = new HalobizMigrations.Models.SupplierService
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
                    SupplierId = profile.SupplierId.Value,
                    IsDeleted = false,
                    ServiceName = request.ServiceName,
                    RightViewImage = request.RightViewImage,
                    CreatedById = createdBy,
                    Type = request.Type,
                };
                _context.SupplierServices.Add(supplierService);
                try
                {
                    var result = _context.SaveChanges();

                    if (result < 1)
                        return false;

                    var verifyPaymentResult = await _adapter.VerifyPaymentAsync((PaymentGateway)GeneralHelper.GetPaymentGateway(request.PaymentGateway), request.PaymentReference);

                    if (!verifyPaymentResult.PaymentSuccessful)
                    {
                        throw new ApplicationException("Payment could not be verified");
                    }

                    var firstName = string.Empty;
                    var lastName = string.Empty;

                    var names = profile.Name.Split(' ');
                    if (names.Length == 2)
                    {
                        firstName = names[0];
                        lastName = names[1];
                    }
                    else
                    {
                        firstName = profile.Name;
                    }

                    var bookAsset = new SupplierBookAssetDTO
                    {
                        Amount = request.BookingAmount,
                        Address = request.BookingAddress,
                        State = request.BookingState,
                        CarModel = request.Model,
                        CarYear = request.Year,
                        PaymentReference = request.PaymentReference,
                        PaymentGateway = request.PaymentGateway,
                        CentreId = request.CentreId,
                        PayType = request.PaymentType,
                        Date = request.AppointmentDate,
                        Time = request.AppointmentTime,
                        ServiceId = request.ServiceId,
                        FirstName = firstName,
                        LastName = lastName,
                        Email = profile.Email,
                        Phone = request.ModelNumber,
                        ProviderId = request.ProviderId,
                    };

                    var bookAssetResult = await BookAssetOnThridPartyService(bookAsset);

                    if (!bookAssetResult.isSuccess)
                    {
                        throw new ApplicationException("Unsuccessful booking of asset");
                    }

                    await transaction.CommitAsync();
                    return true;

                }
                catch (Exception ex)
                {
                    await transaction.RollbackAsync();
                    _logger.LogError(ex.Message);
                    _logger.LogError(ex.StackTrace);
                }
            }
           
           
          
            return false;
        }


        private async Task<(bool isSuccess, string message)> BookAssetOnThridPartyService(SupplierBookAssetDTO bookAsset)
        {
            var client = new RestClient("https://api.myvivcar.com/api/booking");
            var request = new RestRequest(Method.POST);

            request.AddHeader("Content-Type", "application/json");

            var body = JsonConvert.SerializeObject(bookAsset);

            request.AddParameter("application/json", body, ParameterType.RequestBody);
            IRestResponse response = client.Execute(request);

            var result = JsonConvert.DeserializeObject<SupplierBookAssetResponseDTO>(response.Content);

            if (result.Status == "success")
            {
                return (true, result.Message);
            }
            return (false, result.Message);
        }


        public async Task<IEnumerable<SupplierCategoryDTO>> GetSupplierCategories()
        {
            return _context.SupplierCategories.Where(x => x.IsDeleted == false).Select(m => new SupplierCategoryDTO
            {
                CategoryId = m.Id,
                CategoryName = m.CategoryName
            }).AsEnumerable();
        }

        public async Task<IEnumerable<VehicleMakeDTO>> GetVehicleMakes()
        {
            return _context.Makes.Where(x => x.IsDeleted == false).Select(m => new VehicleMakeDTO {
                MakeId = m.Id,
                Name = m.Caption
            }).AsEnumerable();

        }

        public async Task<IEnumerable<VehicleModelDTO>> GetVehicleModels(int makeId)
        {
            return _context.Models.Where(x => x.IsDeleted == false && x.MakeId == makeId).Select(m => new VehicleModelDTO
            {
                ModelId = m.Id,
                Name = m.Caption,
                MakeId = makeId
            }).AsEnumerable();
        }

        public async Task<bool> PostTransactionForBooking(PostTransactionDTO request)
        {
            try
            {
            var transactionId = "SMP" + new Random().Next(100_000_000, 1_000_000_000);
            var createdBy = _configuration["OnlineUserId"] ?? _configuration.GetSection("AppSettings:OnlineUserId").Value;
            var office = _configuration["OnlineOfficeId"] ?? _configuration.GetSection("AppSettings:OnlineOfficeId").Value;
            var branch = _configuration["OnlineBranchId"] ?? _configuration.GetSection("AppSettings:OnlineBranchId").Value;

            var profile = _context.OnlineProfiles.FirstOrDefault(p => p.Id == request.ProfileId);

            var supplier = _context.Suppliers.FirstOrDefault(p => p.Id == profile.SupplierId);

            var voucher = _configuration["WalletTopupVoucherTypeID"] ?? _configuration.GetSection("AppSettings:WalletTopupVoucherTypeID").Value;
                var payDesc = $"Payment on {profile.Name}'s supply with reference number {transactionId} on {GeneralHelper.ConvertDateToLongString(DateTime.UtcNow.AddHours(1))}";

            var accountMaster = new AccountMaster
            {
                CreatedAt = DateTime.UtcNow.AddHours(1),
                UpdatedAt = DateTime.UtcNow.AddHours(1),
                CreatedById = long.Parse(createdBy),
                VoucherId = long.Parse(voucher),
                Value = Convert.ToDouble(request.Value),
                OfficeId = long.Parse(office),
                BranchId = long.Parse(branch),
                TransactionId = transactionId,
                Description = payDesc
            };

            _context.AccountMasters.Add(accountMaster);
            _context.SaveChanges();

            var debitCashBook = _configuration["SupplierDebitCashBookID"] ?? _configuration.GetSection("AppSettings:SupplierDebitCashBookID").Value;

            var creditCashBook = await _accountRepository.GetServiceIncomeAccountForSupplier(supplier);

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
                Description = payDesc,
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
                AccountId = creditCashBook,
                Description = payDesc,
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
           
        }

        public async Task<SupplierDashboardDetails> GetDashboardDetails(int profileId)
        {
           var dashB = new SupplierDashboardDetails();

           var profile = _context.OnlineProfiles.FirstOrDefault(x => x.Id == profileId);

            var services = _context.SupplierServices.Where(x => x.SupplierId == profile.SupplierId).ToList();

            dashB.SupplierName = profile.Name;

            if (services.Count > 0)
            {
                dashB.TotalAssetUnderManagement = services.Count.ToString();
                dashB.DistinctTypes = services.DistinctBy(x => x.ServiceName).Count().ToString();
                dashB.AssetAwaitingInspection = services.Count.ToString();
                dashB.AssetAddedInCurrentMonth = services.Where(x => x.CreatedAt.Year == DateTime.Today.Year && x.CreatedAt.Month == DateTime.Today.Month).Count().ToString();
                dashB.TotalAssetsDueForReAccreditation = 0.ToString();
                dashB.PreferredServiceCentre = 0.ToString();
            }

            dashB.TotalAssetUnderManagement = dashB.TotalAssetUnderManagement ?? "0";
            dashB.DistinctTypes = dashB.DistinctTypes ?? "0";
            dashB.AssetAwaitingInspection = dashB.AssetAwaitingInspection ?? "0";
            dashB.AssetAddedInCurrentMonth = dashB.AssetAddedInCurrentMonth ?? "0";
            dashB.TotalAssetsDueForReAccreditation = dashB.TotalAssetsDueForReAccreditation ?? "0";
            dashB.PreferredServiceCentre = dashB.PreferredServiceCentre ?? "0";

            return dashB;
        }

        public async Task<AssetUnderManagementDTO> AssetsUnderManagement(int profileId)
        {
            var supplierId = _context.OnlineProfiles.FirstOrDefault(x => x.Id == profileId).SupplierId;
            var assets = _context.SupplierServices.Where(x => x.SupplierId == supplierId);
            var pendingAssets = assets.Where(x => x.IsReady == false).Select(x => new Asset
            {
                ServiceName = x.ServiceName,
                PlateNumber = x.IdentificationNumber,
                DateAdded = x.CreatedAt.ToString("MMMM dd, yyyy"),
                PercentageCompleted = 0,
                DaysLeft = 10,
                ImageUrl = x.FrontViewImage
            });

            var completedAssets = assets.Where(x => x.IsReady == true).Select(x => new Asset
            {
                ServiceName = x.ServiceName,
                PlateNumber = x.IdentificationNumber,
                DateAdded = x.CreatedAt.ToString("MMMM dd, yyyy"),
                PercentageCompleted = 100,
                DaysLeft = 0,
                ImageUrl = x.FrontViewImage
            });

            var allAssets = new AssetUnderManagementDTO
            {
                CompletedReview = completedAssets,
                PendingReview = pendingAssets
            };

            return allAssets;
        }
    }
}
