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
    public class QuoteServiceRepositoryImpl : IQuoteServiceRepository
    {
        private readonly DataContext _context;
        private readonly ILogger<QuoteServiceRepositoryImpl> _logger;
        public QuoteServiceRepositoryImpl(DataContext context, ILogger<QuoteServiceRepositoryImpl> logger)
        {
            this._logger = logger;
            this._context = context;
        }

        public async Task<QuoteService> SaveQuoteService(QuoteService quoteService)
        {
            var quoteServiceEntity = await _context.QuoteServices.AddAsync(quoteService);
            if(await SaveChanges())
            {
                return quoteServiceEntity.Entity;
            }
            return null;
        }

        public async Task<bool> SaveQuoteServiceRange(IEnumerable<QuoteService> quoteServices)
        {
            await _context.QuoteServices.AddRangeAsync(quoteServices);
            return await SaveChanges();
        }

        public async Task<QuoteService> FindQuoteServiceById(long Id)
        {
            var quoteService = await _context.QuoteServices
                .Include(x => x.QuoteServiceDocuments)
                .Include(x => x.SBUToQuoteServiceProportions).ThenInclude(x => x.UserInvolved)
                .Include(x => x.ContractServices)
                .Include(x => x.CreatedBy)
                .FirstOrDefaultAsync( quoteService => quoteService.Id == Id && quoteService.IsDeleted == false);
            quoteService.Service = await _context.Services.FirstOrDefaultAsync(x => x.Id == quoteService.ServiceId && !x.IsDeleted);
            return quoteService;
        }

        public async Task<IEnumerable<QuoteService>> FindAllQuoteService()
        {
            return await _context.QuoteServices
                .Include(x => x.QuoteServiceDocuments)
                .Include(x => x.SBUToQuoteServiceProportions)
                .Include(x => x.ContractServices)
                .Where(quoteService => quoteService.IsDeleted == false)
                .OrderBy(quoteService => quoteService.CreatedAt)
                .ToListAsync();
        }

        public async Task<QuoteService> UpdateQuoteService(QuoteService quoteService)
        {
            var quoteServiceEntity =  _context.QuoteServices.Update(quoteService);
            if(await SaveChanges())
            {
                return quoteServiceEntity.Entity;
            }
            return null;
        }

        public async Task<bool> DeleteQuoteService(QuoteService quoteService)
        {
            quoteService.IsDeleted = true;
            _context.QuoteServices.Update(quoteService);
            return await SaveChanges();
        }

        public async Task<bool> DeleteQuoteServiceRange(IEnumerable<QuoteService> quoteServices)
        {
            foreach (var qs in quoteServices)
            {
                qs.IsDeleted = true;
            }
            _context.QuoteServices.UpdateRange(quoteServices);
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