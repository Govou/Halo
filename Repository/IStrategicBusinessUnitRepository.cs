using System.Collections.Generic;
using System.Threading.Tasks;
using HalobizMigrations.Models;

namespace HaloBiz.Repository
{
    public interface IStrategicBusinessUnitRepository
    {
        Task<StrategicBusinessUnit> SaveStrategyBusinessUnit(StrategicBusinessUnit sbu);

        Task<StrategicBusinessUnit> FindStrategyBusinessUnitById(long Id);

        Task<StrategicBusinessUnit> FindStrategyBusinessUnitByName(string name);

        Task<IEnumerable<StrategicBusinessUnit>> FindAllStrategyBusinessUnits();

        Task<StrategicBusinessUnit> UpdateStrategyBusinessUnit(StrategicBusinessUnit sbu);

        Task<bool> DeleteStrategyBusinessUnit(StrategicBusinessUnit sbu);
    }
}