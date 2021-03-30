using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using HaloBiz.Data;
using HaloBiz.Model.LAMS;
using Microsoft.Extensions.Logging;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using HaloBiz.Repository.LAMS;
using halobiz_backend.Helpers;

namespace HaloBiz.Repository.Impl.LAMS
{
    public class ContractServiceForEndorsementRepositoryImpl : IContractServiceForEndorsementRepository
    {
        private readonly DataContext _context;
        private readonly ILogger<ContractServiceForEndorsementRepositoryImpl> _logger;

        public ContractServiceForEndorsementRepositoryImpl(DataContext context, ILogger<ContractServiceForEndorsementRepositoryImpl> logger)
        {
            this._context = context;
            this._logger = logger;
        }

        public async Task<ContractServiceForEndorsement> FindContractServiceForEndorsementById(long Id)
        {
            return await _context.ContractServiceForEndorsements
                .Include(x => x.EndorsementType)
                .FirstOrDefaultAsync(x => x.Id == Id && x.IsDeleted == false);
        }

        public async Task<ContractServiceForEndorsement> GetEndorsementDetailsById(long endorsementId)
        {
            return await _context.ContractServiceForEndorsements.AsNoTracking()
                .Include(x => x.EndorsementType)
                .Include(x => x.Service)
                .ThenInclude(x => x.OperatingEntity)
                .Include(x => x.Service)
                .ThenInclude(x => x.ServiceCategory)
                .Include(x => x.Service)
                .ThenInclude(x => x.Division)
                .Include(x => x.Contract)
                .Include(x => x.CreatedBy)
                .Include(x => x.CustomerDivision)
                .FirstOrDefaultAsync(x => x.Id == endorsementId && x.IsDeleted == false);
        }

        public async Task<ContractServiceForEndorsement> SaveContractServiceForEndorsement(ContractServiceForEndorsement entity)
        {
            var contractServiceForEndorsementEntity = await _context.ContractServiceForEndorsements.AddAsync(entity);

            if (await SaveChanges())
            {
                return contractServiceForEndorsementEntity.Entity;
            }
            return null;            
        }
        public async Task<bool> SaveRangeContractServiceForEndorsement(IEnumerable<ContractServiceForEndorsement> entity)
        {
            await _context.ContractServiceForEndorsements.AddRangeAsync(entity);

            if (await SaveChanges())
            {
                return true;
            }
            return false;            
        }

        public async Task<IEnumerable<ContractServiceForEndorsement>> FindAllUnApprovedContractServicesForEndorsement()
        {
            return await _context.ContractServiceForEndorsements
                .Include(x => x.EndorsementType)
                .Where(x => x.IsRequestedForApproval && !x.IsApproved && !x.IsDeclined)
                .ToListAsync();
        }
        public async Task<IEnumerable<object>> FindAllPossibleEndorsementStartDate(long contractServiceId)
        {
            return await _context.Invoices
                .Where(x => !x.IsReversalInvoice && !x.IsDeleted && !x.IsReversed 
                        && x.StartDate > DateTime.Now && x.ContractServiceId == contractServiceId )
                .OrderBy(x => x.StartDate)
                .Select(x => new{startDate = x.StartDate, validDate = x.IsReceiptedStatus == InvoiceStatus.NotReceipted}).ToListAsync();
        }
        public async Task<ContractServiceForEndorsement> UpdateContractServiceForEndorsement(ContractServiceForEndorsement entity)
        {
            var contractServiceEntityForEndorsement = _context.ContractServiceForEndorsements.Update(entity);
            if (await SaveChanges())
            {
                return contractServiceEntityForEndorsement.Entity;
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