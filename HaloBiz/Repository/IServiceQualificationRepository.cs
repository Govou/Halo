using HalobizMigrations.Models;
using HalobizMigrations.Models.Halobiz;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HaloBiz.Repository
{
    public interface IServiceQualificationRepository
    {
        Task<ServiceQualification> SaveServiceQualification(ServiceQualification serviceQualification);
        Task<ServiceQualification> FindServiceQualificationById(long Id);
        //Task<ServiceQualification> FindServiceQualificationByName(string name);
        Task<IEnumerable<ServiceQualification>> FindAllServiceQualifications();
        Task<ServiceQualification> UpdateServiceQualification(ServiceQualification serviceQualification);
        Task<bool> DeleteServiceQualification(ServiceQualification serviceQualification);
    }
}
