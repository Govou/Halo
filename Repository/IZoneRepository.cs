using System.Collections.Generic;
using System.Threading.Tasks;
using HaloBiz.Model;

namespace HaloBiz.Repository
{
    public interface IZoneRepository
    {
        Task<Zone> SaveZone(Zone zone);

        Task<Zone> FindZoneById(long Id);

        Task<Zone> FindZoneByName(string name);

        Task<IEnumerable<Zone>> FindAllZones();

        Task<Zone> UpdateZone(Zone zone);

        Task<bool> DeleteZone(Zone zone);
    }
}