using HalobizMigrations.Models;
using OnlinePortalBackend.DTOs.TransferDTOs;
using System.Collections.Generic;
using System.Threading.Tasks;
using System;

namespace OnlinePortalBackend.Repository
{
    public interface IEndorsementRepository
    {
        Task<ContractServiceForEndorsement> FindEndorsementById(long userId, long Id);
        Task<EndorsementTrackingDTO> TrackEndorsement(long contractServiceId);
        Task<IEnumerable<ContractServiceForEndorsement>> FindEndorsements(long userId);
        Task<ContractServiceDTO> GetContractService(int id);
        Task<IEnumerable<ContractDTO>> GetContractServices(int userId);
        Task<ContractServiceForEndorsement> SaveContractServiceForEndorsement(ContractServiceForEndorsement entity);
        Task<bool> SetUpApprovalsForContractModificationEndorsement(ContractServiceForEndorsement contractServiceForEndorsement);
    }
}
