using HalobizMigrations.Data;
using HalobizMigrations.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HaloBiz.Repository.Impl
{
    public class ProspectRepositoryImpl : IProspectRepository
    {
        private readonly HalobizContext _context;
        private readonly ILogger<ProspectRepositoryImpl> _logger;
        public ProspectRepositoryImpl(HalobizContext context, ILogger<ProspectRepositoryImpl> logger)
        {
            this._logger = logger;
            this._context = context;
        }

        public async Task<bool> DeleteProspect(Prospect prospect)
        {
            prospect.IsDeleted = true;
            _context.Prospects.Update(prospect);
            return await SaveChanges();
        }

        public async Task<IEnumerable<Prospect>> FindAllProspects()
        {
            return await _context.Prospects
               .Where(prospect => prospect.IsDeleted == false)
               .OrderBy(prospect => prospect.CreatedAt)
               .ToListAsync();
        }

        public async Task<Prospect> FindProspectById(long Id)
        {
            return await _context.Prospects
                .Where(prospect => prospect.IsDeleted == false)
                .FirstOrDefaultAsync(prospect => prospect.Id == Id && prospect.IsDeleted == false);

        }

        public async Task<Prospect> FindProspectByEmail(string email)
        {
            return await _context.Prospects
                .Where(prospect => !prospect.IsDeleted && prospect.Email.ToLower() == email.ToLower())
                .FirstOrDefaultAsync();
        }

        public async Task<Prospect> SaveProspect(Prospect prospect)
        {
            var prospectEntity = await _context.Prospects.AddAsync(prospect);
            if (await SaveChanges())
            {
                return prospectEntity.Entity;
            }
            return null;
        }

        public async Task<Prospect> UpdateProspect(Prospect prospect)
        {
            var prospectEntity = _context.Prospects.Update(prospect);
            if (await SaveChanges())
            {
                return prospectEntity.Entity;
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
