
using System;
using System.ComponentModel.DataAnnotations;

namespace Halobiz.Common.Auths.PermissionParts
{
    public enum Permissions : short
    {
        NotSet = 0, //error or empty state condition

        [Display(ShortName = "ClientManagement", GroupName = "ClientBeneficiary", Name = "Get", Description = "Can view client beneficiary")]
        ClientBeneficiary_Get = 0x1,
        [Display(ShortName = "ClientManagement", GroupName = "ClientBeneficiary", Name = "Post", Description = "Can create client beneficiary")]
        ClientBeneficiary_Post = 0x2,
        [Display(ShortName = "ClientManagement", GroupName = "ClientBeneficiary", Name = "Put", Description = "Can update client beneficiary")]
        ClientBeneficiary_Put = 0x3,
        [Display(ShortName = "ClientManagement", GroupName = "ClientBeneficiary", Name = "Delete", Description = "Can delete client beneficiary")]
        ClientBeneficiary_Delete = 0x4,
        [Display(ShortName = "ClientManagement", GroupName = "ClientEngagement", Name = "Get", Description = "Can view client engagement")]
        ClientEngagement_Get = 0x5,
        [Display(ShortName = "ClientManagement", GroupName = "ClientEngagement", Name = "Post", Description = "Can create client engagement")]
        ClientEngagement_Post = 0x6,
        [Display(ShortName = "ClientManagement", GroupName = "ClientEngagement", Name = "Put", Description = "Can update client engagement")]
        ClientEngagement_Put = 0x7,
        [Display(ShortName = "ClientManagement", GroupName = "ClientEngagement", Name = "Delete", Description = "Can delete client engagement")]
        ClientEngagement_Delete = 0x8,
        [Display(ShortName = "LeadAdministration", GroupName = "ClosureDocument", Name = "Get", Description = "Can view closure document")]
        ClosureDocument_Get = 0x9,
        [Display(ShortName = "LeadAdministration", GroupName = "ClosureDocument", Name = "Post", Description = "Can create closure document")]
        ClosureDocument_Post = 0x10,
        [Display(ShortName = "LeadAdministration", GroupName = "ClosureDocument", Name = "Put", Description = "Can update closure document")]
        ClosureDocument_Put = 0x11,
        [Display(ShortName = "LeadAdministration", GroupName = "ClosureDocument", Name = "Delete", Description = "Can delete closure document")]
        ClosureDocument_Delete = 0x12,
        [Display(ShortName = "ProjectManagment", GroupName = "DeliverableFulfillment", Name = "Get", Description = "Can view deliverable fulfillment")]
        DeliverableFulfillment_Get = 0x13,
        [Display(ShortName = "ProjectManagment", GroupName = "DeliverableFulfillment", Name = "Post", Description = "Can create deliverable fulfillment")]
        DeliverableFulfillment_Post = 0x14,
        [Display(ShortName = "ProjectManagment", GroupName = "DeliverableFulfillment", Name = "Put", Description = "Can update deliverable fulfillment")]
        DeliverableFulfillment_Put = 0x15,
        [Display(ShortName = "ProjectManagment", GroupName = "DeliverableFulfillment", Name = "Delete", Description = "Can delete deliverable fulfillment")]
        DeliverableFulfillment_Delete = 0x16,
        [Display(ShortName = "LeadAdministration", GroupName = "DropReason", Name = "Get", Description = "Can view drop reason")]
        DropReason_Get = 0x17,
        [Display(ShortName = "LeadAdministration", GroupName = "DropReason", Name = "Post", Description = "Can create drop reason")]
        DropReason_Post = 0x18,
        [Display(ShortName = "LeadAdministration", GroupName = "DropReason", Name = "Put", Description = "Can update drop reason")]
        DropReason_Put = 0x19,
        [Display(ShortName = "LeadAdministration", GroupName = "DropReason", Name = "Delete", Description = "Can delete drop reason")]
        DropReason_Delete = 0x20,
        [Display(ShortName = "ClientManagement", GroupName = "Endorsement", Name = "Get", Description = "Can view endorsement")]
        Endorsement_Get = 0x21,
        [Display(ShortName = "ClientManagement", GroupName = "Endorsement", Name = "Post", Description = "Can create endorsement")]
        Endorsement_Post = 0x22,
        [Display(ShortName = "ClientManagement", GroupName = "Endorsement", Name = "Put", Description = "Can update endorsement")]
        Endorsement_Put = 0x23,
        [Display(ShortName = "ClientManagement", GroupName = "Endorsement", Name = "Delete", Description = "Can delete endorsement")]
        Endorsement_Delete = 0x24,
        [Display(ShortName = "ClientManagement", GroupName = "EndorsementType", Name = "Get", Description = "Can view endorsement type")]
        EndorsementType_Get = 0x25,
        [Display(ShortName = "ClientManagement", GroupName = "EndorsementType", Name = "Post", Description = "Can create endorsement type")]
        EndorsementType_Post = 0x26,
        [Display(ShortName = "ClientManagement", GroupName = "EndorsementType", Name = "Put", Description = "Can update endorsement type")]
        EndorsementType_Put = 0x27,
        [Display(ShortName = "ClientManagement", GroupName = "EndorsementType", Name = "Delete", Description = "Can delete endorsement type")]
        EndorsementType_Delete = 0x28,
        [Display(ShortName = "LeadAdministration", GroupName = "LeadDivision", Name = "Get", Description = "Can view lead division")]
        LeadDivision_Get = 0x29,
        [Display(ShortName = "LeadAdministration", GroupName = "LeadDivision", Name = "Post", Description = "Can create lead division")]
        LeadDivision_Post = 0x30,
        [Display(ShortName = "LeadAdministration", GroupName = "LeadDivision", Name = "Put", Description = "Can update lead division")]
        LeadDivision_Put = 0x31,
        [Display(ShortName = "LeadAdministration", GroupName = "LeadDivision", Name = "Delete", Description = "Can delete lead division")]
        LeadDivision_Delete = 0x32,
        [Display(ShortName = "LeadAdministration", GroupName = "LeadEngagement", Name = "Get", Description = "Can view lead engagement")]
        LeadEngagement_Get = 0x33,
        [Display(ShortName = "LeadAdministration", GroupName = "LeadEngagement", Name = "Post", Description = "Can create lead engagement")]
        LeadEngagement_Post = 0x34,
        [Display(ShortName = "LeadAdministration", GroupName = "LeadEngagement", Name = "Put", Description = "Can update lead engagement")]
        LeadEngagement_Put = 0x35,
        [Display(ShortName = "LeadAdministration", GroupName = "LeadEngagement", Name = "Delete", Description = "Can delete lead engagement")]
        LeadEngagement_Delete = 0x36,
        [Display(ShortName = "Setups", GroupName = "LeadType", Name = "Get", Description = "Can view lead type")]
        LeadType_Get = 0x37,
        [Display(ShortName = "Setups", GroupName = "LeadType", Name = "Post", Description = "Can create lead type")]
        LeadType_Post = 0x38,
        [Display(ShortName = "Setups", GroupName = "LeadType", Name = "Put", Description = "Can update lead type")]
        LeadType_Put = 0x39,
        [Display(ShortName = "Setups", GroupName = "LeadType", Name = "Delete", Description = "Can delete lead type")]
        LeadType_Delete = 0x40,
        [Display(ShortName = "LeadAdministration", GroupName = "NegotiationDocument", Name = "Get", Description = "Can view negotiation document")]
        NegotiationDocument_Get = 0x41,
        [Display(ShortName = "LeadAdministration", GroupName = "NegotiationDocument", Name = "Post", Description = "Can create negotiation document")]
        NegotiationDocument_Post = 0x42,
        [Display(ShortName = "LeadAdministration", GroupName = "NegotiationDocument", Name = "Put", Description = "Can update negotiation document")]
        NegotiationDocument_Put = 0x43,
        [Display(ShortName = "LeadAdministration", GroupName = "NegotiationDocument", Name = "Delete", Description = "Can delete negotiation document")]
        NegotiationDocument_Delete = 0x44,
        [Display(ShortName = "LeadAdministration", GroupName = "OtherLeadCaptureInfo", Name = "Get", Description = "Can view other lead capture info")]
        OtherLeadCaptureInfo_Get = 0x45,
        [Display(ShortName = "LeadAdministration", GroupName = "OtherLeadCaptureInfo", Name = "Post", Description = "Can create other lead capture info")]
        OtherLeadCaptureInfo_Post = 0x46,
        [Display(ShortName = "LeadAdministration", GroupName = "OtherLeadCaptureInfo", Name = "Put", Description = "Can update other lead capture info")]
        OtherLeadCaptureInfo_Put = 0x47,
        [Display(ShortName = "LeadAdministration", GroupName = "OtherLeadCaptureInfo", Name = "Delete", Description = "Can delete other lead capture info")]
        OtherLeadCaptureInfo_Delete = 0x48,
        [Display(ShortName = "LeadAdministration", GroupName = "Quote", Name = "Get", Description = "Can view quote")]
        Quote_Get = 0x49,
        [Display(ShortName = "LeadAdministration", GroupName = "Quote", Name = "Post", Description = "Can create quote")]
        Quote_Post = 0x50,
        [Display(ShortName = "LeadAdministration", GroupName = "Quote", Name = "Put", Description = "Can update quote")]
        Quote_Put = 0x51,
        [Display(ShortName = "LeadAdministration", GroupName = "Quote", Name = "Delete", Description = "Can delete quote")]
        Quote_Delete = 0x52,
        [Display(ShortName = "LeadAdministration", GroupName = "QuoteService", Name = "Get", Description = "Can view quote service")]
        QuoteService_Get = 0x53,
        [Display(ShortName = "LeadAdministration", GroupName = "QuoteService", Name = "Post", Description = "Can create quote service")]
        QuoteService_Post = 0x54,
        [Display(ShortName = "LeadAdministration", GroupName = "QuoteService", Name = "Put", Description = "Can update quote service")]
        QuoteService_Put = 0x55,
        [Display(ShortName = "LeadAdministration", GroupName = "QuoteService", Name = "Delete", Description = "Can delete quote service")]
        QuoteService_Delete = 0x56,
        [Display(ShortName = "LeadAdministration", GroupName = "QuoteServiceDocument", Name = "Get", Description = "Can view quote service document")]
        QuoteServiceDocument_Get = 0x57,
        [Display(ShortName = "LeadAdministration", GroupName = "QuoteServiceDocument", Name = "Post", Description = "Can create quote service document")]
        QuoteServiceDocument_Post = 0x58,
        [Display(ShortName = "LeadAdministration", GroupName = "QuoteServiceDocument", Name = "Put", Description = "Can update quote service document")]
        QuoteServiceDocument_Put = 0x59,
        [Display(ShortName = "LeadAdministration", GroupName = "QuoteServiceDocument", Name = "Delete", Description = "Can delete quote service document")]
        QuoteServiceDocument_Delete = 0x60,
        [Display(ShortName = "LeadAdministration", GroupName = "TaskFulfillment", Name = "Get", Description = "Can view task fulfillment")]
        TaskFulfillment_Get = 0x61,
        [Display(ShortName = "LeadAdministration", GroupName = "TaskFulfillment", Name = "Post", Description = "Can create task fulfillment")]
        TaskFulfillment_Post = 0x62,
        [Display(ShortName = "LeadAdministration", GroupName = "TaskFulfillment", Name = "Put", Description = "Can update task fulfillment")]
        TaskFulfillment_Put = 0x63,
        [Display(ShortName = "LeadAdministration", GroupName = "TaskFulfillment", Name = "Delete", Description = "Can delete task fulfillment")]
        TaskFulfillment_Delete = 0x64,
        [Display(ShortName = "RolesManagement", GroupName = "Activity", Name = "Get", Description = "Can view activity")]
        Activity_Get = 0x65,
        [Display(ShortName = "RolesManagement", GroupName = "Activity", Name = "Post", Description = "Can create activity")]
        Activity_Post = 0x66,
        [Display(ShortName = "RolesManagement", GroupName = "Activity", Name = "Put", Description = "Can update activity")]
        Activity_Put = 0x67,
        [Display(ShortName = "RolesManagement", GroupName = "Activity", Name = "Delete", Description = "Can delete activity")]
        Activity_Delete = 0x68,
        [Display(ShortName = "Setups", GroupName = "AppFeedback", Name = "Get", Description = "Can view app feedback")]
        AppFeedback_Get = 0x69,
        [Display(ShortName = "Setups", GroupName = "AppFeedback", Name = "Post", Description = "Can create app feedback")]
        AppFeedback_Post = 0x70,
        [Display(ShortName = "Setups", GroupName = "AppFeedback", Name = "Put", Description = "Can update app feedback")]
        AppFeedback_Put = 0x71,
        [Display(ShortName = "Setups", GroupName = "AppFeedback", Name = "Delete", Description = "Can delete app feedback")]
        AppFeedback_Delete = 0x72,
        [Display(ShortName = "Setups", GroupName = "AppReview", Name = "Get", Description = "Can view app review")]
        AppReview_Get = 0x73,
        [Display(ShortName = "Setups", GroupName = "AppReview", Name = "Post", Description = "Can create app review")]
        AppReview_Post = 0x74,
        [Display(ShortName = "Setups", GroupName = "AppReview", Name = "Put", Description = "Can update app review")]
        AppReview_Put = 0x75,
        [Display(ShortName = "Setups", GroupName = "AppReview", Name = "Delete", Description = "Can delete app review")]
        AppReview_Delete = 0x76,
        [Display(ShortName = "ApprovalManagement", GroupName = "Approval", Name = "Get", Description = "Can view approval")]
        Approval_Get = 0x77,
        [Display(ShortName = "ApprovalManagement", GroupName = "Approval", Name = "Post", Description = "Can create approval")]
        Approval_Post = 0x78,
        [Display(ShortName = "ApprovalManagement", GroupName = "Approval", Name = "Put", Description = "Can update approval")]
        Approval_Put = 0x79,
        [Display(ShortName = "ApprovalManagement", GroupName = "Approval", Name = "Delete", Description = "Can delete approval")]
        Approval_Delete = 0x80,
        [Display(ShortName = "Setups", GroupName = "ApprovalLimit", Name = "Get", Description = "Can view approval limit")]
        ApprovalLimit_Get = 0x81,
        [Display(ShortName = "Setups", GroupName = "ApprovalLimit", Name = "Post", Description = "Can create approval limit")]
        ApprovalLimit_Post = 0x82,
        [Display(ShortName = "Setups", GroupName = "ApprovalLimit", Name = "Put", Description = "Can update approval limit")]
        ApprovalLimit_Put = 0x83,
        [Display(ShortName = "Setups", GroupName = "ApprovalLimit", Name = "Delete", Description = "Can delete approval limit")]
        ApprovalLimit_Delete = 0x84,
        [Display(ShortName = "Setups", GroupName = "ApproverLevel", Name = "Get", Description = "Can view approver level")]
        ApproverLevel_Get = 0x85,
        [Display(ShortName = "Setups", GroupName = "ApproverLevel", Name = "Post", Description = "Can create approver level")]
        ApproverLevel_Post = 0x86,
        [Display(ShortName = "Setups", GroupName = "ApproverLevel", Name = "Put", Description = "Can update approver level")]
        ApproverLevel_Put = 0x87,
        [Display(ShortName = "Setups", GroupName = "ApproverLevel", Name = "Delete", Description = "Can delete approver level")]
        ApproverLevel_Delete = 0x88,
        [Display(ShortName = "SecuredMobility", GroupName = "ArmedEscort", Name = "Get", Description = "Can view armed escort")]
        ArmedEscort_Get = 0x89,
        [Display(ShortName = "SecuredMobility", GroupName = "ArmedEscort", Name = "Post", Description = "Can create armed escort")]
        ArmedEscort_Post = 0x90,
        [Display(ShortName = "SecuredMobility", GroupName = "ArmedEscort", Name = "Put", Description = "Can update armed escort")]
        ArmedEscort_Put = 0x91,
        [Display(ShortName = "SecuredMobility", GroupName = "ArmedEscort", Name = "Delete", Description = "Can delete armed escort")]
        ArmedEscort_Delete = 0x92,
        [Display(ShortName = "SecuredMobility", GroupName = "ArmedEscortRegistration", Name = "Get", Description = "Can view armed escort registration")]
        ArmedEscortRegistration_Get = 0x93,
        [Display(ShortName = "SecuredMobility", GroupName = "ArmedEscortRegistration", Name = "Post", Description = "Can create armed escort registration")]
        ArmedEscortRegistration_Post = 0x94,
        [Display(ShortName = "SecuredMobility", GroupName = "ArmedEscortRegistration", Name = "Put", Description = "Can update armed escort registration")]
        ArmedEscortRegistration_Put = 0x95,
        [Display(ShortName = "SecuredMobility", GroupName = "ArmedEscortRegistration", Name = "Delete", Description = "Can delete armed escort registration")]
        ArmedEscortRegistration_Delete = 0x96,
        [Display(ShortName = "Setups", GroupName = "Auth", Name = "Get", Description = "Can view auth")]
        Auth_Get = 0x97,
        [Display(ShortName = "Setups", GroupName = "Auth", Name = "Post", Description = "Can create auth")]
        Auth_Post = 0x98,
        [Display(ShortName = "Setups", GroupName = "Auth", Name = "Put", Description = "Can update auth")]
        Auth_Put = 0x99,
        [Display(ShortName = "Setups", GroupName = "Auth", Name = "Delete", Description = "Can delete auth")]
        Auth_Delete = 0x100,
        [Display(ShortName = "Setups", GroupName = "Bank", Name = "Get", Description = "Can view bank")]
        Bank_Get = 0x101,
        [Display(ShortName = "Setups", GroupName = "Bank", Name = "Post", Description = "Can create bank")]
        Bank_Post = 0x102,
        [Display(ShortName = "Setups", GroupName = "Bank", Name = "Put", Description = "Can update bank")]
        Bank_Put = 0x103,
        [Display(ShortName = "Setups", GroupName = "Bank", Name = "Delete", Description = "Can delete bank")]
        Bank_Delete = 0x104,
        [Display(ShortName = "Setups", GroupName = "Branch", Name = "Get", Description = "Can view branch")]
        Branch_Get = 0x105,
        [Display(ShortName = "Setups", GroupName = "Branch", Name = "Post", Description = "Can create branch")]
        Branch_Post = 0x106,
        [Display(ShortName = "Setups", GroupName = "Branch", Name = "Put", Description = "Can update branch")]
        Branch_Put = 0x107,
        [Display(ShortName = "Setups", GroupName = "Branch", Name = "Delete", Description = "Can delete branch")]
        Branch_Delete = 0x108,
        [Display(ShortName = "Setups", GroupName = "BusinessRules", Name = "Get", Description = "Can view business rules")]
        BusinessRules_Get = 0x109,
        [Display(ShortName = "Setups", GroupName = "BusinessRules", Name = "Post", Description = "Can create business rules")]
        BusinessRules_Post = 0x110,
        [Display(ShortName = "Setups", GroupName = "BusinessRules", Name = "Put", Description = "Can update business rules")]
        BusinessRules_Put = 0x111,
        [Display(ShortName = "Setups", GroupName = "BusinessRules", Name = "Delete", Description = "Can delete business rules")]
        BusinessRules_Delete = 0x112,
        [Display(ShortName = "ClientManagement", GroupName = "ClientContactQualification", Name = "Get", Description = "Can view client contact qualification")]
        ClientContactQualification_Get = 0x113,
        [Display(ShortName = "ClientManagement", GroupName = "ClientContactQualification", Name = "Post", Description = "Can create client contact qualification")]
        ClientContactQualification_Post = 0x114,
        [Display(ShortName = "ClientManagement", GroupName = "ClientContactQualification", Name = "Put", Description = "Can update client contact qualification")]
        ClientContactQualification_Put = 0x115,
        [Display(ShortName = "ClientManagement", GroupName = "ClientContactQualification", Name = "Delete", Description = "Can delete client contact qualification")]
        ClientContactQualification_Delete = 0x116,
        [Display(ShortName = "ClientManagement", GroupName = "ClientPolicy", Name = "Get", Description = "Can view client policy")]
        ClientPolicy_Get = 0x117,
        [Display(ShortName = "ClientManagement", GroupName = "ClientPolicy", Name = "Post", Description = "Can create client policy")]
        ClientPolicy_Post = 0x118,
        [Display(ShortName = "ClientManagement", GroupName = "ClientPolicy", Name = "Put", Description = "Can update client policy")]
        ClientPolicy_Put = 0x119,
        [Display(ShortName = "ClientManagement", GroupName = "ClientPolicy", Name = "Delete", Description = "Can delete client policy")]
        ClientPolicy_Delete = 0x120,
        [Display(ShortName = "Setups", GroupName = "Commander", Name = "Get", Description = "Can view commander")]
        Commander_Get = 0x121,
        [Display(ShortName = "Setups", GroupName = "Commander", Name = "Post", Description = "Can create commander")]
        Commander_Post = 0x122,
        [Display(ShortName = "Setups", GroupName = "Commander", Name = "Put", Description = "Can update commander")]
        Commander_Put = 0x123,
        [Display(ShortName = "Setups", GroupName = "Commander", Name = "Delete", Description = "Can delete commander")]
        Commander_Delete = 0x124,
        [Display(ShortName = "Setups", GroupName = "CommanderRegistration", Name = "Get", Description = "Can view commander registration")]
        CommanderRegistration_Get = 0x125,
        [Display(ShortName = "Setups", GroupName = "CommanderRegistration", Name = "Post", Description = "Can create commander registration")]
        CommanderRegistration_Post = 0x126,
        [Display(ShortName = "Setups", GroupName = "CommanderRegistration", Name = "Put", Description = "Can update commander registration")]
        CommanderRegistration_Put = 0x127,
        [Display(ShortName = "Setups", GroupName = "CommanderRegistration", Name = "Delete", Description = "Can delete commander registration")]
        CommanderRegistration_Delete = 0x128,
        [Display(ShortName = "Setups", GroupName = "Company", Name = "Get", Description = "Can view company")]
        Company_Get = 0x129,
        [Display(ShortName = "Setups", GroupName = "Company", Name = "Post", Description = "Can create company")]
        Company_Post = 0x130,
        [Display(ShortName = "Setups", GroupName = "Company", Name = "Put", Description = "Can update company")]
        Company_Put = 0x131,
        [Display(ShortName = "Setups", GroupName = "Company", Name = "Delete", Description = "Can delete company")]
        Company_Delete = 0x132,
        [Display(ShortName = "ComplaintManagement", GroupName = "Complaint", Name = "Get", Description = "Can view complaint")]
        Complaint_Get = 0x133,
        [Display(ShortName = "ComplaintManagement", GroupName = "Complaint", Name = "Post", Description = "Can create complaint")]
        Complaint_Post = 0x134,
        [Display(ShortName = "ComplaintManagement", GroupName = "Complaint", Name = "Put", Description = "Can update complaint")]
        Complaint_Put = 0x135,
        [Display(ShortName = "ComplaintManagement", GroupName = "Complaint", Name = "Delete", Description = "Can delete complaint")]
        Complaint_Delete = 0x136,
        [Display(ShortName = "ComplaintManagement", GroupName = "ComplaintHandling", Name = "Get", Description = "Can view complaint handling")]
        ComplaintHandling_Get = 0x137,
        [Display(ShortName = "ComplaintManagement", GroupName = "ComplaintHandling", Name = "Post", Description = "Can create complaint handling")]
        ComplaintHandling_Post = 0x138,
        [Display(ShortName = "ComplaintManagement", GroupName = "ComplaintHandling", Name = "Put", Description = "Can update complaint handling")]
        ComplaintHandling_Put = 0x139,
        [Display(ShortName = "ComplaintManagement", GroupName = "ComplaintHandling", Name = "Delete", Description = "Can delete complaint handling")]
        ComplaintHandling_Delete = 0x140,
        [Display(ShortName = "Setups", GroupName = "ComplaintOrigin", Name = "Get", Description = "Can view complaint origin")]
        ComplaintOrigin_Get = 0x141,
        [Display(ShortName = "Setups", GroupName = "ComplaintOrigin", Name = "Post", Description = "Can create complaint origin")]
        ComplaintOrigin_Post = 0x142,
        [Display(ShortName = "Setups", GroupName = "ComplaintOrigin", Name = "Put", Description = "Can update complaint origin")]
        ComplaintOrigin_Put = 0x143,
        [Display(ShortName = "Setups", GroupName = "ComplaintOrigin", Name = "Delete", Description = "Can delete complaint origin")]
        ComplaintOrigin_Delete = 0x144,
        [Display(ShortName = "Setups", GroupName = "ComplaintSource", Name = "Get", Description = "Can view complaint source")]
        ComplaintSource_Get = 0x145,
        [Display(ShortName = "Setups", GroupName = "ComplaintSource", Name = "Post", Description = "Can create complaint source")]
        ComplaintSource_Post = 0x146,
        [Display(ShortName = "Setups", GroupName = "ComplaintSource", Name = "Put", Description = "Can update complaint source")]
        ComplaintSource_Put = 0x147,
        [Display(ShortName = "Setups", GroupName = "ComplaintSource", Name = "Delete", Description = "Can delete complaint source")]
        ComplaintSource_Delete = 0x148,
        [Display(ShortName = "Setups", GroupName = "ComplaintType", Name = "Get", Description = "Can view complaint type")]
        ComplaintType_Get = 0x149,
        [Display(ShortName = "Setups", GroupName = "ComplaintType", Name = "Post", Description = "Can create complaint type")]
        ComplaintType_Post = 0x150,
        [Display(ShortName = "Setups", GroupName = "ComplaintType", Name = "Put", Description = "Can update complaint type")]
        ComplaintType_Put = 0x151,
        [Display(ShortName = "Setups", GroupName = "ComplaintType", Name = "Delete", Description = "Can delete complaint type")]
        ComplaintType_Delete = 0x152,
        [Display(ShortName = "LeadAdministration", GroupName = "Contact", Name = "Get", Description = "Can view contact")]
        Contact_Get = 0x153,
        [Display(ShortName = "LeadAdministration", GroupName = "Contact", Name = "Post", Description = "Can create contact")]
        Contact_Post = 0x154,
        [Display(ShortName = "LeadAdministration", GroupName = "Contact", Name = "Put", Description = "Can update contact")]
        Contact_Put = 0x155,
        [Display(ShortName = "LeadAdministration", GroupName = "Contact", Name = "Delete", Description = "Can delete contact")]
        Contact_Delete = 0x156,
        [Display(ShortName = "CronJobs", GroupName = "CronJobs", Name = "Get", Description = "Can view cron jobs")]
        CronJobs_Get = 0x157,
        [Display(ShortName = "CronJobs", GroupName = "CronJobs", Name = "Post", Description = "Can create cron jobs")]
        CronJobs_Post = 0x158,
        [Display(ShortName = "CronJobs", GroupName = "CronJobs", Name = "Put", Description = "Can update cron jobs")]
        CronJobs_Put = 0x159,
        [Display(ShortName = "CronJobs", GroupName = "CronJobs", Name = "Delete", Description = "Can delete cron jobs")]
        CronJobs_Delete = 0x160,
        [Display(ShortName = "Setups", GroupName = "Designation", Name = "Get", Description = "Can view designation")]
        Designation_Get = 0x161,
        [Display(ShortName = "Setups", GroupName = "Designation", Name = "Post", Description = "Can create designation")]
        Designation_Post = 0x162,
        [Display(ShortName = "Setups", GroupName = "Designation", Name = "Put", Description = "Can update designation")]
        Designation_Put = 0x163,
        [Display(ShortName = "Setups", GroupName = "Designation", Name = "Delete", Description = "Can delete designation")]
        Designation_Delete = 0x164,
        [Display(ShortName = "Setups", GroupName = "Division", Name = "Get", Description = "Can view division")]
        Division_Get = 0x165,
        [Display(ShortName = "Setups", GroupName = "Division", Name = "Post", Description = "Can create division")]
        Division_Post = 0x166,
        [Display(ShortName = "Setups", GroupName = "Division", Name = "Put", Description = "Can update division")]
        Division_Put = 0x167,
        [Display(ShortName = "Setups", GroupName = "Division", Name = "Delete", Description = "Can delete division")]
        Division_Delete = 0x168,
        [Display(ShortName = "Setups", GroupName = "DTSDetailGenericDays", Name = "Get", Description = "Can view dts detail generic days")]
        DTSDetailGenericDays_Get = 0x169,
        [Display(ShortName = "Setups", GroupName = "DTSDetailGenericDays", Name = "Post", Description = "Can create dts detail generic days")]
        DTSDetailGenericDays_Post = 0x170,
        [Display(ShortName = "Setups", GroupName = "DTSDetailGenericDays", Name = "Put", Description = "Can update dts detail generic days")]
        DTSDetailGenericDays_Put = 0x171,
        [Display(ShortName = "Setups", GroupName = "DTSDetailGenericDays", Name = "Delete", Description = "Can delete dts detail generic days")]
        DTSDetailGenericDays_Delete = 0x172,
        [Display(ShortName = "Setups", GroupName = "DTSMasters", Name = "Get", Description = "Can view dts masters")]
        DTSMasters_Get = 0x173,
        [Display(ShortName = "Setups", GroupName = "DTSMasters", Name = "Post", Description = "Can create dts masters")]
        DTSMasters_Post = 0x174,
        [Display(ShortName = "Setups", GroupName = "DTSMasters", Name = "Put", Description = "Can update dts masters")]
        DTSMasters_Put = 0x175,
        [Display(ShortName = "Setups", GroupName = "DTSMasters", Name = "Delete", Description = "Can delete dts masters")]
        DTSMasters_Delete = 0x176,
        [Display(ShortName = "Setups", GroupName = "EngagementReason", Name = "Get", Description = "Can view engagement reason")]
        EngagementReason_Get = 0x177,
        [Display(ShortName = "Setups", GroupName = "EngagementReason", Name = "Post", Description = "Can create engagement reason")]
        EngagementReason_Post = 0x178,
        [Display(ShortName = "Setups", GroupName = "EngagementReason", Name = "Put", Description = "Can update engagement reason")]
        EngagementReason_Put = 0x179,
        [Display(ShortName = "Setups", GroupName = "EngagementReason", Name = "Delete", Description = "Can delete engagement reason")]
        EngagementReason_Delete = 0x180,
        [Display(ShortName = "LeadAdministration", GroupName = "EngagementType", Name = "Get", Description = "Can view engagement type")]
        EngagementType_Get = 0x181,
        [Display(ShortName = "LeadAdministration", GroupName = "EngagementType", Name = "Post", Description = "Can create engagement type")]
        EngagementType_Post = 0x182,
        [Display(ShortName = "LeadAdministration", GroupName = "EngagementType", Name = "Put", Description = "Can update engagement type")]
        EngagementType_Put = 0x183,
        [Display(ShortName = "LeadAdministration", GroupName = "EngagementType", Name = "Delete", Description = "Can delete engagement type")]
        EngagementType_Delete = 0x184,
        [Display(ShortName = "Setups", GroupName = "EscalationLevel", Name = "Get", Description = "Can view escalation level")]
        EscalationLevel_Get = 0x185,
        [Display(ShortName = "Setups", GroupName = "EscalationLevel", Name = "Post", Description = "Can create escalation level")]
        EscalationLevel_Post = 0x186,
        [Display(ShortName = "Setups", GroupName = "EscalationLevel", Name = "Put", Description = "Can update escalation level")]
        EscalationLevel_Put = 0x187,
        [Display(ShortName = "Setups", GroupName = "EscalationLevel", Name = "Delete", Description = "Can delete escalation level")]
        EscalationLevel_Delete = 0x188,
        [Display(ShortName = "Setups", GroupName = "EscalationMatrix", Name = "Get", Description = "Can view escalation matrix")]
        EscalationMatrix_Get = 0x189,
        [Display(ShortName = "Setups", GroupName = "EscalationMatrix", Name = "Post", Description = "Can create escalation matrix")]
        EscalationMatrix_Post = 0x190,
        [Display(ShortName = "Setups", GroupName = "EscalationMatrix", Name = "Put", Description = "Can update escalation matrix")]
        EscalationMatrix_Put = 0x191,
        [Display(ShortName = "Setups", GroupName = "EscalationMatrix", Name = "Delete", Description = "Can delete escalation matrix")]
        EscalationMatrix_Delete = 0x192,
        [Display(ShortName = "Setups", GroupName = "Evidence", Name = "Get", Description = "Can view evidence")]
        Evidence_Get = 0x193,
        [Display(ShortName = "Setups", GroupName = "Evidence", Name = "Post", Description = "Can create evidence")]
        Evidence_Post = 0x194,
        [Display(ShortName = "Setups", GroupName = "Evidence", Name = "Put", Description = "Can update evidence")]
        Evidence_Put = 0x195,
        [Display(ShortName = "Setups", GroupName = "Evidence", Name = "Delete", Description = "Can delete evidence")]
        Evidence_Delete = 0x196,
        [Display(ShortName = "LeadAdministration", GroupName = "GenerateGroupInvoiceNumber", Name = "Get", Description = "Can view generate group invoice number")]
        GenerateGroupInvoiceNumber_Get = 0x197,
        [Display(ShortName = "LeadAdministration", GroupName = "GenerateGroupInvoiceNumber", Name = "Post", Description = "Can create generate group invoice number")]
        GenerateGroupInvoiceNumber_Post = 0x198,
        [Display(ShortName = "LeadAdministration", GroupName = "GenerateGroupInvoiceNumber", Name = "Put", Description = "Can update generate group invoice number")]
        GenerateGroupInvoiceNumber_Put = 0x199,
        [Display(ShortName = "LeadAdministration", GroupName = "GenerateGroupInvoiceNumber", Name = "Delete", Description = "Can delete generate group invoice number")]
        GenerateGroupInvoiceNumber_Delete = 0x200,
        [Display(ShortName = "Setups", GroupName = "GroupType", Name = "Get", Description = "Can view group type")]
        GroupType_Get = 0x201,
        [Display(ShortName = "Setups", GroupName = "GroupType", Name = "Post", Description = "Can create group type")]
        GroupType_Post = 0x202,
        [Display(ShortName = "Setups", GroupName = "GroupType", Name = "Put", Description = "Can update group type")]
        GroupType_Put = 0x203,
        [Display(ShortName = "Setups", GroupName = "GroupType", Name = "Delete", Description = "Can delete group type")]
        GroupType_Delete = 0x204,
        [Display(ShortName = "Setups", GroupName = "Industry", Name = "Get", Description = "Can view industry")]
        Industry_Get = 0x205,
        [Display(ShortName = "Setups", GroupName = "Industry", Name = "Post", Description = "Can create industry")]
        Industry_Post = 0x206,
        [Display(ShortName = "Setups", GroupName = "Industry", Name = "Put", Description = "Can update industry")]
        Industry_Put = 0x207,
        [Display(ShortName = "Setups", GroupName = "Industry", Name = "Delete", Description = "Can delete industry")]
        Industry_Delete = 0x208,
        [Display(ShortName = "Setups", GroupName = "JourneyStartandStop", Name = "Get", Description = "Can view journey startand stop")]
        JourneyStartandStop_Get = 0x209,
        [Display(ShortName = "Setups", GroupName = "JourneyStartandStop", Name = "Post", Description = "Can create journey startand stop")]
        JourneyStartandStop_Post = 0x210,
        [Display(ShortName = "Setups", GroupName = "JourneyStartandStop", Name = "Put", Description = "Can update journey startand stop")]
        JourneyStartandStop_Put = 0x211,
        [Display(ShortName = "Setups", GroupName = "JourneyStartandStop", Name = "Delete", Description = "Can delete journey startand stop")]
        JourneyStartandStop_Delete = 0x212,
        [Display(ShortName = "Supplier", GroupName = "Make", Name = "Get", Description = "Can view make")]
        Make_Get = 0x213,
        [Display(ShortName = "Supplier", GroupName = "Make", Name = "Post", Description = "Can create make")]
        Make_Post = 0x214,
        [Display(ShortName = "Supplier", GroupName = "Make", Name = "Put", Description = "Can update make")]
        Make_Put = 0x215,
        [Display(ShortName = "Supplier", GroupName = "Make", Name = "Delete", Description = "Can delete make")]
        Make_Delete = 0x216,
        [Display(ShortName = "Setups", GroupName = "MasterServiceAssignment", Name = "Get", Description = "Can view master service assignment")]
        MasterServiceAssignment_Get = 0x217,
        [Display(ShortName = "Setups", GroupName = "MasterServiceAssignment", Name = "Post", Description = "Can create master service assignment")]
        MasterServiceAssignment_Post = 0x218,
        [Display(ShortName = "Setups", GroupName = "MasterServiceAssignment", Name = "Put", Description = "Can update master service assignment")]
        MasterServiceAssignment_Put = 0x219,
        [Display(ShortName = "Setups", GroupName = "MasterServiceAssignment", Name = "Delete", Description = "Can delete master service assignment")]
        MasterServiceAssignment_Delete = 0x220,
        [Display(ShortName = "Setups", GroupName = "MeansOfIdentification", Name = "Get", Description = "Can view means of identification")]
        MeansOfIdentification_Get = 0x221,
        [Display(ShortName = "Setups", GroupName = "MeansOfIdentification", Name = "Post", Description = "Can create means of identification")]
        MeansOfIdentification_Post = 0x222,
        [Display(ShortName = "Setups", GroupName = "MeansOfIdentification", Name = "Put", Description = "Can update means of identification")]
        MeansOfIdentification_Put = 0x223,
        [Display(ShortName = "Setups", GroupName = "MeansOfIdentification", Name = "Delete", Description = "Can delete means of identification")]
        MeansOfIdentification_Delete = 0x224,
        [Display(ShortName = "Supplier", GroupName = "Model", Name = "Get", Description = "Can view model")]
        Model_Get = 0x225,
        [Display(ShortName = "Supplier", GroupName = "Model", Name = "Post", Description = "Can create model")]
        Model_Post = 0x226,
        [Display(ShortName = "Supplier", GroupName = "Model", Name = "Put", Description = "Can update model")]
        Model_Put = 0x227,
        [Display(ShortName = "Supplier", GroupName = "Model", Name = "Delete", Description = "Can delete model")]
        Model_Delete = 0x228,
        [Display(ShortName = "Setups", GroupName = "ModeOfTransport", Name = "Get", Description = "Can view mode of transport")]
        ModeOfTransport_Get = 0x229,
        [Display(ShortName = "Setups", GroupName = "ModeOfTransport", Name = "Post", Description = "Can create mode of transport")]
        ModeOfTransport_Post = 0x230,
        [Display(ShortName = "Setups", GroupName = "ModeOfTransport", Name = "Put", Description = "Can update mode of transport")]
        ModeOfTransport_Put = 0x231,
        [Display(ShortName = "Setups", GroupName = "ModeOfTransport", Name = "Delete", Description = "Can delete mode of transport")]
        ModeOfTransport_Delete = 0x232,
        [Display(ShortName = "Setups", GroupName = "Note", Name = "Get", Description = "Can view note")]
        Note_Get = 0x233,
        [Display(ShortName = "Setups", GroupName = "Note", Name = "Post", Description = "Can create note")]
        Note_Post = 0x234,
        [Display(ShortName = "Setups", GroupName = "Note", Name = "Put", Description = "Can update note")]
        Note_Put = 0x235,
        [Display(ShortName = "Setups", GroupName = "Note", Name = "Delete", Description = "Can delete note")]
        Note_Delete = 0x236,
        [Display(ShortName = "Profile", GroupName = "Office", Name = "Get", Description = "Can view office")]
        Office_Get = 0x237,
        [Display(ShortName = "Profile", GroupName = "Office", Name = "Post", Description = "Can create office")]
        Office_Post = 0x238,
        [Display(ShortName = "Profile", GroupName = "Office", Name = "Put", Description = "Can update office")]
        Office_Put = 0x239,
        [Display(ShortName = "Profile", GroupName = "Office", Name = "Delete", Description = "Can delete office")]
        Office_Delete = 0x240,
        [Display(ShortName = "Setups", GroupName = "OperatingEntity", Name = "Get", Description = "Can view operating entity")]
        OperatingEntity_Get = 0x241,
        [Display(ShortName = "Setups", GroupName = "OperatingEntity", Name = "Post", Description = "Can create operating entity")]
        OperatingEntity_Post = 0x242,
        [Display(ShortName = "Setups", GroupName = "OperatingEntity", Name = "Put", Description = "Can update operating entity")]
        OperatingEntity_Put = 0x243,
        [Display(ShortName = "Setups", GroupName = "OperatingEntity", Name = "Delete", Description = "Can delete operating entity")]
        OperatingEntity_Delete = 0x244,
        [Display(ShortName = "SecuredMobility", GroupName = "Pilot", Name = "Get", Description = "Can view pilot")]
        Pilot_Get = 0x245,
        [Display(ShortName = "SecuredMobility", GroupName = "Pilot", Name = "Post", Description = "Can create pilot")]
        Pilot_Post = 0x246,
        [Display(ShortName = "SecuredMobility", GroupName = "Pilot", Name = "Put", Description = "Can update pilot")]
        Pilot_Put = 0x247,
        [Display(ShortName = "SecuredMobility", GroupName = "Pilot", Name = "Delete", Description = "Can delete pilot")]
        Pilot_Delete = 0x248,
        [Display(ShortName = "SecuredMobility", GroupName = "PilotRegistration", Name = "Get", Description = "Can view pilot registration")]
        PilotRegistration_Get = 0x249,
        [Display(ShortName = "SecuredMobility", GroupName = "PilotRegistration", Name = "Post", Description = "Can create pilot registration")]
        PilotRegistration_Post = 0x250,
        [Display(ShortName = "SecuredMobility", GroupName = "PilotRegistration", Name = "Put", Description = "Can update pilot registration")]
        PilotRegistration_Put = 0x251,
        [Display(ShortName = "SecuredMobility", GroupName = "PilotRegistration", Name = "Delete", Description = "Can delete pilot registration")]
        PilotRegistration_Delete = 0x252,
        [Display(ShortName = "Setups", GroupName = "PriceRegister", Name = "Get", Description = "Can view price register")]
        PriceRegister_Get = 0x253,
        [Display(ShortName = "Setups", GroupName = "PriceRegister", Name = "Post", Description = "Can create price register")]
        PriceRegister_Post = 0x254,
        [Display(ShortName = "Setups", GroupName = "PriceRegister", Name = "Put", Description = "Can update price register")]
        PriceRegister_Put = 0x255,
        [Display(ShortName = "Setups", GroupName = "PriceRegister", Name = "Delete", Description = "Can delete price register")]
        PriceRegister_Delete = 0x256,
        [Display(ShortName = "Setups", GroupName = "ProcessesRequiringApproval", Name = "Get", Description = "Can view processes requiring approval")]
        ProcessesRequiringApproval_Get = 0x257,
        [Display(ShortName = "Setups", GroupName = "ProcessesRequiringApproval", Name = "Post", Description = "Can create processes requiring approval")]
        ProcessesRequiringApproval_Post = 0x258,
        [Display(ShortName = "Setups", GroupName = "ProcessesRequiringApproval", Name = "Put", Description = "Can update processes requiring approval")]
        ProcessesRequiringApproval_Put = 0x259,
        [Display(ShortName = "Setups", GroupName = "ProcessesRequiringApproval", Name = "Delete", Description = "Can delete processes requiring approval")]
        ProcessesRequiringApproval_Delete = 0x260,
        [Display(ShortName = "Setups", GroupName = "ProfileEscalationLevel", Name = "Get", Description = "Can view profile escalation level")]
        ProfileEscalationLevel_Get = 0x261,
        [Display(ShortName = "Setups", GroupName = "ProfileEscalationLevel", Name = "Post", Description = "Can create profile escalation level")]
        ProfileEscalationLevel_Post = 0x262,
        [Display(ShortName = "Setups", GroupName = "ProfileEscalationLevel", Name = "Put", Description = "Can update profile escalation level")]
        ProfileEscalationLevel_Put = 0x263,
        [Display(ShortName = "Setups", GroupName = "ProfileEscalationLevel", Name = "Delete", Description = "Can delete profile escalation level")]
        ProfileEscalationLevel_Delete = 0x264,
        [Display(ShortName = "ProjectManagment", GroupName = "ProjectManagement", Name = "Get", Description = "Can view project management")]
        ProjectManagement_Get = 0x265,
        [Display(ShortName = "ProjectManagment", GroupName = "ProjectManagement", Name = "Post", Description = "Can create project management")]
        ProjectManagement_Post = 0x266,
        [Display(ShortName = "ProjectManagment", GroupName = "ProjectManagement", Name = "Put", Description = "Can update project management")]
        ProjectManagement_Put = 0x267,
        [Display(ShortName = "ProjectManagment", GroupName = "ProjectManagement", Name = "Delete", Description = "Can delete project management")]
        ProjectManagement_Delete = 0x268,
        [Display(ShortName = "LeadAdministration", GroupName = "Prospect", Name = "Get", Description = "Can view prospect")]
        Prospect_Get = 0x269,
        [Display(ShortName = "LeadAdministration", GroupName = "Prospect", Name = "Post", Description = "Can create prospect")]
        Prospect_Post = 0x270,
        [Display(ShortName = "LeadAdministration", GroupName = "Prospect", Name = "Put", Description = "Can update prospect")]
        Prospect_Put = 0x271,
        [Display(ShortName = "LeadAdministration", GroupName = "Prospect", Name = "Delete", Description = "Can delete prospect")]
        Prospect_Delete = 0x272,
        [Display(ShortName = "Setups", GroupName = "Region", Name = "Get", Description = "Can view region")]
        Region_Get = 0x273,
        [Display(ShortName = "Setups", GroupName = "Region", Name = "Post", Description = "Can create region")]
        Region_Post = 0x274,
        [Display(ShortName = "Setups", GroupName = "Region", Name = "Put", Description = "Can update region")]
        Region_Put = 0x275,
        [Display(ShortName = "Setups", GroupName = "Region", Name = "Delete", Description = "Can delete region")]
        Region_Delete = 0x276,
        [Display(ShortName = "Setups", GroupName = "Relationship", Name = "Get", Description = "Can view relationship")]
        Relationship_Get = 0x277,
        [Display(ShortName = "Setups", GroupName = "Relationship", Name = "Post", Description = "Can create relationship")]
        Relationship_Post = 0x278,
        [Display(ShortName = "Setups", GroupName = "Relationship", Name = "Put", Description = "Can update relationship")]
        Relationship_Put = 0x279,
        [Display(ShortName = "Setups", GroupName = "Relationship", Name = "Delete", Description = "Can delete relationship")]
        Relationship_Delete = 0x280,
        [Display(ShortName = "Setups", GroupName = "RequiredServiceDocument", Name = "Get", Description = "Can view required service document")]
        RequiredServiceDocument_Get = 0x281,
        [Display(ShortName = "Setups", GroupName = "RequiredServiceDocument", Name = "Post", Description = "Can create required service document")]
        RequiredServiceDocument_Post = 0x282,
        [Display(ShortName = "Setups", GroupName = "RequiredServiceDocument", Name = "Put", Description = "Can update required service document")]
        RequiredServiceDocument_Put = 0x283,
        [Display(ShortName = "Setups", GroupName = "RequiredServiceDocument", Name = "Delete", Description = "Can delete required service document")]
        RequiredServiceDocument_Delete = 0x284,
        [Display(ShortName = "Setups", GroupName = "RequredServiceQualificationElement", Name = "Get", Description = "Can view requred service qualification element")]
        RequredServiceQualificationElement_Get = 0x285,
        [Display(ShortName = "Setups", GroupName = "RequredServiceQualificationElement", Name = "Post", Description = "Can create requred service qualification element")]
        RequredServiceQualificationElement_Post = 0x286,
        [Display(ShortName = "Setups", GroupName = "RequredServiceQualificationElement", Name = "Put", Description = "Can update requred service qualification element")]
        RequredServiceQualificationElement_Put = 0x287,
        [Display(ShortName = "Setups", GroupName = "RequredServiceQualificationElement", Name = "Delete", Description = "Can delete requred service qualification element")]
        RequredServiceQualificationElement_Delete = 0x288,
        [Display(ShortName = "LeadAdministration", GroupName = "Sbuproportion", Name = "Get", Description = "Can view sbuproportion")]
        Sbuproportion_Get = 0x289,
        [Display(ShortName = "LeadAdministration", GroupName = "Sbuproportion", Name = "Post", Description = "Can create sbuproportion")]
        Sbuproportion_Post = 0x290,
        [Display(ShortName = "LeadAdministration", GroupName = "Sbuproportion", Name = "Put", Description = "Can update sbuproportion")]
        Sbuproportion_Put = 0x291,
        [Display(ShortName = "LeadAdministration", GroupName = "Sbuproportion", Name = "Delete", Description = "Can delete sbuproportion")]
        Sbuproportion_Delete = 0x292,
        [Display(ShortName = "Setups", GroupName = "ServiceAssignmentDetails", Name = "Get", Description = "Can view service assignment details")]
        ServiceAssignmentDetails_Get = 0x293,
        [Display(ShortName = "Setups", GroupName = "ServiceAssignmentDetails", Name = "Post", Description = "Can create service assignment details")]
        ServiceAssignmentDetails_Post = 0x294,
        [Display(ShortName = "Setups", GroupName = "ServiceAssignmentDetails", Name = "Put", Description = "Can update service assignment details")]
        ServiceAssignmentDetails_Put = 0x295,
        [Display(ShortName = "Setups", GroupName = "ServiceAssignmentDetails", Name = "Delete", Description = "Can delete service assignment details")]
        ServiceAssignmentDetails_Delete = 0x296,
        [Display(ShortName = "Setups", GroupName = "ServiceCategory", Name = "Get", Description = "Can view service category")]
        ServiceCategory_Get = 0x297,
        [Display(ShortName = "Setups", GroupName = "ServiceCategory", Name = "Post", Description = "Can create service category")]
        ServiceCategory_Post = 0x298,
        [Display(ShortName = "Setups", GroupName = "ServiceCategory", Name = "Put", Description = "Can update service category")]
        ServiceCategory_Put = 0x299,
        [Display(ShortName = "Setups", GroupName = "ServiceCategory", Name = "Delete", Description = "Can delete service category")]
        ServiceCategory_Delete = 0x300,
        [Display(ShortName = "Setups", GroupName = "ServiceCategoryTask", Name = "Get", Description = "Can view service category task")]
        ServiceCategoryTask_Get = 0x301,
        [Display(ShortName = "Setups", GroupName = "ServiceCategoryTask", Name = "Post", Description = "Can create service category task")]
        ServiceCategoryTask_Post = 0x302,
        [Display(ShortName = "Setups", GroupName = "ServiceCategoryTask", Name = "Put", Description = "Can update service category task")]
        ServiceCategoryTask_Put = 0x303,
        [Display(ShortName = "Setups", GroupName = "ServiceCategoryTask", Name = "Delete", Description = "Can delete service category task")]
        ServiceCategoryTask_Delete = 0x304,
        [Display(ShortName = "Setups", GroupName = "ServiceCustomerMigrations", Name = "Get", Description = "Can view service customer migrations")]
        ServiceCustomerMigrations_Get = 0x305,
        [Display(ShortName = "Setups", GroupName = "ServiceCustomerMigrations", Name = "Post", Description = "Can create service customer migrations")]
        ServiceCustomerMigrations_Post = 0x306,
        [Display(ShortName = "Setups", GroupName = "ServiceCustomerMigrations", Name = "Put", Description = "Can update service customer migrations")]
        ServiceCustomerMigrations_Put = 0x307,
        [Display(ShortName = "Setups", GroupName = "ServiceCustomerMigrations", Name = "Delete", Description = "Can delete service customer migrations")]
        ServiceCustomerMigrations_Delete = 0x308,
        [Display(ShortName = "Setups", GroupName = "ServiceGroup", Name = "Get", Description = "Can view service group")]
        ServiceGroup_Get = 0x309,
        [Display(ShortName = "Setups", GroupName = "ServiceGroup", Name = "Post", Description = "Can create service group")]
        ServiceGroup_Post = 0x310,
        [Display(ShortName = "Setups", GroupName = "ServiceGroup", Name = "Put", Description = "Can update service group")]
        ServiceGroup_Put = 0x311,
        [Display(ShortName = "Setups", GroupName = "ServiceGroup", Name = "Delete", Description = "Can delete service group")]
        ServiceGroup_Delete = 0x312,
        [Display(ShortName = "Setups", GroupName = "ServicePricing", Name = "Get", Description = "Can view service pricing")]
        ServicePricing_Get = 0x313,
        [Display(ShortName = "Setups", GroupName = "ServicePricing", Name = "Post", Description = "Can create service pricing")]
        ServicePricing_Post = 0x314,
        [Display(ShortName = "Setups", GroupName = "ServicePricing", Name = "Put", Description = "Can update service pricing")]
        ServicePricing_Put = 0x315,
        [Display(ShortName = "Setups", GroupName = "ServicePricing", Name = "Delete", Description = "Can delete service pricing")]
        ServicePricing_Delete = 0x316,
        [Display(ShortName = "Setups", GroupName = "ServiceQualification", Name = "Get", Description = "Can view service qualification")]
        ServiceQualification_Get = 0x317,
        [Display(ShortName = "Setups", GroupName = "ServiceQualification", Name = "Post", Description = "Can create service qualification")]
        ServiceQualification_Post = 0x318,
        [Display(ShortName = "Setups", GroupName = "ServiceQualification", Name = "Put", Description = "Can update service qualification")]
        ServiceQualification_Put = 0x319,
        [Display(ShortName = "Setups", GroupName = "ServiceQualification", Name = "Delete", Description = "Can delete service qualification")]
        ServiceQualification_Delete = 0x320,
        [Display(ShortName = "Setups", GroupName = "ServiceRegistration", Name = "Get", Description = "Can view service registration")]
        ServiceRegistration_Get = 0x321,
        [Display(ShortName = "Setups", GroupName = "ServiceRegistration", Name = "Post", Description = "Can create service registration")]
        ServiceRegistration_Post = 0x322,
        [Display(ShortName = "Setups", GroupName = "ServiceRegistration", Name = "Put", Description = "Can update service registration")]
        ServiceRegistration_Put = 0x323,
        [Display(ShortName = "Setups", GroupName = "ServiceRegistration", Name = "Delete", Description = "Can delete service registration")]
        ServiceRegistration_Delete = 0x324,
        [Display(ShortName = "Setups", GroupName = "ServiceRelationships", Name = "Get", Description = "Can view service relationships")]
        ServiceRelationships_Get = 0x325,
        [Display(ShortName = "Setups", GroupName = "ServiceRelationships", Name = "Post", Description = "Can create service relationships")]
        ServiceRelationships_Post = 0x326,
        [Display(ShortName = "Setups", GroupName = "ServiceRelationships", Name = "Put", Description = "Can update service relationships")]
        ServiceRelationships_Put = 0x327,
        [Display(ShortName = "Setups", GroupName = "ServiceRelationships", Name = "Delete", Description = "Can delete service relationships")]
        ServiceRelationships_Delete = 0x328,
        [Display(ShortName = "Setups", GroupName = "Services", Name = "Get", Description = "Can view services")]
        Services_Get = 0x329,
        [Display(ShortName = "Setups", GroupName = "Services", Name = "Post", Description = "Can create services")]
        Services_Post = 0x330,
        [Display(ShortName = "Setups", GroupName = "Services", Name = "Put", Description = "Can update services")]
        Services_Put = 0x331,
        [Display(ShortName = "Setups", GroupName = "Services", Name = "Delete", Description = "Can delete services")]
        Services_Delete = 0x332,
        [Display(ShortName = "Setups", GroupName = "ServiceTaskDeliverable", Name = "Get", Description = "Can view service task deliverable")]
        ServiceTaskDeliverable_Get = 0x333,
        [Display(ShortName = "Setups", GroupName = "ServiceTaskDeliverable", Name = "Post", Description = "Can create service task deliverable")]
        ServiceTaskDeliverable_Post = 0x334,
        [Display(ShortName = "Setups", GroupName = "ServiceTaskDeliverable", Name = "Put", Description = "Can update service task deliverable")]
        ServiceTaskDeliverable_Put = 0x335,
        [Display(ShortName = "Setups", GroupName = "ServiceTaskDeliverable", Name = "Delete", Description = "Can delete service task deliverable")]
        ServiceTaskDeliverable_Delete = 0x336,
        [Display(ShortName = "Setups", GroupName = "ServiceType", Name = "Get", Description = "Can view service type")]
        ServiceType_Get = 0x337,
        [Display(ShortName = "Setups", GroupName = "ServiceType", Name = "Post", Description = "Can create service type")]
        ServiceType_Post = 0x338,
        [Display(ShortName = "Setups", GroupName = "ServiceType", Name = "Put", Description = "Can update service type")]
        ServiceType_Put = 0x339,
        [Display(ShortName = "Setups", GroupName = "ServiceType", Name = "Delete", Description = "Can delete service type")]
        ServiceType_Delete = 0x340,
        [Display(ShortName = "Setups", GroupName = "SMORouteAndRegion", Name = "Get", Description = "Can view smo route and region")]
        SMORouteAndRegion_Get = 0x341,
        [Display(ShortName = "Setups", GroupName = "SMORouteAndRegion", Name = "Post", Description = "Can create smo route and region")]
        SMORouteAndRegion_Post = 0x342,
        [Display(ShortName = "Setups", GroupName = "SMORouteAndRegion", Name = "Put", Description = "Can update smo route and region")]
        SMORouteAndRegion_Put = 0x343,
        [Display(ShortName = "Setups", GroupName = "SMORouteAndRegion", Name = "Delete", Description = "Can delete smo route and region")]
        SMORouteAndRegion_Delete = 0x344,
        [Display(ShortName = "Setups", GroupName = "StandardSlaforOperatingEntity", Name = "Get", Description = "Can view standard slafor operating entity")]
        StandardSlaforOperatingEntity_Get = 0x345,
        [Display(ShortName = "Setups", GroupName = "StandardSlaforOperatingEntity", Name = "Post", Description = "Can create standard slafor operating entity")]
        StandardSlaforOperatingEntity_Post = 0x346,
        [Display(ShortName = "Setups", GroupName = "StandardSlaforOperatingEntity", Name = "Put", Description = "Can update standard slafor operating entity")]
        StandardSlaforOperatingEntity_Put = 0x347,
        [Display(ShortName = "Setups", GroupName = "StandardSlaforOperatingEntity", Name = "Delete", Description = "Can delete standard slafor operating entity")]
        StandardSlaforOperatingEntity_Delete = 0x348,
        [Display(ShortName = "Setups", GroupName = "State", Name = "Get", Description = "Can view state")]
        State_Get = 0x349,
        [Display(ShortName = "Setups", GroupName = "State", Name = "Post", Description = "Can create state")]
        State_Post = 0x350,
        [Display(ShortName = "Setups", GroupName = "State", Name = "Put", Description = "Can update state")]
        State_Put = 0x351,
        [Display(ShortName = "Setups", GroupName = "State", Name = "Delete", Description = "Can delete state")]
        State_Delete = 0x352,
        [Display(ShortName = "Setups", GroupName = "StrategicBusinessUnit", Name = "Get", Description = "Can view strategic business unit")]
        StrategicBusinessUnit_Get = 0x353,
        [Display(ShortName = "Setups", GroupName = "StrategicBusinessUnit", Name = "Post", Description = "Can create strategic business unit")]
        StrategicBusinessUnit_Post = 0x354,
        [Display(ShortName = "Setups", GroupName = "StrategicBusinessUnit", Name = "Put", Description = "Can update strategic business unit")]
        StrategicBusinessUnit_Put = 0x355,
        [Display(ShortName = "Setups", GroupName = "StrategicBusinessUnit", Name = "Delete", Description = "Can delete strategic business unit")]
        StrategicBusinessUnit_Delete = 0x356,
        [Display(ShortName = "Supplier", GroupName = "SupplierCategory", Name = "Get", Description = "Can view supplier category")]
        SupplierCategory_Get = 0x357,
        [Display(ShortName = "Supplier", GroupName = "SupplierCategory", Name = "Post", Description = "Can create supplier category")]
        SupplierCategory_Post = 0x358,
        [Display(ShortName = "Supplier", GroupName = "SupplierCategory", Name = "Put", Description = "Can update supplier category")]
        SupplierCategory_Put = 0x359,
        [Display(ShortName = "Supplier", GroupName = "SupplierCategory", Name = "Delete", Description = "Can delete supplier category")]
        SupplierCategory_Delete = 0x360,
        [Display(ShortName = "Supplier", GroupName = "Supplier", Name = "Get", Description = "Can view supplier")]
        Supplier_Get = 0x361,
        [Display(ShortName = "Supplier", GroupName = "Supplier", Name = "Post", Description = "Can create supplier")]
        Supplier_Post = 0x362,
        [Display(ShortName = "Supplier", GroupName = "Supplier", Name = "Put", Description = "Can update supplier")]
        Supplier_Put = 0x363,
        [Display(ShortName = "Supplier", GroupName = "Supplier", Name = "Delete", Description = "Can delete supplier")]
        Supplier_Delete = 0x364,
        [Display(ShortName = "Supplier", GroupName = "SupplierService", Name = "Get", Description = "Can view supplier service")]
        SupplierService_Get = 0x365,
        [Display(ShortName = "Supplier", GroupName = "SupplierService", Name = "Post", Description = "Can create supplier service")]
        SupplierService_Post = 0x366,
        [Display(ShortName = "Supplier", GroupName = "SupplierService", Name = "Put", Description = "Can update supplier service")]
        SupplierService_Put = 0x367,
        [Display(ShortName = "Supplier", GroupName = "SupplierService", Name = "Delete", Description = "Can delete supplier service")]
        SupplierService_Delete = 0x368,
        [Display(ShortName = "LeadAdministration", GroupName = "Suspect", Name = "Get", Description = "Can view suspect")]
        Suspect_Get = 0x369,
        [Display(ShortName = "LeadAdministration", GroupName = "Suspect", Name = "Post", Description = "Can create suspect")]
        Suspect_Post = 0x370,
        [Display(ShortName = "LeadAdministration", GroupName = "Suspect", Name = "Put", Description = "Can update suspect")]
        Suspect_Put = 0x371,
        [Display(ShortName = "LeadAdministration", GroupName = "Suspect", Name = "Delete", Description = "Can delete suspect")]
        Suspect_Delete = 0x372,
        [Display(ShortName = "LeadAdministration", GroupName = "SuspectQualification", Name = "Get", Description = "Can view suspect qualification")]
        SuspectQualification_Get = 0x373,
        [Display(ShortName = "LeadAdministration", GroupName = "SuspectQualification", Name = "Post", Description = "Can create suspect qualification")]
        SuspectQualification_Post = 0x374,
        [Display(ShortName = "LeadAdministration", GroupName = "SuspectQualification", Name = "Put", Description = "Can update suspect qualification")]
        SuspectQualification_Put = 0x375,
        [Display(ShortName = "LeadAdministration", GroupName = "SuspectQualification", Name = "Delete", Description = "Can delete suspect qualification")]
        SuspectQualification_Delete = 0x376,
        [Display(ShortName = "Setups", GroupName = "Target", Name = "Get", Description = "Can view target")]
        Target_Get = 0x377,
        [Display(ShortName = "Setups", GroupName = "Target", Name = "Post", Description = "Can create target")]
        Target_Post = 0x378,
        [Display(ShortName = "Setups", GroupName = "Target", Name = "Put", Description = "Can update target")]
        Target_Put = 0x379,
        [Display(ShortName = "Setups", GroupName = "Target", Name = "Delete", Description = "Can delete target")]
        Target_Delete = 0x380,
        [Display(ShortName = "Setups", GroupName = "TypesForServiceAssignment", Name = "Get", Description = "Can view types for service assignment")]
        TypesForServiceAssignment_Get = 0x381,
        [Display(ShortName = "Setups", GroupName = "TypesForServiceAssignment", Name = "Post", Description = "Can create types for service assignment")]
        TypesForServiceAssignment_Post = 0x382,
        [Display(ShortName = "Setups", GroupName = "TypesForServiceAssignment", Name = "Put", Description = "Can update types for service assignment")]
        TypesForServiceAssignment_Put = 0x383,
        [Display(ShortName = "Setups", GroupName = "TypesForServiceAssignment", Name = "Delete", Description = "Can delete types for service assignment")]
        TypesForServiceAssignment_Delete = 0x384,
        [Display(ShortName = "Profile", GroupName = "User", Name = "Get", Description = "Can view user")]
        User_Get = 0x385,
        [Display(ShortName = "Profile", GroupName = "User", Name = "Post", Description = "Can create user")]
        User_Post = 0x386,
        [Display(ShortName = "Profile", GroupName = "User", Name = "Put", Description = "Can update user")]
        User_Put = 0x387,
        [Display(ShortName = "Profile", GroupName = "User", Name = "Delete", Description = "Can delete user")]
        User_Delete = 0x388,
        [Display(ShortName = "Setups", GroupName = "VehicleRegistration", Name = "Get", Description = "Can view vehicle registration")]
        VehicleRegistration_Get = 0x389,
        [Display(ShortName = "Setups", GroupName = "VehicleRegistration", Name = "Post", Description = "Can create vehicle registration")]
        VehicleRegistration_Post = 0x390,
        [Display(ShortName = "Setups", GroupName = "VehicleRegistration", Name = "Put", Description = "Can update vehicle registration")]
        VehicleRegistration_Put = 0x391,
        [Display(ShortName = "Setups", GroupName = "VehicleRegistration", Name = "Delete", Description = "Can delete vehicle registration")]
        VehicleRegistration_Delete = 0x392,
        [Display(ShortName = "Setups", GroupName = "Vehicles", Name = "Get", Description = "Can view vehicles")]
        Vehicles_Get = 0x393,
        [Display(ShortName = "Setups", GroupName = "Vehicles", Name = "Post", Description = "Can create vehicles")]
        Vehicles_Post = 0x394,
        [Display(ShortName = "Setups", GroupName = "Vehicles", Name = "Put", Description = "Can update vehicles")]
        Vehicles_Put = 0x395,
        [Display(ShortName = "Setups", GroupName = "Vehicles", Name = "Delete", Description = "Can delete vehicles")]
        Vehicles_Delete = 0x396,
        [Display(ShortName = "Setups", GroupName = "Zone", Name = "Get", Description = "Can view zone")]
        Zone_Get = 0x397,
        [Display(ShortName = "Setups", GroupName = "Zone", Name = "Post", Description = "Can create zone")]
        Zone_Post = 0x398,
        [Display(ShortName = "Setups", GroupName = "Zone", Name = "Put", Description = "Can update zone")]
        Zone_Put = 0x399,
        [Display(ShortName = "Setups", GroupName = "Zone", Name = "Delete", Description = "Can delete zone")]
        Zone_Delete = 0x400,
        [Display(ShortName = "RolesManagement", GroupName = "Role", Name = "Get", Description = "Can view role")]
        Role_Get = 0x401,
        [Display(ShortName = "RolesManagement", GroupName = "Role", Name = "Post", Description = "Can create role")]
        Role_Post = 0x402,
        [Display(ShortName = "RolesManagement", GroupName = "Role", Name = "Put", Description = "Can update role")]
        Role_Put = 0x403,
        [Display(ShortName = "RolesManagement", GroupName = "Role", Name = "Delete", Description = "Can delete role")]
        Role_Delete = 0x404,
        [Display(ShortName = "LeadAdministration", GroupName = "Contract", Name = "Get", Description = "Can view contract")]
        Contract_Get = 0x405,
        [Display(ShortName = "LeadAdministration", GroupName = "Contract", Name = "Post", Description = "Can create contract")]
        Contract_Post = 0x406,
        [Display(ShortName = "LeadAdministration", GroupName = "Contract", Name = "Put", Description = "Can update contract")]
        Contract_Put = 0x407,
        [Display(ShortName = "LeadAdministration", GroupName = "Contract", Name = "Delete", Description = "Can delete contract")]
        Contract_Delete = 0x408,
        [Display(ShortName = "LeadAdministration", GroupName = "ContractService", Name = "Get", Description = "Can view contract service")]
        ContractService_Get = 0x409,
        [Display(ShortName = "LeadAdministration", GroupName = "ContractService", Name = "Post", Description = "Can create contract service")]
        ContractService_Post = 0x410,
        [Display(ShortName = "LeadAdministration", GroupName = "ContractService", Name = "Put", Description = "Can update contract service")]
        ContractService_Put = 0x411,
        [Display(ShortName = "LeadAdministration", GroupName = "ContractService", Name = "Delete", Description = "Can delete contract service")]
        ContractService_Delete = 0x412,
        [Display(ShortName = "LeadAdministration", GroupName = "Customer", Name = "Get", Description = "Can view customer")]
        Customer_Get = 0x413,
        [Display(ShortName = "LeadAdministration", GroupName = "Customer", Name = "Post", Description = "Can create customer")]
        Customer_Post = 0x414,
        [Display(ShortName = "LeadAdministration", GroupName = "Customer", Name = "Put", Description = "Can update customer")]
        Customer_Put = 0x415,
        [Display(ShortName = "LeadAdministration", GroupName = "Customer", Name = "Delete", Description = "Can delete customer")]
        Customer_Delete = 0x416,
        [Display(ShortName = "LeadAdministration", GroupName = "CustomerDivision", Name = "Get", Description = "Can view customer division")]
        CustomerDivision_Get = 0x417,
        [Display(ShortName = "LeadAdministration", GroupName = "CustomerDivision", Name = "Post", Description = "Can create customer division")]
        CustomerDivision_Post = 0x418,
        [Display(ShortName = "LeadAdministration", GroupName = "CustomerDivision", Name = "Put", Description = "Can update customer division")]
        CustomerDivision_Put = 0x419,
        [Display(ShortName = "LeadAdministration", GroupName = "CustomerDivision", Name = "Delete", Description = "Can delete customer division")]
        CustomerDivision_Delete = 0x420,
        [Display(ShortName = "Finance", GroupName = "FinanceVoucherType", Name = "Get", Description = "Can view finance voucher type")]
        FinanceVoucherType_Get = 0x421,
        [Display(ShortName = "Finance", GroupName = "FinanceVoucherType", Name = "Post", Description = "Can create finance voucher type")]
        FinanceVoucherType_Post = 0x422,
        [Display(ShortName = "Finance", GroupName = "FinanceVoucherType", Name = "Put", Description = "Can update finance voucher type")]
        FinanceVoucherType_Put = 0x423,
        [Display(ShortName = "Finance", GroupName = "FinanceVoucherType", Name = "Delete", Description = "Can delete finance voucher type")]
        FinanceVoucherType_Delete = 0x424,
        [Display(ShortName = "LeadAdministration", GroupName = "LeadContact", Name = "Get", Description = "Can view lead contact")]
        LeadContact_Get = 0x425,
        [Display(ShortName = "LeadAdministration", GroupName = "LeadContact", Name = "Post", Description = "Can create lead contact")]
        LeadContact_Post = 0x426,
        [Display(ShortName = "LeadAdministration", GroupName = "LeadContact", Name = "Put", Description = "Can update lead contact")]
        LeadContact_Put = 0x427,
        [Display(ShortName = "LeadAdministration", GroupName = "LeadContact", Name = "Delete", Description = "Can delete lead contact")]
        LeadContact_Delete = 0x428,
        [Display(ShortName = "LeadAdministration", GroupName = "Lead", Name = "Get", Description = "Can view lead")]
        Lead_Get = 0x429,
        [Display(ShortName = "LeadAdministration", GroupName = "Lead", Name = "Post", Description = "Can create lead")]
        Lead_Post = 0x430,
        [Display(ShortName = "LeadAdministration", GroupName = "Lead", Name = "Put", Description = "Can update lead")]
        Lead_Put = 0x431,
        [Display(ShortName = "LeadAdministration", GroupName = "Lead", Name = "Delete", Description = "Can delete lead")]
        Lead_Delete = 0x432,
        [Display(ShortName = "LeadAdministration", GroupName = "LeadDivisionContact", Name = "Get", Description = "Can view lead division contact")]
        LeadDivisionContact_Get = 0x433,
        [Display(ShortName = "LeadAdministration", GroupName = "LeadDivisionContact", Name = "Post", Description = "Can create lead division contact")]
        LeadDivisionContact_Post = 0x434,
        [Display(ShortName = "LeadAdministration", GroupName = "LeadDivisionContact", Name = "Put", Description = "Can update lead division contact")]
        LeadDivisionContact_Put = 0x435,
        [Display(ShortName = "LeadAdministration", GroupName = "LeadDivisionContact", Name = "Delete", Description = "Can delete lead division contact")]
        LeadDivisionContact_Delete = 0x436,
        [Display(ShortName = "LeadAdministration", GroupName = "LeadDivisionKeyPerson", Name = "Get", Description = "Can view lead division key person")]
        LeadDivisionKeyPerson_Get = 0x437,
        [Display(ShortName = "LeadAdministration", GroupName = "LeadDivisionKeyPerson", Name = "Post", Description = "Can create lead division key person")]
        LeadDivisionKeyPerson_Post = 0x438,
        [Display(ShortName = "LeadAdministration", GroupName = "LeadDivisionKeyPerson", Name = "Put", Description = "Can update lead division key person")]
        LeadDivisionKeyPerson_Put = 0x439,
        [Display(ShortName = "LeadAdministration", GroupName = "LeadDivisionKeyPerson", Name = "Delete", Description = "Can delete lead division key person")]
        LeadDivisionKeyPerson_Delete = 0x440,
        [Display(ShortName = "LeadAdministration", GroupName = "LeadKeyPerson", Name = "Get", Description = "Can view lead key person")]
        LeadKeyPerson_Get = 0x441,
        [Display(ShortName = "LeadAdministration", GroupName = "LeadKeyPerson", Name = "Post", Description = "Can create lead key person")]
        LeadKeyPerson_Post = 0x442,
        [Display(ShortName = "LeadAdministration", GroupName = "LeadKeyPerson", Name = "Put", Description = "Can update lead key person")]
        LeadKeyPerson_Put = 0x443,
        [Display(ShortName = "LeadAdministration", GroupName = "LeadKeyPerson", Name = "Delete", Description = "Can delete lead key person")]
        LeadKeyPerson_Delete = 0x444,
        [Display(ShortName = "Setups", GroupName = "LeadOrigin", Name = "Get", Description = "Can view lead origin")]
        LeadOrigin_Get = 0x445,
        [Display(ShortName = "Setups", GroupName = "LeadOrigin", Name = "Post", Description = "Can create lead origin")]
        LeadOrigin_Post = 0x446,
        [Display(ShortName = "Setups", GroupName = "LeadOrigin", Name = "Put", Description = "Can update lead origin")]
        LeadOrigin_Put = 0x447,
        [Display(ShortName = "Setups", GroupName = "LeadOrigin", Name = "Delete", Description = "Can delete lead origin")]
        LeadOrigin_Delete = 0x448,
        [Display(ShortName = "LeadAdministration", GroupName = "SBUQuoteServiceProportion", Name = "Get", Description = "Can view sbu quote service proportion")]
        SBUQuoteServiceProportion_Get = 0x449,
        [Display(ShortName = "LeadAdministration", GroupName = "SBUQuoteServiceProportion", Name = "Post", Description = "Can create sbu quote service proportion")]
        SBUQuoteServiceProportion_Post = 0x450,
        [Display(ShortName = "LeadAdministration", GroupName = "SBUQuoteServiceProportion", Name = "Put", Description = "Can update sbu quote service proportion")]
        SBUQuoteServiceProportion_Put = 0x451,
        [Display(ShortName = "LeadAdministration", GroupName = "SBUQuoteServiceProportion", Name = "Delete", Description = "Can delete sbu quote service proportion")]
        SBUQuoteServiceProportion_Delete = 0x452,
        [Display(ShortName = "Finance", GroupName = "AccountClass", Name = "Get", Description = "Can view account class")]
        AccountClass_Get = 0x453,
        [Display(ShortName = "Finance", GroupName = "AccountClass", Name = "Post", Description = "Can create account class")]
        AccountClass_Post = 0x454,
        [Display(ShortName = "Finance", GroupName = "AccountClass", Name = "Put", Description = "Can update account class")]
        AccountClass_Put = 0x455,
        [Display(ShortName = "Finance", GroupName = "AccountClass", Name = "Delete", Description = "Can delete account class")]
        AccountClass_Delete = 0x456,
        [Display(ShortName = "Finance", GroupName = "Account", Name = "Get", Description = "Can view account")]
        Account_Get = 0x457,
        [Display(ShortName = "Finance", GroupName = "Account", Name = "Post", Description = "Can create account")]
        Account_Post = 0x458,
        [Display(ShortName = "Finance", GroupName = "Account", Name = "Put", Description = "Can update account")]
        Account_Put = 0x459,
        [Display(ShortName = "Finance", GroupName = "Account", Name = "Delete", Description = "Can delete account")]
        Account_Delete = 0x460,
        [Display(ShortName = "Finance", GroupName = "AccountDetail", Name = "Get", Description = "Can view account detail")]
        AccountDetail_Get = 0x461,
        [Display(ShortName = "Finance", GroupName = "AccountDetail", Name = "Post", Description = "Can create account detail")]
        AccountDetail_Post = 0x462,
        [Display(ShortName = "Finance", GroupName = "AccountDetail", Name = "Put", Description = "Can update account detail")]
        AccountDetail_Put = 0x463,
        [Display(ShortName = "Finance", GroupName = "AccountDetail", Name = "Delete", Description = "Can delete account detail")]
        AccountDetail_Delete = 0x464,
        [Display(ShortName = "Finance", GroupName = "AccountMaster", Name = "Get", Description = "Can view account master")]
        AccountMaster_Get = 0x465,
        [Display(ShortName = "Finance", GroupName = "AccountMaster", Name = "Post", Description = "Can create account master")]
        AccountMaster_Post = 0x466,
        [Display(ShortName = "Finance", GroupName = "AccountMaster", Name = "Put", Description = "Can update account master")]
        AccountMaster_Put = 0x467,
        [Display(ShortName = "Finance", GroupName = "AccountMaster", Name = "Delete", Description = "Can delete account master")]
        AccountMaster_Delete = 0x468,
        [Display(ShortName = "Finance", GroupName = "ControlAccount", Name = "Get", Description = "Can view control account")]
        ControlAccount_Get = 0x469,
        [Display(ShortName = "Finance", GroupName = "ControlAccount", Name = "Post", Description = "Can create control account")]
        ControlAccount_Post = 0x470,
        [Display(ShortName = "Finance", GroupName = "ControlAccount", Name = "Put", Description = "Can update control account")]
        ControlAccount_Put = 0x471,
        [Display(ShortName = "Finance", GroupName = "ControlAccount", Name = "Delete", Description = "Can delete control account")]
        ControlAccount_Delete = 0x472,
        [Display(ShortName = "Finance", GroupName = "Invoice", Name = "Get", Description = "Can view invoice")]
        Invoice_Get = 0x473,
        [Display(ShortName = "Finance", GroupName = "Invoice", Name = "Post", Description = "Can create invoice")]
        Invoice_Post = 0x474,
        [Display(ShortName = "Finance", GroupName = "Invoice", Name = "Put", Description = "Can update invoice")]
        Invoice_Put = 0x475,
        [Display(ShortName = "Finance", GroupName = "Invoice", Name = "Delete", Description = "Can delete invoice")]
        Invoice_Delete = 0x476,
        [Display(ShortName = "Finance", GroupName = "Receipt", Name = "Get", Description = "Can view receipt")]
        Receipt_Get = 0x477,
        [Display(ShortName = "Finance", GroupName = "Receipt", Name = "Post", Description = "Can create receipt")]
        Receipt_Post = 0x478,
        [Display(ShortName = "Finance", GroupName = "Receipt", Name = "Put", Description = "Can update receipt")]
        Receipt_Put = 0x479,
        [Display(ShortName = "Finance", GroupName = "Receipt", Name = "Delete", Description = "Can delete receipt")]
        Receipt_Delete = 0x480,

    }

}