using System.Collections.Generic;
using System.Threading.Tasks;
using HaloBiz.Model;

namespace HaloBiz.Repository
{
    public interface IOfficeRepository
    {
        Task<Office> SaveOffice(Office office);
        Task<Office> FindOfficeById(long Id);

        Task<Office> FindOfficeByName(string name);

        Task<IEnumerable<Office>> FindAllOffices();

        Task<Office> UpdateOffice(Office office);

        Task<bool> DeleteOffice(Office office);
        Task<bool> DeleteOfficeRange(IEnumerable<Office> offices);

    }
}