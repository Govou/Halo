using HalobizMigrations.Data;
using HalobizMigrations.Models;
using HalobizMigrations.Models.Halobiz;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HaloBiz.Repository.Impl
{
    public class SuspectQualificationRepositoryImpl : ISuspectQualificationRepository
    {
        private readonly HalobizContext _context;
        private readonly ILogger<SuspectQualificationRepositoryImpl> _logger;
        public SuspectQualificationRepositoryImpl(HalobizContext context, ILogger<SuspectQualificationRepositoryImpl> logger)
        {
            this._logger = logger;
            this._context = context;
        }

        public async Task<bool> DeleteSuspectQualification(SuspectQualification suspectQualification)
        {
            suspectQualification.IsDeleted = true;
            _context.SuspectQualifications.Update(suspectQualification);
            return await SaveChanges();
        }

        public async Task<IEnumerable<SuspectQualification>> FindAllSuspectQualifications()
        {
            return await _context.SuspectQualifications.AsNoTracking()
               .Include(x => x.Suspect)
               .ThenInclude(x => x.Branch)
               .Include(x => x.Suspect)
               .ThenInclude(x => x.Office)
               .Include(x => x.Suspect)
               .ThenInclude(x => x.GroupType)
               .Where(suspectQualification => !suspectQualification.IsDeleted 
                        && suspectQualification.IsActive && !suspectQualification.Suspect.IsConverted)
               .OrderBy(suspectQualification => suspectQualification.CreatedAt)
               .ToListAsync();
        }

        public async Task<SuspectQualification> FindSuspectQualificationById(long Id)
        {
            return await _context.SuspectQualifications.AsNoTracking()
                .Include(x => x.Suspect)
                .ThenInclude(x => x.Branch)
                .Include(x => x.Suspect)
                .ThenInclude(x => x.Office)
                .Where(suspectQualification => !suspectQualification.IsDeleted)
                .FirstOrDefaultAsync(suspectQualification => suspectQualification.Id == Id && suspectQualification.IsDeleted == false);
        }

        /*public async Task<SuspectQualification> FindSuspectQualificationByName(string name)
        {
            return await _context.SuspectQualifications
                 .Where(suspectQualification => suspectQualification.IsDeleted == false)
                 .FirstOrDefaultAsync(suspectQualification => suspectQualification.Caption == name && suspectQualification.IsDeleted == false);
        }*/

        public async Task<SuspectQualification> SaveSuspectQualification(SuspectQualification suspectQualification)
        {
            var suspectQualificationEntity = await _context.SuspectQualifications.AddAsync(suspectQualification);
            if (await SaveChanges())
            {
                return suspectQualificationEntity.Entity;
            }
            return null;
        }

        public async Task<SuspectQualification> UpdateSuspectQualification(SuspectQualification suspectQualification)
        {
            var suspectQualificationEntity = _context.SuspectQualifications.Update(suspectQualification);
            if (await SaveChanges())
            {
                return suspectQualificationEntity.Entity;
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
