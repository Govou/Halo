using System.Threading.Tasks;
using HaloBiz.DTOs.ApiDTOs;
using HaloBiz.DTOs.ReceivingDTOs;
using Microsoft.AspNetCore.Http;

namespace HaloBiz.MyServices
{
    public interface ISupplierService
    {
        Task<ApiResponse> AddSupplier(HttpContext context, SupplierReceivingDTO supplierCategoryReceivingDTO);
        Task<ApiResponse> GetAllSupplierCategories();
        Task<ApiResponse> UpdateSupplier(HttpContext context, long id, SupplierReceivingDTO supplierCategoryReceivingDTO);
        Task<ApiResponse> DeleteSupplier(long id);
    }
}