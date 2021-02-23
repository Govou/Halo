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
using HaloBiz.Model;
using Microsoft.Extensions.Configuration;

namespace HaloBiz.Adapters.Impl
{
    public class MailAdapter : IMailAdapter
    {
        private readonly ILogger<MailAdapter> _logger;
        private readonly IConfiguration _configuration;
        private readonly string _mailBaseUrl;
        public MailAdapter(ILogger<MailAdapter> logger, IConfiguration configuration)
        {
            _logger = logger;
            _configuration = configuration;
            _mailBaseUrl = _configuration["MailServiceBaseUrl"] ?? _configuration.GetSection("AppSettings:MailServiceBaseUrl").Value;
        }

        public async Task<ApiResponse> SendUserAssignedToRoleMail(string message)
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

                return new ApiOkResponse(true);
            }
            catch (Exception ex)
            {
                _logger.LogInformation(ex.Message);
                _logger.LogInformation(ex.StackTrace);
                return new ApiResponse(500, ex.Message);
            }
        }
        public async Task<ApiResponse> SendNewDeliverableAssigned(NewDeliverableAssignedDTO newDeliverableAssignedDTO)
        {
            var baseUrl = $"{_mailBaseUrl}/Mail/SendNewDeliverableAssigned";

            try
            {
                var response = await baseUrl.AllowAnyHttpStatus()
                   .PostJsonAsync(new
                   {
                       emailAddress = newDeliverableAssignedDTO.EmailAddress,
                       userName = newDeliverableAssignedDTO.UserName,
                       customer = newDeliverableAssignedDTO.Customer,
                       taskOwner = newDeliverableAssignedDTO.TaskOwner,
                       taskName = newDeliverableAssignedDTO.TaskName,
                       serviceName = newDeliverableAssignedDTO.ServiceName,
                       quantity = newDeliverableAssignedDTO.Quantity,
                       deliverable = newDeliverableAssignedDTO.Deliverable
                   }).ReceiveJson();

                return new ApiOkResponse(true);
            }
            catch (Exception ex)
            {
                _logger.LogInformation(ex.Message);
                _logger.LogInformation(ex.StackTrace);
                return new ApiResponse(500, ex.Message);
            }
        }
        public async Task<ApiResponse> SendNewUserSignup(string messsage)
        {
            var baseUrl = $"{_mailBaseUrl}/Mail/SendNewUserSignup";

            var userProfile = JsonConvert.DeserializeObject<UserProfile>(messsage);

            try
            {
                var userName = $"{userProfile.FirstName} {userProfile.LastName}";
                var response = await baseUrl.AllowAnyHttpStatus()
                   .PostJsonAsync(new
                   {
                       emailAddress = userProfile.Email,
                       userName = userName
                   }).ReceiveJson();

                //if (!string.IsNullOrWhiteSpace(userProfile.AltEmail)) 
                //{
                //    await baseUrl.AllowAnyHttpStatus()
                //   .PostJsonAsync(new
                //   {
                //       emailAddress = userProfile.AltEmail,
                //       userName = userName
                //   }).ReceiveJson();
                //}

                return new ApiOkResponse(true);
            }
            catch (Exception ex)
            {
                _logger.LogInformation(ex.Message);
                _logger.LogInformation(ex.StackTrace);
                return new ApiResponse(500, ex.Message);
            }
        }

        public async Task<ApiResponse> AssignRoleToNewUser(string serializedUser, string adminEmails)
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

                return new ApiOkResponse(true);
            }
            catch (Exception ex)
            {
                _logger.LogInformation(ex.Message);
                _logger.LogInformation(ex.StackTrace);
                return new ApiResponse(500, ex.Message);
            }
        }
    }
}
