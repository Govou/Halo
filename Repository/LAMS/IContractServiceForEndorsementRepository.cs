using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using HalobizMigrations.Models;

namespace HaloBiz.Repository.LAMS
{
    public interface IContractServiceForEndorsementRepository
    {
        Task<ContractServiceForEndorsement> SaveContractServiceForEndorsement(ContractServiceForEndorsement entity);
        Task<IEnumerable<ContractServiceForEndorsement>> FindAllUnApprovedContractServicesForEndorsement();
        Task<ContractServiceForEndorsement> GetEndorsementDetailsById(long endorsementId);
        Task<ContractServiceForEndorsement> UpdateContractServiceForEndorsement(ContractServiceForEndorsement entity);
        Task<ContractServiceForEndorsement> FindContractServiceForEndorsementById(long Id);
        Task<IEnumerable<object>> FindAllPossibleEndorsementStartDate(long contractServiceId);
        Task<bool> SaveRangeContractServiceForEndorsement(IEnumerable<ContractServiceForEndorsement> entity);
    }
}