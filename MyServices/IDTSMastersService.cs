using HaloBiz.DTOs.ApiDTOs;
using HaloBiz.DTOs.ReceivingDTOs;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HaloBiz.MyServices
{
    public interface IDTSMastersService
    {
        //ArmedEscort
        Task<ApiResponse> AddArmedEscortMaster(HttpContext context, ArmedEscortDTSMastersReceivingDTO armedEscortReceivingDTO);
        Task<ApiResponse> GetAllArmedEscortMasters();
        Task<ApiResponse> GetArmedEscortMasterById(long id);
        Task<ApiResponse> UpdateArmedEscortMaster(HttpContext context, long id, ArmedEscortDTSMastersReceivingDTO armedEscortReceivingDTO);
        Task<ApiResponse> DeleteArmedEscortMaster(long id);

        //Commander
        Task<ApiResponse> AddCommanderMaster(HttpContext context, CommanderDTSMastersReceivingDTO commanderReceivingDTO);
        Task<ApiResponse> GetAllCommanderMasters();
        Task<ApiResponse> GetCommanderMasterById(long id);
        Task<ApiResponse> UpdateCommanderMaster(HttpContext context, long id, CommanderDTSMastersReceivingDTO commanderReceivingDTO);
        Task<ApiResponse> DeleteCommanderMaster(long id);

        //Pilot
        Task<ApiResponse> AddPilotMaster(HttpContext context, PilotDTSMastersReceivingDTO pilotReceivingDTO);
        Task<ApiResponse> GetAllPilotMasters();
        Task<ApiResponse> GetPilotMasterById(long id);
        Task<ApiResponse> UpdatePilotMaster(HttpContext context, long id, PilotDTSMastersReceivingDTO pilotReceivingDTO);
        Task<ApiResponse> DeletePilotMaster(long id);

        //Vehicle
        Task<ApiResponse> AddVehicleMaster(HttpContext context, VehicleDTSMastersReceivingDTO vehicleReceivingDTO);
        Task<ApiResponse> GetAllVehicleMasters();
        Task<ApiResponse> GetVehicleMasterById(long id);
        Task<ApiResponse> UpdateVehicleMaster(HttpContext context, long id, VehicleDTSMastersReceivingDTO vehicleReceivingDTO);
        Task<ApiResponse> DeleteVehicleMaster(long id);
    }
}
