using Halobiz.Common.DTOs.ApiDTOs;
using Halobiz.Common.DTOs.TransferDTOs;
using Microsoft.Extensions.Configuration;
using OnlinePortalBackend.Adapters;
using OnlinePortalBackend.Repository;
using System.Threading.Tasks;
using Flurl.Http;
using System.Collections.Generic;
using System;
using Microsoft.Extensions.Logging;

namespace OnlinePortalBackend.MyServices.Impl
{
    public class ComplaintServiceImpl : IComplaintService
    {
        IComplaintRepository _complaintRepository;
        private readonly IApiInterceptor _apiInterceptor;
        private readonly IConfiguration _configuration;
        private string _HalobizBaseUrl;
        private readonly ILogger<ComplaintServiceImpl> _logger;
        public ComplaintServiceImpl(IComplaintRepository complaintRepository, IApiInterceptor apiInterceptor, IConfiguration configuration, ILogger<ComplaintServiceImpl> logger)
        {
            _complaintRepository = complaintRepository;
            _apiInterceptor = apiInterceptor;
            _configuration = configuration;
            _HalobizBaseUrl = _configuration["HalobizBaseUrl"] ?? _configuration.GetSection("AppSettings:HalobizBaseUrl").Value;
            _logger = logger;

        }

        public async Task<ApiCommonResponse> CreateComplaint(ComplaintDTO complaint)
        {
            ApiCommonResponse responseData = new ApiCommonResponse();
            try
            {
                responseData = await _complaintRepository.CreateComplaint(complaint);
                return responseData;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return CommonResponse.Send(ResponseCodes.FAILURE, null, ex.Message);
            }
        }

        public async Task<ApiCommonResponse> GetComplainType()
        {
            var complaints = _complaintRepository.GetComplainTypes();

            if (complaints == null)
            {
                return CommonResponse.Send(ResponseCodes.NO_DATA_AVAILABLE);
            }
            return CommonResponse.Send(ResponseCodes.SUCCESS, complaints);
        }
    }
}
