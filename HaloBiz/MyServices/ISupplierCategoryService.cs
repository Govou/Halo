using System.Threading.Tasks;
using HaloBiz.DTOs.ApiDTOs;
using HaloBiz.DTOs.ReceivingDTOs;
using Microsoft.AspNetCore.Http;

namespace HaloBiz.MyServices
{
    public interface ISupplierCategoryService
    {
        Task<ApiCommonResponse> AddSupplierCategory(HttpContext context, SupplierCategoryReceivingDTO supplierCategoryReceivingDTO);
        Task<ApiCommonResponse> GetAllSupplierCategories();
        Task<ApiCommonResponse> UpdateSupplierCategory(HttpContext context, long id, SupplierCategoryReceivingDTO supplierCategoryReceivingDTO);
        Task<ApiCommonResponse> DeleteSupplierCategory(long id);
        Task<ApiCommonResponse> GetSupplierCategoryById(long id);
    }
}