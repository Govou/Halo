using System.Threading.Tasks;
using HaloBiz.DTOs.ApiDTOs;
using HaloBiz.DTOs.ReceivingDTOs;
using Microsoft.AspNetCore.Http;

namespace HaloBiz.MyServices
{
    public interface ISupplierServiceService
    {
        Task<ApiCommonResponse> AddSupplierService(HttpContext context, SupplierServiceReceivingDTO supplierCategoryReceivingDTO);
        Task<ApiCommonResponse> GetAllSupplierServiceCategories();
        Task<ApiCommonResponse> UpdateSupplierService(HttpContext context, long id, SupplierServiceReceivingDTO supplierCategoryReceivingDTO);
        Task<ApiCommonResponse> DeleteSupplierService(long id);
        Task<ApiCommonResponse> GetSupplierServiceById(long id);
    }
}