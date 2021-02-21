using HaloBiz.Data;
using HaloBiz.Model.AccountsModel;
using halobiz_backend.DTOs.ReceivingDTOs;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HaloBiz.Repository.Impl
{
    public class AccountMasterRepositoryImpl : IAccountMasterRepository
    {
        private readonly DataContext _context;
        private readonly ILogger<AccountMasterRepositoryImpl> _logger;
        public AccountMasterRepositoryImpl(DataContext context, ILogger<AccountMasterRepositoryImpl> logger)
        {
            this._logger = logger;
            this._context = context;

        }

        public async Task<bool> DeleteAccountMaster(AccountMaster accountMaster)
        {
            accountMaster.IsDeleted = true;
            _context.AccountMasters.Update(accountMaster);
            return await SaveChanges();
        }

        public async Task<AccountMaster> FindAccountMasterById(long Id)
        {
            var accountMaster = await _context.AccountMasters
                .Include(x => x.AccountDetails)
                    .ThenInclude(x => x.Account)
                .FirstOrDefaultAsync(accountMaster => accountMaster.Id == Id && accountMaster.IsDeleted == false);
            if(accountMaster.AccountDetails != null)
            {
                accountMaster.AccountDetails.ToList().ForEach(x =>{
                     x.AccountMaster = null;
                     if(x.Account != null)
                     {
                         x.Account.AccountDetails = null;
                     }
                });
            }

            return accountMaster;
        }

        public async Task<IEnumerable<AccountMaster>> FindAllAccountMasters()
        {
            return await _context.AccountMasters
                .Include(x => x.AccountDetails)
                .Where(user => user.IsDeleted == false).ToListAsync();
        }

        public async Task<IEnumerable<AccountMaster>> FindAllAccountMastersByCustomerId(AccMasterByCustomerIdSearchDto searchDTO)
        {
            var queryable = _context.AccountMasters
                .Join(
                    _context.CustomerDivisions,
                        accountMaster => accountMaster.CustomerDivisionId,
                        customerDivision => customerDivision.Id,
                        (accountMaster, customerDivision) => new {
                            AccountMasterId = accountMaster.Id,
                            CustomerDivisionId = customerDivision.Id
                        }
                ).Join(
                    _context.Contracts,
                    accCust => accCust.CustomerDivisionId,
                    contract => contract.CustomerDivisionId,
                        (accCust, contract) => new {
                            AccountMasterId = accCust.AccountMasterId,
                            ContractYears = contract.CreatedAt.Year,
                            CustomerId = accCust.CustomerDivisionId
                        }
                ).AsQueryable();

                List<long> accountMasterIds = null;

                if(searchDTO.Years.Count() > 0)
                {
                    accountMasterIds = await queryable.Where(x => searchDTO.Years.Contains(x.ContractYears) 
                                            && x.CustomerId == searchDTO.ClientId )
                                                .Select(x => x.AccountMasterId).ToListAsync();
                }else{
                    accountMasterIds = await queryable.Where(x => x.CustomerId == searchDTO.ClientId )
                                                .Select(x => x.AccountMasterId).ToListAsync();
                }

                return  await _context.AccountMasters 
                            .Include(x => x.AccountDetails)
                            .Where(x => accountMasterIds.Contains(x.Id)).ToArrayAsync();
        }

        public async Task<IEnumerable<AccountMaster>> FindAccountMastersByTransactionId(string transactionId)
        {
            transactionId = String.Join("/",transactionId.Split("%2F"));
            return await _context.AccountMasters
                .Include(x => x.AccountDetails.Where(x => x.IsDeleted == false))
                .Where(x => x.IsDeleted == false && x.TransactionId == transactionId).ToListAsync();
        }
        public async Task<AccountMaster> SaveAccountMaster(AccountMaster accountMaster)
        {
            var AccountMasterEntity = await _context.AccountMasters.AddAsync(accountMaster);
            if (await SaveChanges())
            {
                return AccountMasterEntity.Entity;
            }
            return null;
        }

        public async Task<AccountMaster> UpdateAccountMaster(AccountMaster accountMaster)
        {
            var AccountMasterEntity = _context.AccountMasters.Update(accountMaster);
            if (await SaveChanges())
            {
                return AccountMasterEntity.Entity;
            }
            return null;
        }
        private async Task<bool> SaveChanges()
        {
            try
            {
                return await _context.SaveChangesAsync() > 0;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return false;
            }
        }
    }
}
