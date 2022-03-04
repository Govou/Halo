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
    public class VehicleRegistrationRepositoryImpl:IVehicleRegistrationRepository
    {
        private readonly HalobizContext _context;
        private readonly ILogger<VehicleRegistrationRepositoryImpl> _logger;
        private readonly IServiceAssignmentDetailsRepository _serviceAssignmentDetailsRepository;
        private readonly IVehicleRegistrationRepository _vehiclesRepository;
        public VehicleRegistrationRepositoryImpl(HalobizContext context, ILogger<VehicleRegistrationRepositoryImpl> logger, IServiceAssignmentDetailsRepository serviceAssignmentDetailsRepository,
             IVehicleRegistrationRepository vehiclesRepository)
        {
            this._logger = logger;
            this._context = context;
            _serviceAssignmentDetailsRepository = serviceAssignmentDetailsRepository;
            _vehiclesRepository = vehiclesRepository;
        }

        public async Task<bool> DeleteVehicle(Vehicle vehicle)
        {
            vehicle.IsDeleted = true;
            _context.Vehicles.Update(vehicle);
            return await SaveChanges();
        }

        public async Task<bool> DeleteVehicleTie(VehicleSMORoutesResourceTie vehicleTie)
        {
            vehicleTie.IsDeleted = true;
            _context.VehicleSMORoutesResourceTies.Update(vehicleTie);
            return await SaveChanges();
        }

        public async Task<IEnumerable<Vehicle>> FindAllVehicles()
        {
            return await _context.Vehicles.Where(r => r.IsDeleted == false)
                .Include(s=>s.SupplierService).Include(t=>t.VehicleType)
                .Include(office=>office.AttachedOffice).Include(br=>br.AttachedBranch)
                                      .ToListAsync();
        }

        public async Task<IEnumerable<VehicleSMORoutesResourceTie>> FindAllVehicleTies()
        {
            return await _context.VehicleSMORoutesResourceTies.Where(r => r.IsDeleted == false)
                .Include(s => s.Resource).Include(t => t.SMORoute).Include(s => s.Resource.VehicleType)
                .Include(r => r.SMORegion).Include(cr => cr.CreatedBy).Include(r=>r.Resource.SupplierService)
                                      .ToListAsync();
        }

        public async Task<Vehicle> FindVehicleById(long Id) 
        {
            return await _context.Vehicles.Include(s => s.SupplierService).Include(t => t.VehicleType)
                .Include(office => office.AttachedOffice).Include(br => br.AttachedBranch)
                .FirstOrDefaultAsync(v => v.Id == Id && v.IsDeleted == false);
        }

        public Vehicle FindVehicleServiceById(long serviceId) 
        {
            return _context.Vehicles
                .Where(v => v.SupplierServiceId == serviceId && v.IsDeleted == false).FirstOrDefault();
        }

       

        public async Task<VehicleSMORoutesResourceTie> FindVehicleTieById(long Id)
        {
            return await _context.VehicleSMORoutesResourceTies.Include(s => s.Resource).Include(t => t.SMORoute)
                .Include(r => r.SMORegion).Include(cr => cr.CreatedBy).Include(s => s.Resource.VehicleType).Include(r => r.Resource.SupplierService)
                .FirstOrDefaultAsync(v => v.Id == Id && v.IsDeleted == false);
        }

        public async Task<IEnumerable<VehicleSMORoutesResourceTie>> FindVehicleTieByResourceId(long resourceId)
        {
            return await _context.VehicleSMORoutesResourceTies.Where(r => r.IsDeleted == false && r.ResourceId == resourceId)
                .Include(s => s.Resource).Include(t => t.SMORoute).Include(s => s.Resource.VehicleType)
                .Include(r => r.SMORegion).Include(cr => cr.CreatedBy).Include(r => r.Resource.SupplierService)
                                      .ToListAsync();
        }

        //public IEnumerable<VehicleSMORoutesResourceTie> GetAllVehiclesOnRouteByResourceAndRouteId(long? regRessourceId, long? RouteId)
        //{
        //    return _context.VehicleSMORoutesResourceTies.Where
        //                  (ct => ct.ResourceId == regRessourceId && ct.SMORouteId == RouteId && ct.IsDeleted == false).ToList();
        //} 
        public IEnumerable<VehicleSMORoutesResourceTie> GetAllVehiclesOnRouteByResourceAndRouteId( long? RouteId)
        {
            var services = new List<VehicleSMORoutesResourceTie>();
            var query = _context.VehicleSMORoutesResourceTies.Where
                          (ct =>  ct.SMORouteId == RouteId && ct.IsDeleted == false);
            //var getVehicleDetailNoneHeld =  _serviceAssignmentDetailsRepository.FindAllNoneHeldVehicleServiceAssignmentDetails2();
            var getResources = _context.Vehicles.Where(r => r.IsDeleted == false)
              .Include(s => s.SupplierService).Include(t => t.VehicleType)
              .Include(office => office.AttachedOffice).Include(br => br.AttachedBranch)
                                    .ToList();

            foreach (var items in getResources)
            {
                //quuery.Where(x => x.ServiceCode.Contains(items));
                services.AddRange(query.Where(x => x.ResourceId == items.Id));
            }
            return services.ToList();


        }

        //public async Task<VehicleSMORoutesResourceTie> FindVehicleTieByResourceId(long resourceId)
        //{
        //    return await _context.VehicleSMORoutesResourceTies.Include(s => s.Resource).Include(t => t.SMORoute)
        //        .Include(r => r.SMORegion).Include(cr => cr.CreatedBy).Include(s => s.Resource.VehicleType).Include(r => r.Resource.SupplierService)
        //        .FirstOrDefaultAsync(v => v.ResourceId == resourceId && v.IsDeleted == false);
        //}

        public VehicleSMORoutesResourceTie GetResourceRegIdRegionAndRouteId(long regRessourceId, long? RouteId)
        {
            return _context.VehicleSMORoutesResourceTies.Where
               (ct => ct.ResourceId == regRessourceId && ct.SMORouteId == RouteId  && ct.IsDeleted == false).FirstOrDefault();
        }

        public  VehicleSMORoutesResourceTie GetResourceRegIdRegionAndRouteId2(long? regRessourceId, long? RouteId)
        {
            return _context.VehicleSMORoutesResourceTies.Where
                          (ct => ct.ResourceId == regRessourceId && ct.SMORouteId == RouteId && ct.IsDeleted == false).FirstOrDefault();
        }

        public async Task<Vehicle> SaveVehicle(Vehicle vehicle)
        {
            var savedEntity = await _context.Vehicles.AddAsync(vehicle);
            if (await SaveChanges())
            {
                return savedEntity.Entity;
            }
            return null;
        }

        public async Task<VehicleSMORoutesResourceTie> SaveVehicleTie(VehicleSMORoutesResourceTie vehicleTie)
        {
            var savedEntity = await _context.VehicleSMORoutesResourceTies.AddAsync(vehicleTie);
            if (await SaveChanges())
            {
                return savedEntity.Entity;
            }
            return null;
        }

        public async Task<Vehicle> UpdateVehicle(Vehicle vehicle)
        {
            var updatedEntity = _context.Vehicles.Update(vehicle);
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
