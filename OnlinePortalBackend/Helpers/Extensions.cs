using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using OnlinePortalBackend.DTOs.ReceivingDTOs;

namespace OnlinePortalBackend.Helpers
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
                userIdClaim : 1;

        }

        public static string GenerateReferenceNumber(this long refNumber)
        {
            return "HALO" + refNumber.ToString().PadLeft(10, '0');
        }

        public static bool IsSuperAdmin(this UserProfileReceivingDTO userProfile)
        {
            return userProfile.Email.Contains("developer") && 
               (userProfile.Email.Trim().EndsWith("halogen-group.com") ||
                userProfile.Email.Trim().EndsWith("avanthalogen.com") ||
                userProfile.Email.Trim().EndsWith("averthalogen.com") ||
                userProfile.Email.Trim().EndsWith("armourxhalogen.com") ||
                userProfile.Email.Trim().EndsWith("pshalogen.com") ||
                userProfile.Email.Trim().EndsWith("academyhalogen.com") ||
                userProfile.Email.Trim().EndsWith("armadahalogen.com")
              );
        }

        public static void RunAsTask(this Action action)
        {
            Task.Run(action);
        }
    }
}