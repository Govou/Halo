using Halobiz.Common.DTOs.ApiDTOs;
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
        Task<ApiCommonResponse> AddArmedEscortGeneric(HttpContext context, ArmedEscortDTSDetailGenericDaysReceivingDTO armedEscortReceivingDTO);
        Task<ApiCommonResponse> GetAllArmedEscortGenerics();
        Task<ApiCommonResponse> GetArmedEscortGenericById(long id);
        Task<ApiCommonResponse> GetArmedEscortGenericByMasterId(long id);
        Task<ApiCommonResponse> UpdateArmedEscortGeneric(HttpContext context, long id, ArmedEscortDTSDetailGenericDaysReceivingDTO armedEscortReceivingDTO);
        Task<ApiCommonResponse> DeleteArmedEscortGeneric(long id);

        //Commander
        Task<ApiCommonResponse> AddCommanderGeneric(HttpContext context, CommanderDTSDetailGenericDaysReceivingDTO commanderReceivingDTO);
        Task<ApiCommonResponse> GetAllCommanderGenerics();
        Task<ApiCommonResponse> GetCommanderGenericById(long id);
        Task<ApiCommonResponse> GetCommanderGenericByMasterId(long id);
        Task<ApiCommonResponse> UpdateCommanderGeneric(HttpContext context, long id, CommanderDTSDetailGenericDaysReceivingDTO commanderReceivingDTO);
        Task<ApiCommonResponse> DeleteCommanderGeneric(long id);

        //Pilot
        Task<ApiCommonResponse> AddPilotGeneric(HttpContext context, PilotDTSDetailGenericDaysReceivingDTO pilotReceivingDTO);
        Task<ApiCommonResponse> GetAllPilotGenerics();
        Task<ApiCommonResponse> GetPilotGenericById(long id);
        Task<ApiCommonResponse> GetPilotGenericByMasterId(long id);
        Task<ApiCommonResponse> UpdatePilotGeneric(HttpContext context, long id, PilotDTSDetailGenericDaysReceivingDTO pilotReceivingDTO);
        Task<ApiCommonResponse> DeletePilotGeneric(long id);

        //Vehicle
        Task<ApiCommonResponse> AddVehicleGeneric(HttpContext context, VehicleDTSDetailGenericDaysReceivingDTO vehicleReceivingDTO);
        Task<ApiCommonResponse> GetAllVehicleGenerics();
        Task<ApiCommonResponse> GetVehicleGenericById(long id);
        Task<ApiCommonResponse> GetVehicleGenericByMasterId(long id);
        Task<ApiCommonResponse> UpdateVehicleGeneric(HttpContext context, long id, VehicleDTSDetailGenericDaysReceivingDTO vehicleReceivingDTO);
        Task<ApiCommonResponse> DeleteVehicleGeneric(long id);
    }
}
