using Flurl.Http;
using Halobiz.Common.DTOs.ApiDTOs;
using Halobiz.Common.DTOs.ReceivingDTOs;
using Halobiz.Common.DTOs.TransferDTOs;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OnlinePortalBackend.Adapters
{
    public class ApiInterceptor : IApiInterceptor
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

        //public Task<ApiCommonResponse> PostCalls(string url, object request)
        //{
        //    try
        //    {

        //    }
        //    catch (Exception ex)
        //    {
        //        _logger.LogError(ex.StackTrace);
        //        throw;
        //    }
        //    return null;
        //}

        public async Task<string> GetToken()
        {
            //first check if this token is in the cache
            var token = _memoryCache.Get<string>(TokenName);
            if (string.IsNullOrEmpty(token))
                return await RequestToken();
            return token;

        }

        private async Task<string> RequestToken()
        {
            var authToken = string.Empty;
            DateTime expires = new DateTime();
            try
            {
                var baseUrl = string.Concat(_HalobizBaseUrl, "Auth/Login");
                var username = _configuration["HalobizUsername"] ?? _configuration.GetSection("AppSettings:HalobizUsername").Value;
                var password = _configuration["HalobizPassword"] ?? _configuration.GetSection("AppSettings:HalobizPassword").Value;

                            
                var request = new LoginDTO { Email = username, Password = password};
                var resp = await baseUrl.AllowAnyHttpStatus()
                   .PostJsonAsync(request).ReceiveJson();

                foreach (KeyValuePair<string, object> kvp in ((IDictionary<string, object>)resp))
                {
                    if (kvp.Key.ToString() == "responseData")
                    {
                        foreach (KeyValuePair<string, object> kvp1 in ((IDictionary<string, object>)kvp.Value))
                        {
                            if (kvp1.Key.ToString() == "token")
                            {
                                authToken = kvp1.Value.ToString();
                            }
                            if (kvp1.Key.ToString() == "jwtLifespan")
                            {
                                expires = DateTime.Now.AddMinutes(double.Parse(kvp1.Value.ToString()));
                            }
                        }
                    }
                }

                //   var response = JsonConvert.DeserializeObject<ApiCommonResponse>(resp);

                if (resp != null)
                {
                    //save the token with the expiry in mind in the cache
                    if (!_memoryCache.TryGetValue<string>(TokenName, out string token))
                    {
                        DateTime expiry = expires;
                        var cacheEntryOptions = new MemoryCacheEntryOptions()
                                    .SetAbsoluteExpiration(expiry);
                        token = authToken;
                        _memoryCache.Set(TokenName, token, cacheEntryOptions);
                    }
                    return authToken;
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
