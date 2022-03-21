﻿using AutoMapper;
using Google.Apis.Auth;
using Halobiz.Common.DTOs.ApiDTOs;
using Halobiz.Common.DTOs.ReceivingDTOs;
using Halobiz.Common.DTOs.TransferDTOs;
using Halobiz.Common.MyServices;
using Halobiz.Common.MyServices.RoleManagement;
using HaloBiz.DTOs.ReceivingDTOs;
using HaloBiz.DTOs.TransferDTOs;
using HaloBiz.Helpers;
using HalobizMigrations.Data;
using HalobizMigrations.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace HaloBiz.MyServices
{
    public interface IUserAuthentication
    {
        Task<ApiCommonResponse> OtherLogin(LoginDTO login);
        Task<ApiCommonResponse> Login(GoogleLoginReceivingDTO loginReceiving);
        Task<ApiCommonResponse> CreateProfile(AuthUserProfileReceivingDTO authUserProfileReceivingDTO);
    }

    public class UserAuthentication : IUserAuthentication
    {
        private readonly IUserProfileService _userProfileService;
        private readonly ILogger<UserAuthentication> _logger;
        private readonly JwtHelper _jwttHelper;
        private readonly IRoleService _roleService;
        private readonly IMapper _mapper;
        private static DateTime _tokenExpiryTime;
        private readonly HalobizContext _context;


        public UserAuthentication(IUserProfileService userProfileService,
            IConfiguration config,
            JwtHelper jwtHelper,
            IMapper mapper,
            IRoleService roleService,
            HalobizContext context,
            ILogger<UserAuthentication> logger)
        {
            _userProfileService = userProfileService;
            _logger = logger;
            _jwttHelper = jwtHelper;
            _mapper = mapper;
            _roleService = roleService;
            var parameterExpiry = config["JWTExpiryInMinutes"] ?? config.GetSection("AppSettings:JWTExpiryInMinutes").Value;
            double expiry = double.Parse(parameterExpiry ?? "20");
            _tokenExpiryTime = DateTime.Now.AddMinutes(expiry);
            _context = context;
        }

        public async Task<ApiCommonResponse> OtherLogin(LoginDTO login)
        {
            try
            {
                var response = await _userProfileService.FindUserByEmail(login.Email);

                if (!response.responseCode.Contains("00"))
                {
                    _logger.LogWarning($"Could not find user [{login.Email}] => {response.responseMsg}");
                    return CommonResponse.Send(ResponseCodes.FAILURE, null, "Could not find the user");
                }

                if (login.Password != "12345")
                {
                    return CommonResponse.Send(ResponseCodes.FAILURE, null, "Username or password incorrect");
                }

                var user = response.responseData;
                var userProfile = (UserProfile)user;

                //get the permissions of the user
                var permissions = await _roleService.GetPermissionEnumsOnUser(userProfile.Id);

                var jwtToken = _jwttHelper.GenerateToken(userProfile, permissions);

                return CommonResponse.Send(ResponseCodes.SUCCESS, new UserAuthTransferDTO
                {
                    Token = jwtToken,
                    TokenExpiryTime = _tokenExpiryTime,
                    UserProfile = _mapper.Map<UserProfileTransferDTO>(userProfile)
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                _logger.LogError(ex.StackTrace);
                return CommonResponse.Send(ResponseCodes.FAILURE, null, "System error");
            }
        }

        public async Task<ApiCommonResponse> Login(GoogleLoginReceivingDTO loginReceiving)
        {
            try
            {
                GoogleJsonWebSignature.Payload payload;

                try
                {
                    payload = await GoogleJsonWebSignature.ValidateAsync(loginReceiving.IdToken);
                }
                catch (InvalidJwtException invalidJwtException)
                {
                    _logger.LogWarning($"Could not validate Google Id Token [{loginReceiving.IdToken}] => {invalidJwtException.Message}");
                    return CommonResponse.Send(ResponseCodes.FAILURE, null, invalidJwtException.Message);
                }

                if (!payload.EmailVerified)
                {
                    _logger.LogWarning($"Email verification failed. Payload => {JsonConvert.SerializeObject(payload)}");
                    return CommonResponse.Send(ResponseCodes.INVALID_LOGIN_DETAILS, null, "Invalid login details.");
                }

                var email = payload.Email;

                var response = await _userProfileService.FindUserByEmail(email);

                if (!response.responseCode.Contains("00"))

                {
                    _logger.LogWarning($"Could not find user [{email}] => {response.responseData}");

                    return CommonResponse.Send(ResponseCodes.NO_USER_PROFILE_FOUND, null, "Could not find user.");
                }

                var user = response.responseData;
                var userProfile = (UserProfile)user;

                var permissions = await _roleService.GetPermissionEnumsOnUser(userProfile.Id);

                var jwtToken = _jwttHelper.GenerateToken(userProfile, permissions);
                return CommonResponse.Send(ResponseCodes.SUCCESS, new UserAuthTransferDTO
                {
                    Token = jwtToken,
                    TokenExpiryTime = _tokenExpiryTime,
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

                if (!payload.EmailVerified)
                {
                    _logger.LogWarning($"Email verification failed. Payload => {JsonConvert.SerializeObject(payload)}");
                    return CommonResponse.Send(ResponseCodes.FAILURE, null, "Email verification failed.");
                }

                var userProfileDTO = authUserProfileReceivingDTO.UserProfile;

                //userProfileDTO.RoleId = userProfileDTO.IsSuperAdmin() ? 1 : 2;

                var response = await _userProfileService.AddUserProfile(userProfileDTO);

                if (!response.responseCode.Contains("00"))
                {
                    _logger.LogWarning($"Could not create user [{userProfileDTO.Email}] => {response.responseMsg}");

                }

                var user = response.responseData;
                var userProfile = (UserProfile)user;

                var permissions = await _roleService.GetPermissionEnumsOnUser(userProfile.Id);

                var token = _jwttHelper.GenerateToken(userProfile, permissions);


                UserAuthTransferDTO userAuthTransferDTO = new UserAuthTransferDTO()
                {
                    Token = token,
                    TokenExpiryTime = _tokenExpiryTime,
                    UserProfile = _mapper.Map<UserProfileTransferDTO>(userProfile)
                };

                return CommonResponse.Send(ResponseCodes.SUCCESS, userAuthTransferDTO);

            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                _logger.LogError(ex.StackTrace);
                return CommonResponse.Send(ResponseCodes.FAILURE, null, ex.Message);

            }
        }

        //public async Task<ApiCommonResponse> RefreshToken(RefreshTokenRequest model)
        //{
        //    try
        //    {
        //        var user = _context.Users.SingleOrDefault(u => u.RefreshTokens.Any(t => t.Token == model.Token));

        //        // return if no user is found
        //        if (user == null)
        //            return ResponseGenerator<TokenAuthResponse>.GenerateResponse(ResponseCodes.TOKEN_INVALID, null, "Token is invalid");

        //        var refreshToken = user.RefreshTokens.Single(x => x.Token == model.Token);

        //        // return  if token is no longer active
        //        if (!refreshToken.IsActive)
        //            return ResponseGenerator<TokenAuthResponse>.GenerateResponse(ResponseCodes.TOKEN_EXPIRED, null, "No user is found with token");

        //        // replace old refresh token with a new one and save
        //        var newRefreshToken = generateRefreshToken();
        //        refreshToken.Revoked = DateTime.UtcNow;
        //        refreshToken.ReplacedByToken = newRefreshToken.Token;
        //        user.RefreshTokens.Add(newRefreshToken);
        //        _context.Update(user);
        //        await _context.SaveChangesAsync();

        //        // generate new jwt
        //        var jwtToken = generateJwtToken(user);

        //        return ResponseGenerator<TokenAuthResponse>
        //                .GenerateResponse(ResponseCodes.SUCCESS, new List<TokenAuthResponse> { new TokenAuthResponse(user, jwtToken, newRefreshToken.Token) }, string.Empty);

        //    }
        //    catch (Exception ex)
        //    {
        //        _logger.LogError(ex, "RefreshToken error");
        //        return ResponseGenerator<TokenAuthResponse>.GenerateResponse(ResponseCodes.FAILURE, null);
        //    }
        //}

        //public async Task<ApiResponse<TokenRevokeResponse>> RevokeToken(string token)
        //{
        //    try
        //    {
        //        var user = _context.Users.SingleOrDefault(u => u.RefreshTokens.Any(t => t.Token == token));

        //        // return  if no user found with token
        //        if (user == null)
        //            return ResponseGenerator<TokenRevokeResponse>.GenerateResponse(ResponseCodes.FAILURE, null, "No user with token");

        //        var refreshToken = user.RefreshTokens.Single(x => x.Token == token);

        //        // return  if token is not active
        //        if (!refreshToken.IsActive)
        //            return ResponseGenerator<TokenRevokeResponse>.GenerateResponse(ResponseCodes.TOKEN_INACTIVE, null);

        //        // revoke token and save
        //        refreshToken.Revoked = DateTime.UtcNow;
        //        refreshToken.RevokedByIp = null;
        //        _context.Update(user);
        //        await _context.SaveChangesAsync();

        //        return ResponseGenerator<TokenRevokeResponse>.GenerateResponse(ResponseCodes.SUCCESS, null);
        //    }
        //    catch (Exception ex)
        //    {
        //        _logger.LogError(ex, "Error in RevokeToken");
        //        return ResponseGenerator<TokenRevokeResponse>.GenerateResponse(ResponseCodes.FAILURE, null);
        //    }
        //}


        //private RefreshToken generateRefreshToken()
        //{
        //    using (var rngCryptoServiceProvider = new RNGCryptoServiceProvider())
        //    {
        //        var randomBytes = new byte[64];
        //        rngCryptoServiceProvider.GetBytes(randomBytes);
        //        return new RefreshToken
        //        {
        //            Token = Convert.ToBase64String(randomBytes),
        //            Expires = DateTime.UtcNow.AddDays(7),
        //            Created = DateTime.UtcNow,
        //            CreatedByIp = null
        //        };
        //    }
        //}
    }
}
