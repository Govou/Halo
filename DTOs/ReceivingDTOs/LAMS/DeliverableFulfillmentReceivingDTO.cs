using HaloBiz.Helpers;
using HaloBiz.Model.LAMS;
using System;
using System.ComponentModel.DataAnnotations;

namespace HaloBiz.DTOs.ReceivingDTOs
{
    public class DeliverableFulfillmentReceivingDTO
    {
        public string Description { get; set; }
        public string Caption { get; set; }
        public long TaskFullfillmentId { get; set; }
        public long? ResponsibleId { get; set; }
        public DateTime? StartDate { get; set; }
        public DeliverablePriority Priority { get; set; } =  DeliverablePriority.Low;
        public DateTime? EndDate { get; set; }
        public DateTime? DeliveryDate { get; set; }
        public bool IsRequestedForValidation { get; set; } = false;
        public double? Budget { get; set; }
        public string DeliverableCompletionReferenceNo { get; set; }
        public string DeliverableCompletionReferenceUrl { get; set; }
        public string ServiceCode { get; set; }
        public long EscallationTimeDurationForPicking { get; set; } = 0;
    }
}