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
        Task<ApiResponse> AddArmedEscortDetail(HttpContext context, ArmedEscortServiceAssignmentDetailsReceivingDTO armedEscortReceivingDTO);
        Task<ApiResponse> GetAllArmedEscortDetails();
        Task<ApiResponse> GetAllArmedEscortDetailsByAssignmentId(long assignmentId);
        Task<ApiResponse> GetArmedEscortDetailById(long id);
        Task<ApiResponse> UpdateArmedEscortDetail(HttpContext context, long id, ArmedEscortServiceAssignmentDetailsReceivingDTO armedEscortReceivingDTO);
        Task<ApiResponse> DeleteArmedEscortDetail(long id);

        //Commander
        Task<ApiResponse> AddCommanderDetail(HttpContext context, CommanderServiceAssignmentDetailsReceivingDTO commanderReceivingDTO);
        Task<ApiResponse> GetAllCommanderDetails();
        Task<ApiResponse> GetAllCommanderDetailsByAssignmentId(long assignmentId);
        Task<ApiResponse> GetCommanderDetailById(long id);
        Task<ApiResponse> UpdateCommanderDetail(HttpContext context, long id, CommanderServiceAssignmentDetailsReceivingDTO commanderReceivingDTO);
        Task<ApiResponse> DeleteCommanderDetail(long id);

        //Pilot
        Task<ApiResponse> AddPilotDetail(HttpContext context, PilotServiceAssignmentDetailsReceivingDTO pilotReceivingDTO);
        Task<ApiResponse> GetAllPilotDetails();
        Task<ApiResponse> GetAllPilotDetailsByAssignmentId(long assignmentId);
        Task<ApiResponse> GetPilotDetailById(long id);
        Task<ApiResponse> UpdatePilotDetail(HttpContext context, long id, PilotServiceAssignmentDetailsReceivingDTO pilotReceivingDTO);
        Task<ApiResponse> DeletePilotDetail(long id);

        //Vehicle
        Task<ApiResponse> AddVehicleDetail(HttpContext context, VehicleServiceAssignmentDetailsReceivingDTO vehicleReceivingDTO);
        Task<ApiResponse> GetAllVehicleDetails();
        Task<ApiResponse> GetAllVehicleDetailsByAssignmentId(long assignmentId);
        Task<ApiResponse> GetVehicleDetailById(long id);
        Task<ApiResponse> UpdateVehicleDetail(HttpContext context, long id, VehicleServiceAssignmentDetailsReceivingDTO vehicleReceivingDTO);
        Task<ApiResponse> DeleteVehicleDetail(long id);

        //Passenger
        Task<ApiResponse> AddPassenger(HttpContext context, PassengerReceivingDTO passengerReceivingDTO);
        Task<ApiResponse> GetAllPassengers();
        Task<ApiResponse> GetAllPassengersByAssignmentId(long assignmentId);
        Task<ApiResponse> GetPassengerById(long id);
        Task<ApiResponse> UpdatePassenger(HttpContext context, long id, PassengerReceivingDTO passengerReceivingDTO);
        Task<ApiResponse> DeletePassenger(long id);
    }
}
