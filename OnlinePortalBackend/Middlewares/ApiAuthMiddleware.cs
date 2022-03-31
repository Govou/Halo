using Microsoft.AspNetCore.Http;
using OnlinePortalBackend.Adapters;
using System;
using System.Threading.Tasks;

namespace OnlinePortalBackend.Middlewares
{
    public class ApiAuthMiddleware
    {
        private readonly RequestDelegate _next;
        public ApiAuthMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        //public async Task InvokeAsync(HttpContext httpContext, IApiInterceptor interceptor)
        //{
        //     var token = await interceptor.GetToken();

        //    if (!string.IsNullOrEmpty(token))
        //    {
        //        httpContext.
        //    }
        //        await _next.Invoke(httpContext);
        //}
    }
}
