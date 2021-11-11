using HalobizMigrations.Data;
using HaloBiz.Helpers;
using HalobizMigrations.Models.Armada;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using HaloBiz.DTOs.SMODTO;
using AutoMapper;
using HaloBiz.Repository;
using HaloBiz.DTOs.ApiDTOs;
using HaloBiz.DTOs.ReceivingDTOs;
using HaloBiz.DTOs.TransferDTOs;

namespace HaloBiz.MyServices.Impl
{
    public class SMORouteAndRegionServiceImpl:ISMORouteAndRegionService
    {
        private readonly HalobizContext _context;
        private readonly IMapper _mapper;
        private readonly ISMORouteAndRegionRepository _sMORouteAndRegionRepository;

        private readonly ILogger<SMORouteAndRegionServiceImpl> _logger;

        public SMORouteAndRegionServiceImpl(HalobizContext context, ILogger<SMORouteAndRegionServiceImpl> logger, IMapper mapper, ISMORouteAndRegionRepository sMORouteAndRegionRepository)
        {
            _context = context;
            _mapper = mapper;
            _logger = logger;
            _sMORouteAndRegionRepository = sMORouteAndRegionRepository;
        }

        public async Task<ApiResponse> AddSMORoute(HttpContext context, SMORouteAndRegionReceivingDTO sMORouteDTO)
        {
            var route = _mapper.Map<SMORoute>(sMORouteDTO);
            route.CreatedById = context.GetLoggedInUserId();
            var savedRoute = await _sMORouteAndRegionRepository.SaveSMORoute(route);
            if (savedRoute == null)
            {
                return new ApiResponse(500);
            }
            var routeTransferDTO = _mapper.Map<SMORouteTransferDTO>(route);
            return new ApiOkResponse(routeTransferDTO);
           
        }

        public async Task<ApiResponse> UpdateSMORoute(HttpContext httpContext, SMORouteAndRegionReceivingDTO sMORouteDTO, long id)
        {

            var routeToUpdate = await _sMORouteAndRegionRepository.FindSMORouteById(id);
            if (routeToUpdate == null)
            {
                return new ApiResponse(404);
            }

            var summary = $"Initial details before change, \n {routeToUpdate.ToString()} \n";

            routeToUpdate.RouteName = sMORouteDTO.RouteName;
            routeToUpdate.RouteDescription = sMORouteDTO.RouteDescription;
            routeToUpdate.UpdatedAt = DateTime.UtcNow;

            var updatedRoute = await _sMORouteAndRegionRepository.UpdateSMORoute(routeToUpdate);

            summary += $"Details after change, \n {updatedRoute.ToString()} \n";

            if (updatedRoute == null)
            {
                return new ApiResponse(500);
            }

            var routeTransferDTOs = _mapper.Map<SMORouteTransferDTO>(updatedRoute);
            return new ApiOkResponse(sMORouteDTO);


        }

        public async Task<StatusResponse> GetSMORouteById(long id)
        {
            try
            {
                var getRoute = await _context.SMORoutes.FirstOrDefaultAsync(route => route.Id == id);
                if (getRoute == null)
                {
                    return StatusResponse.ErrorMessage($"Route with Id {id} does not exist");
                }
                return StatusResponse.SuccessMessage("Route Updated Successfully", getRoute);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                throw;
            }
        }

        public async Task<StatusResponse> GetAllSMORoutes()
        {
            try
            {
                var getallRoutes = await _context.SMORoutes.ToListAsync();
                return StatusResponse.SuccessMessage("Routes Loaded Successfully", getallRoutes);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                throw;
            }
        }

        //Region
        public async Task<ApiResponse> AddSMORegion(HttpContext context, SMORegionReceivingDTO sMORegionDTO)
        {
            var region = _mapper.Map<SMORegion>(sMORegionDTO);
            region.CreatedById = context.GetLoggedInUserId();
            region.CreatedAt = DateTime.UtcNow;
            var savedRegion = await _sMORouteAndRegionRepository.SaveSMORegion(region);
            if (savedRegion == null)
            {
                return new ApiResponse(500);
            }
            var regionTransferDTO = _mapper.Map<SMORegionTransferDTO>(region);
            return new ApiOkResponse(regionTransferDTO);

        }

        public async Task<ApiResponse> UpdateSMORegion(HttpContext httpContext, SMORegionReceivingDTO sMORegionDTO, long id)
        {

            var regionToUpdate = await _sMORouteAndRegionRepository.FindSMORegionById(id);
            if (regionToUpdate == null)
            {
                return new ApiResponse(404);
            }

            var summary = $"Initial details before change, \n {regionToUpdate.ToString()} \n";

            regionToUpdate.RegionName = sMORegionDTO.RegionName;
            regionToUpdate.RegionDescription = sMORegionDTO.RegionDescription;
            regionToUpdate.UpdatedAt = DateTime.UtcNow;

            var updatedRegion = await _sMORouteAndRegionRepository.UpdateSMORegion(regionToUpdate);

            summary += $"Details after change, \n {regionToUpdate.ToString()} \n";

            if (updatedRegion == null)
            {
                return new ApiResponse(500);
            }

            var routeTransferDTOs = _mapper.Map<SMORouteTransferDTO>(updatedRegion);
            return new ApiOkResponse(sMORegionDTO);


        }

        public async Task<StatusResponse> GetSMORegionById(long id)
        {
            try
            {
                var getRegion = await _context.SMORegions.FirstOrDefaultAsync(r => r.Id == id);
                if (getRegion == null)
                {
                    return StatusResponse.ErrorMessage($"Route with Id {id} does not exist");
                }
                return StatusResponse.SuccessMessage("Route Updated Successfully", getRegion);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                throw;
            }
        }

        public async Task<StatusResponse> GetAllSMORegions()
        {
            try
            {
                var getallRegions = await _context.SMORegions.ToListAsync();
                return StatusResponse.SuccessMessage("Routes Loaded Successfully", getallRegions);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                throw;
            }
        }
    }


}
