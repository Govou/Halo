using System.Threading.Tasks;
using HaloBiz.DTOs.ApiDTOs;
using HaloBiz.DTOs.ReceivingDTOs;
using Microsoft.AspNetCore.Http;

namespace HaloBiz.MyServices.LAMS
{
    public interface IFinancialVoucherTypeService
    {
        Task<ApiResponse> AddFinancialVoucherType(HttpContext context, FinancialVoucherTypeReceivingDTO voucherTypeReceivingDTO);
        Task<ApiResponse> GetAllFinanclalVoucherTypes();
        Task<ApiResponse> GetFinancialVoucherTypeById(long id);
        Task<ApiResponse> UpdateFinancialVoucherType(HttpContext context, long id, FinancialVoucherTypeReceivingDTO voucherTypeReceivingDTO);
        Task<ApiResponse> DeleteFinancialVoucherType(long id);
    }
}