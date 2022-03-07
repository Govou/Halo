using System.Threading.Tasks;
using Halobiz.Common.DTOs.ApiDTOs;
using HaloBiz.DTOs.ReceivingDTOs;
using Microsoft.AspNetCore.Http;

namespace HaloBiz.MyServices.LAMS
{
    public interface IFinancialVoucherTypeService
    {
        Task<ApiCommonResponse> AddFinancialVoucherType(HttpContext context, FinancialVoucherTypeReceivingDTO voucherTypeReceivingDTO);
        Task<ApiCommonResponse> GetAllFinancialVoucherTypes();
        Task<ApiCommonResponse> GetFinancialVoucherTypeById(long id);
        Task<ApiCommonResponse> UpdateFinancialVoucherType(HttpContext context, long id, FinancialVoucherTypeReceivingDTO voucherTypeReceivingDTO);
        Task<ApiCommonResponse> DeleteFinancialVoucherType(long id);
    }
}