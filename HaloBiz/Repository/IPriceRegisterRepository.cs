using HalobizMigrations.Models;
using HalobizMigrations.Models.Armada;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HaloBiz.Repository
{
    public interface IPriceRegisterRepository
    {
        Task<PriceRegister> SavePriceRegister(PriceRegister priceRegister);

        Task<PriceRegister> FindPriceRegisterById(long Id);

        Task<IEnumerable<PriceRegister>> FindAllPriceRegisters();

        Task<IEnumerable<PriceRegister>> FindAllPriceRegistersWithByRouteId(long routeId);
        Task<IEnumerable<PriceRegister>> FindAllPriceRegistersWithByRouteId_(long? routeId, long? categoryId);
        Task<IEnumerable<PriceRegister>> FindAllPriceRegistersWithByServiceCategoryId(long categoryId);

        //PriceRegister GetServiceRegIdRegionAndRoute(long regServiceId, long RouteId, long RegionId);
        PriceRegister GetServiceRegIdRegionAndRoute(long regServiceId, long RouteId);

        Task<PriceRegister> UpdatePriceRegister(PriceRegister priceRegister);

        Task<bool> DeletePriceRegister(PriceRegister priceRegister);
    }
}
