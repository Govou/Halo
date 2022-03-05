using HalobizMigrations.Models;
using HalobizMigrations.Models.Complaints;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HaloBiz.Repository
{
    public interface IEscalationLevelRepository
    {
        Task<EscalationLevel> SaveEscalationLevel(EscalationLevel escalationLevel);
        Task<EscalationLevel> FindEscalationLevelById(long Id);
        Task<EscalationLevel> FindEscalationLevelByName(string name);
        Task<IEnumerable<EscalationLevel>> FindAllEscalationLevels();
        Task<EscalationLevel> UpdateEscalationLevel(EscalationLevel escalationLevel);
        Task<bool> DeleteEscalationLevel(EscalationLevel escalationLevel);
    }
}
