using AutoMapper;
using Halobiz.Common.Auths.PermissionParts;
using Halobiz.Common.DTOs.ApiDTOs;
using HaloBiz.Model;
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
        private readonly IConfiguration _configuration;
       


        public AuthenticationHandler(RequestDelegate next, IJwtHelper jwtHelper,
            ILogger<AuthenticationHandler> logger, IConfiguration configuration      
            )
        {
            _next = next;
            _jwtHelper = jwtHelper;
            _logger = logger;
            _configuration = configuration;
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

           // context.Response.Headers.Add("Access-Control-Allow-Origin", "*");

            if (string.IsNullOrEmpty(controllerName) || string.IsNullOrEmpty(actionName))
            {
                context.Response.StatusCode = StatusCodes.Status404NotFound;
                await context.Response.WriteAsync("Path not found");
                return;
            }
            
            bool isExempted = (controllerName.ToLower() == "auth" && (actionName.ToLower() == "login" || actionName.ToLower() == "googlelogin"));
            if (!isExempted)
            {
                var token = context.Request?.Headers["Authorization"].FirstOrDefault()?.Split(" ")?.Last();
                var refreshToken = context.Request?.Headers["xr-token"].FirstOrDefault();

                _logger.LogInformation($"For action: {actionName}; Token supplied: {token}");

                if (token != null)
                {
                    //validate the token
                    
                    if (token.ToLower() != "null")
                    {
                        var (isValid, isExpired, authUser) = _jwtHelper.ValidateToken(token);
                        var permissionsList = new List<int>();
                        if(authUser != null)
                        {
                            permissionsList = JsonConvert.DeserializeObject<List<int>>(authUser.permissionString);
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
                            if(!_jwtHelper.GrantToGetNewToken(refreshToken, authUser.Id))
                            {
                                context.Response.StatusCode = (int) AppDefinedHttpStatusCodes.LOGIN_AGAIN;
                                await context.Response.WriteAsync("Access token given previously. Use it please");
                                return;
                            }
                            if (!_jwtHelper.IsDbRefreshTokenActive(authUser.Id, refreshToken))
                            {
                                context.Response.StatusCode = (int)AppDefinedHttpStatusCodes.LOGIN_AGAIN;
                                await context.Response.WriteAsync("Refresh token revoked or expired");
                                return;
                            }

                            //get a replacement token for this guy
                            var (newToken, lifeSPan) = _jwtHelper.GenerateToken(authUser.Email, authUser.Id, authUser.permissionString, authUser.hasAdminRole);
                            //indicate that this guy has received access token
                            _jwtHelper.AddRefreshTokenToTracker(authUser.Id, refreshToken);


                            //check the refresh token and use it to refresh the jwt at this point
                            context.Response.Headers.Add("Access-Control-Expose-Headers", "x-Token");
                            context.Response.Headers.Add("x-Token", newToken);

                            if (!authUser.hasAdminRole && !CheckAuthorization(context, controllerName, actionName, permissionsList))
                            {
                                //use 200 ok here so that the user can know that he does not have access to
                                context.Response.StatusCode = StatusCodes.Status200OK;
                                await context.Response.WriteAsJsonAsync(CommonResponse.Send(ResponseCodes.UNAUTHORIZED, null, $"You do not have permission for {actionVerb} in {controllerName}"));
                                return;
                            }
                        }
                        else if(isValid && !isExpired)
                        {
                            //test for the authorization
                            if (!authUser.hasAdminRole && !CheckAuthorization(context, controllerName, actionName, permissionsList))
                            {
                                //use 200 ok here so that the user can know that he does not have access to
                                context.Response.StatusCode = StatusCodes.Status200OK;
                                await context.Response.WriteAsJsonAsync(CommonResponse.Send(ResponseCodes.UNAUTHORIZED, null, $"You do not have permission for {actionVerb} in {controllerName}"));
                                return;
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

      

        private bool CheckAuthorization(HttpContext context, string controller,string actionName, List<int> permisssions)
        {
            var actionVerb = context.Request.Method.ToLower();
            if (actionVerb == "get") 
                return true; 

            var exemptedList = _configuration.GetSection("AuthorizationExemption")?.Get<List<AuthorizationExemption>>();
            if (exemptedList.Any())
            {
                var exemptions = exemptedList.Where(x => x.Controller.ToLower().Contains(controller.ToLower())).FirstOrDefault() ?? new AuthorizationExemption();
                if (exemptions.ActionVerbs.Contains(actionVerb, StringComparer.OrdinalIgnoreCase) || exemptions.Endpoints.Contains(actionName.ToLower(), StringComparer.OrdinalIgnoreCase))
                {
                    return true;
                }
            }           

            var permissionEnum = $"{controller}_{actionVerb}";           

            if (!Enum.TryParse(typeof(Permissions), permissionEnum, true, out var permission))
            {
                _logger.LogError($"No permission has be defined for {permissionEnum}");

                //this would ensure all devs have the endpoints written in the rightful place
                throw new Exception("This endpoint controller and action has not been added to Permission.cs");
            }

            var permissionInt = (int)permission;
            return permisssions.Contains(permissionInt);
        }

       
    }    
}

