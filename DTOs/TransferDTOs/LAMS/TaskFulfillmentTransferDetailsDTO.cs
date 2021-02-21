using System;
using System.Collections.Generic;
using HaloBiz.Model;
using HaloBiz.Model.LAMS;

namespace HaloBiz.DTOs.TransferDTOs.LAMS
{
    public class TaskFulfillmentTransferDetailsDTO
    {
        public long Id { get; set; }
        public string Caption { get; set; }
        public string Description { get; set; }
        public long CustomerDivisionId { get; set; }
        public CustomerDivision CustomerDivision { get; set; }
        public long ContractServiceId { get; set; }
        public ContractServiceTransferDTO ContractService { get; set; }
        public long? ResponsibleId { get; set; }
        public UserProfile Responsible { get; set; }
        public long? AccountableId { get; set; }
        public UserProfile Accountable { get; set; }
        public long? ConsultedId { get; set; }
        public UserProfile Consulted { get; set; }
        public long? InformedId { get; set; }
        public UserProfile Informed { get; set; }
        public long? SupportId { get; set; }
        public UserProfile Support { get; set; }
        public double? Budget { get; set; }
        public string ProjectCode { get; set; }
        public bool IsPicked { get; set; } = false;
        public DateTime? DateTimePicked { get; set; }
        public DateTime? ProjectDeliveryDate { get; set; }
        public bool IsAllDeliverableAssigned { get; set; } = false;
        public DateTime? TaskCompletionDateTime { get; set; }
        public bool TaskCompletionStatus { get; set; } = false;
        public long CreatedById { get; set; }
        public virtual UserProfile CreatedBy { get; set; }

        // Additional Details.
        public LeadDivisionContact PrimaryContact { get; set; }
        public LeadDivisionContact SecondaryContact { get; set; }
        public IEnumerable<LeadDivisionKeyPerson> LeadDivisionKeyPersons { get; set; }
        public IEnumerable<DeliverableFulfillmentWithouthTaskFulfillmentTransferDTO> DeliverableFulfillments { get; set; }
        public ServiceDivisionDetails ServiceDivisionDetails { get; set; }
    }

    public class ServiceDivisionDetails
    {
        public string Division { get; set; }
        public string OperatingEntity { get; set; }
        public string ServiceCategory { get; set; }
        public string ServiceGroup { get; set; }
        public string Service { get; set; }
    }
}