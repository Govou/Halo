using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HalobizMigrations.Data;
using HaloBiz.DTOs.TransferDTOs.LAMS;
using HaloBiz.Repository.LAMS;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using HalobizMigrations.Models;

namespace HaloBiz.Repository.Impl.LAMS
{
    public class CustomerDivisionRepositoryImpl : ICustomerDivisionRepository
    {
        private readonly HalobizContext _context;
        private readonly ILogger<CustomerDivisionRepositoryImpl> _logger;
        public CustomerDivisionRepositoryImpl(HalobizContext context, ILogger<CustomerDivisionRepositoryImpl> logger)
        {
            this._logger = logger;
            this._context = context;
        }

        public async Task<CustomerDivision> SaveCustomerDivision(CustomerDivision entity)
        {
            var CustomerDivisionEntity = await _context.CustomerDivisions.AddAsync(entity);

            if (await SaveChanges())
            {
                return CustomerDivisionEntity.Entity;
            }
            return null;            
        }

        public async Task<CustomerDivision> FindCustomerDivisionById(long Id)
        {
            var client = await _context.CustomerDivisions
                .Include(x => x.Customer)
                .Include(x => x.AccountMasters)
                .FirstOrDefaultAsync(entity => entity.Id == Id && entity.IsDeleted == false);
            if(client == null)
            {
                return null;
            }
            client.Contracts = await _context.Contracts
                .Include(x => x.ContractServices.Where(x => x.Version == (int)Helpers.VersionType.Latest && x.ContractEndDate >= DateTime.Now))
                    .ThenInclude(x => x.Service)
                .Where(x => x.CustomerDivisionId == client.Id).ToListAsync();
            
            client.Customer.CustomerDivisions = null;

            client.PrimaryContact = await _context.LeadDivisionContacts
                .Where(x => x.Id == client.PrimaryContactId && x.IsDeleted == false).FirstOrDefaultAsync();

            client.SecondaryContact = await _context.LeadDivisionContacts
                .Where(x => x.Id == client.SecondaryContactId && x.IsDeleted == false).FirstOrDefaultAsync();

            return client;
        }

        public async Task<CustomerDivision> FindCustomerDivisionByDTrackCustomerNumber(string dTrackCustomerNumber)
        {
            var client = await _context.CustomerDivisions.AsNoTracking()
                .Include(x => x.Customer)
                .FirstOrDefaultAsync(entity => entity.DTrackCustomerNumber == dTrackCustomerNumber && !entity.IsDeleted);
            
            if (client == null)
            {
                return null;
            }

            client.Customer.CustomerDivisions = null;

            return client;
        }

        public async Task<List<TaskFulfillment>> FindTaskAndFulfillmentsByCustomerDivisionId(long customerDivisionId)
        {
            var taskFulfillments = await _context.TaskFulfillments.Where(x => x.CustomerDivisionId == customerDivisionId && x.IsDeleted == false)
                .Include(x => x.Accountable)
                .Include(x => x.Consulted)
                .Include(x => x.Informed)
                .Include(x => x.Responsible)
                .Include(x => x.Support)
                .Include(x => x.DeliverableFulfillments.Where(x => x.IsDeleted == false))
                    .ThenInclude(x => x.Responsible)
                .Include(x => x.DeliverableFulfillments.Where(x => x.IsDeleted == false))
                    .ThenInclude(x => x.CreatedBy)
                    .ToListAsync();

            return taskFulfillments;
        }

        public async Task<IEnumerable<object>> FindAllCustomerDivision()
        {
            return await _context.CustomerDivisions.Where(x => !x.IsDeleted)
                .OrderBy(x => x.DivisionName)
                .Select(x => new { Id = x.Id, DivisionName = x.DivisionName })
                .ToListAsync();
        }

        public async Task<IEnumerable<object>> FindCustomerDivisionsByGroupType(long groupTypeId)
        {
           return await _context.Customers.Join(
               _context.CustomerDivisions,
               customer => customer.Id, division => division.CustomerId,
               (customer, division) => new {
                    GroupTypeId = customer.GroupTypeId,
                    ClientName = division.DivisionName,
                    ClientId = division.Id,
                    IsDeleted = division.IsDeleted
               }
           ).Where(x => x.GroupTypeId == groupTypeId && x.IsDeleted == false).ToListAsync();
        }

        public async Task<CustomerDivision> GetCustomerDivisionBreakDownById(long id)
        {
            var client = await _context.CustomerDivisions
                .Include(x => x.OtherLeadCaptureInfo)
                .Where(x => x.Id == id && x.IsDeleted == false).FirstOrDefaultAsync();
            if(client == null)
            {
                return null;
            }
            client.Contracts = await _context.Contracts
                .Where(x => x.IsDeleted == false && x.Id == client.CustomerId).ToListAsync();

            foreach (var contract in client.Contracts)
            {
                contract.ContractServices = await _context.ContractServices
                        .Where(x => x.ContractId == contract.Id && !x.IsDeleted).ToListAsync();
            }
            return client;
        }        

