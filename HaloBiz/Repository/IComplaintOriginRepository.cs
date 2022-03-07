using HalobizMigrations.Models;
using HalobizMigrations.Models.Complaints;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HaloBiz.Repository
{
    public interface IComplaintOriginRepository
    {
        Task<ComplaintOrigin> SaveComplaintOrigin(ComplaintOrigin complaintOrigin);
        Task<ComplaintOrigin> FindComplaintOriginById(long Id);
        Task<ComplaintOrigin> FindComplaintOriginByName(string name);
        Task<IEnumerable<ComplaintOrigin>> FindAllComplaintOrigins();
        Task<ComplaintOrigin> UpdateComplaintOrigin(ComplaintOrigin complaintOrigin);
        Task<bool> DeleteComplaintOrigin(ComplaintOrigin complaintOrigin);
    }
}
