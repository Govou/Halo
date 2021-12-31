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
        Task<ApiResponse> AddVehicleType(HttpContext context, VehicleTypeReceivingDTO vehicleTypeReceivingDTO);
        Task<ApiResponse> GetAllVehicleTypes();
        Task<ApiResponse> GetVehicleTypeById(long id);
        Task<ApiResponse> UpdateVehicleType(HttpContext context, long id, VehicleTypeReceivingDTO vehicleTypeReceivingDTO);
        Task<ApiResponse> DeleteVehicleType(long id);
    }
}
