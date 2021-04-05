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
    public class SupplierController : ControllerBase
    {
        private readonly ISupplierService _supplierCategoryService;

        public SupplierController(ISupplierService supplierCategoryService)
        {
            this._supplierCategoryService = supplierCategoryService;
        }

        [HttpGet("")]
        public async Task<ActionResult> GetSupplier()
        {
            var response = await _supplierCategoryService.GetAllSupplierCategories();
            if (response.StatusCode >= 400)
                return StatusCode(response.StatusCode, response);
            var supplierCategory = ((ApiOkResponse)response).Result;
            return Ok(supplierCategory);
        }

        [HttpPost("")]
        public async Task<ActionResult> AddNewSupplier(SupplierReceivingDTO supplierCategoryReceiving)
        {
            var response = await _supplierCategoryService.AddSupplier(HttpContext, supplierCategoryReceiving);
            if (response.StatusCode >= 400)
                return StatusCode(response.StatusCode, response);
            var supplierCategory = ((ApiOkResponse)response).Result;
            return Ok(supplierCategory);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateById(long id, SupplierReceivingDTO supplierCategoryReceiving)
        {
            var response = await _supplierCategoryService.UpdateSupplier(HttpContext, id, supplierCategoryReceiving);
            if (response.StatusCode >= 400)
                return StatusCode(response.StatusCode, response);
            var supplierCategory = ((ApiOkResponse)response).Result;
            return Ok(supplierCategory);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteById(int id)
        {
            var response = await _supplierCategoryService.DeleteSupplier(id);
            return StatusCode(response.StatusCode);
        }

    }
}