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
        Task<ApiResponse> AddSMORoute(HttpContext context, SMORouteReceivingDTO sMORouteReceivingDTO);
        Task<ApiResponse> UpdateSMORoute(HttpContext context, long id, SMORouteReceivingDTO sMORouteReceivingDTO);
        Task<ApiResponse> GetSMORouteById(long id);

        Task<ApiResponse> GetAllSMORoutes();
        Task<ApiResponse> DeleteSMORoute(long id);

        ////Region
        Task<ApiResponse> AddSMORegion(HttpContext context, SMORegionReceivingDTO sMOReceivingDTO);
        Task<ApiResponse> UpdateSMORegion(HttpContext context, long id, SMORegionReceivingDTO sMOReceivingDTO);
        Task<ApiResponse> GetSMORegionById(long id);

        Task<ApiResponse> GetAllSMORegions();
        Task<ApiResponse> DeleteSMORegion(long id);

        //ReturnRoute
        Task<ApiResponse> AddSMOReturnRoute(HttpContext context, SMOReturnRouteReceivingDTO sMOReturnRouteReceivingDTO);
        Task<ApiResponse> UpdateSMOReturnRoute(HttpContext context, long id, SMOReturnRouteReceivingDTO sMOReturnRouteReceivingDTO);
        Task<ApiResponse> GetSMOReturnRouteById(long id);

        Task<ApiResponse> GetAllSMOReturnRoutes();
        Task<ApiResponse> DeleteSMOReturnRoute(long id);
    }
}
