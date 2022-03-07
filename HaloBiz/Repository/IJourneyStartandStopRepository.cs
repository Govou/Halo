using HalobizMigrations.Models.Armada;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HaloBiz.Repository
{
    public interface IJourneyStartandStopRepository
    {
        //Start
        Task<ArmadaJourneyStart> SaveJourneyStart(ArmadaJourneyStart journeyStart);

        Task<ArmadaJourneyStart> FindJourneyStartById(long Id);

        Task<IEnumerable<ArmadaJourneyStart>> FindAllJouneyStarts();

        //ArmadaJourneyStart GetTypename(string rankName);

        Task<ArmadaJourneyStart> UpdateJourneyStartForStop(ArmadaJourneyStart journeyStart);
        Task<ArmadaJourneyStart> UpdateEndJouneyStart(ArmadaJourneyStart journeyStart);
        Task<bool> UpdateStopJouneyStart(ArmadaJourneyStart journeyStart);
        Task<bool> UpdateCancelJouneyStart(ArmadaJourneyStart journeyStart);
        Task<bool> UpdateCServiceAssignmentOnJouneyStart(MasterServiceAssignment masterServiceAssignment);
        Task<bool> UpdateCServiceAssignmentOnJouneyStartForEndJourney(MasterServiceAssignment masterServiceAssignment);

        Task<bool> DeleteJouneyStart(ArmadaJourneyStart journeyStart);

        //Stop
        Task<ArmadaJourneyStop> SaveJourneyStop(ArmadaJourneyStop journeyStop);

        Task<ArmadaJourneyStop> FindJourneyStopById(long Id);

        Task<IEnumerable<ArmadaJourneyStop>> FindAllJouneyStops();
        Task<IEnumerable<ArmadaJourneyStop>> FindAllJouneyStopsByStartId(long startId);
        //ArmadaJourneyStart GetTypename(string rankName);

        Task<ArmadaJourneyStop> UpdateJourneyStop(ArmadaJourneyStop journeyStop);

        Task<bool> DeleteJouneyStop(ArmadaJourneyStop journeyStop);

        //Incident
        Task<JourneyIncident> SaveJourneyIncident(JourneyIncident incident);

        Task<JourneyIncident> FindJourneyIncidentById(long Id);

        Task<IEnumerable<JourneyIncident>> FindAllJouneyIncidents();
        Task<IEnumerable<JourneyIncident>> FindAllJouneyIncidentsByJourneyStartId(long journeyStartId);

        //ArmadaJourneyStart GetTypename(string rankName);

        Task<JourneyIncident> UpdateJourneyIncident(JourneyIncident incident);

        Task<bool> DeleteJouneyIncident(JourneyIncident incident);

        //IncidentPic
        Task<JourneyIncidentPicture> SaveJourneyIncidentPic(JourneyIncidentPicture incidentPic);

        Task<JourneyIncidentPicture> FindJourneyIncidentPicById(long Id);

        Task<IEnumerable<JourneyIncidentPicture>> FindAllJouneyIncidentPics();
        Task<IEnumerable<JourneyIncidentPicture>> FindAllJouneyIncidentPicsByIncidentId(long incidentId);

        // Task<JourneyIncidentPicture> UpdateJourneyIncident(JourneyIncidentPicture incident);

        Task<bool> DeleteJouneyIncidentPic(JourneyIncidentPicture incidentPic);

        //LeadCommander
        Task<JourneyLeadCommander> SaveJourneyLeadCommander(JourneyLeadCommander leadCommander);

        Task<JourneyLeadCommander> FindJourneyLeadCommanderById(long Id);

        Task<IEnumerable<JourneyLeadCommander>> FindAllJouneyLeadCommanders();
        //Task<IEnumerable<JourneyIncidentPicture>> FindAllJouneyIncidentPicsByIncidentId(long incidentId);

        // Task<JourneyIncidentPicture> UpdateJourneyIncident(JourneyIncidentPicture incident);

        Task<bool> DeleteJourneyLeadCommander(JourneyLeadCommander leadCommander);
        Task<bool> RelinquishLeadCommander(JourneyLeadCommander leadCommander);

        //Notes
        Task<JourneyNote> SaveJourneyNote(JourneyNote journeyNote);

        Task<JourneyNote> FindJourneyNoteById(long Id);

        Task<IEnumerable<JourneyNote>> FindAllJourneyNotes();
        Task<IEnumerable<JourneyNote>> FindAllJouneyNotesByStartId(long journeyStartId);
        Task<JourneyNote> UpdateJourneyNote(JourneyNote journeyNote);

        Task<bool> DeleteJourneyNote(JourneyNote journeyNote);
    }
}
