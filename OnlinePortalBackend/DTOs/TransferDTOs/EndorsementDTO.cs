using HalobizMigrations.Models;
using System;

namespace OnlinePortalBackend.DTOs.TransferDTOs
{
    public class EndorsementDTO
    {
        public long Id { get; set; }
        public long EndorsementTypeId { get; set; }
        public bool IsRequestedForApproval { get; set; }
        public bool IsApproved { get; set; }
        public bool IsDeclined { get; set; }
        public double? UnitPrice { get; set; }
        public long Quantity { get; set; }
        public double Discount { get; set; }

        public double? Vat { get; set; }

        public double? BillableAmount
        {
            get;
            set;
        }

        public double? Budget
        {
            get;
            set;
        }

        public DateTime? ContractStartDate
        {
            get;
            set;
        }

        public DateTime? ContractEndDate
        {
            get;
            set;
        }

        public int? PaymentCycle
        {
            get;
            set;
        }

        public long? PaymentCycleInDays
        {
            get;
            set;
        }

        public long? InvoiceCycleInDays
        {
            get;
            set;
        }

        public DateTime? FirstInvoiceSendDate
        {
            get;
            set;
        }

        public int? InvoicingInterval
        {
            get;
            set;
        }

        public string ProblemStatement
        {
            get;
            set;
        }

        public DateTime? ActivationDate
        {
            get;
            set;
        }

        public DateTime? FulfillmentStartDate
        {
            get;
            set;
        }

        public DateTime? FulfillmentEndDate
        {
            get;
            set;
        }

        public DateTime? TentativeDateForSiteSurvey
        {
            get;
            set;
        }

        public DateTime? PickupDateTime
        {
            get;
            set;
        }

        public DateTime? DropoffDateTime
        {
            get;
            set;
        }
        public string PickupLocation
        {
            get;
            set;
        }

        public string Dropofflocation
        {
            get;
            set;
        }

        public string BeneficiaryName
        {
            get;
            set;
        }

        public string BeneficiaryIdentificationType
        {
            get;
            set;
        }

        public string BenificiaryIdentificationNumber
        {
            get;
            set;
        }

        public DateTime? TentativeProofOfConceptStartDate
        {
            get;
            set;
        }

        public DateTime? TentativeProofOfConceptEndDate
        {
            get;
            set;
        }

        public DateTime? TentativeFulfillmentDate
        {
            get;
            set;
        }

        public DateTime? ProgramCommencementDate
        {
            get;
            set;
        }

        public long? ProgramDuration
        {
            get;
            set;
        }

        public DateTime? ProgramEndDate
        {
            get;
            set;
        }

        public DateTime? TentativeDateOfSiteVisit
        {
            get;
            set;
        }

        public long ServiceId
        {
            get;
            set;
        }

        public long ContractId
        {
            get;
            set;
        }

        public long CreatedById
        {
            get;
            set;
        }

        public bool IsDeleted
        {
            get;
            set;
        }

        public DateTime CreatedAt
        {
            get;
            set;
        }

        public DateTime UpdatedAt
        {
            get;
            set;
        }

        public long? PreviousContractServiceId
        {
            get;
            set;
        }

        public DateTime? DateForNewContractToTakeEffect
        {
            get;
            set;
        }

        public bool? IsConvertedToContractService
        {
            get;
            set;
        }

        public string EndorsementDescription
        {
            get;
            set;
        }

        public virtual Contract Contract
        {
            get;
            set;
        }

        public virtual EndorsementType EndorsementType
        {
            get;
            set;
        }

        public virtual Service Service
        {
            get;
            set;
        }



      
        public string UniqueTag
        {
            get;
            set;
        }

        public long? QuoteServiceId
        {
            get;
            set;
        }
        public QuoteService QuoteService
        {
            get;
            set;
        }
    }
}
