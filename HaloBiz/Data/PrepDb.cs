using HalobizMigrations.Models;
using HalobizMigrations.Data;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using HaloBiz.Model.RoleManagement;

namespace HaloBiz.Data
{
    public static class PrepDb
    {
        public static void PrepDatabase(IApplicationBuilder app)
        {
            using(var serviceScope = app.ApplicationServices.CreateScope())
            {
                SetUpApplicationClaims(serviceScope.ServiceProvider.GetService<HalobizContext>());
            }
        }

        public static void SetUpApplicationClaims(HalobizContext context) 
        {
            var allClaims = context.Claims.ToListAsync().GetAwaiter().GetResult();
            var newClaims = new List<Claim>();
            
            foreach (ClaimEnum item in Enum.GetValues(typeof(ClaimEnum)))
            {
                var claimExistInDb = allClaims.Any(x => x.ClaimEnum == (int)item);
                if (!claimExistInDb) 
                {
                    newClaims.Add(new Claim { ClaimEnum = (int)item, Description = item.ToString(), Name =  item.ToString() });
                }
            }
            context.Claims.AddRangeAsync(newClaims).GetAwaiter().GetResult();
            context.SaveChangesAsync().GetAwaiter().GetResult();
        }
    }
}