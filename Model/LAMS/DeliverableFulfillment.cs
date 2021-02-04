using System;
using System.ComponentModel.DataAnnotations;

namespace HaloBiz.Model.LAMS
{
    public class DeliverableFulfillment
    {
        [Key]
        public long Id { get; set; }
        public string Description { get; set; }
        public string   Caption { get; set; }
        public long TaskFullfillmentId { get; set; }
        public TaskFulfillment TaskFullfillment { get; set; }
        public long? ResponsibleId { get; set; }
        public UserProfile Responsible { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public DateTime DeliveryDate { get; set; }
        public bool IsPicked { get; set; }
        public DateTime DateAndTimePicked { get; set; }
        public DateTime TaskCompletionDate { get; set; }
        public DateTime TaskCompletionTime { get; set; }
        public bool WasReassigned { get; set; } = false;
        public DateTime DateTimeReassigned { get; set; }
        public bool IsRequestedForValidation { get; set; } = false;
        public DateTime DateTimeRequestedForValidation { get; set; }
        public bool DeliverableStatus { get; set; } = false;
        public double Budget {get; set;}
        public string DeliverableCompletionReferenceNo { get; set; }
        public string DeliverableCompletionReferenceUrl { get; set; }
        public DateTime DateAndTimeOfProvidedEvidence { get; set; }
        public DateTime DeliverableCompletionDate { get; set; }
        public DateTime DeliverableCompletionTime { get; set; }
    }
}

/*
DependencyType
Dependencies
InterDependencyType
InterDependencies
*/