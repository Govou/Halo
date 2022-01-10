using HalobizMigrations.Models;
using HalobizMigrations.Models.Armada;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HaloBiz.Repository
{
    public interface IServiceAssignmentMasterRepository
    {
        Task<MasterServiceAssignment> SaveServiceAssignment(MasterServiceAssignment serviceAssignment);

        Task<MasterServiceAssignment> FindServiceAssignmentById(long Id);

        Task<IEnumerable<MasterServiceAssignment>> FindAllServiceAssignments();
        Task<IEnumerable<object>> FindAllCustomerDivision();

        //MasterServiceAssignment GetName(string name);

        Task<MasterServiceAssignment> UpdateServiceAssignment(MasterServiceAssignment serviceAssignment);

        Task<bool> DeleteServiceAssignment(MasterServiceAssignment serviceAssignment);
        Task<bool> UpdateReadyStatus(MasterServiceAssignment serviceAssignment);
    }
}
