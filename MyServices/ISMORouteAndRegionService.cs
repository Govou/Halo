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
        Task<ApiResponse> AddSMORoute(HttpContext context, SMORouteAndRegionReceivingDTO sMORouteReceivingDTO);
        Task<ApiResponse> UpdateSMORoute(HttpContext context, SMORouteAndRegionReceivingDTO sMORouteReceivingDTO, long id);
        Task<StatusResponse> GetSMORouteById(long id);

        Task<StatusResponse> GetAllSMORoutes();

        ////Region
        Task<ApiResponse> AddSMORegion(HttpContext context, SMORegionReceivingDTO sMOReceivingDTO);
        Task<ApiResponse> UpdateSMORegion(HttpContext context, SMORegionReceivingDTO sMOReceivingDTO, long id);
        Task<StatusResponse> GetSMORegionById(long id);

        Task<StatusResponse> GetAllSMORegions();
    }
}
