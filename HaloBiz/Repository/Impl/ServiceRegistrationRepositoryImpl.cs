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

        public async Task<bool> DeleteArmedEscortResource(ArmedEscortResourceRequiredPerService escort)
        {
            escort.IsDeleted = true;
            _context.ArmedEscortResourceRequiredPerServices.Update(escort);
            return await SaveChanges();
        }

        public async Task<bool> DeleteCommanderResource(CommanderResourceRequiredPerService commander)
        {
            commander.IsDeleted = true;
            _context.CommanderResourceRequiredPerServices.Update(commander);
            return await SaveChanges();
        }

        public async Task<bool> DeletePilotResource(PilotResourceRequiredPerService pilot)
        {
            pilot.IsDeleted = true;
            _context.PilotResourceRequiredPerService.Update(pilot);
            return await SaveChanges();
        }

      

        public async Task<bool> DeleteService(ServiceRegistration serviceRegistration)
        {
            serviceRegistration.IsDeleted = true;
            _context.ServiceRegistrations.Update(serviceRegistration);
            return await SaveChanges();
        }

        public async Task<bool> DeleteVehicleResource(VehicleResourceRequiredPerService vehicle)
        {
            vehicle.IsDeleted = true;
            _context.VehicleResourceRequiredPerServices.Update(vehicle);
            return await SaveChanges();
        }

        public async Task<IEnumerable<ArmedEscortResourceRequiredPerService>> FindAllArmedEscortResources()
        {

            return await _context.ArmedEscortResourceRequiredPerServices.Where(s => s.IsDeleted == false)
                          .Include(s => s.CreatedBy).Include(s=>s.ServiceRegistration).Include(s=>s.ServiceRegistration.Service)
                          .Include(s=>s.ArmedEscortType.ServiceRegistration).Include(s=>s.ServiceRegistration.ApplicableArmedEscortTypes)
                          .Include(s=>s.ArmedEscortType.ServiceRegistration.Service)
                                    .ToListAsync();
        }

        public async Task<IEnumerable<CommanderResourceRequiredPerService>> FindAllCommanderResources()
        {
            return await _context.CommanderResourceRequiredPerServices.Where(s => s.IsDeleted == false)
                         .Include(s => s.CreatedBy).Include(s => s.ServiceRegistration).Include(s => s.ServiceRegistration.Service)
                         .Include(s => s.CommanderType.ServiceRegistration).Include(s => s.ServiceRegistration.ApplicableArmedEscortTypes)
                         .Include(s => s.CommanderType.ServiceRegistration.Service)
                                   .ToListAsync();
        }

        public async Task<IEnumerable<PilotResourceRequiredPerService>> FindAllPilotResources()
        {
            return await _context.PilotResourceRequiredPerService.Where(s => s.IsDeleted == false)
                        .Include(s => s.CreatedBy).Include(s => s.ServiceRegistration).Include(s => s.ServiceRegistration.Service)
                        .Include(s => s.PilotType.ServiceRegistration).Include(s => s.ServiceRegistration.ApplicableArmedEscortTypes)
                        .Include(s => s.PilotType.ServiceRegistration.Service)
                                  .ToListAsync();
        }

        public async Task<IEnumerable<ServiceRegistration>> FindAllServicess()
        {
            //add service codes from appsettings
            return await _context.ServiceRegistrations.Where(s => s.IsDeleted == false )
                          .Include(s=>s.ApplicableArmedEscortTypes.Where(type=>type.IsDeleted == false))
                          .Include(s=>s.ApplicableCommanderTypes.Where(type=>type.IsDeleted == false))
                          .Include(s=>s.ApplicablePilotTypes.Where(type=>type.IsDeleted ==  false))
                          .Include(s=>s.ApplicableVehicleTypes.Where(type=>type.IsDeleted == false))
                          .Include(s=>s.Service).Include(s=>s.Service.ServiceCategory).Include(s=>s.CreatedBy)
                          
                                    .ToListAsync();
            //.Include(s => s.Service.ServiceType).Include(s => s.Service.ServiceGroup).Include(s => s.Service.Target)
            //              .Include(s => s.Service.OperatingEntity)
        }

        public async Task<IEnumerable<VehicleResourceRequiredPerService>> FindAllVehicleResources()
        {
            return await _context.VehicleResourceRequiredPerServices.Where(s => s.IsDeleted == false)
                        .Include(s => s.CreatedBy).Include(s => s.ServiceRegistration).Include(s => s.ServiceRegistration.Service)
                        .Include(s => s.VehicleType.ServiceRegistration).Include(s => s.ServiceRegistration.ApplicableArmedEscortTypes)
                        .Include(s => s.VehicleType.ServiceRegistration.Service)
                                  .ToListAsync();
        }

        public async Task<ArmedEscortResourceRequiredPerService> FindArmedEscortResourceById(long Id)
        {
            return await _context.ArmedEscortResourceRequiredPerServices.Where(s => s.IsDeleted == false)
                .Include(s => s.CreatedBy).Include(s => s.ServiceRegistration).Include(s => s.ServiceRegistration.Service)
                        .Include(s => s.ArmedEscortType.ServiceRegistration).Include(s => s.ServiceRegistration.ApplicableArmedEscortTypes)
                        .Include(s => s.ArmedEscortType.ServiceRegistration.Service)
                .FirstOrDefaultAsync(s => s.Id == Id && s.IsDeleted == false);
        }

        public async Task<ArmedEscortResourceRequiredPerService> FindArmedEscortResourceByServiceRegId(long seviceRegId)
        {
            return await _context.ArmedEscortResourceRequiredPerServices
               .Include(s => s.CreatedBy).Include(s => s.ServiceRegistration).Include(s => s.ServiceRegistration.Service)
                       .Include(s => s.ArmedEscortType.ServiceRegistration).Include(s => s.ServiceRegistration.ApplicableArmedEscortTypes.Where(x=>x.IsDeleted == false))
                       .Include(s => s.ArmedEscortType.ServiceRegistration.Service)
               .FirstOrDefaultAsync(s => s.ServiceRegistrationId == seviceRegId && s.IsDeleted == false);
        }

        public async Task<CommanderResourceRequiredPerService> FindCommanderResourceById(long Id)
        {
            return await _context.CommanderResourceRequiredPerServices.Where(s => s.IsDeleted == false)
               .Include(s => s.CreatedBy).Include(s => s.ServiceRegistration).Include(s => s.ServiceRegistration.Service)
                       .Include(s => s.CommanderType.ServiceRegistration).Include(s => s.ServiceRegistration.ApplicableArmedEscortTypes)
                       .Include(s => s.CommanderType.ServiceRegistration.Service)
               .FirstOrDefaultAsync(s => s.Id == Id && s.IsDeleted == false);
        }

        public async Task<CommanderResourceRequiredPerService> FindCommanderResourceByServiceRegId(long seviceRegId)
        {
            return await _context.CommanderResourceRequiredPerServices
                .Include(s => s.CreatedBy).Include(s => s.ServiceRegistration).Include(s => s.ServiceRegistration.Service)
                        .Include(s => s.CommanderType.ServiceRegistration).Include(s => s.ServiceRegistration.ApplicableArmedEscortTypes)
                        .Include(s => s.CommanderType.ServiceRegistration.Service)
                .FirstOrDefaultAsync(s => s.ServiceRegistrationId == seviceRegId && s.IsDeleted == false);
        }

        public async Task<PilotResourceRequiredPerService> FindPilotResourceById(long Id)
        {
            return await _context.PilotResourceRequiredPerService.Where(s => s.IsDeleted == false)
             .Include(s => s.CreatedBy).Include(s => s.ServiceRegistration).Include(s => s.ServiceRegistration.Service)
                     .Include(s => s.PilotType.ServiceRegistration).Include(s => s.ServiceRegistration.ApplicableArmedEscortTypes)
                     .Include(s => s.PilotType.ServiceRegistration.Service)
             .FirstOrDefaultAsync(s => s.Id == Id && s.IsDeleted == false);
        }

        public async Task<PilotResourceRequiredPerService> FindPilotResourceByServiceRegId(long seviceRegId)
        {
            return await _context.PilotResourceRequiredPerService
            .Include(s => s.CreatedBy).Include(s => s.ServiceRegistration).Include(s => s.ServiceRegistration.Service)
                    .Include(s => s.PilotType.ServiceRegistration).Include(s => s.ServiceRegistration.ApplicableArmedEscortTypes)
                    .Include(s => s.PilotType.ServiceRegistration.Service)
            .FirstOrDefaultAsync(s => s.ServiceRegistrationId  == seviceRegId && s.IsDeleted == false);
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

        public async Task<VehicleResourceRequiredPerService> FindVehicleResourceById(long Id)
        {
            return await _context.VehicleResourceRequiredPerServices.Where(s => s.IsDeleted == false)
             .Include(s => s.CreatedBy).Include(s => s.ServiceRegistration).Include(s => s.ServiceRegistration.Service)
                     .Include(s => s.VehicleType.ServiceRegistration).Include(s => s.ServiceRegistration.ApplicableArmedEscortTypes)
                     .Include(s => s.VehicleType.ServiceRegistration.Service)
             .FirstOrDefaultAsync(s => s.Id == Id && s.IsDeleted == false);
        }

        public async Task<VehicleResourceRequiredPerService> FindVehicleResourceByServiceRegId(long seviceRegId)
        {
            return await _context.VehicleResourceRequiredPerServices
            .Include(s => s.CreatedBy).Include(s => s.ServiceRegistration).Include(s => s.ServiceRegistration.Service)
                    .Include(s => s.VehicleType.ServiceRegistration).Include(s => s.ServiceRegistration.ApplicableArmedEscortTypes)
                    .Include(s => s.VehicleType.ServiceRegistration.Service)
            .FirstOrDefaultAsync(s => s.ServiceRegistrationId == seviceRegId && s.IsDeleted == false);
        }

        public ArmedEscortResourceRequiredPerService GetArmedEscortResourceApplicableTypeReqById(long serviceRegId, long? applicableTypeId)
        {
            return _context.ArmedEscortResourceRequiredPerServices.Where
                (ct => ct.ServiceRegistrationId == serviceRegId && ct.ArmedEscortTypeId == applicableTypeId  && ct.IsDeleted == false).FirstOrDefault();
        }

        public ArmedEscortResourceRequiredPerService GetArmedEscortTypeAndRegServiceId(long RegServiceId, long? ArmedEscortTypeId)
        {
            return _context.ArmedEscortResourceRequiredPerServices.Where
                (ct => ct.ServiceRegistrationId == RegServiceId && ct.ArmedEscortTypeId == ArmedEscortTypeId && ct.IsDeleted == false).FirstOrDefault();

        }

        public CommanderResourceRequiredPerService GetCommanderResourceApplicableTypeReqById(long serviceRegId, long? applicableTypeId)
        {
            return _context.CommanderResourceRequiredPerServices.Where
                (ct => ct.ServiceRegistrationId == serviceRegId && ct.CommanderTypeId == applicableTypeId && ct.IsDeleted == false).FirstOrDefault();
        }

        public CommanderResourceRequiredPerService GetCommanderTypeAndRegServiceId(long RegServiceId, long? CommanderTypeId)
        {
            return _context.CommanderResourceRequiredPerServices.Where
               (ct => ct.ServiceRegistrationId == RegServiceId && ct.CommanderTypeId == CommanderTypeId && ct.IsDeleted == false).FirstOrDefault();
        }

        public PilotResourceRequiredPerService GetPilotResourceApplicableTypeReqById(long serviceRegId, long? applicableTypeId)
        {
            return _context.PilotResourceRequiredPerService.Where
                (ct => ct.ServiceRegistrationId == serviceRegId && ct.PilotTypeId == applicableTypeId && ct.IsDeleted == false).FirstOrDefault();
        }

        public PilotResourceRequiredPerService GetPilotTypeAndRegServiceId(long RegServiceId, long? PilotTypeId)
        {
            return _context.PilotResourceRequiredPerService.Where
                (ct => ct.ServiceRegistrationId == RegServiceId && ct.PilotTypeId == PilotTypeId && ct.IsDeleted == false).FirstOrDefault();
        }

        public ServiceRegistration GetserviceId(long serviceId)
        {
            return _context.ServiceRegistrations
                                       .Where(ct => ct.ServiceId == serviceId && ct.IsDeleted == false).FirstOrDefault();
        }

        public VehicleResourceRequiredPerService GetVehicleResourceApplicableTypeReqById(long serviceRegId, long? applicableTypeId)
        {
            return _context.VehicleResourceRequiredPerServices.Where
                (ct => ct.ServiceRegistrationId == serviceRegId && ct.VehicleTypeId == applicableTypeId && ct.IsDeleted == false).FirstOrDefault();
        }

        public VehicleResourceRequiredPerService GetVehicleTypeAndRegServiceId(long RegServiceId, long? VehicleTypeId)
        {
            return _context.VehicleResourceRequiredPerServices.Where
              (ct => ct.ServiceRegistrationId == RegServiceId && ct.VehicleTypeId == VehicleTypeId && ct.IsDeleted == false).FirstOrDefault();
        }

        public async Task<ArmedEscortResourceRequiredPerService> SaveArmedEscortResource(ArmedEscortResourceRequiredPerService escort)
        {
            var savedEntity = await _context.ArmedEscortResourceRequiredPerServices.AddAsync(escort);
            if (await SaveChanges())
            {
                return savedEntity.Entity;
            }
            return null;
        }

        public async Task<CommanderResourceRequiredPerService> SaveCommanderResource(CommanderResourceRequiredPerService commander)
        {
            var savedEntity = await _context.CommanderResourceRequiredPerServices.AddAsync(commander);
            if (await SaveChanges())
            {
                return savedEntity.Entity;
            }
            return null;
        }

        public async Task<PilotResourceRequiredPerService> SavePilotResource(PilotResourceRequiredPerService pilot)
        {
            var savedEntity = await _context.PilotResourceRequiredPerService.AddAsync(pilot);
            if (await SaveChanges())
            {
                return savedEntity.Entity;
            }
            return null;
        }

        public async Task<VehicleResourceRequiredPerService> SaveResourceVehicle(VehicleResourceRequiredPerService vehicle)
        {
            var savedEntity = await _context.VehicleResourceRequiredPerServices.AddAsync(vehicle);
            if (await SaveChanges())
            {
                return savedEntity.Entity;
            }
            return null;
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
