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
        private readonly HalobizContext _context;

        public SMORouteAndRegionController(ISMORouteAndRegionService sMORoutesAndRegionServices)
        {
            _sMORoutesAndRegionServices = sMORoutesAndRegionServices;
           // _context = context;
        }

        [HttpGet("GetAllRegions")]
        public async Task<ActionResult> GetAllRegions()
        {
            var response = await _sMORoutesAndRegionServices.GetAllSMORegions();
            if (response.StatusCode >= 400)
                return StatusCode(response.StatusCode, response);
            var items = ((ApiOkResponse)response).Result;
            return Ok(items);
        }

        [HttpGet("GetAllReturnRoutes")]
        public async Task<ActionResult> GetAllReturnRoutes()
        {
            var response = await _sMORoutesAndRegionServices.GetAllSMOReturnRoutes();
            if (response.StatusCode >= 400)
                return StatusCode(response.StatusCode, response);
            var items = ((ApiOkResponse)response).Result;
            return Ok(items);
        }

        [HttpGet("GetAllRoutes")]
        public async Task<ActionResult> GetAllRoutes()
        {
            var response = await _sMORoutesAndRegionServices.GetAllSMORoutes();
            if (response.StatusCode >= 400)
                return StatusCode(response.StatusCode, response);
            var items = ((ApiOkResponse)response).Result;
            return Ok(items);
        }

        [HttpGet("GetRoutesByName")]
        public async Task<ActionResult> GetRoutesByName(string routeName)
        {
            var response = await _sMORoutesAndRegionServices.GetAllSMORoutesByName(routeName);
            if (response.StatusCode >= 400)
                return StatusCode(response.StatusCode, response);
            var items = ((ApiOkResponse)response).Result;
            return Ok(items);
        }

        //[HttpGet("GetAllRouteRegions")]
        //public async Task<ActionResult> GetAllRouteRegions()
        //{
        //    return new SMORouteRegionTransferDTO()
        //    {
        //       SMORoutes = _sMORoutesAndRegionServices.GetAllSMORoutes(),
        //       SMORegions = _sMORoutesAndRegionServices.GetAllSMORegions()
        //    };
        //    //var response = await _sMORoutesAndRegionServices.GetAllSMORoutes();
        //    //if (response.StatusCode >= 400)
        //    //    return StatusCode(response.StatusCode, response);
        //    //var items = ((ApiOkResponse)response).Result;
        //    //return Ok(items);
        //}

        [HttpGet("GetAllSMORoutesWithReturnRoute")]
        public async Task<ActionResult> GetAllSMORoutesWithReturnRoute()
        {
            var response = await _sMORoutesAndRegionServices.GetAllSMORoutesWithReturnRoute();
            if (response.StatusCode >= 400)
                return StatusCode(response.StatusCode, response);
            var items = ((ApiOkResponse)response).Result;
            return Ok(items);
        }

        [HttpGet("GetRegionById/{id}")]
        public async Task<ActionResult> GetRegionById(long id)
        {
            var response = await _sMORoutesAndRegionServices.GetSMORegionById(id);
            if (response.StatusCode >= 400)
                return StatusCode(response.StatusCode, response);
            var item = ((ApiOkResponse)response).Result;
            return Ok(item);
        }

        [HttpGet("GetReturnRouteById/{id}")]
        public async Task<ActionResult> GetReturnRouteById(long id)
        {
            var response = await _sMORoutesAndRegionServices.GetSMOReturnRouteById(id);
            if (response.StatusCode >= 400)
                return StatusCode(response.StatusCode, response);
            var item = ((ApiOkResponse)response).Result;
            return Ok(item);
        }

        [HttpGet("GetRouteById/{id}")]
        public async Task<ActionResult> GetRouteById(long id)
        {
            var response = await _sMORoutesAndRegionServices.GetSMORouteById(id);
            if (response.StatusCode >= 400)
                return StatusCode(response.StatusCode, response);
            var item = ((ApiOkResponse)response).Result;
            return Ok(item);
        }

        [HttpPost("AddNewRegion")]
        public async Task<ActionResult> AddNewRegion(SMORegionReceivingDTO ReceivingDTO)
        {
            var response = await _sMORoutesAndRegionServices.AddSMORegion(HttpContext, ReceivingDTO);
            if (response.StatusCode >= 400)
                return StatusCode(response.StatusCode, response);
            var item = ((ApiOkResponse)response).Result;
            return Ok(item);
        }

        [HttpPost("AddNewReturnRoute")]
        public async Task<ActionResult> AddNewReturnRoute(SMOReturnRouteReceivingDTO ReceivingDTO)
        {
            var response = await _sMORoutesAndRegionServices.AddSMOReturnRoute(HttpContext, ReceivingDTO);
            if (response.StatusCode >= 400)
                return StatusCode(response.StatusCode, response);
            var item = ((ApiOkResponse)response).Result;
            return Ok(item);
        }

        [HttpPost("AddNewRoute")]
        public async Task<ActionResult> AddNewRoute(SMORouteReceivingDTO ReceivingDTO)
        {
            var response = await _sMORoutesAndRegionServices.AddSMORoute(HttpContext, ReceivingDTO);
            if (response.StatusCode >= 400)
                return StatusCode(response.StatusCode, response);
            var item = ((ApiOkResponse)response).Result;
            return Ok(item);
        }

        [HttpPut("UpdateRegionById/{id}")]
        public async Task<IActionResult> UpdateRegionById(long id, SMORegionReceivingDTO Receiving)
        {
            var response = await _sMORoutesAndRegionServices.UpdateSMORegion(HttpContext, id, Receiving);
            if (response.StatusCode >= 400)
                return StatusCode(response.StatusCode, response);
            var item = ((ApiOkResponse)response).Result;
            return Ok(item);
        }

        [HttpPut("UpdateReturnRouteById/{id}")]
        public async Task<IActionResult> UpdateReturnRouteById(long id, SMOReturnRouteReceivingDTO Receiving)
        {
            var response = await _sMORoutesAndRegionServices.UpdateSMOReturnRoute(HttpContext, id, Receiving);
            if (response.StatusCode >= 400)
                return StatusCode(response.StatusCode, response);
            var item = ((ApiOkResponse)response).Result;
            return Ok(item);
        }

        [HttpPut("UpdateRouteById/{id}")]
        public async Task<IActionResult> UpdateRouteById(long id, SMORouteReceivingDTO Receiving)
        {
            var response = await _sMORoutesAndRegionServices.UpdateSMORoute(HttpContext, id, Receiving);
            if (response.StatusCode >= 400)
                return StatusCode(response.StatusCode, response);
            var item = ((ApiOkResponse)response).Result;
            return Ok(item);
        }

        [HttpDelete("DeleteRegionById/{id}")]
        public async Task<ActionResult> DeleteRegionById(int id)
        {
            var response = await _sMORoutesAndRegionServices.DeleteSMORegion(id);
            return StatusCode(response.StatusCode);
        }

        [HttpDelete("DeleteReturnRouteById/{id}")]
        public async Task<ActionResult> DeleteReturnRouteById(int id)
        {
            var response = await _sMORoutesAndRegionServices.DeleteSMOReturnRoute(id);
            return StatusCode(response.StatusCode);
        }

        [HttpDelete("DeleteRouteById/{id}")]
        public async Task<ActionResult> DeleteRouteById(int id)
        {
            var response = await _sMORoutesAndRegionServices.DeleteSMORoute(id);
            return StatusCode(response.StatusCode);
        }

    }
}
