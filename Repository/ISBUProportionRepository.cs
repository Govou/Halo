using HaloBiz.Data;
using System.Collections.Generic;
using System.Threading.Tasks;
using HaloBiz.Model;

namespace HaloBiz.Repository
{
    public interface ISBUProportionRepository
    {
        Task<SBUProportion> SaveSBUProportion(SBUProportion sbuProportion);

        Task<SBUProportion> FindSBUProportionById(long Id);

        Task<IEnumerable<SBUProportion>> FindAllSBUProportions();

        Task<SBUProportion> UpdateSBUProportion(SBUProportion sbuProportion);
        Task<SBUProportion> FindSBUProportionByOperatingEntityId(long Id);
        Task<bool> DeleteSBUProportion(SBUProportion sbuProportion);
    }
}