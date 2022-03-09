using HalobizMigrations.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OnlinePortalBackend.DTOs.ApiDTOs;
using OnlinePortalBackend.DTOs.ReceivingDTOs;
using OnlinePortalBackend.DTOs.TransferDTOs;
using OnlinePortalBackend.MyServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OnlinePortalBackend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProspectController : ControllerBase
    {
        private readonly IProspectService _prospectService;
        private readonly HalobizContext _context;
        public ProspectController(
            HalobizContext context,
            IProspectService prospectService)
        {
            _context = context;
            _prospectService = prospectService;
        }

        [HttpPost("SaveCartItems/{prospectId}")]
        public async Task<ActionResult> SaveCartItems(CartItemsReceivingDTO cartItems, long prospectId)
        {
            var response = await _prospectService.SaveCartItems(HttpContext, cartItems, prospectId);
            if (response.StatusCode >= 400)
                return StatusCode(response.StatusCode, response);
            var theResponse = ((ApiOkResponse)response).Result;
            return Ok(theResponse);
        }

        [HttpGet("GetCartItems/{prospectId}")]
        public async Task<ActionResult> GetCartItems(long prospectId)
        {
            var response = await _prospectService.GetCartItems(HttpContext, prospectId);
            if (response.StatusCode >= 400)
                return StatusCode(response.StatusCode, response);
            var theResponse = ((ApiOkResponse)response).Result;
            return Ok(theResponse);
        }

        [HttpGet("GetLeadDivisions/{prospectId}")]
        public async Task<ActionResult> GetLeadDivisions(long prospectId)
        {
            var response = await _prospectService.GetLeadDivisions(HttpContext, prospectId);
            if (response.StatusCode >= 400)
                return StatusCode(response.StatusCode, response);
            var theResponse = ((ApiOkResponse)response).Result;
            return Ok(theResponse);
        }
    }
}
