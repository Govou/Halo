using System.Collections.Generic;
using System.Threading.Tasks;
using HaloBiz.Model;

namespace HaloBiz.Repository
{
    public interface IServiceTaskDeliverableRepository
    {
        Task<ServiceTaskDeliverable> SaveServiceTaskDeliverable(ServiceTaskDeliverable serviceTaskDeliverable);

        Task<ServiceTaskDeliverable> FindServiceTaskDeliverableById(long Id);

        Task<ServiceTaskDeliverable> FindServiceTaskDeliverableByName(string name);

        Task<IEnumerable<ServiceTaskDeliverable>> FindAllServiceTaskDeliverables();

        Task<ServiceTaskDeliverable> UpdateServiceTaskDeliverable(ServiceTaskDeliverable serviceTaskDeliverable);

        Task<bool> DeleteServiceTaskDeliverable(ServiceTaskDeliverable serviceTaskDeliverable);
        Task<bool> DeleteServiceTaskDeliverableRange(IEnumerable<ServiceTaskDeliverable> serviceTaskDeliverables);
    }
}