using Halobiz.Common.DTOs.ApiDTOs;
using Halobiz.Common.DTOs.ReceivingDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OnlinePortalBackend.Adapters
{
    public interface IReceiptAdapter
    {
        Task<ApiCommonResponse> AddReceipt(ReceiptReceivingDTO receiptReceiving);
    }
    public class ReceiptAdapter
    {
        public ReceiptAdapter()
        {
            
        }

        public Task<ApiCommonResponse> AddReceipt(ReceiptReceivingDTO receiptReceiving)
        {
            return null;
        }
    }
}
