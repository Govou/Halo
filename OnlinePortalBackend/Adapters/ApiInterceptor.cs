using Flurl.Http;
using Halobiz.Common.DTOs.ApiDTOs;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OnlinePortalBackend.Adapters
{
    public class ApiInterceptor
    {
        private readonly IConfiguration _configuration;
        private readonly ILogger<ApiInterceptor> _logger;
        private readonly IMemoryCache _memoryCache;
        private static string TokenName = "token";
        private string _HalobizBaseUrl;

        public ApiInterceptor(IConfiguration configuration, 
            ILogger<ApiInterceptor> logger,
            IMemoryCache memoryCache)
        {
            _configuration = configuration;
            _logger = logger;
            _memoryCache = memoryCache;
            _HalobizBaseUrl = _configuration["HalobizBaseUrl"] ?? _configuration.GetSection("AppSettings:HalobizBaseUrl").Value;

        }

        public Task<ApiCommonResponse> PostCalls(string url, object request)
        {
            try
            {

            }
            catch (Exception ex)
            {
                _logger.LogError(ex.StackTrace);
                throw;
            }
            return null;
        }

        private async Task<string> GetToken()
        {
            //first check if this token is in the cache
            var token = _memoryCache.Get<string>(TokenName);
            if (string.IsNullOrEmpty(token))
                return await RequestToken();
            return token;

        }

        private async Task<string> RequestToken()
        {
            try
            {
                var baseUrl = string.Concat(_HalobizBaseUrl, "/Auth/OnlinePortalNotice");
                var username = _configuration["HalobizUsername"] ?? _configuration.GetSection("AppSettings:HalobizUsername").Value;
                var password = _configuration["HalobizPassword"] ?? _configuration.GetSection("AppSettings:HalobizPassword").Value;


                var request = new {username = username, password = password};
                var response = await baseUrl.AllowAnyHttpStatus()
                   .PostJsonAsync(request).ReceiveJson();

                var responseData = response?.responseData;
                if (responseData != null)
                {
                    //save the token with the expiry in mind in the cache
                    if (!_memoryCache.TryGetValue<string>(TokenName, out string token))
                    {
                        DateTime expiry = DateTime.Parse(responseData.TokenExpiryTime);
                        var cacheEntryOptions = new MemoryCacheEntryOptions()
                                    .SetAbsoluteExpiration(expiry);
                        token = responseData.Token;
                        _memoryCache.Set(TokenName, token, cacheEntryOptions);
                    }
                    return responseData.Token;
                }
                else
                    throw new Exception("No token received from Halobiz");
            }
            catch (Exception ex)
            {
                _logger.LogInformation(ex.Message);
                throw;
            }
        }
    }
}
