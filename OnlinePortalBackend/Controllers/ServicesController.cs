﻿using Halobiz.Common.DTOs.ApiDTOs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OnlinePortalBackend.MyServices;
using System.Threading.Tasks;

namespace OnlinePortalBackend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ServicesController : ControllerBase
    {
        private readonly IServicesService _service;

        public ServicesController(IServicesService service)
        {
            _service = service;
        }

        [HttpGet("GetContractServiceDetails")]
        public async Task<ApiCommonResponse> GetContractServiceDetails(int contractServiceId)
        {
            return await _service.GetServiceDetails(contractServiceId);
          
        }
    }

}
