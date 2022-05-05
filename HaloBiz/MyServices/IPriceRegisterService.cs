using Halobiz.Common.DTOs.ApiDTOs;
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
        Task<ApiCommonResponse> AddPriceRegister(HttpContext context, PriceRegisterReceivingDTO priceRegisterReceivingDTO);
        Task<ApiCommonResponse> GetAllPriceRegisters();
        Task<ApiCommonResponse> GetAllPriceRegistersByRouteId(long routeId);
        Task<ApiCommonResponse> GetAllPriceRegistersByServiceCategoryId(long categoryId);
        //Task<ApiCommonResponse> GetAllRoutesWithPriceRegisters(string routeName);
        Task<ApiCommonResponse> GetPriceRegisterId(long id);
        Task<ApiCommonResponse> UpdatePriceRegister(HttpContext context, long id, PriceRegisterReceivingDTO priceRegisterReceivingDTO);
        Task<ApiCommonResponse> DeletePriceRegister(long id);
    }
}
