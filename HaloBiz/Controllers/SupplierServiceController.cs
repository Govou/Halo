
using Halobiz.Common.Auths;
using Halobiz.Common.DTOs.ApiDTOs;
using HaloBiz.DTOs.ReceivingDTOs;
using HaloBiz.MyServices;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace HaloBiz.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    [ModuleName(HalobizModules.Supplier)]

    public class SupplierServiceController : Controller
    {
       
            private readonly ISupplierServiceService _supplierCategoryService;

            public SupplierServiceController(ISupplierServiceService supplierCategoryService)
            {
                this._supplierCategoryService = supplierCategoryService;
            }

            [HttpGet("")]
            public async Task<ApiCommonResponse> GetSupplierService()
            {
                return await _supplierCategoryService.GetAllSupplierServiceCategories();
            }
            [HttpGet("{id}")]
            public async Task<ApiCommonResponse> GetById(long id)
            {
                return await _supplierCategoryService.GetSupplierServiceById(id);
            }

            [HttpPost("")]
            public async Task<ApiCommonResponse> AddNewSupplierService(SupplierServiceReceivingDTO supplierCategoryReceiving)
            {
                return await _supplierCategoryService.AddSupplierService(HttpContext, supplierCategoryReceiving);
            }

            [HttpPut("{id}")]
            public async Task<ApiCommonResponse> UpdateById(long id, SupplierServiceReceivingDTO supplierCategoryReceiving)
            {
                return await _supplierCategoryService.UpdateSupplierService(HttpContext, id, supplierCategoryReceiving);
            }

            [HttpDelete("{id}")]
            public async Task<ApiCommonResponse> DeleteById(int id)
            {
                return await _supplierCategoryService.DeleteSupplierService(id);
            }

    }
}
