using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HaloBiz.Model;
using HalobizMigrations.Data;
using HalobizMigrations.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace HaloBiz.Repository.Impl
{
    public class SupplierContactRepositoryImpl : ISupplierContactRepository
    {
        private readonly HalobizContext _context;
        private readonly ILogger<SupplierContactRepositoryImpl> _logger;

        public SupplierContactRepositoryImpl(HalobizContext context, ILogger<SupplierContactRepositoryImpl> logger)
        {
            this._logger = logger;
            this._context = context;
        }
        public async Task<SupplierContactMapping> SaveSupplierContact(SupplierContactMapping supplierContact)
        {
            var supplierContactEntity = await _context.SupplierContactMappings.AddAsync(supplierContact);
            if (await SaveChanges())
            {
                return supplierContactEntity.Entity;
            }
            return null;
        }

        public async Task<IEnumerable<SupplierContactMapping>> GetContactsBySupplier(long SupplierId)
        {
            return await _context.SupplierContactMappings
                 .Where(x => !x.IsDeleted && x.SupplierId == SupplierId)
                 .ToListAsync();
        }

        public async Task<bool> DeleteSupplierContact(long SupplierId)
        {
            _context.SupplierContactMappings.RemoveRange(_context.SupplierContactMappings.Where(x => x.SupplierId == SupplierId));
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
