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
        Task<ApiCommonResponse> AddArmedEscortMaster(HttpContext context, ArmedEscortDTSMastersReceivingDTO armedEscortReceivingDTO);
        Task<ApiCommonResponse> GetAllArmedEscortMasters();
        Task<ApiCommonResponse> GetArmedEscortMasterById(long id);
        Task<ApiCommonResponse> UpdateArmedEscortMaster(HttpContext context, long id, ArmedEscortDTSMastersReceivingDTO armedEscortReceivingDTO);
        Task<ApiCommonResponse> DeleteArmedEscortMaster(long id);

        //Commander
        Task<ApiCommonResponse> AddCommanderMaster(HttpContext context, CommanderDTSMastersReceivingDTO commanderReceivingDTO);
        Task<ApiCommonResponse> GetAllCommanderMasters();
        Task<ApiCommonResponse> GetCommanderMasterById(long id);
        Task<ApiCommonResponse> UpdateCommanderMaster(HttpContext context, long id, CommanderDTSMastersReceivingDTO commanderReceivingDTO);
        Task<ApiCommonResponse> DeleteCommanderMaster(long id);

        //Pilot
        Task<ApiCommonResponse> AddPilotMaster(HttpContext context, PilotDTSMastersReceivingDTO pilotReceivingDTO);
        Task<ApiCommonResponse> GetAllPilotMasters();
        Task<ApiCommonResponse> GetPilotMasterById(long id);
        Task<ApiCommonResponse> UpdatePilotMaster(HttpContext context, long id, PilotDTSMastersReceivingDTO pilotReceivingDTO);
        Task<ApiCommonResponse> DeletePilotMaster(long id);

        //Vehicle
        Task<ApiCommonResponse> AddVehicleMaster(HttpContext context, VehicleDTSMastersReceivingDTO vehicleReceivingDTO);
        Task<ApiCommonResponse> GetAllVehicleMasters();
        Task<ApiCommonResponse> GetVehicleMasterById(long id);
        Task<ApiCommonResponse> UpdateVehicleMaster(HttpContext context, long id, VehicleDTSMastersReceivingDTO vehicleReceivingDTO);
        Task<ApiCommonResponse> DeleteVehicleMaster(long id);
    }
}
