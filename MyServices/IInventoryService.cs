using HaloBiz.DTOs.ApiDTOs;
using HaloBiz.DTOs.ReceivingDTOs;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HaloBiz.MyServices
{
    public interface IInventoryService
    {
        Task<ApiResponse> CreateInventoryItem(HttpContext context);
        Task<ApiResponse> CreateInventoryItemGroup(HttpContext context);
    }
}
