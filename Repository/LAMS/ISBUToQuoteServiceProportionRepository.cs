using System.Collections.Generic;
using System.Threading.Tasks;
using HaloBiz.Model.LAMS;

namespace HaloBiz.Repository.LAMS
{
    public interface ISBUToQuoteServiceProportionRepository
    {
        Task<SBUToQuoteServiceProportion> FindSBUToQuoteServiceProportionById(long Id);
        Task<SBUToQuoteServiceProportion> SaveSBUToQuoteServiceProportion(SBUToQuoteServiceProportion entity);
        Task<IEnumerable<SBUToQuoteServiceProportion>> FindAllSBUToQuoteServiceProportionByQuoteServiceId(long quoteServiceId);
        Task<SBUToQuoteServiceProportion> UpdateSBUToQuoteServiceProportion(SBUToQuoteServiceProportion entity);
        Task<bool> DeleteSBUToQuoteServiceProportion(SBUToQuoteServiceProportion entity);
        Task<IEnumerable<SBUToQuoteServiceProportion>> SaveSBUToQuoteServiceProportion(IEnumerable<SBUToQuoteServiceProportion> entities);
        Task<bool> DeleteSBUToQuoteServiceProportion(IEnumerable<SBUToQuoteServiceProportion> entities);
        Task<IEnumerable<SBUToQuoteServiceProportion>> UpdateSBUToQuoteServiceProportion(IEnumerable<SBUToQuoteServiceProportion> entities);
    }
}