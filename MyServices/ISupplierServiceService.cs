using System.Threading.Tasks;
using HaloBiz.DTOs.ApiDTOs;
using HaloBiz.DTOs.ReceivingDTOs;
using Microsoft.AspNetCore.Http;

namespace HaloBiz.MyServices
{
    public interface ISupplierServiceService
    {
        Task<ApiResponse> AddSupplierService(HttpContext context, SupplierServiceReceivingDTO supplierCategoryReceivingDTO);
        Task<ApiResponse> GetAllSupplierServiceCategories();
        Task<ApiResponse> UpdateSupplierService(HttpContext context, long id, SupplierServiceReceivingDTO supplierCategoryReceivingDTO);
        Task<ApiResponse> DeleteSupplierService(long id);
        Task<ApiResponse> GetSupplierServiceById(long id);
    }
}