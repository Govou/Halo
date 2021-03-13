using System.Collections.Generic;
using System.Threading.Tasks;
using HaloBiz.Model;

namespace HaloBiz.Repository
{
    public interface IStandardSLAForOperatingEntitiesRepository
    {
        Task<StandardSLAForOperatingEntities> SaveStandardSLAForOperatingEntities(StandardSLAForOperatingEntities standardSLAForOperatingEntities);
        Task<StandardSLAForOperatingEntities> FindStandardSLAForOperatingEntitiesById(long Id);
        Task<StandardSLAForOperatingEntities> FindStandardSLAForOperatingEntitiesByName(string name);
        Task<IEnumerable<StandardSLAForOperatingEntities>> FindAllStandardSLAForOperatingEntities();
        Task<StandardSLAForOperatingEntities> UpdateStandardSLAForOperatingEntities(StandardSLAForOperatingEntities standardSLAForOperatingEntities);
        Task<bool> DeleteStandardSLAForOperatingEntities(StandardSLAForOperatingEntities standardSLAForOperatingEntities);
    }
}