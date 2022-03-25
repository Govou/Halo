using AutoMapper;
using Halobiz.Common.Auths.PermissionParts;
using Halobiz.Common.DTOs.ApiDTOs;
using HaloBiz.Models;
using HalobizMigrations.Data;
using HalobizMigrations.Models;
using HalobizMigrations.Models.Halobiz;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.IdentityModel.Tokens.Jwt;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace HaloBiz.Helpers
{

    public class AuthenticationHandler
    {
        private readonly RequestDelegate _next;
        private readonly IJwtHelper _jwtHelper;
        private readonly ILogger<AuthenticationHandler> _logger;
        private IMemoryCache _memoryCache;
        private IMapper _mapper;


        public AuthenticationHandler(RequestDelegate next, IJwtHelper jwtHelper,
            ILogger<AuthenticationHandler> logger,
            IMemoryCache memory,
            IMapper mapper
            )
        {
            _next = next;
            _jwtHelper = jwtHelper;
            _logger = logger;
            _memoryCache = memory;
            _mapper = mapper;
        }

        public async Task Invoke(HttpContext context)
        {          

            var controllerActionDescriptor = context?
                        .GetEndpoint()?
                        .Metadata?
                        .GetMetadata<ControllerActionDescriptor>();

            var controllerName = controllerActionDescriptor?.ControllerName;
            var actionName = controllerActionDescriptor?.ActionName;
            var actionVerb = context.Request.Method;   

           //var p = _context.authUsers

            if (string.IsNullOrEmpty(controllerName) || string.IsNullOrEmpty(actionName))
            {
                context.Response.StatusCode = StatusCodes.Status404NotFound;
                await context.Response.WriteAsync("Path not found");
                return;
            }
            
            bool isExempted = (controllerName.ToLower() == "auth" && (actionName.ToLower() == "login" || actionName.ToLower() == "otherlogin") || actionName.ToLower()== "createuser");
            if (!isExempted)
            {
                var token = context.Request?.Headers["Authorization"].FirstOrDefault()?.Split(" ")?.Last();
                var refreshToken = context.Request?.Headers["xr-token"].FirstOrDefault();

                if (token != null)
                {
                    //validate the token
                    if (token.ToLower() != "null")
                    {
                        var (isValid, isExpired, authUser) = _jwtHelper.ValidateToken(token);
                        var permissionsList = new List<short>();
                        if(authUser != null)
                        {
                            permissionsList = JsonConvert.DeserializeObject<List<short>>(authUser.permissionString);
                        }

                        if (isValid && isExpired)
                        {
                            //check the refreshToken for this guy
                            if (string.IsNullOrEmpty(refreshToken))
                            {
                                context.Response.StatusCode = (int)AppDefinedHttpStatusCodes.NO_REFRESH_TOKEN_SUPPLIED;
                                await context.Response.WriteAsync("Access token expired but no refresh token supplied.");
                                return;
                            }

                            //get memory refreshtoken
                            var rToken = GetMemoryRefreshToken(refreshToken, authUser.Id);
                            if(!string.IsNullOrEmpty(rToken) && rToken != refreshToken)
                            {
                                context.Response.StatusCode = (int) AppDefinedHttpStatusCodes.LOGIN_AGAIN;
                                await context.Response.WriteAsync("Refresh token invalid or expired");
                                return;
                            }else if (string.IsNullOrEmpty(rToken))
                            {
                                //get refresh token from db
                                var id = long.Parse(authUser.Id);
                                var isDbTokenValid = IsDbbRefreshTokenActive(id, refreshToken);
                                if(!isDbTokenValid)
                                {
                                    context.Response.StatusCode = (int)AppDefinedHttpStatusCodes.LOGIN_AGAIN;
                                    await context.Response.WriteAsync("Refresh token invalid or expired");
                                    return;
                                }
                            }
                            else
                            {
                                //get a replacement token for this guy
                                var (newToken, lifeSPan) = _jwtHelper.GenerateToken(authUser.Email, authUser.Id, authUser.permissionString);

                                //check the refresh token and use it to refresh the jwt at this point
                                context.Response.Headers.Add("x-Token", newToken);

                                if (!(controllerName.ToLower() == "user" && (actionVerb.ToLower() == "get")))
                                {
                                    if (!CheckAuthorization(context, controllerName, permissionsList))
                                    {
                                        //use 200 ok here so that the user can know that he does not have access to
                                        context.Response.StatusCode = StatusCodes.Status200OK;
                                        await context.Response.WriteAsJsonAsync(CommonResponse.Send(ResponseCodes.UNAUTHORIZED, null, $"You do not have permission for {actionVerb} in {controllerName}"));
                                        return;
                                    }
                                }
                            }                           
                        }
                        else if(isValid && !isExpired)
                        {
                            //test for the authorization
                            //exempt users get
                            if (!(controllerName.ToLower() == "user" && (actionVerb.ToLower() == "get")))
                            {
                                if (!CheckAuthorization(context, controllerName, permissionsList))
                                {
                                    //use 200 ok here so that the user can know that he does not have access to
                                    context.Response.StatusCode = StatusCodes.Status200OK;
                                    await context.Response.WriteAsJsonAsync(CommonResponse.Send(ResponseCodes.UNAUTHORIZED, null, $"You do not have permission for {actionVerb} in {controllerName}"));
                                    return;
                                }
                            }
                        }
                        else
                        {
                            context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                            await context.Response.WriteAsync("Unauthorized user");
                            return;
                        }
                    }
                    else
                    {
                        context.Response.StatusCode = (int)AppDefinedHttpStatusCodes.NO_ACCESS_TOKEN_SUPPLIED;
                        await context.Response.WriteAsync("Unauthorized user");
                        return;
                    }
                }
                else
                {
                    context.Response.StatusCode = (int) AppDefinedHttpStatusCodes.NO_ACCESS_TOKEN_SUPPLIED;
                    await context.Response.WriteAsync("Unauthorized user. No access token");
                    return;
                }
            } 

            await _next(context);
        }

       

        private bool CheckAuthorization(HttpContext context, string controller, List<short> permisssions)
        {
            var actionVerb = context.Request.Method;

            var permissionEnum = $"{controller}_{actionVerb}";

            if (!Enum.TryParse(typeof(Permissions), permissionEnum, true, out var permission))
            {
                _logger.LogError($"No permission has be defined for {permissionEnum}");

                //this would ensure all devs have the endpoints written in the rightful place
                throw new Exception("This endpoint controller and action has not been added to this system");
            }

            var permissionInt = (short)permission;
            return permisssions.Contains(permissionInt);
        }

        private bool AddRefreshTokenToTracker(string id, string token)
        {
            if (!_memoryCache.TryGetValue(token, out RefreshTokenTracker tracker))
            {
                tracker = new RefreshTokenTracker
                {
                    Id = id,
                    Token = token
                };

                var cacheEntryOptions = new MemoryCacheEntryOptions()
                            .SetAbsoluteExpiration(TimeSpan.FromMinutes(30));
                _memoryCache.Set(token, tracker, cacheEntryOptions);
            }
            return true;
        }


        /// <summary>
        /// (refreshToken, isUseable)
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        private string GetMemoryRefreshToken(string token, string id)
        {
            var record = _memoryCache.Get<RefreshTokenTracker>(token);
            if(record != null)
            {
                if(record.Id==id)
                    return (record.Token);
            }

            return string.Empty;
        }

        private bool IsDbbRefreshTokenActive(long Id, string token)
        {
            var context = new HalobizContext();
            var tokenRecord = context.RefreshTokens.Where(x => x.AssignedTo == Id && x.Token == token).FirstOrDefault();
            if (tokenRecord == null) return false;
            var mappedTokenRecord = _mapper.Map<mRefreshToken>(tokenRecord);
            if (mappedTokenRecord.IsActive) { AddRefreshTokenToTracker(Id.ToString(), token); return true; }
            else return false;
        }
    }    
}

