using System;
using System.Collections.Generic;
using HaloBiz.Helpers;

namespace halobiz_backend.DTOs.TransferDTOs
{
    public class TaskDeliverablesSummary
    {
        public string TaskCaption { get; set; }
        public long TaskId { get; set; }
        public long? TaskResponsibleId { get; set; }
        public long DeliverableId { get; set; }
        public DeliverablePriority Priority { get; set; }
        public string DeliverableCaption { get; set; }
        public DateTime? DeliveryDate { get; set; }
        public DateTime StartDate { get; set; }
        public string TaskResponsibleImageUrl { get; set; }
        public long? DeliverableResponsibleId { get; set; }
        public bool IsPicked { get; set; }
        public string TaskResponsibleName { get; set; }
        public bool DeliverableStatus { get; set; }
        public bool DeliverableWasReassigned { get; set; }
        public bool IsRequestedForValidation { get; set; }
        public string Project { get; set; }
        
        
        
    }

    public class TaskWithListOfDeliverables
    {
        public string TaskCaption { get; set; }
        public long TaskId { get; set; }
        public long? TaskResponsibleId { get; set; }
        public string TaskResponsibleImageUrl { get; set; }
        public string TaskResponsibleName { get; set; }
        public List<DeliverableSummary> DeliverableSummaries { get; set; }
        public string Project { get; set; }
    }

    public class DeliverableSummary
    {
        public long DeliverableId { get; set; }
        public string DeliverableCaption { get; set; }
        public DateTime? DeliveryDate { get; set; }
        public bool DeliverableStatus { get; set; }
        public long? DeliverableResponsibleId { get; set; }
        public bool IsRequestedForValidation { get; set; }
        
        public bool IsPicked { get; set; }
        public DeliverablePriority Priority { get; set; }
        public DeliveryState DeliveryState { get; set; }
    }

    public enum DeliveryState
    {
       OnTrack, AtRisk, Overdue
    }
}