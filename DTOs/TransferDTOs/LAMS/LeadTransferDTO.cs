using System;
using System.Collections.Generic;
using HalobizMigrations.Models;


namespace HaloBiz.DTOs.TransferDTOs.LAMS
{
    public class LeadTransferDTO
    {
        public long Id { get; set; }
        public string ReferenceNo { get; set; }
        public virtual LeadTypeWithoutOriginDTO LeadType { get; set; }
        public long LeadOriginId { get; set; }
        public virtual LeadOriginTransferDTO LeadOrigin { get; set; }
        public string Industry { get; set; }
        public string RCNumber { get; set; }
        public string GroupName { get; set; }
        public GroupTypeTransferDTO GroupType { get; set; }
        public string LogoUrl { get; set; }
        public bool LeadCaptureStatus { get; set; }
        public bool LeadQualificationStatus { get; set; } 
        public bool LeadOpportunityStatus { get; set; } 
        public bool LeadClosureStatus { get; set; }
        public bool LeadConversionStatus { get; set; } 
        public bool IsLeadDropped { get; set; }
        public string DropLearning { get; set; }
        public string LeadCaptureDocumentUrl { get; set; }
        public  DropReasonTransferDTO DropReason { get; set; }
        public  LeadContactTransferDTO PrimaryContact { get; set; }
        public LeadContactTransferDTO SecondaryContact { get; set; }
        public IEnumerable<LeadKeyPersonTransferDTO> LeadKeyPersons { get; set; }
        public IEnumerable<LeadDivisionTransferDTO> LeadDivisions { get; set; }
        public UserProfileTransferDTO CreatedBy { get; set; }
        
    }
    public class LeadWithoutModelsTransferDTO
    {
        public long Id { get; set; }
        public string ReferenceNo { get; set; }
        public virtual LeadTypeTransferDTO LeadType { get; set; }
        public long LeadOriginId { get; set; }
        public virtual LeadOriginTransferDTO LeadOrigin { get; set; }
        public string Industry { get; set; }
        public string RCNumber { get; set; }
        public string GroupName { get; set; }
        public GroupTypeTransferDTO GroupType { get; set; }
        public string LogoUrl { get; set; }
        public bool LeadCaptureStatus { get; set; }
        public bool LeadQualificationStatus { get; set; } 
        public bool LeadOpportunityStatus { get; set; } 
        public bool LeadClosureStatus { get; set; }
        public bool LeadConversionStatus { get; set; } 
        public bool IsLeadDropped { get; set; }
        public string DropLearning { get; set; }
        public  DropReasonTransferDTO DropReason { get; set; }
        public  LeadContactTransferDTO PrimaryContact { get; set; }
        public LeadContactTransferDTO SecondaryContact { get; set; }
        public UserProfileTransferDTO CreatedBy { get; set; }
        
    }
}