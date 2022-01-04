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
        Task<ApiCommonResponse> AddVehicle(HttpContext context, VehicleReceivingDTO vehicleReceivingDTO);
        Task<ApiCommonResponse> GetAllVehicles();
        Task<ApiCommonResponse> GetVehicleById(long id);
        Task<ApiCommonResponse> UpdateVehicle(HttpContext context, long id, VehicleReceivingDTO vehicleReceivingDTO);
        Task<ApiCommonResponse> DeleteVehicle(long id);

        //Tie
        Task<ApiCommonResponse> AddVehicleTie(HttpContext context, VehicleSMORoutesResourceTieReceivingDTO vehicleTieReceivingDTO);
        Task<ApiCommonResponse> GetAllVehicleTies();
        Task<ApiCommonResponse> GetVehicleTieById(long id);
        Task<ApiCommonResponse> DeleteVehicleTie(long id);
    }
}
