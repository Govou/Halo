using Flurl.Http;
using Halobiz.Common.DTOs.ApiDTOs;
using Halobiz.Common.DTOs.ReceivingDTOs;
using HalobizMigrations.Data;
using HalobizMigrations.Models;
using HalobizMigrations.Models.OnlinePortal;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OnlinePortalBackend.MyServices
{
    public interface IMailService
    {
        Task<ApiCommonResponse> ConfirmCodeSending(OnlinePortalDTO request);
    }
    public class MailService : IMailService
    {
        private readonly ILogger<MailService> _logger;
        private readonly HalobizContext _context;
        private readonly IConfiguration _configuration;
        private readonly string _mailBaseUrl;
        public MailService(ILogger<MailService> logger, HalobizContext context, IConfiguration configuration)
        {
            _logger = logger;
            _configuration = configuration;
            _mailBaseUrl = _configuration["MailServiceBaseUrl"] ?? _configuration.GetSection("AppSettings:MailServiceBaseUrl").Value;
            _context = context;
        }

        public async Task<ApiCommonResponse> ConfirmCodeSending(OnlinePortalDTO request)
        {
            var baseUrl = $"{_mailBaseUrl}/Mail/OnlinePortalNotice";
            try
            {
                var response = await baseUrl.AllowAnyHttpStatus()
                   .PostJsonAsync(request).ReceiveJson();
                return CommonResponse.Send(ResponseCodes.SUCCESS);
            }
            catch (Exception ex)
            {
                _logger.LogInformation(ex.Message);
                return CommonResponse.Send(ResponseCodes.FAILURE, null, ex.Message);
            }
        }
    }
}
