using HalobizMigrations.Models;
using HalobizMigrations.Models.Complaints;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HaloBiz.Repository
{
    public interface IComplaintRepository
    {
        Task<Complaint> SaveComplaint(Complaint complaint);
        Task<Complaint> FindComplaintById(long Id);
        Task<Complaint> FindComplaintByName(string name);
        Task<IEnumerable<Complaint>> FindAllComplaints();
        Task<Complaint> UpdateComplaint(Complaint complaint);
        Task<bool> DeleteComplaint(Complaint complaint);
    }
}
