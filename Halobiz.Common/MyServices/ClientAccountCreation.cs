using Halobiz.Common.DTOs.ApiDTOs;
using Halobiz.Common.DTOs.ReceivingDTO;
using Halobiz.Common.DTOs.ReceivingDTOs;
using HalobizMigrations.Data;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;

namespace Halobiz.Common.MyServices
{
    public class ClientAccountCreation
    {
        private IUserProfileService _userProfileService;
        private readonly HalobizContext _context;
        private readonly ILogger<ClientAccountCreation> _logger;
        public ClientAccountCreation(IUserProfileService userProfileService,
            ILogger<ClientAccountCreation> logger)
        {
            _userProfileService = userProfileService;
            _logger = logger;
        }

        public async Task<ApiCommonResponse> CreateAccount(UserProfileReceivingDTO user)
        {
            try
            {
                var response = await _userProfileService.FindUserByEmail(user.Email);

                if (!response.responseCode.Contains("00"))
                {
                    return CommonResponse.Send(ResponseCodes.FAILURE, null, "You already have a profile");
                }

                //check if this customer division has an email
                if (!_context.CustomerDivisions.Any(x=>x.Email==user.Email))
                {
                    return CommonResponse.Send(ResponseCodes.FAILURE, null, $"This email {user.Email} does not exist for a customer");
                }

                //check if the security code for this guy has been used


                //hash password for this guy and create profile
                await _userProfileService.AddUserProfile(new UserProfileReceivingDTO
                {
                    Email = user.Email,
                    EmailConfirmed = true,
                    NormalizedEmail = user.Email.ToUpper(),
                    PasswordHash = HashPassword(user.Password),
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    ImageUrl = "",
                    StaffId = 0,
                    DateOfBirth = DateTime.Now.ToString("yyyy-MM-dd"),
                });

                //send code to the client
                return CommonResponse.Send(ResponseCodes.SUCCESS, null, $"A confirmation code has been send to {user.Email}");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.StackTrace);
                return CommonResponse.Send(ResponseCodes.FAILURE, null, "System error");
            }
        }

        private string HashPassword(string password)
        {
            // generate a 128-bit salt using a cryptographically strong random sequence of nonzero values
            byte[] salt = new byte[128 / 8];
            using (var rngCsp = new RNGCryptoServiceProvider())
            {
                rngCsp.GetNonZeroBytes(salt);
            }

            // derive a 256-bit subkey (use HMACSHA256 with 100,000 iterations)
            string hashed = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                password: password,
                salt: salt,
                prf: KeyDerivationPrf.HMACSHA256,
                iterationCount: 100000,
                numBytesRequested: 256 / 8));

            return hashed;
        }

        
    }
}
