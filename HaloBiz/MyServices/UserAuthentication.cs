using AutoMapper;
using Google.Apis.Auth;
using Halobiz.Common.Auths.PermissionParts;
using Halobiz.Common.DTOs.ApiDTOs;
using Halobiz.Common.DTOs.ReceivingDTO;
using Halobiz.Common.DTOs.ReceivingDTOs;
using Halobiz.Common.DTOs.ReceivingDTOs.RoleManagement;
using Halobiz.Common.DTOs.TransferDTOs;
using Halobiz.Common.Model;
using Halobiz.Common.MyServices;
using Halobiz.Common.MyServices.RoleManagement;
using Halobiz.MyServices;
using HaloBiz.DTOs.ReceivingDTOs;
using HaloBiz.DTOs.TransferDTOs;
using HaloBiz.Helpers;
using HaloBiz.Model;
using HaloBiz.Models;
using HalobizMigrations.Data;
using HalobizMigrations.Models;
using HalobizMigrations.Models.Halobiz;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace HaloBiz.MyServices
{
    public interface IUserAuthentication
    {
        Task<ApiCommonResponse> CreatePassword(CreatePasswordDTO user);
        Task<ApiCommonResponse> UpdatePassword(UpdatePassworddDTO user);
        Task<ApiCommonResponse> Login(LoginDTO user);
        Task<ApiCommonResponse> GoogleLogin(GoogleLoginReceivingDTO loginReceiving);
        Task<ApiCommonResponse> CreateProfile(AuthUserProfileReceivingDTO authUserProfileReceivingDTO);
        Task<ApiCommonResponse> RevokeToken(RefreshTokenDTO token);

    }

    public class UserAuthentication : IUserAuthentication
    {
        private readonly IUserProfileService _userProfileService;
        private readonly ILogger<UserAuthentication> _logger;
        private readonly IJwtHelper _jwttHelper;
        private readonly IRoleService _roleService;
        private readonly IMapper _mapper;
        private readonly HalobizContext _context;
        private readonly IMemoryCache _memoryCache;
        private readonly List<string> _allowedDomains;
        private readonly IConfiguration _configuration;


        public UserAuthentication(IUserProfileService userProfileService,
            IJwtHelper jwtHelper,
            IMapper mapper,
            IRoleService roleService,
            HalobizContext context,
            IMemoryCache cache,
            ILogger<UserAuthentication> logger,
            IConfiguration config)
        {
            _userProfileService = userProfileService;
            _logger = logger;
            _jwttHelper = jwtHelper;
            _mapper = mapper;
            _roleService = roleService;            
            _context = context;
            _memoryCache = cache;
            _configuration = config;
            _allowedDomains = config.GetSection("AllowedLoginDomains").Get<List<string>>();
        }

      
        /// <summary>
        /// Login with google token
        /// </summary>
        /// <param name="loginReceiving"></param>
        /// <returns></returns>
        public async Task<ApiCommonResponse> GoogleLogin(GoogleLoginReceivingDTO loginReceiving)
        {
            try
            {
                GoogleJsonWebSignature.Payload payload;

                try
                {
                    payload = await GoogleJsonWebSignature.ValidateAsync(loginReceiving.IdToken);
                    if(!_allowedDomains.Contains(payload.HostedDomain))
                    {
                        return CommonResponse.Send(ResponseCodes.FAILURE, null, "Your hosted domain is not allowed to access this site");
                    }
                }
                catch (InvalidJwtException invalidJwtException)
                {
                    _logger.LogWarning($"Could not validate Google Id Token [{loginReceiving.IdToken}] => {invalidJwtException.Message}");
                    return CommonResponse.Send(ResponseCodes.FAILURE, null, invalidJwtException.Message);
                }

                if (!payload.EmailVerified)
                {
                    return CommonResponse.Send(ResponseCodes.FAILURE, null, "Your email has not been verified.");
                }

                var email = payload.Email;

                var userProfile = await _context.UserProfiles.Where(x=>x.Email==email).FirstOrDefaultAsync();
                if (userProfile == null)
                {
                    return await CreateNewProfile(email);
                }
                              

                //get the permissions of the user
                var (permissions, roleList) = await _roleService.GetPermissionEnumsOnUser(userProfile.Id);
                var hasAdminRole = await UserHasAdminRole(userProfile.Id);

                var (jwtToken, jwtLifespan) = _jwttHelper.GenerateToken(new UserProfile { Id = userProfile.Id, Email = userProfile.Email }, permissions, hasAdminRole);
               
                //get a refresh token for this user
                var refreshToken = GenerateRefreshToken(userProfile.Id);

                var responseCode = ResponseCodes.SUCCESS;
                if (string.IsNullOrEmpty(userProfile.MobileNumber))
                    responseCode = ResponseCodes.CREATE_PROFILE; 

                return CommonResponse.Send(responseCode, new UserAuthTransferDTO
                {
                    Token = jwtToken,
                    JwtLifespan = jwtLifespan,
                    RefreshToken = refreshToken,
                    Roles = roleList,
                    UserProfile = _mapper.Map<UserProfileTransferDTO>(userProfile)
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                _logger.LogError(ex.StackTrace);
                return CommonResponse.Send(ResponseCodes.FAILURE, null, ex.Message);
            }
        }

        private async Task<bool> UserHasAdminRole(long userId)
        {
            var adminInfo = _configuration.GetSection("AdminRoleInformation")?.Get<AdminRole>();
            var adminRole = await _context.Roles.Where(x => x.Name == adminInfo.RoleName).FirstOrDefaultAsync();
            return _context.UserRoles.Any(x => x.UserId == userId && x.RoleId == adminRole.Id);

        }
        private async Task<ApiCommonResponse> CreateNewProfile(string email)
        {
            var names = email.Split('@')[0].Split('.');
            var firstName = System.Globalization.CultureInfo.CurrentCulture.TextInfo.ToTitleCase(names[0]);
            var lastName = "";
            if(names.Length > 1)
            {
                lastName = System.Globalization.CultureInfo.CurrentCulture.TextInfo.ToTitleCase(names[1]);
            }

            var profile = new UserProfileReceivingDTO {
                Email = email,
                DateOfBirth = DateTime.Now.ToString("yyyy-MM-dd"),
                FirstName = firstName,
                LastName = lastName,
                ImageUrl = "",
                EmailConfirmed = true,                
            };

            var response = await _userProfileService.AddUserProfile(profile);

            if (!response.responseCode.Contains("00"))
            {
                _logger.LogWarning($"Could not create user [{profile.Email}] => {response.responseMsg}");
                return CommonResponse.Send(ResponseCodes.FAILURE, null, response.responseMsg);
            }

            var user = response.responseData;
            var userProfile = (UserProfileTransferDTO)user;

            await CreateAdminRoleAssignment(userProfile.Id);

            //get the permissions of the user
            var (permissions, roleList) = await _roleService.GetPermissionEnumsOnUser(userProfile.Id);
            var hasAdminRole = await UserHasAdminRole(userProfile.Id);

            var (jwtToken, jwtLifespan) = _jwttHelper.GenerateToken(new UserProfile { Id = userProfile.Id, Email = userProfile.Email }, permissions, hasAdminRole);

            //get a refresh token for this user
            var refreshToken = GenerateRefreshToken(userProfile.Id);
            return CommonResponse.Send(ResponseCodes.CREATE_PROFILE, new UserAuthTransferDTO
            {
                Token = jwtToken,
                JwtLifespan = jwtLifespan,
                RefreshToken = refreshToken,
                Roles = roleList,
                UserProfile = userProfile
            });
        }

        private async Task<bool> CreateAdminRoleAssignment(long userProfileId)
        {
            try
            {
                var userProfile = await _context.UserProfiles.FindAsync(userProfileId);

                var adminInfo = _configuration.GetSection("AdminRoleInformation")?.Get<AdminRole>();

                //check if this user is whitelisted for admin privilege
                if (!adminInfo.RoleAssignees.Contains(userProfile.Email, StringComparer.OrdinalIgnoreCase))
                    return false;

                //get seeder profile
                var seeder = await _context.UserProfiles.Where(x => x.Email.ToLower().Contains("seeder")).FirstOrDefaultAsync();
                if (seeder == null)
                    throw new Exception("No seeder profile created");

                //check if the role exist
                var adminRole = await _context.Roles.Where(x => x.Name == adminInfo.RoleName).FirstOrDefaultAsync();
                if (adminRole == null)
                {
                    List<Permissions> initialPermissions = new List<Permissions>() { Permissions.NotSet };
                    var role = new RoleTemp(adminInfo.RoleName, adminInfo.RoleDescription, initialPermissions);
                    var roleEntity = await _context.Roles.AddAsync(role);
                    await _context.SaveChangesAsync();
                    adminRole = roleEntity.Entity;
                }

                await _context.UserRoles.AddAsync(new UserRole
                {
                    RoleId = adminRole.Id,
                    UserId = userProfile.Id,
                    CreatedAt = DateTime.Now,
                    UpdatedAt = DateTime.Now,
                    CreatedById = seeder.Id
                });

                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw;
            }

            return true;
        }

        /// <summary>
        /// CreateProfile
        /// </summary>
        /// <param name="authUserProfileReceivingDTO"></param>
        /// <returns></returns>
        public async Task<ApiCommonResponse> CreateProfile(AuthUserProfileReceivingDTO authUserProfileReceivingDTO)
        {
            try
            {
                GoogleJsonWebSignature.Payload payload;

                try
                {
                    payload = await GoogleJsonWebSignature.ValidateAsync(authUserProfileReceivingDTO.IdToken);
                }
                catch (InvalidJwtException invalidJwtException)
                {
                    _logger.LogWarning(JsonConvert.SerializeObject(authUserProfileReceivingDTO));
                    _logger.LogWarning($"Could not validate Google Id Token [{authUserProfileReceivingDTO.IdToken}] => {invalidJwtException.Message}");
                    return CommonResponse.Send(ResponseCodes.FAILURE, null, invalidJwtException.Message);
                }

                if (!_allowedDomains.Contains(payload.HostedDomain))
                {
                    return CommonResponse.Send(ResponseCodes.FAILURE, null, "Your hosted domain is not allowed to access this site");
                }

                if (!payload.EmailVerified)
                {
                    _logger.LogWarning($"Email verification failed. Payload => {JsonConvert.SerializeObject(payload)}");
                    return CommonResponse.Send(ResponseCodes.FAILURE, null, "Email verification failed.");
                }

                var userProfileDTO = authUserProfileReceivingDTO.UserProfile;
                var response = await _userProfileService.AddUserProfile(userProfileDTO);

                if (!response.responseCode.Contains("00"))
                {
                    _logger.LogWarning($"Could not create user [{userProfileDTO.Email}] => {response.responseMsg}");
                    return CommonResponse.Send(ResponseCodes.FAILURE, null, response.responseMsg);

                }

                var user = response.responseData;
                var userProfile = (UserProfile)user;               

                //get the permissions of the user
                var (permissions, roleList) = await _roleService.GetPermissionEnumsOnUser(userProfile.Id);
                var hasAdminRole = await UserHasAdminRole(userProfile.Id);

                var (jwtToken, jwtLifespan) = _jwttHelper.GenerateToken(userProfile, permissions, hasAdminRole);

                //get a refresh token for this user
                var refreshToken = GenerateRefreshToken(userProfile.Id);
                return CommonResponse.Send(ResponseCodes.SUCCESS, new UserAuthTransferDTO
                {
                    Token = jwtToken,
                    JwtLifespan = jwtLifespan,
                    RefreshToken = refreshToken,
                    Roles = roleList,
                    UserProfile = _mapper.Map<UserProfileTransferDTO>(userProfile)
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                _logger.LogError(ex.StackTrace);
                return CommonResponse.Send(ResponseCodes.FAILURE, null, ex.Message);
            }
        }       

        private string GenerateRefreshToken(long UserId)
        {
            using (var rngCryptoServiceProvider = new RNGCryptoServiceProvider())
            {
                var randomBytes = new byte[64];
                rngCryptoServiceProvider.GetBytes(randomBytes);
                var token = new HalobizMigrations.Models.Halobiz.RefreshToken
                {
                    Token = Convert.ToBase64String(randomBytes),
                    Expires = DateTime.Now.AddDays(7),
                    CreatedAt = DateTime.Now,
                    AssignedTo = UserId
                };

                _context.RefreshTokens.Add(token);
                _context.SaveChanges();
                return token.Token;
            }
        }

        public async Task<ApiCommonResponse> RevokeToken(RefreshTokenDTO token)
        {
            try
            {
                var tokenRecord = _context.RefreshTokens.SingleOrDefault(t => t.Token == token.Token);

                // return  if no user found with token
                if (tokenRecord == null)
                    return CommonResponse.Send(ResponseCodes.FAILURE, null, "No user with token");

                // return  if token is not active
                var mTokenRecord = _mapper.Map<mRefreshToken>(tokenRecord);
                if (!mTokenRecord.IsActive)
                    return CommonResponse.Send(ResponseCodes.TOKEN_INACTIVE);

                // revoke token and save
                tokenRecord.Revoked = DateTime.Now;
                _context.RefreshTokens.Update(tokenRecord);
                await _context.SaveChangesAsync();

                return CommonResponse.Send(ResponseCodes.SUCCESS);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in Revoking Token");
                return CommonResponse.Send(ResponseCodes.FAILURE, null, ex.Message);
            }
        }

       

        private bool LockAccount(string email)
        {
            if (!_memoryCache.TryGetValue<LoginFailureTracker>(email, out LoginFailureTracker tracker))
            {
                tracker = new LoginFailureTracker
                {
                    Email = email,
                    LockedExpiration = DateTime.Now.AddMinutes(5)
                };

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

        public async Task<ApiCommonResponse> CreatePassword(CreatePasswordDTO user)
        {
            try
            {
                var userProfile = await _context.UserProfiles.Where(x => x.Email == user.Email).FirstOrDefaultAsync();

                if (userProfile == null)
                {
                    return CommonResponse.Send(ResponseCodes.FAILURE, null, $"You need login with Google first and create a profile on Halobiz");
                }

                //check if this customer division has an email
                if (userProfile.PasswordHash != null && userProfile.SecurityStamp != null)
                {
                    return CommonResponse.Send(ResponseCodes.FAILURE, null, $"You have already created password previously");
                }

                if (user.Password != user.ConfirmPassword)
                {
                    return CommonResponse.Send(ResponseCodes.FAILURE, null, $"Confirm password does not match password");
                }

                var (salt, hashed) = HashPassword(new byte[] { }, user.Password);
                userProfile.PasswordHash = hashed;
                userProfile.SecurityStamp = Convert.ToBase64String(salt);
                userProfile.UpdatedAt = DateTime.Now;

                //hash password for this guy and create profile
                var profileResult =  _context.UserProfiles.Update(userProfile);
                await _context.SaveChangesAsync();

                return CommonResponse.Send(ResponseCodes.SUCCESS, null, $"Password successfully created");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.StackTrace);
                return CommonResponse.Send(ResponseCodes.FAILURE, null, ex.Message);
            }
        }

        public async Task<ApiCommonResponse> UpdatePassword(UpdatePassworddDTO user)
        {
            try
            {
                var userProfile = await _context.UserProfiles.Where(x => x.Email == user.Email).FirstOrDefaultAsync();

                if (userProfile == null)
                {
                    return CommonResponse.Send(ResponseCodes.FAILURE, null, $"You need login with Google first and create a profile on Halobiz");
                }

                //check if this customer division has an email
                if (userProfile.PasswordHash == null && userProfile.SecurityStamp == null)
                {
                    return CommonResponse.Send(ResponseCodes.FAILURE, null, $"You did not have password previously. Please create");
                }

                if(user.Password != user.ConfirmPassword)
                {
                    return CommonResponse.Send(ResponseCodes.FAILURE, null, $"Confirm password does not match password");
                }

                if(!IsPreviousPasswordValid(userProfile, user.CurrentPassword))
                {
                    return CommonResponse.Send(ResponseCodes.FAILURE, null, $"Current password supplied is invalid");
                }

                var (salt, hashed) = HashPassword(new byte[] { }, user.Password);
                userProfile.PasswordHash = hashed;
                userProfile.SecurityStamp = Convert.ToBase64String(salt);
                userProfile.UpdatedAt = DateTime.Now;

                //hash password for this guy and create profile
                var profileResult = _context.UserProfiles.Update(userProfile);
                await _context.SaveChangesAsync();

                return CommonResponse.Send(ResponseCodes.SUCCESS, null, $"Password successfully updated");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.StackTrace);
                return CommonResponse.Send(ResponseCodes.FAILURE, null, ex.Message);
            }
        }

        private bool IsPreviousPasswordValid(UserProfile user, string previousPassword)
        {
            var byteSalt = Convert.FromBase64String(user.SecurityStamp);
            var (salt, hashed) = HashPassword(byteSalt, previousPassword);
            if (!_context.UserProfiles.Any(x => x.Email == user.Email && x.PasswordHash == hashed))
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// Login with username and password
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public async Task<ApiCommonResponse> Login(LoginDTO user)
        {
            try
            {
                var profile = await _context.UserProfiles.Where(x => x.Email == user.Email).FirstOrDefaultAsync();
                if (profile == null)
                {
                    return CommonResponse.Send(ResponseCodes.FAILURE, null, "No user with this email");
                }

                //check if this account is locked
                var (isLocked, timeLeft) = IsAccountLocked(user.Email);
                if (isLocked)
                {
                    return CommonResponse.Send(ResponseCodes.FAILURE, null, $"Account locked. Please try again in {timeLeft.ToString("#.##")} minutes");
                }

                var byteSalt = Convert.FromBase64String(profile.SecurityStamp);
                var (salt, hashed) = HashPassword(byteSalt, user.Password);
                if (!_context.UserProfiles.Any(x => x.Email == user.Email && x.PasswordHash == hashed))
                {
                    profile.AccessFailedCount = ++profile.AccessFailedCount;
                    if (profile.AccessFailedCount >= 3)
                    {
                        LockAccount(user.Email);
                    }

                    await _context.SaveChangesAsync();
                    return CommonResponse.Send(ResponseCodes.FAILURE, null, "Username or password is incorrect");
                }

                //get the permissions of the user
                var (permissions, roles) = await _roleService.GetPermissionEnumsOnUser(profile.Id);
                var hasAdminRole = await UserHasAdminRole(profile.Id);
                var (jwtToken, jwtLifespan) = _jwttHelper.GenerateToken(profile, permissions, hasAdminRole);

                //get a refresh token for this user
                var refreshToken = GenerateRefreshToken(profile.Id);
                return CommonResponse.Send(ResponseCodes.SUCCESS, new UserAuthTransferDTO
                {
                    Token = jwtToken,
                    JwtLifespan = jwtLifespan,
                    RefreshToken = refreshToken,
                    Roles = roles,
                    UserProfile = _mapper.Map<UserProfileTransferDTO>(profile)
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
