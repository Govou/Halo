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
    public class LeadDivisionContactRepositoryImpl : ILeadDivisionContactRepository
    {
        private readonly DataContext _context;
        private readonly ILogger<LeadDivisionContactRepositoryImpl> _logger;
        public LeadDivisionContactRepositoryImpl(DataContext context, ILogger<LeadDivisionContactRepositoryImpl> logger)
        {
            this._logger = logger;
            this._context = context;

        }

        public async Task<LeadDivisionContact> SaveLeadDivisionContact(LeadDivisionContact entity)
        {
            var LeadDivisionContactEntity = await _context.LeadDivisionContacts.AddAsync(entity);
            return LeadDivisionContactEntity.Entity;
        }

        public async Task<LeadDivisionContact> FindLeadDivisionContactById(long Id)
        {
            return await _context.LeadDivisionContacts
                .FirstOrDefaultAsync(entity => entity.Id == Id && entity.IsDeleted == false);
        }

        public async Task<IEnumerable<LeadDivisionContact>> FindAllLeadDivisionContact()
        {
            return await _context.LeadDivisionContacts
                .Where(entity => entity.IsDeleted == false)
                .OrderByDescending(entity => entity.FirstName)
                .ToListAsync();
        }

        public async Task<LeadDivisionContact> UpdateLeadDivisionContact(LeadDivisionContact entity)
        {
            var LeadDivisionContactEntity = _context.LeadDivisionContacts.Update(entity);
            if (await SaveChanges())
            {
                return LeadDivisionContactEntity.Entity;
            }
            return null;
        }

        public async Task<bool> DeleteLeadDivisionContact(LeadDivisionContact entity)
        {
            entity.IsDeleted = true;
            _context.LeadDivisionContacts.Update(entity);
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
    }
}
