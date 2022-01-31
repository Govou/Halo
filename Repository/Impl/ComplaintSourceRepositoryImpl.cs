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
    public class ComplaintSourceRepositoryImpl : IComplaintSourceRepository
    {
        private readonly HalobizContext _context;
        private readonly ILogger<ComplaintSourceRepositoryImpl> _logger;
        public ComplaintSourceRepositoryImpl(HalobizContext context, ILogger<ComplaintSourceRepositoryImpl> logger)
        {
            this._logger = logger;
            this._context = context;
        }

        public async Task<bool> DeleteComplaintSource(ComplaintSource complaintSource)
        {
            complaintSource.IsDeleted = true;
            _context.ComplaintSources.Update(complaintSource);
            return await SaveChanges();
        }

        public async Task<IEnumerable<ComplaintSource>> FindAllComplaintSources()
        {
            return await _context.ComplaintSources
               .Where(complaintSource => complaintSource.IsDeleted == false)
               .OrderBy(complaintSource => complaintSource.CreatedAt)
               .ToListAsync();
        }

        public async Task<ComplaintSource> FindComplaintSourceById(long Id)
        {
            return await _context.ComplaintSources
                .Where(complaintSource => complaintSource.IsDeleted == false)
                .FirstOrDefaultAsync(complaintSource => complaintSource.Id == Id && complaintSource.IsDeleted == false);

        }

        public async Task<ComplaintSource> FindComplaintSourceByName(string name)
        {
            return await _context.ComplaintSources
                 .Where(complaintSource => complaintSource.IsDeleted == false)
                 .FirstOrDefaultAsync(complaintSource => complaintSource.Caption == name && complaintSource.IsDeleted == false);

        }

        public async Task<ComplaintSource> SaveComplaintSource(ComplaintSource complaintSource)
        {
            var complaintSourceEntity = await _context.ComplaintSources.AddAsync(complaintSource);
            if (await SaveChanges())
            {
                return complaintSourceEntity.Entity;
            }
            return null;
        }

        public async Task<ComplaintSource> UpdateComplaintSource(ComplaintSource complaintSource)
        {
            var complaintSourceEntity = _context.ComplaintSources.Update(complaintSource);
            if (await SaveChanges())
            {
                return complaintSourceEntity.Entity;
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
