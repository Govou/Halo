using HalobizMigrations.Models.Armada;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HaloBiz.Repository
{
    public interface IServiceRegistrationRepository
    {
        Task<ServiceRegistration> SaveService(ServiceRegistration serviceRegistration);

        Task<ServiceRegistration> FindServiceById(long Id);

        Task<IEnumerable<ServiceRegistration>> FindAllServicess();

        ServiceRegistration GetserviceId(long serviceId);

        Task<ServiceRegistration> UpdateServices(ServiceRegistration serviceRegistration);

        Task<bool> DeleteService(ServiceRegistration serviceRegistration);
    }
}
