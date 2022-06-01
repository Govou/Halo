using AutoMapper;
using Halobiz.Common.DTOs.ApiDTOs;
using Halobiz.Common.DTOs.ReceivingDTO;
using Halobiz.Common.DTOs.ReceivingDTOs;
using Halobiz.Common.DTOs.TransferDTOs;
using OnlinePortalBackend.Helpers;
using HalobizMigrations.Data;
using HalobizMigrations.Models;
using HalobizMigrations.Models.OnlinePortal;
using HalobizMigrations.Models.Shared;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
using OnlinePortalBackend.DTOs.TransferDTOs;
using Microsoft.Extensions.Caching.Memory;
using Halobiz.Common.Model;

namespace OnlinePortalBackend.MyServices
{
    public interface IOnlineAccounts
    {
        Task<ApiCommonResponse> SendConfirmCodeToClient(string Email);
        Task<ApiCommonResponse> SendConfirmCodeToClient_v2(string Email);
        Task<ApiCommonResponse> CreateAccount(CreatePasswordDTO user);
        Task<ApiCommonResponse> Login(LoginDTO user);
        Task<ApiCommonResponse> Login_v2(LoginDTO user);
        Task<ApiCommonResponse> VerifyCode(CodeVerifyModel model);
    }

    public class OnlineAccounts : IOnlineAccounts
    {
        private readonly IMailService _mailService;
        private readonly HalobizContext _context;
        private readonly ILogger<OnlineAccounts> _logger;
        private readonly JwtHelper _jwttHelper;
        private readonly IMapper _mapper;
        private readonly IMemoryCache _memoryCache;


        public OnlineAccounts(IMailService mailService, 
            IMemoryCache memoryCache,
            JwtHelper jwtHelper,
            IMapper mapper,
            ILogger<OnlineAccounts> logger,
            HalobizContext context
         )
        {
            _mailService = mailService;
            _logger = logger;
            _mapper = mapper;
            _jwttHelper = jwtHelper;
            _context = context;
            _memoryCache = memoryCache;
        }
      

