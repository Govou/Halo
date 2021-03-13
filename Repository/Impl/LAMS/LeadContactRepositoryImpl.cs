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
    public class LeadContactRepositoryImpl : ILeadContactRepository
    {
        private readonly DataContext _context;
        private readonly ILogger<LeadContactRepositoryImpl> _logger;
        public LeadContactRepositoryImpl(DataContext context, ILogger<LeadContactRepositoryImpl> logger)
        {
            this._logger = logger;
            this._context = context;

        }

        public async Task<LeadContact> SaveLeadContact(LeadContact entity)
        {
            var leadContactEntity = await _context.LeadContacts.AddAsync(entity);
            return leadContactEntity.Entity;
        }

        public async Task<LeadContact> FindLeadContactById(long Id)
        {
            return await _context.LeadContacts
                .FirstOrDefaultAsync( entity => entity.Id == Id && entity.IsDeleted == false);
        }

        public async Task<IEnumerable<LeadContact>> FindAllLeadContact()
        {
            return await _context.LeadContacts   
                .Where(entity => entity.IsDeleted == false)
                .OrderByDescending(entity => entity.FirstName)
                .ToListAsync();
        }

        public LeadContact UpdateLeadContact(LeadContact entity)
        {
            var leadContactEntity =  _context.LeadContacts.Update(entity);
            return leadContactEntity.Entity;
        }

        public async Task<bool> DeleteLeadContact(LeadContact entity)
        {
            entity.IsDeleted = true;
            _context.LeadContacts.Update(entity);
            return await SaveChanges();
        }
        public async Task<bool> SaveChanges()
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