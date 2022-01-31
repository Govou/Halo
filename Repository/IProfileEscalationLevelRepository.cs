using HalobizMigrations.Models;
using HalobizMigrations.Models.Complaints;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HaloBiz.Repository
{
    public interface IProfileEscalationLevelRepository
    {
        Task<ProfileEscalationLevel> SaveProfileEscalationLevel(ProfileEscalationLevel profileEscalationLevel);
        Task<ProfileEscalationLevel> FindProfileEscalationLevelById(long Id);
        //Task<ProfileEscalationLevel> FindProfileEscalationLevelByName(string name);
        Task<IEnumerable<ProfileEscalationLevel>> FindAllProfileEscalationLevels();
        Task<ProfileEscalationLevel> UpdateProfileEscalationLevel(ProfileEscalationLevel profileEscalationLevel);
        Task<bool> DeleteProfileEscalationLevel(ProfileEscalationLevel profileEscalationLevel);
        Task<bool> AlreadyHasProfileEscalationConfigured(long userProfileId);
    }
}
