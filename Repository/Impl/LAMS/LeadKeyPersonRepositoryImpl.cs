using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HaloBiz.Data;
using HaloBiz.Model.LAMS;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace HaloBiz.Repository.Impl.LAMS
{
    public class LeadKeyPersonRepositoryImpl : ILeadKeyPersonRepository
    {
        private readonly DataContext _context;
        private readonly ILogger<LeadKeyPersonRepositoryImpl> _logger;
        public LeadKeyPersonRepositoryImpl(DataContext context, ILogger<LeadKeyPersonRepositoryImpl> logger)
        {
            this._logger = logger;
            this._context = context;
        }

        public async Task<LeadKeyPerson> SaveLeadKeyPerson(LeadKeyPerson entity)
        {
            var leadKeyPersonEntity = await _context.LeadKeyPeople.AddAsync(entity);
            if(! await SaveChanges())
            {
                return null;
            }
            return leadKeyPersonEntity.Entity;
        }

        public async Task<LeadKeyPerson> FindLeadKeyPersonById(long Id)
        {
            return await _context.LeadKeyPeople
                .Include(x => x.Lead)
                .FirstOrDefaultAsync( entity => entity.Id == Id && entity.IsDeleted == false);
        }

        public async Task<IEnumerable<LeadKeyPerson>> FindAllLeadKeyPerson()
        {
            return await _context.LeadKeyPeople   
                .Include(x => x.Lead)
                .Where(entity => entity.IsDeleted == false)
                .OrderByDescending(entity => entity.FirstName)
                .ToListAsync();
        }

        public async  Task<LeadKeyPerson> UpdateLeadKeyPerson(LeadKeyPerson entity)
        {
            var leadKeyPersonEntity =  _context.LeadKeyPeople.Update(entity);
            if(! await SaveChanges())
            {
                return null;
            }
            return leadKeyPersonEntity.Entity;
            
        }

        public async Task<bool> DeleteLeadKeyPerson(LeadKeyPerson entity)
        {
            entity.IsDeleted = true;
            _context.LeadKeyPeople.Update(entity);
            return await SaveChanges();
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

         public async Task<IEnumerable<LeadKeyPerson>> FindAllLeadKeyPersonByLeadId(long leadId)
        {
            return await _context.LeadKeyPeople
                    .Include(x => x.Lead)
                    .Where(x => x.LeadId == leadId && x.IsDeleted == false)
                    .ToListAsync();
        }
    }
}