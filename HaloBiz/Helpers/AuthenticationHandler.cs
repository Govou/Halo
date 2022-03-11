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

namespace HaloBiz.Helpers
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

            if(string.IsNullOrEmpty(controllerName) || string.IsNullOrEmpty(actionName))
            {
                context.Response.StatusCode = StatusCodes.Status404NotFound;
                await context.Response.WriteAsync("Path not found");

                return;
            }
            
            bool isExempted = (controllerName.ToLower() == "auth" && (actionName.ToLower() == "login" || actionName.ToLower() == "otherlogin") || actionName.ToLower()== "createuser");

            if (!isExempted)
            {
                var token = context.Request?.Headers["Authorization"].FirstOrDefault()?.Split(" ")?.Last();
                if (token != null)
                {
                    //validate the token
                    if (token.ToLower() != "null")
                    {
                        var (isValid, permissionsList) =  _jwtHelper.IsValidToken(token);
                        if (!isValid)
                        {
                            context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                            await context.Response.WriteAsync("Token is invalid or expired");
                            return;
                        }
                        else
                        {
                            //test for the authorization
                            if(!CheckAuthorization(context, controllerName, permissionsList))
                            {
                                //use 200 ok here so that the user can know what he has access to
                                context.Response.StatusCode = StatusCodes.Status200OK;
                                await context.Response.WriteAsJsonAsync(CommonResponse.Send(ResponseCodes.UNAUTHORIZED, null, "You do not have permission to access this endpoint"));
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
                    context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                    await context.Response.WriteAsync("Unauthorized user");
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
    }    
}

