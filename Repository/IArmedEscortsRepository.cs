using HalobizMigrations.Models;
using HalobizMigrations.Models.Armada;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HaloBiz.Repository
{
    public interface IArmedEscortsRepository
    {
        //Type
        Task<ArmedEscortType> SaveArmedEscortType(ArmedEscortType armedEscortType);

        Task<ArmedEscortType> FindArmedEscortTypeById(long Id);

        Task<IEnumerable<ArmedEscortType>> FindAllArmedEscortTypes();

        Task<ArmedEscortType> UpdateArmedEscortType(ArmedEscortType armedEscortType);

        Task<bool> DeleteArmedEscortType(ArmedEscortType armedEscortType);

        //Rank
        Task<ArmedEscortRank> SaveArmedEscortRank(ArmedEscortRank armedEscortRank);

        Task<ArmedEscortRank> FindArmedEscortRankById(long Id);

        long FindAllArmedEscortRanksCount(long ctypeId);


        Task<IEnumerable<ArmedEscortRank>> FindAllArmedEscortRanks();

        Task<ArmedEscortRank> UpdateArmedEscortRank(ArmedEscortRank armedEscortRank);

        Task<bool> DeleteArmedEscortRank(ArmedEscortRank armedEscortRank);


    }
}
