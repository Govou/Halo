using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HaloBiz.Data;
using HaloBiz.Model.LAMS;
using HaloBiz.Repository.LAMS;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace HaloBiz.Repository.Impl.LAMS
{
    public class ContractServiceRepositoryImpl : IContractServiceRepository
    {
        private readonly DataContext _context;
        private readonly ILogger<ContractServiceRepositoryImpl> _logger;
        public ContractServiceRepositoryImpl(DataContext context, ILogger<ContractServiceRepositoryImpl> logger)
        {
            this._logger = logger;
            this._context = context;
        }

        public async Task<ContractService> SaveContractService(ContractService entity)
        {
            var contractServiceEntity = await _context.ContractServices.AddAsync(entity);

            if (await SaveChanges())
            {
                return contractServiceEntity.Entity;
            }
            return null;            
        }

        public async Task<ContractService> FindContractServiceById(long Id)
        {
            return await _context.ContractServices
                .Include(x => x.Contract)
                .Include(x => x.QuoteService)
                .Include(x => x.Service)
                .Include(x => x.SBUToContractServiceProportions)
                .Include(x => x.ClosureDocuments)
                .FirstOrDefaultAsync(x => x.Id == Id && x.IsDeleted == false);
        }

        public async Task<IEnumerable<ContractService>> FindAllContractServicesForAContract(long contractId)
        {
            return await _context.ContractServices
                .Include(x => x.Contract)
                .Include(x => x.QuoteService)
                .Include(x => x.Service)
                .Include(x => x.SBUToContractServiceProportions)
                .Include(x => x.ClosureDocuments)
                .Where( x => x.ContractId == contractId && x.IsDeleted == false)
                .ToListAsync();
        }
        public async Task<IEnumerable<ContractService>> FindContractServicesByReferenceNumber(string refNo)
        {
            return await _context.ContractServices
                .Include(x => x.Contract)
                .Include(x => x.QuoteService)
                .Include(x => x.Service)
                .Include(x => x.SBUToContractServiceProportions)
                .Include(x => x.ClosureDocuments)
                .Where(x => x.ReferenceNo == refNo && x.IsDeleted == false)
                .ToListAsync();
        }

        public async Task<IEnumerable<ContractService>> FindContractServicesByGroupInvoiceNumber(string groupInvoiceNumber)
        {
            return await _context.ContractServices
                .Include(x => x.Service)
                .Where(x => x.GroupInvoiceNumber == groupInvoiceNumber && x.IsDeleted == false)
                .ToListAsync();
        }

        public async Task<ContractService> UpdateContractService(ContractService entity)
        {
            var contractServiceEntity = _context.ContractServices.Update(entity);
            if (await SaveChanges())
            {
                return contractServiceEntity.Entity;
            }
            return null;
        }

        public async Task<bool> DeleteContractService(ContractService entity)
        {
            entity.IsDeleted = true;
            _context.ContractServices.Update(entity);
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