using HalobizMigrations.Data;
using HalobizMigrations.Models;
using HalobizMigrations.Models.Complaints;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HaloBiz.Repository.Impl
{
    public class ComplaintOriginRepositoryImpl : IComplaintOriginRepository
    {
        private readonly HalobizContext _context;
        private readonly ILogger<ComplaintOriginRepositoryImpl> _logger;
        public ComplaintOriginRepositoryImpl(HalobizContext context, ILogger<ComplaintOriginRepositoryImpl> logger)
        {
            this._logger = logger;
            this._context = context;
        }

        public async Task<bool> DeleteComplaintOrigin(ComplaintOrigin complaintOrigin)
        {
            complaintOrigin.IsDeleted = true;
            _context.ComplaintOrigins.Update(complaintOrigin);
            return await SaveChanges();
        }

        public async Task<IEnumerable<ComplaintOrigin>> FindAllComplaintOrigins()
        {
            return await _context.ComplaintOrigins
               .Where(complaintOrigin => complaintOrigin.IsDeleted == false)
               .OrderBy(complaintOrigin => complaintOrigin.CreatedAt)
               .ToListAsync();
        }

        public async Task<ComplaintOrigin> FindComplaintOriginById(long Id)
        {
            return await _context.ComplaintOrigins
                .Where(complaintOrigin => complaintOrigin.IsDeleted == false)
                .FirstOrDefaultAsync(complaintOrigin => complaintOrigin.Id == Id && complaintOrigin.IsDeleted == false);

        }

        public async Task<ComplaintOrigin> FindComplaintOriginByName(string name)
        {
            return await _context.ComplaintOrigins
                 .Where(complaintOrigin => complaintOrigin.IsDeleted == false)
                 .FirstOrDefaultAsync(complaintOrigin => complaintOrigin.Caption == name && complaintOrigin.IsDeleted == false);

        }

        public async Task<ComplaintOrigin> SaveComplaintOrigin(ComplaintOrigin complaintOrigin)
        {
            var complaintOriginEntity = await _context.ComplaintOrigins.AddAsync(complaintOrigin);
            if (await SaveChanges())
            {
                return complaintOriginEntity.Entity;
            }
            return null;
        }

        public async Task<ComplaintOrigin> UpdateComplaintOrigin(ComplaintOrigin complaintOrigin)
        {
            var complaintOriginEntity = _context.ComplaintOrigins.Update(complaintOrigin);
            if (await SaveChanges())
            {
                return complaintOriginEntity.Entity;
            }
            return null;
        }

        private async Task<bool> SaveChanges()
        {
            try
            {
                return await _context.SaveChangesAsync() > 0;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return false;
            }
        }
    }
}
