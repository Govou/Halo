using Halobiz.Common.DTOs.ApiDTOs;
using HaloBiz.DTOs.ReceivingDTOs;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HaloBiz.MyServices
{
    public interface IServiceAssignmentDetailsService
    {
        //ArmedEscort
        Task<ApiCommonResponse> AddArmedEscortDetail(HttpContext context, ArmedEscortServiceAssignmentDetailsReceivingDTO armedEscortReceivingDTO);
        Task<ApiCommonResponse> AddArmedEscortDetailReplacement(HttpContext context, ArmedEscortReplacementReceivingDTO armedEscortReceivingDTO);
        Task<ApiCommonResponse> GetAllArmedEscortDetails();
        Task<ApiCommonResponse> GetAllUniqueAvailableArmedEscortDetails();
        Task<ApiCommonResponse> GetAllUniqueHeldArmedEscortDetails();
        Task<ApiCommonResponse> GetAllArmedEscortDetailsByAssignmentId(long assignmentId);
        Task<ApiCommonResponse> GetArmedEscortDetailById(long id);
        Task<ApiCommonResponse> UpdateArmedEscortDetail(HttpContext context, long id, ArmedEscortServiceAssignmentDetailsReceivingDTO armedEscortReceivingDTO);
        Task<ApiCommonResponse> DeleteArmedEscortDetail(long id);
        Task<ApiCommonResponse> DeleteArmedEscortDetail_(long id); //For is Deleted Status
        Task<ApiCommonResponse> UpdateArmedEscortDetailHeldForActionByAssignmentId(long id);
        Task<ApiCommonResponse> UpdateServiceDetailsHeldForActionAndReadyStatusForOnlineByAssignmentId(long[] id);
        Task<ApiCommonResponse> UpdateServiceDetailsHeldForActionAndReadyStatusByAssignmentId(long id);

        //Commander
        Task<ApiCommonResponse> AddCommanderDetail(HttpContext context, CommanderServiceAssignmentDetailsReceivingDTO commanderReceivingDTO);
        Task<ApiCommonResponse> AddCommanderDetailReplacement(HttpContext context, CommanderReplacementReceivingDTO commanderReceivingDTO);
        Task<ApiCommonResponse> GetAllCommanderDetails();
        Task<ApiCommonResponse> GetAllUniqueAvailableCommanderDetails();
        Task<ApiCommonResponse> GetAllUniqueHeldCommanderDetails();
        Task<ApiCommonResponse> GetAllCommanderDetailsByAssignmentId(long assignmentId);
        Task<ApiCommonResponse> GetAllCommanderDetailsByProfileId(long profileId);
        Task<ApiCommonResponse> GetCommanderDetailById(long id);
        Task<ApiCommonResponse> UpdateCommanderDetail(HttpContext context, long id, CommanderServiceAssignmentDetailsReceivingDTO commanderReceivingDTO);
        //Task<ApiCommonResponse> UpdateCommanderDetailForVehicleReplacementByAssIdandResourceId(HttpContext context, long assId, long resourceId, CommanderServiceAssignmentDetailsReceivingDTO commanderReceivingDTO);
        Task<ApiCommonResponse> DeleteCommanderDetail(long id);
        Task<ApiCommonResponse> DeleteCommanderDetail_(long id);
        //Task<ApiCommonResponse> UpdateCommanderDetailHeldForActionByAssignmentId(long id);

        //Pilot
        Task<ApiCommonResponse> AddPilotDetail(HttpContext context, PilotServiceAssignmentDetailsReceivingDTO pilotReceivingDTO);
        Task<ApiCommonResponse> AddPilotDetailReplacement(HttpContext context, PilotReplacementReceivingDTO pilotReceivingDTO);
        Task<ApiCommonResponse> GetAllPilotDetails();
        Task<ApiCommonResponse> GetAllUniqueAvailablePilotDetails();
        Task<ApiCommonResponse> GetAllUniqueHeldPilotDetails();
        Task<ApiCommonResponse> GetAllPilotDetailsByAssignmentId(long assignmentId);
        Task<ApiCommonResponse> GetPilotDetailById(long id);
        Task<ApiCommonResponse> UpdatePilotDetail(HttpContext context, long id, PilotServiceAssignmentDetailsReceivingDTO pilotReceivingDTO);
        //Task<ApiCommonResponse> UpdatePilotDetailForVehicleReplacementByAssIdandResourceId(HttpContext context, long assId, long resourceId, PilotServiceAssignmentDetailsReceivingDTO pilotReceivingDTO);

        Task<ApiCommonResponse> DeletePilotDetail(long id);
        Task<ApiCommonResponse> DeletePilotDetail_(long id);
        //Task<ApiCommonResponse> UpdatePilotDetailHeldForActionByAssignmentId(long id);

        //Vehicle
        Task<ApiCommonResponse> AddVehicleDetail(HttpContext context, VehicleServiceAssignmentDetailsReceivingDTO vehicleReceivingDTO);
        Task<ApiCommonResponse> AddVehicleDetailReplacement(HttpContext context, VehicleReplacementReceivingDTO vehicleReceivingDTO);
        Task<ApiCommonResponse> GetAllVehicleDetails();
        Task<ApiCommonResponse> GetAllUniqueAvailableVehicleDetails();
        Task<ApiCommonResponse> GetAllUniqueHeldVehicleDetails();
        Task<ApiCommonResponse> GetAllVehicleDetailsByAssignmentId(long assignmentId);
        Task<ApiCommonResponse> GetVehicleDetailById(long id);
        Task<ApiCommonResponse> UpdateVehicleDetail(HttpContext context, long id, VehicleServiceAssignmentDetailsReceivingDTO vehicleReceivingDTO);
        Task<ApiCommonResponse> DeleteVehicleDetail(long id);
        Task<ApiCommonResponse> DeleteVehicleDetail_(long id);//isDeleted Status
        //Task<ApiCommonResponse> UpdateVehicleDetailHeldForActionByAssignmentId(long id);


        //Passenger
        Task<ApiCommonResponse> AddPassenger(HttpContext context, PassengerReceivingDTO passengerReceivingDTO);
        Task<ApiCommonResponse> GetAllPassengers();
        Task<ApiCommonResponse> GetAllPassengersByAssignmentId(long assignmentId);
        Task<ApiCommonResponse> GetPassengerById(long id);
        Task<ApiCommonResponse> UpdatePassenger(HttpContext context, long id, PassengerReceivingDTO passengerReceivingDTO);
        Task<ApiCommonResponse> DeletePassenger(long id);

        Task<ApiCommonResponse> GetAllContracts();
    }
}
