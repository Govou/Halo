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
using HaloBiz.Models;

namespace HaloBiz.Helpers
{
    public interface IJwtHelper
    {
        (string, double) GenerateToken(UserProfile userProfile, IEnumerable<Permissions> permissions);
        (bool, bool, AuthUser) ValidateToken(string token);
        (string, double) GenerateToken(string email, string id, string permissionsString);
    }
    public class JwtHelper : IJwtHelper
    {
        private readonly ILogger<JwtHelper> _logger;
        private readonly string _secret;
        private readonly IConfiguration _config;


        public JwtHelper(
            IConfiguration config,
            ILogger<JwtHelper> logger)
        {
            _logger = logger;
            _config = config;
            _secret = config["JWTSecretKey"] ?? config.GetSection("AppSettings:JWTSecretKey").Value;
        }

        public (string, double) GenerateToken(UserProfile userProfile, IEnumerable<Permissions> permissions)
        {
            //get the permissions for this guy
            var permissionStr = JsonConvert.SerializeObject(permissions);
            return GeneratTokenInternal(userProfile.Email, userProfile.Id.ToString(), permissionStr);
        }

        public (string, double) GenerateToken(string email, string id, string permissionsString)
        {
            //get the permissions for this guy
            var permissionStr = JsonConvert.SerializeObject(permissionsString);
            return GeneratTokenInternal(email, id, permissionStr);
        }

        private (string, double) GeneratTokenInternal(string email, string id, string permissionStr)
        {
            List<Claim> claims = new List<Claim>()
            {
                new Claim(ClaimTypes.NameIdentifier, id),
                new Claim(ClaimTypes.Email, email),
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
                Expires = DateTime.UtcNow.AddMinutes(jwtExpiryLifespan),
                SigningCredentials = credentials
            };

            Console.WriteLine($"Expiry given {tokenDescriptor.Expires}");
            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return (tokenHandler.WriteToken(token), jwtExpiryLifespan);
        }

        /// <summary>
        /// (isValid, isEXpired, permissions)
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>

        public (bool, bool, AuthUser) ValidateToken(string token)
        {
            var emptyPermissions = new List<short>();
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
                    ValidateLifetime = false, //would check for lifetime myself
                    // set clockskew to zero so tokens expire exactly at token expiration time (instead of 5 minutes later)
                    ClockSkew = TimeSpan.Zero
                }, out SecurityToken validatedToken);

                var jwtToken = (JwtSecurityToken)validatedToken;

                var permssionsStr = jwtToken.Claims.First(x => x.Type == "Permissions").Value;
                var email = jwtToken.Claims.First(x => x.Type == "email").Value;
                var id = jwtToken.Claims.First(x => x.Type == "nameid").Value;

                var tokenExpiry = validatedToken.ValidTo;
                Console.WriteLine($"Valid to {validatedToken.ValidTo}");
                var authUser = new AuthUser { Id = id, Email = email, permissionString = permssionsStr };
                if (tokenExpiry < DateTime.UtcNow)
                {
                    return (true, true, authUser); //(isValid, isEXpired, permissions)
                }
                
                return (true, false, authUser); //(isValid, isEXpired, permissions)
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.StackTrace);
                return (false, false, null); //(isValid, isEXpired, permissions)
            }
        }
    }
}
