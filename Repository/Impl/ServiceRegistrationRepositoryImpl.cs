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
    public class ServiceRegistrationRepositoryImpl:IServiceRegistrationRepository
    {
        private readonly HalobizContext _context;
        private readonly ILogger<ServiceRegistrationRepositoryImpl> _logger;
        public ServiceRegistrationRepositoryImpl(HalobizContext context, ILogger<ServiceRegistrationRepositoryImpl> logger)
        {
            this._logger = logger;
            this._context = context;
        }

        public async Task<bool> DeleteService(ServiceRegistration serviceRegistration)
        {
            serviceRegistration.IsDeleted = true;
            _context.ServiceRegistrations.Update(serviceRegistration);
            return await SaveChanges();
        }

        public async Task<IEnumerable<ServiceRegistration>> FindAllServicess()
        {
            return await _context.ServiceRegistrations.Where(s => s.IsDeleted == false)
                          .Include(s=>s.ApplicableArmedEscortTypes.Where(type=>type.IsDeleted == false))
                          .Include(s=>s.ApplicableCommanderTypes.Where(type=>type.IsDeleted == false))
                          .Include(s=>s.ApplicablePilotTypes.Where(type=>type.IsDeleted ==  false))
                          .Include(s=>s.ApplicableVehicleTypes.Where(type=>type.IsDeleted == false))
                          .Include(s=>s.Service).Include(s=>s.Service.ServiceCategory).Include(s=>s.CreatedBy)
                          
                                    .ToListAsync();
            //.Include(s => s.Service.ServiceType).Include(s => s.Service.ServiceGroup).Include(s => s.Service.Target)
            //              .Include(s => s.Service.OperatingEntity)
        }

        public async Task<ServiceRegistration> FindServiceById(long Id)
        {
            return await _context.ServiceRegistrations.Where(s => s.IsDeleted == false)
                           .Include(s => s.ApplicableArmedEscortTypes.Where(type => type.IsDeleted == false))
                          .Include(s => s.ApplicableCommanderTypes.Where(type => type.IsDeleted == false))
                          .Include(s => s.ApplicablePilotTypes.Where(type => type.IsDeleted == false))
                          .Include(s => s.ApplicableVehicleTypes.Where(type => type.IsDeleted == false))
                          .Include(s => s.Service).Include(s => s.Service.ServiceCategory).Include(s => s.CreatedBy)
                       
                 .FirstOrDefaultAsync(ae => ae.Id == Id && ae.IsDeleted == false);
               //.Include(s => s.Service.ServiceType).Include(s => s.Service.ServiceGroup).Include(s => s.Service.Target)
               //           .Include(s => s.Service.OperatingEntity)
        }

        public ServiceRegistration GetserviceId(long serviceId)
        {
            return _context.ServiceRegistrations
                                       .Where(ct => ct.ServiceId == serviceId && ct.IsDeleted == false).FirstOrDefault();
        }

        public async Task<ServiceRegistration> SaveService(ServiceRegistration serviceRegistration)
        {
            var savedEntity = await _context.ServiceRegistrations.AddAsync(serviceRegistration);
            if (await SaveChanges())
            {
                return savedEntity.Entity;
            }
            return null;
        }

        public async Task<ArmedEscortType> UpdateArmedEscortTypes(ArmedEscortType armedEscortType)
        {
            var updatedEntity = _context.ArmedEscortTypes.Update(armedEscortType);
            if (await SaveChanges())
            {
                return updatedEntity.Entity;
            }
            return null;
        }

        public async Task<CommanderType> UpdateCommanderTypess(CommanderType commanderType)
        {
            var updatedEntity = _context.CommanderTypes.Update(commanderType);
            if (await SaveChanges())
            {
                return updatedEntity.Entity;
            }
            return null;
        }

        public async Task<PilotType> UpdatePilotTypes(PilotType pilotType)
        {
            var updatedEntity = _context.PilotTypes.Update(pilotType);
            if (await SaveChanges())
            {
                return updatedEntity.Entity;
            }
            return null;
        }

        public async Task<ServiceRegistration> UpdateServices(ServiceRegistration serviceRegistration)
        {
            var updatedEntity = _context.ServiceRegistrations.Update(serviceRegistration);
            if (await SaveChanges())
            {
                return updatedEntity.Entity;
            }
            return null;
        }

        public async Task<VehicleType> UpdateVehicleTypes(VehicleType vehicleType)
        {
            var updatedEntity = _context.VehicleTypes.Update(vehicleType);
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
