using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HaloBiz.Data;
using HaloBiz.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace HaloBiz.Repository.Impl
{

    public class IndustryRepositoryImpl : IIndustryRepository
    {
        private readonly DataContext _context;
        private readonly ILogger<IndustryRepositoryImpl> _logger;
        public IndustryRepositoryImpl(DataContext context, ILogger<IndustryRepositoryImpl> logger)
        {
            this._logger = logger;
            this._context = context;
        }
        public async Task<Industry> SaveIndustry(Industry industry)
        {
            var industryEntity = await _context.Industries.AddAsync(industry);
            if(await SaveChanges())
            {
                return industryEntity.Entity;
            }
            return null;
        }
        public async Task<IEnumerable<Industry>> GetIndustries()
        {
            return await _context.Industries
                .Where(x => x.IsDeleted == false)
                .OrderBy(x => x.Caption)
                .ToListAsync();
        }

        public async Task<Industry> UpdateIndustry(Industry industry)
        {
             var industryEntity =  _context.Industries.Update(industry);
            if(await SaveChanges())
            {
                return industryEntity.Entity;
            }
            return null;
        }
        public async Task<bool> DeleteIndustry(Industry industry)
        {
            industry.IsDeleted = true;
            _context.Industries.Update(industry);
            return await SaveChanges();
        }
        public async Task<Industry> FindIndustryById(long Id)
        {
           return await _context.Industries
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