using HaloBiz.Data;
using HaloBiz.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HaloBiz.Repository.Impl
{
    public class TargetRepositoryImpl : ITargetRepository
    {
        private readonly DataContext _context;
        private readonly ILogger<TargetRepositoryImpl> _logger;
        public TargetRepositoryImpl(DataContext context, ILogger<TargetRepositoryImpl> logger)
        {
            this._logger = logger;
            this._context = context;
        }

        public async Task<bool> DeleteTarget(Target target)
        {
            target.IsDeleted = true;
            _context.Targets.Update(target);
            return await SaveChanges();
        }

        public async Task<IEnumerable<Target>> FindAllTargets()
        {
            return await _context.Targets
               .Where(target => target.IsDeleted == false)
               .OrderBy(target => target.CreatedAt)
               .ToListAsync();
        }

        public async Task<Target> FindTargetById(long Id)
        {
            return await _context.Targets
                .Where(target => target.IsDeleted == false)
                .FirstOrDefaultAsync(target => target.Id == Id && target.IsDeleted == false);

        }

        public async Task<Target> FindTargetByName(string name)
        {
            return await _context.Targets
                 .Where(target => target.IsDeleted == false)
                 .FirstOrDefaultAsync(target => target.Caption == name && target.IsDeleted == false);

        }

        public async Task<Target> SaveTarget(Target target)
        {
            var targetEntity = await _context.Targets.AddAsync(target);
            if (await SaveChanges())
            {
                return targetEntity.Entity;
            }
            return null;
        }

        public async Task<Target> UpdateTarget(Target target)
        {
            var targetEntity = _context.Targets.Update(target);
            if (await SaveChanges())
            {
                return targetEntity.Entity;
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
