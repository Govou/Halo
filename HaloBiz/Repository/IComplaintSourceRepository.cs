using HalobizMigrations.Models;
using HalobizMigrations.Models.Complaints;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HaloBiz.Repository
{
    public interface IComplaintSourceRepository
    {
        Task<ComplaintSource> SaveComplaintSource(ComplaintSource complaintSource);
        Task<ComplaintSource> FindComplaintSourceById(long Id);
        Task<ComplaintSource> FindComplaintSourceByName(string name);
        Task<IEnumerable<ComplaintSource>> FindAllComplaintSources();
        Task<ComplaintSource> UpdateComplaintSource(ComplaintSource complaintSource);
        Task<bool> DeleteComplaintSource(ComplaintSource complaintSource);
    }
}
