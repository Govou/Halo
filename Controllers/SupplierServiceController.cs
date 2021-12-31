using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HaloBiz.DTOs.ApiDTOs;
using HaloBiz.DTOs.ReceivingDTOs;
using HaloBiz.DTOs.TransferDTOs.LAMS;
using HaloBiz.MyServices;
using Microsoft.AspNetCore.Mvc;
//using Controllers.Models;

namespace HaloBiz.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class SupplierServiceController : ControllerBase
    {
        private readonly ISupplierServiceService _supplierCategoryService;

        public SupplierServiceController(ISupplierServiceService supplierCategoryService)
        {
            this._supplierCategoryService = supplierCategoryService;
        }

        [HttpGet("")]
        public async Task<ApiCommonResponse> GetSupplierService()
        {
            var response = await _supplierCategoryService.GetAllSupplierServiceCategories();
            if (response.StatusCode >= 400)
                return StatusCode(response.StatusCode, response);
            var supplierCategory = ((ApiOkResponse)response).Result;
            return Ok(supplierCategory);
        }
        [HttpGet("{id}")]
        public async Task<ApiCommonResponse> GetById(long id)
        {
            var response = await _supplierCategoryService.GetSupplierServiceById(id);
            if (response.StatusCode >= 400)
                return StatusCode(response.StatusCode, response);
            var groupType = ((ApiOkResponse)response).Result;
            return Ok(groupType);
        }

        [HttpPost("")]
        public async Task<ApiCommonResponse> AddNewSupplierService(SupplierServiceReceivingDTO supplierCategoryReceiving)
        {
            var response = await _supplierCategoryService.AddSupplierService(HttpContext, supplierCategoryReceiving);
            if (response.StatusCode >= 400)
                return StatusCode(response.StatusCode, response);
            var supplierCategory = ((ApiOkResponse)response).Result;
            return Ok(supplierCategory);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateById(long id, SupplierServiceReceivingDTO supplierCategoryReceiving)
        {
            var response = await _supplierCategoryService.UpdateSupplierService(HttpContext, id, supplierCategoryReceiving);
            if (response.StatusCode >= 400)
                return StatusCode(response.StatusCode, response);
            var supplierCategory = ((ApiOkResponse)response).Result;
            return Ok(supplierCategory);
        }

        [HttpDelete("{id}")]
        public async Task<ApiCommonResponse> DeleteById(int id)
        {
            var response = await _supplierCategoryService.DeleteSupplierService(id);
            return StatusCode(response.StatusCode);
        }

    }
}