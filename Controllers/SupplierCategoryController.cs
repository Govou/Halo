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
    public class SupplierCategoryController : ControllerBase
    {
        private readonly ISupplierCategoryService _supplierCategoryService;

        public SupplierCategoryController(ISupplierCategoryService supplierCategoryService)
        {
            this._supplierCategoryService = supplierCategoryService;
        }

        [HttpGet("")]
        public async Task<ActionResult> GetSupplierCategory()
        {
            var response = await _supplierCategoryService.GetAllSupplierCategories();
            if (response.StatusCode >= 400)
                return StatusCode(response.StatusCode, response);
            var supplierCategory = ((ApiOkResponse)response).Result;
            return Ok(supplierCategory);
        }

        [HttpPost("")]
        public async Task<ActionResult> AddNewSupplierCategory(SupplierCategoryReceivingDTO supplierCategoryReceiving)
        {
            var response = await _supplierCategoryService.AddSupplierCategory(HttpContext, supplierCategoryReceiving);
            if (response.StatusCode >= 400)
                return StatusCode(response.StatusCode, response);
            var supplierCategory = ((ApiOkResponse)response).Result;
            return Ok(supplierCategory);
        }
        [HttpGet("{id}")]
        public async Task<ActionResult> GetById(long id)
        {
            var response = await _supplierCategoryService.GetSupplierCategoryById(id);
            if (response.StatusCode >= 400)
                return StatusCode(response.StatusCode, response);
            var groupType = ((ApiOkResponse)response).Result;
            return Ok(groupType);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateById(long id, SupplierCategoryReceivingDTO supplierCategoryReceiving)
        {
            var response = await _supplierCategoryService.UpdateSupplierCategory(HttpContext, id, supplierCategoryReceiving);
            if (response.StatusCode >= 400)
                return StatusCode(response.StatusCode, response);
            var supplierCategory = ((ApiOkResponse)response).Result;
            return Ok(supplierCategory);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteById(int id)
        {
            var response = await _supplierCategoryService.DeleteSupplierCategory(id);
            return StatusCode(response.StatusCode);
        }

    }
}