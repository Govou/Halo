using HaloBiz.DTOs.ApiDTOs;
using HaloBiz.DTOs.ReceivingDTOs;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HaloBiz.MyServices
{
    public interface IVehicleRegistrationService
    {
        Task<ApiResponse> AddVehicle(HttpContext context, VehicleReceivingDTO vehicleReceivingDTO);
        Task<ApiResponse> GetAllVehicles();
        Task<ApiResponse> GetVehicleById(long id);
        Task<ApiResponse> UpdateVehicle(HttpContext context, long id, VehicleReceivingDTO vehicleReceivingDTO);
        Task<ApiResponse> DeleteVehicle(long id);

        //Tie
        Task<ApiResponse> AddVehicleTie(HttpContext context, VehicleSMORoutesResourceTieReceivingDTO vehicleTieReceivingDTO);
        Task<ApiResponse> GetAllVehicleTies();
        Task<ApiResponse> GetAllVehicleTiesByResourceId(long resourceId);
        Task<ApiResponse> GetVehicleTieById(long id);
        Task<ApiResponse> DeleteVehicleTie(long id);
    }
}
