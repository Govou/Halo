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
    public class FinancialVoucherTypeRepositoryImpl : IFinancialVoucherTypeRepository
    {
        private readonly DataContext _context;
        private readonly ILogger<FinancialVoucherTypeRepositoryImpl> _logger;
        public FinanceVoucherTypeRepositoryImpl(DataContext context, ILogger<FinancialVoucherTypeRepositoryImpl> logger)
        {
            this._logger = logger;
            this._context = context;

        }

        public async Task<FinanceVoucherType> SaveFinanceVoucherType(FinanceVoucherType entity)
        {
            var financialVoucherTypeEntity = await _context.FinanceVoucherTypes.AddAsync(entity);
            if(await SaveChanges())
            {
                return financialVoucherTypeEntity.Entity;
            }
            return null;
        }

        public async Task<FinanceVoucherType> FindFinanceVoucherTypeById(long Id)
        {
            return await _context.FinanceVoucherTypes
                .FirstOrDefaultAsync( entity => entity.Id == Id && entity.IsDeleted == false);
        }

        public async Task<IEnumerable<FinanceVoucherType>> FindAllFinanceVoucherType()
        {
            return await _context.FinanceVoucherTypes   
                .Where(entity => entity.IsDeleted == false)
                .OrderBy(entity => entity.CreatedAt)
                .ToListAsync();
        }

        public async Task<FinanceVoucherType> UpdateFinanceVoucherType(FinanceVoucherType entity)
        {
            var financialVoucherTypeEntity =  _context.FinanceVoucherTypes.Update(entity);
            if(await SaveChanges())
            {
                return financialVoucherTypeEntity.Entity;
            }
            return null;
        }

        public async Task<bool> DeleteFinanceVoucherType(FinanceVoucherType entity)
        {
            entity.IsDeleted = true;
            _context.FinanceVoucherTypes.Update(entity);
            return await SaveChanges();
        }
        private async Task<bool> SaveChanges()
        {
           try{
               return  await _context.SaveChangesAsync() > 0;
           }catch(Exception ex)
           {
               _logger.LogError(ex.Message);
               return false;
           }
        }
    }
}