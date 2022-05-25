using OnlinePortalBackend.DTOs.TransferDTOs;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace OnlinePortalBackend.Repository
{
    public interface IUtilityRepository
    {
        Task<IEnumerable<StateDTO>> GetStates();
        Task<IEnumerable<LocalGovtAreaDTO>> GetLocalGovtAreas(int stateId);
        Task<StateDTO> GetStateById(int id);
        Task<LocalGovtAreaDTO> GetLocalGovtAreaById(int id);
        Task<IEnumerable<CommonResposeDTO>> GetBusinessTypes();

    }
}
