using HaloBiz.DTOs.ApiDTOs;
using Flurl;
using Flurl.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using HaloBiz.DTOs.MailDTOs;
using Newtonsoft.Json;
using HalobizMigrations.Models;
using Microsoft.Extensions.Configuration;

using HalobizMigrations.Data;
using Microsoft.EntityFrameworkCore;
using HaloBiz.DTOs.ReceivingDTOs;

namespace HaloBiz.Adapters.Impl
{
    public class MailAdapter : IMailAdapter
    {
        private readonly ILogger<MailAdapter> _logger;
        private readonly HalobizContext _context;
        private readonly IConfiguration _configuration;
        private readonly string _mailBaseUrl;
        public MailAdapter(ILogger<MailAdapter> logger, HalobizContext context, IConfiguration configuration)
        {
            _logger = logger;
            _configuration = configuration;
            _mailBaseUrl = _configuration["MailServiceBaseUrl"] ?? _configuration.GetSection("AppSettings:MailServiceBaseUrl").Value;
            _context = context;
        }

        public async Task<ApiCommonResponse> SendUserAssignedToRoleMail(string message)
        {            
            var baseUrl = $"{_mailBaseUrl}/Mail/SendNewRoleAssigned";

            var userProfile = JsonConvert.DeserializeObject<UserProfile>(message);

            try
            {
                var response = await baseUrl.AllowAnyHttpStatus()
                   .PostJsonAsync(new
                   {
                       emailAddress = userProfile.Email,
                       userName = $"{userProfile.FirstName} {userProfile.LastName}",
                       role = userProfile.Role.Name,
                       roleClaims = userProfile.Role.RoleClaims.Select(x => x.Name).ToArray()
                   }).ReceiveJson();

                return CommonResponse.Send(ResponseCodes.SUCCESS);
            }
            catch (Exception ex)
            {
                _logger.LogInformation(ex.Message);
                _logger.LogInformation(ex.StackTrace);
                return  CommonResponse.Send(ResponseCodes.FAILURE, null, "Some system errors occurred");
            }
        }
        public async Task<ApiCommonResponse> SendNewDeliverableAssigned(string serializedDeliverable)
        {
            var baseUrl = $"{_mailBaseUrl}/Mail/SendNewDeliverableAssigned";

            var deliverableFulfillment = JsonConvert.DeserializeObject<DeliverableFulfillment>(serializedDeliverable);
            
            try
            {
                var response = await baseUrl.AllowAnyHttpStatus()
                   .PostJsonAsync(new
                   {
                       emailAddress = deliverableFulfillment.Responsible.Email,
                       userName = $"{deliverableFulfillment.Responsible.FirstName} {deliverableFulfillment.Responsible.LastName}",
                       client = deliverableFulfillment.TaskFullfillment.CustomerDivision.DivisionName,
                       taskOwner = $"{deliverableFulfillment.TaskFullfillment.Responsible.FirstName} {deliverableFulfillment.TaskFullfillment.Responsible.LastName}",
                       taskName = deliverableFulfillment.TaskFullfillment.Caption,
                       serviceName = deliverableFulfillment.TaskFullfillment.ContractService.Service.Name,
                       serviceQuantity = deliverableFulfillment.TaskFullfillment.ContractService.Quantity.ToString(),
                       deliverableName = deliverableFulfillment.Caption
                   }).ReceiveJson();

               return CommonResponse.Send(ResponseCodes.SUCCESS);
            }
            catch (Exception ex)
            {
                _logger.LogInformation(ex.Message);
                _logger.LogInformation(ex.StackTrace);
                return  CommonResponse.Send(ResponseCodes.FAILURE, null, "Some system errors occurred");
            }
        }

