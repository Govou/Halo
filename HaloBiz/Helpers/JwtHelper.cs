using Halobiz.Common.Auths.PermissionParts;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using HalobizMigrations.Models;
using Claim = System.Security.Claims.Claim;
using HalobizMigrations.Models.Halobiz;
using System.Security.Cryptography;
using HalobizMigrations.Data;

namespace HaloBiz.Helpers
{
    public interface IJwtHelper
    {
        (string, double, string) GenerateToken(UserProfile userProfile, IEnumerable<Permissions> permissions);
        string GetUserRefreshToken(long UserId);
        (bool, List<short>) IsValidToken(string token);
    }
    public class JwtHelper : IJwtHelper
    {
        private readonly ILogger<JwtHelper> _logger;
        private readonly string _secret;
        private readonly IConfiguration _config;
        private readonly HalobizContext _context;


        public JwtHelper(
            IConfiguration config,
            HalobizContext context,
            ILogger<JwtHelper> logger)
        {
            _logger = logger;
            _config = config;
            _context = context;
            _secret = config["JWTSecretKey"] ?? config.GetSection("AppSettings:JWTSecretKey").Value;
        }

        public (string, double, string) GenerateToken(UserProfile userProfile, IEnumerable<Permissions> permissions)
        {
            //get the permissions for this guy
            var permissionStr = JsonConvert.SerializeObject(permissions);

            List<Claim> claims = new List<Claim>()
            {
                new Claim(ClaimTypes.NameIdentifier, userProfile.Id.ToString()),
                new Claim(ClaimTypes.Email, userProfile.Email),
                new Claim(ClaimTypes.Role, userProfile.Role?.Name ?? string.Empty),
                new Claim("RoleId", userProfile.RoleId.ToString()),
                new Claim("Permissions", permissionStr)
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_secret));

            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

            var parameterExpiry = _config["JWTExpiryInMinutes"] ?? _config.GetSection("AppSettings:JWTExpiryInMinutes").Value;
            double jwtExpiryLifespan = double.Parse(parameterExpiry ?? "20");
           
            //get the refreshToken for this guy
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.Now.AddMinutes(jwtExpiryLifespan),
                SigningCredentials = credentials
            };

            var refreshToken = GetUserRefreshToken(userProfile.Id);
            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return (tokenHandler.WriteToken(token), jwtExpiryLifespan, refreshToken);
        }

        public string GetUserRefreshToken(long UserId)
        {
            var tokenRecord = _context.RefreshTokens.Where(x => x.AssignedTo == UserId).FirstOrDefault();
            if(tokenRecord == null)
            {
                return GenerateRefreshToken(UserId)?.Token;
            }
            else
            {
                //re-generate if is expired or less than/= two days to expiry
                if (tokenRecord.Expires < DateTime.Now || (tokenRecord.Expires.Date - DateTime.Today.Date).TotalDays <= 2)
                {
                    //gnerate new token
                    return GenerateRefreshToken(UserId)?.Token;
                }
            }

            return tokenRecord.Token;
        }

        private RefreshToken GenerateRefreshToken(long UserId)
        {
            using (var rngCryptoServiceProvider = new RNGCryptoServiceProvider())
            {
                var randomBytes = new byte[64];
                rngCryptoServiceProvider.GetBytes(randomBytes);
                var token = new RefreshToken
                {
                    Token = Convert.ToBase64String(randomBytes),
                    Expires = DateTime.Now.AddDays(7),
                    CreatedAt = DateTime.Now,
                    AssignedTo = UserId
                };

                //check if the user has token, otherwise create new
                var tokenRecord = _context.RefreshTokens.Where(x => x.AssignedTo == UserId).FirstOrDefault();
                if(tokenRecord == null)
                {
                    _context.RefreshTokens.Add(token);
                }
                else
                {
                    tokenRecord.AssignedTo = UserId;
                    tokenRecord.UpdatedAt = DateTime.Now;
                    tokenRecord.Token = token.Token;
                    tokenRecord.Expires = token.Expires;
                    _context.RefreshTokens.Update(tokenRecord);
                }

                _context.SaveChanges();
                return token;
            }
        }

        public (bool, List<short>) IsValidToken(string token)
        {
            try
            {
                var tokenHandler = new JwtSecurityTokenHandler();

                var key = Encoding.UTF8.GetBytes(_secret);

                tokenHandler.ValidateToken(token, new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    // set clockskew to zero so tokens expire exactly at token expiration time (instead of 5 minutes later)
                    ClockSkew = TimeSpan.Zero
                }, out SecurityToken validatedToken);

                var jwtToken = (JwtSecurityToken)validatedToken;

                var permssionsStr = jwtToken.Claims.First(x => x.Type == "Permissions").Value;
                var permissions = JsonConvert.DeserializeObject<List<short>>(permssionsStr);
                return (true, permissions);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.StackTrace);
                return (false, new List<short>());
            }
        }
    }
}
