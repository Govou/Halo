using HalobizMigrations.Models;
using OnlinePortalBackend.DTOs.TransferDTOs;
using System.Threading.Tasks;

namespace OnlinePortalBackend.Repository
{
    public interface ICustomerInfoRepository
    {
        Task<CustomerContractInfoDTO> GetCotractInfos(int customerDiv);
        Task<string> GetDtrackCustomerNumber(CustomerDivision customer);
    }
}
