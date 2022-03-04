using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HalobizMigrations.Data;
using HalobizMigrations.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace HaloBiz.Repository.Impl
{

    public class SupplierCategoryRepositoryImpl : ISupplierCategoryRepository
    {
        private readonly HalobizContext _context;
        private readonly ILogger<SupplierCategoryRepositoryImpl> _logger;
        public SupplierCategoryRepositoryImpl(HalobizContext context, ILogger<SupplierCategoryRepositoryImpl> logger)
        {
            this._logger = logger;
            this._context = context;
        }
        public async Task<SupplierCategory> SaveSupplierCategory(SupplierCategory supplierCategory)
        {
            var supplierCategoryEntity = await _context.SupplierCategories.AddAsync(supplierCategory);
            if(await SaveChanges())
            {
                return supplierCategoryEntity.Entity;
            }
            return null;
        }
        public async Task<IEnumerable<SupplierCategory>> GetSupplierCategories()
        {
            return await _context.SupplierCategories
                .Where(x => x.IsDeleted == false)
                .OrderBy(x => x.CategoryName)
                .ToListAsync();
        }

        public async Task<SupplierCategory> UpdateSupplierCategory(SupplierCategory supplierCategory)
        {
             var supplierCategoryEntity =  _context.SupplierCategories.Update(supplierCategory);
            if(await SaveChanges())
            {
                return supplierCategoryEntity.Entity;
            }
            return null;
        }
        public async Task<bool> DeleteSupplierCategory(SupplierCategory supplierCategory)
        {
            supplierCategory.IsDeleted = true;
            _context.SupplierCategories.Update(supplierCategory);
            return await SaveChanges();
        }
        public async Task<SupplierCategory> FindSupplierCategoryById(long Id)
        {
           return await _context.SupplierCategories
            .FirstOrDefaultAsync(x => x.Id == Id && x.IsDeleted == false);
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