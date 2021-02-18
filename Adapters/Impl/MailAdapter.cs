using HaloBiz.DTOs.ApiDTOs;
using Flurl;
using Flurl.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace HaloBiz.Adapters.Impl
{
    public class MailAdapter : IMailAdapter
    {
        private readonly ILogger<MailAdapter> _logger;
        public MailAdapter(ILogger<MailAdapter> logger)
        {
            _logger = logger;
        }

        public async Task<ApiResponse> SendUserAssignedToRoleMail(string userEmail, string userName)
        {
            var baseUrl = "http://halobiz-mail-api.herokuapp.com/";

            try
            {
                var response = await baseUrl.AllowAnyHttpStatus()
               .AppendPathSegments("/Mail​/SendSampleNotification")
               .PostJsonAsync(new
               {
                   emailAddress = userEmail,
                   userName
               }).ReceiveJson<object>();

                return new ApiOkResponse(response);
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
