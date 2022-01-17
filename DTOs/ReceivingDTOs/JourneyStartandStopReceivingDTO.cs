using HalobizMigrations.Models.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HaloBiz.DTOs.ReceivingDTOs
{
    public class JourneyStartandStopReceivingDTO
    {
    }
    public class JourneyStartReceivingDTO
    {
        //public int TotalTimeSpentOnJourney { get; set; }
        public string JourneyStartActualAddress { get; set; }
        public double JourneyStartActualLatitude { get; set; }
        //public DateTime JourneyStartDatetime { get; set; }
        public double JourneyStartActualLongitude { get; set; }
        public long ServiceAssignmentId { get; set; }

    }
    public class JourneyStopReceivingDTO
    {
      
        public string JourneyStopCaption { get; set; }
        public string JourneyStopDescription { get; set; }
        public long JourneyStartId { get; set; }
        public double JourneyStopActualLongitude { get; set; }
        public double JourneyStopActualLatitude { get; set; }
        public bool IsJourneyStopPlanned { get; set; }
    
    }
    public class JourneyIncidentReceivingDTO
    {
        public string JourneyIncidentActualAddress { get; set; }
        public double JourneyIncidentActualLongitude { get; set; }
        public double JourneyIncidentActualLatitude { get; set; }
        public JourneyIncidentType JourneyIncidentType { get; set; }
        public string JourneyIncidentDescription { get; set; }
        public string JourneyIncidentCaption { get; set; }
        public long JourneyStartId { get; set; }
    }
    public class JourneyIncidentPictureReceivingDTO
    {
        public string JourneyIncidentId { get; set; }
        public string JourneyPicture { get; set; }
    }
    public class JourneyLeadCommanderReceivingDTO
    {
        //public bool IsActive { get; set; }
        public long JourneyStartId { get; set; }
        public long LeadCommanderId { get; set; }
    }
    public class JourneyNoteReceivingDTO
    {
        public long JourneyStartId { get; set; }
        public string JourneyNoteCaption { get; set; }
        public string JourneyNoteDescription { get; set; }
    }

    public class JourneyEndReceivingDTO
    {
        public int TotalTimeSpentOnJourney { get; set; }
        public string JourneyEndActualAddress { get; set; }
        public double JourneyEndActualLongitude { get; set; }
        public double JourneyEndActualLatitude { get; set; }
        public long ServiceAssignmentId { get; set; }
        
    }
}
