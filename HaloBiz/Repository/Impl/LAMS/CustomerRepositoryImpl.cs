using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HalobizMigrations.Data;

using HaloBiz.Repository.LAMS;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using HalobizMigrations.Models;

namespace HaloBiz.Repository.Impl.LAMS
{
    public class CustomerRepositoryImpl : ICustomerRepository
    {
        private readonly HalobizContext _context;
        private readonly ILogger<CustomerRepositoryImpl> _logger;
        public CustomerRepositoryImpl(HalobizContext context, ILogger<CustomerRepositoryImpl> logger)
        {
            this._logger = logger;
            this._context = context;
        }

        public async Task<Customer> SaveCustomer(Customer entity)
        {
            var CustomerEntity = await _context.Customers.AddAsync(entity);

            if (await SaveChanges())
            {
                return CustomerEntity.Entity;
            }
            return null;            
        }



        public async Task<Customer> FindCustomerById(long Id)
        {
            var customer =  await _context.Customers
            .Include(x => x.LeadKeyPeople)
                .Include(x => x.GroupType)
                .FirstOrDefaultAsync(entity => entity.Id == Id && entity.IsDeleted == false);
            if(customer != null)
            {
                customer.CustomerDivisions = await _context.CustomerDivisions
                    .Include(x => x.LeadKeyPeople)
                    .Where(x => x.CustomerId == customer.Id && x.IsDeleted == false)
                    .ToListAsync();
            }
            foreach (var division in customer.CustomerDivisions)
            {
                division.Customer = null;
            }
            return customer;
        }

        public async Task<IEnumerable<Customer>> FindAllCustomer()
        {
            return await _context.Customers
                .Where(entity => entity.IsDeleted == false)
                .OrderByDescending(entity => entity.GroupName)
                .ToListAsync();
        }

        public async Task<IEnumerable<object>> FindCustomersByGroupType(long groupTypeId)
        {
            return await _context.Customers
                .Where(entity => entity.IsDeleted == false && entity.GroupTypeId == groupTypeId)
                .OrderByDescending(entity => entity.GroupName).Select(x => new {id = x.Id, groupName = x.GroupName})
                .ToListAsync();
        }
        public async Task<Customer> FindCustomerByName(string name)
        {
            var customer =  await _context.Customers
                .Include(x => x.GroupType)
                .FirstOrDefaultAsync(entity => entity.GroupName == name && entity.IsDeleted == false);
            if(customer != null)
            {
                customer.CustomerDivisions = await _context.CustomerDivisions
                    .Where(x => x.CustomerId == customer.Id)
                    .ToListAsync();
            }
            foreach (var division in customer.CustomerDivisions)
            {
                division.Customer = null;
            }
            return customer;
        }

        public async Task<Customer> UpdateCustomer(Customer entity)
        {
            var CustomerEntity = _context.Customers.Update(entity);
            if (await SaveChanges())
            {
                return CustomerEntity.Entity;
            }
            return null;
        }

        public async Task<bool> DeleteCustomer(Customer entity)
        {
            entity.IsDeleted = true;
            _context.Customers.Update(entity);
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
