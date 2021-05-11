using HalobizMigrations.Models;
using HalobizMigrations.Models.Halobiz;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HaloBiz.Repository
{
    public interface ISuspectQualificationRepository
    {
        Task<SuspectQualification> SaveSuspectQualification(SuspectQualification suspectQualification);
        Task<SuspectQualification> FindSuspectQualificationById(long Id);
        //Task<SuspectQualification> FindSuspectQualificationByName(string name);
        Task<IEnumerable<SuspectQualification>> FindAllSuspectQualifications();
        Task<SuspectQualification> UpdateSuspectQualification(SuspectQualification suspectQualification);
        Task<bool> DeleteSuspectQualification(SuspectQualification suspectQualification);
    }
}
