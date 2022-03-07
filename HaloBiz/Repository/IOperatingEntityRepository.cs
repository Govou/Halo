using System.Collections.Generic;
using System.Threading.Tasks;
using HalobizMigrations.Models;

namespace HaloBiz.Repository
{
    public interface IOperatingEntityRepository
    {
        Task<OperatingEntity> SaveOperatingEntity(OperatingEntity operatingEntity);

        Task<OperatingEntity> FindOperatingEntityById(long Id);

        Task<OperatingEntity> FindOperatingEntityByName(string name);

        Task<IEnumerable<OperatingEntity>> FindAllOperatingEntity();

        Task<OperatingEntity> UpdateOperatingEntity(OperatingEntity operatingEntity);

        Task<bool> DeleteOperatingEntity(OperatingEntity operatingEntity);
        Task<bool> DeleteOperatingEntityRange(IEnumerable<OperatingEntity> operatingEntities);
        Task<IEnumerable<OperatingEntity>> FindAllOperatingEntityWithSbuproportion();
    }
}