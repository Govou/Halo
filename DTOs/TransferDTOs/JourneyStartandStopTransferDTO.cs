using HalobizMigrations.Models;
using HalobizMigrations.Models.Armada;
using HalobizMigrations.Models.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HaloBiz.DTOs.TransferDTOs
{
    public class JourneyStartandStopTransferDTO
    {
    }
    public class JourneyStartTransferDTO
    {
        public long Id { get; set; }
        public DateTime CreatedAt { get; set; }
        public UserProfile CreatedBy { get; set; }
        public long CreatedById { get; set; }
        public int TotalTimeSpentOnJourney { get; set; }
        public string JourneyEndActualAddress { get; set; }
        public double JourneyEndActualLongitude { get; set; }
        public double JourneyEndActualLatitude { get; set; }
        public DateTime JourneyEndDatetime { get; set; }
        public DateTime UpdatedAt { get; set; }
        public string JourneyStartActualAddress { get; set; }
        public double JourneyStartActualLatitude { get; set; }
        public DateTime JourneyStartDatetime { get; set; }
        public bool IsJourneyStopped { get; set; }
        public bool IsJourneyCancelled { get; set; }
        public bool IsJourneyStarted { get; set; }
        public MasterServiceAssignment ServiceAssignment { get; set; }
        public long? ServiceAssignmentId { get; set; }
        public double JourneyStartActualLongitude { get; set; }
        public bool IsDeleted { get; set; }
    }
    public class JourneyStopTransferDTO
    {

   
        public long Id { get; set; }
        public long? JourneyStartId { get; set; }
        public ArmadaJourneyStart JourneyStart { get; set; }
        public string JourneyStopCaption { get; set; }
        public string JourneyStopDescription { get; set; }
        public DateTime JourneyStopTime { get; set; }
        public double JourneyStopActualLongitude { get; set; }
        public double JourneyStopActualLatitude { get; set; }
        public bool IsJourneyStopPlanned { get; set; }
        public long CreatedById { get; set; }
        public UserProfile CreatedBy { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public bool IsDeleted { get; set; }
    }
    public class JourneyIncidentTransferDTO
    {
        public DateTime CreatedAt { get; set; }
       
        public UserProfile CreatedBy { get; set; }
        public long CreatedById { get; set; }
        public DateTime JourneyIncidentTime { get; set; }
        public string JourneyIncidentActualAddress { get; set; }
        public double JourneyIncidentActualLongitude { get; set; }
        public DateTime UpdatedAt { get; set; }
        public double JourneyIncidentActualLatitude { get; set; }
        public JourneyIncidentType JourneyIncidentType { get; set; }
        public string JourneyIncidentDescription { get; set; }
        public string JourneyIncidentCaption { get; set; }
        public ArmadaJourneyStart JourneyStart { get; set; }
        public long? JourneyStartId { get; set; }
        
        public long Id { get; set; }
        public int JourneyIncidentSeverityRating { get; set; }
        public bool IsDeleted { get; set; }
    }
    public class JourneyIncidentPictureTransferDTO
    {
       
        public long Id { get; set; }
        public long? JourneyIncidentId { get; set; }
        
        public JourneyIncident JourneyIncident { get; set; }
        public string JourneyPicture { get; set; }
        public long CreatedById { get; set; }
       
        public UserProfile CreatedBy { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public bool IsDeleted { get; set; }
    }
    public class JourneyLeadCommanderTransferDTO
    {
       
        public long Id { get; set; }
        public long? JourneyStartId { get; set; }
        public ArmadaJourneyStart JourneyStart { get; set; }
        public bool IsActive { get; set; }
        public long? LeadCommanderId { get; set; }
       
        public CommanderProfile LeadCommander { get; set; }
        public long CreatedById { get; set; }
        
        public UserProfile CreatedBy { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public bool IsDeleted { get; set; }
    }
    public class JourneyNoteTransferDTO
    {
       
        public long Id { get; set; }
        public long? JourneyStartId { get; set; }
       
        public ArmadaJourneyStart JourneyStart { get; set; }
        public string JourneyNoteCaption { get; set; }
        public string JourneyNoteDescription { get; set; }
        public long CreatedById { get; set; }
      
        public UserProfile CreatedBy { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public bool IsDeleted { get; set; }
    }
}
