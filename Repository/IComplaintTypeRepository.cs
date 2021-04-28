using HalobizMigrations.Models;
using HalobizMigrations.Models.Complaints;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HaloBiz.Repository
{
    public interface IComplaintTypeRepository
    {
        Task<ComplaintType> SaveComplaintType(ComplaintType complaintType);
        Task<ComplaintType> FindComplaintTypeById(long Id);
        Task<ComplaintType> FindComplaintTypeByName(string name);
        Task<IEnumerable<ComplaintType>> FindAllComplaintTypes();
        Task<ComplaintType> UpdateComplaintType(ComplaintType complaintType);
        Task<bool> DeleteComplaintType(ComplaintType complaintType);
    }
}
