using AutoMapper;
using Halobiz.Common.DTOs.ApiDTOs;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using OnlinePortalBackend.DTOs.ApiDTOs;
using OnlinePortalBackend.DTOs.TransferDTOs;
using OnlinePortalBackend.Helpers;
using OnlinePortalBackend.Repository;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Flurl.Http;
using Microsoft.Extensions.Configuration;
using System;
using HalobizMigrations.Data;
using Halobiz.Common.DTOs.TransferDTOs;
using OnlinePortalBackend.Adapters;

namespace OnlinePortalBackend.MyServices.Impl
{
    public class EndorsementServiceImpl : IEndorsementService
    {
        private readonly ILogger<EndorsementServiceImpl> _logger;
        private readonly IEndorsementRepository _endorsementRepo;
        private readonly IMapper _mapper;
        private readonly IConfiguration _configuration;
        private string _HalobizBaseUrl;
        private readonly HalobizContext _context;
        private readonly IApiInterceptor _apiInterceptor;
        public EndorsementServiceImpl(ILogger<EndorsementServiceImpl> logger,
            IEndorsementRepository endorsementRepo,
            IMapper mapper,
            IConfiguration configuration,
            HalobizContext context,
            IApiInterceptor apiInterceptor
            )
        {
            _logger = logger;
            _endorsementRepo = endorsementRepo;
            _mapper = mapper;
            _configuration = configuration;
            _context = context;
            _apiInterceptor = apiInterceptor;
            _HalobizBaseUrl = _configuration["HalobizBaseUrl"] ?? _configuration.GetSection("AppSettings:HalobizBaseUrl").Value;
        }

        public async Task<ApiCommonResponse> EndorsementTopUp(HttpContext context, List<ContractServiceForEndorsementReceivingDto> endorsements)
        {
            ApiCommonResponse responseData = new ApiCommonResponse();
            var userId = context.GetLoggedInUserId();
            var topup = _context.EndorsementTypes.FirstOrDefault(x => x.Caption.ToLower().Contains("topup"))?.Id;
            if (topup == null)
            {
                responseData.responseMsg = "An error occured";
                return responseData;
            }

            foreach (var item in endorsements)
            {
                item.CreatedById = userId;
                item.EndorsementTypeId = topup.Value;
            }

            try
            {
                var token = await _apiInterceptor.GetToken();
                var baseUrl = string.Concat(_HalobizBaseUrl, "Endorsement");


                var response = await baseUrl.AllowAnyHttpStatus().WithOAuthBearerToken($"{token}")
                   .PostJsonAsync(endorsements)?.ReceiveJson();

                foreach (KeyValuePair<string, object> kvp in (IDictionary<string, object>)response)
                {
                    if (kvp.Key.ToString() == "responseCode")
                    {
                        responseData.responseCode = kvp.Value.ToString();
                    }
                    if (kvp.Key.ToString() == "responseData")
                    {
                        responseData.responseData = kvp.Value;
                    }
                    if (kvp.Key.ToString() == "responseMsg")
                    {
                        responseData.responseMsg = kvp.Value.ToString();
                    }
                }

            }
            catch (Exception ex)
            {
                _logger.LogInformation(ex.Message);
                responseData.responseMsg = "An error occured";
            }
            return responseData;
        }

        public async Task<ApiCommonResponse> FetchEndorsements(HttpContext context, int limit = 10)
        {
            var endorsements = await _endorsementRepo.FindEndorsements(context.GetLoggedInUserId(), limit);
            if (endorsements.Count() == 0)
            {
                return CommonResponse.Send(ResponseCodes.NO_DATA_AVAILABLE); ;
            }
            var endorsementDTOs = _mapper.Map<IEnumerable<EndorsementDTO>>(endorsements);
            return CommonResponse.Send(ResponseCodes.SUCCESS, endorsementDTOs);
        }

        public async Task<ApiCommonResponse> TrackEndorsement(HttpContext context, long endorsementId)
        {
            var endorsement = await _endorsementRepo.FindEndorsementById(context.GetLoggedInUserId(), endorsementId);
            var result = string.Empty;
            if (endorsement == null)
            {
                return CommonResponse.Send(ResponseCodes.NO_DATA_AVAILABLE); ;
            }

            if (endorsement.IsDeclined) result = "Declined";
            else if (endorsement.IsApproved) result = "Approved";
            else result = "Pending";

            return CommonResponse.Send(ResponseCodes.SUCCESS, result);
        }

        public async Task<ApiCommonResponse> EndorsementReduction(HttpContext context, List<ContractServiceForEndorsementReceivingDto> endorsements)
        {

                ApiCommonResponse responseData = new ApiCommonResponse();
                var userId = context.GetLoggedInUserId();
                var topup = _context.EndorsementTypes.FirstOrDefault(x => x.Caption.ToLower().Contains("reduction"))?.Id;
                if (topup == null)
                {
                    responseData.responseMsg = "An error occured";
                    return responseData;
                }

                foreach (var item in endorsements)
                {
                    item.CreatedById = userId;
                    item.EndorsementTypeId = topup.Value;
                }

                try
                {
                    var token = await _apiInterceptor.GetToken();
                    var baseUrl = string.Concat(_HalobizBaseUrl, "Endorsement");


                    var response = await baseUrl.AllowAnyHttpStatus().WithOAuthBearerToken($"{token}")
                       .PostJsonAsync(endorsements)?.ReceiveJson();

                    foreach (KeyValuePair<string, object> kvp in (IDictionary<string, object>)response)
                    {
                        if (kvp.Key.ToString() == "responseCode")
                        {
                            responseData.responseCode = kvp.Value.ToString();
                        }
                        if (kvp.Key.ToString() == "responseData")
                        {
                            responseData.responseData = kvp.Value;
                        }
                        if (kvp.Key.ToString() == "responseMsg")
                        {
                            responseData.responseMsg = kvp.Value.ToString();
                        }
                    }

                }
                catch (Exception ex)
                {
                    _logger.LogInformation(ex.Message);
                    responseData.responseMsg = "An error occured";
                }
                return responseData;
            
        }
    }
}
