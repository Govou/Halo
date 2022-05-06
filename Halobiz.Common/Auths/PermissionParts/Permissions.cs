
using System;
using System.ComponentModel.DataAnnotations;

namespace Halobiz.Common.Auths.PermissionParts
{
    public enum Permissions : int
    {
        NotSet = 0, //error or empty state condition

        [Display(ShortName = "ClientManagement", GroupName = "ClientBeneficiary", Name = "Get", Description = "Can view client beneficiary")]
        ClientBeneficiary_Get = 0x1081,
        [Display(ShortName = "ClientManagement", GroupName = "ClientBeneficiary", Name = "Post", Description = "Can create client beneficiary")]
        ClientBeneficiary_Post = 0x1082,
        [Display(ShortName = "ClientManagement", GroupName = "ClientBeneficiary", Name = "Put", Description = "Can update client beneficiary")]
        ClientBeneficiary_Put = 0x1083,
        [Display(ShortName = "ClientManagement", GroupName = "ClientBeneficiary", Name = "Delete", Description = "Can delete client beneficiary")]
        ClientBeneficiary_Delete = 0x1084,
        [Display(ShortName = "ClientManagement", GroupName = "ClientEngagement", Name = "Get", Description = "Can view client engagement")]
        ClientEngagement_Get = 0x1091,
        [Display(ShortName = "ClientManagement", GroupName = "ClientEngagement", Name = "Post", Description = "Can create client engagement")]
        ClientEngagement_Post = 0x1092,
        [Display(ShortName = "ClientManagement", GroupName = "ClientEngagement", Name = "Put", Description = "Can update client engagement")]
        ClientEngagement_Put = 0x1093,
        [Display(ShortName = "ClientManagement", GroupName = "ClientEngagement", Name = "Delete", Description = "Can delete client engagement")]
        ClientEngagement_Delete = 0x1094,
        [Display(ShortName = "LeadAdministration", GroupName = "ClosureDocument", Name = "Get", Description = "Can view closure document")]
        ClosureDocument_Get = 0x1101,
        [Display(ShortName = "LeadAdministration", GroupName = "ClosureDocument", Name = "Post", Description = "Can create closure document")]
        ClosureDocument_Post = 0x1102,
        [Display(ShortName = "LeadAdministration", GroupName = "ClosureDocument", Name = "Put", Description = "Can update closure document")]
        ClosureDocument_Put = 0x1103,
        [Display(ShortName = "LeadAdministration", GroupName = "ClosureDocument", Name = "Delete", Description = "Can delete closure document")]
        ClosureDocument_Delete = 0x1104,
        [Display(ShortName = "ProjectManagment", GroupName = "DeliverableFulfillment", Name = "Get", Description = "Can view deliverable fulfillment")]
        DeliverableFulfillment_Get = 0x8501,
        [Display(ShortName = "ProjectManagment", GroupName = "DeliverableFulfillment", Name = "Post", Description = "Can create deliverable fulfillment")]
        DeliverableFulfillment_Post = 0x8502,
        [Display(ShortName = "ProjectManagment", GroupName = "DeliverableFulfillment", Name = "Put", Description = "Can update deliverable fulfillment")]
        DeliverableFulfillment_Put = 0x8503,
        [Display(ShortName = "ProjectManagment", GroupName = "DeliverableFulfillment", Name = "Delete", Description = "Can delete deliverable fulfillment")]
        DeliverableFulfillment_Delete = 0x8504,
        [Display(ShortName = "LeadAdministration", GroupName = "DropReason", Name = "Get", Description = "Can view drop reason")]
        DropReason_Get = 0x1151,
        [Display(ShortName = "LeadAdministration", GroupName = "DropReason", Name = "Post", Description = "Can create drop reason")]
        DropReason_Post = 0x1152,
        [Display(ShortName = "LeadAdministration", GroupName = "DropReason", Name = "Put", Description = "Can update drop reason")]
        DropReason_Put = 0x1153,
        [Display(ShortName = "LeadAdministration", GroupName = "DropReason", Name = "Delete", Description = "Can delete drop reason")]
        DropReason_Delete = 0x1154,
        [Display(ShortName = "ClientManagement", GroupName = "Endorsement", Name = "Get", Description = "Can view endorsement")]
        Endorsement_Get = 0x1161,
        [Display(ShortName = "ClientManagement", GroupName = "Endorsement", Name = "Post", Description = "Can create endorsement")]
        Endorsement_Post = 0x1162,
        [Display(ShortName = "ClientManagement", GroupName = "Endorsement", Name = "Put", Description = "Can update endorsement")]
        Endorsement_Put = 0x1163,
        [Display(ShortName = "ClientManagement", GroupName = "Endorsement", Name = "Delete", Description = "Can delete endorsement")]
        Endorsement_Delete = 0x1164,
        [Display(ShortName = "ClientManagement", GroupName = "EndorsementType", Name = "Get", Description = "Can view endorsement type")]
        EndorsementType_Get = 0x1171,
        [Display(ShortName = "ClientManagement", GroupName = "EndorsementType", Name = "Post", Description = "Can create endorsement type")]
        EndorsementType_Post = 0x1172,
        [Display(ShortName = "ClientManagement", GroupName = "EndorsementType", Name = "Put", Description = "Can update endorsement type")]
        EndorsementType_Put = 0x1173,
        [Display(ShortName = "ClientManagement", GroupName = "EndorsementType", Name = "Delete", Description = "Can delete endorsement type")]
        EndorsementType_Delete = 0x1174,
        [Display(ShortName = "LeadAdministration", GroupName = "LeadDivision", Name = "Get", Description = "Can view lead division")]
        LeadDivision_Get = 0x1221,
        [Display(ShortName = "LeadAdministration", GroupName = "LeadDivision", Name = "Post", Description = "Can create lead division")]
        LeadDivision_Post = 0x1222,
        [Display(ShortName = "LeadAdministration", GroupName = "LeadDivision", Name = "Put", Description = "Can update lead division")]
        LeadDivision_Put = 0x1223,
        [Display(ShortName = "LeadAdministration", GroupName = "LeadDivision", Name = "Delete", Description = "Can delete lead division")]
        LeadDivision_Delete = 0x1224,
        [Display(ShortName = "LeadAdministration", GroupName = "LeadEngagement", Name = "Get", Description = "Can view lead engagement")]
        LeadEngagement_Get = 0x1241,
        [Display(ShortName = "LeadAdministration", GroupName = "LeadEngagement", Name = "Post", Description = "Can create lead engagement")]
        LeadEngagement_Post = 0x1242,
        [Display(ShortName = "LeadAdministration", GroupName = "LeadEngagement", Name = "Put", Description = "Can update lead engagement")]
        LeadEngagement_Put = 0x1243,
        [Display(ShortName = "LeadAdministration", GroupName = "LeadEngagement", Name = "Delete", Description = "Can delete lead engagement")]
        LeadEngagement_Delete = 0x1244,
        [Display(ShortName = "Setups", GroupName = "LeadType", Name = "Get", Description = "Can view lead type")]
        LeadType_Get = 0x1261,
        [Display(ShortName = "Setups", GroupName = "LeadType", Name = "Post", Description = "Can create lead type")]
        LeadType_Post = 0x1262,
        [Display(ShortName = "Setups", GroupName = "LeadType", Name = "Put", Description = "Can update lead type")]
        LeadType_Put = 0x1263,
        [Display(ShortName = "Setups", GroupName = "LeadType", Name = "Delete", Description = "Can delete lead type")]
        LeadType_Delete = 0x1264,
        [Display(ShortName = "LeadAdministration", GroupName = "NegotiationDocument", Name = "Get", Description = "Can view negotiation document")]
        NegotiationDocument_Get = 0x1271,
        [Display(ShortName = "LeadAdministration", GroupName = "NegotiationDocument", Name = "Post", Description = "Can create negotiation document")]
        NegotiationDocument_Post = 0x1272,
        [Display(ShortName = "LeadAdministration", GroupName = "NegotiationDocument", Name = "Put", Description = "Can update negotiation document")]
        NegotiationDocument_Put = 0x1273,
        [Display(ShortName = "LeadAdministration", GroupName = "NegotiationDocument", Name = "Delete", Description = "Can delete negotiation document")]
        NegotiationDocument_Delete = 0x1274,
        [Display(ShortName = "LeadAdministration", GroupName = "OtherLeadCaptureInfo", Name = "Get", Description = "Can view other lead capture info")]
        OtherLeadCaptureInfo_Get = 0x1281,
        [Display(ShortName = "LeadAdministration", GroupName = "OtherLeadCaptureInfo", Name = "Post", Description = "Can create other lead capture info")]
        OtherLeadCaptureInfo_Post = 0x1282,
        [Display(ShortName = "LeadAdministration", GroupName = "OtherLeadCaptureInfo", Name = "Put", Description = "Can update other lead capture info")]
        OtherLeadCaptureInfo_Put = 0x1283,
        [Display(ShortName = "LeadAdministration", GroupName = "OtherLeadCaptureInfo", Name = "Delete", Description = "Can delete other lead capture info")]
        OtherLeadCaptureInfo_Delete = 0x1284,
        [Display(ShortName = "LeadAdministration", GroupName = "Quote", Name = "Get", Description = "Can view quote")]
        Quote_Get = 0x1291,
        [Display(ShortName = "LeadAdministration", GroupName = "Quote", Name = "Post", Description = "Can create quote")]
        Quote_Post = 0x1292,
        [Display(ShortName = "LeadAdministration", GroupName = "Quote", Name = "Put", Description = "Can update quote")]
        Quote_Put = 0x1293,
        [Display(ShortName = "LeadAdministration", GroupName = "Quote", Name = "Delete", Description = "Can delete quote")]
        Quote_Delete = 0x1294,
        [Display(ShortName = "LeadAdministration", GroupName = "QuoteService", Name = "Get", Description = "Can view quote service")]
        QuoteService_Get = 0x1301,
        [Display(ShortName = "LeadAdministration", GroupName = "QuoteService", Name = "Post", Description = "Can create quote service")]
        QuoteService_Post = 0x1302,
        [Display(ShortName = "LeadAdministration", GroupName = "QuoteService", Name = "Put", Description = "Can update quote service")]
        QuoteService_Put = 0x1303,
        [Display(ShortName = "LeadAdministration", GroupName = "QuoteService", Name = "Delete", Description = "Can delete quote service")]
        QuoteService_Delete = 0x1304,
        [Display(ShortName = "LeadAdministration", GroupName = "QuoteServiceDocument", Name = "Get", Description = "Can view quote service document")]
        QuoteServiceDocument_Get = 0x1311,
        [Display(ShortName = "LeadAdministration", GroupName = "QuoteServiceDocument", Name = "Post", Description = "Can create quote service document")]
        QuoteServiceDocument_Post = 0x1312,
        [Display(ShortName = "LeadAdministration", GroupName = "QuoteServiceDocument", Name = "Put", Description = "Can update quote service document")]
        QuoteServiceDocument_Put = 0x1313,
        [Display(ShortName = "LeadAdministration", GroupName = "QuoteServiceDocument", Name = "Delete", Description = "Can delete quote service document")]
        QuoteServiceDocument_Delete = 0x1314,
        [Display(ShortName = "LeadAdministration", GroupName = "TaskFulfillment", Name = "Get", Description = "Can view task fulfillment")]
        TaskFulfillment_Get = 0x1341,
        [Display(ShortName = "LeadAdministration", GroupName = "TaskFulfillment", Name = "Post", Description = "Can create task fulfillment")]
        TaskFulfillment_Post = 0x1342,
        [Display(ShortName = "LeadAdministration", GroupName = "TaskFulfillment", Name = "Put", Description = "Can update task fulfillment")]
        TaskFulfillment_Put = 0x1343,
        [Display(ShortName = "LeadAdministration", GroupName = "TaskFulfillment", Name = "Delete", Description = "Can delete task fulfillment")]
        TaskFulfillment_Delete = 0x1344,
        [Display(ShortName = "RolesManagement", GroupName = "Activity", Name = "Get", Description = "Can view activity")]
        Activity_Get = 0x1361,
        [Display(ShortName = "RolesManagement", GroupName = "Activity", Name = "Post", Description = "Can create activity")]
        Activity_Post = 0x1362,
        [Display(ShortName = "RolesManagement", GroupName = "Activity", Name = "Put", Description = "Can update activity")]
        Activity_Put = 0x1363,
        [Display(ShortName = "RolesManagement", GroupName = "Activity", Name = "Delete", Description = "Can delete activity")]
        Activity_Delete = 0x1364,
        [Display(ShortName = "Setups", GroupName = "AppFeedback", Name = "Get", Description = "Can view app feedback")]
        AppFeedback_Get = 0x1371,
        [Display(ShortName = "Setups", GroupName = "AppFeedback", Name = "Post", Description = "Can create app feedback")]
        AppFeedback_Post = 0x1372,
        [Display(ShortName = "Setups", GroupName = "AppFeedback", Name = "Put", Description = "Can update app feedback")]
        AppFeedback_Put = 0x1373,
        [Display(ShortName = "Setups", GroupName = "AppFeedback", Name = "Delete", Description = "Can delete app feedback")]
        AppFeedback_Delete = 0x1374,
        [Display(ShortName = "Setups", GroupName = "AppReview", Name = "Get", Description = "Can view app review")]
        AppReview_Get = 0x1381,
        [Display(ShortName = "Setups", GroupName = "AppReview", Name = "Post", Description = "Can create app review")]
        AppReview_Post = 0x1382,
        [Display(ShortName = "Setups", GroupName = "AppReview", Name = "Put", Description = "Can update app review")]
        AppReview_Put = 0x1383,
        [Display(ShortName = "Setups", GroupName = "AppReview", Name = "Delete", Description = "Can delete app review")]
        AppReview_Delete = 0x1384,
        [Display(ShortName = "ApprovalManagement", GroupName = "Approval", Name = "Get", Description = "Can view approval")]
        Approval_Get = 0x2171,
        [Display(ShortName = "ApprovalManagement", GroupName = "Approval", Name = "Post", Description = "Can create approval")]
        Approval_Post = 0x2172,
        [Display(ShortName = "ApprovalManagement", GroupName = "Approval", Name = "Put", Description = "Can update approval")]
        Approval_Put = 0x2173,
        [Display(ShortName = "ApprovalManagement", GroupName = "Approval", Name = "Delete", Description = "Can delete approval")]
        Approval_Delete = 0x2174,
        [Display(ShortName = "Setups", GroupName = "ApprovalLimit", Name = "Get", Description = "Can view approval limit")]
        ApprovalLimit_Get = 0x1391,
        [Display(ShortName = "Setups", GroupName = "ApprovalLimit", Name = "Post", Description = "Can create approval limit")]
        ApprovalLimit_Post = 0x1392,
        [Display(ShortName = "Setups", GroupName = "ApprovalLimit", Name = "Put", Description = "Can update approval limit")]
        ApprovalLimit_Put = 0x1393,
        [Display(ShortName = "Setups", GroupName = "ApprovalLimit", Name = "Delete", Description = "Can delete approval limit")]
        ApprovalLimit_Delete = 0x1394,
        [Display(ShortName = "Setups", GroupName = "ApproverLevel", Name = "Get", Description = "Can view approver level")]
        ApproverLevel_Get = 0x1401,
        [Display(ShortName = "Setups", GroupName = "ApproverLevel", Name = "Post", Description = "Can create approver level")]
        ApproverLevel_Post = 0x1402,
        [Display(ShortName = "Setups", GroupName = "ApproverLevel", Name = "Put", Description = "Can update approver level")]
        ApproverLevel_Put = 0x1403,
        [Display(ShortName = "Setups", GroupName = "ApproverLevel", Name = "Delete", Description = "Can delete approver level")]
        ApproverLevel_Delete = 0x1404,
        [Display(ShortName = "SecuredMobility", GroupName = "ArmedEscort", Name = "Get", Description = "Can view armed escort")]
        ArmedEscort_Get = 0x1411,
        [Display(ShortName = "SecuredMobility", GroupName = "ArmedEscort", Name = "Post", Description = "Can create armed escort")]
        ArmedEscort_Post = 0x1412,
        [Display(ShortName = "SecuredMobility", GroupName = "ArmedEscort", Name = "Put", Description = "Can update armed escort")]
        ArmedEscort_Put = 0x1413,
        [Display(ShortName = "SecuredMobility", GroupName = "ArmedEscort", Name = "Delete", Description = "Can delete armed escort")]
        ArmedEscort_Delete = 0x1414,
        [Display(ShortName = "SecuredMobility", GroupName = "ArmedEscortRegistration", Name = "Get", Description = "Can view armed escort registration")]
        ArmedEscortRegistration_Get = 0x1421,
        [Display(ShortName = "SecuredMobility", GroupName = "ArmedEscortRegistration", Name = "Post", Description = "Can create armed escort registration")]
        ArmedEscortRegistration_Post = 0x1422,
        [Display(ShortName = "SecuredMobility", GroupName = "ArmedEscortRegistration", Name = "Put", Description = "Can update armed escort registration")]
        ArmedEscortRegistration_Put = 0x1423,
        [Display(ShortName = "SecuredMobility", GroupName = "ArmedEscortRegistration", Name = "Delete", Description = "Can delete armed escort registration")]
        ArmedEscortRegistration_Delete = 0x1424,
        [Display(ShortName = "Setups", GroupName = "Auth", Name = "Get", Description = "Can view auth")]
        Auth_Get = 0x1431,
        [Display(ShortName = "Setups", GroupName = "Auth", Name = "Post", Description = "Can create auth")]
        Auth_Post = 0x1432,
        [Display(ShortName = "Setups", GroupName = "Auth", Name = "Put", Description = "Can update auth")]
        Auth_Put = 0x1433,
        [Display(ShortName = "Setups", GroupName = "Auth", Name = "Delete", Description = "Can delete auth")]
        Auth_Delete = 0x1434,
        [Display(ShortName = "Setups", GroupName = "Bank", Name = "Get", Description = "Can view bank")]
        Bank_Get = 0x1441,
        [Display(ShortName = "Setups", GroupName = "Bank", Name = "Post", Description = "Can create bank")]
        Bank_Post = 0x1442,
        [Display(ShortName = "Setups", GroupName = "Bank", Name = "Put", Description = "Can update bank")]
        Bank_Put = 0x1443,
        [Display(ShortName = "Setups", GroupName = "Bank", Name = "Delete", Description = "Can delete bank")]
        Bank_Delete = 0x1444,
        [Display(ShortName = "Setups", GroupName = "Branch", Name = "Get", Description = "Can view branch")]
        Branch_Get = 0x1451,
        [Display(ShortName = "Setups", GroupName = "Branch", Name = "Post", Description = "Can create branch")]
        Branch_Post = 0x1452,
        [Display(ShortName = "Setups", GroupName = "Branch", Name = "Put", Description = "Can update branch")]
        Branch_Put = 0x1453,
        [Display(ShortName = "Setups", GroupName = "Branch", Name = "Delete", Description = "Can delete branch")]
        Branch_Delete = 0x1454,
        [Display(ShortName = "Setups", GroupName = "BusinessRules", Name = "Get", Description = "Can view business rules")]
        BusinessRules_Get = 0x1461,
        [Display(ShortName = "Setups", GroupName = "BusinessRules", Name = "Post", Description = "Can create business rules")]
        BusinessRules_Post = 0x1462,
        [Display(ShortName = "Setups", GroupName = "BusinessRules", Name = "Put", Description = "Can update business rules")]
        BusinessRules_Put = 0x1463,
        [Display(ShortName = "Setups", GroupName = "BusinessRules", Name = "Delete", Description = "Can delete business rules")]
        BusinessRules_Delete = 0x1464,
        [Display(ShortName = "ClientManagement", GroupName = "ClientContactQualification", Name = "Get", Description = "Can view client contact qualification")]
        ClientContactQualification_Get = 0x1471,
        [Display(ShortName = "ClientManagement", GroupName = "ClientContactQualification", Name = "Post", Description = "Can create client contact qualification")]
        ClientContactQualification_Post = 0x1472,
        [Display(ShortName = "ClientManagement", GroupName = "ClientContactQualification", Name = "Put", Description = "Can update client contact qualification")]
        ClientContactQualification_Put = 0x1473,
        [Display(ShortName = "ClientManagement", GroupName = "ClientContactQualification", Name = "Delete", Description = "Can delete client contact qualification")]
        ClientContactQualification_Delete = 0x1474,
        [Display(ShortName = "ClientManagement", GroupName = "ClientPolicy", Name = "Get", Description = "Can view client policy")]
        ClientPolicy_Get = 0x1481,
        [Display(ShortName = "ClientManagement", GroupName = "ClientPolicy", Name = "Post", Description = "Can create client policy")]
        ClientPolicy_Post = 0x1482,
        [Display(ShortName = "ClientManagement", GroupName = "ClientPolicy", Name = "Put", Description = "Can update client policy")]
        ClientPolicy_Put = 0x1483,
        [Display(ShortName = "ClientManagement", GroupName = "ClientPolicy", Name = "Delete", Description = "Can delete client policy")]
        ClientPolicy_Delete = 0x1484,
        [Display(ShortName = "SecuredMobility", GroupName = "Commander", Name = "Get", Description = "Can view commander")]
        Commander_Get = 0x1491,
        [Display(ShortName = "SecuredMobility", GroupName = "Commander", Name = "Post", Description = "Can create commander")]
        Commander_Post = 0x1492,
        [Display(ShortName = "SecuredMobility", GroupName = "Commander", Name = "Put", Description = "Can update commander")]
        Commander_Put = 0x1493,
        [Display(ShortName = "SecuredMobility", GroupName = "Commander", Name = "Delete", Description = "Can delete commander")]
        Commander_Delete = 0x1494,
        [Display(ShortName = "SecuredMobility", GroupName = "CommanderRegistration", Name = "Get", Description = "Can view commander registration")]
        CommanderRegistration_Get = 0x2181,
        [Display(ShortName = "SecuredMobility", GroupName = "CommanderRegistration", Name = "Post", Description = "Can create commander registration")]
        CommanderRegistration_Post = 0x2182,
        [Display(ShortName = "SecuredMobility", GroupName = "CommanderRegistration", Name = "Put", Description = "Can update commander registration")]
        CommanderRegistration_Put = 0x2183,
        [Display(ShortName = "SecuredMobility", GroupName = "CommanderRegistration", Name = "Delete", Description = "Can delete commander registration")]
        CommanderRegistration_Delete = 0x2184,
        [Display(ShortName = "Setups", GroupName = "Company", Name = "Get", Description = "Can view company")]
        Company_Get = 0x1501,
        [Display(ShortName = "Setups", GroupName = "Company", Name = "Post", Description = "Can create company")]
        Company_Post = 0x1502,
        [Display(ShortName = "Setups", GroupName = "Company", Name = "Put", Description = "Can update company")]
        Company_Put = 0x1503,
        [Display(ShortName = "Setups", GroupName = "Company", Name = "Delete", Description = "Can delete company")]
        Company_Delete = 0x1504,
        [Display(ShortName = "ComplaintManagement", GroupName = "Complaint", Name = "Get", Description = "Can view complaint")]
        Complaint_Get = 0x1511,
        [Display(ShortName = "ComplaintManagement", GroupName = "Complaint", Name = "Post", Description = "Can create complaint")]
        Complaint_Post = 0x1512,
        [Display(ShortName = "ComplaintManagement", GroupName = "Complaint", Name = "Put", Description = "Can update complaint")]
        Complaint_Put = 0x1513,
        [Display(ShortName = "ComplaintManagement", GroupName = "Complaint", Name = "Delete", Description = "Can delete complaint")]
        Complaint_Delete = 0x1514,
        [Display(ShortName = "ComplaintManagement", GroupName = "ComplaintHandling", Name = "Get", Description = "Can view complaint handling")]
        ComplaintHandling_Get = 0x1521,
        [Display(ShortName = "ComplaintManagement", GroupName = "ComplaintHandling", Name = "Post", Description = "Can create complaint handling")]
        ComplaintHandling_Post = 0x1522,
        [Display(ShortName = "ComplaintManagement", GroupName = "ComplaintHandling", Name = "Put", Description = "Can update complaint handling")]
        ComplaintHandling_Put = 0x1523,
        [Display(ShortName = "ComplaintManagement", GroupName = "ComplaintHandling", Name = "Delete", Description = "Can delete complaint handling")]
        ComplaintHandling_Delete = 0x1524,
        [Display(ShortName = "Setups", GroupName = "ComplaintOrigin", Name = "Get", Description = "Can view complaint origin")]
        ComplaintOrigin_Get = 0x1531,
        [Display(ShortName = "Setups", GroupName = "ComplaintOrigin", Name = "Post", Description = "Can create complaint origin")]
        ComplaintOrigin_Post = 0x1532,
        [Display(ShortName = "Setups", GroupName = "ComplaintOrigin", Name = "Put", Description = "Can update complaint origin")]
        ComplaintOrigin_Put = 0x1533,
        [Display(ShortName = "Setups", GroupName = "ComplaintOrigin", Name = "Delete", Description = "Can delete complaint origin")]
        ComplaintOrigin_Delete = 0x1534,
        [Display(ShortName = "Setups", GroupName = "ComplaintSource", Name = "Get", Description = "Can view complaint source")]
        ComplaintSource_Get = 0x1541,
        [Display(ShortName = "Setups", GroupName = "ComplaintSource", Name = "Post", Description = "Can create complaint source")]
        ComplaintSource_Post = 0x1542,
        [Display(ShortName = "Setups", GroupName = "ComplaintSource", Name = "Put", Description = "Can update complaint source")]
        ComplaintSource_Put = 0x1543,
        [Display(ShortName = "Setups", GroupName = "ComplaintSource", Name = "Delete", Description = "Can delete complaint source")]
        ComplaintSource_Delete = 0x1544,
        [Display(ShortName = "Setups", GroupName = "ComplaintType", Name = "Get", Description = "Can view complaint type")]
        ComplaintType_Get = 0x1551,
        [Display(ShortName = "Setups", GroupName = "ComplaintType", Name = "Post", Description = "Can create complaint type")]
        ComplaintType_Post = 0x1552,
        [Display(ShortName = "Setups", GroupName = "ComplaintType", Name = "Put", Description = "Can update complaint type")]
        ComplaintType_Put = 0x1553,
        [Display(ShortName = "Setups", GroupName = "ComplaintType", Name = "Delete", Description = "Can delete complaint type")]
        ComplaintType_Delete = 0x1554,
        [Display(ShortName = "LeadAdministration", GroupName = "Contact", Name = "Get", Description = "Can view contact")]
        Contact_Get = 0x1561,
        [Display(ShortName = "LeadAdministration", GroupName = "Contact", Name = "Post", Description = "Can create contact")]
        Contact_Post = 0x1562,
        [Display(ShortName = "LeadAdministration", GroupName = "Contact", Name = "Put", Description = "Can update contact")]
        Contact_Put = 0x1563,
        [Display(ShortName = "LeadAdministration", GroupName = "Contact", Name = "Delete", Description = "Can delete contact")]
        Contact_Delete = 0x1564,
        [Display(ShortName = "CronJobs", GroupName = "CronJobs", Name = "Get", Description = "Can view cron jobs")]
        CronJobs_Get = 0x1571,
        [Display(ShortName = "CronJobs", GroupName = "CronJobs", Name = "Post", Description = "Can create cron jobs")]
        CronJobs_Post = 0x1572,
        [Display(ShortName = "CronJobs", GroupName = "CronJobs", Name = "Put", Description = "Can update cron jobs")]
        CronJobs_Put = 0x1573,
        [Display(ShortName = "CronJobs", GroupName = "CronJobs", Name = "Delete", Description = "Can delete cron jobs")]
        CronJobs_Delete = 0x1574,
        [Display(ShortName = "Setups", GroupName = "Designation", Name = "Get", Description = "Can view designation")]
        Designation_Get = 0x1581,
        [Display(ShortName = "Setups", GroupName = "Designation", Name = "Post", Description = "Can create designation")]
        Designation_Post = 0x1582,
        [Display(ShortName = "Setups", GroupName = "Designation", Name = "Put", Description = "Can update designation")]
        Designation_Put = 0x1583,
        [Display(ShortName = "Setups", GroupName = "Designation", Name = "Delete", Description = "Can delete designation")]
        Designation_Delete = 0x1584,
        [Display(ShortName = "Setups", GroupName = "Division", Name = "Get", Description = "Can view division")]
        Division_Get = 0x1591,
        [Display(ShortName = "Setups", GroupName = "Division", Name = "Post", Description = "Can create division")]
        Division_Post = 0x1592,
        [Display(ShortName = "Setups", GroupName = "Division", Name = "Put", Description = "Can update division")]
        Division_Put = 0x1593,
        [Display(ShortName = "Setups", GroupName = "Division", Name = "Delete", Description = "Can delete division")]
        Division_Delete = 0x1594,
        [Display(ShortName = "SecuredMobility", GroupName = "DTSDetailGenericDays", Name = "Get", Description = "Can view dts detail generic days")]
        DTSDetailGenericDays_Get = 0x1601,
        [Display(ShortName = "SecuredMobility", GroupName = "DTSDetailGenericDays", Name = "Post", Description = "Can create dts detail generic days")]
        DTSDetailGenericDays_Post = 0x1602,
        [Display(ShortName = "SecuredMobility", GroupName = "DTSDetailGenericDays", Name = "Put", Description = "Can update dts detail generic days")]
        DTSDetailGenericDays_Put = 0x1603,
        [Display(ShortName = "SecuredMobility", GroupName = "DTSDetailGenericDays", Name = "Delete", Description = "Can delete dts detail generic days")]
        DTSDetailGenericDays_Delete = 0x1604,
        [Display(ShortName = "SecuredMobility", GroupName = "DTSMasters", Name = "Get", Description = "Can view dts masters")]
        DTSMasters_Get = 0x1611,
        [Display(ShortName = "SecuredMobility", GroupName = "DTSMasters", Name = "Post", Description = "Can create dts masters")]
        DTSMasters_Post = 0x1612,
        [Display(ShortName = "SecuredMobility", GroupName = "DTSMasters", Name = "Put", Description = "Can update dts masters")]
        DTSMasters_Put = 0x1613,
        [Display(ShortName = "SecuredMobility", GroupName = "DTSMasters", Name = "Delete", Description = "Can delete dts masters")]
        DTSMasters_Delete = 0x1614,
        [Display(ShortName = "Setups", GroupName = "EngagementReason", Name = "Get", Description = "Can view engagement reason")]
        EngagementReason_Get = 0x1621,
        [Display(ShortName = "Setups", GroupName = "EngagementReason", Name = "Post", Description = "Can create engagement reason")]
        EngagementReason_Post = 0x1622,
        [Display(ShortName = "Setups", GroupName = "EngagementReason", Name = "Put", Description = "Can update engagement reason")]
        EngagementReason_Put = 0x1623,
        [Display(ShortName = "Setups", GroupName = "EngagementReason", Name = "Delete", Description = "Can delete engagement reason")]
        EngagementReason_Delete = 0x1624,
        [Display(ShortName = "LeadAdministration", GroupName = "EngagementType", Name = "Get", Description = "Can view engagement type")]
        EngagementType_Get = 0x2191,
        [Display(ShortName = "LeadAdministration", GroupName = "EngagementType", Name = "Post", Description = "Can create engagement type")]
        EngagementType_Post = 0x2192,
        [Display(ShortName = "LeadAdministration", GroupName = "EngagementType", Name = "Put", Description = "Can update engagement type")]
        EngagementType_Put = 0x2193,
        [Display(ShortName = "LeadAdministration", GroupName = "EngagementType", Name = "Delete", Description = "Can delete engagement type")]
        EngagementType_Delete = 0x2194,
        [Display(ShortName = "Setups", GroupName = "EscalationLevel", Name = "Get", Description = "Can view escalation level")]
        EscalationLevel_Get = 0x1631,
        [Display(ShortName = "Setups", GroupName = "EscalationLevel", Name = "Post", Description = "Can create escalation level")]
        EscalationLevel_Post = 0x1632,
        [Display(ShortName = "Setups", GroupName = "EscalationLevel", Name = "Put", Description = "Can update escalation level")]
        EscalationLevel_Put = 0x1633,
        [Display(ShortName = "Setups", GroupName = "EscalationLevel", Name = "Delete", Description = "Can delete escalation level")]
        EscalationLevel_Delete = 0x1634,
        [Display(ShortName = "Setups", GroupName = "EscalationMatrix", Name = "Get", Description = "Can view escalation matrix")]
        EscalationMatrix_Get = 0x1651,
        [Display(ShortName = "Setups", GroupName = "EscalationMatrix", Name = "Post", Description = "Can create escalation matrix")]
        EscalationMatrix_Post = 0x1652,
        [Display(ShortName = "Setups", GroupName = "EscalationMatrix", Name = "Put", Description = "Can update escalation matrix")]
        EscalationMatrix_Put = 0x1653,
        [Display(ShortName = "Setups", GroupName = "EscalationMatrix", Name = "Delete", Description = "Can delete escalation matrix")]
        EscalationMatrix_Delete = 0x1654,
        [Display(ShortName = "Setups", GroupName = "Evidence", Name = "Get", Description = "Can view evidence")]
        Evidence_Get = 0x1661,
        [Display(ShortName = "Setups", GroupName = "Evidence", Name = "Post", Description = "Can create evidence")]
        Evidence_Post = 0x1662,
        [Display(ShortName = "Setups", GroupName = "Evidence", Name = "Put", Description = "Can update evidence")]
        Evidence_Put = 0x1663,
        [Display(ShortName = "Setups", GroupName = "Evidence", Name = "Delete", Description = "Can delete evidence")]
        Evidence_Delete = 0x1664,
        [Display(ShortName = "LeadAdministration", GroupName = "GenerateGroupInvoiceNumber", Name = "Get", Description = "Can view generate group invoice number")]
        GenerateGroupInvoiceNumber_Get = 0x1671,
        [Display(ShortName = "LeadAdministration", GroupName = "GenerateGroupInvoiceNumber", Name = "Post", Description = "Can create generate group invoice number")]
        GenerateGroupInvoiceNumber_Post = 0x1672,
        [Display(ShortName = "LeadAdministration", GroupName = "GenerateGroupInvoiceNumber", Name = "Put", Description = "Can update generate group invoice number")]
        GenerateGroupInvoiceNumber_Put = 0x1673,
        [Display(ShortName = "LeadAdministration", GroupName = "GenerateGroupInvoiceNumber", Name = "Delete", Description = "Can delete generate group invoice number")]
        GenerateGroupInvoiceNumber_Delete = 0x1674,
        [Display(ShortName = "Setups", GroupName = "GroupType", Name = "Get", Description = "Can view group type")]
        GroupType_Get = 0x1681,
        [Display(ShortName = "Setups", GroupName = "GroupType", Name = "Post", Description = "Can create group type")]
        GroupType_Post = 0x1682,
        [Display(ShortName = "Setups", GroupName = "GroupType", Name = "Put", Description = "Can update group type")]
        GroupType_Put = 0x1683,
        [Display(ShortName = "Setups", GroupName = "GroupType", Name = "Delete", Description = "Can delete group type")]
        GroupType_Delete = 0x1684,
        [Display(ShortName = "Setups", GroupName = "Industry", Name = "Get", Description = "Can view industry")]
        Industry_Get = 0x2201,
        [Display(ShortName = "Setups", GroupName = "Industry", Name = "Post", Description = "Can create industry")]
        Industry_Post = 0x2202,
        [Display(ShortName = "Setups", GroupName = "Industry", Name = "Put", Description = "Can update industry")]
        Industry_Put = 0x2203,
        [Display(ShortName = "Setups", GroupName = "Industry", Name = "Delete", Description = "Can delete industry")]
        Industry_Delete = 0x2204,
        [Display(ShortName = "SecuredMobility", GroupName = "JourneyStartandStop", Name = "Get", Description = "Can view journey startand stop")]
        JourneyStartandStop_Get = 0x1691,
        [Display(ShortName = "SecuredMobility", GroupName = "JourneyStartandStop", Name = "Post", Description = "Can create journey startand stop")]
        JourneyStartandStop_Post = 0x1692,
        [Display(ShortName = "SecuredMobility", GroupName = "JourneyStartandStop", Name = "Put", Description = "Can update journey startand stop")]
        JourneyStartandStop_Put = 0x1693,
        [Display(ShortName = "SecuredMobility", GroupName = "JourneyStartandStop", Name = "Delete", Description = "Can delete journey startand stop")]
        JourneyStartandStop_Delete = 0x1694,
        [Display(ShortName = "Supplier", GroupName = "Make", Name = "Get", Description = "Can view make")]
        Make_Get = 0x1701,
        [Display(ShortName = "Supplier", GroupName = "Make", Name = "Post", Description = "Can create make")]
        Make_Post = 0x1702,
        [Display(ShortName = "Supplier", GroupName = "Make", Name = "Put", Description = "Can update make")]
        Make_Put = 0x1703,
        [Display(ShortName = "Supplier", GroupName = "Make", Name = "Delete", Description = "Can delete make")]
        Make_Delete = 0x1704,
        [Display(ShortName = "SecuredMobility", GroupName = "MasterServiceAssignment", Name = "Get", Description = "Can view master service assignment")]
        MasterServiceAssignment_Get = 0x1711,
        [Display(ShortName = "SecuredMobility", GroupName = "MasterServiceAssignment", Name = "Post", Description = "Can create master service assignment")]
        MasterServiceAssignment_Post = 0x1712,
        [Display(ShortName = "SecuredMobility", GroupName = "MasterServiceAssignment", Name = "Put", Description = "Can update master service assignment")]
        MasterServiceAssignment_Put = 0x1713,
        [Display(ShortName = "SecuredMobility", GroupName = "MasterServiceAssignment", Name = "Delete", Description = "Can delete master service assignment")]
        MasterServiceAssignment_Delete = 0x1714,
        [Display(ShortName = "Setups", GroupName = "MeansOfIdentification", Name = "Get", Description = "Can view means of identification")]
        MeansOfIdentification_Get = 0x1721,
        [Display(ShortName = "Setups", GroupName = "MeansOfIdentification", Name = "Post", Description = "Can create means of identification")]
        MeansOfIdentification_Post = 0x1722,
        [Display(ShortName = "Setups", GroupName = "MeansOfIdentification", Name = "Put", Description = "Can update means of identification")]
        MeansOfIdentification_Put = 0x1723,
        [Display(ShortName = "Setups", GroupName = "MeansOfIdentification", Name = "Delete", Description = "Can delete means of identification")]
        MeansOfIdentification_Delete = 0x1724,
        [Display(ShortName = "Supplier", GroupName = "Model", Name = "Get", Description = "Can view model")]
        Model_Get = 0x1731,
        [Display(ShortName = "Supplier", GroupName = "Model", Name = "Post", Description = "Can create model")]
        Model_Post = 0x1732,
        [Display(ShortName = "Supplier", GroupName = "Model", Name = "Put", Description = "Can update model")]
        Model_Put = 0x1733,
        [Display(ShortName = "Supplier", GroupName = "Model", Name = "Delete", Description = "Can delete model")]
        Model_Delete = 0x1734,
        [Display(ShortName = "Setups", GroupName = "ModeOfTransport", Name = "Get", Description = "Can view mode of transport")]
        ModeOfTransport_Get = 0x1741,
        [Display(ShortName = "Setups", GroupName = "ModeOfTransport", Name = "Post", Description = "Can create mode of transport")]
        ModeOfTransport_Post = 0x1742,
        [Display(ShortName = "Setups", GroupName = "ModeOfTransport", Name = "Put", Description = "Can update mode of transport")]
        ModeOfTransport_Put = 0x1743,
        [Display(ShortName = "Setups", GroupName = "ModeOfTransport", Name = "Delete", Description = "Can delete mode of transport")]
        ModeOfTransport_Delete = 0x1744,
        [Display(ShortName = "SecuredMobility", GroupName = "Note", Name = "Get", Description = "Can view note")]
        Note_Get = 0x1751,
        [Display(ShortName = "SecuredMobility", GroupName = "Note", Name = "Post", Description = "Can create note")]
        Note_Post = 0x1752,
        [Display(ShortName = "SecuredMobility", GroupName = "Note", Name = "Put", Description = "Can update note")]
        Note_Put = 0x1753,
        [Display(ShortName = "SecuredMobility", GroupName = "Note", Name = "Delete", Description = "Can delete note")]
        Note_Delete = 0x1754,
        [Display(ShortName = "Profile", GroupName = "Office", Name = "Get", Description = "Can view office")]
        Office_Get = 0x1761,
        [Display(ShortName = "Profile", GroupName = "Office", Name = "Post", Description = "Can create office")]
        Office_Post = 0x1762,
        [Display(ShortName = "Profile", GroupName = "Office", Name = "Put", Description = "Can update office")]
        Office_Put = 0x1763,
        [Display(ShortName = "Profile", GroupName = "Office", Name = "Delete", Description = "Can delete office")]
        Office_Delete = 0x1764,
        [Display(ShortName = "Setups", GroupName = "OperatingEntity", Name = "Get", Description = "Can view operating entity")]
        OperatingEntity_Get = 0x1771,
        [Display(ShortName = "Setups", GroupName = "OperatingEntity", Name = "Post", Description = "Can create operating entity")]
        OperatingEntity_Post = 0x1772,
        [Display(ShortName = "Setups", GroupName = "OperatingEntity", Name = "Put", Description = "Can update operating entity")]
        OperatingEntity_Put = 0x1773,
        [Display(ShortName = "Setups", GroupName = "OperatingEntity", Name = "Delete", Description = "Can delete operating entity")]
        OperatingEntity_Delete = 0x1774,
        [Display(ShortName = "SecuredMobility", GroupName = "Pilot", Name = "Get", Description = "Can view pilot")]
        Pilot_Get = 0x1781,
        [Display(ShortName = "SecuredMobility", GroupName = "Pilot", Name = "Post", Description = "Can create pilot")]
        Pilot_Post = 0x1782,
        [Display(ShortName = "SecuredMobility", GroupName = "Pilot", Name = "Put", Description = "Can update pilot")]
        Pilot_Put = 0x1783,
        [Display(ShortName = "SecuredMobility", GroupName = "Pilot", Name = "Delete", Description = "Can delete pilot")]
        Pilot_Delete = 0x1784,
        [Display(ShortName = "SecuredMobility", GroupName = "PilotRegistration", Name = "Get", Description = "Can view pilot registration")]
        PilotRegistration_Get = 0x1791,
        [Display(ShortName = "SecuredMobility", GroupName = "PilotRegistration", Name = "Post", Description = "Can create pilot registration")]
        PilotRegistration_Post = 0x1792,
        [Display(ShortName = "SecuredMobility", GroupName = "PilotRegistration", Name = "Put", Description = "Can update pilot registration")]
        PilotRegistration_Put = 0x1793,
        [Display(ShortName = "SecuredMobility", GroupName = "PilotRegistration", Name = "Delete", Description = "Can delete pilot registration")]
        PilotRegistration_Delete = 0x1794,
        [Display(ShortName = "Setups", GroupName = "PriceRegister", Name = "Get", Description = "Can view price register")]
        PriceRegister_Get = 0x1801,
        [Display(ShortName = "Setups", GroupName = "PriceRegister", Name = "Post", Description = "Can create price register")]
        PriceRegister_Post = 0x1802,
        [Display(ShortName = "Setups", GroupName = "PriceRegister", Name = "Put", Description = "Can update price register")]
        PriceRegister_Put = 0x1803,
        [Display(ShortName = "Setups", GroupName = "PriceRegister", Name = "Delete", Description = "Can delete price register")]
        PriceRegister_Delete = 0x1804,
        [Display(ShortName = "Setups", GroupName = "ProcessesRequiringApproval", Name = "Get", Description = "Can view processes requiring approval")]
        ProcessesRequiringApproval_Get = 0x1811,
        [Display(ShortName = "Setups", GroupName = "ProcessesRequiringApproval", Name = "Post", Description = "Can create processes requiring approval")]
        ProcessesRequiringApproval_Post = 0x1812,
        [Display(ShortName = "Setups", GroupName = "ProcessesRequiringApproval", Name = "Put", Description = "Can update processes requiring approval")]
        ProcessesRequiringApproval_Put = 0x1813,
        [Display(ShortName = "Setups", GroupName = "ProcessesRequiringApproval", Name = "Delete", Description = "Can delete processes requiring approval")]
        ProcessesRequiringApproval_Delete = 0x1814,
        [Display(ShortName = "Setups", GroupName = "ProfileEscalationLevel", Name = "Get", Description = "Can view profile escalation level")]
        ProfileEscalationLevel_Get = 0x1821,
        [Display(ShortName = "Setups", GroupName = "ProfileEscalationLevel", Name = "Post", Description = "Can create profile escalation level")]
        ProfileEscalationLevel_Post = 0x1822,
        [Display(ShortName = "Setups", GroupName = "ProfileEscalationLevel", Name = "Put", Description = "Can update profile escalation level")]
        ProfileEscalationLevel_Put = 0x1823,
        [Display(ShortName = "Setups", GroupName = "ProfileEscalationLevel", Name = "Delete", Description = "Can delete profile escalation level")]
        ProfileEscalationLevel_Delete = 0x1824,
        [Display(ShortName = "ProjectManagment", GroupName = "ProjectManagement", Name = "Get", Description = "Can view project management")]
        ProjectManagement_Get = 0x1831,
        [Display(ShortName = "ProjectManagment", GroupName = "ProjectManagement", Name = "Post", Description = "Can create project management")]
        ProjectManagement_Post = 0x1832,
        [Display(ShortName = "ProjectManagment", GroupName = "ProjectManagement", Name = "Put", Description = "Can update project management")]
        ProjectManagement_Put = 0x1833,
        [Display(ShortName = "ProjectManagment", GroupName = "ProjectManagement", Name = "Delete", Description = "Can delete project management")]
        ProjectManagement_Delete = 0x1834,
        [Display(ShortName = "LeadAdministration", GroupName = "Prospect", Name = "Get", Description = "Can view prospect")]
        Prospect_Get = 0x1841,
        [Display(ShortName = "LeadAdministration", GroupName = "Prospect", Name = "Post", Description = "Can create prospect")]
        Prospect_Post = 0x1842,
        [Display(ShortName = "LeadAdministration", GroupName = "Prospect", Name = "Put", Description = "Can update prospect")]
        Prospect_Put = 0x1843,
        [Display(ShortName = "LeadAdministration", GroupName = "Prospect", Name = "Delete", Description = "Can delete prospect")]
        Prospect_Delete = 0x1844,
        [Display(ShortName = "Setups", GroupName = "Region", Name = "Get", Description = "Can view region")]
        Region_Get = 0x2211,
        [Display(ShortName = "Setups", GroupName = "Region", Name = "Post", Description = "Can create region")]
        Region_Post = 0x2212,
        [Display(ShortName = "Setups", GroupName = "Region", Name = "Put", Description = "Can update region")]
        Region_Put = 0x2213,
        [Display(ShortName = "Setups", GroupName = "Region", Name = "Delete", Description = "Can delete region")]
        Region_Delete = 0x2214,
        [Display(ShortName = "Setups", GroupName = "Relationship", Name = "Get", Description = "Can view relationship")]
        Relationship_Get = 0x1851,
        [Display(ShortName = "Setups", GroupName = "Relationship", Name = "Post", Description = "Can create relationship")]
        Relationship_Post = 0x1852,
        [Display(ShortName = "Setups", GroupName = "Relationship", Name = "Put", Description = "Can update relationship")]
        Relationship_Put = 0x1853,
        [Display(ShortName = "Setups", GroupName = "Relationship", Name = "Delete", Description = "Can delete relationship")]
        Relationship_Delete = 0x1854,
        [Display(ShortName = "Setups", GroupName = "RequiredServiceDocument", Name = "Get", Description = "Can view required service document")]
        RequiredServiceDocument_Get = 0x1861,
        [Display(ShortName = "Setups", GroupName = "RequiredServiceDocument", Name = "Post", Description = "Can create required service document")]
        RequiredServiceDocument_Post = 0x1862,
        [Display(ShortName = "Setups", GroupName = "RequiredServiceDocument", Name = "Put", Description = "Can update required service document")]
        RequiredServiceDocument_Put = 0x1863,
        [Display(ShortName = "Setups", GroupName = "RequiredServiceDocument", Name = "Delete", Description = "Can delete required service document")]
        RequiredServiceDocument_Delete = 0x1864,
        [Display(ShortName = "Setups", GroupName = "RequredServiceQualificationElement", Name = "Get", Description = "Can view requred service qualification element")]
        RequredServiceQualificationElement_Get = 0x1871,
        [Display(ShortName = "Setups", GroupName = "RequredServiceQualificationElement", Name = "Post", Description = "Can create requred service qualification element")]
        RequredServiceQualificationElement_Post = 0x1872,
        [Display(ShortName = "Setups", GroupName = "RequredServiceQualificationElement", Name = "Put", Description = "Can update requred service qualification element")]
        RequredServiceQualificationElement_Put = 0x1873,
        [Display(ShortName = "Setups", GroupName = "RequredServiceQualificationElement", Name = "Delete", Description = "Can delete requred service qualification element")]
        RequredServiceQualificationElement_Delete = 0x1874,
        [Display(ShortName = "LeadAdministration", GroupName = "Sbuproportion", Name = "Get", Description = "Can view sbuproportion")]
        Sbuproportion_Get = 0x1881,
        [Display(ShortName = "LeadAdministration", GroupName = "Sbuproportion", Name = "Post", Description = "Can create sbuproportion")]
        Sbuproportion_Post = 0x1882,
        [Display(ShortName = "LeadAdministration", GroupName = "Sbuproportion", Name = "Put", Description = "Can update sbuproportion")]
        Sbuproportion_Put = 0x1883,
        [Display(ShortName = "LeadAdministration", GroupName = "Sbuproportion", Name = "Delete", Description = "Can delete sbuproportion")]
        Sbuproportion_Delete = 0x1884,
        [Display(ShortName = "Setups", GroupName = "ServiceAssignmentDetails", Name = "Get", Description = "Can view service assignment details")]
        ServiceAssignmentDetails_Get = 0x1891,
        [Display(ShortName = "Setups", GroupName = "ServiceAssignmentDetails", Name = "Post", Description = "Can create service assignment details")]
        ServiceAssignmentDetails_Post = 0x1892,
        [Display(ShortName = "Setups", GroupName = "ServiceAssignmentDetails", Name = "Put", Description = "Can update service assignment details")]
        ServiceAssignmentDetails_Put = 0x1893,
        [Display(ShortName = "Setups", GroupName = "ServiceAssignmentDetails", Name = "Delete", Description = "Can delete service assignment details")]
        ServiceAssignmentDetails_Delete = 0x1894,
        [Display(ShortName = "Setups", GroupName = "ServiceCategory", Name = "Get", Description = "Can view service category")]
        ServiceCategory_Get = 0x1901,
        [Display(ShortName = "Setups", GroupName = "ServiceCategory", Name = "Post", Description = "Can create service category")]
        ServiceCategory_Post = 0x1902,
        [Display(ShortName = "Setups", GroupName = "ServiceCategory", Name = "Put", Description = "Can update service category")]
        ServiceCategory_Put = 0x1903,
        [Display(ShortName = "Setups", GroupName = "ServiceCategory", Name = "Delete", Description = "Can delete service category")]
        ServiceCategory_Delete = 0x1904,
        [Display(ShortName = "Setups", GroupName = "ServiceCategoryTask", Name = "Get", Description = "Can view service category task")]
        ServiceCategoryTask_Get = 0x1911,
        [Display(ShortName = "Setups", GroupName = "ServiceCategoryTask", Name = "Post", Description = "Can create service category task")]
        ServiceCategoryTask_Post = 0x1912,
        [Display(ShortName = "Setups", GroupName = "ServiceCategoryTask", Name = "Put", Description = "Can update service category task")]
        ServiceCategoryTask_Put = 0x1913,
        [Display(ShortName = "Setups", GroupName = "ServiceCategoryTask", Name = "Delete", Description = "Can delete service category task")]
        ServiceCategoryTask_Delete = 0x1914,
        [Display(ShortName = "Setups", GroupName = "ServiceCustomerMigrations", Name = "Get", Description = "Can view service customer migrations")]
        ServiceCustomerMigrations_Get = 0x1921,
        [Display(ShortName = "Setups", GroupName = "ServiceCustomerMigrations", Name = "Post", Description = "Can create service customer migrations")]
        ServiceCustomerMigrations_Post = 0x1922,
        [Display(ShortName = "Setups", GroupName = "ServiceCustomerMigrations", Name = "Put", Description = "Can update service customer migrations")]
        ServiceCustomerMigrations_Put = 0x1923,
        [Display(ShortName = "Setups", GroupName = "ServiceCustomerMigrations", Name = "Delete", Description = "Can delete service customer migrations")]
        ServiceCustomerMigrations_Delete = 0x1924,
        [Display(ShortName = "Setups", GroupName = "ServiceGroup", Name = "Get", Description = "Can view service group")]
        ServiceGroup_Get = 0x1931,
        [Display(ShortName = "Setups", GroupName = "ServiceGroup", Name = "Post", Description = "Can create service group")]
        ServiceGroup_Post = 0x1932,
        [Display(ShortName = "Setups", GroupName = "ServiceGroup", Name = "Put", Description = "Can update service group")]
        ServiceGroup_Put = 0x1933,
        [Display(ShortName = "Setups", GroupName = "ServiceGroup", Name = "Delete", Description = "Can delete service group")]
        ServiceGroup_Delete = 0x1934,
        [Display(ShortName = "Setups", GroupName = "ServicePricing", Name = "Get", Description = "Can view service pricing")]
        ServicePricing_Get = 0x1941,
        [Display(ShortName = "Setups", GroupName = "ServicePricing", Name = "Post", Description = "Can create service pricing")]
        ServicePricing_Post = 0x1942,
        [Display(ShortName = "Setups", GroupName = "ServicePricing", Name = "Put", Description = "Can update service pricing")]
        ServicePricing_Put = 0x1943,
        [Display(ShortName = "Setups", GroupName = "ServicePricing", Name = "Delete", Description = "Can delete service pricing")]
        ServicePricing_Delete = 0x1944,
        [Display(ShortName = "Setups", GroupName = "ServiceQualification", Name = "Get", Description = "Can view service qualification")]
        ServiceQualification_Get = 0x1951,
        [Display(ShortName = "Setups", GroupName = "ServiceQualification", Name = "Post", Description = "Can create service qualification")]
        ServiceQualification_Post = 0x1952,
        [Display(ShortName = "Setups", GroupName = "ServiceQualification", Name = "Put", Description = "Can update service qualification")]
        ServiceQualification_Put = 0x1953,
        [Display(ShortName = "Setups", GroupName = "ServiceQualification", Name = "Delete", Description = "Can delete service qualification")]
        ServiceQualification_Delete = 0x1954,
        [Display(ShortName = "Setups", GroupName = "ServiceRegistration", Name = "Get", Description = "Can view service registration")]
        ServiceRegistration_Get = 0x1961,
        [Display(ShortName = "Setups", GroupName = "ServiceRegistration", Name = "Post", Description = "Can create service registration")]
        ServiceRegistration_Post = 0x1962,
        [Display(ShortName = "Setups", GroupName = "ServiceRegistration", Name = "Put", Description = "Can update service registration")]
        ServiceRegistration_Put = 0x1963,
        [Display(ShortName = "Setups", GroupName = "ServiceRegistration", Name = "Delete", Description = "Can delete service registration")]
        ServiceRegistration_Delete = 0x1964,
        [Display(ShortName = "Setups", GroupName = "ServiceRelationships", Name = "Get", Description = "Can view service relationships")]
        ServiceRelationships_Get = 0x1971,
        [Display(ShortName = "Setups", GroupName = "ServiceRelationships", Name = "Post", Description = "Can create service relationships")]
        ServiceRelationships_Post = 0x1972,
        [Display(ShortName = "Setups", GroupName = "ServiceRelationships", Name = "Put", Description = "Can update service relationships")]
        ServiceRelationships_Put = 0x1973,
        [Display(ShortName = "Setups", GroupName = "ServiceRelationships", Name = "Delete", Description = "Can delete service relationships")]
        ServiceRelationships_Delete = 0x1974,
        [Display(ShortName = "Setups", GroupName = "Services", Name = "Get", Description = "Can view services")]
        Services_Get = 0x1981,
        [Display(ShortName = "Setups", GroupName = "Services", Name = "Post", Description = "Can create services")]
        Services_Post = 0x1982,
        [Display(ShortName = "Setups", GroupName = "Services", Name = "Put", Description = "Can update services")]
        Services_Put = 0x1983,
        [Display(ShortName = "Setups", GroupName = "Services", Name = "Delete", Description = "Can delete services")]
        Services_Delete = 0x1984,
        [Display(ShortName = "Setups", GroupName = "ServiceTaskDeliverable", Name = "Get", Description = "Can view service task deliverable")]
        ServiceTaskDeliverable_Get = 0x1991,
        [Display(ShortName = "Setups", GroupName = "ServiceTaskDeliverable", Name = "Post", Description = "Can create service task deliverable")]
        ServiceTaskDeliverable_Post = 0x1992,
        [Display(ShortName = "Setups", GroupName = "ServiceTaskDeliverable", Name = "Put", Description = "Can update service task deliverable")]
        ServiceTaskDeliverable_Put = 0x1993,
        [Display(ShortName = "Setups", GroupName = "ServiceTaskDeliverable", Name = "Delete", Description = "Can delete service task deliverable")]
        ServiceTaskDeliverable_Delete = 0x1994,
        [Display(ShortName = "Setups", GroupName = "ServiceType", Name = "Get", Description = "Can view service type")]
        ServiceType_Get = 0x2001,
        [Display(ShortName = "Setups", GroupName = "ServiceType", Name = "Post", Description = "Can create service type")]
        ServiceType_Post = 0x2002,
        [Display(ShortName = "Setups", GroupName = "ServiceType", Name = "Put", Description = "Can update service type")]
        ServiceType_Put = 0x2003,
        [Display(ShortName = "Setups", GroupName = "ServiceType", Name = "Delete", Description = "Can delete service type")]
        ServiceType_Delete = 0x2004,
        [Display(ShortName = "SecuredMobility", GroupName = "SMORouteAndRegion", Name = "Get", Description = "Can view smo route and region")]
        SMORouteAndRegion_Get = 0x2011,
        [Display(ShortName = "SecuredMobility", GroupName = "SMORouteAndRegion", Name = "Post", Description = "Can create smo route and region")]
        SMORouteAndRegion_Post = 0x2012,
        [Display(ShortName = "SecuredMobility", GroupName = "SMORouteAndRegion", Name = "Put", Description = "Can update smo route and region")]
        SMORouteAndRegion_Put = 0x2013,
        [Display(ShortName = "SecuredMobility", GroupName = "SMORouteAndRegion", Name = "Delete", Description = "Can delete smo route and region")]
        SMORouteAndRegion_Delete = 0x2014,
        [Display(ShortName = "Setups", GroupName = "StandardSlaforOperatingEntity", Name = "Get", Description = "Can view standard slafor operating entity")]
        StandardSlaforOperatingEntity_Get = 0x2021,
        [Display(ShortName = "Setups", GroupName = "StandardSlaforOperatingEntity", Name = "Post", Description = "Can create standard slafor operating entity")]
        StandardSlaforOperatingEntity_Post = 0x2022,
        [Display(ShortName = "Setups", GroupName = "StandardSlaforOperatingEntity", Name = "Put", Description = "Can update standard slafor operating entity")]
        StandardSlaforOperatingEntity_Put = 0x2023,
        [Display(ShortName = "Setups", GroupName = "StandardSlaforOperatingEntity", Name = "Delete", Description = "Can delete standard slafor operating entity")]
        StandardSlaforOperatingEntity_Delete = 0x2024,
        [Display(ShortName = "Setups", GroupName = "State", Name = "Get", Description = "Can view state")]
        State_Get = 0x2031,
        [Display(ShortName = "Setups", GroupName = "State", Name = "Post", Description = "Can create state")]
        State_Post = 0x2032,
        [Display(ShortName = "Setups", GroupName = "State", Name = "Put", Description = "Can update state")]
        State_Put = 0x2033,
        [Display(ShortName = "Setups", GroupName = "State", Name = "Delete", Description = "Can delete state")]
        State_Delete = 0x2034,
        [Display(ShortName = "Setups", GroupName = "StrategicBusinessUnit", Name = "Get", Description = "Can view strategic business unit")]
        StrategicBusinessUnit_Get = 0x2041,
        [Display(ShortName = "Setups", GroupName = "StrategicBusinessUnit", Name = "Post", Description = "Can create strategic business unit")]
        StrategicBusinessUnit_Post = 0x2042,
        [Display(ShortName = "Setups", GroupName = "StrategicBusinessUnit", Name = "Put", Description = "Can update strategic business unit")]
        StrategicBusinessUnit_Put = 0x2043,
        [Display(ShortName = "Setups", GroupName = "StrategicBusinessUnit", Name = "Delete", Description = "Can delete strategic business unit")]
        StrategicBusinessUnit_Delete = 0x2044,
        [Display(ShortName = "Supplier", GroupName = "SupplierCategory", Name = "Get", Description = "Can view supplier category")]
        SupplierCategory_Get = 0x2051,
        [Display(ShortName = "Supplier", GroupName = "SupplierCategory", Name = "Post", Description = "Can create supplier category")]
        SupplierCategory_Post = 0x2052,
        [Display(ShortName = "Supplier", GroupName = "SupplierCategory", Name = "Put", Description = "Can update supplier category")]
        SupplierCategory_Put = 0x2053,
        [Display(ShortName = "Supplier", GroupName = "SupplierCategory", Name = "Delete", Description = "Can delete supplier category")]
        SupplierCategory_Delete = 0x2054,
        [Display(ShortName = "Supplier", GroupName = "Supplier", Name = "Get", Description = "Can view supplier")]
        Supplier_Get = 0x2061,
        [Display(ShortName = "Supplier", GroupName = "Supplier", Name = "Post", Description = "Can create supplier")]
        Supplier_Post = 0x2062,
        [Display(ShortName = "Supplier", GroupName = "Supplier", Name = "Put", Description = "Can update supplier")]
        Supplier_Put = 0x2063,
        [Display(ShortName = "Supplier", GroupName = "Supplier", Name = "Delete", Description = "Can delete supplier")]
        Supplier_Delete = 0x2064,
        [Display(ShortName = "Supplier", GroupName = "SupplierService", Name = "Get", Description = "Can view supplier service")]
        SupplierService_Get = 0x2071,
        [Display(ShortName = "Supplier", GroupName = "SupplierService", Name = "Post", Description = "Can create supplier service")]
        SupplierService_Post = 0x2072,
        [Display(ShortName = "Supplier", GroupName = "SupplierService", Name = "Put", Description = "Can update supplier service")]
        SupplierService_Put = 0x2073,
        [Display(ShortName = "Supplier", GroupName = "SupplierService", Name = "Delete", Description = "Can delete supplier service")]
        SupplierService_Delete = 0x2074,
        [Display(ShortName = "LeadAdministration", GroupName = "Suspect", Name = "Get", Description = "Can view suspect")]
        Suspect_Get = 0x2081,
        [Display(ShortName = "LeadAdministration", GroupName = "Suspect", Name = "Post", Description = "Can create suspect")]
        Suspect_Post = 0x2082,
        [Display(ShortName = "LeadAdministration", GroupName = "Suspect", Name = "Put", Description = "Can update suspect")]
        Suspect_Put = 0x2083,
        [Display(ShortName = "LeadAdministration", GroupName = "Suspect", Name = "Delete", Description = "Can delete suspect")]
        Suspect_Delete = 0x2084,
        [Display(ShortName = "LeadAdministration", GroupName = "SuspectQualification", Name = "Get", Description = "Can view suspect qualification")]
        SuspectQualification_Get = 0x2091,
        [Display(ShortName = "LeadAdministration", GroupName = "SuspectQualification", Name = "Post", Description = "Can create suspect qualification")]
        SuspectQualification_Post = 0x2092,
        [Display(ShortName = "LeadAdministration", GroupName = "SuspectQualification", Name = "Put", Description = "Can update suspect qualification")]
        SuspectQualification_Put = 0x2093,
        [Display(ShortName = "LeadAdministration", GroupName = "SuspectQualification", Name = "Delete", Description = "Can delete suspect qualification")]
        SuspectQualification_Delete = 0x2094,
        [Display(ShortName = "Setups", GroupName = "Target", Name = "Get", Description = "Can view target")]
        Target_Get = 0x2101,
        [Display(ShortName = "Setups", GroupName = "Target", Name = "Post", Description = "Can create target")]
        Target_Post = 0x2102,
        [Display(ShortName = "Setups", GroupName = "Target", Name = "Put", Description = "Can update target")]
        Target_Put = 0x2103,
        [Display(ShortName = "Setups", GroupName = "Target", Name = "Delete", Description = "Can delete target")]
        Target_Delete = 0x2104,
        [Display(ShortName = "Setups", GroupName = "TypesForServiceAssignment", Name = "Get", Description = "Can view types for service assignment")]
        TypesForServiceAssignment_Get = 0x2111,
        [Display(ShortName = "Setups", GroupName = "TypesForServiceAssignment", Name = "Post", Description = "Can create types for service assignment")]
        TypesForServiceAssignment_Post = 0x2112,
        [Display(ShortName = "Setups", GroupName = "TypesForServiceAssignment", Name = "Put", Description = "Can update types for service assignment")]
        TypesForServiceAssignment_Put = 0x2113,
        [Display(ShortName = "Setups", GroupName = "TypesForServiceAssignment", Name = "Delete", Description = "Can delete types for service assignment")]
        TypesForServiceAssignment_Delete = 0x2114,
        [Display(ShortName = "Profile", GroupName = "User", Name = "Get", Description = "Can view user")]
        User_Get = 0x2131,
        [Display(ShortName = "Profile", GroupName = "User", Name = "Post", Description = "Can create user")]
        User_Post = 0x2132,
        [Display(ShortName = "Profile", GroupName = "User", Name = "Put", Description = "Can update user")]
        User_Put = 0x2133,
        [Display(ShortName = "Profile", GroupName = "User", Name = "Delete", Description = "Can delete user")]
        User_Delete = 0x2134,
        [Display(ShortName = "SecuredMobility", GroupName = "VehicleRegistration", Name = "Get", Description = "Can view vehicle registration")]
        VehicleRegistration_Get = 0x2141,
        [Display(ShortName = "SecuredMobility", GroupName = "VehicleRegistration", Name = "Post", Description = "Can create vehicle registration")]
        VehicleRegistration_Post = 0x2142,
        [Display(ShortName = "SecuredMobility", GroupName = "VehicleRegistration", Name = "Put", Description = "Can update vehicle registration")]
        VehicleRegistration_Put = 0x2143,
        [Display(ShortName = "SecuredMobility", GroupName = "VehicleRegistration", Name = "Delete", Description = "Can delete vehicle registration")]
        VehicleRegistration_Delete = 0x2144,
        [Display(ShortName = "SecuredMobility", GroupName = "Vehicles", Name = "Get", Description = "Can view vehicles")]
        Vehicles_Get = 0x2151,
        [Display(ShortName = "SecuredMobility", GroupName = "Vehicles", Name = "Post", Description = "Can create vehicles")]
        Vehicles_Post = 0x2152,
        [Display(ShortName = "SecuredMobility", GroupName = "Vehicles", Name = "Put", Description = "Can update vehicles")]
        Vehicles_Put = 0x2153,
        [Display(ShortName = "SecuredMobility", GroupName = "Vehicles", Name = "Delete", Description = "Can delete vehicles")]
        Vehicles_Delete = 0x2154,
        [Display(ShortName = "Setups", GroupName = "Zone", Name = "Get", Description = "Can view zone")]
        Zone_Get = 0x2161,
        [Display(ShortName = "Setups", GroupName = "Zone", Name = "Post", Description = "Can create zone")]
        Zone_Post = 0x2162,
        [Display(ShortName = "Setups", GroupName = "Zone", Name = "Put", Description = "Can update zone")]
        Zone_Put = 0x2163,
        [Display(ShortName = "Setups", GroupName = "Zone", Name = "Delete", Description = "Can delete zone")]
        Zone_Delete = 0x2164,
        [Display(ShortName = "RolesManagement", GroupName = "Role", Name = "Get", Description = "Can view role")]
        Role_Get = 0x1351,
        [Display(ShortName = "RolesManagement", GroupName = "Role", Name = "Post", Description = "Can create role")]
        Role_Post = 0x1352,
        [Display(ShortName = "RolesManagement", GroupName = "Role", Name = "Put", Description = "Can update role")]
        Role_Put = 0x1353,
        [Display(ShortName = "RolesManagement", GroupName = "Role", Name = "Delete", Description = "Can delete role")]
        Role_Delete = 0x1354,
        [Display(ShortName = "LeadAdministration", GroupName = "Contract", Name = "Get", Description = "Can view contract")]
        Contract_Get = 0x1111,
        [Display(ShortName = "LeadAdministration", GroupName = "Contract", Name = "Post", Description = "Can create contract")]
        Contract_Post = 0x1112,
        [Display(ShortName = "LeadAdministration", GroupName = "Contract", Name = "Put", Description = "Can update contract")]
        Contract_Put = 0x1113,
        [Display(ShortName = "LeadAdministration", GroupName = "Contract", Name = "Delete", Description = "Can delete contract")]
        Contract_Delete = 0x1114,
        [Display(ShortName = "LeadAdministration", GroupName = "ContractService", Name = "Get", Description = "Can view contract service")]
        ContractService_Get = 0x1121,
        [Display(ShortName = "LeadAdministration", GroupName = "ContractService", Name = "Post", Description = "Can create contract service")]
        ContractService_Post = 0x1122,
        [Display(ShortName = "LeadAdministration", GroupName = "ContractService", Name = "Put", Description = "Can update contract service")]
        ContractService_Put = 0x1123,
        [Display(ShortName = "LeadAdministration", GroupName = "ContractService", Name = "Delete", Description = "Can delete contract service")]
        ContractService_Delete = 0x1124,
        [Display(ShortName = "LeadAdministration", GroupName = "Customer", Name = "Get", Description = "Can view customer")]
        Customer_Get = 0x1131,
        [Display(ShortName = "LeadAdministration", GroupName = "Customer", Name = "Post", Description = "Can create customer")]
        Customer_Post = 0x1132,
        [Display(ShortName = "LeadAdministration", GroupName = "Customer", Name = "Put", Description = "Can update customer")]
        Customer_Put = 0x1133,
        [Display(ShortName = "LeadAdministration", GroupName = "Customer", Name = "Delete", Description = "Can delete customer")]
        Customer_Delete = 0x1134,
        [Display(ShortName = "LeadAdministration", GroupName = "CustomerDivision", Name = "Get", Description = "Can view customer division")]
        CustomerDivision_Get = 0x1141,
        [Display(ShortName = "LeadAdministration", GroupName = "CustomerDivision", Name = "Post", Description = "Can create customer division")]
        CustomerDivision_Post = 0x1142,
        [Display(ShortName = "LeadAdministration", GroupName = "CustomerDivision", Name = "Put", Description = "Can update customer division")]
        CustomerDivision_Put = 0x1143,
        [Display(ShortName = "LeadAdministration", GroupName = "CustomerDivision", Name = "Delete", Description = "Can delete customer division")]
        CustomerDivision_Delete = 0x1144,
        [Display(ShortName = "Finance", GroupName = "FinanceVoucherType", Name = "Get", Description = "Can view finance voucher type")]
        FinanceVoucherType_Get = 0x1181,
        [Display(ShortName = "Finance", GroupName = "FinanceVoucherType", Name = "Post", Description = "Can create finance voucher type")]
        FinanceVoucherType_Post = 0x1182,
        [Display(ShortName = "Finance", GroupName = "FinanceVoucherType", Name = "Put", Description = "Can update finance voucher type")]
        FinanceVoucherType_Put = 0x1183,
        [Display(ShortName = "Finance", GroupName = "FinanceVoucherType", Name = "Delete", Description = "Can delete finance voucher type")]
        FinanceVoucherType_Delete = 0x1184,
        [Display(ShortName = "LeadAdministration", GroupName = "LeadContact", Name = "Get", Description = "Can view lead contact")]
        LeadContact_Get = 0x1191,
        [Display(ShortName = "LeadAdministration", GroupName = "LeadContact", Name = "Post", Description = "Can create lead contact")]
        LeadContact_Post = 0x1192,
        [Display(ShortName = "LeadAdministration", GroupName = "LeadContact", Name = "Put", Description = "Can update lead contact")]
        LeadContact_Put = 0x1193,
        [Display(ShortName = "LeadAdministration", GroupName = "LeadContact", Name = "Delete", Description = "Can delete lead contact")]
        LeadContact_Delete = 0x1194,
        [Display(ShortName = "LeadAdministration", GroupName = "Lead", Name = "Get", Description = "Can view lead")]
        Lead_Get = 0x1201,
        [Display(ShortName = "LeadAdministration", GroupName = "Lead", Name = "Post", Description = "Can create lead")]
        Lead_Post = 0x1202,
        [Display(ShortName = "LeadAdministration", GroupName = "Lead", Name = "Put", Description = "Can update lead")]
        Lead_Put = 0x1203,
        [Display(ShortName = "LeadAdministration", GroupName = "Lead", Name = "Delete", Description = "Can delete lead")]
        Lead_Delete = 0x1204,
        [Display(ShortName = "LeadAdministration", GroupName = "LeadDivisionContact", Name = "Get", Description = "Can view lead division contact")]
        LeadDivisionContact_Get = 0x1211,
        [Display(ShortName = "LeadAdministration", GroupName = "LeadDivisionContact", Name = "Post", Description = "Can create lead division contact")]
        LeadDivisionContact_Post = 0x1212,
        [Display(ShortName = "LeadAdministration", GroupName = "LeadDivisionContact", Name = "Put", Description = "Can update lead division contact")]
        LeadDivisionContact_Put = 0x1213,
        [Display(ShortName = "LeadAdministration", GroupName = "LeadDivisionContact", Name = "Delete", Description = "Can delete lead division contact")]
        LeadDivisionContact_Delete = 0x1214,
        [Display(ShortName = "LeadAdministration", GroupName = "LeadDivisionKeyPerson", Name = "Get", Description = "Can view lead division key person")]
        LeadDivisionKeyPerson_Get = 0x1231,
        [Display(ShortName = "LeadAdministration", GroupName = "LeadDivisionKeyPerson", Name = "Post", Description = "Can create lead division key person")]
        LeadDivisionKeyPerson_Post = 0x1232,
        [Display(ShortName = "LeadAdministration", GroupName = "LeadDivisionKeyPerson", Name = "Put", Description = "Can update lead division key person")]
        LeadDivisionKeyPerson_Put = 0x1233,
        [Display(ShortName = "LeadAdministration", GroupName = "LeadDivisionKeyPerson", Name = "Delete", Description = "Can delete lead division key person")]
        LeadDivisionKeyPerson_Delete = 0x1234,
        [Display(ShortName = "LeadAdministration", GroupName = "LeadKeyPerson", Name = "Get", Description = "Can view lead key person")]
        LeadKeyPerson_Get = 0x1251,
        [Display(ShortName = "LeadAdministration", GroupName = "LeadKeyPerson", Name = "Post", Description = "Can create lead key person")]
        LeadKeyPerson_Post = 0x1252,
        [Display(ShortName = "LeadAdministration", GroupName = "LeadKeyPerson", Name = "Put", Description = "Can update lead key person")]
        LeadKeyPerson_Put = 0x1253,
        [Display(ShortName = "LeadAdministration", GroupName = "LeadKeyPerson", Name = "Delete", Description = "Can delete lead key person")]
        LeadKeyPerson_Delete = 0x1254,
        [Display(ShortName = "Setups", GroupName = "LeadOrigin", Name = "Get", Description = "Can view lead origin")]
        LeadOrigin_Get = 0x2221,
        [Display(ShortName = "Setups", GroupName = "LeadOrigin", Name = "Post", Description = "Can create lead origin")]
        LeadOrigin_Post = 0x2222,
        [Display(ShortName = "Setups", GroupName = "LeadOrigin", Name = "Put", Description = "Can update lead origin")]
        LeadOrigin_Put = 0x2223,
        [Display(ShortName = "Setups", GroupName = "LeadOrigin", Name = "Delete", Description = "Can delete lead origin")]
        LeadOrigin_Delete = 0x2224,
        [Display(ShortName = "LeadAdministration", GroupName = "SBUContractServiceProportion", Name = "Get", Description = "Can view sbu contract service proportion")]
        SBUContractServiceProportion_Get = 0x1321,
        [Display(ShortName = "LeadAdministration", GroupName = "SBUContractServiceProportion", Name = "Post", Description = "Can create sbu contract service proportion")]
        SBUContractServiceProportion_Post = 0x1322,
        [Display(ShortName = "LeadAdministration", GroupName = "SBUContractServiceProportion", Name = "Put", Description = "Can update sbu contract service proportion")]
        SBUContractServiceProportion_Put = 0x1323,
        [Display(ShortName = "LeadAdministration", GroupName = "SBUContractServiceProportion", Name = "Delete", Description = "Can delete sbu contract service proportion")]
        SBUContractServiceProportion_Delete = 0x1324,
        [Display(ShortName = "LeadAdministration", GroupName = "SBUQuoteServiceProportion", Name = "Get", Description = "Can view sbu quote service proportion")]
        SBUQuoteServiceProportion_Get = 0x1331,
        [Display(ShortName = "LeadAdministration", GroupName = "SBUQuoteServiceProportion", Name = "Post", Description = "Can create sbu quote service proportion")]
        SBUQuoteServiceProportion_Post = 0x1332,
        [Display(ShortName = "LeadAdministration", GroupName = "SBUQuoteServiceProportion", Name = "Put", Description = "Can update sbu quote service proportion")]
        SBUQuoteServiceProportion_Put = 0x1333,
        [Display(ShortName = "LeadAdministration", GroupName = "SBUQuoteServiceProportion", Name = "Delete", Description = "Can delete sbu quote service proportion")]
        SBUQuoteServiceProportion_Delete = 0x1334,
        [Display(ShortName = "Finance", GroupName = "AccountClass", Name = "Get", Description = "Can view account class")]
        AccountClass_Get = 0x1011,
        [Display(ShortName = "Finance", GroupName = "AccountClass", Name = "Post", Description = "Can create account class")]
        AccountClass_Post = 0x1012,
        [Display(ShortName = "Finance", GroupName = "AccountClass", Name = "Put", Description = "Can update account class")]
        AccountClass_Put = 0x1013,
        [Display(ShortName = "Finance", GroupName = "AccountClass", Name = "Delete", Description = "Can delete account class")]
        AccountClass_Delete = 0x1014,
        [Display(ShortName = "Finance", GroupName = "Account", Name = "Get", Description = "Can view account")]
        Account_Get = 0x1021,
        [Display(ShortName = "Finance", GroupName = "Account", Name = "Post", Description = "Can create account")]
        Account_Post = 0x1022,
        [Display(ShortName = "Finance", GroupName = "Account", Name = "Put", Description = "Can update account")]
        Account_Put = 0x1023,
        [Display(ShortName = "Finance", GroupName = "Account", Name = "Delete", Description = "Can delete account")]
        Account_Delete = 0x1024,
        [Display(ShortName = "Finance", GroupName = "AccountDetail", Name = "Get", Description = "Can view account detail")]
        AccountDetail_Get = 0x1031,
        [Display(ShortName = "Finance", GroupName = "AccountDetail", Name = "Post", Description = "Can create account detail")]
        AccountDetail_Post = 0x1032,
        [Display(ShortName = "Finance", GroupName = "AccountDetail", Name = "Put", Description = "Can update account detail")]
        AccountDetail_Put = 0x1033,
        [Display(ShortName = "Finance", GroupName = "AccountDetail", Name = "Delete", Description = "Can delete account detail")]
        AccountDetail_Delete = 0x1034,
        [Display(ShortName = "Finance", GroupName = "AccountMaster", Name = "Get", Description = "Can view account master")]
        AccountMaster_Get = 0x1041,
        [Display(ShortName = "Finance", GroupName = "AccountMaster", Name = "Post", Description = "Can create account master")]
        AccountMaster_Post = 0x1042,
        [Display(ShortName = "Finance", GroupName = "AccountMaster", Name = "Put", Description = "Can update account master")]
        AccountMaster_Put = 0x1043,
        [Display(ShortName = "Finance", GroupName = "AccountMaster", Name = "Delete", Description = "Can delete account master")]
        AccountMaster_Delete = 0x1044,
        [Display(ShortName = "Finance", GroupName = "ControlAccount", Name = "Get", Description = "Can view control account")]
        ControlAccount_Get = 0x1051,
        [Display(ShortName = "Finance", GroupName = "ControlAccount", Name = "Post", Description = "Can create control account")]
        ControlAccount_Post = 0x1052,
        [Display(ShortName = "Finance", GroupName = "ControlAccount", Name = "Put", Description = "Can update control account")]
        ControlAccount_Put = 0x1053,
        [Display(ShortName = "Finance", GroupName = "ControlAccount", Name = "Delete", Description = "Can delete control account")]
        ControlAccount_Delete = 0x1054,
        [Display(ShortName = "Finance", GroupName = "Invoice", Name = "Get", Description = "Can view invoice")]
        Invoice_Get = 0x1061,
        [Display(ShortName = "Finance", GroupName = "Invoice", Name = "Post", Description = "Can create invoice")]
        Invoice_Post = 0x1062,
        [Display(ShortName = "Finance", GroupName = "Invoice", Name = "Put", Description = "Can update invoice")]
        Invoice_Put = 0x1063,
        [Display(ShortName = "Finance", GroupName = "Invoice", Name = "Delete", Description = "Can delete invoice")]
        Invoice_Delete = 0x1064,
        [Display(ShortName = "Finance", GroupName = "Receipt", Name = "Get", Description = "Can view receipt")]
        Receipt_Get = 0x1071,
        [Display(ShortName = "Finance", GroupName = "Receipt", Name = "Post", Description = "Can create receipt")]
        Receipt_Post = 0x1072,
        [Display(ShortName = "Finance", GroupName = "Receipt", Name = "Put", Description = "Can update receipt")]
        Receipt_Put = 0x1073,
        [Display(ShortName = "Finance", GroupName = "Receipt", Name = "Delete", Description = "Can delete receipt")]
        Receipt_Delete = 0x1074,

