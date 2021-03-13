using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HaloBiz.Data;
using HaloBiz.Model.LAMS;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace HaloBiz.Repository.Impl.LAMS
{
    public class ContractRepositoryImpl : IContractRepository
    {
        private readonly DataContext _context;
        private readonly ILogger<ContractRepositoryImpl> _logger;
        public ContractRepositoryImpl(DataContext context, ILogger<ContractRepositoryImpl> logger)
        {
            this._logger = logger;
            this._context = context;
        }

        public async Task<Contract> SaveContract(Contract entity)
        {
            var contractEntity = await _context.Contracts.AddAsync(entity);

            if (await SaveChanges())
            {
                return contractEntity.Entity;
            }
            return null;            
        }

        public async Task<Contract> FindContractById(long Id)
        {
            return await _context.Contracts
                .Include(x => x.ContractServices)
                .Include(x => x.CustomerDivision)
                .FirstOrDefaultAsync(x => x.Id == Id && x.IsDeleted == false);
        }

        public async Task<IEnumerable<Contract>> FindAllContract()
        {
            return await _context.Contracts
                 .Include(x => x.ContractServices)
                .Include(x => x.CustomerDivision)
                .Where(entity => entity.IsDeleted == false)
                .OrderByDescending(entity => entity.ReferenceNo)
                .ToListAsync();
        }
        public async Task<Contract> FindContractByReferenceNumber(string refNo)
        {
            return await _context.Contracts
                .Include(x => x.ContractServices)
                .Include(x => x.CustomerDivision)
                .FirstOrDefaultAsync(x => x.ReferenceNo == refNo && x.IsDeleted == false);
        }

        public async Task<Contract> UpdateContract(Contract entity)
        {
            var contractEntity = _context.Contracts.Update(entity);
            if (await SaveChanges())
            {
                return contractEntity.Entity;
            }
            return null;
        }

        public async Task<bool> DeleteContract(Contract entity)
        {
            entity.IsDeleted = true;
            _context.Contracts.Update(entity);
            return await SaveChanges();
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