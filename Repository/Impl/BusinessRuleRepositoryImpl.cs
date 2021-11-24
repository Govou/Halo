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

        public async Task<bool> DeleteRule(BusinessRule businessRule)
        {
            businessRule.IsDeleted = true;
            _context.BusinessRules.Update(businessRule);
            return await SaveChanges();
        }

        public async Task<IEnumerable<BusinessRule>> FindAllRules()
        {
            return await _context.BusinessRules.Where(s => s.IsDeleted == false)
                                    .Include(s => s.ServiceRegistration).Include(s => s.CreatedBy)
                                              .ToListAsync();
        }

        public async Task<BusinessRule> FindRuleById(long Id)
        {
            return await _context.BusinessRules.Where(s => s.IsDeleted == false)
                .Include(s => s.ServiceRegistration).Include(s => s.CreatedBy)
                 .FirstOrDefaultAsync(ae => ae.Id == Id && ae.IsDeleted == false);
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

        public async Task<BusinessRule> SaveRule(BusinessRule businessRule)
        {
            var savedEntity = await _context.BusinessRules.AddAsync(businessRule);
            if (await SaveChanges())
            {
                return savedEntity.Entity;
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
    }
}
