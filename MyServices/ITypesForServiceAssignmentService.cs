using HaloBiz.DTOs.ApiDTOs;
using HaloBiz.DTOs.ReceivingDTOs;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HaloBiz.MyServices
{
    public interface ITypesForServiceAssignmentService
    {
        //Trip
        Task<ApiResponse> AddTripType(HttpContext context, TripTypesForServiceAssignmentReceivingDTO tripReceivingDTO);
        Task<ApiResponse> GetAllTripTypes();
        Task<ApiResponse> GetTripTypeById(long id);
        Task<ApiResponse> UpdateTripType(HttpContext context, long id, TripTypesForServiceAssignmentReceivingDTO tripReceivingDTO);
        Task<ApiResponse> DeleteTripType(long id);

        //Passenger
        Task<ApiResponse> AddPassengerType(HttpContext context, PassengerTypesForServiceAssignmentReceivingDTO passengerReceivingDTO);
        Task<ApiResponse> GetAllPassengerTypes();
        Task<ApiResponse> GetPassengerTypeById(long id);
        Task<ApiResponse> UpdatePassengerType(HttpContext context, long id, PassengerTypesForServiceAssignmentReceivingDTO passengerReceivingDTO);
        Task<ApiResponse> DeletePassengerType(long id);

        //Source
        Task<ApiResponse> AddSourceType(HttpContext context, SourceTypesForServiceAssignmentReceivingDTO sourceReceivingDTO);
        Task<ApiResponse> GetAllSourceTypes();
        Task<ApiResponse> GetSourceTypeById(long id);
        Task<ApiResponse> UpdateSourceType(HttpContext context, long id, SourceTypesForServiceAssignmentReceivingDTO sourceReceivingDTO);
        Task<ApiResponse> DeleteSourceType(long id);


        //Release
        Task<ApiResponse> AddReleaseType(HttpContext context, ReleaseTypesForServiceAssignmentReceivingDTO releaseReceivingDTO);
        Task<ApiResponse> GetAllReleaseTypes();
        Task<ApiResponse> GetReleaseTypeById(long id);
        Task<ApiResponse> UpdateReleaseType(HttpContext context, long id, ReleaseTypesForServiceAssignmentReceivingDTO releaseReceivingDTO);
        Task<ApiResponse> DeleteReleaseType(long id);
    }
}
