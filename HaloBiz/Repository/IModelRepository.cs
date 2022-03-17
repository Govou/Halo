using System.Collections.Generic;
using System.Threading.Tasks;
using HaloBiz.Model;

namespace HaloBiz.Repository
{
    public interface IModelRepository
    {
        Task<HalobizMigrations.Models.Model> SaveModel(HalobizMigrations.Models.Model model);
        Task<IEnumerable<HalobizMigrations.Models.Model>> GetModel(int makeId);
        Task<bool> DeleteModel(HalobizMigrations.Models.Model model);
        Task<HalobizMigrations.Models.Model> FindModelById(long Id);
        Task<List<IValidation>> ValidateModel(string modelCaption);
        Task<HalobizMigrations.Models.Model> UpdateModel(HalobizMigrations.Models.Model model);
    }
}