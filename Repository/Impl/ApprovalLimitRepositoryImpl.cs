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

    public class ApprovalLimitRepositoryImpl : IApprovalLimitRepository
    {
        private readonly DataContext _context;
        private readonly ILogger<ApprovalLimitRepositoryImpl> _logger;
        public ApprovalLimitRepositoryImpl(DataContext context, ILogger<ApprovalLimitRepositoryImpl> logger)
        {
            this._logger = logger;
            this._context = context;
        }
        public async Task<ApprovalLimit> SaveApprovalLimit(ApprovalLimit approvalLimit)
        {
            var approvalLimitEntity = await _context.ApprovalLimits.AddAsync(approvalLimit);
            if(await SaveChanges())
            {
                return approvalLimitEntity.Entity;
            }
            return null;
        }
        public async Task<IEnumerable<ApprovalLimit>> GetApprovalLimits()
        {
            return await _context.ApprovalLimits
                .Where(x => x.IsDeleted == false)
                .Include(x => x.ApproverLevel)
                .Include(x => x.ProcessesRequiringApproval)
                .OrderBy(x => x.Caption)
                .ToListAsync();
        }

        public async Task<ApprovalLimit> UpdateApprovalLimit(ApprovalLimit approvalLimit)
        {
             var approvalLimitEntity =  _context.ApprovalLimits.Update(approvalLimit);
            if(await SaveChanges())
            {
                return approvalLimitEntity.Entity;
            }
            return null;
        }
        public async Task<bool> DeleteApprovalLimit(ApprovalLimit approvalLimit)
        {
            approvalLimit.IsDeleted = true;
            _context.ApprovalLimits.Update(approvalLimit);
            return await SaveChanges();
        }
        public async Task<ApprovalLimit> FindApprovalLimitById(long Id)
        {
           return await _context.ApprovalLimits
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

        public async Task<IEnumerable<ApprovalLimit>> GetApprovalLimitsByModule(long moduleId)
        {
            return await _context.ApprovalLimits
                .Where(x => x.ProcessesRequiringApprovalId == moduleId && x.IsBypassRequired == false && x.IsDeleted == false)
                .Include(x => x.ApproverLevel)
                .Include(x => x.ProcessesRequiringApproval)
                .OrderBy(x => x.Caption)
                .ToListAsync();
        }
    }
}