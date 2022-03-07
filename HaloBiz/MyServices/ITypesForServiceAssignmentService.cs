using Halobiz.Common.DTOs.ApiDTOs;
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
        Task<ApiCommonResponse> AddTripType(HttpContext context, TripTypesForServiceAssignmentReceivingDTO tripReceivingDTO);
        Task<ApiCommonResponse> GetAllTripTypes();
        Task<ApiCommonResponse> GetTripTypeById(long id);
        Task<ApiCommonResponse> UpdateTripType(HttpContext context, long id, TripTypesForServiceAssignmentReceivingDTO tripReceivingDTO);
        Task<ApiCommonResponse> DeleteTripType(long id);

        //Passenger
        Task<ApiCommonResponse> AddPassengerType(HttpContext context, PassengerTypesForServiceAssignmentReceivingDTO passengerReceivingDTO);
        Task<ApiCommonResponse> GetAllPassengerTypes();
        Task<ApiCommonResponse> GetPassengerTypeById(long id);
        Task<ApiCommonResponse> UpdatePassengerType(HttpContext context, long id, PassengerTypesForServiceAssignmentReceivingDTO passengerReceivingDTO);
        Task<ApiCommonResponse> DeletePassengerType(long id);

        //Source
        Task<ApiCommonResponse> AddSourceType(HttpContext context, SourceTypesForServiceAssignmentReceivingDTO sourceReceivingDTO);
        Task<ApiCommonResponse> GetAllSourceTypes();
        Task<ApiCommonResponse> GetSourceTypeById(long id);
        Task<ApiCommonResponse> UpdateSourceType(HttpContext context, long id, SourceTypesForServiceAssignmentReceivingDTO sourceReceivingDTO);
        Task<ApiCommonResponse> DeleteSourceType(long id);


        //Release
        Task<ApiCommonResponse> AddReleaseType(HttpContext context, ReleaseTypesForServiceAssignmentReceivingDTO releaseReceivingDTO);
        Task<ApiCommonResponse> GetAllReleaseTypes();
        Task<ApiCommonResponse> GetReleaseTypeById(long id);
        Task<ApiCommonResponse> UpdateReleaseType(HttpContext context, long id, ReleaseTypesForServiceAssignmentReceivingDTO releaseReceivingDTO);
        Task<ApiCommonResponse> DeleteReleaseType(long id);
    }
}
