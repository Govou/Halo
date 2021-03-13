using System.Collections.Generic;
using System.Threading.Tasks;
using HaloBiz.Model.LAMS;

namespace HaloBiz.Repository
{
    public interface IContractRepository
    {
        Task<Contract> SaveContract(Contract entity);

        Task<Contract> FindContractById(long Id);
        

        Task<IEnumerable<Contract>> FindAllContract();
        Task<Contract> FindContractByReferenceNumber(string refNo);

        Task<Contract> UpdateContract(Contract entity);

        Task<bool> DeleteContract(Contract entity);
    }
}