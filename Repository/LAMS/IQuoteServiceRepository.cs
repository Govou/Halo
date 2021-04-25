using HalobizMigrations.Models;
using System.Collections.Generic;
using System.Threading.Tasks;


namespace HaloBiz.Repository.LAMS
{
    public interface IQuoteServiceRepository
    {
        Task<QuoteService> SaveQuoteService(QuoteService quoteService);
        Task<QuoteService> FindQuoteServiceById(long Id);
        Task<IEnumerable<QuoteService>> FindAllQuoteService();
        Task<QuoteService> UpdateQuoteService(QuoteService quoteService);
        Task<bool> DeleteQuoteService(QuoteService quoteService);
        Task<bool> DeleteQuoteServiceRange(IEnumerable<QuoteService> quoteServices);
        Task<bool> SaveQuoteServiceRange(IEnumerable<QuoteService> quoteServices);
    }
}