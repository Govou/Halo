using System.Collections.Generic;
using System.Threading.Tasks;
using HalobizMigrations.Models;

namespace HaloBiz.Repository
{
    public interface IStandardSlaforOperatingEntityRepository
    {
        Task<StandardSlaforOperatingEntity> SaveStandardSlaforOperatingEntity(StandardSlaforOperatingEntity standardSLAForOperatingEntities);
        Task<StandardSlaforOperatingEntity> FindStandardSlaforOperatingEntityById(long Id);
        Task<StandardSlaforOperatingEntity> FindStandardSlaforOperatingEntityByName(string name);
        Task<IEnumerable<StandardSlaforOperatingEntity>> FindAllStandardSlaforOperatingEntity();
        Task<StandardSlaforOperatingEntity> UpdateStandardSlaforOperatingEntity(StandardSlaforOperatingEntity standardSLAForOperatingEntities);
        Task<bool> DeleteStandardSlaforOperatingEntity(StandardSlaforOperatingEntity standardSLAForOperatingEntities);
    }
}