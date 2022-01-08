using System.Threading.Tasks;
using HaloBiz.DTOs.ApiDTOs;
using HaloBiz.DTOs.ReceivingDTOs;
using Microsoft.AspNetCore.Http;

namespace HaloBiz.MyServices
{
    public interface ISupplierService
    {
        Task<ApiCommonResponse> AddSupplier(HttpContext context, SupplierReceivingDTO supplierCategoryReceivingDTO);
        Task<ApiCommonResponse> GetAllSupplierCategories();
        Task<ApiCommonResponse> UpdateSupplier(HttpContext context, long id, SupplierReceivingDTO supplierCategoryReceivingDTO);
        Task<ApiCommonResponse> DeleteSupplier(long id);
        Task<ApiCommonResponse> GetSupplierById(long id);
    }
}