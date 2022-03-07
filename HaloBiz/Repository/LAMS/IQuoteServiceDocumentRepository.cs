using HalobizMigrations.Models;
using System.Collections.Generic;
using System.Threading.Tasks;


namespace HaloBiz.Repository.LAMS
{
    public interface IQuoteServiceDocumentRepository
    {
        Task<QuoteServiceDocument> SaveQuoteServiceDocument(List<QuoteServiceDocument> quoteServiceDocument);
        Task<QuoteServiceDocument> FindQuoteServiceDocumentById(long Id);
        Task<QuoteServiceDocument> FindQuoteServiceDocumentByCaption(string caption);
        Task<IEnumerable<QuoteServiceDocument>> FindAllQuoteServiceDocument();
        Task<QuoteServiceDocument> UpdateQuoteServiceDocument(QuoteServiceDocument quoteServiceDocument);
        Task<bool> DeleteQuoteServiceDocument(QuoteServiceDocument quoteServiceDocument);
        Task<IEnumerable<QuoteServiceDocument>> FindAllQuoteServiceDocumentForAQuoteService(long quoteServiceId);
    }
}