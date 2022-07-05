using AutoMapper;
using Halobiz.Common.DTOs.ApiDTOs;
using Halobiz.Common.DTOs.ReceivingDTOs;
using HalobizMigrations.Data;
using HalobizMigrations.Models;
using HalobizMigrations.Models.Halobiz;
using HalobizMigrations.Models.OnlinePortal;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Security.Cryptography;
using Microsoft.EntityFrameworkCore;
using OnlinePortalBackend.Helpers;
using HalobizMigrations.Models.Shared;
using OnlinePortalBackend.DTOs.TransferDTOs;
using OnlinePortalBackend.DTOs.ReceivingDTOs;
using Microsoft.Extensions.Configuration;

namespace OnlinePortalBackend.Repository.Impl
{
    public class SMSAccountRepository : ISMSAccountRepository
    {
        private readonly HalobizContext _context;
        private readonly ILogger<SMSAccountRepository> _logger;
        private readonly IMapper _mapper;
        private readonly ICustomerInfoRepository _customerInfoRepository;

        private readonly IConfiguration _configuration;

        private readonly string RETAIL_VAT_ACCOUNT = "RETAIL VAT ACCOUNT";
        private readonly string VatControlAccount = "VAT";
        public SMSAccountRepository(HalobizContext context,
            ILogger<SMSAccountRepository> logger,
            IMapper mapper, IConfiguration configuration,
            ICustomerInfoRepository customerInfoRepository)
        {
            _logger = logger;
            _context = context;
            _mapper = mapper;
            _customerInfoRepository = customerInfoRepository;
            _configuration = configuration;
        }
        public async Task<(bool success, string message)> CreateBusinessAccount(SMSBusinessAccountDTO accountDTO)
        {
            var exist = _context.OnlineProfiles.Any(x => x.Email.ToLower() == accountDTO.AccountLogin.Email.ToLower());

            if (exist)
                return (false, "User already exists");

            var createdBy = _context.UserProfiles.FirstOrDefault(x => x.Email.ToLower().Contains("online")).Id;
            var grouptype = _context.GroupTypes.FirstOrDefault(x => x.Caption.ToLower() == "sme").Id;
            var leadOrigin = _context.LeadOrigins.FirstOrDefault(x => x.Caption.ToLower() == "email").Id;
            var branchId = _context.Branches.FirstOrDefault(x => x.Name.ToLower().Contains("hq")).Id;
            var leadtype = _context.LeadTypes.FirstOrDefault(x => x.Caption.ToLower() == "rfq").Id;
            var office = _context.Offices.FirstOrDefault(x => x.Name.ToLower().Contains("office")).Id;

            //var receivableAcctId = 0;
            var gender = accountDTO.ContactPerson.Gender == "M" ? Gender.Male : Gender.Female;

            using var transaction = await _context.Database.BeginTransactionAsync();

            var contact = new Contact
            {
                CreatedAt = DateTime.UtcNow.AddHours(1),
                UpdatedAt = DateTime.UtcNow.AddHours(1),
                CreatedById = createdBy,
                Email = accountDTO.AccountLogin.Email,
                FirstName = accountDTO.ContactPerson.FirstName,
                LastName = accountDTO.ContactPerson.LastName,
                Gender = gender,
                Mobile = accountDTO.ContactPerson.PhoneNumber,
                DateOfBirth = accountDTO.ContactPerson.DateOfBirth,

            };

            var leadDivisionContact = new LeadDivisionContact
            {
                CreatedAt = DateTime.UtcNow.AddHours(1),
                UpdatedAt = DateTime.UtcNow.AddHours(1),
                CreatedById = createdBy,
                Email = accountDTO.AccountLogin.Email,
                FirstName = accountDTO.ContactPerson.FirstName,
                LastName = accountDTO.ContactPerson.LastName,
                Gender = accountDTO.ContactPerson.Gender,
                Type = (int)leadtype,
                MobileNumber = accountDTO.PhoneNumber
            };

            var lga = _context.Lgas.FirstOrDefault(x => x.Id == accountDTO.LGAId).Name;
            var state = _context.States.FirstOrDefault(x => x.Id == accountDTO.StateId).Name;

            var suspect = new Suspect
            {
                Address = accountDTO.Address + ", " + lga + ", " + state,
                BusinessName = accountDTO.CompanyName,
                CreatedAt = DateTime.UtcNow.AddHours(1),
                UpdatedAt = DateTime.UtcNow.AddHours(1),
                BranchId = branchId,
                CreatedById = createdBy,
                Email = accountDTO.AccountLogin.Email,
                GroupTypeId = grouptype,
                ImageUrl = accountDTO.LogoUrl,
                IndustryId = accountDTO.IndustryId,
                MobileNumber = accountDTO.PhoneNumber,
                LeadOriginId = leadOrigin,
                LeadTypeId = leadtype,
                LgaId = accountDTO.LGAId,
                OfficeId = office,
                StateId = accountDTO.StateId,
                RCNumber = accountDTO.RCNumber
            };

            var leadReference = await GetReferenceNumber();
            var leadReferenceNum = leadReference.ReferenceNo;
            var leadNextReference = leadReferenceNum.GenerateReferenceNumber();

            var lead = new Lead
            {
                ReferenceNo = leadNextReference,
                CreatedAt = DateTime.UtcNow.AddHours(1),
                UpdatedAt = DateTime.UtcNow.AddHours(1),
                CreatedById = createdBy,
                GroupName = accountDTO.CompanyName,
                GroupTypeId = grouptype,
                Industry = accountDTO.Industry,
                LeadOriginId = leadOrigin,
                LeadTypeId = leadtype,
                LogoUrl = accountDTO.LogoUrl,
                Rcnumber = accountDTO.RCNumber,
            };

            var suspectContact = new SuspectContact
            {
                ContactDesignation = ContactDesignation.Admin,
                ContactPriority = ContactPriority.PrimaryContact,
                ContactQualification = ContactQualification.DecisionMaker
            };

            var leadDivision = new LeadDivision
            {
                Address = accountDTO.Address + ", " + lga + ", " + state,
                CreatedAt = DateTime.UtcNow.AddHours(1),
                UpdatedAt = DateTime.UtcNow.AddHours(1),
                BranchId = branchId,
                CreatedById = createdBy,
                DivisionName = accountDTO.CompanyName,
                Email = accountDTO.AccountLogin.Email,
                Industry = accountDTO.Industry,
                LeadOriginId = leadOrigin,
                LeadTypeId = leadtype,
                StateId = accountDTO.StateId,
                Lgaid = accountDTO.LGAId,
                LogoUrl = accountDTO.LogoUrl,
                PhoneNumber = accountDTO.PhoneNumber,
                OfficeId = office,
                Rcnumber = accountDTO.RCNumber,
                Street = accountDTO.Address
            };

            var (salt, hashed) = HashPassword(new byte[] { }, accountDTO.AccountLogin.Password);

            var onlinProfile = new OnlineProfile
            {
                Email = accountDTO.AccountLogin.Email,
                NormalizedEmail = accountDTO.AccountLogin.Email.ToUpper(),
                PasswordHash = hashed,
                SecurityStamp = Convert.ToBase64String(salt),
                Name = accountDTO.CompanyName,
                CreatedAt = DateTime.UtcNow.AddHours(1)
            };

            try
            {
                _context.LeadDivisionContacts.Add(leadDivisionContact);
                await _context.SaveChangesAsync();

                _context.Suspects.Add(suspect);
                await _context.SaveChangesAsync();

                lead.SuspectId = suspect.Id;

                _context.Leads.Add(lead);
                await _context.SaveChangesAsync();

                _context.Contacts.Add(contact);
                await _context.SaveChangesAsync();

                suspectContact.ContactId = contact.Id;
                suspectContact.SuspectId = suspect.Id;

                _context.SuspectContacts.Add(suspectContact);
                await _context.SaveChangesAsync();

                leadReference.ReferenceNo = leadReference.ReferenceNo + 1;
                await UpdateReferenceNumber(leadReference);

                leadDivision.LeadId = lead.Id;
                leadDivision.PrimaryContactId = leadDivisionContact.Id;

                _context.LeadDivisions.Add(leadDivision);
                await _context.SaveChangesAsync();

                onlinProfile.LeadDivisionId = leadDivision.Id;

                _context.OnlineProfiles.Add(onlinProfile);
                await _context.SaveChangesAsync();

                await transaction.CommitAsync();

                return (true, "success");
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message);
                _logger.LogError(e.StackTrace);
                await transaction.RollbackAsync();
                return (false, "An error has occured");
            }
        }

