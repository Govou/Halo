using HaloBiz.DTOs.ApiDTOs;
using HaloBiz.Repository;
using HalobizMigrations.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HaloBiz.MyServices.Impl
{
    public class ComplaintHandlingServiceImpl : IComplaintHandlingService
    {
        private readonly HalobizContext _context;
        private readonly ILogger<ComplaintHandlingServiceImpl> _logger;
        private readonly IComplaintHandlingRepository _complaintHandlingRepo;
        public ComplaintHandlingServiceImpl(HalobizContext context, ILogger<ComplaintHandlingServiceImpl> logger, IComplaintHandlingRepository complaintHandlingRepo)
        {
            _context = context;
            _logger = logger;
            _complaintHandlingRepo = complaintHandlingRepo;
        }

        public Task<ApiResponse> GetComplaintHandlingStats(HttpContext context, long userProfileID)
        {
            throw new NotImplementedException();
        }

        public Task<ApiResponse> GetComplaintsHandling(HttpContext context, long userProfileID)
        {
            throw new NotImplementedException();
        }
    }
}
