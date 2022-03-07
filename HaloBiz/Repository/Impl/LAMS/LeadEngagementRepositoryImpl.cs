using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HalobizMigrations.Data;
using HalobizMigrations.Models;
using HaloBiz.Repository.LAMS;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace HaloBiz.Repository.Impl.LAMS
{
    public class LeadEngagementRepositoryImpl : ILeadEngagementRepository
    {
        private readonly HalobizContext _context;
        private readonly ILogger<LeadEngagementRepositoryImpl> _logger;
        public LeadEngagementRepositoryImpl(HalobizContext context, ILogger<LeadEngagementRepositoryImpl> logger)
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
                .Include(x => x.EngagementType)
                .Include(x => x.EngagementReason)
                //.Include(x => x.LeadDivisionKeyPersonLeadEngagements)
                //.Include(x => x.LeadDivisionContactLeadEngagements)
                //.Include(x => x.LeadEngagementUserProfiles)
                .FirstOrDefaultAsync( leadEngagement => leadEngagement.Id == Id && leadEngagement.IsDeleted == false);
        }

        public async Task<List<LeadEngagement>> FindLeadEngagementsByLeadId(long leadId)
        {
            return await _context.LeadEngagements
                .Include(x => x.EngagementType)
                .Include(x => x.EngagementReason)
                //.Include(x => x.LeadDivisionKeyPersonLeadEngagements)
                //.Include(x => x.LeadDivisionContactLeadEngagements)
                //.Include(x => x.LeadEngagementUserProfiles)
                .Where(leadEngagement => leadEngagement.LeadId == leadId && leadEngagement.IsDeleted == false)
                .OrderBy(leadEngagement => leadEngagement.Date)
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
                .Include(x => x.EngagementType)
                .Where(leadEngagement => leadEngagement.IsDeleted == false)
                .OrderBy(leadEngagement => leadEngagement.Date)
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