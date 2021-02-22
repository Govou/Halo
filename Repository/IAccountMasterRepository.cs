using HaloBiz.Model.AccountsModel;
using halobiz_backend.DTOs.QueryParamsDTOs;
using halobiz_backend.DTOs.ReceivingDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HaloBiz.Repository
{
    public interface IAccountMasterRepository
    {
        Task<AccountMaster> SaveAccountMaster(AccountMaster accountMaster);
        Task<AccountMaster> FindAccountMasterById(long Id);
        Task<IEnumerable<AccountMaster>> FindAllAccountMasters();
        Task<AccountMaster> UpdateAccountMaster(AccountMaster accountMaster);
        IQueryable<AccountMaster> GetAccountMastersQueryable();
        Task<bool> DeleteAccountMaster(AccountMaster accountMaster);
        Task<IEnumerable<AccountMaster>> FindAccountMastersByTransactionId(string transactionId);
        Task<IEnumerable<AccountMaster>> FindAllAccountMastersByCustomerId(AccountMasterTransactionDateQueryParams searchDTO);
    }
}
