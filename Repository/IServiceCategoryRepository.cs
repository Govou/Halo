using System.Collections.Generic;
using System.Threading.Tasks;
using HaloBiz.Model_;

namespace HaloBiz.Repository
{
    public interface IServiceCategoryRepository
    {
        Task<ServiceCategory> SaveServiceCategory(ServiceCategory serviceCategory);

        Task<ServiceCategory> FindServiceCategoryById(long Id);

        Task<ServiceCategory> FindServiceCategoryByName(string name);

        Task<IEnumerable<ServiceCategory>> FindAllServiceCategories();

        Task<ServiceCategory> UpdateServiceCategory(ServiceCategory category);

        Task<bool> DeleteServiceCategory(ServiceCategory category);

    }
}