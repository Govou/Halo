using System.Collections.Generic;
using System.Threading.Tasks;
using HaloBiz.Model;

namespace HaloBiz.Repository
{
    public interface IServiceCategoryTaskRepository
    {
        Task<ServiceCategoryTask> SaveServiceCategoryTask(ServiceCategoryTask ServiceCategoryTask);

        Task<ServiceCategoryTask> FindServiceCategoryTaskById(long Id);

        Task<ServiceCategoryTask> FindServiceCategoryTaskByName(string name);

        Task<IEnumerable<ServiceCategoryTask>> FindAllServiceCategoryTasks();

        Task<ServiceCategoryTask> UpdateServiceCategoryTask(ServiceCategoryTask ServiceCategoryTask);

        Task<bool> DeleteServiceCategoryTask(ServiceCategoryTask ServiceCategoryTask);
        Task<bool> DeleteServiceCategoryTaskRange(IEnumerable<ServiceCategoryTask> ServiceCategoryTasks);
    }
}