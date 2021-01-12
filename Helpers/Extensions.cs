using System;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;

namespace HaloBiz.Helpers
{
    public static class Extensions
    {
        public static void AddApplicationError(this HttpResponse response, string message)
        {
            response.Headers.Add("Application-Error", message);
            response.Headers.Add("Access-Control-Expose-Headers", "Application-Error");
            response.Headers.Add("Access-Control-Allow-Origin", "*");

        }

        public static long GetLoggedInUserId(this HttpContext context)
        {
            return long.TryParse(context.User.FindFirstValue(ClaimTypes.NameIdentifier), out long userIdClaim) ?
                userIdClaim : 31;

        }
    }
}