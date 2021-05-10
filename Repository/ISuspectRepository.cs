using HalobizMigrations.Models;
using HalobizMigrations.Models.Halobiz;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HaloBiz.Repository
{
    public interface ISuspectRepository
    {
        Task<Suspect> SaveSuspect(Suspect suspect);
        Task<Suspect> FindSuspectById(long Id);
        //Task<Suspect> FindSuspectByName(string name);
        Task<IEnumerable<Suspect>> FindAllSuspects();
        Task<Suspect> UpdateSuspect(Suspect suspect);
        Task<bool> DeleteSuspect(Suspect suspect);
    }
}
