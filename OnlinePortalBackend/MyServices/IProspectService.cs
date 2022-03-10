using Microsoft.AspNetCore.Http;
using OnlinePortalBackend.DTOs.ApiDTOs;
using OnlinePortalBackend.DTOs.ReceivingDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OnlinePortalBackend.MyServices
{
    public interface IProspectService
    {
        Task<ApiResponse> GetLeadDivisions(HttpContext context, long prospectId);
        Task<ApiResponse> GetCartItems(HttpContext context, long prospectId);
        Task<ApiResponse> SaveCartItems(HttpContext context, CartItemsReceivingDTO cartItems, long prospectId);
    }
}
