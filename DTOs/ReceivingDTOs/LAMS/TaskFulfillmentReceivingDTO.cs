
using System;
using System.ComponentModel.DataAnnotations;

namespace HaloBiz.DTOs.ReceivingDTOs
{
    public class TaskFulfillmentReceivingDTO
    {
        public string Caption { get; set; }
        public string Description { get; set; }
        public long CustomerDivisionId { get; set; }
        public long ContractServiceId { get; set; }
        public long? ResponsibleId { get; set; }
        public long? AccountableId { get; set; }
        public long? ConsultedId { get; set; }
        public long? InformedId { get; set; }
        public long? SupportId { get; set; }
        public double? Budget { get; set; }
        public string ProjectCode { get; set; }
        public DateTime? ProjectDeliveryDate { get; set; }
        public bool IsPicked { get; set; } = false;
        public DateTime? DateTimePicked { get; set; }
        public bool IsAllDeliverableAssigned { get; set; } = false;
        public DateTime? TaskCompletionDateTime { get; set; }
        public bool TaskCompletionStatus { get; set; } = false;
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string ServiceCode { get; set; }
    }
}