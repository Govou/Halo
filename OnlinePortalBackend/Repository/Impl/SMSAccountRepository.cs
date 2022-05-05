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
        public async Task<bool> CreateBusinessAccount(SMSBusinessAccountDTO accountDTO)
        {
            var createdBy = _context.UserProfiles.FirstOrDefault(x => x.Email.ToLower().Contains("online")).Id;
            var grouptype = _context.GroupTypes.FirstOrDefault(x => x.Caption.ToLower() == "sme").Id;
            var leadOrigin = _context.LeadOrigins.FirstOrDefault(x => x.Caption.ToLower() == "email").Id;
            var branchId = _context.Branches.FirstOrDefault(x => x.Name.ToLower().Contains("hq")).Id;
            var leadtype = _context.LeadTypes.FirstOrDefault(x => x.Caption.ToLower() == "rfq").Id;
            //var receivableAcctId = 0;

            using var transaction = await _context.Database.BeginTransactionAsync();
            //var primaryContact = new Contact
            //{
            //    CreatedAt = DateTime.UtcNow.AddHours(1),
            //    UpdatedAt = DateTime.UtcNow.AddHours(1),
            //    CreatedById = createdBy,
            //    Email = accountDTO.AccountLogin.Email,
            //    n
            //}
            var suspect = new Suspect
            {
                Address = accountDTO.Address,
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
              //  OfficeId = office,
                StateId = accountDTO.StateId,
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
                CustomerId = 0,
                GroupName = accountDTO.CompanyName,
                GroupTypeId = grouptype,
                Industry = accountDTO.Industry,
                LeadOriginId = leadOrigin,
                LeadTypeId = leadtype,
                LogoUrl = accountDTO.LogoUrl
            };

            var leadDivision = new LeadDivision
            {
                Address = accountDTO.Address,
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
            };

            var customer = new Customer
            {
                CreatedAt = DateTime.UtcNow.AddHours(1),
                CreatedById = createdBy,
                Email = accountDTO.AccountLogin.Email,
                UpdatedAt = DateTime.UtcNow.AddHours(1),
                LogoUrl = accountDTO.LogoUrl,
                GroupName = accountDTO.CompanyName,
                PhoneNumber = accountDTO.PhoneNumber,
                GroupTypeId = grouptype,
                Industry = accountDTO.Industry,
                Rcnumber = "NULL"
            };


            var custDivision = new CustomerDivision
            {
                Address = accountDTO.Address,
                CreatedAt = DateTime.UtcNow.AddHours(1),
                UpdatedAt = DateTime.UtcNow.AddHours(1),
               // ReceivableAccountId = receivableAcctId,
                Lgaid = accountDTO.LGAId,
                Industry = accountDTO.Industry,
                CreatedById = createdBy,
                Email = accountDTO.AccountLogin.Email,
                PhoneNumber = accountDTO.PhoneNumber,
                LogoUrl = accountDTO.LogoUrl,
                StateId = accountDTO.StateId,
                DivisionName = accountDTO.CompanyName
            };

            var (salt, hashed) = HashPassword(new byte[] { }, accountDTO.AccountLogin.Password);

            var onlinProfile = new OnlineProfile
            {
                Email = accountDTO.AccountLogin.Email,
                EmailConfirmed = true,
                NormalizedEmail = accountDTO.AccountLogin.Email.ToUpper(),
                PasswordHash = hashed,
                SecurityStamp = Convert.ToBase64String(salt),
                Name = accountDTO.CompanyName,
                CustomerDivisionId = customer.Id,
                CreatedAt = DateTime.UtcNow.AddHours(1),
            }; 

            try
            {
                _context.Suspects.Add(suspect);
                await _context.SaveChangesAsync();

                _context.Customers.Add(customer);
                await _context.SaveChangesAsync();

                lead.SuspectId = suspect.Id;
                lead.CustomerId = customer.Id;

                _context.Leads.Add(lead);
                await _context.SaveChangesAsync();

                leadReference.ReferenceNo = leadReference.ReferenceNo + 1;
                await UpdateReferenceNumber(leadReference);

                leadDivision.LeadId = lead.Id;

                _context.LeadDivisions.Add(leadDivision);
                await _context.SaveChangesAsync();

                customer.CustomerLeadId = leadDivision.Id;

                custDivision.CustomerId = customer.Id;
                custDivision.LeadDivisionId = leadDivision.Id;

                _context.CustomerDivisions.Add(custDivision);
                await _context.SaveChangesAsync();

                onlinProfile.CustomerDivisionId = custDivision.Id;

                _context.OnlineProfiles.Add(onlinProfile);
                await _context.SaveChangesAsync();

                await transaction.CommitAsync();

                return true;
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message);
                _logger.LogError(e.StackTrace);
                await transaction.RollbackAsync();
                return false;
            }


        }

        public async Task<bool> CreateIndividualAccount(SMSIndividualAccountDTO accountDTO)
        {
            throw new System.NotImplementedException();
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
    }
}
