using HaloBiz.DTOs.ApiDTOs;
using HaloBiz.DTOs.ReceivingDTOs;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HaloBiz.MyServices
{
    public interface IVehicleService
    {
        //Type
        Task<ApiCommonResponse> AddVehicleType(HttpContext context, VehicleTypeReceivingDTO vehicleTypeReceivingDTO);
        Task<ApiCommonResponse> GetAllVehicleTypes();
        Task<ApiCommonResponse> GetVehicleTypeById(long id);
        Task<ApiCommonResponse> UpdateVehicleType(HttpContext context, long id, VehicleTypeReceivingDTO vehicleTypeReceivingDTO);
        Task<ApiCommonResponse> DeleteVehicleType(long id);
    }
}