        [Display(ShortName = "Employment", GroupName = "Employment", Name = "Get", Description = "Can view employment")]
        Employment_Get = 0x6,
        [Display(ShortName = "Employment", GroupName = "Employment", Name = "Post", Description = "Can create employment")]
        Employment_Post = 0x7,
        [Display(ShortName = "Employment", GroupName = "Employment", Name = "Put", Description = "Can update employment")]
        Employment_Put = 0x8,
        [Display(ShortName = "Employment", GroupName = "Employment", Name = "Delete", Description = "Can delete employment")]
        Employment_Delete = 0x9,
        [Display(ShortName = "InventoryManagement", GroupName = "InventoryConfirmation", Name = "Get", Description = "Can view inventory confirmation")]
        InventoryConfirmation_Get = 0x11,
        [Display(ShortName = "InventoryManagement", GroupName = "InventoryConfirmation", Name = "Post", Description = "Can create inventory confirmation")]
        InventoryConfirmation_Post = 0x12,
        [Display(ShortName = "InventoryManagement", GroupName = "InventoryConfirmation", Name = "Put", Description = "Can update inventory confirmation")]
        InventoryConfirmation_Put = 0x13,
        [Display(ShortName = "InventoryManagement", GroupName = "InventoryConfirmation", Name = "Delete", Description = "Can delete inventory confirmation")]
        InventoryConfirmation_Delete = 0x14,
        [Display(ShortName = "InventoryManagement", GroupName = "Inventory", Name = "Get", Description = "Can view inventory")]
        Inventory_Get = 0x16,
        [Display(ShortName = "InventoryManagement", GroupName = "Inventory", Name = "Post", Description = "Can create inventory")]
        Inventory_Post = 0x17,
        [Display(ShortName = "InventoryManagement", GroupName = "Inventory", Name = "Put", Description = "Can update inventory")]
        Inventory_Put = 0x18,
        [Display(ShortName = "InventoryManagement", GroupName = "Inventory", Name = "Delete", Description = "Can delete inventory")]
        Inventory_Delete = 0x19,
        [Display(ShortName = "InventoryManagement", GroupName = "InventoryPolicy", Name = "Get", Description = "Can view inventory policy")]
        InventoryPolicy_Get = 0x21,
        [Display(ShortName = "InventoryManagement", GroupName = "InventoryPolicy", Name = "Post", Description = "Can create inventory policy")]
        InventoryPolicy_Post = 0x22,
        [Display(ShortName = "InventoryManagement", GroupName = "InventoryPolicy", Name = "Put", Description = "Can update inventory policy")]
        InventoryPolicy_Put = 0x23,
        [Display(ShortName = "InventoryManagement", GroupName = "InventoryPolicy", Name = "Delete", Description = "Can delete inventory policy")]
        InventoryPolicy_Delete = 0x24,
        [Display(ShortName = "InventoryManagement", GroupName = "InventoryRequest", Name = "Get", Description = "Can view inventory request")]
        InventoryRequest_Get = 0x26,
        [Display(ShortName = "InventoryManagement", GroupName = "InventoryRequest", Name = "Post", Description = "Can create inventory request")]
        InventoryRequest_Post = 0x27,
        [Display(ShortName = "InventoryManagement", GroupName = "InventoryRequest", Name = "Put", Description = "Can update inventory request")]
        InventoryRequest_Put = 0x28,
        [Display(ShortName = "InventoryManagement", GroupName = "InventoryRequest", Name = "Delete", Description = "Can delete inventory request")]
        InventoryRequest_Delete = 0x29,
        [Display(ShortName = "InventoryManagement", GroupName = "InventoryStore", Name = "Get", Description = "Can view inventory store")]
        InventoryStore_Get = 0x31,
        [Display(ShortName = "InventoryManagement", GroupName = "InventoryStore", Name = "Post", Description = "Can create inventory store")]
        InventoryStore_Post = 0x32,
        [Display(ShortName = "InventoryManagement", GroupName = "InventoryStore", Name = "Put", Description = "Can update inventory store")]
        InventoryStore_Put = 0x33,
        [Display(ShortName = "InventoryManagement", GroupName = "InventoryStore", Name = "Delete", Description = "Can delete inventory store")]
        InventoryStore_Delete = 0x34,
        [Display(ShortName = "PayrollManagement", GroupName = "Payroll", Name = "Get", Description = "Can view payroll")]
        Payroll_Get = 0x36,
        [Display(ShortName = "PayrollManagement", GroupName = "Payroll", Name = "Post", Description = "Can create payroll")]
        Payroll_Post = 0x37,
        [Display(ShortName = "PayrollManagement", GroupName = "Payroll", Name = "Put", Description = "Can update payroll")]
        Payroll_Put = 0x38,
        [Display(ShortName = "PayrollManagement", GroupName = "Payroll", Name = "Delete", Description = "Can delete payroll")]
        Payroll_Delete = 0x39,
        [Display(ShortName = "Employment", GroupName = "Premployment", Name = "Get", Description = "Can view premployment")]
        Premployment_Get = 0x41,
        [Display(ShortName = "Employment", GroupName = "Premployment", Name = "Post", Description = "Can create premployment")]
        Premployment_Post = 0x42,
        [Display(ShortName = "Employment", GroupName = "Premployment", Name = "Put", Description = "Can update premployment")]
        Premployment_Put = 0x43,
        [Display(ShortName = "Employment", GroupName = "Premployment", Name = "Delete", Description = "Can delete premployment")]
        Premployment_Delete = 0x44,
        [Display(ShortName = "Employment", GroupName = "JobTitle", Name = "Get", Description = "Can view job title")]
        JobTitle_Get = 0x51,
        [Display(ShortName = "Employment", GroupName = "JobTitle", Name = "Post", Description = "Can create job title")]
        JobTitle_Post = 0x52,
        [Display(ShortName = "Employment", GroupName = "JobTitle", Name = "Put", Description = "Can update job title")]
        JobTitle_Put = 0x53,
        [Display(ShortName = "Employment", GroupName = "JobTitle", Name = "Delete", Description = "Can delete job title")]
        JobTitle_Delete = 0x54,
        [Display(ShortName = "Employment", GroupName = "JobTypes", Name = "Get", Description = "Can view job types")]
        JobTypes_Get = 0x56,
        [Display(ShortName = "Employment", GroupName = "JobTypes", Name = "Post", Description = "Can create job types")]
        JobTypes_Post = 0x57,
        [Display(ShortName = "Employment", GroupName = "JobTypes", Name = "Put", Description = "Can update job types")]
        JobTypes_Put = 0x58,
        [Display(ShortName = "Employment", GroupName = "JobTypes", Name = "Delete", Description = "Can delete job types")]
        JobTypes_Delete = 0x59,
        [Display(ShortName = "SecuredMobility", GroupName = "OnlineLocationFavorite", Name = "Get", Description = "Can view online location favorite")]
        OnlineLocationFavorite_Get = 0x2251,
        [Display(ShortName = "SecuredMobility", GroupName = "OnlineLocationFavorite", Name = "Post", Description = "Can create online location favorite")]
        OnlineLocationFavorite_Post = 0x2252,
        [Display(ShortName = "SecuredMobility", GroupName = "OnlineLocationFavorite", Name = "Put", Description = "Can update online location favorite")]
        OnlineLocationFavorite_Put = 0x2253,
        [Display(ShortName = "SecuredMobility", GroupName = "OnlineLocationFavorite", Name = "Delete", Description = "Can delete online location favorite")]
        OnlineLocationFavorite_Delete = 0x2254,

    }

}