        public async Task<ApiCommonResponse> SendNewTaskAssigned(string serializedTask, string operatingEntityName)
        {
            var baseUrl = $"{_mailBaseUrl}/Mail/NewTaskAssigned";

            var task = JsonConvert.DeserializeObject<TaskFulfillment>(serializedTask);

            try
            {
                var response = await baseUrl.AllowAnyHttpStatus()
                   .PostJsonAsync(new
                   {
                       emailAddresses = new string[] { task.Responsible?.Email },
                       userName = $"{task.Responsible?.FirstName} {task.Responsible?.LastName}",
                       customer = task.CustomerDivision?.DivisionName,
                       operatingEntityName = task.ContractService?.Service?.OperatingEntity?.Name,
                       taskName = task.Caption
                   }).ReceiveJson();

                return CommonResponse.Send(ResponseCodes.SUCCESS);
            }
            catch (Exception ex)
            {
                _logger.LogInformation(ex.Message);
                _logger.LogInformation(ex.StackTrace);
                return  CommonResponse.Send(ResponseCodes.FAILURE, null, "Some system errors occurred");
            }
        }

        public async Task<ApiCommonResponse> SendNewUserSignup(string messsage)
        {
            var baseUrl = $"{_mailBaseUrl}/Mail/SendNewUserSignup";

            var userProfile = JsonConvert.DeserializeObject<UserProfile>(messsage);

            try
            {
                var userName = $"{userProfile.FirstName} {userProfile.LastName}";
                var response =  await baseUrl.AllowAnyHttpStatus()
                   .PostJsonAsync(new
                   {
                       emailAddress = userProfile.Email,
                       userName = userName
                   }).ReceiveJson();

                if (!string.IsNullOrWhiteSpace(userProfile.AltEmail))
                {
                    await baseUrl.AllowAnyHttpStatus()
                   .PostJsonAsync(new
                   {
                       emailAddress = userProfile.AltEmail,
                       userName = userName
                   }).ReceiveJson();
                }

                return CommonResponse.Send(ResponseCodes.SUCCESS);
            }
            catch (Exception ex)
            {
                _logger.LogInformation(ex.Message);
                _logger.LogInformation(ex.StackTrace);
                return  CommonResponse.Send(ResponseCodes.FAILURE, null, "Some system errors occurred");
            }
        }

        public async Task<ApiCommonResponse> AssignRoleToNewUser(string serializedUser, string adminEmails)
        {
            var baseUrl = $"{_mailBaseUrl}/Mail/AssignRoleToNewUser";

            var userProfile = JsonConvert.DeserializeObject<UserProfile>(serializedUser);

            try
            {
                var response = await baseUrl.AllowAnyHttpStatus()
                   .PostJsonAsync(new
                   {
                       userName = $"{userProfile.FirstName} {userProfile.LastName}",
                       emailAddress = JsonConvert.DeserializeObject<string[]>(adminEmails)
                   }).ReceiveJson();

                return CommonResponse.Send(ResponseCodes.SUCCESS);
            }
            catch (Exception ex)
            {
                _logger.LogInformation(ex.Message);
                _logger.LogInformation(ex.StackTrace);
                return  CommonResponse.Send(ResponseCodes.FAILURE, null, "Some system errors occurred");
            }
        }

        public async Task<ApiCommonResponse> ApproveNewService(string serializedApproval)
        {
            var baseUrl = $"{_mailBaseUrl}/Mail/ApproveNewService";

            var approval = JsonConvert.DeserializeObject<Approval>(serializedApproval);

            try
            {
                if (approval.Responsible != null && approval.Services != null)
                {
                    var response = await baseUrl.AllowAnyHttpStatus()
                    .PostJsonAsync(new
                    {
                        emailAddresses = new string[] { approval.Responsible?.Email },
                        userName = $"{approval.Responsible.FirstName} {approval.Responsible.LastName}",
                        operatingEntityName = approval.Services.OperatingEntity.Name,
                        serviceCategoryName = approval.Services.ServiceCategory.Name,
                        divisionName = approval.Services.Division.Name,
                        serviceName = approval.Services.Name
                    }).ReceiveJson();
                }
                else
                {
                    var approvalInfo = JsonConvert.SerializeObject(approval, new JsonSerializerSettings { ReferenceLoopHandling = ReferenceLoopHandling.Ignore });
                    _logger.LogInformation($"Failed to send mail for approval {approvalInfo} due to missing parameters");
                }
                                     
                return CommonResponse.Send(ResponseCodes.SUCCESS);
            }
            catch (Exception ex)
            {
                _logger.LogInformation(ex.Message);
                _logger.LogInformation(ex.StackTrace);
                return  CommonResponse.Send(ResponseCodes.FAILURE, null, "Some system errors occurred");
            }
        }

