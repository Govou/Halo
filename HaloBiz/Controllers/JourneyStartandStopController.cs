using Halobiz.Common.Auths;
using Halobiz.Common.DTOs.ApiDTOs;
using HaloBiz.DTOs.ReceivingDTOs;
using HaloBiz.MyServices;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HaloBiz.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    [ModuleName(HalobizModules.Setups)]

    public class JourneyStartandStopController : ControllerBase
    {
        private readonly IJourneyStartandStopService _journeyStartandStopService;

        public JourneyStartandStopController(IJourneyStartandStopService journeyStartandStopService)
        {
            _journeyStartandStopService = journeyStartandStopService;
        }
        //Jouney Start
        [HttpGet("GetAllJourneyStarts")]
        public async Task<ApiCommonResponse> GetAllJourneyStarts()
        {
            return await _journeyStartandStopService.GetAllStartJourneys();
        }

        [HttpGet("GetJouneyStartById/{id}")]
        public async Task<ApiCommonResponse> GetJouneyStartById(long id)
        {
            return await _journeyStartandStopService.GetStartJourneyById(id);
        }
        [HttpGet("GetJouneyStartByAssignmentId/{assignmentId}")]
        public async Task<ApiCommonResponse> GetJouneyStartByAssignmentId(long assignmentId)
        {
            return await _journeyStartandStopService.GetStartJourneyByAssignmentId(assignmentId);
        }

        [HttpPost("AddNewJourneyStart")]
        public async Task<ApiCommonResponse> AddNewJourneyStart(JourneyStartReceivingDTO ReceivingDTO)
        {
            return await _journeyStartandStopService.AddStartJourney(HttpContext, ReceivingDTO);
        }

        //[HttpPut("UpdateTypeById/{id}")]
        //public async Task<ApiCommonResponse> UpdateTypeById(long id, ArmedEscortTypeReceivingDTO TypeReceiving)
        //{
        //    return await _armedEscortService.UpdateArmedEscortType(HttpContext, id, TypeReceiving);
        //}

        [HttpDelete("DeleteJourneyStartById/{id}")]
        public async Task<ApiCommonResponse> DeleteJourneyStartById(int id)
        {
            return await _journeyStartandStopService.DeleteStartJourney(id);
        }

        [HttpDelete("UpdateCancelJourneyStartById/{id}")]
        public async Task<ApiCommonResponse> UpdateCancelJourneyStartById(int id)
        {
            return await _journeyStartandStopService.UpdateCancelStartJourney(id);
        }
        [HttpPut("UpdateEndJourneyStartById/{id}")]
        public async Task<ApiCommonResponse> UpdateEndJourneyStartById(long id, JourneyEndReceivingDTO Receiving)
        {
            return await _journeyStartandStopService.UpdateEndStartJourney(HttpContext, id, Receiving);
        }
        //Jouney Stop
        [HttpGet("GetAllJourneyStops")]
        public async Task<ApiCommonResponse> GetAllJourneyStops()
        {
            return await _journeyStartandStopService.GetAllStopJourneys();
        }


        [HttpGet("GetAllJourneyStopsByJourneyStartId/{id}")]
        public async Task<ApiCommonResponse> GetAllJourneyStopsByJourneyStartId(long id)
        {
            return await _journeyStartandStopService.GetAllJourneyStopsByJourneyStartId(id);
        }

        [HttpGet("GetJouneyStopById/{id}")]
        public async Task<ApiCommonResponse> GetJouneyStopById(long id)
        {
            return await _journeyStartandStopService.GetStopJourneyById(id);
        }

        [HttpPost("AddNewJourneyStop")]
        public async Task<ApiCommonResponse> AddNewJourneyStop(JourneyStopReceivingDTO ReceivingDTO)
        {
            return await _journeyStartandStopService.AddSStopJourney(HttpContext, ReceivingDTO);
        }

        [HttpPut("UpdateJourneyStopById/{id}")]
        public async Task<ApiCommonResponse> UpdateJourneyStopById(long id, JourneyStopReceivingDTO Receiving)
        {
            return await _journeyStartandStopService.UpdateStopJourney(HttpContext, id, Receiving);
        }

        [HttpDelete("DeleteJourneyStopById/{id}")]
        public async Task<ApiCommonResponse> DeleteJourneyStopById(int id)
        {
            return await _journeyStartandStopService.DeleteStopJourney(id);
        }

        //Jouney Incident
        [HttpGet("GetAllJourneyIncidents")]
        public async Task<ApiCommonResponse> GetAllJourneyIncidents()
        {
            return await _journeyStartandStopService.GetAllJourneyIncidents();
        }

        [HttpGet("GetAllJourneyIncidentsByJourneyStartId/{id}")]
        public async Task<ApiCommonResponse> GetAllJourneyIncidentsByJourneyStartId(long id)
        {
            return await _journeyStartandStopService.GetAllJourneyIncidentsByJourneyStartId(id);
        }

        [HttpGet("GetJouneyIncidentById/{id}")]
        public async Task<ApiCommonResponse> GetJouneyIncidentById(long id)
        {
            return await _journeyStartandStopService.GetJourneyIncidentById(id);
        }

        [HttpPost("AddNewJourneyIncident")]
        public async Task<ApiCommonResponse> AddNewJourneyIncident(JourneyIncidentReceivingDTO ReceivingDTO)
        {
            return await _journeyStartandStopService.AddJourneyIncident(HttpContext, ReceivingDTO);
        }

        [HttpPut("UpdateJourneyIncidentById/{id}")]
        public async Task<ApiCommonResponse> UpdateJourneyIncidentById(long id, JourneyIncidentReceivingDTO Receiving)
        {
            return await _journeyStartandStopService.UpdateJourneyIncident(HttpContext, id, Receiving);
        }

        [HttpDelete("DeleteJourneyIncidentById/{id}")]
        public async Task<ApiCommonResponse> DeleteJourneyIncidentById(int id)
        {
            return await _journeyStartandStopService.DeleteJourneyIncident(id);
        }

        //Jouney IncidentPics
        [HttpGet("GetAllJourneyIncidentPics")]
        public async Task<ApiCommonResponse> GetAllJourneyIncidentPics()
        {
            return await _journeyStartandStopService.GetAllJourneyIncidentPics();
        }

        [HttpGet("GetAllJourneyIncidentPicsByJourneyIncidentId/{id}")]
        public async Task<ApiCommonResponse> GetAllJourneyIncidentPicsByJourneyIncidentId(long id)
        {
            return await _journeyStartandStopService.GetAllJourneyIncidentPicsByJourneyIncidentId(id);
        }

        [HttpGet("GetJouneyIncidentPicById/{id}")]
        public async Task<ApiCommonResponse> GetJouneyIncidentPicById(long id)
        {
            return await _journeyStartandStopService.GetJourneyIncidentPicById(id);
        }

        [HttpPost("AddNewJourneyIncidentPic")]
        public async Task<ApiCommonResponse> AddNewJourneyIncidentPic(JourneyIncidentPictureReceivingDTO ReceivingDTO)
        {
            return await _journeyStartandStopService.AddJourneyIncidentPic(HttpContext, ReceivingDTO);
        }

        [HttpDelete("DeleteJourneyIncidentPicById/{id}")]
        public async Task<ApiCommonResponse> DeleteJourneyIncidentPicById(int id)
        {
            return await _journeyStartandStopService.DeleteJourneyIncidentPic(id);
        }

        //Jouney Lead Commander
        [HttpGet("GetAllJourneyLeadCommanders")]
        public async Task<ApiCommonResponse> GetAllJourneyLeadCommanders()
        {
            return await _journeyStartandStopService.GetAllJourneyLeadCommanders();
        }

        [HttpGet("GetJouneyLeadCommanderById/{id}")]
        public async Task<ApiCommonResponse> GetJouneyLeadCommanderById(long id)
        {
            return await _journeyStartandStopService.GetJourneyLeadCommanderById(id);
        }

        [HttpPost("AddNewJourneyLeadCommander")]
        public async Task<ApiCommonResponse> AddNewJourneyLeadCommander(JourneyLeadCommanderReceivingDTO ReceivingDTO)
        {
            return await _journeyStartandStopService.AddJourneyLeadCommander(HttpContext, ReceivingDTO);
        }

        [HttpDelete("RelinquishJourneyLeadCommanderById/{id}")]
        public async Task<ApiCommonResponse> RelinquishJourneyLeadCommanderById(int id)
        {
            return await _journeyStartandStopService.RelinquishJourneyLeadCommander(id);
        }

        [HttpDelete("DeleteJourneyLeadCommanderById/{id}")]
        public async Task<ApiCommonResponse> DeleteJourneyLeadCommanderById(int id)
        {
            return await _journeyStartandStopService.DeleteJourneyLeadCommander(id);
        }

        //Jouney Note
        [HttpGet("GetAllJourneyNotes")]
        public async Task<ApiCommonResponse> GetAllJourneyNotes()
        {
            return await _journeyStartandStopService.GetAllJourneyNotes();
        }
        [HttpGet("GetAllJourneyNotesByStartId/{id}")]
        public async Task<ApiCommonResponse> GetAllJourneyNotesByStartId(long id)
        {
            return await _journeyStartandStopService.GetAllJourneyNotesByJourneyStarttId(id);
        }

        [HttpGet("GetJouneyNoteById/{id}")]
        public async Task<ApiCommonResponse> GetJouneyNoteById(long id)
        {
            return await _journeyStartandStopService.GetJourneyNoteById(id);
        }

        [HttpPost("AddNewJourneyNote")]
        public async Task<ApiCommonResponse> AddNewJourneyNote(JourneyNoteReceivingDTO ReceivingDTO)
        {
            return await _journeyStartandStopService.AddJourneyNote(HttpContext, ReceivingDTO);
        }

       

        [HttpDelete("DeleteJourneyNoteById/{id}")]
        public async Task<ApiCommonResponse> DeleteJourneyNoteById(int id)
        {
            return await _journeyStartandStopService.DeleteJourneyNote(id);
        }

        //Feedback
        [HttpPost("AddFeedbackMaster")]
        public async Task<ApiCommonResponse> AddFeedbackMaster(FeedbackMasterReceivingDTO feedback)
        {
            return await _journeyStartandStopService.AddFeedbackMaster(HttpContext, feedback);
        }
        [HttpGet("GetAllFeedbackMasters")]
        public async Task<ApiCommonResponse> GetAllFeedbackMasters()
        {
            return await _journeyStartandStopService.GetAllFeedbackMasters();
        }
        [HttpGet("GetFeedbackMasterById/{id}")]
        public async Task<ApiCommonResponse> GetFeedbackMasterById(long id)
        {
            return await _journeyStartandStopService.GetFeedbackMasterById(id);
        }
        [HttpGet("GetFeedbackMasterByAssignmentId/{assignmentId}")]
        public async Task<ApiCommonResponse> GetFeedbackMasterByAssignmentId(long assignmentId)
        {
            return await _journeyStartandStopService.GetFeedbackMasterByAssignmentId(assignmentId);
        }

        [HttpGet("GetGeneralFeedbackByAssignmentId/{assignId}")]
        public async Task<ApiCommonResponse> GetGeneralFeedbackByAssignmentId(long assignId)
        {
            return await _journeyStartandStopService.GetGeneralFeedbackByAssignmenrId(assignId);
        }
        [HttpPost("AddGeneralFeedback")]
        public async Task<ApiCommonResponse> AddGeneralFeedback(GeneralFeedbackReceivingDTO feedback)
        {
            return await _journeyStartandStopService.AddGeneralFeedback(HttpContext, feedback);
        }

        [HttpPost("AddArmedEscortFeedback")]
        public async Task<ApiCommonResponse> AddArmedEscortFeedback(ArmedEscortFeedbackReceivingDTO feedback)
        {
            return await _journeyStartandStopService.AddArmedEscortFeedback(HttpContext, feedback);
        }

        [HttpPost("AddCommanderFeedback")]
        public async Task<ApiCommonResponse> AddCommanderFeedback(CommanderFeedbackReceivingDTO feedback)
        {
            return await _journeyStartandStopService.AddCommanderFeedback(HttpContext, feedback);
        }

        [HttpPost("AddPilotFeedback")]
        public async Task<ApiCommonResponse> AddPilotFeedback(PilotFeedbackReceivingDTO feedback)
        {
            return await _journeyStartandStopService.AddPilotFeedback(HttpContext, feedback);
        }

        [HttpPost("AddVehicleFeedback")]
        public async Task<ApiCommonResponse> AddVehicleFeedback(VehicleFeedbackReceivingDTO feedback)
        {
            return await _journeyStartandStopService.AddVehicleFeedback(HttpContext, feedback);
        }
    }
}
