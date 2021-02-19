using HaloBiz.DTOs.ApiDTOs;
using Flurl;
using Flurl.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using HaloBiz.DTOs.MailDTOs;

namespace HaloBiz.Adapters.Impl
{
    public class MailAdapter : IMailAdapter
    {
        private readonly ILogger<MailAdapter> _logger;
        public MailAdapter(ILogger<MailAdapter> logger)
        {
            _logger = logger;
        }

        public async Task<ApiResponse> SendUserAssignedToRoleMail(NewRoleAssignedDTO newRoleAssignedDTO)
        {
            var baseUrl = "http://halobiz-mail-api.herokuapp.com/Mail/SendNewRoleAssigned";

            try
            {
                var response = await baseUrl.AllowAnyHttpStatus()
                   .PostJsonAsync(new
                   {
                       emailAddress = newRoleAssignedDTO.EmailAddress,
                       userName = newRoleAssignedDTO.UserName,
                       role = newRoleAssignedDTO.Role,
                       roleClaims = newRoleAssignedDTO.RoleClaims
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
            var baseUrl = "http://halobiz-mail-api.herokuapp.com/Mail/SendNewDeliverableAssigned";

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
    }
}
