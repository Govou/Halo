using HalobizMigrations.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HaloBiz.Repository
{
    public interface IServiceAssignmentRepository
    {
        Task<ServiceAssignment> SaveServiceAssignment(ServiceAssignment serviceAssignment);

        Task<ServiceAssignment> FindServiceAssignmentById(long Id);

        Task<IEnumerable<ServiceAssignment>> FindAllServiceAssignments();

        ServiceAssignment GetName(string name);

        Task<ServiceAssignment> UpdateServiceAssignment(ServiceAssignment serviceAssignment);

        Task<bool> DeleteServiceAssignment(ServiceAssignment serviceAssignment);
    }
}
