using HaloBiz.DTOs.ApiDTOs;
using HaloBiz.DTOs.MailDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HaloBiz.Adapters
{
    public interface IMailAdapter
    {
        Task<ApiResponse> SendUserAssignedToRoleMail(string userProfile);
        Task<ApiResponse> SendNewDeliverableAssigned(string serializedDeliverable);
        Task<ApiResponse> SendNewTaskAssigned(string serializedTask, string operatingEntityName);
        Task<ApiResponse> SendNewUserSignup(string userProfile);
        Task<ApiResponse> AssignRoleToNewUser(string serializedUser, string adminEmails);
        Task<ApiResponse> ApproveNewService(string serializedApproval);
        Task<ApiResponse> ApproveNewQuoteService(string serializedApprovals);
        Task<ApiResponse> SendQuoteNotification(string serializedQuote);
        Task<ApiResponse> SendPeriodicInvoice(InvoiceMailDTO invoiceMailDTO);
        Task<ApiResponse> SendComplaintResolutionConfirmationMail(ConfirmComplaintResolutionMailDTO model);
    }
}
