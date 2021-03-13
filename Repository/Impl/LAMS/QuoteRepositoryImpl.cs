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
    public class QuoteRepositoryImpl : IQuoteRepository
    {
        private readonly DataContext _context;
        private readonly ILogger<QuoteRepositoryImpl> _logger;
        private readonly IQuoteServiceRepository _quoteServiceRepository;
        public QuoteRepositoryImpl(DataContext context, ILogger<QuoteRepositoryImpl> logger, IQuoteServiceRepository quoteServiceRepository)
        {
            this._logger = logger;
            this._context = context;
            this._quoteServiceRepository = quoteServiceRepository;
        }

        public async Task<Quote> SaveQuote(Quote quote)
        {
            var quoteEntity = await _context.Quotes.AddAsync(quote);
            if(await SaveChanges())
            {
                return quoteEntity.Entity;
            }
            return null;
        }

        public async Task<Quote> FindQuoteById(long Id)
        {
            return await _context.Quotes.Include(a => a.QuoteServices.Where(a => a.IsDeleted == false))
                .FirstOrDefaultAsync( quote => quote.Id == Id && quote.IsDeleted == false);
        }

        public async Task<Quote> FindQuoteByReferenceNumber(string referenceNumber)
        {
            return await _context.Quotes.Include(a => a.QuoteServices.Where(a => a.IsDeleted == false))
                .FirstOrDefaultAsync( quote => quote.ReferenceNo == referenceNumber && quote.IsDeleted == false);
        }

        public async Task<IEnumerable<Quote>> FindAllQuote()
        {
            return await _context.Quotes.Include(a => a.QuoteServices.Where(a => a.IsDeleted == false))
                .Where(quote => quote.IsDeleted == false)
                .OrderBy(quote => quote.CreatedAt)
                .ToListAsync();
        }

        public async Task<Quote> UpdateQuote(Quote quote)
        {
            var quoteEntity =  _context.Quotes.Update(quote);
            if(await SaveChanges())
            {
                return quoteEntity.Entity;
            }
            return null;
        }

        public async Task<bool> DeleteQuote(Quote quote)
        {
            await _quoteServiceRepository.DeleteQuoteServiceRange(quote.QuoteServices);

            quote.IsDeleted = true;
            _context.Quotes.Update(quote);
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