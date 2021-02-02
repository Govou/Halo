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
    public class LeadDivisionKeyPersonRepositoryImpl : ILeadDivisionKeyPersonRepository
    {
        private readonly DataContext _context;
        private readonly ILogger<LeadDivisionKeyPersonRepositoryImpl> _logger;
        public LeadDivisionKeyPersonRepositoryImpl(DataContext context, ILogger<LeadDivisionKeyPersonRepositoryImpl> logger)
        {
            this._logger = logger;
            this._context = context;

        }

        public async Task<LeadDivisionKeyPerson> SaveLeadDivisionKeyPerson(LeadDivisionKeyPerson entity)
        {
            var LeadDivisionKeyPersonEntity = await _context.LeadDivisionKeyPeople.AddAsync(entity);
             if(! await SaveChanges())
            {
                return null;
            }
            return LeadDivisionKeyPersonEntity.Entity;
        }

        public async Task<LeadDivisionKeyPerson> FindLeadDivisionKeyPersonById(long Id)
        {
            return await _context.LeadDivisionKeyPeople
                .FirstOrDefaultAsync(entity => entity.Id == Id && entity.IsDeleted == false);
        }

        public async Task<IEnumerable<LeadDivisionKeyPerson>> FindAllLeadDivisionKeyPerson()
        {
            return await _context.LeadDivisionKeyPeople
                .Where(entity => entity.IsDeleted == false)
                .OrderByDescending(entity => entity.FirstName)
                .ToListAsync();
        }

        public async Task<LeadDivisionKeyPerson> UpdateLeadDivisionKeyPerson(LeadDivisionKeyPerson entity)
        {
            var LeadDivisionKeyPersonEntity = _context.LeadDivisionKeyPeople.Update(entity);
            if (await SaveChanges())
            {
                return LeadDivisionKeyPersonEntity.Entity;
            }
            return null;
        }

        public async Task<bool> DeleteLeadDivisionKeyPerson(LeadDivisionKeyPerson entity)
        {
            entity.IsDeleted = true;
            _context.LeadDivisionKeyPeople.Update(entity);
            return await SaveChanges();
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

        public async Task<IEnumerable<LeadDivisionKeyPerson>> FindAllLeadDivisionKeyPersonByLeadDivisionId(long leadDivisionId)
        {
            return await _context.LeadDivisionKeyPeople
                    .Where(x => x.LeadDivisionId == leadDivisionId && x.IsDeleted == false)
                    .ToListAsync();
        }
    }
}
