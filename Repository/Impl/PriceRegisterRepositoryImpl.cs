using HalobizMigrations.Data;
using HalobizMigrations.Models.Armada;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HaloBiz.Repository.Impl
{
    public class PriceRegisterRepositoryImpl:IPriceRegisterRepository
    {
        private readonly HalobizContext _context;
        private readonly ILogger<PriceRegisterRepositoryImpl> _logger;
        public PriceRegisterRepositoryImpl(HalobizContext context, ILogger<PriceRegisterRepositoryImpl> logger)
        {
            this._logger = logger;
            this._context = context;
        }

        public async Task<bool> DeletePriceRegister(PriceRegister priceRegister)
        {
            priceRegister.IsDeleted = true;
            _context.PriceRegisters.Update(priceRegister);
            return await SaveChanges();
        }

        public async Task<IEnumerable<PriceRegister>> FindAllPriceRegisters()
        {
            return await _context.PriceRegisters.Where(rank => rank.IsDeleted == false)
               .Include(ct => ct.ServiceRegistration).Include(ct=>ct.ServiceRegistration.Service)
               .Include(ct=>ct.SMORegion).Include(ct=>ct.SMORoute).Include(ct=>ct.CreatedBy)
               .ToListAsync();
        }

        public async Task<PriceRegister> FindPriceRegisterById(long Id)
        {
            return await _context.PriceRegisters.Include(ct => ct.ServiceRegistration).Include(ct => ct.ServiceRegistration.Service)
               .Include(ct => ct.SMORegion).Include(ct => ct.SMORoute).Include(ct => ct.CreatedBy)
                .FirstOrDefaultAsync(aer => aer.Id == Id && aer.IsDeleted == false);
        }

        public async Task<PriceRegister> SavePriceRegister(PriceRegister priceRegister)
        {
            var savedEntity = await _context.PriceRegisters.AddAsync(priceRegister);
            if (await SaveChanges())
            {
                return savedEntity.Entity;
            }
            return null;
        }

        public async Task<PriceRegister> UpdatePriceRegister(PriceRegister priceRegister)
        {
            var updatedEntity = _context.PriceRegisters.Update(priceRegister);
            if (await SaveChanges())
            {
                return updatedEntity.Entity;
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
