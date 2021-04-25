using HalobizMigrations.Data;
using System.Collections.Generic;
using System.Threading.Tasks;
using HalobizMigrations.Models;

namespace HaloBiz.Repository
{
    public interface ISbuproportionRepository
    {
        Task<Sbuproportion> SaveSbuproportion(Sbuproportion sbuProportion);

        Task<Sbuproportion> FindSbuproportionById(long Id);

        Task<IEnumerable<Sbuproportion>> FindAllSbuproportions();

        Task<Sbuproportion> UpdateSbuproportion(Sbuproportion sbuProportion);
        Task<Sbuproportion> FindSbuproportionByOperatingEntityId(long Id);
        Task<bool> DeleteSbuproportion(Sbuproportion sbuProportion);
    }
}