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
    public class RequiredServiceDocumentRepositoryImpl : IRequiredServiceDocumentRepository
    {
        private readonly DataContext _context;
        private readonly ILogger<RequiredServiceDocumentRepositoryImpl> _logger;
        public RequiredServiceDocumentRepositoryImpl(DataContext context, ILogger<RequiredServiceDocumentRepositoryImpl> logger)
        {
            this._logger = logger;
            this._context = context;
        }

        public async Task<RequiredServiceDocument> SaveRequiredServiceDocument(RequiredServiceDocument requiredServiceDocument)
        {
            var savedEntity = await _context.RequiredServiceDocuments.AddAsync(requiredServiceDocument);
            if(! await SaveChanges())
            {
                return null;    
            }
            
            return savedEntity.Entity;
        }

        public async Task<RequiredServiceDocument> FindRequiredServiceDocumentById(long Id)
        {
            return await _context.RequiredServiceDocuments
                .Include(serviceDoc => serviceDoc.Services)
                .FirstOrDefaultAsync( RequiredServiceDocument => RequiredServiceDocument.Id == Id && RequiredServiceDocument.IsDeleted == false);
        }

        public async Task<RequiredServiceDocument> FindRequiredServiceDocumentByName(string caption)
        {
            return await _context.RequiredServiceDocuments
                .FirstOrDefaultAsync( requiredServiceDocument => requiredServiceDocument.Caption == caption && requiredServiceDocument.IsDeleted == false);
        }

        public async Task<IEnumerable<RequiredServiceDocument>> FindAllRequiredServiceDocuments()
        {
            return await _context.RequiredServiceDocuments.Where(RequiredServiceDocument => RequiredServiceDocument.IsDeleted == false)
                .ToListAsync();
        }

        public async Task<RequiredServiceDocument> UpdateRequiredServiceDocument(RequiredServiceDocument RequiredServiceDocument)
        {
            var updatedEntity =  _context.RequiredServiceDocuments.Update(RequiredServiceDocument);
            if(await SaveChanges())
            {
                return updatedEntity.Entity;
            }
            return null;
        }

        public async Task<bool> DeleteRequiredServiceDocument(RequiredServiceDocument RequiredServiceDocument)
        {
            RequiredServiceDocument.IsDeleted = true;
            _context.RequiredServiceDocuments.Update(RequiredServiceDocument);
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