using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using HaloBiz.Model.LAMS;

namespace HaloBiz.Repository.LAMS
{
    public interface IContractServiceForEndorsementRepository
    {
        Task<ContractServiceForEndorsement> SaveContractServiceForEndorsement(ContractServiceForEndorsement entity);
        Task<IEnumerable<ContractServiceForEndorsement>> FindAllUnApprovedContractServicesForEndorsement();
        Task<ContractServiceForEndorsement> UpdateContractServiceForEndorsement(ContractServiceForEndorsement entity);
        Task<ContractServiceForEndorsement> FindContractServiceForEndorsementById(long Id);
    }
}