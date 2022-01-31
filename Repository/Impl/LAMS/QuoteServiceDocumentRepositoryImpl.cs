using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HalobizMigrations.Data;

using HaloBiz.Repository.LAMS;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using HalobizMigrations.Models;

namespace HaloBiz.Repository.Impl.LAMS
{
    public class QuoteServiceDocumentRepositoryImpl : IQuoteServiceDocumentRepository
    {
        private readonly HalobizContext _context;
        private readonly ILogger<QuoteServiceDocumentRepositoryImpl> _logger;
        public QuoteServiceDocumentRepositoryImpl(HalobizContext context, ILogger<QuoteServiceDocumentRepositoryImpl> logger)
        {
            this._logger = logger;
            this._context = context;
        }

        public async Task<QuoteServiceDocument> SaveQuoteServiceDocument(List<QuoteServiceDocument> quoteServiceDocument)
        {
             await _context.QuoteServiceDocuments.AddRangeAsync(quoteServiceDocument);
            
            if(await SaveChanges())
            {
                return quoteServiceDocument.FirstOrDefault();
            }

            return null;
        }

        public async Task<QuoteServiceDocument> FindQuoteServiceDocumentById(long Id)
        {
            return await _context.QuoteServiceDocuments
                .FirstOrDefaultAsync( quoteServiceDocument => quoteServiceDocument.Id == Id && quoteServiceDocument.IsDeleted == false);
        }

        public async Task<QuoteServiceDocument> FindQuoteServiceDocumentByCaption(string caption)
        {
            return await _context.QuoteServiceDocuments
                .FirstOrDefaultAsync( quoteServiceDocument => quoteServiceDocument.Caption == caption && quoteServiceDocument.IsDeleted == false);
        }

        public async Task<IEnumerable<QuoteServiceDocument>> FindAllQuoteServiceDocument()
        {
            return await _context.QuoteServiceDocuments
                .Where(quoteServiceDocument => quoteServiceDocument.IsDeleted == false)
                .OrderBy(quoteServiceDocument => quoteServiceDocument.CreatedAt)
                .ToListAsync();
        }

        public async Task<IEnumerable<QuoteServiceDocument>> FindAllQuoteServiceDocumentForAQuoteService(long quoteServiceId)
        {
            return await _context.QuoteServiceDocuments
                .Where(quoteServiceDocument => quoteServiceDocument.QuoteServiceId == quoteServiceId && quoteServiceDocument.IsDeleted == false)
                .OrderBy(quoteServiceDocument => quoteServiceDocument.CreatedAt)
                .ToListAsync();
        }

        public async Task<QuoteServiceDocument> UpdateQuoteServiceDocument(QuoteServiceDocument quoteServiceDocument)
        {
            var quoteServiceDocumentEntity =  _context.QuoteServiceDocuments.Update(quoteServiceDocument);
            if(await SaveChanges())
            {
                return quoteServiceDocumentEntity.Entity;
            }
            return null;
        }

        public async Task<bool> DeleteQuoteServiceDocument(QuoteServiceDocument quoteServiceDocument)
        {
            quoteServiceDocument.IsDeleted = true;
            _context.QuoteServiceDocuments.Update(quoteServiceDocument);
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