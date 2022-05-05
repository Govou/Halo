using Halobiz.Common.Auths;
using Halobiz.Common.DTOs.ApiDTOs;
using HaloBiz.DTOs.ReceivingDTOs;
using HaloBiz.MyServices;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HaloBiz.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    [ModuleName(HalobizModules.Setups,80)]

    public class PriceRegisterController : ControllerBase
    {
        private readonly IPriceRegisterService _priceRegisterService;

        public PriceRegisterController(IPriceRegisterService priceRegisterService)
        {
            _priceRegisterService = priceRegisterService;
        }

        [HttpGet("GetAllPriceRegisters")]
        public async Task<ApiCommonResponse> GetAllPriceRegisters()
        {
            return await _priceRegisterService.GetAllPriceRegisters();
        }

        [HttpGet("GetAllPriceRegistersByRouteId/{routeId}")]
        public async Task<ApiCommonResponse> GetAllPriceRegistersByRouteId(long routeId)
        {
            return await _priceRegisterService.GetAllPriceRegistersByRouteId(routeId);
        }

        [HttpGet("GetAllPriceRegistersByServiceCategoryId/{categoryId}")]
        public async Task<ApiCommonResponse> GetAllPriceRegistersByServiceCategoryId(long categoryId)
        {
            return await _priceRegisterService.GetAllPriceRegistersByServiceCategoryId(categoryId);
        }
        [HttpGet("GetPriceRegisterById/{id}")]
        public async Task<ApiCommonResponse> GetPriceRegisterById(long id)
        {
            return await _priceRegisterService.GetPriceRegisterId(id);
        }

        [HttpPost("AddNewPriceRegister")]
        public async Task<ApiCommonResponse> AddNewPriceRegister(PriceRegisterReceivingDTO ReceivingDTO)
        {
            return await _priceRegisterService.AddPriceRegister(HttpContext, ReceivingDTO);
        }

        [HttpPut("UpdatePriceRegisterById/{id}")]
        public async Task<ApiCommonResponse> UpdateTypeById(long id, PriceRegisterReceivingDTO Receiving)
        {
            return await _priceRegisterService.UpdatePriceRegister(HttpContext, id, Receiving);
        }

        [HttpDelete("DeletePriceRegisterById/{id}")] 
        public async Task<ApiCommonResponse> DeletePriceRegisterById(int id)
        {
            return await _priceRegisterService.DeletePriceRegister(id);
        }
    }
}
