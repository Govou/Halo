using System.Collections.Generic;
using System.Threading.Tasks;
using HaloBiz.DTOs.TransferDTOs.LAMS;
using HalobizMigrations.Models;

namespace HaloBiz.Repository
{
    public interface IServicesRepository
    {
        Task<Service> SaveService(Service service);
        Task<Service> FindServicesById(long Id);
        Task<Service> FindServiceByName(string name);
        Task<IEnumerable<Service>> FindAllServices();
        Task<Service> UpdateServices(Service service);
        Task<bool> DeleteService(Service service);
        Task<IEnumerable<Service>> FindAllUnplishedServices();
        Task<ServiceDivisionDetails> GetServiceDetails(long Id);
    }
}