using HaloBiz.DTOs.ApiDTOs;
using HaloBiz.DTOs.ReceivingDTOs;
using HaloBiz.DTOs.SMODTO;
using HaloBiz.Helpers;
using HalobizMigrations.Models.Armada;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HaloBiz.MyServices
{
    public interface ISMORouteAndRegionService
    {
        //Route
        Task<ApiCommonResponse> AddSMORoute(HttpContext context, SMORouteReceivingDTO sMORouteReceivingDTO);
        Task<ApiCommonResponse> UpdateSMORoute(HttpContext context, long id, SMORouteReceivingDTO sMORouteReceivingDTO);
        Task<ApiCommonResponse> GetSMORouteById(long id);

        Task<ApiCommonResponse> GetAllSMORoutes();
        Task<ApiCommonResponse> GetAllSMORoutesByName(string routeName);

        //Task<ApiCommonResponse> GetAllSMORouteAndRegions();
        Task<ApiCommonResponse> GetAllSMORoutesWithReturnRoute();
        Task<ApiCommonResponse> DeleteSMORoute(long id);

        ////Region
        Task<ApiCommonResponse> AddSMORegion(HttpContext context, SMORegionReceivingDTO sMOReceivingDTO);
        Task<ApiCommonResponse> UpdateSMORegion(HttpContext context, long id, SMORegionReceivingDTO sMOReceivingDTO);
        Task<ApiCommonResponse> GetSMORegionById(long id);

        Task<ApiCommonResponse> GetAllSMORegions();
        Task<ApiCommonResponse> DeleteSMORegion(long id);

        //ReturnRoute
        Task<ApiCommonResponse> AddSMOReturnRoute(HttpContext context, SMOReturnRouteReceivingDTO sMOReturnRouteReceivingDTO);
        Task<ApiCommonResponse> UpdateSMOReturnRoute(HttpContext context, long id, SMOReturnRouteReceivingDTO sMOReturnRouteReceivingDTO);
        Task<ApiCommonResponse> GetSMOReturnRouteById(long id);

        Task<ApiCommonResponse> GetAllSMOReturnRoutes();
        Task<ApiCommonResponse> DeleteSMOReturnRoute(long id);

        //Routemap
        Task<ApiCommonResponse> AddSMORouteMap(HttpContext context, SMORouteMapReceivingDTO sMORouteMapReceivingDTO);
        //Task<ApiCommonResponse> UpdateSMOReturnRoute(HttpContext context, long id, SMOReturnRouteReceivingDTO sMOReturnRouteReceivingDTO);
        Task<ApiCommonResponse> GetSMORouteMapById(long id);
        Task<ApiCommonResponse> GetAllRouteMapsByRouteId(long routeId);
        Task<ApiCommonResponse> GetAllSMORouteMaps();
        Task<ApiCommonResponse> DeleteSMORouteMap(long id);
    }
}
