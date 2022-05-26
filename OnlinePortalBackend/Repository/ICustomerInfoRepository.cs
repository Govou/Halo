using OnlinePortalBackend.DTOs.TransferDTOs;
using System.Threading.Tasks;

namespace OnlinePortalBackend.Repository
{
    public interface ICustomerInfoRepository
    {
        Task<CustomerContractInfoDTO> GetCotractInfos(int customerDiv);
    }
}
