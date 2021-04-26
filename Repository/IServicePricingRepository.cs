using HalobizMigrations.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HaloBiz.Repository
{
    public interface IServicePricingRepository
    {
        Task<ServicePricing> SaveServicePricing(ServicePricing servicePricing);
        Task<ServicePricing> FindServicePricingById(long Id);
        //Task<ServicePricing> FindServicePricingByName(string name);
        Task<IEnumerable<ServicePricing>> FindAllServicePricings();
        Task<ServicePricing> UpdateServicePricing(ServicePricing servicePricing);
        Task<bool> DeleteServicePricing(ServicePricing servicePricing);
    }
}
