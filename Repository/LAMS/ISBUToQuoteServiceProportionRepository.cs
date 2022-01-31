using HalobizMigrations.Models;
using System.Collections.Generic;
using System.Threading.Tasks;


namespace HaloBiz.Repository.LAMS
{
    public interface ISbutoQuoteServiceProportionRepository
    {
        Task<SbutoQuoteServiceProportion> FindSbutoQuoteServiceProportionById(long Id);
        Task<SbutoQuoteServiceProportion> SaveSbutoQuoteServiceProportion(SbutoQuoteServiceProportion entity);
        Task<IEnumerable<SbutoQuoteServiceProportion>> FindAllSbutoQuoteServiceProportionByQuoteServiceId(long quoteServiceId);
        Task<SbutoQuoteServiceProportion> UpdateSbutoQuoteServiceProportion(SbutoQuoteServiceProportion entity);
        Task<bool> DeleteSbutoQuoteServiceProportion(SbutoQuoteServiceProportion entity);
        Task<IEnumerable<SbutoQuoteServiceProportion>> SaveSbutoQuoteServiceProportion(IEnumerable<SbutoQuoteServiceProportion> entities);
        Task<bool> DeleteSbutoQuoteServiceProportion(IEnumerable<SbutoQuoteServiceProportion> entities);
        Task<IEnumerable<SbutoQuoteServiceProportion>> UpdateSbutoQuoteServiceProportion(IEnumerable<SbutoQuoteServiceProportion> entities);
    }
}