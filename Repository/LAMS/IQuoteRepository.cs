using HalobizMigrations.Models;
using System.Collections.Generic;
using System.Threading.Tasks;


namespace HaloBiz.Repository.LAMS
{
    public interface IQuoteRepository
    {
        Task<Quote> SaveQuote(Quote quote);
        Task<Quote> FindQuoteById(long Id);
        Task<Quote> FindQuoteByReferenceNumber(string reference);
        Task<IEnumerable<Quote>> FindAllQuote();
        Task<Quote> UpdateQuote(Quote quote);
        Task<bool> DeleteQuote(Quote quote);
    }
}