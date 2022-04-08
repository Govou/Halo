using Halobiz.Common.DTOs.ApiDTOs;
using HaloBiz.DTOs.MailDTOs;
using HaloBiz.DTOs.ReceivingDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HaloBiz.Adapters
{
    public interface IMailAdapter
    {
        Task<ApiCommonResponse> SendUserAssignedToRoleMail(string userProfile);
        Task<ApiCommonResponse> SendNewDeliverableAssigned(string serializedDeliverable);
        Task<ApiCommonResponse> SendNewTaskAssigned(string serializedTask, string operatingEntityName);
        Task<ApiCommonResponse> SendNewUserSignup(string userProfile);
        Task<ApiCommonResponse> AssignRoleToNewUser(string serializedUser, string adminEmails);
        Task<ApiCommonResponse> ApproveNewService(string serializedApproval);
        Task<ApiCommonResponse> ApproveNewQuoteService(string serializedApprovals);
        Task<ApiCommonResponse> SendQuoteNotification(string serializedQuote);
        Task<ApiCommonResponse> SendPeriodicInvoice(InvoiceMailDTO invoiceMailDTO);
        Task<ApiCommonResponse> SendJourneyManagementPlan(MasterServiceAssignmentMailVMDTO masterServiceAssignmentMailVMDTO);
        Task<ApiCommonResponse> SendPaidJourneyConfirmationMail(MasterServiceAssignmentMailVMDTO masterServiceAssignmentMailVMDTO);
        Task<ApiCommonResponse> SendNoPaymentConfirmationMail(MasterServiceAssignmentMailVMDTO masterServiceAssignmentMailVMDTO);
        Task<ApiCommonResponse> SendComplaintResolutionConfirmationMail(ConfirmComplaintResolutionMailDTO model);
    }
}
