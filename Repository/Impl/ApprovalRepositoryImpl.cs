using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HalobizMigrations.Data;
using HalobizMigrations.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace HaloBiz.Repository.Impl
{

    public class ApprovalRepositoryImpl : IApprovalRepository
    {
        private readonly HalobizContext _context;
        private readonly ILogger<ApprovalRepositoryImpl> _logger;
        public ApprovalRepositoryImpl(HalobizContext context, ILogger<ApprovalRepositoryImpl> logger)
        {
            this._logger = logger;
            this._context = context;
        }
        public async Task<Approval> SaveApproval(Approval approval)
        {
            var approvalEntity = await _context.Approvals.AddAsync(approval);
            if(await SaveChanges())
            {
                return approvalEntity.Entity;
            }
            return null;
        }

        public async Task<bool> SaveApprovalRange(List<Approval> approvals)
        {
            try
            {
                await _context.Approvals.AddRangeAsync(approvals);
                var affected = await _context.SaveChangesAsync();
                _context.ChangeTracker.Clear();
                return affected > 1 ? true : false;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.StackTrace);
                throw;
            }
        }

        public async Task<IEnumerable<Approval>> GetApprovals()
        {
            return await _context.Approvals
                .Where(x => x.IsDeleted == false)
                .Include(x => x.Responsible)
                .OrderBy(x => x.Caption)
                .ToListAsync();
        }

        public async Task<IEnumerable<Approval>> GetPendingApprovals()
        {
            return await _context.Approvals
                .Where(x => x.IsApproved == false && x.IsDeleted == false)
                .OrderBy(x => x.Caption)
                .ToListAsync();
        }

        public async Task<IEnumerable<Approval>> GetUserPendingApprovals(long userId)
        {
            return await _context.Approvals
                .Where(x => x.IsApproved == false && x.ResponsibleId == userId && x.IsDeleted == false)
                .OrderBy(x => x.Caption)
                .ToListAsync();
        }

        public async Task<IEnumerable<Approval>> GetPendingApprovalsByQuoteId(long quoteId)
        {
            return await _context.Approvals
                .Where(x => x.IsApproved == false && x.QuoteId == quoteId && x.IsDeleted == false)
                .Include(x => x.Responsible)
                .OrderBy(x => x.Sequence)
                .ToListAsync();
        }
        
        public async Task<IEnumerable<Approval>> GetApprovalsByQuoteId(long quoteId)
        {
            return await _context.Approvals
                .Where(x => x.QuoteId == quoteId && x.IsDeleted == false)
                .Include(x => x.Responsible)
                .OrderBy(x => x.Sequence)
                .ToListAsync();
        }
    

        public async Task<IEnumerable<Approval>> GetPendingApprovalsByServiceId(long serviceId)
        {
            return await _context.Approvals
                .Where(x => x.IsApproved == false && x.ServicesId == serviceId && x.IsDeleted == false)
                .Include(x => x.Responsible)
                .OrderBy(x => x.Caption)
                .ToListAsync();
        }
        
        public async Task<IEnumerable<Approval>> GetApprovalsByServiceId(long serviceId)
        {
            return await _context.Approvals
                .Where(x => x.ServicesId == serviceId && x.IsDeleted == false)
                .Include(x => x.Responsible)
                .OrderBy(x => x.Caption)
                .ToListAsync();
        }

        public async Task<IEnumerable<Approval>> GetApprovalsByEndorsementId(long endorsementId)
        {
            return await _context.Approvals
                .Where(x => x.ContractServiceForEndorsementId == endorsementId && x.IsDeleted == false)
                .Include(x => x.Responsible)
                .OrderBy(x => x.Caption)
                .ToListAsync();
        }

        public async Task<Approval> UpdateApproval(Approval approval)
        {
             var approvalEntity =  _context.Approvals.Update(approval);
            if(await SaveChanges())
            {
                return approvalEntity.Entity;
            }
            return null;
        }
        public async Task<bool> DeleteApproval(Approval approval)
        {
            approval.IsDeleted = true;
            _context.Approvals.Update(approval);
            return await SaveChanges();
        }
        public async Task<Approval> FindApprovalById(long Id)
        {
           return await _context.Approvals
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