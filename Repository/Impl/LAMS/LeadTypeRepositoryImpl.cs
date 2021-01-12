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
    public class LeadTypeRepositoryImpl : ILeadTypeRepository
    {
        private readonly DataContext _context;
        private readonly ILogger<LeadTypeRepositoryImpl> _logger;
        public LeadTypeRepositoryImpl(DataContext context, ILogger<LeadTypeRepositoryImpl> logger)
        {
            this._logger = logger;
            this._context = context;
        }

        public async Task<LeadType> SaveLeadType(LeadType leadType)
        {
            var leadTypeEntity = await _context.LeadTypes.AddAsync(leadType);
            if(await SaveChanges())
            {
                return leadTypeEntity.Entity;
            }
            return null;
        }

        public async Task<LeadType> FindLeadTypeById(long Id)
        {
            return await _context.LeadTypes
                .Include(leadType => leadType.LeadOrigins
                    .Where(leadOrigin => leadOrigin.IsDeleted == false))
                .FirstOrDefaultAsync( leadType => leadType.Id == Id && leadType.IsDeleted == false);
        }

        public async Task<LeadType> FindLeadTypeByName(string name)
        {
            return await _context.LeadTypes
                .Include(leadType => leadType.LeadOrigins
                    .Where(leadOrigin => leadOrigin.IsDeleted == false))
                .FirstOrDefaultAsync( leadType => leadType.Caption == name && leadType.IsDeleted == false);
        }

        public async Task<IEnumerable<LeadType>> FindAllLeadType()
        {
            return await _context.LeadTypes   
                .Include(leadType => leadType.LeadOrigins
                    .Where(leadOrigin => leadOrigin.IsDeleted == false))
                .Where(leadType => leadType.IsDeleted == false)
                .OrderBy(leadType => leadType.CreatedAt)
                .ToListAsync();
        }

        public async Task<LeadType> UpdateLeadType(LeadType leadType)
        {
            var leadTypeEntity =  _context.LeadTypes.Update(leadType);
            if(await SaveChanges())
            {
                return leadTypeEntity.Entity;
            }
            return null;
        }

        public async Task<bool> DeleteLeadType(LeadType leadType)
        {
            leadType.IsDeleted = true;
            _context.LeadTypes.Update(leadType);
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