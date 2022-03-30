using OnlinePortalBackend.DTOs.TransferDTOs;
using System.Threading.Tasks;

namespace OnlinePortalBackend.Repository
{
    public interface IServicesRepo
    {
        Task<ServiceDetailDTO> GetContractServciceById(int contractServciceId);
    }
}
