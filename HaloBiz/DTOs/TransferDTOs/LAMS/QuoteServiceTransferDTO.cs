using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Halobiz.Common.DTOs.ReceivingDTOs;
using Halobiz.Common.Helpers;
using HaloBiz.Helpers;
using HalobizMigrations.Models;


namespace HaloBiz.DTOs.TransferDTOs.LAMS
{
    public class QuoteServiceTransferDTO
    {
        public long Id { get; set; }
        public double? UnitPrice { get; set; }
        public long Quantity { get; set; }
        public double Discount { get; set; }
        public double? VAT { get; set; }
        public double? BillableAmount { get; set; }
        public double? Budget { get; set; }
        public DateTime? ContractStartDate { get; set; }
        public DateTime? ContractEndDate { get; set; }
        public TimeCycle? PaymentCycle { get; set; }
        public long? PaymentCycleInDays { get; set; }
        public long? InvoiceCycleInDays {get ; set; }
        public DateTime? FirstInvoiceSendDate { get; set; }
        public TimeCycle? InvoicingInterval { get; set; }
        public string ProblemStatement { get; set; }
        public DateTime? ActivationDate { get; set; }
        public DateTime? FulfillmentStartDate { get; set; }
        public DateTime? FulfillmentEndDate { get; set; }
        public DateTime? TentativeDateForSiteSurvey { get; set; }
        public DateTime? PickupDateTime { get; set; }
        public DateTime? DropoffDateTime { get; set; }

        public string PickupLocation { get; set; }

        public string Dropofflocation { get; set; }

        public string BeneficiaryName { get; set; }

        public string BeneficiaryIdentificationType { get; set; }
  
        public string BenificiaryIdentificationNumber { get; set; }

        public DateTime? TentativeProofOfConceptStartDate { get; set; }
        public DateTime? TentativeProofOfConceptEndDate { get; set; }
        public DateTime? TentativeFulfillmentDate { get; set; }
        public DateTime? ProgramCommencementDate { get; set; }
        public long? ProgramDuration { get; set; }
        public DateTime? ProgramEndDate { get; set; }
        public DateTime? TentativeDateOfSiteVisit { get; set; }
        public bool IsConvertedToContractService { get; set; }
        public long ServiceId { get; set; }
        public Service Service { get; set; }
        public long QuoteId { get; set; }

        public IEnumerable<ContractService> ContractService { get; set; }
        public IEnumerable<QuoteServiceDocumentTransferDTO> QuoteServiceDocuments { get; set; }
        public IEnumerable<SbutoQuoteServiceProportionTransferDTO> SbutoQuoteServiceProportions { get; set; }
        public VersionType Version { get; set; }
        public long CreatedById { get; set; }
        public virtual UserProfile CreatedBy { get; set; }
        public string UniqueTag { get; set; }
        public string AdminDirectTie { get; set; }
        public long BranchId { get; set; }
        public long OfficeId { get; set; }

    }
}