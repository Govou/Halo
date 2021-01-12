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
    public class LeadOriginRepositoryImpl : ILeadOriginRepository
    {
        private readonly DataContext _context;
        private readonly ILogger<LeadOriginRepositoryImpl> _logger;
        public LeadOriginRepositoryImpl(DataContext context, ILogger<LeadOriginRepositoryImpl> logger)
        {
            this._logger = logger;
            this._context = context;

        }

        public async Task<LeadOrigin> SaveLeadOrigin(LeadOrigin leadOrigin)
        {
            var leadOriginEntity = await _context.LeadOrigins.AddAsync(leadOrigin);
            if(await SaveChanges())
            {
                return leadOriginEntity.Entity;
            }
            return null;
        }

        public async Task<LeadOrigin> FindLeadOriginById(long Id)
        {
            return await _context.LeadOrigins
                .FirstOrDefaultAsync( leadOrigin => leadOrigin.Id == Id && leadOrigin.IsDeleted == false);
        }

        public async Task<LeadOrigin> FindLeadOriginByName(string name)
        {
            return await _context.LeadOrigins
                .FirstOrDefaultAsync( leadOrigin => leadOrigin.Caption == name && leadOrigin.IsDeleted == false);
        }

        public async Task<IEnumerable<LeadOrigin>> FindAllLeadOrigin()
        {
            return await _context.LeadOrigins   
                .Where(leadOrigin => leadOrigin.IsDeleted == false)
                .OrderBy(leadOrigin => leadOrigin.CreatedAt)
                .ToListAsync();
        }

        public async Task<LeadOrigin> UpdateLeadOrigin(LeadOrigin leadOrigin)
        {
            var leadOriginEntity =  _context.LeadOrigins.Update(leadOrigin);
            if(await SaveChanges())
            {
                return leadOriginEntity.Entity;
            }
            return null;
        }

        public async Task<bool> DeleteLeadOrigin(LeadOrigin leadOrigin)
        {
            leadOrigin.IsDeleted = true;
            _context.LeadOrigins.Update(leadOrigin);
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