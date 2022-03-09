using HalobizMigrations.Data;
using HalobizMigrations.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using OnlinePortalBackend.DTOs.ApiDTOs;
using OnlinePortalBackend.DTOs.ReceivingDTOs;
using OnlinePortalBackend.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace OnlinePortalBackend.MyServices.Impl
{
    public class ProspectServiceImpl : IProspectService
    {
        private readonly HalobizContext _context;
        private readonly ILogger<CartServiceImpl> _logger;
        private readonly IHttpClientFactory _httpClient;
        private readonly IConfiguration _config;

        public ProspectServiceImpl(
            HalobizContext context,
            ILogger<CartServiceImpl> logger,
            IHttpClientFactory httpClient,
            IConfiguration config)
        {
            _context = context;
            _logger = logger;
            _config = config;
            _httpClient = httpClient;
        }

        public async Task<ApiResponse> SaveCartItems(HttpContext context, CartItemsReceivingDTO cartItems, long prospectId)
        {
            try
            {
                var prospect = await _context.Prospects.FindAsync(prospectId);

                prospect.CartItems = JsonConvert.SerializeObject(cartItems);

                _context.Prospects.Update(prospect);

                return new ApiOkResponse(true);

            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                _logger.LogError(ex.StackTrace);
                return new ApiResponse(500, ex.Message);
            }
        }

        public async Task<ApiResponse> GetLeadDivisions(HttpContext context, long prospectId)
        {
            try
            {
                var prospect = await _context.Prospects.FindAsync(prospectId);

                var leadDivisions = await _context.LeadDivisions
                                                .Where(x => x.LeadId == prospect.LeadId && !x.IsDeleted)
                                                .ToListAsync();

                return new ApiOkResponse(leadDivisions);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                _logger.LogError(ex.StackTrace);
                return new ApiResponse(500, ex.Message);
            }
        }

        public async Task<ApiResponse> GetCartItems(HttpContext context, long prospectId)
        {
            try
            {
                var prospect = await _context.Prospects.FindAsync(prospectId);

                object cartItems = null;

                if(prospect.CartItems != null)
                {
                    cartItems = JsonConvert.DeserializeObject(prospect.CartItems);
                }

                return new ApiOkResponse(cartItems);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                _logger.LogError(ex.StackTrace);
                return new ApiResponse(500, ex.Message);
            }
        }
    }
}
