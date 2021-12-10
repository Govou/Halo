using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
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


        public AuthenticationHandler(RequestDelegate next, IConfiguration configuration, JwtHelper jwtHelper)
        {
            _next = next;
            _configuration = configuration;
            _jwtHelper = jwtHelper;
        }

        public async Task Invoke(HttpContext context)
        {
            var controllerActionDescriptor = context?
                        .GetEndpoint()?
                        .Metadata?
                        .GetMetadata<ControllerActionDescriptor>();

            var controllerName = controllerActionDescriptor.ControllerName;
            var actionName = controllerActionDescriptor.ActionName;
            //var actionVerb = context.Request.Method;

            bool isExempted = (controllerName.ToLower() == "auth" && (actionName.ToLower() == "login" || actionName.ToLower() == "otherlogin"));

            if (!isExempted)
            {
                var token = context.Request?.Headers["Authorization"].FirstOrDefault()?.Split(" ")?.Last();

                if (token != null)
                {
                    //validate the token
                    if (token.ToLower() != "null")
                    {
                        bool isValid = await _jwtHelper.IsValidToken(token);
                        if (!isValid)
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
                else
                {
                    context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                    await context.Response.WriteAsync("Unauthorized user");
                    return;
                }
            }           

            await _next(context);
        }

       
    }
}

