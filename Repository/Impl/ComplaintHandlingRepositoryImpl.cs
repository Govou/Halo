using HaloBiz.DTOs.TransferDTOs;
using HalobizMigrations.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HaloBiz.Repository.Impl
{
    public class ComplaintHandlingRepositoryImpl : IComplaintHandlingRepository
    {
        private readonly HalobizContext _context;

        public ComplaintHandlingRepositoryImpl(HalobizContext context)
        {
            _context = context;
        }

        public Task<ComplaintHandlingStatsDTO> GetComplaintHandlingStats(long userProfileID)
        {
            throw new NotImplementedException();
        }

        public Task<ComplaintHandlingDTO> GetComplaintsHandling(long userProfileID)
        {
            throw new NotImplementedException();
        }
    }
}
