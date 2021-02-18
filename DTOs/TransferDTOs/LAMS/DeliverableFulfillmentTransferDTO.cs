using System;
using System.Collections.Generic;
using HaloBiz.Model;
using HaloBiz.Model.LAMS;

namespace HaloBiz.DTOs.TransferDTOs.LAMS
{
    public class DeliverableFulfillmentTransferDTO : DeliverableFulfillmentWithouthTaskFulfillmentTransferDTO
    {
        public TaskFulfillment TaskFullfillment { get; set; }
    }
    public class DeliverableFulfillmentWithouthTaskFulfillmentTransferDTO
    {
        public long Id { get; set; }
        public string Description { get; set; }
        public string Caption { get; set; }
        public long TaskFullfillmentId { get; set; }
        public long? ResponsibleId { get; set; }
        public UserProfile Responsible { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public DateTime? DeliveryDate { get; set; }
        public bool IsPicked { get; set; } = false;
        public DateTime? DateAndTimePicked { get; set; }
        public DateTime? TaskCompletionDate { get; set; }
        public DateTime? TaskCompletionTime { get; set; }
        public bool WasReassigned { get; set; } = false;
        public DateTime? DateTimeReassigned { get; set; }
        public bool IsRequestedForValidation { get; set; } = false;
        public DateTime? DateTimeRequestedForValidation { get; set; }
        public bool DeliverableStatus { get; set; } = false;
        public double? Budget { get; set; }
        public string DeliverableCompletionReferenceNo { get; set; }
        public string DeliverableCompletionReferenceUrl { get; set; }
        public DateTime? DateAndTimeOfProvidedEvidence { get; set; }
        public DateTime? DeliverableCompletionDate { get; set; }
        public DateTime? DeliverableCompletionTime { get; set; }
        public long CreatedById { get; set; }
        public UserProfile CreatedBy { get; set; }
        public string ServiceCode { get; set; }
        public long EscallationTimeDurationForPicking { get; set; }
    }
}