        public async Task<List<ContractToPaidAmountTransferDTO>> GetPaymentsPerContractByCustomerDivisionId(long customerId)
        {
            var totalPayedAmount = await _context.CustomerDivisions.Join(
                _context.Contracts, 
                division => division.Id, contract => contract.CustomerDivisionId,
                (contract, division) => new {
                    ContractId = contract.Id,
                    CuatomerId = division.Id
                }
            ).Join( 
                _context.Invoices, contract => contract.ContractId, invoice => invoice.ContractId,
                (prev, invoice) => new {
                    ContractId = prev.ContractId,
                    InvoiceId = invoice.Id,
                    CuatomerId = prev.CuatomerId,
                    IsInvoiceDeleted = invoice.IsDeleted
                }
             ).Join(
                 _context.Receipts, prev => prev.InvoiceId, receipt => receipt.InvoiceId,
                 (prev, receipt) => new {
                     ContractId = prev.ContractId,
                     Amount = receipt.ReceiptValue,
                     CustomerId = prev.CuatomerId,
                     IsReceiptDeleted = receipt.IsDeleted,
                     IsInvoiceDeleted = prev.IsInvoiceDeleted
                 }
             ).Where( x => x.CustomerId == customerId && !x.IsInvoiceDeleted && !x.IsReceiptDeleted)
                .GroupBy(x => new{
                    ContractId = x.ContractId
                }).Select(y => new ContractToPaidAmountTransferDTO(){
                    AmountPaid = y.Sum( x => x.Amount),
                    ContractId = y.Key.ContractId
                }).ToListAsync();

             return totalPayedAmount;

        }
        
        public async Task<CustomerDivision> FindCustomerDivisionByName(string name)
        {
            return await _context.CustomerDivisions
                .Include(x => x.Customer)
                .Include(x => x.AccountMasters)
                .FirstOrDefaultAsync(entity => entity.DivisionName == name && entity.IsDeleted == false);
        }

        public async Task<CustomerDivision> UpdateCustomerDivision(CustomerDivision entity)
        {
            var CustomerDivisionEntity = _context.CustomerDivisions.Update(entity);
            if (await SaveChanges())
            {
                return CustomerDivisionEntity.Entity;
            }
            return null;
        }

        public async Task<bool> DeleteCustomerDivision(CustomerDivision entity)
        {
            entity.IsDeleted = true;
            _context.CustomerDivisions.Update(entity);
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

        public Task<List<CustomerDivision>> GetClientsWithSecuredMobilityContractServices()
        {
            var clients = _context.CustomerDivisions
                .Where(x => x.Contracts.Any(x => x.ContractServices.Any(x => x.Service.ServiceCategory.Name == "Secured Mobility")))
                .Include(x => x.Contracts)
                .ThenInclude(x => x.ContractServices)
                .ThenInclude(x => x.Service)
                .ToListAsync();

            return clients;
        }

        public async Task<IEnumerable<object>> GetClientsUnAssignedToRMSbu()
        {
            var customerDivisions = await _context.CustomerDivisions
                .Include(x => x.Customer)
                .ThenInclude(x => x.GroupType)
                .Where(x => !x.IsDeleted && x.SbuId == null).Select(x => new 
                {
                    x.Id,
                    x.DivisionName,
                    x.Customer.GroupName,
                    GroupType = x.Customer.GroupType.Caption
                })
                .ToListAsync();

            return customerDivisions;
        }

        public async Task<IEnumerable<object>> GetClientsAttachedToRMSbu(long sbuId)
        {
            var customerDivisions = await _context.CustomerDivisions
                .Include(x => x.Customer)
                .ThenInclude(x => x.GroupType)
                .Where(x => !x.IsDeleted && x.SbuId == sbuId).Select(x => new
                {
                    x.Id,
                    x.DivisionName,
                    x.Customer.GroupName,
                    GroupType = x.Customer.GroupType.Caption
                })
                .ToListAsync();

            return customerDivisions;
        }

        public async Task<IEnumerable<object>> GetRMSbuClientsByGroupType(long sbuId, long clientTypeId)
        {
            var customerDivisions = await _context.CustomerDivisions
                .Include(x => x.Customer)
                .ThenInclude(x => x.GroupType)
                .Where(x => !x.IsDeleted && x.SbuId == sbuId && x.Customer.GroupTypeId == clientTypeId)
                .Select(x => new
                {
                    x.Id,
                    x.DivisionName,
                    x.Customer.GroupName,
                    GroupType = x.Customer.GroupType.Caption,
                    x.CreatedAt
                })
                .OrderBy(x => x.CreatedAt)
                .ToListAsync();

            return customerDivisions;
        }
    }
}
