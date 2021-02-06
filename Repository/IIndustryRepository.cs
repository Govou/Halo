using System.Collections.Generic;
using System.Threading.Tasks;
using HaloBiz.Model;

namespace HaloBiz.Repository
{
    public interface IIndustryRepository
    {
        Task<Industry> SaveIndustry(Industry industry);
        Task<IEnumerable<Industry>> GetIndustries();
        Task<Industry> UpdateIndustry(Industry industry);
        Task<bool> DeleteIndustry(Industry industry);
        Task<Industry> FindIndustryById(long Id);
    }
}