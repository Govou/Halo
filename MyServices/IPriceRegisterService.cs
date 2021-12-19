using HaloBiz.DTOs.ApiDTOs;
using HaloBiz.DTOs.ReceivingDTOs;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HaloBiz.MyServices
{
    public interface IPriceRegisterService
    {
        Task<ApiResponse> AddPriceRegister(HttpContext context, PriceRegisterReceivingDTO priceRegisterReceivingDTO);
        Task<ApiResponse> GetAllPriceRegisters();
        Task<ApiResponse> GetAllPriceRegistersByRouteId(long routeId);
        //Task<ApiResponse> GetAllRoutesWithPriceRegisters(string routeName);
        Task<ApiResponse> GetPriceRegisterId(long id);
        Task<ApiResponse> UpdatePriceRegister(HttpContext context, long id, PriceRegisterReceivingDTO priceRegisterReceivingDTO);
        Task<ApiResponse> DeletePriceRegister(long id);
    }
}
