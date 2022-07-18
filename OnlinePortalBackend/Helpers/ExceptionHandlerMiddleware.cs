using Halobiz.Common.DTOs.ApiDTOs;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using OnlinePortalBackend.Model;
using System;
using System.IO;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace OnlinePortalBackend.Helpers
{
    public class ExceptionHandlerMiddleware
    {
        private readonly RequestDelegate _next;

        public ExceptionHandlerMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next.Invoke(context);
            }
            catch (Exception ex)
            {
                await HandleExceptionMessageAsync(context, ex).ConfigureAwait(false);
            }
        }

        private static Task HandleExceptionMessageAsync(HttpContext context, Exception exception)
        {
            context.Response.ContentType = "application/json";
            int statusCode = (int)HttpStatusCode.InternalServerError;
            //var result = JsonConvert.SerializeObject(new
            //{
            //    StatusCode = statusCode,
            //    ErrorMessage = exception.Message
            //});
            using (StreamReader reader
                 = new StreamReader(context.Request.Body, Encoding.UTF8, true, 1024, true))
            {
                var bodyStr = reader.ReadToEndAsync().Result;
            }
          
            var requestBody = context.Request.Body;
            var requestVal = context.Request.RouteValues;
            var result = JsonConvert.SerializeObject(new ApiCommonResponse
            {
                responseCode = "04",
                responseData = null,
                responseMsg = "A system error occured. Please contact the system administrator"
            });
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = statusCode;
            return context.Response.WriteAsync(result);
        }
    }
}
