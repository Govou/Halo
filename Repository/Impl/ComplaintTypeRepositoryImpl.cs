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
    public class ComplaintTypeRepositoryImpl : IComplaintTypeRepository
    {
        private readonly HalobizContext _context;
        private readonly ILogger<ComplaintTypeRepositoryImpl> _logger;
        public ComplaintTypeRepositoryImpl(HalobizContext context, ILogger<ComplaintTypeRepositoryImpl> logger)
        {
            this._logger = logger;
            this._context = context;
        }

        public async Task<bool> DeleteComplaintType(ComplaintType complaintType)
        {
            complaintType.IsDeleted = true;
            _context.ComplaintTypes.Update(complaintType);
            return await SaveChanges();
        }

        public async Task<IEnumerable<ComplaintType>> FindAllComplaintTypes()
        {
            return await _context.ComplaintTypes
               .Where(complaintType => complaintType.IsDeleted == false)
               .OrderBy(complaintType => complaintType.CreatedAt)
               .ToListAsync();
        }

        public async Task<ComplaintType> FindComplaintTypeById(long Id)
        {
            return await _context.ComplaintTypes
                .Where(complaintType => complaintType.IsDeleted == false)
                .FirstOrDefaultAsync(complaintType => complaintType.Id == Id && complaintType.IsDeleted == false);

        }

        public async Task<ComplaintType> FindComplaintTypeByName(string name)
        {
            return await _context.ComplaintTypes
                 .Where(complaintType => complaintType.IsDeleted == false)
                 .FirstOrDefaultAsync(complaintType => complaintType.Caption == name && complaintType.IsDeleted == false);

        }

        public async Task<ComplaintType> SaveComplaintType(ComplaintType complaintType)
        {
            var complaintTypeEntity = await _context.ComplaintTypes.AddAsync(complaintType);
            if (await SaveChanges())
            {
                return complaintTypeEntity.Entity;
            }
            return null;
        }

        public async Task<ComplaintType> UpdateComplaintType(ComplaintType complaintType)
        {
            var complaintTypeEntity = _context.ComplaintTypes.Update(complaintType);
            if (await SaveChanges())
            {
                return complaintTypeEntity.Entity;
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
