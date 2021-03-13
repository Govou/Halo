using System.Collections.Generic;
using System.Threading.Tasks;
using HaloBiz.Model.LAMS;

namespace HaloBiz.Repository.LAMS
{
    public interface IContractServiceRepository
    {
        Task<ContractService> SaveContractService(ContractService entity);
        Task<ContractService> FindContractServiceById(long Id);
        Task<IEnumerable<ContractService>> FindAllContractServicesForAContract(long contractId);
        Task<IEnumerable<ContractService>> FindContractServicesByReferenceNumber(string refNo);

        Task<ContractService> UpdateContractService(ContractService entity);

        Task<bool> DeleteContractService(ContractService entity);
    }
}