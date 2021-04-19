using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HaloBiz.Data;
using HaloBiz.Model.LAMS;
using HaloBiz.Repository.LAMS;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace HaloBiz.Repository.Impl.LAMS
{
    public class LeadEngagementRepositoryImpl : ILeadEngagementRepository
    {
        private readonly DataContext _context;
        private readonly ILogger<LeadEngagementRepositoryImpl> _logger;
        public LeadEngagementRepositoryImpl(DataContext context, ILogger<LeadEngagementRepositoryImpl> logger)
        {
            this._logger = logger;
            this._context = context;
        }

        public async Task<LeadEngagement> SaveLeadEngagement(LeadEngagement leadEngagement)
        {
            var leadEngagementEntity = await _context.LeadEngagements.AddAsync(leadEngagement);
            if(await SaveChanges())
            {
                return leadEngagementEntity.Entity;
            }
            return null;
        }

        public async Task<LeadEngagement> FindLeadEngagementById(long Id)
        {
            return await _context.LeadEngagements
                .FirstOrDefaultAsync( leadEngagement => leadEngagement.Id == Id && leadEngagement.IsDeleted == false);
        }

        public async Task<List<LeadEngagement>> FindLeadEngagementsByLeadId(long leadId)
        {
            return await _context.LeadEngagements
                .Where(leadEngagement => leadEngagement.LeadId == leadId && leadEngagement.IsDeleted == false)
                .ToListAsync();
        }

        public async Task<LeadEngagement> FindLeadEngagementByName(string name)
        {
            return await _context.LeadEngagements
                .FirstOrDefaultAsync( leadEngagement => leadEngagement.Caption == name && leadEngagement.IsDeleted == false);
        }

        public async Task<IEnumerable<LeadEngagement>> FindAllLeadEngagement()
        {
            return await _context.LeadEngagements
                .Where(leadEngagement => leadEngagement.IsDeleted == false)
                .OrderBy(leadEngagement => leadEngagement.CreatedAt)
                .ToListAsync();
        }

        public async Task<LeadEngagement> UpdateLeadEngagement(LeadEngagement leadEngagement)
        {
            var leadEngagementEntity =  _context.LeadEngagements.Update(leadEngagement);
            if(await SaveChanges())
            {
                return leadEngagementEntity.Entity;
            }
            return null;
        }

        public async Task<bool> DeleteLeadEngagement(LeadEngagement leadEngagement)
        {
            leadEngagement.IsDeleted = true;
            _context.LeadEngagements.Update(leadEngagement);
            return await SaveChanges();
        }
        private async Task<bool> SaveChanges()
        {
           try{
               return  await _context.SaveChangesAsync() > 0;
           }catch(Exception ex)
           {
               _logger.LogError(ex.Message);
               return false;
           }
        }
    }
}