using Halobiz.Common.DTOs.ApiDTOs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OnlinePortalBackend.MyServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OnlinePortalBackend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomersController : ControllerBase
    {
        private readonly ICustomer _customer;
        public CustomersController(ICustomer customer)
        {
            _customer = customer;
        }

        [HttpGet("GetCustomerById/{Id}")]
        public async Task<ApiCommonResponse> GetCustomer(long Id)
        {
            return await _customer.GetCustomerInfo(Id);
        }

        [HttpGet("GetCustomerByEmail/{email}")]
        public async Task<ApiCommonResponse> GetCustomerByEmail(string email)
        {
            return await _customer.GetCustomerInfo(email);
        }
    }
}
