using HaloBiz.DTOs.ApiDTOs;
using HaloBiz.DTOs.ReceivingDTOs;
using HaloBiz.Helpers;
using HaloBiz.MyServices;
using HalobizMigrations.Data;
//using HaloBiz.SMOServices;
using HalobizMigrations.Models.Armada;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace HaloBiz.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SMORouteAndRegionController : ControllerBase
    {
        private readonly ISMORouteAndRegionService _sMORoutesAndRegionServices;
        //private readonly SMORoutesAndRegionServices _sMORoutesAndRegionServices;
        private readonly HalobizContext _context;

        public SMORouteAndRegionController(ISMORouteAndRegionService sMORoutesAndRegionServices, HalobizContext context)
        {
            _sMORoutesAndRegionServices = sMORoutesAndRegionServices;
            _context = context;
        }
        // GET: api/<SMORouteAndRegionController>
        [HttpGet("GetAllSMORoutes")]
        //[Route]
        //[Produces("application/json", "application/xml", Type = typeof(StatusResponse))]
        public async Task<IActionResult> GetAllSMORoutes()
        {
            try
            {
                var resp = await _sMORoutesAndRegionServices.GetAllSMORoutes();
                if (resp.status)
                {
                    return Ok(StatusResponse.SuccessMessage());
                }
                return resp.statusCode > 0 ? StatusCode(resp.statusCode, resp) : StatusCode(StatusCodes.Status400BadRequest, resp);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpGet("GetSMORouteById/{id}")]
        //[Route("GetSMORouteById")]
        public async Task<IActionResult> GetSMORouteById(long id)
        {
            try
            {
                var resp = await _sMORoutesAndRegionServices.GetSMORouteById(id);
                if (resp.status)
                {
                    return Ok(StatusResponse.SuccessMessage());
                }
                return resp.statusCode > 0 ? StatusCode(resp.statusCode, resp) : StatusCode(StatusCodes.Status400BadRequest, resp);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpPost("AddRoute")]
        //[Produces("application/json", "application/xml", Type = typeof(StatusResponse))]
        public async Task<IActionResult> AddSMORoute(SMORouteAndRegionReceivingDTO sMORoute)
        {
            try
            {
                var response = await _sMORoutesAndRegionServices.AddSMORoute(HttpContext, sMORoute);
                if (response.StatusCode >= 400)
                    return StatusCode(response.StatusCode, response);
                var route = ((ApiOkResponse)response).Result;
                return Ok(route);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
           
        }

        [HttpPut("UpdateRouteById/{id}")]
        public async Task<IActionResult> UpdateSMORoute(long id, SMORouteAndRegionReceivingDTO sMORoute)
        {
            var response = await _sMORoutesAndRegionServices.UpdateSMORoute(HttpContext, sMORoute, id);
            if (response.StatusCode >= 400)
                return StatusCode(response.StatusCode, response);
            var region = ((ApiOkResponse)response).Result;
            return Ok(region);
        }

        //Region
        [HttpPost("AddRegion")]
        //[Produces("application/json", "application/xml", Type = typeof(StatusResponse))]
        public async Task<IActionResult> AddSMORegion(SMORegionReceivingDTO sMORegion)
        {
            try
            {
                var response = await _sMORoutesAndRegionServices.AddSMORegion(HttpContext, sMORegion);
                if (response.StatusCode >= 400)
                    return StatusCode(response.StatusCode, response);
                var region = ((ApiOkResponse)response).Result;
                return Ok(region);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }

        }

        [HttpPut("UpdateRegionById/{id}")]
        public async Task<IActionResult> UpdateSMORegion(long id, SMORegionReceivingDTO sMORegion)
        {
            var response = await _sMORoutesAndRegionServices.UpdateSMORegion(HttpContext, sMORegion, id);
            if (response.StatusCode >= 400)
                return StatusCode(response.StatusCode, response);
            var region = ((ApiOkResponse)response).Result;
            return Ok(region);
        }
    }
}
