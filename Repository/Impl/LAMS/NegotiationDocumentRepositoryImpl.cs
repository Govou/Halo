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
    public class NegotiationDocumentRepositoryImpl : INegotiationDocumentRepository
    {
        private readonly DataContext _context;
        private readonly ILogger<NegotiationDocumentRepositoryImpl> _logger;
        public NegotiationDocumentRepositoryImpl(DataContext context, ILogger<NegotiationDocumentRepositoryImpl> logger)
        {
            this._logger = logger;
            this._context = context;
        }

        public async Task<NegotiationDocument> SaveNegotiationDocument(NegotiationDocument negotiationDocument)
        {
            var negotiationDocumentEntity = await _context.NegotiationDocuments.AddAsync(negotiationDocument);
            if(await SaveChanges())
            {
                return negotiationDocumentEntity.Entity;
            }
            return null;
        }

        public async Task<NegotiationDocument> FindNegotiationDocumentById(long Id)
        {
            return await _context.NegotiationDocuments
                .FirstOrDefaultAsync( negotiationDocument => negotiationDocument.Id == Id && negotiationDocument.IsDeleted == false);
        }

        public async Task<NegotiationDocument> FindNegotiationDocumentByCaption(string caption)
        {
            return await _context.NegotiationDocuments
                .FirstOrDefaultAsync( negotiationDocument => negotiationDocument.Caption == caption && negotiationDocument.IsDeleted == false);
        }

        public async Task<IEnumerable<NegotiationDocument>> FindAllNegotiationDocument()
        {
            return await _context.NegotiationDocuments
                .Where(negotiationDocument => negotiationDocument.IsDeleted == false)
                .OrderBy(negotiationDocument => negotiationDocument.CreatedAt)
                .ToListAsync();
        }

        public async Task<NegotiationDocument> UpdateNegotiationDocument(NegotiationDocument negotiationDocument)
        {
            var negotiationDocumentEntity =  _context.NegotiationDocuments.Update(negotiationDocument);
            if(await SaveChanges())
            {
                return negotiationDocumentEntity.Entity;
            }
            return null;
        }

        public async Task<bool> DeleteNegotiationDocument(NegotiationDocument negotiationDocument)
        {
            negotiationDocument.IsDeleted = true;
            _context.NegotiationDocuments.Update(negotiationDocument);
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