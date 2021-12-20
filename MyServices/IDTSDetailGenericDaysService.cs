using HaloBiz.DTOs.ApiDTOs;
using HaloBiz.DTOs.ReceivingDTOs;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HaloBiz.MyServices
{
    public interface IDTSDetailGenericDaysService
    {
        //ArmedEscort
        Task<ApiResponse> AddArmedEscortGeneric(HttpContext context, ArmedEscortDTSDetailGenericDaysReceivingDTO armedEscortReceivingDTO);
        Task<ApiResponse> GetAllArmedEscortGenerics();
        Task<ApiResponse> GetArmedEscortGenericById(long id);
        Task<ApiResponse> GetArmedEscortGenericByMasterId(long id);
        Task<ApiResponse> UpdateArmedEscortGeneric(HttpContext context, long id, ArmedEscortDTSDetailGenericDaysReceivingDTO armedEscortReceivingDTO);
        Task<ApiResponse> DeleteArmedEscortGeneric(long id);

        //Commander
        Task<ApiResponse> AddCommanderGeneric(HttpContext context, CommanderDTSDetailGenericDaysReceivingDTO commanderReceivingDTO);
        Task<ApiResponse> GetAllCommanderGenerics();
        Task<ApiResponse> GetCommanderGenericById(long id);
        Task<ApiResponse> GetCommanderGenericByMasterId(long id);
        Task<ApiResponse> UpdateCommanderGeneric(HttpContext context, long id, CommanderDTSDetailGenericDaysReceivingDTO commanderReceivingDTO);
        Task<ApiResponse> DeleteCommanderGeneric(long id);

        //Pilot
        Task<ApiResponse> AddPilotGeneric(HttpContext context, PilotDTSDetailGenericDaysReceivingDTO pilotReceivingDTO);
        Task<ApiResponse> GetAllPilotGenerics();
        Task<ApiResponse> GetPilotGenericById(long id);
        Task<ApiResponse> GetPilotGenericByMasterId(long id);
        Task<ApiResponse> UpdatePilotGeneric(HttpContext context, long id, PilotDTSDetailGenericDaysReceivingDTO pilotReceivingDTO);
        Task<ApiResponse> DeletePilotGeneric(long id);

        //Vehicle
        Task<ApiResponse> AddVehicleGeneric(HttpContext context, VehicleDTSDetailGenericDaysReceivingDTO vehicleReceivingDTO);
        Task<ApiResponse> GetAllVehicleGenerics();
        Task<ApiResponse> GetVehicleGenericById(long id);
        Task<ApiResponse> GetVehicleGenericByMasterId(long id);
        Task<ApiResponse> UpdateVehicleGeneric(HttpContext context, long id, VehicleDTSDetailGenericDaysReceivingDTO vehicleReceivingDTO);
        Task<ApiResponse> DeleteVehicleGeneric(long id);
    }
}
