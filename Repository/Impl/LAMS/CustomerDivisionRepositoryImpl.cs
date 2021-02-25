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
    public class CustomerDivisionRepositoryImpl : ICustomerDivisionRepository
    {
        private readonly DataContext _context;
        private readonly ILogger<CustomerDivisionRepositoryImpl> _logger;
        public CustomerDivisionRepositoryImpl(DataContext context, ILogger<CustomerDivisionRepositoryImpl> logger)
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
                .Include(x => x.AccountMaster)
                .FirstOrDefaultAsync(entity => entity.Id == Id && entity.IsDeleted == false);
            if(client == null)
            {
                return null;
            }
            client.Contracts = await _context.Contracts
                .Include(x => x.ContractServices)
                    .ThenInclude(x => x.Service)
                .Where(x => x.CustomerDivisionId == client.Id).ToListAsync();
            
            client.Customer.CustomerDivisions = null;

            client.PrimaryContact = await _context.LeadDivisionContacts
                .Where(x => x.Id == client.PrimaryContactId && x.IsDeleted == false).FirstOrDefaultAsync();

            client.SecondaryContact = await _context.LeadDivisionContacts
                .Where(x => x.Id == client.SecondaryContactId && x.IsDeleted == false).FirstOrDefaultAsync();
            
            return client;
        }

        public async Task<IEnumerable<CustomerDivision>> FindAllCustomerDivision()
        {
            return await _context.CustomerDivisions
                .Include(x => x.Customer)
                .Where(entity => entity.IsDeleted == false)
                .OrderByDescending(entity => entity.DivisionName)
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
        public async Task<CustomerDivision> FindCustomerDivisionByName(string name)
        {
            return await _context.CustomerDivisions
                .Include(x => x.Customer)
                .Include(x => x.AccountMaster)
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
    }
}
