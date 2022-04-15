using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Halobiz.Common.Auths;
using Halobiz.Common.DTOs.ApiDTOs;
using HaloBiz.DTOs.ReceivingDTOs;
using HaloBiz.DTOs.TransferDTOs.LAMS;
using HaloBiz.MyServices;
using Microsoft.AspNetCore.Mvc;
//using Controllers.Models;

namespace HaloBiz.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    [ModuleName(HalobizModules.Supplier,105)]

    public class SupplierCategoryController : ControllerBase
    {
        private readonly ISupplierCategoryService _supplierCategoryService;

        public SupplierCategoryController(ISupplierCategoryService supplierCategoryService)
        {
            this._supplierCategoryService = supplierCategoryService;
        }

        [HttpGet("")]
        public async Task<ApiCommonResponse> GetSupplierCategory()
        {
            return await _supplierCategoryService.GetAllSupplierCategories(); 
        }

        [HttpPost("")]
        public async Task<ApiCommonResponse> AddNewSupplierCategory(SupplierCategoryReceivingDTO supplierCategoryReceiving)
        {
            return await _supplierCategoryService.AddSupplierCategory(HttpContext, supplierCategoryReceiving); 
        }
        [HttpGet("{id}")]
        public async Task<ApiCommonResponse> GetById(long id)
        {
            return await _supplierCategoryService.GetSupplierCategoryById(id); 
        }

        [HttpPut("{id}")]
        public async Task<ApiCommonResponse> UpdateById(long id, SupplierCategoryReceivingDTO supplierCategoryReceiving)
        {
            return await _supplierCategoryService.UpdateSupplierCategory(HttpContext, id, supplierCategoryReceiving); 
        }

        [HttpDelete("{id}")]
        public async Task<ApiCommonResponse> DeleteById(int id)
        {
            return await _supplierCategoryService.DeleteSupplierCategory(id);
         }

    }
}