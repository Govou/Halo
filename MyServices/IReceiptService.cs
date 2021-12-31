using System.Threading.Tasks;
using HaloBiz.DTOs.ApiDTOs;
using HaloBiz.DTOs.ReceivingDTOs;
using Microsoft.AspNetCore.Http;

namespace HaloBiz.MyServices
{
    public interface IReceiptService
    {
         Task<ApiResponse> AddReceipt(HttpContext context, ReceiptReceivingDTO receiptReceivingDTO);
         Task<ApiResponse> GetReceiptBreakDown(long invoiceId, double totalReceiptAmount);
    }
}