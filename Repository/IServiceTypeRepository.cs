using HaloBiz.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HaloBiz.Repository
{
    public interface IServiceTypeRepository
    {
        Task<ServiceType> SaveServiceType(ServiceType serviceType);
        Task<ServiceType> FindServiceTypeById(long Id);
        Task<ServiceType> FindServiceTypeByName(string name);
        Task<IEnumerable<ServiceType>> FindAllServiceTypes();
        Task<ServiceType> UpdateServiceType(ServiceType serviceType);
        Task<bool> DeleteServiceType(ServiceType serviceType);
    }
}
