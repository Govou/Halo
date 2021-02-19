using System.Collections.Generic;
using System.Threading.Tasks;
using HaloBiz.Model;

namespace HaloBiz.Repository
{
    public interface IServicesRepository
    {
        Task<Services> SaveService(Services service);
        Task<Services> FindServicesById(long Id);
        Task<Services> FindServiceByName(string name);
        Task<IEnumerable<Services>> FindAllServices();
        Task<Services> UpdateServices(Services service);
        Task<bool> DeleteService(Services service);
        Task<IEnumerable<Services>> FindAllUnplishedServices();
    }
}