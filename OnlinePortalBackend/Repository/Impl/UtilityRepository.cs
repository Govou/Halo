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

        public async Task<IEnumerable<CommonResposeDTO>> GetBusinessTypes()
        {
            return _context.Industries.Select(x => new CommonResposeDTO
            {
                Name = x.Caption,
                Id = (int)x.Id
            });
        }

        public async Task<LocalGovtAreaDTO> GetLocalGovtAreaById(int id)
        {
            LocalGovtAreaDTO lgaDTO = null;
            var lga = _context.Lgas.FirstOrDefault(x => x.Id == id);
            if (lga != null)
            {
                lgaDTO = new LocalGovtAreaDTO
                {
                    LGAId = (int)lga.Id,
                    LGAName = lga.Name,
                    StateId = (int)lga.StateId
                };
            }
            return lgaDTO;
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

        public async Task<StateDTO> GetStateById(int id)
        {
            StateDTO stateDTO = null;
            var state = _context.States.FirstOrDefault(x => x.Id == id);
            if (state != null)
            {
                stateDTO = new StateDTO
                {
                    StateId = (int)state.Id,
                    StateName = state.Name
                };
            }
            return stateDTO;
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
