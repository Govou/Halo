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
    public class LeadDivisionRepositoryImpl : ILeadDivisionRepository
    {
        private readonly DataContext _context;
        private readonly ILogger<LeadDivisionRepositoryImpl> _logger;
        public LeadDivisionRepositoryImpl(DataContext context, ILogger<LeadDivisionRepositoryImpl> logger)
        {
            this._logger = logger;
            this._context = context;
        }

        public async Task<LeadDivision> SaveLeadDivision(LeadDivision leadDivision)
        {
            var leadDivisionEntity = await _context.LeadDivisions.AddAsync(leadDivision);
            if(await SaveChanges())
            {
                return leadDivisionEntity.Entity;
            }
            return null;
        }

        public async Task<LeadDivision> FindLeadDivisionById(long Id)
        {
            return await _context.LeadDivisions
                .Include(x => x.Branch)
                .Include(x => x.Office)
                .Include(x => x.PrimaryContact)
                .Include(x => x.SecondaryContact)
                .Include(x => x.LeadDivisionKeyPersons)
                .Include(x => x.LeadOrigin)
                .Include(x => x.Quote)
                .FirstOrDefaultAsync( leadDivision => leadDivision.Id == Id && leadDivision.IsDeleted == false);
        }

        public async Task<LeadDivision> FindLeadDivisionByName(string name)
        {
            return await _context.LeadDivisions
                .Include(x => x.Branch)
                .Include(x => x.Office)
                .Include(x => x.PrimaryContact)
                .Include(x => x.SecondaryContact)
                .Include(x => x.LeadDivisionKeyPersons)
                .Include(x => x.LeadOrigin)
                .Include(x => x.Quote)
                .FirstOrDefaultAsync( leadDivision => leadDivision.DivisionName == name && leadDivision.IsDeleted == false);
        }

        public async Task<LeadDivision> FindLeadDivisionByRCNumber(string rcNumber)
        {
            return await _context.LeadDivisions
                .Include(x => x.Branch)
                .Include(x => x.Office)
                .Include(x => x.PrimaryContact)
                .Include(x => x.SecondaryContact)
                .Include(x => x.LeadDivisionKeyPersons)
                .Include(x => x.LeadOrigin)
                .Include(x => x.Quote)
                .FirstOrDefaultAsync(leadDivision => leadDivision.RCNumber == rcNumber && leadDivision.IsDeleted == false);
        }

        public async Task<IEnumerable<LeadDivision>> FindAllLeadDivision()
        {
            return await _context.LeadDivisions
                .Include(x => x.Branch)
                .Include(x => x.Office)
                .Include(x => x.PrimaryContact)
                .Include(x => x.SecondaryContact)
                .Include(x => x.LeadDivisionKeyPersons)
                .Include(x => x.LeadOrigin)
                .Include(x => x.Quote)
                .Where(leadDivision => leadDivision.IsDeleted == false)
                .OrderBy(leadDivision => leadDivision.CreatedAt)
                .ToListAsync();
        }

        public async Task<LeadDivision> UpdateLeadDivision(LeadDivision leadDivision)
        {
            var leadDivisionEntity =  _context.LeadDivisions.Update(leadDivision);
            if(await SaveChanges())
            {
                return leadDivisionEntity.Entity;
            }
            return null;
        }

        public async Task<bool> DeleteLeadDivision(LeadDivision leadDivision)
        {
            leadDivision.IsDeleted = true;
            _context.LeadDivisions.Update(leadDivision);
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