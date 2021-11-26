using HalobizMigrations.Models.Armada;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HaloBiz.Repository
{
    public interface IBusinessRulesRepository
    {
        Task<BusinessRule> SaveRule(BusinessRule businessRule);

        Task<BusinessRule> FindRuleById(long Id);

        Task<IEnumerable<BusinessRule>> FindAllRules();

        BusinessRule GetRegServiceId(long regServiceId);

        Task<BusinessRule> UpdateRule(BusinessRule businessRule);

        Task<bool> DeleteRule(BusinessRule businessRule);

        //BRPaiarble
        Task<BRPairable> SavePairable(BRPairable bRPairable);

        Task<BRPairable> FindPairableById(long Id);

        Task<IEnumerable<BRPairable>> FindAllPairables();

        Task<IEnumerable<BRPairable>> FindAllActivePairables();

        //BRPairable GetPairableId(long regServiceId);

        Task<BRPairable> UpdatePairable(BRPairable bRPairable);

        Task<bool> DeletePairable(BRPairable bRPairable);

        bool requiresCommander(long? id);
        bool requiresVehicle(long? id);
        bool requiresArmedEscort(long? id);
        bool requiresPilot(long? id);
    }
}
