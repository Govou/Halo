﻿using Halobiz.Common.Auths.PermissionParts;
using HalobizMigrations.Models;
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
using Claim = System.Security.Claims.Claim;

namespace HalobizIdentityServer.Helpers
{
    public class JwtHelper
    {
        private readonly ILogger<JwtHelper> _logger;
        private readonly string _secret;


        public JwtHelper(
            IConfiguration config,
            ILogger<JwtHelper> logger)
        {
            _logger = logger;
            _secret = config["JWTSecretKey"] ?? config.GetSection("AppSettings:JWTSecretKey").Value;

        }

        public string GenerateToken(UserProfile userProfile, IEnumerable<Permissions> permissions)
        {
            //get the permissions for this guy
            var permissionStr = JsonConvert.SerializeObject(permissions);

            List<Claim> claims = new List<Claim>()
            {
                new Claim(ClaimTypes.NameIdentifier, userProfile.Id.ToString()),
                new Claim(ClaimTypes.Email, userProfile.Email),
                //new Claim(ClaimTypes.Role, userProfile.Role?.Name ?? string.Empty),
                //new Claim("RoleId", userProfile.RoleId.ToString()),
               new Claim("Permissions", permissionStr)
            };


            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_secret));

            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.Now.AddMinutes(20),
                SigningCredentials = credentials
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
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
