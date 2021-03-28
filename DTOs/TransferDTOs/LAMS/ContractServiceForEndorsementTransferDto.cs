using System;
using System.ComponentModel.DataAnnotations;
using HaloBiz.Helpers;
using HaloBiz.Model;
using HaloBiz.Model.LAMS;
using Contract = HaloBiz.Model.LAMS.Contract;

namespace HaloBiz.DTOs.TransferDTOs.LAMS
{
    public class ContractServiceForEndorsementTransferDto
    {
        public long Id { get; set; }
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
        public ServicesTransferDTO Service { get; set; }
        public long ContractId { get; set; }
        public ContractTransferDTO Contract { get; set; }
        public string GroupInvoiceNumber { get; set; }
        public long EndorsementTypeId { get; set; }        
        public EndorsementType EndorsementType { get; set; }
        public long CreatedById { get; set; }
        public UserProfile CreatedBy { get; set; }
        public bool IsRequestedForApproval { get; set; } = true;
        public bool IsApproved { get; set; } = false;
        public bool IsDeclined { get; set; } = false;
        public long CustomerDivisionId { get; set; }
        public CustomerDivision CustomerDivision { get; set; }
        public long BranchId { get; set; }
        public long OfficeId { get; set; }
        public long PreviousContractServiceId { get; set; }
        public DateTime DateForNewContractToTakeEffect { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}