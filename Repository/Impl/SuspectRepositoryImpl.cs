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
    public class SuspectRepositoryImpl : ISuspectRepository
    {
        private readonly HalobizContext _context;
        private readonly ILogger<SuspectRepositoryImpl> _logger;
        public SuspectRepositoryImpl(HalobizContext context, ILogger<SuspectRepositoryImpl> logger)
        {
            this._logger = logger;
            this._context = context;
        }

        public async Task<bool> DeleteSuspect(Suspect suspect)
        {
            suspect.IsDeleted = true;
            _context.Suspects.Update(suspect);
            return await SaveChanges();
        }

        public async Task<IEnumerable<Suspect>> FindAllSuspects()
        {
            return await _context.Suspects
               .Include(x => x.LeadOrigin)
               .Include(x => x.GroupType)
               .Include(x => x.Branch)
               .Include(x => x.Office)
               .Where(suspect => suspect.IsDeleted == false)
               .OrderBy(suspect => suspect.CreatedAt)
               .ToListAsync();
        }

        public async Task<Suspect> FindSuspectById(long Id)
        {
            return await _context.Suspects
                .Include(x => x.LeadOrigin)
                .Include(x => x.GroupType)
                .Include(x => x.Branch)
                .Include(x => x.Office)
                .Include(x => x.State)
                .Include(x => x.Lga)
                .Include(x => x.Industry)
                .Include(x => x.LeadType)
                .Include(x => x.SuspectQualifications.Where(x => !x.IsDeleted && x.IsActive))
                .ThenInclude(x => x.ServiceQualifications)
                .ThenInclude(x => x.Service)
                .FirstOrDefaultAsync(suspect => !suspect.IsDeleted && suspect.Id == Id);
        }

        /*public async Task<Suspect> FindSuspectByName(string name)
        {
            return await _context.Suspects
                 .Where(suspect => suspect.IsDeleted == false)
                 .FirstOrDefaultAsync(suspect => suspect.Caption == name && suspect.IsDeleted == false);
        }*/

        public async Task<Suspect> SaveSuspect(Suspect suspect)
        {
            var suspectEntity = await _context.Suspects.AddAsync(suspect);
            if (await SaveChanges())
            {
                return suspectEntity.Entity;
            }
            return null;
        }

        public async Task<Suspect> UpdateSuspect(Suspect suspect)
        {
            var suspectEntity = _context.Suspects.Update(suspect);
            if (await SaveChanges())
            {
                return suspectEntity.Entity;
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
