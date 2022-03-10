using Microsoft.AspNetCore.Mvc;
using OnlinePortalBackend.DTOs.ApiDTOs;
using OnlinePortalBackend.DTOs.ReceivingDTOs;
using OnlinePortalBackend.DTOs.TransferDTOs;
using OnlinePortalBackend.MyServices;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace OnlinePortalBackend.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class ExistingCustomerController : ControllerBase
    {
        private readonly IExistingCustomerService _existingCustomerService;
        public ExistingCustomerController(IExistingCustomerService existingCustomerService)
        {
            _existingCustomerService = existingCustomerService;
        }

        [HttpGet("GetCustomerByEmail/{email}")]
        public async Task<ActionResult> GetCustomerByEmail(string email)
        {
            var response = await _existingCustomerService.GetCustomerByEmail(email);
            if (response.StatusCode >= 400)
                return StatusCode(response.StatusCode, response);
            var customer = ((ApiOkResponse)response).Result;
            return Ok(customer);
        }
    }
}
