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
    public class EvidenceRepositoryImpl : IEvidenceRepository
    {
        private readonly HalobizContext _context;
        private readonly ILogger<EvidenceRepositoryImpl> _logger;
        public EvidenceRepositoryImpl(HalobizContext context, ILogger<EvidenceRepositoryImpl> logger)
        {
            this._logger = logger;
            this._context = context;
        }

        public async Task<bool> DeleteEvidence(Evidence evidence)
        {
            evidence.IsDeleted = true;
            _context.Evidences.Update(evidence);
            return await SaveChanges();
        }

        public async Task<IEnumerable<Evidence>> FindAllEvidences()
        {
            return await _context.Evidences
               .Where(evidence => evidence.IsDeleted == false)
               .OrderBy(evidence => evidence.CreatedAt)
               .ToListAsync();
        }

        public async Task<Evidence> FindEvidenceById(long Id)
        {
            return await _context.Evidences
                .Where(evidence => evidence.IsDeleted == false)
                .FirstOrDefaultAsync(evidence => evidence.Id == Id && evidence.IsDeleted == false);

        }

        public async Task<Evidence> FindEvidenceByName(string name)
        {
            return await _context.Evidences
                 .Where(evidence => evidence.IsDeleted == false)
                 .FirstOrDefaultAsync(evidence => evidence.Caption == name && evidence.IsDeleted == false);

        }

        public async Task<Evidence> SaveEvidence(Evidence evidence)
        {
            var evidenceEntity = await _context.Evidences.AddAsync(evidence);
            if (await SaveChanges())
            {
                return evidenceEntity.Entity;
            }
            return null;
        }

        public async Task<Evidence> UpdateEvidence(Evidence evidence)
        {
            var evidenceEntity = _context.Evidences.Update(evidence);
            if (await SaveChanges())
            {
                return evidenceEntity.Entity;
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
