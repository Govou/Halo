using System.Collections.Generic;
using System.Threading.Tasks;
using HaloBiz.Model;
using HalobizMigrations.Models;

namespace HaloBiz.Repository
{
    public interface IMakeRepository
    {
        Task<Make> SaveMake(Make make);
        Task<IEnumerable<Make>> GetMakes();
        Task<bool> DeleteMake(Make make);
        Task<Make> FindMakeById(long Id);
        Task<List<IValidation>> ValidateMake(string makeCaption);
    }
}