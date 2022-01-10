using HaloBiz.DTOs.ApiDTOs;
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
        Task<ApiCommonResponse> GetAllArmedEscortDetails();
        Task<ApiCommonResponse> GetAllArmedEscortDetailsByAssignmentId(long assignmentId);
        Task<ApiCommonResponse> GetArmedEscortDetailById(long id);
        Task<ApiCommonResponse> UpdateArmedEscortDetail(HttpContext context, long id, ArmedEscortServiceAssignmentDetailsReceivingDTO armedEscortReceivingDTO);
        Task<ApiCommonResponse> DeleteArmedEscortDetail(long id);
        //Task<ApiCommonResponse> DeleteArmedEscortDetailByAssignmentId(long id);

        //Commander
        Task<ApiCommonResponse> AddCommanderDetail(HttpContext context, CommanderServiceAssignmentDetailsReceivingDTO commanderReceivingDTO);
        Task<ApiCommonResponse> GetAllCommanderDetails();
        Task<ApiCommonResponse> GetAllCommanderDetailsByAssignmentId(long assignmentId);
        Task<ApiCommonResponse> GetCommanderDetailById(long id);
        Task<ApiCommonResponse> UpdateCommanderDetail(HttpContext context, long id, CommanderServiceAssignmentDetailsReceivingDTO commanderReceivingDTO);
        Task<ApiCommonResponse> DeleteCommanderDetail(long id);
        //Task<ApiCommonResponse> DeleteCommanderDetailByAssignmentId(long id);

        //Pilot
        Task<ApiCommonResponse> AddPilotDetail(HttpContext context, PilotServiceAssignmentDetailsReceivingDTO pilotReceivingDTO);
        Task<ApiCommonResponse> GetAllPilotDetails();
        Task<ApiCommonResponse> GetAllPilotDetailsByAssignmentId(long assignmentId);
        Task<ApiCommonResponse> GetPilotDetailById(long id);
        Task<ApiCommonResponse> UpdatePilotDetail(HttpContext context, long id, PilotServiceAssignmentDetailsReceivingDTO pilotReceivingDTO);
        Task<ApiCommonResponse> DeletePilotDetail(long id);
        //Task<ApiCommonResponse> DeletePilotDetailByAssignmentId(long id);

        //Vehicle
        Task<ApiCommonResponse> AddVehicleDetail(HttpContext context, VehicleServiceAssignmentDetailsReceivingDTO vehicleReceivingDTO);
        Task<ApiCommonResponse> GetAllVehicleDetails();
        Task<ApiCommonResponse> GetAllVehicleDetailsByAssignmentId(long assignmentId);
        Task<ApiCommonResponse> GetVehicleDetailById(long id);
        Task<ApiCommonResponse> UpdateVehicleDetail(HttpContext context, long id, VehicleServiceAssignmentDetailsReceivingDTO vehicleReceivingDTO);
        Task<ApiCommonResponse> DeleteVehicleDetail(long id);


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
