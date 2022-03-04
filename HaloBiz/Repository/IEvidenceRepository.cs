using HalobizMigrations.Models;
using HalobizMigrations.Models.Complaints;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HaloBiz.Repository
{
    public interface IEvidenceRepository
    {
        Task<Evidence> SaveEvidence(Evidence evidence);
        Task<Evidence> FindEvidenceById(long Id);
        Task<Evidence> FindEvidenceByName(string name);
        Task<IEnumerable<Evidence>> FindAllEvidences();
        Task<Evidence> UpdateEvidence(Evidence evidence);
        Task<bool> DeleteEvidence(Evidence evidence);
    }
}