        public async Task<ApiCommonResponse> SendConfirmCodeToClient(string Email)
        {
            try
            {
                var response = await _context.OnlineProfiles.Where(x=>x.Email==Email).FirstOrDefaultAsync();

                if (response != null)
                {
                    return CommonResponse.Send(ResponseCodes.FAILURE, null, "You already have a profile");
                }

                //check if this customer division has an email
                if (!_context.CustomerDivisions.Any(x => x.Email == Email))
                {
                    return CommonResponse.Send(ResponseCodes.EMAIL_NOT_EXIST, null, $"This email {Email} does not exist for a customer");
                }

                if (_context.UsersCodeVerifications.Any(x => x.Email == Email && x.CodeExpiryTime >= DateTime.Now && x.CodeUsedTime == null))
                {
                    return CommonResponse.Send(ResponseCodes.DUPLICATE_REQUEST, null, $"The code for {Email} has not been used");
                }

                //save security code for this guy
                var code = await GenerateCode();
                var codeModel = new UsersCodeVerification
                {
                    Email = Email,
                    CodeExpiryTime = DateTime.UtcNow.AddHours(1).AddMinutes(10),
                    Code = code,
                    Purpose = CodePurpose.Onboarding
                };

                var entity = await _context.UsersCodeVerifications.AddAsync(codeModel);
                await _context.SaveChangesAsync();

                List<string> detail = new List<string>();
                detail.Add($"Your verification code for the online portal is <strong>{code}</strong>. Please note that it expires it 10 minutes");

                //send email with the code
                var request = new OnlinePortalDTO
                {
                    Recepients = new string[] { Email },
                    Name = "",
                    Salutation = "Hi",
                    Subject = "Confirmation code for Account Creation",
                    DetailsInPara = detail
                };

                var mailresponse = await _mailService.ConfirmCodeSending(request);
                mailresponse.responseData = $"Verification Code has been sent to {Email}";
                return mailresponse;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.StackTrace);
                return CommonResponse.Send(ResponseCodes.FAILURE, null, ex.Message);
            }          
        }
        public async Task<ApiCommonResponse> SendConfirmCodeToClient_v2(string Email)
        {
            try
            {
                var response = await _context.OnlineProfiles.Where(x=>x.Email==Email).FirstOrDefaultAsync();

                //check if this customer division has an email
                if (!_context.LeadDivisions.Any(x => x.Email == Email))
                {
                    return CommonResponse.Send(ResponseCodes.EMAIL_NOT_EXIST, null, $"This email {Email} does not exist for a lead");
                }

                if (_context.UsersCodeVerifications.Any(x => x.Email == Email && x.CodeExpiryTime >= DateTime.Now && x.CodeUsedTime == null))
                {
                    return CommonResponse.Send(ResponseCodes.DUPLICATE_REQUEST, null, $"The code for {Email} has not been used");
                }

                //save security code for this guy
                var code = await GenerateCode();
                var codeModel = new UsersCodeVerification
                {
                    Email = Email,
                    CodeExpiryTime = DateTime.UtcNow.AddHours(1).AddMinutes(10),
                    Code = code,
                    Purpose = CodePurpose.Onboarding
                };

                var entity = await _context.UsersCodeVerifications.AddAsync(codeModel);
                await _context.SaveChangesAsync();

                List<string> detail = new List<string>();
                detail.Add($"Your verification code for the online portal is <strong>{code}</strong>. Please note that it expires it 10 minutes");

                //send email with the code
                var request = new OnlinePortalDTO
                {
                    Recepients = new string[] { Email },
                    Name = "",
                    Salutation = "Hi",
                    Subject = "Confirmation code for Account Creation",
                    DetailsInPara = detail
                };

                var mailresponse = await _mailService.ConfirmCodeSending(request);
                mailresponse.responseData = $"Verfication Code has been sent to {Email}";
                return mailresponse;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.StackTrace);
                return CommonResponse.Send(ResponseCodes.FAILURE, null, ex.Message);
            }          
        }

        public async Task<ApiCommonResponse> VerifyCode(CodeVerifyModel model)
        {
            try
            {
                var codModel = _context.UsersCodeVerifications.Where(x => x.Email == model.Email && x.CodeExpiryTime >= DateTime.Now && x.Code == model.Code && x.Purpose==CodePurpose.Onboarding).FirstOrDefault();
                if (codModel == null)
                {
                    return CommonResponse.Send(ResponseCodes.FAILURE, null, $"The code for {model.Email} is invalid or expired");
                }

                codModel.CodeUsedTime = DateTime.Now;
                await _context.SaveChangesAsync();

                var profile = _context.OnlineProfiles.FirstOrDefault(x => x.Email == codModel.Email);
                if (profile != null)
                {
                    profile.EmailConfirmed = true;
                    await _context.SaveChangesAsync();
                }

                return CommonResponse.Send(ResponseCodes.SUCCESS, null, $"You have successfully used code for {model.Email}");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.StackTrace);
                return CommonResponse.Send(ResponseCodes.FAILURE, null, ex.Message);
            }
        }
        private async Task<string> GenerateCode()
        {
            string code = string.Empty;
            StringBuilder builder = new StringBuilder();
            Random random = new Random();

            while (builder.Length < 6)
            {
                int num = random.Next(0, 9);
                builder.Append(num);
            }

            code = builder.ToString();

            //check if this code exist on the system
            if (await _context.UsersCodeVerifications.AnyAsync(x => x.Code == code))
            {
                return await GenerateCode();
            }

            return code;
        }

        private bool LockAccount(string email)
        {
            if (!_memoryCache.TryGetValue<LoginFailureTracker>(email, out LoginFailureTracker tracker))
            {
                tracker = new LoginFailureTracker {
                    Email = email,
                    LockedExpiration = DateTime.Now.AddMinutes(5) };

                var cacheEntryOptions = new MemoryCacheEntryOptions()
                            .SetAbsoluteExpiration(TimeSpan.FromMinutes(5));
                _memoryCache.Set(email, tracker, cacheEntryOptions);
            }

            return true;
        }

        private (bool, double) IsAccountLocked(string email)
        {
            var lockedRecord = _memoryCache.Get<LoginFailureTracker>(email);
            return lockedRecord == null ? (false, 0) : (true, (lockedRecord.LockedExpiration - DateTime.Now).TotalMinutes);
        }
      
        public async Task<ApiCommonResponse> CreateAccount(CreatePasswordDTO user)
        {
            try
            {
                var response = await _context.OnlineProfiles.Where(x => x.Email == user.Email).FirstOrDefaultAsync();

                if (response != null)
                {
                    return CommonResponse.Send(ResponseCodes.FAILURE, null, $"This email {user.Email} already has a profile");
                }

                var customer = await _context.CustomerDivisions.Where(x => x.Email == user.Email).FirstOrDefaultAsync();

                //check if this customer division has an email
                if (customer == null)
                {
                    return CommonResponse.Send(ResponseCodes.FAILURE, null, $"This email {user.Email} does not exist for a customer");
                }

                //check if the security code for this guy has been used
                var code = await _context.UsersCodeVerifications.Where(x => x.Email == user.Email && x.CodeUsedTime != null).OrderByDescending(x=>x.Id).FirstOrDefaultAsync();
                if (code == null)
                {
                    return CommonResponse.Send(ResponseCodes.FAILURE, null, $"The email {user.Email} does not have verified code");
                }

                //check password complextity
                //at least 1 lower, at least 1 upper, at least 1 number, atleast 1 special char, at least 6 characters length
                var strongRegex = new Regex(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*[0-9])(?=.*[!@#\$%\^&\*])(?=.{6,})");
                if (!strongRegex.IsMatch(user.Password))
                {
                    return CommonResponse.Send(ResponseCodes.FAILURE, null, $"Weak password. Pasword rule is: at least 1 lower, at least 1 upper, at least 1 number, at least 1 special character, at least 6 characters length");
                }

                var (salt, hashed) = HashPassword(new byte[] { }, user.Password);
                var userpro = new OnlineProfile
                {
                    Email = user.Email,
                    EmailConfirmed = true,
                    NormalizedEmail = user.Email.ToUpper(),
                    PasswordHash = hashed,
                    SecurityStamp = Convert.ToBase64String(salt),                   
                    Name = customer.DivisionName,
                    Origin = user.Origin,
                    CustomerDivisionId = customer.Id,
                    CreatedAt = DateTime.Now
                };

                //hash password for this guy and create profile
                var profileResult = await _context.OnlineProfiles.AddAsync(userpro);
                await _context.SaveChangesAsync();

                return CommonResponse.Send(ResponseCodes.SUCCESS, null, $"Account successfully created");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.StackTrace);
                return CommonResponse.Send(ResponseCodes.FAILURE, null, ex.Message);
            }
        }

        public async Task<ApiCommonResponse> Login(LoginDTO user)
        {
            try
            {
                var profile = await _context.OnlineProfiles.Where(x => x.Email == user.Email).FirstOrDefaultAsync();
                if (profile == null)
                {
                    return CommonResponse.Send(ResponseCodes.FAILURE, null, "No user with this email");
                }

                if (!profile.EmailConfirmed)
                {
                    return CommonResponse.Send(ResponseCodes.FAILURE, null, "User yet to verify email");
                }

                //check if this account is locked
                var (isLocked, timeLeft) = IsAccountLocked(user.Email);
                if (isLocked)
                {
                    return CommonResponse.Send(ResponseCodes.FAILURE, null, $"Account locked. Please try again in {timeLeft.ToString("#.##")} minutes");
                }

                var byteSalt = Convert.FromBase64String(profile.SecurityStamp);
                var (salt, hashed) = HashPassword(byteSalt, user.Password);
                if (!_context.OnlineProfiles.Any(x => x.Email == user.Email && x.PasswordHash == hashed))
                {
                    profile.AccessFailedCount = ++profile.AccessFailedCount;
                    if(profile.AccessFailedCount >= 3)
                    {
                        LockAccount(user.Email);
                    }

                    await _context.SaveChangesAsync();

                    return CommonResponse.Send(ResponseCodes.FAILURE, null, "Username or password is incorrect");
                }                

                var jwtToken = _jwttHelper.GenerateToken(profile);

                var mappedProfile = _mapper.Map<OnlineProfileTransferDTO>(profile);

                //reset the access failed count
                profile.AccessFailedCount = 0;
                await _context.SaveChangesAsync();

                return CommonResponse.Send(ResponseCodes.SUCCESS, new
                {
                    Token = jwtToken,
                    UserProfile = mappedProfile
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.StackTrace);
                return CommonResponse.Send(ResponseCodes.FAILURE, null, ex.Message);
            }
        }

        public async Task<ApiCommonResponse> Login_v2(LoginDTO user)
        {
            try
            {
                var profile = await _context.OnlineProfiles.Where(x => x.Email == user.Email).FirstOrDefaultAsync();
                if (profile == null)
                {
                    return CommonResponse.Send(ResponseCodes.FAILURE, null, "No user with this email");
                }

                if (!profile.EmailConfirmed)
                {
                    return CommonResponse.Send(ResponseCodes.FAILURE, null, "User yet to verify email");
                }

                //check if this account is locked
                var (isLocked, timeLeft) = IsAccountLocked(user.Email);
                if (isLocked)
                {
                    return CommonResponse.Send(ResponseCodes.FAILURE, null, $"Account locked. Please try again in {timeLeft.ToString("#.##")} minutes");
                }

                var byteSalt = Convert.FromBase64String(profile.SecurityStamp);
                var (salt, hashed) = HashPassword(byteSalt, user.Password);
                if (!_context.OnlineProfiles.Any(x => x.Email == user.Email && x.PasswordHash == hashed))
                {
                    profile.AccessFailedCount = ++profile.AccessFailedCount;
                    if (profile.AccessFailedCount >= 3)
                    {
                        LockAccount(user.Email);
                    }

                    await _context.SaveChangesAsync();

                    return CommonResponse.Send(ResponseCodes.FAILURE, null, "Username or password is incorrect");
                }

                var jwtToken = _jwttHelper.GenerateToken(profile);

                var mappedProfile = new OnlineProfileTransferDetailDTO
                {
                    AccessFailedCount = profile.AccessFailedCount,
                    CustomerDivisionId = profile.CustomerDivisionId,
                    Email = user.Email,
                    LockoutEnabled = profile.LockoutEnabled,
                    Name = profile.Name,
                    Id = profile.Id,
                    
                };
                var contract = _context.Contracts.Where(x => x.CustomerDivisionId == profile.CustomerDivisionId).OrderByDescending(x => x.Id).FirstOrDefault();
                var profileContractDetails = new List<ProfileContractDetail>();

                if (contract != null)
                {
                    var profileContractDetail = new ProfileContractDetail { ContractId = contract.Id };
                    var profileContractServiceDetails = new List<ProfileContractServiceDetail>();
                    var contratcServices = _context.ContractServices.Where(x => x.ContractId == contract.Id).ToList();

                    foreach (var item in contratcServices)
                    {
                        profileContractServiceDetails.Add(new ProfileContractServiceDetail
                        {
                            ContractServiceId = item.Id,
                            ServiceId = item.ServiceId,
                        });
                    }
                    profileContractDetails.Add(new ProfileContractDetail
                    {
                        ContractId = contract.Id,
                        ContractServices = profileContractServiceDetails
                    });

                }




                mappedProfile.ProfileContractDetail = profileContractDetails;
                //reset the access failed count
                profile.AccessFailedCount = 0;
                await _context.SaveChangesAsync();

                return CommonResponse.Send(ResponseCodes.SUCCESS, new
                {
                    Token = jwtToken,
                    UserProfile = mappedProfile
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.StackTrace);
                return CommonResponse.Send(ResponseCodes.FAILURE, null, ex.Message);
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
       
    }
}
