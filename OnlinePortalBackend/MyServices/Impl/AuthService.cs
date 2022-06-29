using Halobiz.Common.Auths.PermissionParts;
using Halobiz.Common.DTOs.ApiDTOs;
using Halobiz.Common.DTOs.ReceivingDTOs.RoleManagement;
using Halobiz.Common.DTOs.TransferDTOs;
using HalobizMigrations.Data;
using HalobizMigrations.Models;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using OnlinePortalBackend.Helpers;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Claim = System.Security.Claims.Claim;

namespace OnlinePortalBackend.MyServices.Impl
{
    public class AuthService : IAuthService
    {
        private readonly IJwtHelper _jwtHelper;
        private readonly HalobizContext _context;
        private readonly IConfiguration _config;
        private readonly ILogger<AuthService> _logger;
        public AuthService(IJwtHelper jwtHeleper, HalobizContext context, IConfiguration config, ILogger<AuthService> logger)
        {
            _jwtHelper = jwtHeleper;
            _context = context; 
            _config = config;
            _logger = logger;
        }
      
        public async Task<ApiCommonResponse> GenerateToken(string email, string password)
        {

            try
            {
                var profile = await _context.UserProfiles.Where(x => x.Email == email).FirstOrDefaultAsync();
                if (profile == null)
                {
                    return CommonResponse.Send(ResponseCodes.FAILURE, null, "No user with this email");
                }

                //check if this account is locked
                //   var (isLocked, timeLeft) = IsAccountLocked(email);
                //if (isLocked)
                //{
                //    return CommonResponse.Send(ResponseCodes.FAILURE, null, $"Account locked. Please try again in {timeLeft.ToString("#.##")} minutes");
                //}

                var byteSalt = Convert.FromBase64String(profile.SecurityStamp);
                var (salt, hashed) = HashPassword(byteSalt, password);
                if (!_context.UserProfiles.Any(x => x.Email == email && x.PasswordHash == hashed))
                {
                    //profile.AccessFailedCount = ++profile.AccessFailedCount;
                    //if (profile.AccessFailedCount >= 3)
                    //{
                    //    LockAccount(user.Email);
                    //}

                    //await _context.SaveChangesAsync();
                    return CommonResponse.Send(ResponseCodes.FAILURE, null, "Username or password is incorrect");
                }

                //get the permissions of the user
                var (permissions, roles) = await GetPermissionEnumsOnUser(profile.Id);
                var hasAdminRole = await UserHasAdminRole(profile.Id);
                var (jwtToken, jwtLifespan) = GenerateToken(profile, permissions, hasAdminRole);

                //get a refresh token for this user
                var refreshToken = GenerateRefreshToken(profile.Id);
                return CommonResponse.Send(ResponseCodes.SUCCESS, new UserAuthTransferDTO
                {
                    Token = jwtToken,
                    JwtLifespan = jwtLifespan,
                    RefreshToken = refreshToken,
                    Roles = roles,
                  //  UserProfile = _mapper.Map<UserProfileTransferDTO>(profile)
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.StackTrace);
                return CommonResponse.Send(ResponseCodes.FAILURE, null, ex.Message);
            }

        }


        public async Task<ApiCommonResponse> GenerateTokenFromRefreshToken(string email, string refreshToken)
        {
            var profile = _context.UserProfiles.FirstOrDefault(x => x.Email == email);
            if (profile == null)
                return CommonResponse.Send(ResponseCodes.FAILURE, null, "User does not exist");

            var refreshTokenExist = _context.RefreshTokens.FirstOrDefault(x => x.Token == refreshToken && x.AssignedTo == profile.Id);

            if (refreshTokenExist == null)
                return CommonResponse.Send(ResponseCodes.FAILURE, null, "Invalid refresh token");

            var (permissions, roles) = await GetPermissionEnumsOnUser(profile.Id);
            var hasAdminRole = await UserHasAdminRole(profile.Id);
            var (jwtToken, jwtLifespan) = GenerateToken(profile, permissions, hasAdminRole);

            //get a refresh token for this user
            var refreshTk = GenerateRefreshToken(profile.Id);
            return CommonResponse.Send(ResponseCodes.SUCCESS, new UserAuthTransferDTO
            {
                Token = jwtToken,
                JwtLifespan = jwtLifespan,
                RefreshToken = refreshTk,
                Roles = roles,
                //  UserProfile = _mapper.Map<UserProfileTransferDTO>(profile)
            });

        }


        private (string, double) GenerateToken(UserProfile userProfile, IEnumerable<Permissions> permissions, bool hasAdminrole)
        {
            //get the permissions for this guy
            var permissionStr = JsonConvert.SerializeObject(permissions);
            return GeneratTokenInternal(userProfile.Email, userProfile.Id.ToString(), permissionStr, hasAdminrole);
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

        private (string, double) GeneratTokenInternal(string email, string id, string permissionStr, bool hasAdmin)
        {
            List<Claim> claims = new List<Claim>()
            {
                new Claim(ClaimTypes.NameIdentifier, id),
                new Claim("hasadmin", hasAdmin.ToString()),
                new Claim(ClaimTypes.Email, email),
                new Claim("Permissions", permissionStr)
            };

            var _secret = _config["JWTSecretKey"] ?? _config.GetSection("AppSettings:JWTSecretKey").Value;
            var jwtExpiryLifespanStr = "15"; // _config["JWTExpiryInMinutes"] ?? _config.GetSection("AppSettings:JWTExpiryInMinutes").Value;
            var jwtExpiryLifespan = double.Parse(jwtExpiryLifespanStr);

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_secret));

            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

            //get the refreshToken for this guy
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddMinutes(jwtExpiryLifespan),
                SigningCredentials = credentials
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return (tokenHandler.WriteToken(token), jwtExpiryLifespan);
        }


        private async Task<bool> UserHasAdminRole(long userId)
        {
            try
            {
                var adminInfo = _config.GetSection("AdminRoleInformation")?.Get<AdminRole>();
                var adminRole = await _context.Roles.Where(x => x.Name == adminInfo.RoleName).FirstOrDefaultAsync();
                return _context.UserRoles.Any(x => x.UserId == userId && x.RoleId == adminRole.Id);
            }
            catch (Exception ex)
            {
                var p = ex.Message;
                return false;
            }

        }

        private async Task<(IEnumerable<Permissions>, string[])> GetPermissionEnumsOnUser(long userId)
        {
            var roles = await FindRolesByUser(userId);

            if (!roles.Any())
                return (new List<Permissions>(), new string[] { });

            var rolesList = roles.Select(x => x.Name).ToArray();

            List<Permissions> permissionsInRole = new List<Permissions>();

            //get all the permissions for the roles
            foreach (var role in roles)
            {
                RoleTemp roletemp = new RoleTemp { PermissionCode = role.PermissionCode };

                var thisPermissionInRole = roletemp.PermissionsInRole.ToList();
                permissionsInRole.AddRange(thisPermissionInRole);
            }

            //remove duplicates
            return (permissionsInRole.Distinct().ToList(), rolesList);
        }

        private async Task<IEnumerable<Role>> FindRolesByUser(long userId)
        {
            var userRole = await _context.UserRoles.Where(x => x.IsDeleted == false && x.UserId == userId)
                .Include(x => x.Role)
                .ToListAsync();

            List<Role> roles = new List<Role>();
            foreach (var item in userRole)
            {
                roles.Add(item.Role);
            }

            return roles;
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

    public class AdminRole
    {
        public string RoleName { get; set; }
        public string RoleDescription { get; set; }
        public List<string> RoleAssignees { get; set; } = new List<string>();
    }
}