        public async Task<(bool success, string message)> CreateIndividualAccount(SMSIndividualAccountDTO accountDTO)
        {
            var exist = _context.OnlineProfiles.Any(x => x.Email.ToLower() == accountDTO.AccountLogin.Email.ToLower());

            if (exist)
                return (false, "User already exists");

            var createdBy = _context.UserProfiles.FirstOrDefault(x => x.Email.ToLower().Contains("online")).Id;
            var grouptype = _context.GroupTypes.FirstOrDefault(x => x.Caption.ToLower() == "individual").Id;
            var leadOrigin = _context.LeadOrigins.FirstOrDefault(x => x.Caption.ToLower() == "email").Id;
            var branchId = _context.Branches.FirstOrDefault(x => x.Name.ToLower().Contains("hq")).Id;
            var leadtype = _context.LeadTypes.FirstOrDefault(x => x.Caption.ToLower() == "rfq").Id;
            var office = _context.Offices.FirstOrDefault(x => x.Name.ToLower().Contains("office")).Id;
            //var receivableAcctId = 0;

            using var transaction = await _context.Database.BeginTransactionAsync();
            var gender = accountDTO.Gender == "M" ? Gender.Male : Gender.Female;
            var contact = new Contact
            {
                CreatedAt = DateTime.UtcNow.AddHours(1),
                UpdatedAt = DateTime.UtcNow.AddHours(1),
                CreatedById = createdBy,
                Email = accountDTO.AccountLogin.Email,
                FirstName = accountDTO.FirstName,
                LastName = accountDTO.LastName,
                Gender = gender,
                ProfilePicture = accountDTO.ImageUrl,
                Mobile = accountDTO.PhoneNumber,

            };
            var lga = _context.Lgas.FirstOrDefault(x => x.Id == accountDTO.LGAId).Name;
            var state = _context.States.FirstOrDefault(x => x.Id == accountDTO.StateId).Name;

            var suspect = new Suspect
            {
                Address = accountDTO.Address + ", " + lga + ", " + state,
                CreatedAt = DateTime.UtcNow.AddHours(1),
                UpdatedAt = DateTime.UtcNow.AddHours(1),
                BranchId = branchId,
                CreatedById = createdBy,
                Email = accountDTO.AccountLogin.Email,
                GroupTypeId = grouptype,
                ImageUrl = accountDTO.ImageUrl,
                MobileNumber = accountDTO.PhoneNumber,
                LeadOriginId = leadOrigin,
                LeadTypeId = leadtype,
                LgaId = accountDTO.LGAId,
                OfficeId = office,
                StateId = accountDTO.StateId,
                FirstName = accountDTO.FirstName,
                LastName = accountDTO.LastName,
                Street = accountDTO.Address
            };

            var leadReference = await GetReferenceNumber();
            var leadReferenceNum = leadReference.ReferenceNo;
            var leadNextReference = leadReferenceNum.GenerateReferenceNumber();

            var lead = new Lead
            {
                ReferenceNo = leadNextReference,
                CreatedAt = DateTime.UtcNow.AddHours(1),
                UpdatedAt = DateTime.UtcNow.AddHours(1),
                CreatedById = createdBy,
                GroupName = accountDTO.FirstName + " " + accountDTO.LastName,
                GroupTypeId = grouptype,
                LeadOriginId = leadOrigin,
                LeadTypeId = leadtype,
                LogoUrl = accountDTO.ImageUrl
            };

            var leadDivision = new LeadDivision
            {
                Address = accountDTO.Address + ", " + lga + ", " + state,
                CreatedAt = DateTime.UtcNow.AddHours(1),
                UpdatedAt = DateTime.UtcNow.AddHours(1),
                BranchId = branchId,
                CreatedById = createdBy,
                DivisionName = accountDTO.FirstName + " " + accountDTO.LastName,
                Email = accountDTO.AccountLogin.Email,
                LeadOriginId = leadOrigin,
                LeadTypeId = leadtype,
                StateId = accountDTO.StateId,
                Lgaid = accountDTO.LGAId,
                LogoUrl = accountDTO.ImageUrl,
                PhoneNumber = accountDTO.PhoneNumber,
                OfficeId = office,
                Street = accountDTO.Address
            };

            var suspectContact = new SuspectContact
            {
                ContactDesignation = ContactDesignation.Self,
                ContactQualification = ContactQualification.DecisionMaker,
                ContactPriority = ContactPriority.PrimaryContact,
            };

            var leadDivisionContact = new LeadDivisionContact
            {
                CreatedAt = DateTime.UtcNow.AddHours(1),
                UpdatedAt = DateTime.UtcNow.AddHours(1),
                CreatedById = createdBy,
                Email = accountDTO.AccountLogin.Email,
                FirstName = accountDTO.FirstName,
                LastName = accountDTO.LastName,
                Gender = accountDTO.Gender,
                Type = (int)leadtype,
                MobileNumber = accountDTO.PhoneNumber
            };

            //var custDivision = new CustomerDivision
            //{
            //    Address = accountDTO.Address,
            //    CreatedAt = DateTime.UtcNow.AddHours(1),
            //    UpdatedAt = DateTime.UtcNow.AddHours(1),
            //    // ReceivableAccountId = receivableAcctId,
            //    Lgaid = accountDTO.LGAId,
            //    CreatedById = createdBy,
            //    Email = accountDTO.AccountLogin.Email,
            //    PhoneNumber = accountDTO.PhoneNumber,
            //    LogoUrl = accountDTO.ImageUrl,
            //    StateId = accountDTO.StateId,
            //    DivisionName = accountDTO.FirstName + " " + accountDTO.LastName
            //};

            var (salt, hashed) = HashPassword(new byte[] { }, accountDTO.AccountLogin.Password);

            var onlinProfile = new OnlineProfile
            {
                Email = accountDTO.AccountLogin.Email,
                NormalizedEmail = accountDTO.AccountLogin.Email.ToUpper(),
                PasswordHash = hashed,
                SecurityStamp = Convert.ToBase64String(salt),
                Name = accountDTO.FirstName + " " + accountDTO.LastName,
                CreatedAt = DateTime.UtcNow.AddHours(1)
            };

            try
            {
                _context.LeadDivisionContacts.Add(leadDivisionContact);
                await _context.SaveChangesAsync();

                _context.Suspects.Add(suspect);
                await _context.SaveChangesAsync();

                lead.SuspectId = suspect.Id;

                _context.Leads.Add(lead);
                await _context.SaveChangesAsync();

                _context.Contacts.Add(contact);
                await _context.SaveChangesAsync();

                suspectContact.ContactId = contact.Id;
                suspectContact.SuspectId = suspect.Id;

                _context.SuspectContacts.Add(suspectContact);
                await _context.SaveChangesAsync();

                leadReference.ReferenceNo = leadReference.ReferenceNo + 1;
                await UpdateReferenceNumber(leadReference);

                leadDivision.LeadId = lead.Id;
                leadDivision.PrimaryContactId = leadDivisionContact.Id;

                _context.LeadDivisions.Add(leadDivision);
                await _context.SaveChangesAsync();

                onlinProfile.LeadDivisionId = leadDivision.Id;

                _context.OnlineProfiles.Add(onlinProfile);
                await _context.SaveChangesAsync();

                await transaction.CommitAsync();

                return (true, "success");
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message);
                _logger.LogError(e.StackTrace);
                await transaction.RollbackAsync();
                return (false, "An error has occured");
            }

        }

        private static (byte[], string) HashPassword(byte[] salt, string password)
        {
            // generate a 128-bit salt using a cryptographically strong random sequence of nonzero values
            if (salt.Length == 0)
            {
                salt = new byte[128 / 8];
                using (var rngCsp = new RNGCryptoServiceProvider())
                {
                    rngCsp.GetNonZeroBytes(salt);
                }
            }

            // derive a 256-bit subkey (use HMACSHA256 with 100,000 iterations)
            string hashed = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                password: password,
                salt: salt,
                prf: KeyDerivationPrf.HMACSHA256,
                iterationCount: 100000,
                numBytesRequested: 256 / 8));

            return (salt, hashed);
        }


        public async Task<ReferenceNumber> GetReferenceNumber()
        {
            return await _context.ReferenceNumbers.FirstOrDefaultAsync(referenceNo => referenceNo.Id == 1);
        }

        public async Task<bool> UpdateReferenceNumber(ReferenceNumber refNumber)
        {
            _context.ReferenceNumbers.Update(refNumber);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<OnlineProfileDTO> GetCustomerProfile(int profileId)
        {
            var onlineProfile = _context.OnlineProfiles.FirstOrDefault(x => x.Id == profileId);
            if (onlineProfile == null)
            {
                return null;
            }

            var leadProfile = _context.LeadDivisions.Include(x => x.State).Include(x => x.Lga).FirstOrDefault(x => x.Id == onlineProfile.LeadDivisionId);
            var profile = new OnlineProfileDTO
            {
                CreatedAt = onlineProfile.CreatedAt,
                Email = onlineProfile.Email,
                Name = onlineProfile.Name,
                profileImage = leadProfile.LogoUrl,
                PercentageCompletion = String.IsNullOrEmpty(leadProfile.LogoUrl) ? "90%" : "100%",
                Id = profileId,
                LGAId = (int)leadProfile.Lgaid.Value,
                StateId = (int)leadProfile.StateId.Value,
                Street = leadProfile.Street,
                StateName = leadProfile.State.Name,
                LGAName = leadProfile.Lga.Name,
                PhoneNumber = leadProfile.PhoneNumber
            };

            return profile;

        }

        public async Task<string> GetProfileImage(long profileID)
        {
            var profile = _context.OnlineProfiles.FirstOrDefault(x => x.Id == profileID);
            if (profile == null)
            {
                return null;
            }
            var leadDiv = _context.LeadDivisions.FirstOrDefault(x => x.Id == profile.LeadDivisionId);

            if (leadDiv == null)
                return null;

            return leadDiv.LogoUrl;

        }

        public async Task<(bool success, string message)> CreateSupplierBusinessAccount(SMSSupplierBusinessAccountDTO accountDTO)
        {
            var exist = _context.OnlineProfiles.Any(x => x.Email.ToLower() == accountDTO.AccountLogin.Email.ToLower());

            if (exist)
                return (false, "User already exists");

            var exist1 = _context.Suppliers.Any(x => x.SupplierEmail.ToLower() == accountDTO.AccountLogin.Email.ToLower());

            if (exist1)
                return (false, "User already exists");

            var createdBy = _context.UserProfiles.FirstOrDefault(x => x.Email.ToLower().Contains("online")).Id;
            var grouptype = _context.GroupTypes.FirstOrDefault(x => x.Caption.ToLower() == "individual").Id;
            var leadOrigin = _context.LeadOrigins.FirstOrDefault(x => x.Caption.ToLower() == "email").Id;
            var branchId = _context.Branches.FirstOrDefault(x => x.Name.ToLower().Contains("hq")).Id;
            var leadtype = _context.LeadTypes.FirstOrDefault(x => x.Caption.ToLower() == "rfq").Id;
            var office = _context.Offices.FirstOrDefault(x => x.Name.ToLower().Contains("office")).Id;
            //var receivableAcctId = 0;

            using var transaction = await _context.Database.BeginTransactionAsync();
            var gender = accountDTO.PrimaryContactGender == "M" ? Gender.Male : Gender.Female;

            var firstName = accountDTO.PrimaryContactName.Split(' ')[0];
            var lastName = accountDTO.PrimaryContactName.Split(' ')[1];

            var contact = new Contact
            {
                CreatedAt = DateTime.UtcNow.AddHours(1),
                UpdatedAt = DateTime.UtcNow.AddHours(1),
                CreatedById = createdBy,
                Email = accountDTO.AccountLogin.Email,
                FirstName = firstName,
                LastName = lastName,
                Gender = gender,
                ProfilePicture = accountDTO.ImageUrl,
                Mobile = accountDTO.MobileNumber,

            };
            var lga = _context.Lgas.FirstOrDefault(x => x.Id == accountDTO.LGAId).Name;
            var state = _context.States.FirstOrDefault(x => x.Id == accountDTO.StateId).Name;

            var supplier = new Supplier
            {
                ImageUrl = accountDTO.ImageUrl,
                PrimaryContactGender = accountDTO.PrimaryContactGender,
                PrimaryContactEmail = accountDTO.PrimaryContactEmail,
                PrimaryContactMobile = accountDTO.PrimaryContactMobile,
                Lgaid = accountDTO.LGAId,
                PrimaryContactName = accountDTO.PrimaryContactName,
                MobileNumber = accountDTO.MobileNumber,
                SupplierName = accountDTO.SupplierName,
                Description = accountDTO.SupplierName,
                SupplierEmail = accountDTO.SupplierName,
                StateId = accountDTO.StateId,
                UpdatedAt = DateTime.UtcNow.AddHours(1),
                CreatedAt = DateTime.UtcNow.AddHours(1),
                CreatedById = createdBy,
                SupplierCategoryId = accountDTO.SupplierCategoryId,
                Street = accountDTO.Street,
                Address = accountDTO.Street + " " + accountDTO.State,
            };

            var (salt, hashed) = HashPassword(new byte[] { }, accountDTO.AccountLogin.Password);

            var onlinProfile = new OnlineProfile
            {
                Email = accountDTO.AccountLogin.Email,
                NormalizedEmail = accountDTO.AccountLogin.Email.ToUpper(),
                PasswordHash = hashed,
                SecurityStamp = Convert.ToBase64String(salt),
                Name = firstName + " " + lastName,
                CreatedAt = DateTime.UtcNow.AddHours(1),
            };

            try
            {

                _context.Contacts.Add(contact);
                await _context.SaveChangesAsync();

                _context.Suppliers.Add(supplier);
                _context.SaveChanges();

                var suppliercontact = new SupplierContactMapping { ContactId = contact.Id, SupplierId = supplier.Id, CreatedAt = DateTime.UtcNow.AddHours(1), CreatedById = createdBy };
                _context.SupplierContactMappings.Add(suppliercontact);
                _context.SaveChanges();

                onlinProfile.SupplierId = supplier.Id;
                _context.OnlineProfiles.Add(onlinProfile);
                await _context.SaveChangesAsync();

                await GetServiceIncomeAccountForSupplier(supplier);

                await transaction.CommitAsync();

                return (true, "success");
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message);
                _logger.LogError(e.StackTrace);
                await transaction.RollbackAsync();
                return (false, "An error has occured");
            }

        }

        public async Task<(bool success, string message)> CreateSupplierIndividualAccount(SMSSupplierIndividualAccountDTO accountDTO)
        {
            var exist = _context.OnlineProfiles.Any(x => x.Email.ToLower() == accountDTO.AccountLogin.Email.ToLower());

            if (exist)
                return (false, "User already exists");

            var exist1 = _context.Suppliers.Any(x => x.SupplierEmail.ToLower() == accountDTO.AccountLogin.Email.ToLower());

            if (exist1)
                return (false, "User already exists");

            var createdBy = _context.UserProfiles.FirstOrDefault(x => x.Email.ToLower().Contains("online")).Id;
            var grouptype = _context.GroupTypes.FirstOrDefault(x => x.Caption.ToLower() == "individual").Id;
            var leadOrigin = _context.LeadOrigins.FirstOrDefault(x => x.Caption.ToLower() == "email").Id;
            var branchId = _context.Branches.FirstOrDefault(x => x.Name.ToLower().Contains("hq")).Id;
            var leadtype = _context.LeadTypes.FirstOrDefault(x => x.Caption.ToLower() == "rfq").Id;
            var office = _context.Offices.FirstOrDefault(x => x.Name.ToLower().Contains("office")).Id;
            //var receivableAcctId = 0;

            using var transaction = await _context.Database.BeginTransactionAsync();
            var gender = accountDTO.PrimaryContactGender == "M" ? Gender.Male : Gender.Female;

            var firstName = accountDTO.FirstName;
            var lastName = accountDTO.LastName;

            var contact = new Contact
            {
                CreatedAt = DateTime.UtcNow.AddHours(1),
                UpdatedAt = DateTime.UtcNow.AddHours(1),
                CreatedById = createdBy,
                Email = accountDTO.AccountLogin.Email,
                FirstName = firstName,
                LastName = lastName,
                Gender = gender,
                ProfilePicture = accountDTO.ImageUrl,
                Mobile = accountDTO.MobileNumber,

            };
            var lga = _context.Lgas.FirstOrDefault(x => x.Id == accountDTO.LGAId).Name;
            var state = _context.States.FirstOrDefault(x => x.Id == accountDTO.StateId).Name;

            var supplier = new Supplier
            {
                ImageUrl = accountDTO.ImageUrl,
                PrimaryContactGender = accountDTO.PrimaryContactGender,
                PrimaryContactEmail = accountDTO.PrimaryContactEmail,
                PrimaryContactMobile = accountDTO.PrimaryContactMobile,
                Lgaid = accountDTO.LGAId,
                PrimaryContactName = accountDTO.PrimaryContactName,
                MobileNumber = accountDTO.MobileNumber,
                SupplierName = accountDTO.FirstName + " " + accountDTO.LastName,
                Description = accountDTO.FirstName + " " + accountDTO.LastName,
                SupplierEmail = accountDTO.PrimaryContactEmail,
                StateId = accountDTO.StateId,
                UpdatedAt = DateTime.UtcNow.AddHours(1),
                CreatedAt = DateTime.UtcNow.AddHours(1),
                CreatedById = createdBy,
                SupplierCategoryId = accountDTO.SupplierCategoryId,
                Street = accountDTO.Street,
                Address = accountDTO.Street + " " + accountDTO.State
            };

            var (salt, hashed) = HashPassword(new byte[] { }, accountDTO.AccountLogin.Password);

            var onlinProfile = new OnlineProfile
            {
                Email = accountDTO.AccountLogin.Email,
                NormalizedEmail = accountDTO.AccountLogin.Email.ToUpper(),
                PasswordHash = hashed,
                SecurityStamp = Convert.ToBase64String(salt),
                Name = firstName + " " + lastName,
                CreatedAt = DateTime.UtcNow.AddHours(1),
            };

            try
            {
                _context.Contacts.Add(contact);
                await _context.SaveChangesAsync();

                _context.Suppliers.Add(supplier);
                _context.SaveChanges();

                var suppliercontact = new SupplierContactMapping { ContactId = contact.Id, SupplierId = supplier.Id, CreatedAt = DateTime.UtcNow.AddHours(1), CreatedById = createdBy };
                _context.SupplierContactMappings.Add(suppliercontact);
                _context.SaveChanges();

                onlinProfile.SupplierId = supplier.Id;
                _context.OnlineProfiles.Add(onlinProfile);
                await _context.SaveChangesAsync();

                await GetServiceIncomeAccountForSupplier(supplier);

                await transaction.CommitAsync();

                return (true, "success");
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message);
                _logger.LogError(e.StackTrace);
                await transaction.RollbackAsync();
                return (false, "An error has occured");
            }

        }

        public async Task<(bool success, string message)> UpdateCustomerProfile(ProfileUpdateDTO request)
        {

            using var transaction = await _context.Database.BeginTransactionAsync();
            var profile = _context.OnlineProfiles.FirstOrDefault(x => x.Id == request.profileId);
            try
            {
                if (profile == null)
                {
                    return (false, "Profile does not exist");
                }

                var leadDiv = _context.LeadDivisions.FirstOrDefault(x => x.Id == profile.LeadDivisionId);

                leadDiv.Email = String.IsNullOrEmpty(request.Email) ? leadDiv.Email : request.Email;
                leadDiv.LogoUrl = String.IsNullOrEmpty(request.ProfileImage) ? leadDiv.LogoUrl : request.ProfileImage;
                leadDiv.PhoneNumber = String.IsNullOrEmpty(request.PhoneNumber) ? leadDiv.PhoneNumber : request.PhoneNumber;
                leadDiv.UpdatedAt = DateTime.UtcNow.AddHours(1);
                leadDiv.StateId = request.StateId ?? leadDiv.StateId;
                leadDiv.Lgaid = request.LGAId ?? leadDiv.Lgaid;
                leadDiv.Street = String.IsNullOrEmpty(request.Street) ? leadDiv.Street : request.Street;
                _context.SaveChanges();


                var leadDivNew = _context.LeadDivisions.Include(x => x.State).Include(x => x.Lga).FirstOrDefault(x => x.Id == profile.LeadDivisionId);
                leadDiv.Address = leadDivNew.Street + ", " + leadDivNew.Lga.Name + ", " + leadDivNew.State.Name;
                _context.SaveChanges();

                var lead = _context.Leads.FirstOrDefault(x => x.Id == leadDiv.LeadId);

                lead.LogoUrl = request.ProfileImage;
                _context.SaveChanges();


                if (profile.CustomerDivisionId != null)
                {
                    var custDiv = _context.CustomerDivisions.FirstOrDefault(x => x.Id == profile.CustomerDivisionId);
                    custDiv.Email = String.IsNullOrEmpty(request.Email) ? custDiv.Email : request.Email;
                    custDiv.LogoUrl = String.IsNullOrEmpty(request.ProfileImage) ? custDiv.LogoUrl : request.ProfileImage;
                    custDiv.PhoneNumber = String.IsNullOrEmpty(request.PhoneNumber) ? custDiv.PhoneNumber : request.PhoneNumber;
                    custDiv.UpdatedAt = DateTime.UtcNow.AddHours(1);
                    _context.SaveChanges();

                    var cust = _context.Customers.FirstOrDefault(x => x.Id == custDiv.CustomerId);
                    cust.Email = String.IsNullOrEmpty(request.Email) ? cust.Email : request.Email;
                    cust.LogoUrl = String.IsNullOrEmpty(request.ProfileImage) ? cust.LogoUrl : request.ProfileImage;
                    cust.PhoneNumber = String.IsNullOrEmpty(request.PhoneNumber) ? cust.PhoneNumber : request.PhoneNumber;
                    cust.UpdatedAt = DateTime.UtcNow.AddHours(1);
                    _context.SaveChanges();

                }

                profile.Email = String.IsNullOrEmpty(request.Email) ? profile.Email : request.Email;
                profile.NormalizedEmail = String.IsNullOrEmpty(request.Email) ? profile.NormalizedEmail : request.Email.ToUpper();
                _context.SaveChanges();

                await transaction.CommitAsync();
                return (true, "successful");

            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                _logger.LogError(ex.StackTrace);
                await transaction.RollbackAsync();
            }
            return (false, "failed updating");
        }

        public async Task<long> GetServiceIncomeAccountForSupplier(Supplier profile)
        {
            var controlAccount = _configuration["SupplierControlAccountID"] ?? _configuration.GetSection("AppSettings:SupplierControlAccountID").Value;
            var controlAccountId = long.Parse(controlAccount);
            var acctName = $"Supplier Service Income Account for {profile.SupplierName.ToUpper()}";

            var incomeAcct = _context.Accounts.FirstOrDefault(x => x.ControlAccountId == controlAccountId && x.Name == acctName);

            if (incomeAcct != null)
            {
                return incomeAcct.Id;
            }

            var createdBy = _context.UserProfiles.FirstOrDefault(x => x.Email.ToLower().Contains("online")).Id;



             var dtrack = await _customerInfoRepository.GetDtrackCustomerNumber(new CustomerDivision { DivisionName = profile.SupplierName });

            var account = new Account
            {
                ControlAccountId = controlAccountId,
                UpdatedAt = DateTime.UtcNow.AddHours(1),
                CreatedAt = DateTime.UtcNow.AddHours(1),
                CreatedById = createdBy,
                IsActive = true,
                Description = acctName,
                Name = acctName,
                IsDebitBalance = false,
                Alias = dtrack
            };

            var accountId = await SaveAccount(account);
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

               // _context.ChangeTracker.Clear();
                //remove exception throwing
                account.Alias = account.Alias ?? "";

                var savedAccount = await _context.Accounts.AddAsync(account);
                await _context.SaveChangesAsync();
             //   _context.ChangeTracker.Clear();
                return savedAccount.Entity.Id;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                throw;
            }

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
    }
}
