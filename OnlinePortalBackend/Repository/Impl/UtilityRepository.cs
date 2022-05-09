using HalobizMigrations.Data;
using OnlinePortalBackend.DTOs.TransferDTOs;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OnlinePortalBackend.Repository.Impl
{
    public class UtilityRepository : IUtilityRepository
    {
        private readonly HalobizContext _context;
        public UtilityRepository(HalobizContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<LocalGovtAreaDTO>> GetLocalGovtAreas(int stateId)
        {
            return _context.Lgas.Where(x => x.StateId == stateId).Select(x => new LocalGovtAreaDTO
            {
                LGAId = (int)x.Id,
                LGAName = x.Name,
                StateId = (int)x.StateId
            });
        }

        public async Task<IEnumerable<StateDTO>> GetStates()
        {
            return _context.States.Select(x => new StateDTO
            {
                StateName = x.Name,
                StateId = (int)x.Id
            });
        }
    }
}
