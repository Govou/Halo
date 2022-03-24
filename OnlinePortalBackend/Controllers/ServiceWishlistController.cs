using Microsoft.AspNetCore.Mvc;
using OnlinePortalBackend.DTOs.ApiDTOs;
using OnlinePortalBackend.DTOs.ReceivingDTOs;
using OnlinePortalBackend.DTOs.TransferDTOs;
using OnlinePortalBackend.MyServices;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HaloBiz.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class ServiceWishlistController : ControllerBase
    {
        private readonly IServiceWishlistService _serviceWishlistService;

        public ServiceWishlistController(IServiceWishlistService serviceWishlistService)
        {
            this._serviceWishlistService = serviceWishlistService;
        }

        [HttpGet("")]
        public async Task<ActionResult> GetServiceWishlists()
        {
            var response = await _serviceWishlistService.FindAllServiceWishlists();
            if (response.StatusCode >= 400)
                return StatusCode(response.StatusCode, response);
            var serviceWishlist = ((ApiOkResponse)response).Result;
            return Ok((IEnumerable<ServiceWishlistTransferDTO>)serviceWishlist);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult> GetById(long id)
        {
            var response = await _serviceWishlistService.FindServiceWishlistById(id);
            if (response.StatusCode >= 400)
                return StatusCode(response.StatusCode, response);
            var serviceWishlist = ((ApiOkResponse)response).Result;
            return Ok((ServiceWishlistTransferDTO)serviceWishlist);
        }

        [HttpGet("GetByProspect/{prospectId}")]
        public async Task<ActionResult> GetByProspect(long prospectId)
        {
            var response = await _serviceWishlistService.FindServiceWishlistsByProspectId(prospectId);
            if (response.StatusCode >= 400)
                return StatusCode(response.StatusCode, response);
            var serviceWishlist = ((ApiOkResponse)response).Result;
            return Ok((IEnumerable<ServiceWishlistTransferDTO>)serviceWishlist);
        }

        [HttpPost("")]
        public async Task<ActionResult> AddNewServiceWishlist(ServiceWishlistReceivingDTO serviceWishlistReceiving)
        {
            var response = await _serviceWishlistService.AddServiceWishlist(HttpContext, serviceWishlistReceiving);
            if (response.StatusCode >= 400)
                return StatusCode(response.StatusCode, response);
            var serviceWishlist = ((ApiOkResponse)response).Result;
            return Ok((ServiceWishlistTransferDTO)serviceWishlist);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateById(long id, ServiceWishlistReceivingDTO serviceWishlistReceiving)
        {
            var response = await _serviceWishlistService.UpdateServiceWishlist(HttpContext, id, serviceWishlistReceiving);
            if (response.StatusCode >= 400)
                return StatusCode(response.StatusCode, response);
            var serviceWishlist = ((ApiOkResponse)response).Result;
            return Ok((ServiceWishlistTransferDTO)serviceWishlist);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteById(int id)
        {
            var response = await _serviceWishlistService.DeleteServiceWishlist(id);
            return StatusCode(response.StatusCode);
        }
    }
}