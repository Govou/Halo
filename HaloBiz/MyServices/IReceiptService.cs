using System.Threading.Tasks;
using Halobiz.Common.DTOs.ApiDTOs;
using HaloBiz.DTOs.ReceivingDTOs;
using Microsoft.AspNetCore.Http;

namespace HaloBiz.MyServices
{
    public interface IReceiptService
    {
         Task<ApiCommonResponse> AddReceipt(HttpContext context, ReceiptReceivingDTO receiptReceivingDTO);
         Task<ApiCommonResponse> GetReceiptBreakDown(long invoiceId, double totalReceiptAmount);
    }
}