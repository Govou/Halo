using HalobizMigrations.Data;
using HalobizMigrations.Models;
using HalobizMigrations.Models.Armada;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HaloBiz.Repository.Impl
{
    public class ServiceAssignmentMasterRepositoryImpl: IServiceAssignmentMasterRepository
    {
        private readonly HalobizContext _context;
        private readonly ILogger<ServiceAssignmentMasterRepositoryImpl> _logger;
        public ServiceAssignmentMasterRepositoryImpl(HalobizContext context, ILogger<ServiceAssignmentMasterRepositoryImpl> logger)
        {
            this._logger = logger;
            this._context = context;
        }

        public async Task<bool> DeleteServiceAssignment(MasterServiceAssignment serviceAssignment)
        {
            serviceAssignment.IsDeleted = true;
            _context.MasterServiceAssignments.Update(serviceAssignment);
            return await SaveChanges();
        }

        public async Task<IEnumerable<object>> FindAllCustomerDivision()
        {
            return await _context.CustomerDivisions.Where(x => !x.IsDeleted)//.Include(x=>x.Contracts).Select(x=>x.ContractServiceForEndorsements)
               .OrderBy(x => x.DivisionName)
               .Select(x => new { Id = x.Id, DivisionName = x.DivisionName, Contracts = x.Contracts.Where(x=> x.IsDeleted == false), ContractServiceForEndorsements = x.ContractServiceForEndorsements })
               .ToListAsync();
        }

        public async Task<IEnumerable<MasterServiceAssignment>> FindAllServiceAssignments()
        {
            return await _context.MasterServiceAssignments.Where(type => type.IsDeleted == false)
               .Include(ct => ct.ContractService).Include(t=>t.CustomerDivision).Include(t=>t.SMORegion).Include(t=>t.SMORegion)

               .Include(t=>t.SMORoute).Include(t=>t.SourceType).Include(t=>t.TripType).Include(t=>t.CreatedBy).Include(t=>t.ServiceRegistration)
               .Include(t=>t.ServiceRegistration.Service).Include(t => t.ServiceRegistration.ApplicableArmedEscortTypes.Where(t=>t.IsDeleted == false))
               .Include(t => t.ServiceRegistration.ApplicableCommanderTypes.Where(t => t.IsDeleted == false))
               .Include(t => t.ServiceRegistration.ApplicablePilotTypes.Where(t => t.IsDeleted == false))
               .Include(t => t.ServiceRegistration.ApplicableVehicleTypes.Where(t => t.IsDeleted == false))
               .OrderByDescending(x => x.Id)
               .ToListAsync();
        }

        public async Task<MasterServiceAssignment> FindServiceAssignmentById(long Id)
        {
            return await _context.MasterServiceAssignments
               .Include(ct => ct.ContractService).Include(t => t.SMORegion).Include(t => t.SMORegion)
               .Include(t => t.SMORoute).Include(t => t.SourceType).Include(t => t.TripType).Include(t => t.CreatedBy).Include(t => t.ServiceRegistration)
               .Include(t => t.ServiceRegistration.Service).Include(t => t.ServiceRegistration.ApplicableArmedEscortTypes.Where(t => t.IsDeleted == false))
               .Include(t => t.ServiceRegistration.ApplicableCommanderTypes.Where(t => t.IsDeleted == false))
               .Include(t => t.ServiceRegistration.ApplicablePilotTypes.Where(t => t.IsDeleted == false))
               .Include(t => t.ServiceRegistration.ApplicableVehicleTypes.Where(t => t.IsDeleted == false))
               .FirstOrDefaultAsync(aer => aer.Id == Id && aer.IsDeleted == false);
        }

        public async Task<MasterServiceAssignment> SaveServiceAssignment(MasterServiceAssignment serviceAssignment)
        {
            var savedEntity = await _context.MasterServiceAssignments.AddAsync(serviceAssignment);
            if (await SaveChanges())
            {
                return savedEntity.Entity;
            }
            return null;
        }

        public async Task<bool> UpdateReadyStatus(MasterServiceAssignment serviceAssignment)
        {
            serviceAssignment.ReadyStatus = true;
            _context.MasterServiceAssignments.Update(serviceAssignment);
            return await SaveChanges();
        }

        public async Task<MasterServiceAssignment> UpdateServiceAssignment(MasterServiceAssignment serviceAssignment)
        {
            var updatedEntity = _context.MasterServiceAssignments.Update(serviceAssignment);
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
