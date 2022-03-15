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

    public class SupplierServiceRepositoryImpl : ISupplierServiceRepository
    {
        private readonly HalobizContext _context;
        private readonly ILogger<SupplierServiceRepositoryImpl> _logger;
        public SupplierServiceRepositoryImpl(HalobizContext context, ILogger<SupplierServiceRepositoryImpl> logger)
        {
            this._logger = logger;
            this._context = context;
        }
        public async Task<SupplierService> SaveSupplierService(SupplierService supplier)
        {
            var supplierEntity = await _context.SupplierServices.AddAsync(supplier);
            if(await SaveChanges())
            {
                return supplierEntity.Entity;
            }
            return null;
        }
        public async Task<IEnumerable<SupplierService>> GetSupplierServices()
        {
            return await _context.SupplierServices
                .Where(x => !x.IsDeleted)
                .OrderBy(x => x.ServiceName)
                .ToListAsync();
        }

        public async Task<SupplierService> UpdateSupplierService(SupplierService supplier)
        {
             var supplierEntity =  _context.SupplierServices.Update(supplier);
            if(await SaveChanges())
            {
                return supplierEntity.Entity;
            }
            return null;
        }
        public async Task<bool> DeleteSupplierService(SupplierService supplier)
        {
            supplier.IsDeleted = true;
            _context.SupplierServices.Update(supplier);
            return await SaveChanges();
        }
        public async Task<SupplierService> FindSupplierServiceById(long Id)
        {
           return await _context.SupplierServices
            .FirstOrDefaultAsync(x => x.Id == Id && x.IsDeleted == false);
        }

        public async Task<List<IValidation>> ValidateSupplierService(string supplierServiceName, long supplierID)
        {
            List<SupplierService> validateName = await _context.SupplierServices
                .Where(x => !x.IsDeleted && x.ServiceName == supplierServiceName && x.SupplierId == supplierID)
                .OrderBy(x => x.ServiceName)
                .ToListAsync();

            List<IValidation> res = new List<IValidation>();

            if (validateName.Count > 0)
            {
                res.Add(new IValidation { Message = "Supplier Service With This Name Already In Use", Field = "Name" });
            }

            return res;

        }

        private async Task<bool> SaveChanges()
        {
           try
           {
               return  await _context.SaveChangesAsync() > 0;
           }
           catch(Exception ex)
           {
               _logger.LogError(ex.Message);
               return false;
           }
        }
    }
}