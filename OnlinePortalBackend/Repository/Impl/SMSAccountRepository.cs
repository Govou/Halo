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

namespace OnlinePortalBackend.Repository.Impl
{
    public class SMSAccountRepository : ISMSAccountRepository
    {
        private readonly HalobizContext _context;
        private readonly ILogger<SMSAccountRepository> _logger;
        private readonly IMapper _mapper;
        public SMSAccountRepository(HalobizContext context,
            ILogger<SMSAccountRepository> logger,
            IMapper mapper)
        {
            _logger = logger;
            _context = context;
            _mapper = mapper;
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
                Rcnumber = accountDTO.RCNumber
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
                LastName= accountDTO.LastName,
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
                OfficeId = office
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

        public async Task<OnlineProfile> GetCustomerProfile(int profileId)
        {
            return _context.OnlineProfiles.FirstOrDefault(x => x.Id == profileId);
            
        }

        public async Task<(bool success, string message)> CreateSupplierAccount(SMSSupplierAccountDTO accountDTO)
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
                Description = accountDTO.Description,
                SupplierEmail = accountDTO.SupplierName,
                StateId = accountDTO.StateId,
                UpdatedAt = DateTime.UtcNow.AddHours(1),
                CreatedAt = DateTime.UtcNow.AddHours(1),
                CreatedById = createdBy,
                SupplierCategoryId = accountDTO.SupplierCategoryId,
                Street = accountDTO.Street,
                Address = accountDTO.Street + " " + accountDTO.State
            };

            var customer = new Customer
            {
                CreatedAt = DateTime.UtcNow.AddHours(1),
                UpdatedAt = DateTime.UtcNow.AddHours(1),
                CreatedById = createdBy,
                Email = accountDTO.PrimaryContactEmail,
                PhoneNumber = accountDTO.MobileNumber,
                GroupName = accountDTO.SupplierName
            };

            var custDiv = new CustomerDivision
            {
                Address = accountDTO.Street + " " + accountDTO.State,
                CreatedAt = DateTime.UtcNow.AddHours(1),
                UpdatedAt = DateTime.UtcNow.AddHours(1),
                Email = accountDTO.PrimaryContactEmail,
                Lgaid = accountDTO.LGAId,
                CreatedById = createdBy,
                DivisionName = accountDTO.PrimaryContactName,
                PhoneNumber = accountDTO.MobileNumber,
                StateId = accountDTO.StateId,
                Street = accountDTO.Street
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
                EmailConfirmed = true
            };

            try
            {
                _context.Customers.Add(customer);
                _context.SaveChanges();

                custDiv.CustomerId = customer.Id;
                _context.CustomerDivisions.Add(custDiv);
                
                _context.Contacts.Add(contact);
                await _context.SaveChangesAsync();

                _context.Suppliers.Add(supplier);
                _context.SaveChanges();

                onlinProfile.CustomerDivisionId = custDiv.Id;
                onlinProfile.SupplierId = supplier.Id;
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
    }
}
