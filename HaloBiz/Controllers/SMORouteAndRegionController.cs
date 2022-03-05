using HaloBiz.DTOs.ApiDTOs;
using HaloBiz.DTOs.ReceivingDTOs;
using HaloBiz.DTOs.TransferDTOs;
using HaloBiz.Helpers;
using HaloBiz.MyServices;
using HalobizMigrations.Data;
//using HaloBiz.SMOServices;
using HalobizMigrations.Models.Armada;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace HaloBiz.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class SMORouteAndRegionController : ControllerBase
    {
        private readonly ISMORouteAndRegionService _sMORoutesAndRegionServices;
        //private readonly SMORoutesAndRegionServices _sMORoutesAndRegionServices;

        public SMORouteAndRegionController(ISMORouteAndRegionService sMORoutesAndRegionServices)
        {
            _sMORoutesAndRegionServices = sMORoutesAndRegionServices;
           // _context = context;
        }

        [HttpGet("GetAllRegions")]
        public async Task<ApiCommonResponse> GetAllRegions()
        {
            return await _sMORoutesAndRegionServices.GetAllSMORegions(); 
        }

        [HttpGet("GetAllReturnRoutes")]
        public async Task<ApiCommonResponse> GetAllReturnRoutes()
        {
            return await _sMORoutesAndRegionServices.GetAllSMOReturnRoutes(); 
        }

        [HttpGet("GetAllRoutes")]
        public async Task<ApiCommonResponse> GetAllRoutes()
        {
            return await _sMORoutesAndRegionServices.GetAllSMORoutes(); 
        }

        [HttpGet("GetAllRouteMaps")]
        public async Task<ApiCommonResponse> GetAllRouteMaps()
        {
            return await _sMORoutesAndRegionServices.GetAllSMORouteMaps();
        }

        [HttpGet("GetRoutesByName")]
        public async Task<ApiCommonResponse> GetRoutesByName(string routeName)
        {
            return await _sMORoutesAndRegionServices.GetAllSMORoutesByName(routeName); 
        }

        //[HttpGet("GetAllRouteRegions")]
        //public async Task<ApiCommonResponse> GetAllRouteRegions()
        //{
        //    return new SMORouteRegionTransferDTO()
        //    {
        //       SMORoutes = _sMORoutesAndRegionServices.GetAllSMORoutes(),
        //       SMORegions = _sMORoutesAndRegionServices.GetAllSMORegions()
        //    };
        //    //return await _sMORoutesAndRegionServices.GetAllSMORoutes();
        //    //
        //    //    
        //    //var items = ((ApiOkResponse)response).Result;
        //    //return Ok(items);
        //}

        [HttpGet("GetAllSMORoutesWithReturnRoute")]
        public async Task<ApiCommonResponse> GetAllSMORoutesWithReturnRoute()
        {
            return await _sMORoutesAndRegionServices.GetAllSMORoutesWithReturnRoute(); 
        }

        [HttpGet("GetRegionById/{id}")]
        public async Task<ApiCommonResponse> GetRegionById(long id)
        {
            return await _sMORoutesAndRegionServices.GetSMORegionById(id); 
        }

        [HttpGet("GetReturnRouteById/{id}")]
        public async Task<ApiCommonResponse> GetReturnRouteById(long id)
        {
            return await _sMORoutesAndRegionServices.GetSMOReturnRouteById(id); 
        }

        [HttpGet("GetRouteById/{id}")]
        public async Task<ApiCommonResponse> GetRouteById(long id)
        {
            return await _sMORoutesAndRegionServices.GetSMORouteById(id); 
        }

        [HttpGet("GetRouteMapById/{id}")]
        public async Task<ApiCommonResponse> GetRouteMapById(long id)
        {
            return await _sMORoutesAndRegionServices.GetSMORouteMapById(id);
        }

        [HttpGet("GetAllRouteMapsByRouteId/{id}")]
        public async Task<ApiCommonResponse> GetAllRouteMapsByRouteId(long id)
        {
            return await _sMORoutesAndRegionServices.GetAllRouteMapsByRouteId(id);
        }
        [HttpPost("AddNewRegion")]
        public async Task<ApiCommonResponse> AddNewRegion(SMORegionReceivingDTO ReceivingDTO)
        {
            return await _sMORoutesAndRegionServices.AddSMORegion(HttpContext, ReceivingDTO); 
        }

        [HttpPost("AddNewReturnRoute")]
        public async Task<ApiCommonResponse> AddNewReturnRoute(SMOReturnRouteReceivingDTO ReceivingDTO)
        {
            return await _sMORoutesAndRegionServices.AddSMOReturnRoute(HttpContext, ReceivingDTO); 
        }

        [HttpPost("AddNewRoute")]
        public async Task<ApiCommonResponse> AddNewRoute(SMORouteReceivingDTO ReceivingDTO)
        {
            return await _sMORoutesAndRegionServices.AddSMORoute(HttpContext, ReceivingDTO); 
        }

        [HttpPost("AddNewRouteMap")]
        public async Task<ApiCommonResponse> AddNewRouteMap(SMORouteMapReceivingDTO ReceivingDTO)
        {
            return await _sMORoutesAndRegionServices.AddSMORouteMap(HttpContext, ReceivingDTO);
        }

        [HttpPut("UpdateRegionById/{id}")]
        public async Task<ApiCommonResponse> UpdateRegionById(long id, SMORegionReceivingDTO Receiving)
        {
            return await _sMORoutesAndRegionServices.UpdateSMORegion(HttpContext, id, Receiving); 
        }

        [HttpPut("UpdateReturnRouteById/{id}")]
        public async Task<ApiCommonResponse> UpdateReturnRouteById(long id, SMOReturnRouteReceivingDTO Receiving)
        {
            return await _sMORoutesAndRegionServices.UpdateSMOReturnRoute(HttpContext, id, Receiving); 
        }

        [HttpPut("UpdateRouteById/{id}")]
        public async Task<ApiCommonResponse> UpdateRouteById(long id, SMORouteReceivingDTO Receiving)
        {
            return await _sMORoutesAndRegionServices.UpdateSMORoute(HttpContext, id, Receiving); 
        }

        [HttpDelete("DeleteRegionById/{id}")]
        public async Task<ApiCommonResponse> DeleteRegionById(int id)
        {
            return await _sMORoutesAndRegionServices.DeleteSMORegion(id); 
        }

        [HttpDelete("DeleteReturnRouteById/{id}")]
        public async Task<ApiCommonResponse> DeleteReturnRouteById(int id)
        {
            return await _sMORoutesAndRegionServices.DeleteSMOReturnRoute(id);
        }

        [HttpDelete("DeleteRouteById/{id}")]
        public async Task<ApiCommonResponse> DeleteRouteById(int id)
        {
            return await _sMORoutesAndRegionServices.DeleteSMORoute(id);
        }

        [HttpDelete("DeleteRouteMapById/{id}")]
        public async Task<ApiCommonResponse> DeleteRouteMapById(int id)
        {
            return await _sMORoutesAndRegionServices.DeleteSMORouteMap(id);
        }

    }
}
