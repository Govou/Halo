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

    public class SupplierRepositoryImpl : ISupplierRepository
    {
        private readonly HalobizContext _context;
        private readonly ILogger<SupplierRepositoryImpl> _logger;
        
        public SupplierRepositoryImpl(HalobizContext context, ILogger<SupplierRepositoryImpl> logger)
        {
            this._logger = logger;
            this._context = context;
        }
        public async Task<Supplier> SaveSupplier(Supplier supplier)
        {
            var supplierEntity = await _context.Suppliers.AddAsync(supplier);
            if(await SaveChanges())
            {
                return supplierEntity.Entity;
            }
            return null;
        }
        public async Task<IEnumerable<Supplier>> GetSuppliers()
        {
            return await _context.Suppliers
                .Where(x => !x.IsDeleted)
                .OrderBy(x => x.SupplierName)
                .ToListAsync();
        }

        public async Task<Supplier> UpdateSupplier(Supplier supplier)
        {
             var supplierEntity =  _context.Suppliers.Update(supplier);
            if(await SaveChanges())
            {
                return supplierEntity.Entity;
            }
            return null;
        }
        public async Task<bool> DeleteSupplier(Supplier supplier)
        {
            supplier.IsDeleted = true;
            _context.Suppliers.Update(supplier);
            return await SaveChanges();
        }
        public async Task<Supplier> FindSupplierById(long Id)
        {
           return await _context.Suppliers
            .FirstOrDefaultAsync(x => x.Id == Id && x.IsDeleted == false);
        }

        public async Task<List<IValidation>> ValidateSupplier(string supplierName, string supplierEmail, string supplierPhone)
        {
            List<Supplier> validateName = await _context.Suppliers
                .Where(x => !x.IsDeleted && x.SupplierName == supplierName)
                .OrderBy(x => x.SupplierName)
                .ToListAsync();

            List<Supplier> validateEmail = await _context.Suppliers
                .Where(x => !x.IsDeleted && x.SupplierEmail == supplierEmail)
                .OrderBy(x => x.SupplierName)
                .ToListAsync();

            List<Supplier> validatePhone = await _context.Suppliers
                .Where(x => !x.IsDeleted && x.MobileNumber == supplierPhone)
                .OrderBy(x => x.SupplierName)
                .ToListAsync();

            List<IValidation> res = new List<IValidation>();

            if (validateName.Count > 0)
            {
                res.Add(new IValidation { Message="Supplier Name Already In Use",Field="Name"});
            }

            if (validateEmail.Count > 0)
            {
                res.Add(new IValidation { Message = "Supplier Email Already In Use", Field = "Email" });
            }

            if (validatePhone.Count > 0)
            {
                res.Add(new IValidation { Message = "Supplier Phone Already In Use", Field = "Phone" });
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