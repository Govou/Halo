using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HalobizMigrations.Data;
using HalobizMigrations.Models;
using HaloBiz.Repository.LAMS;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace HaloBiz.Repository.Impl.LAMS
{
    public class ClosureDocumentRepositoryImpl : IClosureDocumentRepository
    {
        private readonly HalobizContext _context;
        private readonly ILogger<ClosureDocumentRepositoryImpl> _logger;
        public ClosureDocumentRepositoryImpl(HalobizContext context, ILogger<ClosureDocumentRepositoryImpl> logger)
        {
            this._logger = logger;
            this._context = context;
        }

        public async Task<ClosureDocument> SaveClosureDocument(ClosureDocument closureDocument)
        {
            var closureDocumentEntity = await _context.ClosureDocuments.AddAsync(closureDocument);
            if(await SaveChanges())
            {
                return closureDocumentEntity.Entity;
            }
            return null;
        }

        public async Task<ClosureDocument> FindClosureDocumentById(long Id)
        {
            return await _context.ClosureDocuments
                .FirstOrDefaultAsync( closureDocument => closureDocument.Id == Id && closureDocument.IsDeleted == false);
        }

        public async Task<ClosureDocument> FindClosureDocumentByCaption(string caption)
        {
            return await _context.ClosureDocuments
                .FirstOrDefaultAsync( closureDocument => closureDocument.Caption == caption && closureDocument.IsDeleted == false);
        }

        public async Task<IEnumerable<ClosureDocument>> FindAllClosureDocument()
        {
            return await _context.ClosureDocuments
                .Where(closureDocument => closureDocument.IsDeleted == false)
                .OrderBy(closureDocument => closureDocument.CreatedAt)
                .ToListAsync();
        }

        public async Task<ClosureDocument> UpdateClosureDocument(ClosureDocument closureDocument)
        {
            var closureDocumentEntity =  _context.ClosureDocuments.Update(closureDocument);
            if(await SaveChanges())
            {
                return closureDocumentEntity.Entity;
            }
            return null;
        }

        public async Task<bool> DeleteClosureDocument(ClosureDocument closureDocument)
        {
            closureDocument.IsDeleted = true;
            _context.ClosureDocuments.Update(closureDocument);
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