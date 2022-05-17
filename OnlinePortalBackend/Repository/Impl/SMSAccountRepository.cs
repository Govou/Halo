﻿using AutoMapper;
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
            var exist = _context.LeadDivisions.Any(x => x.Email.ToLower() == accountDTO.AccountLogin.Email.ToLower());

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

            var leadContact = new LeadContact
            {
                CreatedAt = DateTime.UtcNow.AddHours(1),
                UpdatedAt = DateTime.UtcNow.AddHours(1),
                CreatedById = createdBy,
                Email = accountDTO.AccountLogin.Email,
                FirstName = accountDTO.ContactPerson.FirstName,
                LastName = accountDTO.ContactPerson.LastName,
                Gender = accountDTO.ContactPerson.Gender,
                MobileNumber = accountDTO.ContactPerson.PhoneNumber,
                Type = (int)leadtype
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
                OfficeId = office,
                Rcnumber = accountDTO.RCNumber
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

                leadContact.LeadId = lead.Id;

                _context.LeadContacts.Add(leadContact);
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
            var exist = _context.LeadDivisions.Any(x => x.Email.ToLower() == accountDTO.AccountLogin.Email.ToLower());

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
            //var contact = new Contact
            //{
            //    CreatedAt = DateTime.UtcNow.AddHours(1),
            //    UpdatedAt = DateTime.UtcNow.AddHours(1),
            //    CreatedById = createdBy,
            //    Email = accountDTO.AccountLogin.Email,
            //    FirstName = accountDTO.FirstName,
            //    LastName = accountDTO.LastName,
            //    Gender = gender,
            //    ProfilePicture = accountDTO.ImageUrl,
            //    Mobile = accountDTO.PhoneNumber
            //};

            var suspect = new Suspect
            {
                Address = accountDTO.Address,
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
                LastName= accountDTO.LastName
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
                Address = accountDTO.Address,
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

            var leadContact = new LeadContact
            {
                CreatedAt = DateTime.UtcNow.AddHours(1),
                UpdatedAt = DateTime.UtcNow.AddHours(1),
                CreatedById = createdBy,
                Email = accountDTO.AccountLogin.Email,
                FirstName = accountDTO.FirstName,
                LastName = accountDTO.LastName,
                Gender = accountDTO.Gender,
                MobileNumber = accountDTO.PhoneNumber,
                Type = (int)leadtype
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
                EmailConfirmed = true,
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

                leadContact.LeadId = lead.Id;

                _context.LeadContacts.Add(leadContact);
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
    }
}