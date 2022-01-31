using System.Collections.Generic;
using System.Threading.Tasks;
using HalobizMigrations.Models;

namespace HaloBiz.Repository
{
    public interface IRegionRepository
    {
        Task<Region> SaveRegion(Region region);

        Task<Region> FindRegionById(long Id);

        Task<Region> FindRegionByName(string name);

        Task<IEnumerable<Region>> FindAllRegions();

        Task<Region> UpdateRegion(Region region);

        Task<bool> DeleteRegion(Region region);
    }
}