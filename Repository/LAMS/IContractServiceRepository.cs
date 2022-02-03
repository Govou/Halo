using HalobizMigrations.Models;
using System.Collections.Generic;
using System.Threading.Tasks;


namespace HaloBiz.Repository.LAMS
{
    public interface IContractServiceRepository
    {
        Task<ContractService> SaveContractService(ContractService entity);
        Task<ContractService> FindContractServiceById(long Id);
        Task<IEnumerable<ContractService>> FindAllContractServicesForAContract(long contractId);
        Task<IEnumerable<ContractService>> FindContractServicesByReferenceNumber(string refNo);
        Task<IEnumerable<ContractService>> FindContractServicesByGroupInvoiceNumber(string groupInvoiceNumber);
        Task<ContractService> UpdateContractService(ContractService entity);
        Task<ContractService> FindContractServiceByTag(string tag);
        Task<bool> DeleteContractService(ContractService entity);
        Task<List<ContractService>> FindAllContractServices(long CustomerDivisionId);
        Task<IEnumerable<ContractService>> FindAllContractServices();
    }
}