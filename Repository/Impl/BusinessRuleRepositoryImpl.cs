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
    public class BusinessRuleRepositoryImpl:IBusinessRulesRepository
    {
        private readonly HalobizContext _context;
        private readonly ILogger<BusinessRuleRepositoryImpl> _logger;
        public BusinessRuleRepositoryImpl(HalobizContext context, ILogger<BusinessRuleRepositoryImpl> logger)
        {
            this._logger = logger;
            this._context = context;
        }

        public async Task<bool> DeletePairable(BRPairable bRPairable)
        {
            bRPairable.IsDeleted = true;
            _context.BRPairables.Update(bRPairable);
            return await SaveChanges();
        }

        public async Task<bool> DeleteRule(BusinessRule businessRule)
        {
            businessRule.IsDeleted = true;
            _context.BusinessRules.Update(businessRule);
            return await SaveChanges();
        }

        public async Task<IEnumerable<BRPairable>> FindAllActivePairables()
        {
            return await _context.BRPairables.Where(s => s.IsDeleted == false && s.BusinessRule.IsPairingRequired == true)
                                              .Include(s => s.BusinessRule).Include(s => s.ServiceRegistration)
                                                        .ToListAsync();
        }

        public async Task<IEnumerable<BusinessRule>> FindAllPairableRules()
        {
            return await _context.BusinessRules.Where(s => s.IsDeleted == false && s.IsPairingRequired == true)
                                 .Include(s => s.ServiceRegistration).Include(s => s.ServiceRegistration.Service)
                                 .Include(s => s.CreatedBy).Include(s => s.ServiceRegistration.Service.ServiceCategory)
                                 .Include(s => s.ServiceRegistration.Service.Division).Include(s => s.ServiceRegistration.Service.ServiceGroup)
                                 .Include(s => s.ServiceRegistration.Service.ServiceType)
                                           .ToListAsync();
        }

        public async Task<IEnumerable<BRPairable>> FindAllPairables()
        {
            return await _context.BRPairables.Where(s => s.IsDeleted == false)
                                             .Include(s => s.BusinessRule).Include(s => s.BusinessRule.ServiceRegistration)
                .Include(s => s.BusinessRule.ServiceRegistration.Service)
               .Include(s => s.ServiceRegistration).Include(s => s.ServiceRegistration.Service)
                                   .Include(s => s.CreatedBy)
                                   .Include(s => s.ServiceRegistration.Service.ServiceType)

                                   .Include(s=>s.CreatedBy)
                                                        .ToListAsync();
        }

        public async Task<IEnumerable<BusinessRule>> FindAllRules()
        {
            return await _context.BusinessRules.Where(s => s.IsDeleted == false)
                                    .Include(s => s.ServiceRegistration).Include(s=>s.ServiceRegistration.Service)
                                    .Include(s => s.CreatedBy).Include(s=>s.ServiceRegistration.Service.ServiceCategory)
                                    .Include(s=>s.ServiceRegistration.Service.Division).Include(s=>s.ServiceRegistration.Service.ServiceGroup)
                                    .Include(s=>s.ServiceRegistration.Service.ServiceType)
                                              .ToListAsync();
        }

        public async Task<BRPairable> FindPairableById(long Id)
        {
            return await _context.BRPairables.Where(s => s.IsDeleted == false).Include(s=>s.BusinessRule).Include(s=>s.BusinessRule.ServiceRegistration)
                .Include(s=>s.BusinessRule.ServiceRegistration.Service)
               .Include(s => s.ServiceRegistration).Include(s => s.ServiceRegistration.Service)
                                   .Include(s => s.CreatedBy).Include(s => s.ServiceRegistration.Service.ServiceCategory)
                                 
                .FirstOrDefaultAsync(ae => ae.Id == Id && ae.IsDeleted == false);
        }

        public async Task<BusinessRule> FindRuleById(long Id)
        {
            return await _context.BusinessRules.Where(s => s.IsDeleted == false)
                .Include(s => s.ServiceRegistration).Include(s => s.ServiceRegistration.Service)
                                    .Include(s => s.CreatedBy).Include(s => s.ServiceRegistration.Service.ServiceCategory)
                                    .Include(s => s.ServiceRegistration.Service.Division).Include(s => s.ServiceRegistration.Service.ServiceGroup)
                                    .Include(s => s.ServiceRegistration.Service.ServiceType).Include(s => s.ServiceRegistration.Service.Target)
                 .FirstOrDefaultAsync(ae => ae.Id == Id && ae.IsDeleted == false);
        }

        public BRPairable GetBusinessAndRegServiceId(long? BusinessRuleId, long RegServiceId)
        {
            return _context.BRPairables.Where(ct => ct.ServiceRegistrationId == RegServiceId && ct.BusinessRuleId ==BusinessRuleId && ct.IsDeleted == false).FirstOrDefault();
        }

        public BRPairable GetBusinessRileRegServiceId(long? regServiceId)
        {
            return _context.BRPairables.Where(ct => ct.BusinessRuleId == regServiceId && ct.IsDeleted == false).FirstOrDefault();
        }

        public BusinessRule GetRegServiceId(long regServiceId)
        {
            return _context.BusinessRules.Where(ct => ct.ServiceRegistrationId == regServiceId && ct.IsDeleted == false).FirstOrDefault();
        }

        public bool requiresArmedEscort(long? id)
        {
            var requires = _context.ServiceRegistrations
                              .Where(ct => ct.RequiresArmedEscort == true && ct.Id == id ).FirstOrDefault();
            if (requires != null) return true;
            else
                return false;
        }

        public bool requiresCommander(long? id)
        {
            var requires = _context.ServiceRegistrations
                                        .Where(ct => ct.RequiresCommander == true && ct.Id == id).FirstOrDefault();
            if (requires != null) return true;
            else
                return false;
        }

        public bool requiresPilot(long? id)
        {
            var requires = _context.ServiceRegistrations
                                        .Where(ct => ct.RequiresPilot == true && ct.Id == id).FirstOrDefault();
            if (requires != null) return true;
            else
                return false;
        }

        public bool requiresVehicle(long? id)
        {
            var requires = _context.ServiceRegistrations
                                        .Where(ct => ct.RequiresVehicle == true && ct.Id == id).FirstOrDefault();
            if (requires != null) return true;
            else
                return false;
        }

        public async Task<BRPairable> SavePairable(BRPairable bRPairable)
        {
            _context.Database.ExecuteSqlRaw("SET IDENTITY_INSERT armada.BRPairables ON;");
            var savedEntity = await _context.BRPairables.AddAsync(bRPairable);
            if (await SaveMultiSelectChanges())
            {
                return savedEntity.Entity;
            }
            return null;
        }
        public async Task<List<BRPairable>> SaveRangePairable(List<BRPairable> bRPairable)
        {
              await _context.BRPairables.AddRangeAsync(bRPairable);
            await _context.SaveChangesAsync();
            return bRPairable;
            //return null;
        }

        public async Task<BusinessRule> SaveRule(BusinessRule businessRule)
        {
            var savedEntity = await _context.BusinessRules.AddAsync(businessRule);
            if (await SaveChanges())
            {
                return savedEntity.Entity;
            }
            return null;
        }

        public async Task<BRPairable> UpdatePairable(BRPairable bRPairable)
        {
            var updatedEntity = _context.BRPairables.Update(bRPairable);
            if (await SaveChanges())
            {
                return updatedEntity.Entity;
            }
            return null;
        }

        public async Task<BusinessRule> UpdateRule(BusinessRule businessRule)
        {
            var updatedEntity = _context.BusinessRules.Update(businessRule);
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
        private async Task<bool> SaveMultiSelectChanges()
        {
            try
            {
                _context.Database.ExecuteSqlRaw("SET IDENTITY_INSERT armada.BRPairables ON;");
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
