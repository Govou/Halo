using HalobizMigrations.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HaloBiz.Repository
{
    public interface IProspectRepository
    {
        Task<Prospect> SaveProspect(Prospect prospect);
        Task<Prospect> FindProspectById(long Id);
        Task<IEnumerable<Prospect>> FindAllProspects();
        Task<Prospect> UpdateProspect(Prospect prospect);
        Task<bool> DeleteProspect(Prospect prospect);
    }
}
