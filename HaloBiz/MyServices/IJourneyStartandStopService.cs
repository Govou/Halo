using Halobiz.Common.DTOs.ApiDTOs;
using HaloBiz.DTOs.ReceivingDTOs;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HaloBiz.MyServices
{
    public interface IJourneyStartandStopService
    {
        //Journey Start
        Task<ApiCommonResponse> AddStartJourney(HttpContext context, JourneyStartReceivingDTO journeyStartReceiving);
        Task<ApiCommonResponse> GetAllStartJourneys();
        Task<ApiCommonResponse> GetStartJourneyById(long id);
        // Task<ApiCommonResponse> UpdateArmedEscortType(HttpContext context, long id, JourneyStartReceivingDTO journeyStartReceiving);
        //Task<ApiCommonResponse> UpdateStartJourneyForStop(long id);
        Task<ApiCommonResponse> DeleteStartJourney(long id);
        Task<ApiCommonResponse> UpdateCancelStartJourney(long id);
        //Task<ApiCommonResponse> UpdateServiceAssignmentOnStartJourney(long serviceAssignmentId);
        Task<ApiCommonResponse> UpdateEndStartJourney(HttpContext context, long id, JourneyEndReceivingDTO journeyEndReceiving);

        //Journey Stop
        Task<ApiCommonResponse> AddSStopJourney(HttpContext context, JourneyStopReceivingDTO journeyStopReceiving);
        Task<ApiCommonResponse> GetAllStopJourneys();
        Task<ApiCommonResponse> GetAllJourneyStopsByJourneyStartId(long startId);
        Task<ApiCommonResponse> GetStopJourneyById(long id);
         Task<ApiCommonResponse> UpdateStopJourney(HttpContext context, long id, JourneyStopReceivingDTO journeyStopReceiving);
        //Task<ApiCommonResponse> UpdateStopJourneyForStop(long id);
        Task<ApiCommonResponse> DeleteStopJourney(long id);

        //Journey Incident
        Task<ApiCommonResponse> AddJourneyIncident(HttpContext context, JourneyIncidentReceivingDTO journeyIncidentReceiving);
        Task<ApiCommonResponse> GetAllJourneyIncidents();
        Task<ApiCommonResponse> GetAllJourneyIncidentsByJourneyStartId(long startId);
        Task<ApiCommonResponse> GetJourneyIncidentById(long id);
         Task<ApiCommonResponse> UpdateJourneyIncident(HttpContext context, long id, JourneyIncidentReceivingDTO journeyIncidentReceiving);
        //Task<ApiCommonResponse> UpdateStopJourneyForStop(long id);
        Task<ApiCommonResponse> DeleteJourneyIncident(long id);

        //Journey IncidentPics
        Task<ApiCommonResponse> AddJourneyIncidentPic(HttpContext context, JourneyIncidentPictureReceivingDTO journeyIncidentPicReceiving);
        Task<ApiCommonResponse> GetAllJourneyIncidentPics();
        Task<ApiCommonResponse> GetAllJourneyIncidentPicsByJourneyIncidentId(long incidentId);
        Task<ApiCommonResponse> GetJourneyIncidentPicById(long id);
       // Task<ApiCommonResponse> UpdateArmedEscortType(HttpContext context, long id, JourneyIncidentPictureReceivingDTO journeyIncidentPicReceiving);
        Task<ApiCommonResponse> DeleteJourneyIncidentPic(long id);

        //Journey Lead Commander
        Task<ApiCommonResponse> AddJourneyLeadCommander(HttpContext context, JourneyLeadCommanderReceivingDTO journeyLeadCommanderReceiving);
        Task<ApiCommonResponse> GetAllJourneyLeadCommanders();
        //Task<ApiCommonResponse> GetAllJourneyIncidentPicsByJourneyIncidentId(long incidentId);
        Task<ApiCommonResponse> GetJourneyLeadCommanderById(long id);
        // Task<ApiCommonResponse> UpdateArmedEscortType(HttpContext context, long id, JourneyIncidentPictureReceivingDTO journeyIncidentPicReceiving);
        Task<ApiCommonResponse> RelinquishJourneyLeadCommander(long id);
        Task<ApiCommonResponse> DeleteJourneyLeadCommander(long id);

        //Journey Notes
        Task<ApiCommonResponse> AddJourneyNote(HttpContext context, JourneyNoteReceivingDTO journeyNoteReceiving);
        Task<ApiCommonResponse> GetAllJourneyNotes();
        Task<ApiCommonResponse> GetAllJourneyNotesByJourneyStarttId(long startId);
        Task<ApiCommonResponse> GetJourneyNoteById(long id);
        // Task<ApiCommonResponse> UpdateArmedEscortType(HttpContext context, long id, JourneyIncidentPictureReceivingDTO journeyIncidentPicReceiving);
        //Task<ApiCommonResponse> RelinquishJourneyNote(long id);
        Task<ApiCommonResponse> DeleteJourneyNote(long id);

        //FeedBack
        Task<ApiCommonResponse> AddFeedbackMaster(HttpContext context, FeedbackMasterReceivingDTO feedback);
        Task<ApiCommonResponse> AddGeneralFeedback(HttpContext context, GeneralFeedbackReceivingDTO feedback);
        Task<ApiCommonResponse> AddArmedEscortFeedback(HttpContext context, ArmedEscortFeedbackReceivingDTO feedback);
        Task<ApiCommonResponse> AddCommanderFeedback(HttpContext context, CommanderFeedbackReceivingDTO feedback);
        Task<ApiCommonResponse> AddPilotFeedback(HttpContext context, PilotFeedbackReceivingDTO feedback);
        Task<ApiCommonResponse> AddVehicleFeedback(HttpContext context, VehicleFeedbackReceivingDTO feedback);

    }
}
