using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using HalobizMigrations.Data;

using Microsoft.Extensions.Logging;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using HaloBiz.Repository.LAMS;
using halobiz_backend.Helpers;
using HalobizMigrations.Models;

namespace HaloBiz.Repository.Impl.LAMS
{
    public class ContractServiceForEndorsementRepositoryImpl : IContractServiceForEndorsementRepository
    {
        private readonly HalobizContext _context;
        private readonly ILogger<ContractServiceForEndorsementRepositoryImpl> _logger;

        public ContractServiceForEndorsementRepositoryImpl(HalobizContext context, ILogger<ContractServiceForEndorsementRepositoryImpl> logger)
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
            var result = await _context.ContractServiceForEndorsements.AsNoTracking()
                .Include(x => x.EndorsementType)
                .Include(x => x.Contract)
                .Include(x => x.CreatedBy)
                .Include(x => x.Branch)
                .Include(x => x.Office)
                .FirstOrDefaultAsync(x => x.Id == endorsementId && x.IsDeleted == false);

            result.Service = await _context.Services.AsNoTracking()
                                        .Include(x => x.OperatingEntity)
                                        .Include(x => x.ServiceCategory)
                                        .Include(x => x.Division)
                                        .FirstOrDefaultAsync(x => x.Id == result.ServiceId && x.IsDeleted != true);
            result.CustomerDivision = await _context.CustomerDivisions.AsNoTracking()
                                                .Include(x => x.Customer)
                                                .FirstOrDefaultAsync(x => x.Id == result.CustomerDivisionId && x.IsDeleted != true);

            return result;
        }

        public async Task<ContractServiceForEndorsement> SaveContractServiceForEndorsement(ContractServiceForEndorsement entity)
        {
            try
            {
                var contractServiceForEndorsementEntity = await _context.ContractServiceForEndorsements.AddAsync(entity);
               int affected = await _context.SaveChangesAsync();
                return affected > 0 ? entity : null;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.StackTrace);
                throw;
            }
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
            return await _context.ContractServiceForEndorsements.AsNoTracking()
                .Include(x => x.EndorsementType)
                .Include(x => x.CustomerDivision)
                .Where(x => x.IsRequestedForApproval && !x.IsApproved && !x.IsDeclined)
                .ToListAsync();
        }
        public async Task<IEnumerable<object>> FindAllPossibleEndorsementStartDate(long contractServiceId)
        {
            var contractService = await _context.ContractServices.FindAsync(contractServiceId);

            if (contractService == null) return Array.Empty<object>();

            return await _context.Invoices
               .Where(x => !x.IsReversalInvoice.Value && !x.IsDeleted && !x.IsReversed.Value
                       && x.StartDate > DateTime.Now && x.ContractServiceId == contractServiceId && x.Quantity > 0 && x.IsAccountPosted==false)
               .OrderBy(x => x.StartDate)
               .Select(x => new { startDate = x.StartDate, validDate = x.IsReceiptedStatus == (int)InvoiceStatus.NotReceipted }).ToListAsync();
          

            //if(contractService.Contract?.GroupInvoiceNumber != null)
            //{
            //    var invoices = await _context.Invoices
            //    .Where(x => !x.IsReversalInvoice.Value && !x.IsDeleted && !x.IsReversed.Value
            //            && x.StartDate > DateTime.Now && x.GroupInvoiceNumber == contractService.Contract.GroupInvoiceNumber)
            //    .ToListAsync();
            //    //.OrderBy(x => x.StartDate)
            //    return invoices.GroupBy(x => x.StartDate)
            //    .Select(x => new { startDate = x.Key, validDate = x.All(y => y.IsReceiptedStatus == (int)InvoiceStatus.NotReceipted) })
            //    .OrderBy(x => x.startDate).ToList();
            //    //.Select(x => new { startDate = x.StartDate, validDate = x.IsReceiptedStatus == (int)InvoiceStatus.NotReceipted }).ToListAsync();
            //}   
        }

        public async Task<IEnumerable<object>> GetEndorsementHistory(long contractServiceId)
        {
            var contractService = await _context.ContractServices.FindAsync(contractServiceId);

            if (contractService == null) return Array.Empty<object>();

            return await _context.ContractServiceForEndorsements
                            .Include(x => x.EndorsementType)
                            .Where(x => x.PreviousContractServiceId == contractServiceId)
                            .OrderBy(x => x.CreatedAt)
                            .Select(x => new 
                            {
                                ContractServiceId = x.PreviousContractServiceId,
                                x.EndorsementDescription,
                                x.EndorsementType,
                                x.BillableAmount
                            })
                            .ToListAsync();
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