using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using OnlinePortalBackend.DTOs.ApiDTOs;
using OnlinePortalBackend.DTOs.ReceivingDTOs;
using OnlinePortalBackend.DTOs.TransferDTOs;
using OnlinePortalBackend.Helpers;
using HalobizMigrations.Models.OnlinePortal;
using OnlinePortalBackend.Repository;
using HalobizMigrations.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace OnlinePortalBackend.MyServices.Impl
{
    public class ExistingCustomerServiceImpl : IExistingCustomerService
    {
        private readonly HalobizContext _context;
        private readonly ILogger<ExistingCustomerServiceImpl> _logger;
        private readonly IMapper _mapper;
        private readonly IModificationHistoryRepository _historyRepo;
        public ExistingCustomerServiceImpl(
            HalobizContext context,
            ILogger<ExistingCustomerServiceImpl> logger,
            IMapper mapper, 
            IModificationHistoryRepository historyRepo)
        {
            _mapper = mapper;
            _logger = logger;
            _context = context;
            _historyRepo = historyRepo;
        }

        public async Task<ApiResponse> GetCustomerByEmail(string email)
        {
            try
            {
                var existingCustomerDivision = await _context.CustomerDivisions.AsNoTracking()
                    .Include(x => x.Customer)
                    .FirstOrDefaultAsync(x => x.Email.ToLower() == email.ToLower());

                if (existingCustomerDivision == null) return new ApiResponse(400, $"No Customer Division with email {email} does not exist");

                return new ApiOkResponse(existingCustomerDivision.Customer);
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