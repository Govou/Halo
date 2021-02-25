using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HaloBiz.Data;
using HaloBiz.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace HaloBiz.Repository.Impl
{

    public class ApproverLevelRepositoryImpl : IApproverLevelRepository
    {
        private readonly DataContext _context;
        private readonly ILogger<ApproverLevelRepositoryImpl> _logger;
        public ApproverLevelRepositoryImpl(DataContext context, ILogger<ApproverLevelRepositoryImpl> logger)
        {
            this._logger = logger;
            this._context = context;
        }
        public async Task<ApproverLevel> SaveApproverLevel(ApproverLevel approverLevel)
        {
            var approverLevelEntity = await _context.ApproverLevels.AddAsync(approverLevel);
            if(await SaveChanges())
            {
                return approverLevelEntity.Entity;
            }
            return null;
        }
        public async Task<IEnumerable<ApproverLevel>> GetApproverLevels()
        {
            return await _context.ApproverLevels
                .Where(x => x.IsDeleted == false)
                .OrderBy(x => x.Caption)
                .ToListAsync();
        }

        public async Task<ApproverLevel> UpdateApproverLevel(ApproverLevel approverLevel)
        {
             var approverLevelEntity =  _context.ApproverLevels.Update(approverLevel);
            if(await SaveChanges())
            {
                return approverLevelEntity.Entity;
            }
            return null;
        }
        public async Task<bool> DeleteApproverLevel(ApproverLevel approverLevel)
        {
            approverLevel.IsDeleted = true;
            _context.ApproverLevels.Update(approverLevel);
            return await SaveChanges();
        }
        public async Task<ApproverLevel> FindApproverLevelById(long Id)
        {
           return await _context.ApproverLevels
            .FirstOrDefaultAsync(x => x.Id == Id && x.IsDeleted == false);
        }

        private async Task<bool> SaveChanges()
        {
           try
           {
               return  await _context.SaveChangesAsync() > 0;
           }
           catch(Exception ex)
           {
               _logger.LogError(ex.Message);
               return false;
           }
        }
    }
}