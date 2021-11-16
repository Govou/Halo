using HalobizMigrations.Models.Armada;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HaloBiz.Repository
{
    public interface ISMORouteAndRegionRepository
    {
        //Route
        Task<SMORoute> SaveSMORoute(SMORoute sMORoute);
        Task<SMORoute> UpdateSMORoute(SMORoute sMORoute);
        Task<SMORoute> FindSMORouteById(long id);

        SMORoute GetRouteName(string Name);
        Task<IEnumerable<SMORoute>> FindAllSMORoutes();
        Task<bool> DeleteSMORoute(SMORoute sMORoute);

        //Region
        Task<SMORegion> SaveSMORegion(SMORegion sMORegion);

        Task<SMORegion> FindSMORegionById(long Id);

        Task<IEnumerable<SMORegion>> FindAllSMORegions();
        SMORegion GetRegionName(string Name);

        Task<SMORegion> UpdateSMORegion(SMORegion sMORegion);

        Task<bool> DeleteSMORegion(SMORegion sMORegion);


        //ReturnRoute
        Task<SMOReturnRoute> SaveSMOReturnRoute(SMOReturnRoute sMOReturnRoute);
        Task<SMOReturnRoute> UpdateSMOReturnRoute(SMOReturnRoute sMOReturnRoute);
        Task<SMOReturnRoute> FindSMOReturnRouteById(long id);

        Task<IEnumerable<SMOReturnRoute>> FindAllSMOReturnRoutes();
        Task<bool> DeleteSMOReturnRoute(SMOReturnRoute sMOReturnRoute);
    }


}
