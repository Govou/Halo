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
        public async Task<ApiCommonResponse> GetSupplier()
        {
            return await _supplierCategoryService.GetAllSupplierCategories(); 
        }
        [HttpGet("{id}")]
        public async Task<ApiCommonResponse> GetById(long id)
        {
            return await _supplierCategoryService.GetSupplierById(id); 
        }

        [HttpPost("")]
        public async Task<ApiCommonResponse> AddNewSupplier(SupplierReceivingDTO supplierCategoryReceiving)
        {
            return await _supplierCategoryService.AddSupplier(HttpContext, supplierCategoryReceiving); 
        }

        [HttpPut("{id}")]
        public async Task<ApiCommonResponse> UpdateById(long id, SupplierReceivingDTO supplierCategoryReceiving)
        {
            return await _supplierCategoryService.UpdateSupplier(HttpContext, id, supplierCategoryReceiving); 
        }

        [HttpDelete("{id}")]
        public async Task<ApiCommonResponse> DeleteById(int id)
        {
            return await _supplierCategoryService.DeleteSupplier(id);
        }

    }
}