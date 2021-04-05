using System.Threading.Tasks;
using HaloBiz.DTOs.ApiDTOs;
using HaloBiz.DTOs.ReceivingDTOs;
using Microsoft.AspNetCore.Http;

namespace HaloBiz.MyServices
{
    public interface ISupplierCategoryService
    {
        Task<ApiResponse> AddSupplierCategory(HttpContext context, SupplierCategoryReceivingDTO supplierCategoryReceivingDTO);
        Task<ApiResponse> GetAllSupplierCategories();
        Task<ApiResponse> UpdateSupplierCategory(HttpContext context, long id, SupplierCategoryReceivingDTO supplierCategoryReceivingDTO);
        Task<ApiResponse> DeleteSupplierCategory(long id);
    }
}