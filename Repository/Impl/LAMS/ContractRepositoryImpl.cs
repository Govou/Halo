using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HalobizMigrations.Data;
using HalobizMigrations.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace HaloBiz.Repository.Impl.LAMS
{
    public class ContractRepositoryImpl : IContractRepository
    {
        private readonly HalobizContext _context;
        private readonly ILogger<ContractRepositoryImpl> _logger;
        public ContractRepositoryImpl(HalobizContext context, ILogger<ContractRepositoryImpl> logger)
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

        public async Task<IEnumerable<Contract>> FindContractsByLeadId(long leadId)
        {
            var lead = await _context.Divisions.FindAsync(leadId);

            var customer = await _context.Customers.AsNoTracking()
                                        .Where(x => x.GroupName.ToLower() == lead.Name.ToLower())
                                        .FirstOrDefaultAsync();

            var customerDivisionIds = await _context.CustomerDivisions.AsNoTracking()
                                        .Where(x => x.CustomerId == customer.Id)
                                        .Select(x => x.Id)
                                        .ToListAsync();

            return await _context.Contracts.AsNoTracking()
                    .Include(x => x.ContractServices)
                    .Include(x => x.CustomerDivision)
                    .Where(x => !x.IsDeleted && customerDivisionIds.Contains(x.CustomerDivisionId))
                    .ToListAsync();
        }
    }
}