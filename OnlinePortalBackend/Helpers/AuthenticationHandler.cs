using Halobiz.Common.Auths.PermissionParts;
using Halobiz.Common.DTOs.ApiDTOs;
using HalobizMigrations.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Controllers;
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
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace OnlinePortalBackend.Helpers
{

    public class AuthenticationHandler
    {
        private readonly RequestDelegate _next;
        private readonly IConfiguration _configuration;
        private readonly JwtHelper _jwtHelper;
        private readonly ILogger<AuthenticationHandler> _logger;


        public AuthenticationHandler(RequestDelegate next, IConfiguration configuration, JwtHelper jwtHelper, ILogger<AuthenticationHandler> logger)
        {
            _next = next;
            _configuration = configuration;
            _jwtHelper = jwtHelper;
            _logger = logger;
        }

        public async Task Invoke(HttpContext context)
        {
            var controllerActionDescriptor = context?
                        .GetEndpoint()?
                        .Metadata?
                        .GetMetadata<ControllerActionDescriptor>();

            var controllerName = controllerActionDescriptor?.ControllerName;
            var actionName = controllerActionDescriptor?.ActionName;

            var allowedMethods = new List<string>();
            allowedMethods.Add("SendCode");
            allowedMethods.Add("CreatePassword");
            allowedMethods.Add("Login");
            allowedMethods.Add("VerifyCode");
            allowedMethods.Add("SupplierLogin");
            allowedMethods.Add("ResendVerificationCode");
            allowedMethods.Add("GetStates");
            allowedMethods.Add("GetLocalGovtAreas");
            allowedMethods.Add("GetStateById");
            allowedMethods.Add("GetLocalGovtAreaById");
            allowedMethods.Add("GetBusinessTypes");
            allowedMethods.Add("GenerateToken");
            allowedMethods.Add("GenerateTokenFromRefreshToken");
            //var actionVerb = context.Request.Method;
            var anonymousMethod = false;

            if (allowedMethods.Any(x => x == actionName))
            {
               anonymousMethod = true;
            }
            


            if (string.IsNullOrEmpty(controllerName) || string.IsNullOrEmpty(actionName))
            {
                context.Response.StatusCode = StatusCodes.Status404NotFound;
                await context.Response.WriteAsJsonAsync(CommonResponse.Send(ResponseCodes.FAILURE, null, "Path not found"));

                return;
            }            

            if (anonymousMethod == false)
            {
                var token = context.Request?.Headers["Authorization"].FirstOrDefault()?.Split(" ")?.Last();
                if (token != null)
                {
                    //validate the token
                    if (token.ToLower() != "null")
                    {
                        var isValid =  _jwtHelper.IsValidToken(token);
                        if (!isValid)
                        {
                            context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                            await context.Response.WriteAsJsonAsync(CommonResponse.Send(ResponseCodes.UNAUTHORIZED, null, "Token is invalid or expired"));
                            return;
                        }
                    }
                    else
                    {
                        context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                        await context.Response.WriteAsJsonAsync(CommonResponse.Send(ResponseCodes.UNAUTHORIZED, null, "Unathorized user"));
                        return;
                    }
                }
                else
                {
                    context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                    await context.Response.WriteAsJsonAsync(CommonResponse.Send(ResponseCodes.UNAUTHORIZED, null, "Unathorized user"));
                    return;
                }
            } 

            await _next(context);
        }       
    }    
}

