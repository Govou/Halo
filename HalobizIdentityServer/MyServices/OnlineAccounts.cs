using AutoMapper;
using Halobiz.Common.DTOs.ApiDTOs;
using Halobiz.Common.DTOs.ReceivingDTO;
using Halobiz.Common.DTOs.ReceivingDTOs;
using Halobiz.Common.DTOs.TransferDTOs;
using Halobiz.Common.MyServices;
using Halobiz.Common.MyServices.RoleManagement;
using HalobizIdentityServer.Helpers;
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

namespace HalobizIdentityServer.MyServices
{
    public interface IOnlineAccounts
    {
        Task<ApiCommonResponse> SendConfirmCodeToClient(string Email);
        Task<ApiCommonResponse> CreateAccount(UserProfileReceivingDTO user);
        Task<ApiCommonResponse> Login(LoginDTO user);
        Task<ApiCommonResponse> VerifyCode(CodeVerifyModel model);
    }

    public class OnlineAccounts : IOnlineAccounts
    {
        private readonly IMailService _mailService;
        private IUserProfileService _userProfileService;
        private readonly HalobizContext _context;
        private readonly ILogger<OnlineAccounts> _logger;
        private readonly JwtHelper _jwttHelper;
        private readonly IMapper _mapper;
        private readonly IRoleService _roleService;


        public OnlineAccounts(IMailService mailService,
              IUserProfileService userProfileService,
            JwtHelper jwtHelper,
            IMapper mapper,
            IRoleService roleService,
            ILogger<OnlineAccounts> logger,
            HalobizContext context
         )
        {
            _mailService = mailService;
            _userProfileService = userProfileService;
            _logger = logger;
            _mapper = mapper;
            _roleService = roleService;
            _jwttHelper = jwtHelper;
            _context = context;

        }

        public async Task<ApiCommonResponse> SendConfirmCodeToClient(string Email)
        {
            try
            {
                var response = await _userProfileService.FindUserByEmail(Email);

                if (response.responseCode.Contains("00"))
                {
                    return CommonResponse.Send(ResponseCodes.FAILURE, null, "You already have a profile");
                }

                //check if this customer division has an email
                if (!_context.CustomerDivisions.Any(x => x.Email == Email))
                {
                    return CommonResponse.Send(ResponseCodes.FAILURE, null, $"This email {Email} does not exist for a customer");
                }

                if (!_context.UsersCodeVerifications.Any(x => x.Email == Email && x.CodeExpiryTime >= DateTime.Now && x.CodeUsedTime == null))
                {
                    return CommonResponse.Send(ResponseCodes.FAILURE, null, $"The code for {Email} has not been used");
                }

                //save security code for this guy
                var code = await GenerateCode();
                var codeModel = new UsersCodeVerification
                {
                    Email = Email,
                    CodeExpiryTime = DateTime.Now.AddMinutes(10),
                    Code = code,
                    Purpose = CodePurpose.Onboarding
                };

                var entity = await _context.UsersCodeVerifications.AddAsync(codeModel);
                await _context.SaveChangesAsync();

                var codeBody = (UsersCodeVerification)response.responseData;
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
                mailresponse.responseData = $"Code for {Email} is: {code}";
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

        public async Task<ApiCommonResponse> CreateAccount(UserProfileReceivingDTO user)
        {
            try
            {
                var response = await _userProfileService.FindUserByEmail(user.Email);

                if (response.responseCode.Contains("00"))
                {
                    return CommonResponse.Send(ResponseCodes.FAILURE, null, "You already have a profile");
                }

                //check if this customer division has an email
                if (!_context.CustomerDivisions.Any(x => x.Email == user.Email))
                {
                    return CommonResponse.Send(ResponseCodes.FAILURE, null, $"This email {user.Email} does not exist for a customer");
                }

                //check if the security code for this guy has been used
                if (!_context.UsersCodeVerifications.Any(x => x.Email == user.Email && x.CodeExpiryTime <= DateTime.Now && x.CodeUsedTime == null))
                {
                    return CommonResponse.Send(ResponseCodes.FAILURE, null, $"The code for {user.Email} has not been used");
                }

                var (salt, hashed) = HashPassword(new byte[] { }, user.Password);
                //hash password for this guy and create profile
                await _userProfileService.AddUserProfile(new UserProfileReceivingDTO
                {
                    Email = user.Email,
                    EmailConfirmed = true,
                    NormalizedEmail = user.Email.ToUpper(),
                    PasswordHash = hashed,
                    SecurityStamp = Convert.ToBase64String(salt),
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    ImageUrl = "",
                    StaffId = 0,
                    DateOfBirth = DateTime.Now.ToString("yyyy-MM-dd"),
                });

                //send code to the client
                return CommonResponse.Send(ResponseCodes.SUCCESS, null, $"A profile has been created for {user.Email}");
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
                var profile = _context.UserProfiles.Where(x => x.Email == user.Email).FirstOrDefault();
                if (profile == null)
                {
                    return CommonResponse.Send(ResponseCodes.FAILURE, null, "User has not been created");
                }

                var byteSalt = Convert.FromBase64String(profile.SecurityStamp);
                var (salt, hashed) = HashPassword(byteSalt, user.Password);
                if (!_context.UserProfiles.Any(x => x.Email == user.Email && x.PasswordHash == hashed))
                {
                    return CommonResponse.Send(ResponseCodes.FAILURE, null, "Invalid username or password");
                }
                
                //get the permissions of the user
                var permissions = await _roleService.GetPermissionEnumsOnUser(profile.Id);

                var jwtToken = _jwttHelper.GenerateToken(profile, permissions);

                var mappedProfile = _mapper.Map<UserProfileTransferDTO>(profile);
                return CommonResponse.Send(ResponseCodes.SUCCESS, new UserAuthTransferDTO
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
