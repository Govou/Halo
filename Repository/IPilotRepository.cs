using HalobizMigrations.Models.Armada;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HaloBiz.Repository
{
    public interface IPilotRepository
    {
        //Type
        Task<PilotType> SavePilotType(PilotType pilotType);

        Task<PilotType> FindPilotTypeById(long Id);

        Task<IEnumerable<PilotType>> FindAllPilotTypes();

        PilotType GetTypename(string Name);

        Task<PilotType> UpdatePilotType(PilotType pilotType);

        Task<bool> DeletePilotType(PilotType pilotType);

        //Rank
        Task<PilotRank> SavePilotRank(PilotRank pilotRank);

        Task<PilotRank> FindPilotRankById(long Id);

        Task<IEnumerable<PilotRank>> FindAllPilotRanks();
        PilotRank GetRankname(string Name);
        long FindAllPilotRanksCount(long? ctypeId);
        Task<PilotRank> UpdatePilotRank(PilotRank pilotRank);

        Task<bool> DeletePilotRank(PilotRank pilotRank);
    }
}
