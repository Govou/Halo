using HalobizMigrations.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HaloBiz.Repository
{
    public interface IArmedEscortRegistrationRepository
    {
        Task<ArmedEscortProfile> SaveArmedEscort(ArmedEscortProfile armedEscortProfile);

        Task<ArmedEscortProfile> FindArmedEscortById(long Id);

        Task<IEnumerable<ArmedEscortProfile>> FindAllArmedEscorts();

       // ArmedEscortProfile GetProfileName(string Name);

        Task<ArmedEscortProfile> UpdateArmedEscort(ArmedEscortProfile armedEscortProfile);

        Task<bool> DeleteArmedEscort(ArmedEscortProfile armedEscortProfile);
    }
}
