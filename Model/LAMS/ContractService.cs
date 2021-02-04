using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using HaloBiz.Helpers;

namespace HaloBiz.Model.LAMS
{
    public class ContractService
    {
        [Key]
        public long Id { get; set; }
        [StringLength(50)]
        public string ReferenceNo { get; set; }
        public double? UnitPrice {get ; set; }
        public long Quantity {get ; set; }
        public double Discount {get ; set; }
        public double? VAT {get ; set; }
        public double? BillableAmount {get ; set; }
        public double? Budget {get ; set; }
        public DateTime? ContractStartDate {get ; set; }
        public DateTime? ContractEndDate {get ; set; }
        public TimeCycle? PaymentCycle {get ; set; }
        public long? PaymentCycleInDays {get ; set; }
        public long? InvoiceCycleInDays {get ; set; }
        public DateTime? FirstInvoiceSendDate {get ; set; }
        public TimeCycle? InvoicingInterval  {get ; set; }
        [StringLength(2000)]
        public string ProblemStatement {get ; set; }
        public DateTime? ActivationDate {get ; set; }
        public DateTime? FulfillmentStartDate {get ; set; }
        public DateTime? FulfillmentEndDate {get ; set; }
        public DateTime? TentativeDateForSiteSurvey {get ; set; }
        public DateTime? PickupDateTime {get ; set; }
        public DateTime? DropoffDateTime {get ; set; }
        [StringLength(1000)]
        public string PickupLocation {get ; set; }
        [StringLength(1000)]
        public string Dropofflocation {get ; set; }
        [StringLength(200)]
        public string BeneficiaryName {get ; set; }
        [StringLength(200)]
        public string BeneficiaryIdentificationType {get ; set; }
        [StringLength(200)]
        public string BenificiaryIdentificationNumber {get ; set; }

        public DateTime? TentativeProofOfConceptStartDate {get ; set; }
        public DateTime? TentativeProofOfConceptEndDate {get ; set; }
        public DateTime? TentativeFulfillmentDate {get ; set; }
        public DateTime? ProgramCommencementDate {get ; set; }
        public long? ProgramDuration {get ; set; }
        public DateTime? ProgramEndDate {get ; set; }
        public DateTime? TentativeDateOfSiteVisit {get ; set; }
        public long ServiceId { get; set; }
        public Services Service { get; set; }
        public long ContractId { get; set; }
        public virtual Contract Contract { get; set; }
        public long QuoteServiceId { get; set; }
        public IEnumerable<TaskFulfillment> TaskFulfillments { get; set; }
        public virtual QuoteService QuoteService { get; set; }
        public IEnumerable<ClosureDocument> ClosureDocuments { get; set; }
        public IEnumerable<SBUToContractServiceProportion> SBUToContractServiceProportions { get; set; }
        public VersionType Version { get; set; } = VersionType.Latest;
        public long CreatedById { get; set; }
        public virtual UserProfile CreatedBy { get; set; }
        public bool IsDeleted { get; set; } = false;
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public DateTime CreatedAt { get; set; }
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public DateTime UpdatedAt { get; set; }
    }
}