        public async Task<ApiCommonResponse> ApproveNewQuoteService(string serializedApprovals)
        {
            var baseUrl = $"{_mailBaseUrl}/Mail/ApproveNewQuoteService";

            var approval = JsonConvert.DeserializeObject<Approval>(serializedApprovals);

            try
            {
                if(approval.Responsible != null && approval.QuoteService != null && approval.QuoteService.Service != null)
                {
                    var response = await baseUrl.AllowAnyHttpStatus()
                    .PostJsonAsync(new
                    {
                        emailAddresses = new string[] { approval.Responsible?.Email },
                        userName = $"{approval.Responsible.FirstName} {approval.Responsible.LastName}",
                        operatingEntityName = approval.QuoteService.Service.OperatingEntity.Name,
                        serviceCategoryName = approval.QuoteService.Service.ServiceCategory.Name,
                        divisionName = approval.QuoteService.Service.Division.Name,
                        quoteServiceName = approval.QuoteService.Service.Name                
                    }).ReceiveJson();
                }
                else
                {
                    var approvalInfo = JsonConvert.SerializeObject(approval, new JsonSerializerSettings { ReferenceLoopHandling = ReferenceLoopHandling.Ignore });
                    _logger.LogError($"Failed to send mail for approval {approvalInfo} due to missing parameters");
                }            

                return CommonResponse.Send(ResponseCodes.SUCCESS);
            }
            catch (Exception ex)
            {
                _logger.LogInformation(ex.Message);
                _logger.LogInformation(ex.StackTrace);
                return  CommonResponse.Send(ResponseCodes.FAILURE, null, "Some system errors occurred");
            }
        }

        public async Task<ApiCommonResponse> SendQuoteNotification(string serializedQuote)
        {
            var baseUrl = $"{_mailBaseUrl}/Mail/SendQuoteNotification";

            var quotesDetails = JsonConvert.DeserializeObject<Quote>(serializedQuote);

            try
            {
                var response = await baseUrl.AllowAnyHttpStatus().PostJsonAsync(new
                {
                    nameOfContact = quotesDetails.LeadDivision?.PrimaryContact?.FirstName,
                    quoteservices = JsonConvert.SerializeObject(quotesDetails.QuoteServices.Select(x => x.Service.Name)),
                    emailAddress = quotesDetails.LeadDivision?.PrimaryContact?.Email
                }).ReceiveJson();

                return CommonResponse.Send(ResponseCodes.SUCCESS);
            }
            catch (Exception ex)
            {
                _logger.LogInformation(ex.Message);
                _logger.LogInformation(ex.StackTrace);
                return  CommonResponse.Send(ResponseCodes.FAILURE, null, "Some system errors occurred");
            }
        }

        public async Task<ApiCommonResponse> SendPeriodicInvoice(InvoiceMailDTO invoiceMailDTO)
        {
            var baseUrl = $"{_mailBaseUrl}/Mail/SendPeriodicInvoice";
            try
            {
                var response = await baseUrl.AllowAnyHttpStatus()
                   .PostJsonAsync(invoiceMailDTO).ReceiveJson();
                return CommonResponse.Send(ResponseCodes.SUCCESS);
            }
            catch (Exception ex)
            {
                _logger.LogInformation(ex.Message);
                _logger.LogInformation(ex.StackTrace);
                return  CommonResponse.Send(ResponseCodes.FAILURE, null, "Some system errors occurred");
            }
        }



