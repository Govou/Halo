using HaloBiz.Model.RoleManagement;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;

namespace HaloBiz.Data
{
    public static class PrepDb
    {
        public static void PrepDatabase(IApplicationBuilder app)
        {
            using(var serviceScope = app.ApplicationServices.CreateScope())
            {
                SeedData(serviceScope.ServiceProvider.GetService<DataContext>());
                SetUpApplicationClaims(serviceScope.ServiceProvider.GetService<DataContext>());
            }
        }

        public static void SeedData(DataContext context)
        {
            context.Database.Migrate();
        }

        public static void SetUpApplicationClaims(DataContext context) 
        {
            var allClaims = context.Claims.ToListAsync().GetAwaiter().GetResult();
            var newClaims = new List<Claim>();
            
            foreach (ClaimEnum item in Enum.GetValues(typeof(ClaimEnum)))
            {
                var claimExistInDb = allClaims.Any(x => x.ClaimEnum == item);
                if (!claimExistInDb) 
                {
                    newClaims.Add(new Claim { ClaimEnum = item, Description = item.ToString(), Name =  item.ToString() });
                }
            }
            context.Claims.AddRangeAsync(newClaims).GetAwaiter().GetResult();
            context.SaveChangesAsync().GetAwaiter().GetResult();
        }
    }
}