        public async Task<ApiCommonResponse> SendComplaintResolutionConfirmationMail(ConfirmComplaintResolutionMailDTO model)
        {
            var baseUrl = $"{_mailBaseUrl}/Mail/ComplaintResolutionConfirmation";
            try
            {
                var response = await baseUrl.AllowAnyHttpStatus()
                   .PostJsonAsync(model).ReceiveJson();
                return CommonResponse.Send(ResponseCodes.SUCCESS);
            }
            catch(Exception ex)
            {
                _logger.LogInformation(ex.Message);
                _logger.LogInformation(ex.StackTrace);
                return  CommonResponse.Send(ResponseCodes.FAILURE, null, "Some system errors occurred");
            }
        }

        public async Task<ApiCommonResponse> SendJourneyManagementPlan(MasterServiceAssignmentMailVMDTO masterServiceAssignmentMailVMDTO)
        {

            var baseUrl = $"{_mailBaseUrl}/Mail/JourneyManagementPlan";
            try
            {
                var response = await baseUrl.AllowAnyHttpStatus()
                   .PostJsonAsync(masterServiceAssignmentMailVMDTO).ReceiveJson();
                return CommonResponse.Send(ResponseCodes.SUCCESS);
            }
            catch (Exception ex)
            {
                _logger.LogInformation(ex.Message);
                _logger.LogInformation(ex.StackTrace);
                return CommonResponse.Send(ResponseCodes.FAILURE, null, "Some system errors occurred");
            }
        }

        //public async Task<ApiCommonResponse> LeadConversionTriggered(string serializedLeadtoClient)
        //{
        //    var baseUrl = $"{_mailBaseUrl}/Mail/LeadConversionTriggered";

        //    var quotesDetails = JsonConvert.DeserializeObject<Quote>(serializedLeadtoClient);

        //    try
        //    {
        //        return await baseUrl.AllowAnyHttpStatus()
        //           .PostJsonAsync(new
        //           {
        //               nameOfContact = quotesDetails.LeadDivision?.PrimaryContact?.FirstName,
        //               quoteservices = JsonConvert.SerializeObject(quotesDetails.QuoteService.Select(x => x.Service.Name)),
        //               emailAddress = quotesDetails.LeadDivision?.PrimaryContact?.Email
        //           }).ReceiveJson();

        //        return CommonResponse.Send(ResponseCodes.SUCCESS);
        //    }
        //    catch (Exception ex)
        //    {
        //        _logger.LogInformation(ex.Message);
        //        _logger.LogInformation(ex.StackTrace);
        //        return  CommonResponse.Send(ResponseCodes.FAILURE, null, "Some system errors occurred");
        //    }
        //}

        //public async Task<ApiCommonResponse> LeadDroppedNotification(string serializedLead)
        //{
        //    var baseUrl = $"{_mailBaseUrl}/Mail/LeadDroppedNotification";

        //    var leadDetails = JsonConvert.DeserializeObject<LeadDivision>(serializedLead);
        //    var userProfile = JsonConvert.DeserializeObject<UserProfile>(serializedLead);

        //    try
        //    {
        //        return await baseUrl.AllowAnyHttpStatus()
        //           .PostJsonAsync(new
        //           {
        //               leadName = leadDetails.DivisionName,
        //               emailAddress = userProfile.Email
        //           }).ReceiveJson();

        //        return CommonResponse.Send(ResponseCodes.SUCCESS);
        //    }
        //    catch (Exception ex)
        //    {
        //        _logger.LogInformation(ex.Message);
        //        _logger.LogInformation(ex.StackTrace);
        //        return  CommonResponse.Send(ResponseCodes.FAILURE, null, "Some system errors occurred");
        //    }
        //}
    }
}
