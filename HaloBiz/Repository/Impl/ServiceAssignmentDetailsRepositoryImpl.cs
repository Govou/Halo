using HaloBiz.Helpers;
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
    public class ServiceAssignmentDetailsRepositoryImpl: IServiceAssignmentDetailsRepository
    {
        private readonly HalobizContext _context;
        private readonly ILogger<ServiceAssignmentDetailsRepositoryImpl> _logger;
        public ServiceAssignmentDetailsRepositoryImpl(HalobizContext context, ILogger<ServiceAssignmentDetailsRepositoryImpl> logger)
        {
            this._logger = logger;
            this._context = context;
        }

        public async Task<bool> DeleteCommanderServiceAssignmentDetail(CommanderServiceAssignmentDetail serviceAssignmentDetail)
        {
            //serviceAssignmentDetail.recoverydatetime = 
            serviceAssignmentDetail.IsHeldForAction = false;
            serviceAssignmentDetail.IsTemporarilyHeld = false;
            serviceAssignmentDetail.DateActionReleased = DateTime.Now;
            serviceAssignmentDetail.DateTemporarilyReleased = DateTime.Now;
            _context.CommanderServiceAssignmentDetails.Update(serviceAssignmentDetail);
            return await SaveChanges();
        }

        public async Task<bool> DeleteCommanderServiceAssignmentDetailById(CommanderServiceAssignmentDetail serviceAssignmentDetail)
        {
            serviceAssignmentDetail.IsDeleted = true;
            _context.CommanderServiceAssignmentDetails.Update(serviceAssignmentDetail);
            return await SaveChanges();
        }

        public async Task<bool> DeleteEscortServiceAssignmentDetail(ArmedEscortServiceAssignmentDetail serviceAssignmentDetail)
        {
            serviceAssignmentDetail.IsHeldForAction  = false;
            serviceAssignmentDetail.IsTemporarilyHeld  = false;
            serviceAssignmentDetail.DateActionReleased = DateTime.UtcNow ;
            serviceAssignmentDetail.DateTemporarilyReleased = DateTime.UtcNow ;

            _context.ArmedEscortServiceAssignmentDetails.Update(serviceAssignmentDetail);
            return await SaveChanges();
        }

        public async Task<bool> DeleteEscortServiceAssignmentDetailById(ArmedEscortServiceAssignmentDetail serviceAssignmentDetail)
        {
            serviceAssignmentDetail.IsDeleted = true;
            _context.ArmedEscortServiceAssignmentDetails.Update(serviceAssignmentDetail);
            return await SaveChanges();
        }

        public async Task<bool> DeletePassenger(Passenger passenger)
        {
            passenger.IsDeleted = true;
            _context.Passengers.Update(passenger);
            return await SaveChanges();
        }

        public async Task<bool> DeletePassengerByAssignmentId(Passenger passenger)
        {
            passenger.IsDeleted = true;
            _context.Passengers.Update(passenger);
            return await SaveChanges();
        }

        public async Task<bool> DeletePilotServiceAssignmentDetail(PilotServiceAssignmentDetail serviceAssignmentDetail)
        {
            serviceAssignmentDetail.IsHeldForAction = false;
            serviceAssignmentDetail.IsTemporarilyHeld = false;
            serviceAssignmentDetail.DateActionReleased = DateTime.UtcNow;
            serviceAssignmentDetail.DateTemporarilyReleased = DateTime.UtcNow;
            _context.PilotServiceAssignmentDetails.Update(serviceAssignmentDetail);
            return await SaveChanges();
        }

        public async Task<bool> DeletePilotServiceAssignmentDetailById(PilotServiceAssignmentDetail serviceAssignmentDetail)
        {
            serviceAssignmentDetail.IsDeleted = true;
            _context.PilotServiceAssignmentDetails.Update(serviceAssignmentDetail);
            return await SaveChanges();
        }

        public async Task<bool> DeleteVehicleServiceAssignmentDetail(VehicleServiceAssignmentDetail serviceAssignmentDetail)
        {
            serviceAssignmentDetail.IsHeldForAction = false;
            serviceAssignmentDetail.IsTemporarilyHeld = false;
            serviceAssignmentDetail.DateActionReleased = DateTime.UtcNow;
            serviceAssignmentDetail.DateTemporarilyReleased = DateTime.UtcNow;
            _context.VehicleServiceAssignmentDetails.Update(serviceAssignmentDetail);
            return await SaveChanges();
        }

        public async Task<bool> DeleteVehicleServiceAssignmentDetailById(VehicleServiceAssignmentDetail serviceAssignmentDetail)
        {
            serviceAssignmentDetail.IsDeleted = true;
            _context.VehicleServiceAssignmentDetails.Update(serviceAssignmentDetail);
            return await SaveChanges();
        }

        public async Task<IEnumerable<CommanderServiceAssignmentDetail>> FindAllCommanderServiceAssignmentDetails()
        {
            return await _context.CommanderServiceAssignmentDetails.Where(type => type.IsDeleted == false)
               .Include(ct => ct.CommanderResource).Include(t => t.CommanderResource.Profile).Include(t => t.TiedVehicleResource).Include(t => t.ServiceAssignment)
               .Include(t => t.TempReleaseType).Include(t => t.CreatedBy).Include(t=>t.ActionReleaseType).Include(t => t.CommanderResource.CommanderType)
               .OrderByDescending(x => x.Id)
               .ToListAsync();
        }

        public async Task<IEnumerable<CommanderServiceAssignmentDetail>> FindAllCommanderServiceAssignmentDetailsByAssignmentId(long assignmentId)
        {
            return await _context.CommanderServiceAssignmentDetails.Where(type => type.IsDeleted == false  && type.ServiceAssignmentId == assignmentId)
               .Include(ct => ct.CommanderResource).ThenInclude(t => t.Profile).Include(t => t.TiedVehicleResource).Include(t => t.ServiceAssignment)
               .Include(ct => ct.ServiceAssignment.ServiceRegistration).Include(ct => ct.ServiceAssignment.ServiceRegistration.Service)
              .Include(t => t.CommanderResource.CommanderType)
               .OrderByDescending(x => x.Id)
               .ToListAsync();
        }

        public async Task<IEnumerable<CommanderServiceAssignmentDetail>> FindAllCommanderServiceAssignmentDetailsByProfileId(long profileId)
        {
            var query =  _context.CommanderServiceAssignmentDetails.Where(type => type.IsDeleted == false)
             .Include(ct => ct.CommanderResource).Include(t => t.CommanderResource.Profile).Include(t => t.TiedVehicleResource).Include(t => t.ServiceAssignment)
             .Include(ct=>ct.ServiceAssignment.ServiceRegistration).Include(ct => ct.ServiceAssignment.ServiceRegistration.Service)
             .Include(t => t.TempReleaseType).Include(t => t.CreatedBy).Include(t => t.ActionReleaseType).Include(t => t.CommanderResource.CommanderType);

            return await query.Where(type => type.CommanderResource.ProfileId == profileId && type.ServiceAssignment.AssignmentStatus == "Open" && type.ServiceAssignment.ReadyStatus == true).OrderByDescending(x => x.Id).ToListAsync();
             //   _context.CommanderServiceAssignmentDetails.Where(type => type.IsDeleted == false)
             //.Include(ct => ct.CommanderResource).Include(t => t.CommanderResource.Profile).Include(t => t.TiedVehicleResource).Include(t => t.ServiceAssignment)
             //.Include(t => t.TempReleaseType).Include(t => t.CreatedBy).Include(t => t.ActionReleaseType).Include(t => t.CommanderResource.CommanderType).Where(type => type.CommanderResource.ProfileId == profileId && type.ServiceAssignment.AssignmentStatus == "Open" && type.ServiceAssignment.ReadyStatus == true)
             //.OrderByDescending(x => x.Id)
             //.ToListAsync();
        }

        public async Task<IEnumerable<Contract>> FindAllContracts()
        {
            return await _context.Contracts
                .Include(x => x.ContractServices)
               
               .Where(entity => entity.IsDeleted == false)
               .OrderByDescending(entity => entity.Id)
               .ToListAsync();
        }

        public async Task<IEnumerable<ArmedEscortServiceAssignmentDetail>> FindAllEscortServiceAssignmentDetails()
        {
            return await _context.ArmedEscortServiceAssignmentDetails.Where(type => type.IsDeleted == false)
              .Include(ct => ct.ArmedEscortResource).Include(t => t.ActionReleaseType).Include(t => t.TempReleaseType).Include(t => t.ServiceAssignment)
              .Include(t => t.TempReleaseType).Include(t => t.CreatedBy).Include(t => t.ArmedEscortResource.ArmedEscortType)
              .OrderByDescending(x => x.Id)
              .ToListAsync();
        }

        public async Task<IEnumerable<ArmedEscortServiceAssignmentDetail>> FindAllEscortServiceAssignmentDetailsByAssignmentId(long assignmentId)
        {
            return await _context.ArmedEscortServiceAssignmentDetails.Where(type => type.IsDeleted == false  && type.ServiceAssignmentId == assignmentId)
              .Include(ct => ct.ArmedEscortResource).Include(t => t.ActionReleaseType).Include(t => t.TempReleaseType).Include(t => t.ServiceAssignment)
              .Include(t => t.TempReleaseType).Include(t => t.CreatedBy).Include(t => t.ArmedEscortResource.ArmedEscortType)
              .OrderByDescending(x => x.Id)
              .ToListAsync();
        }

        public async Task<IEnumerable<CommanderServiceAssignmentDetail>> FindAllNoneHeldCommanderServiceAssignmentDetails()
        {
            return await _context.CommanderServiceAssignmentDetails.Where(type => type.IsDeleted == false && (type.IsTemporarilyHeld == false || type.IsHeldForAction == false))
              .Include(ct => ct.CommanderResource).Include(t => t.CommanderResource.Profile).Include(t => t.TiedVehicleResource).Include(t => t.ServiceAssignment)
              .Include(t => t.TempReleaseType).Include(t => t.CreatedBy).Include(t => t.ActionReleaseType).Include(t => t.CommanderResource.CommanderType)
              .OrderByDescending(x => x.CreatedAt)
              .ToListAsync();
        }

        public async Task<IEnumerable<ArmedEscortServiceAssignmentDetail>> FindAllNoneHeldEscortServiceAssignmentDetails()
        {
            return await _context.ArmedEscortServiceAssignmentDetails.Where(type => type.IsDeleted == false && (type.IsTemporarilyHeld == false || type.IsHeldForAction == false))
              .Include(ct => ct.ArmedEscortResource).Include(t => t.ActionReleaseType).Include(t => t.TempReleaseType).Include(t => t.ServiceAssignment)
              .Include(t => t.TempReleaseType).Include(t => t.CreatedBy).Include(t => t.ArmedEscortResource.ArmedEscortType)
              .OrderByDescending(x => x.CreatedAt)
              .ToListAsync();
        }

        public async Task<IEnumerable<PilotServiceAssignmentDetail>> FindAllNoneHeldPilotServiceAssignmentDetails()
        {
            return await _context.PilotServiceAssignmentDetails.Where(type => type.IsDeleted == false && (type.IsTemporarilyHeld == false || type.IsHeldForAction == false))
              .Include(ct => ct.PilotResource).Include(t => t.ActionReleaseType).Include(t => t.TempReleaseType).Include(t => t.ServiceAssignment)
              .Include(t => t.TempReleaseType).Include(t => t.CreatedBy).Include(t => t.PilotResource.PilotType)
              .OrderByDescending(x => x.Id)
              .ToListAsync();
        }

        public async Task<IEnumerable<VehicleServiceAssignmentDetail>> FindAllNoneHeldVehicleServiceAssignmentDetails()
        {
            return await _context.VehicleServiceAssignmentDetails.Where(type => type.IsDeleted == false && type.IsTemporarilyHeld == false )
             .Include(ct => ct.VehicleResource).Include(t => t.ActionReleaseType).Include(t => t.TempReleaseType).Include(t => t.ServiceAssignment)
             .Include(t => t.TempReleaseType).Include(t => t.CreatedBy).Include(t => t.VehicleResource.SupplierService).Include(t => t.VehicleResource.VehicleType)
             .OrderByDescending(x => x.CreatedAt)
             .ToListAsync();
        }

        public List<VehicleServiceAssignmentDetail> FindAllNoneHeldVehicleServiceAssignmentDetails2()
        {
            return  _context.VehicleServiceAssignmentDetails.Where(type => type.IsDeleted == false && type.IsTemporarilyHeld == false)
             .Include(ct => ct.VehicleResource).Include(t => t.ActionReleaseType).Include(t => t.TempReleaseType).Include(t => t.ServiceAssignment)
             .Include(t => t.TempReleaseType).Include(t => t.CreatedBy).Include(t => t.VehicleResource.SupplierService).Include(t => t.VehicleResource.VehicleType)
             .OrderByDescending(x => x.CreatedAt)
             .ToList();
        }

        public async Task<IEnumerable<Passenger>> FindAllPassengers()
        {
            return await _context.Passengers.Where(type => type.IsDeleted == false)
             .Include(ct => ct.PassengerType).Include(t => t.ServiceAssignment)
            
             .OrderByDescending(x => x.Id)
             .ToListAsync();
        }

        public async Task<IEnumerable<Passenger>> FindAllPassengersByAssignmentId(long assignmentId)
        {
            return await _context.Passengers.Where(type => type.IsDeleted == false && type.ServiceAssignmentId == assignmentId)
            .Include(ct => ct.PassengerType).Include(t => t.ServiceAssignment)
            .OrderByDescending(x => x.Id)
            .ToListAsync();
        }

        public async Task<IEnumerable<PilotServiceAssignmentDetail>> FindAllPilotServiceAssignmentDetails()
        {
            return await _context.PilotServiceAssignmentDetails.Where(type => type.IsDeleted == false)
              .Include(ct => ct.PilotResource).Include(t => t.ActionReleaseType).Include(t => t.TempReleaseType).Include(t => t.ServiceAssignment)
              .Include(t => t.TempReleaseType).Include(t => t.CreatedBy).Include(t=>t.PilotResource.PilotType)
              .OrderByDescending(x => x.Id)
              .ToListAsync();
        }

        public async Task<IEnumerable<PilotServiceAssignmentDetail>> FindAllPilotServiceAssignmentDetailsByAssignmentId(long assignmentId)
        {
            return await _context.PilotServiceAssignmentDetails.Where(type => type.IsDeleted == false  && type.ServiceAssignmentId == assignmentId)
              .Include(ct => ct.PilotResource).Include(t => t.ActionReleaseType).Include(t => t.TempReleaseType).Include(t => t.ServiceAssignment)
              .Include(t => t.TempReleaseType).Include(t => t.CreatedBy).Include(t => t.PilotResource.PilotType)
              .OrderByDescending(x => x.Id)
              .ToListAsync();
        }

        public async Task<IEnumerable<CommanderProfile>> FindAllUniqueAvailableCommanderServiceAssignmentDetails()
        {
            var query = _context.CommanderServiceAssignmentDetails.Where(type => type.IsDeleted == false && type.IsTemporarilyHeld == true)
              //.Include(ct => ct.CommanderResource).Include(t => t.CommanderResource.Profile).Include(t => t.TiedVehicleResource).Include(t => t.ServiceAssignment)
              //.Include(t => t.TempReleaseType).Include(t => t.CreatedBy).Include(t => t.ActionReleaseType).Include(t => t.CommanderResource.CommanderType)
              .OrderByDescending(x => x.Id).DistinctBy(y => y.CommanderResourceId).ToList();
            var resourceList = _context.CommanderProfiles.Where(cm => cm.IsDeleted == false).Include(x=>x.Profile).Include(x=>x.CommanderType).ToList();
            var newList = new List<CommanderProfile>();
            newList = resourceList.Where(x => !query.Any(y => y.CommanderResourceId == x.Id)).ToList();
            return  newList.ToList();
            
        }

        public async Task<IEnumerable<ArmedEscortProfile>> FindAllUniqueAvailableEscortServiceAssignmentDetails()
        {
            var query =   _context.ArmedEscortServiceAssignmentDetails.Where(type => type.IsDeleted == false && type.IsTemporarilyHeld == true)
             //.Include(ct => ct.ArmedEscortResource).Include(t => t.ActionReleaseType).Include(t => t.TempReleaseType).Include(t => t.ServiceAssignment)
             //.Include(t => t.TempReleaseType).Include(t => t.CreatedBy).Include(t => t.ArmedEscortResource.ArmedEscortType)
             .OrderByDescending(x => x.Id).DistinctBy(c=>c.ArmedEscortResourceId)
             .ToList();
            var resourceList = _context.ArmedEscortProfiles.Where(cm => cm.IsDeleted == false).Include(x=>x.ServiceAssignment).Include(x=>x.ArmedEscortType).ToList();
            var newList = new List<ArmedEscortProfile>();
            newList = resourceList.Where(x => !query.Any(y => y.ArmedEscortResourceId == x.Id)).ToList();
            return newList.ToList();
        }

        public async Task<IEnumerable<PilotProfile>> FindAllUniqueAvailablePilotServiceAssignmentDetails()
        {
            var query =  _context.PilotServiceAssignmentDetails.Where(type => type.IsDeleted == false && type.IsTemporarilyHeld == true)
             //.Include(ct => ct.PilotResource).Include(t => t.ActionReleaseType).Include(t => t.TempReleaseType).Include(t => t.ServiceAssignment)
             //.Include(t => t.TempReleaseType).Include(t => t.CreatedBy).Include(t => t.PilotResource.PilotType)
             .OrderByDescending(x => x.Id).DistinctBy(c=>c.PilotResourceId).ToList();

            var resourceList = _context.PilotProfiles.Where(cm => cm.IsDeleted == false).Include(x=>x.PilotType).ToList();
            var newList = new List<PilotProfile>();
            newList = resourceList.Where(x => !query.Any(y => y.PilotResourceId == x.Id)).ToList();
            return newList.ToList();
        }

        public async Task<IEnumerable<Vehicle>> FindAllUniqueAvailableVehicleServiceAssignmentDetails()
        {
            var query =  _context.VehicleServiceAssignmentDetails.Where(type => type.IsDeleted == false && type.IsTemporarilyHeld == true)
           //.Include(ct => ct.VehicleResource).Include(t => t.ActionReleaseType).Include(t => t.TempReleaseType).Include(t => t.ServiceAssignment)
           //.Include(t => t.TempReleaseType).Include(t => t.CreatedBy).Include(t => t.VehicleResource.SupplierService).Include(t => t.VehicleResource.VehicleType)
           .OrderByDescending(x => x.Id).DistinctBy(c=>c.VehicleResourceId).ToList();

            var resourceList = _context.Vehicles.Where(cm => cm.IsDeleted == false).Include(x=>x.SupplierService).Include(x=>x.VehicleType).ToList();
            var newList = new List<Vehicle>();
            newList = resourceList.Where(x => !query.Any(y => y.VehicleResourceId == x.Id)).ToList();
            return newList.ToList();
        }

        public async Task<IEnumerable<CommanderServiceAssignmentDetail>> FindAllUniqueHeldCommanderServiceAssignmentDetails()
        {
            var query = _context.CommanderServiceAssignmentDetails.Where(type => type.IsDeleted == false && type.IsTemporarilyHeld == true)
             .Include(ct => ct.CommanderResource).ThenInclude(t => t.Profile).Include(t => t.ServiceAssignment)
             .Include(t => t.CommanderResource.CommanderType)
             .OrderByDescending(x => x.Id).DistinctBy(y => y.CommanderResourceId);

            return query.ToList();
        }

        public async Task<IEnumerable<ArmedEscortServiceAssignmentDetail>> FindAllUniqueHeldEscortServiceAssignmentDetails()
        {
            var query = _context.ArmedEscortServiceAssignmentDetails.Where(type => type.IsDeleted == false && type.IsTemporarilyHeld == true)
            .Include(ct => ct.ArmedEscortResource).Include(t => t.ServiceAssignment).Include(t => t.ArmedEscortResource.ArmedEscortType)
            .OrderByDescending(x => x.Id).DistinctBy(c => c.ArmedEscortResourceId);

            return query.ToList();
        }

        public async Task<IEnumerable<PilotServiceAssignmentDetail>> FindAllUniqueHeldPilotServiceAssignmentDetails()
        {
            var query = _context.PilotServiceAssignmentDetails.Where(type => type.IsDeleted == false && type.IsTemporarilyHeld == true)
            .Include(ct => ct.PilotResource).Include(t => t.ServiceAssignment).Include(t => t.PilotResource.PilotType)
            .OrderByDescending(x => x.Id).DistinctBy(c => c.PilotResourceId);
            return query.ToList();
        }

        public async Task<IEnumerable<VehicleServiceAssignmentDetail>> FindAllUniqueHeldVehicleServiceAssignmentDetails()
        {
            var query = _context.VehicleServiceAssignmentDetails.Where(type => type.IsDeleted == false && type.IsTemporarilyHeld == true)
           .Include(ct => ct.VehicleResource).Include(t => t.ServiceAssignment)
          .Include(t => t.VehicleResource.SupplierService).Include(t => t.VehicleResource.VehicleType)
           .OrderByDescending(x => x.Id).DistinctBy(c => c.VehicleResourceId);
            return query.ToList();
        }

        public async Task<IEnumerable<VehicleServiceAssignmentDetail>> FindAllVehicleServiceAssignmentDetails()
        {
            return await _context.VehicleServiceAssignmentDetails.Where(type => type.IsDeleted == false)
             .Include(ct => ct.VehicleResource).Include(t => t.ActionReleaseType).Include(t => t.TempReleaseType).Include(t => t.ServiceAssignment)
             .Include(t => t.TempReleaseType).Include(t => t.CreatedBy).Include(t=>t.VehicleResource.SupplierService).Include(t=>t.VehicleResource.VehicleType)
             .OrderByDescending(x => x.Id)
             .ToListAsync();
        }

        public async Task<IEnumerable<VehicleServiceAssignmentDetail>> FindAllVehicleServiceAssignmentDetailsByAssignmentId(long assignmentId)
        {
            //added extra IsTemporarilyHeld == true bcz of resource replacement, might have the same assignId && type.IsTemporarilyHeld == true
            return await _context.VehicleServiceAssignmentDetails.Where(type => type.IsDeleted == false  && type.ServiceAssignmentId == assignmentId )
            .Include(ct => ct.VehicleResource).Include(t => t.TempReleaseType).Include(t => t.ServiceAssignment)
            .Include(t => t.VehicleResource.SupplierService).Include(t => t.VehicleResource.VehicleType)
            .OrderByDescending(x => x.Id)
            .ToListAsync();
        }

        public async Task<CommanderServiceAssignmentDetail> FindCommanderServiceAssignmentDetailByAssignmentId(long Id)
        {
            return await _context.CommanderServiceAssignmentDetails
              .Include(ct => ct.CommanderResource).Include(t => t.CommanderResource.Profile).Include(t => t.TiedVehicleResource).Include(t => t.ServiceAssignment)
              .Include(t => t.TempReleaseType).Include(t => t.CreatedBy).Include(t => t.ActionReleaseType).Include(t => t.CommanderResource.CommanderType)
              .FirstOrDefaultAsync(aer => aer.ServiceAssignmentId == Id && aer.IsDeleted == false);
        }

        public async Task<CommanderServiceAssignmentDetail> FindCommanderServiceAssignmentDetailById(long Id)
        {
            return await _context.CommanderServiceAssignmentDetails
               .Include(ct => ct.CommanderResource).Include(t => t.CommanderResource.Profile).Include(t => t.TiedVehicleResource).Include(t => t.ServiceAssignment)
               .Include(t => t.TempReleaseType).Include(t => t.CreatedBy).Include(t => t.ActionReleaseType).Include(t => t.CommanderResource.CommanderType)
               .FirstOrDefaultAsync(aer => aer.Id == Id && aer.IsDeleted == false);
        }

        public async Task<CommanderServiceAssignmentDetail> FindCommanderServiceAssignmentDetailByResourceId(long resourceId)
        {
            return await _context.CommanderServiceAssignmentDetails
             .Include(ct => ct.CommanderResource).Include(t => t.CommanderResource.Profile).Include(t => t.TiedVehicleResource).Include(t => t.ServiceAssignment)
             .Include(t => t.TempReleaseType).Include(t => t.CreatedBy).Include(t => t.ActionReleaseType).Include(t => t.CommanderResource.CommanderType)
             .OrderByDescending(x => x.Id)
             .FirstOrDefaultAsync(aer => aer.CommanderResourceId == resourceId && aer.IsDeleted == false);
        }

        public async Task<CommanderServiceAssignmentDetail> FindCommanderServiceAssignmentDetailByResourceId2(long? resourceId)
        {
            return await _context.CommanderServiceAssignmentDetails
            .Include(ct => ct.CommanderResource).Include(t => t.CommanderResource.Profile).Include(t => t.TiedVehicleResource).Include(t => t.ServiceAssignment)
            .Include(t => t.TempReleaseType).Include(t => t.CreatedBy).Include(t => t.ActionReleaseType).Include(t => t.CommanderResource.CommanderType)
            .FirstOrDefaultAsync(aer => aer.CommanderResourceId == resourceId && aer.IsDeleted == false && aer.IsTemporarilyHeld == true);
        }

        public async Task<CommanderServiceAssignmentDetail> FindCommanderServiceAssignmentDetailByResourceIdAndAssignmentId(long resourceId, long assId)
        {
            return await _context.CommanderServiceAssignmentDetails
             .Include(ct => ct.CommanderResource).Include(t => t.CommanderResource.Profile).Include(t => t.TiedVehicleResource).Include(t => t.ServiceAssignment)
             .Include(t => t.TempReleaseType).Include(t => t.CreatedBy).Include(t => t.ActionReleaseType).Include(t => t.CommanderResource.CommanderType)
             .FirstOrDefaultAsync(aer => aer.CommanderResourceId == resourceId && aer.ServiceAssignmentId == assId && aer.IsDeleted == false);
        }

        public async Task<IEnumerable<CommanderServiceAssignmentDetail>> FindCommanderServiceAssignmentDetailByTiedVehicleResourceIdAndAssignmentId(long? tiedResourceId, long assId)
        {
            return await _context.CommanderServiceAssignmentDetails.Where(aer => aer.TiedVehicleResourceId == tiedResourceId && aer.ServiceAssignmentId == assId && aer.IsDeleted == false)
             .Include(ct => ct.CommanderResource).Include(t => t.CommanderResource.Profile).Include(t => t.TiedVehicleResource).Include(t => t.ServiceAssignment)
             .Include(t => t.TempReleaseType).Include(t => t.CreatedBy).Include(t => t.ActionReleaseType).Include(t => t.CommanderResource.CommanderType)
             .ToListAsync();
        }

        public async Task<ArmedEscortServiceAssignmentDetail> FindEscortServiceAssignmentDetailByAssignmentId(long Id)
        {
            return await _context.ArmedEscortServiceAssignmentDetails
               .Include(ct => ct.ArmedEscortResource).Include(t => t.ActionReleaseType).Include(t => t.TempReleaseType).Include(t => t.ServiceAssignment)
             .Include(t => t.TempReleaseType).Include(t => t.CreatedBy).Include(t => t.ArmedEscortResource.ArmedEscortType)
               .FirstOrDefaultAsync(aer => aer.ServiceAssignmentId == Id && aer.IsDeleted == false);
        }

        public async Task<ArmedEscortServiceAssignmentDetail> FindEscortServiceAssignmentDetailById(long Id)
        {
            return await _context.ArmedEscortServiceAssignmentDetails
                .Include(ct => ct.ArmedEscortResource).Include(t => t.ActionReleaseType).Include(t => t.TempReleaseType).Include(t => t.ServiceAssignment)
              .Include(t => t.TempReleaseType).Include(t => t.CreatedBy).Include(t => t.ArmedEscortResource.ArmedEscortType)
                .FirstOrDefaultAsync(aer => aer.Id == Id && aer.IsDeleted == false);
        }

        public async Task<ArmedEscortServiceAssignmentDetail> FindEscortServiceAssignmentDetailByResourceId(long resourceId)
        {
            return await _context.ArmedEscortServiceAssignmentDetails
               .Include(ct => ct.ArmedEscortResource).Include(t => t.ActionReleaseType).Include(t => t.TempReleaseType).Include(t => t.ServiceAssignment)
             .Include(t => t.TempReleaseType).Include(t => t.CreatedBy).Include(t => t.ArmedEscortResource.ArmedEscortType).OrderByDescending(x=>x.Id)
               .FirstOrDefaultAsync(aer => aer.ArmedEscortResourceId == resourceId && aer.IsDeleted == false);
        }

        public async Task<ArmedEscortServiceAssignmentDetail> FindEscortServiceAssignmentDetailByResourceId2(long? resourceId)
        {
            return await _context.ArmedEscortServiceAssignmentDetails
             .Include(ct => ct.ArmedEscortResource).Include(t => t.ActionReleaseType).Include(t => t.TempReleaseType).Include(t => t.ServiceAssignment)
           .Include(t => t.TempReleaseType).Include(t => t.CreatedBy).Include(t => t.ArmedEscortResource.ArmedEscortType)
             .FirstOrDefaultAsync(aer => aer.ArmedEscortResourceId == resourceId && aer.IsDeleted == false && aer.IsTemporarilyHeld == true);
        }

        public async Task<ArmedEscortServiceAssignmentDetail> FindEscortServiceAssignmentDetailByResourceIdAndAssignmentId(long resourceId, long assId)
        {
            return await _context.ArmedEscortServiceAssignmentDetails
              .Include(ct => ct.ArmedEscortResource).Include(t => t.ActionReleaseType).Include(t => t.TempReleaseType).Include(t => t.ServiceAssignment)
            .Include(t => t.TempReleaseType).Include(t => t.CreatedBy).Include(t => t.ArmedEscortResource.ArmedEscortType)
              .FirstOrDefaultAsync(aer => aer.ArmedEscortResourceId == resourceId && aer.ServiceAssignmentId == assId && aer.IsDeleted == false);
        }

        public async Task<Passenger> FindPassengerByAssignmentId(long Id)
        {
            return await _context.Passengers
             .Include(ct => ct.ServiceAssignment).Include(t => t.PassengerType)
             .FirstOrDefaultAsync(aer => aer.ServiceAssignmentId == Id && aer.IsDeleted == false);
        }

        public async Task<Passenger> FindPassengerById(long Id)
        {
            return await _context.Passengers
              .Include(ct => ct.ServiceAssignment).Include(t => t.PassengerType)
              .FirstOrDefaultAsync(aer => aer.Id == Id && aer.IsDeleted == false);
        }

        public async Task<PilotServiceAssignmentDetail> FindPilotServiceAssignmentDetailByAssignmentId(long Id)
        {
            return await _context.PilotServiceAssignmentDetails
              .Include(ct => ct.PilotResource).Include(t => t.ActionReleaseType).Include(t => t.TempReleaseType).Include(t => t.ServiceAssignment)
            .Include(t => t.TempReleaseType).Include(t => t.CreatedBy).Include(t => t.PilotResource.PilotType)
              .FirstOrDefaultAsync(aer => aer.ServiceAssignmentId == Id && aer.IsDeleted == false);
        }

        public async Task<PilotServiceAssignmentDetail> FindPilotServiceAssignmentDetailById(long Id)
        {
            return await _context.PilotServiceAssignmentDetails
              .Include(ct => ct.PilotResource).Include(t => t.ActionReleaseType).Include(t => t.TempReleaseType).Include(t => t.ServiceAssignment)
            .Include(t => t.TempReleaseType).Include(t => t.CreatedBy).Include(t => t.PilotResource.PilotType)
              .FirstOrDefaultAsync(aer => aer.Id == Id && aer.IsDeleted == false);
        }

        public async Task<PilotServiceAssignmentDetail> FindPilotServiceAssignmentDetailByResourceId(long resourceId)
        {
            return await _context.PilotServiceAssignmentDetails
             .Include(ct => ct.PilotResource).Include(t => t.ActionReleaseType).Include(t => t.TempReleaseType).Include(t => t.ServiceAssignment)
           .Include(t => t.TempReleaseType).Include(t => t.CreatedBy).Include(t => t.PilotResource.PilotType).OrderByDescending(x=>x.Id)
             .FirstOrDefaultAsync(aer => aer.PilotResourceId == resourceId && aer.IsDeleted == false);
        }

        public async Task<PilotServiceAssignmentDetail> FindPilotServiceAssignmentDetailByResourceId2(long? resourceId)
        {
            return await _context.PilotServiceAssignmentDetails
            .Include(ct => ct.PilotResource).Include(t => t.ActionReleaseType).Include(t => t.TempReleaseType).Include(t => t.ServiceAssignment)
          .Include(t => t.TempReleaseType).Include(t => t.CreatedBy).Include(t => t.PilotResource.PilotType)
            .FirstOrDefaultAsync(aer => aer.PilotResourceId == resourceId && aer.IsDeleted == false && aer.IsTemporarilyHeld == true);
        }

        public async Task<PilotServiceAssignmentDetail> FindPilotServiceAssignmentDetailByResourceIdAndAssignmentId(long resourceId, long assId)
        {
            return await _context.PilotServiceAssignmentDetails
            .Include(ct => ct.PilotResource).Include(t => t.ActionReleaseType).Include(t => t.TempReleaseType).Include(t => t.ServiceAssignment)
          .Include(t => t.TempReleaseType).Include(t => t.CreatedBy).Include(t => t.PilotResource.PilotType)
            .FirstOrDefaultAsync(aer => aer.PilotResourceId == resourceId && aer.ServiceAssignmentId == assId && aer.IsDeleted == false);
        }

        public async Task<IEnumerable<PilotServiceAssignmentDetail>> FindPilotServiceAssignmentDetailByTiedVehicleResourceIdAndAssignmentId(long? tiedResourceId, long assId)
        {
            return await _context.PilotServiceAssignmentDetails.Where(aer => aer.TiedVehicleResourceId == tiedResourceId && aer.ServiceAssignmentId == assId && aer.IsDeleted == false)
           .Include(ct => ct.PilotResource).Include(t => t.ActionReleaseType).Include(t => t.TempReleaseType).Include(t => t.ServiceAssignment)
         .Include(t => t.TempReleaseType).Include(t => t.CreatedBy).Include(t => t.PilotResource.PilotType)
           .ToListAsync();
        }

        public async Task<VehicleServiceAssignmentDetail> FindVehicleServiceAssignmentDetailByAssignmentId(long Id)
        {
            return await _context.VehicleServiceAssignmentDetails
               .Include(ct => ct.VehicleResource).Include(t => t.ActionReleaseType).Include(t => t.TempReleaseType).Include(t => t.ServiceAssignment)
             .Include(t => t.TempReleaseType).Include(t => t.CreatedBy).Include(t => t.VehicleResource.SupplierService).Include(t => t.VehicleResource.VehicleType)
               .FirstOrDefaultAsync(aer => aer.ServiceAssignmentId == Id && aer.IsDeleted == false);
        }

        public async Task<VehicleServiceAssignmentDetail> FindVehicleServiceAssignmentDetailById(long Id)
        {
            return await _context.VehicleServiceAssignmentDetails
               .Include(ct => ct.VehicleResource).Include(t => t.ActionReleaseType).Include(t => t.TempReleaseType).Include(t => t.ServiceAssignment)
             .Include(t => t.TempReleaseType).Include(t => t.CreatedBy).Include(t => t.VehicleResource.SupplierService).Include(t => t.VehicleResource.VehicleType)
               .FirstOrDefaultAsync(aer => aer.Id == Id && aer.IsDeleted == false);
        }

        public async Task<VehicleServiceAssignmentDetail> FindVehicleServiceAssignmentDetailByResourceId(long resourceId)
        {
            return await _context.VehicleServiceAssignmentDetails
               .Include(ct => ct.VehicleResource).Include(t => t.ActionReleaseType).Include(t => t.TempReleaseType).Include(t => t.ServiceAssignment)
             .Include(t => t.TempReleaseType).Include(t => t.CreatedBy).Include(t => t.VehicleResource.SupplierService).Include(t => t.VehicleResource.VehicleType)
             .OrderByDescending(x => x.Id)
               .FirstOrDefaultAsync(aer => aer.VehicleResourceId == resourceId && aer.IsDeleted == false);
        }

        public async Task<VehicleServiceAssignmentDetail> FindVehicleServiceAssignmentDetailByResourceId2(long? resourceId)
        {
            //var ddd = _context.VehicleServiceAssignmentDetails
            //   .Include(ct => ct.VehicleResource).Include(t => t.ActionReleaseType).Include(t => t.TempReleaseType).Include(t => t.ServiceAssignment)
            // .Include(t => t.TempReleaseType).Include(t => t.CreatedBy).Include(t => t.VehicleResource.SupplierService).Include(t => t.VehicleResource.VehicleType)
            //   .FirstOrDefaultAsync(aer => aer.VehicleResourceId == resourceId && aer.IsDeleted == false && aer.IsTemporarilyHeld == true);

            return await _context.VehicleServiceAssignmentDetails
               .Include(ct => ct.VehicleResource).Include(t => t.ActionReleaseType).Include(t => t.TempReleaseType).Include(t => t.ServiceAssignment)
             .Include(t => t.TempReleaseType).Include(t => t.CreatedBy).Include(t => t.VehicleResource.SupplierService).Include(t => t.VehicleResource.VehicleType)
               .FirstOrDefaultAsync(aer => aer.VehicleResourceId == resourceId && aer.IsDeleted == false && aer.IsTemporarilyHeld == true);
        }

        public async Task<VehicleServiceAssignmentDetail> FindVehicleServiceAssignmentDetailByResourceIdAndAssignmentId(long resourceId, long AssId)
        {
            return await _context.VehicleServiceAssignmentDetails
              .Include(ct => ct.VehicleResource).Include(t => t.ActionReleaseType).Include(t => t.TempReleaseType).Include(t => t.ServiceAssignment)
            .Include(t => t.TempReleaseType).Include(t => t.CreatedBy).Include(t => t.VehicleResource.SupplierService).Include(t => t.VehicleResource.VehicleType)
              .FirstOrDefaultAsync(aer => aer.VehicleResourceId == resourceId && aer.ServiceAssignmentId == AssId && aer.IsDeleted == false);
        }

        public async Task<VehicleServiceAssignmentDetail> FindVehicleServiceAssignmentDetailByResourceId_(long? resourceId)
        {
            return await _context.VehicleServiceAssignmentDetails
              .Include(ct => ct.VehicleResource).Include(t => t.ActionReleaseType).Include(t => t.TempReleaseType).Include(t => t.ServiceAssignment)
            .Include(t => t.TempReleaseType).Include(t => t.CreatedBy).Include(t => t.VehicleResource.SupplierService).Include(t => t.VehicleResource.VehicleType)
              .FirstOrDefaultAsync(aer => aer.VehicleResourceId == resourceId && aer.IsDeleted == false);
        }

        public async Task<ArmedEscortServiceAssignmentDetailReplacement> ReplaceArmedEscortServiceAssignmentdetail(ArmedEscortServiceAssignmentDetailReplacement serviceAssignmentDetail)
        {
            var savedEntity = await _context.ArmedEscortServiceAssignmentDetailReplacements.AddAsync(serviceAssignmentDetail);
            if (await SaveChanges())
            {
                return savedEntity.Entity;
            }
            return null;
        }

        public async Task<CommanderServiceAssignmentDetailReplacement> ReplaceCommanderServiceAssignmentdetail(CommanderServiceAssignmentDetailReplacement serviceAssignmentDetail)
        {
            var savedEntity = await _context.CommanderServiceAssignmentDetailReplacements.AddAsync(serviceAssignmentDetail);
            if (await SaveChanges())
            {
                return savedEntity.Entity;
            }
            return null;
        }

        public async Task<PilotServiceAssignmentDetailReplacement> ReplacePilotServiceAssignmentdetail(PilotServiceAssignmentDetailReplacement serviceAssignmentDetail)
        {
            var savedEntity = await _context.PilotServiceAssignmentDetailReplacements.AddAsync(serviceAssignmentDetail);
            if (await SaveChanges())
            {
                return savedEntity.Entity;
            }
            return null;
        }

        public async Task<VehicleAssignmentDetailReplacement> ReplaceVehicleServiceAssignmentdetail(VehicleAssignmentDetailReplacement serviceAssignmentDetail)
        {
            var savedEntity = await _context.VehicleAssignmentDetailReplacements.AddAsync(serviceAssignmentDetail);
            if (await SaveChanges())
            {
                return savedEntity.Entity;
            }
            return null;
        }

        public async Task<CommanderServiceAssignmentDetail> SaveCommanderServiceAssignmentdetail(CommanderServiceAssignmentDetail serviceAssignmentDetail)
        {
            var savedEntity = await _context.CommanderServiceAssignmentDetails.AddAsync(serviceAssignmentDetail);
            if (await SaveChanges())
            {
                return savedEntity.Entity;
            }
            return null;
        }

        public async Task<ArmedEscortServiceAssignmentDetail> SaveEscortServiceAssignmentdetail(ArmedEscortServiceAssignmentDetail serviceAssignmentDetail)
        {
            var savedEntity = await _context.ArmedEscortServiceAssignmentDetails.AddAsync(serviceAssignmentDetail);
            if (await SaveChanges())
            {
                return savedEntity.Entity;
            }
            return null;
        }

        public async Task<Passenger> SavePassenger(Passenger passenger)
        {
            var savedEntity = await _context.Passengers.AddAsync(passenger);
            if (await SaveChanges())
            {
                return savedEntity.Entity;
            }
            return null;
        }

        public async Task<PilotServiceAssignmentDetail> SavePilotServiceAssignmentdetail(PilotServiceAssignmentDetail serviceAssignmentDetail)
        {
            var savedEntity = await _context.PilotServiceAssignmentDetails.AddAsync(serviceAssignmentDetail);
            if (await SaveChanges())
            {
                return savedEntity.Entity;
            }
            return null;
        }

        public async Task<VehicleServiceAssignmentDetail> SaveVehicleServiceAssignmentdetail(VehicleServiceAssignmentDetail serviceAssignmentDetail)
        {
            var savedEntity = await _context.VehicleServiceAssignmentDetails.AddAsync(serviceAssignmentDetail);
            if (await SaveChanges())
            {
                return savedEntity.Entity;
            }
            return null;
        }

        public async Task<bool> UpdateArmedEscortServiceAssignmentDetailHeldByAssignmentId(ArmedEscortServiceAssignmentDetail serviceAssignmentDetail)
        {
            serviceAssignmentDetail.IsHeldForAction = true;
            serviceAssignmentDetail.DateHeldForAction = DateTime.UtcNow;
            _context.ArmedEscortServiceAssignmentDetails.Update(serviceAssignmentDetail);
            return await SaveChanges();
        }

        public async Task<CommanderServiceAssignmentDetail> UpdateCommanderServiceAssignmentDetail(CommanderServiceAssignmentDetail serviceAssignmentDetail)
        {
            var updatedEntity = _context.CommanderServiceAssignmentDetails.Update(serviceAssignmentDetail);
            if (await SaveChanges())
            {
                return updatedEntity.Entity;
            }
            return null;
        }

        public async Task<bool> UpdateCommanderServiceAssignmentDetailForEndJourneyByAssignmentId(CommanderServiceAssignmentDetail serviceAssignmentDetail)
        {
            serviceAssignmentDetail.IsTemporarilyHeld = false;
            serviceAssignmentDetail.IsHeldForAction = false;
            _context.CommanderServiceAssignmentDetails.Update(serviceAssignmentDetail);
            return await SaveChanges();
        }

        public async Task<bool> UpdateCommanderServiceAssignmentDetailHeldByAssignmentId(CommanderServiceAssignmentDetail serviceAssignmentDetail)
        {
            serviceAssignmentDetail.IsHeldForAction = true;
            serviceAssignmentDetail.DateHeldForAction = DateTime.UtcNow;
            _context.CommanderServiceAssignmentDetails.Update(serviceAssignmentDetail);
            return await SaveChanges();
        }

        public async Task<ArmedEscortServiceAssignmentDetail> UpdateEscortServiceAssignmentDetail(ArmedEscortServiceAssignmentDetail serviceAssignmentDetail)
        {
            var updatedEntity = _context.ArmedEscortServiceAssignmentDetails.Update(serviceAssignmentDetail);
            if (await SaveChanges())
            {
                return updatedEntity.Entity;
            }
            return null;
        }

        public async Task<bool> UpdateEscortServiceAssignmentDetailForEndJourneyByAssignmentId(ArmedEscortServiceAssignmentDetail serviceAssignmentDetail)
        {
            serviceAssignmentDetail.IsTemporarilyHeld = false;
            serviceAssignmentDetail.IsHeldForAction = false;
            _context.ArmedEscortServiceAssignmentDetails.Update(serviceAssignmentDetail);
            return await SaveChanges();
        }

        public async Task<Passenger> UpdatePassenger(Passenger passenger)
        {
            var updatedEntity = _context.Passengers.Update(passenger);
            if (await SaveChanges())
            {
                return updatedEntity.Entity;
            }
            return null;
        }

        public async Task<PilotServiceAssignmentDetail> UpdatePilotServiceAssignmentDetail(PilotServiceAssignmentDetail serviceAssignmentDetail)
        {
            var updatedEntity = _context.PilotServiceAssignmentDetails.Update(serviceAssignmentDetail);
            if (await SaveChanges())
            {
                return updatedEntity.Entity;
            }
            return null;
        }

        public async Task<bool> UpdatePilotServiceAssignmentDetailForEndJourneyByAssignmentId(PilotServiceAssignmentDetail serviceAssignmentDetail)
        {
            serviceAssignmentDetail.IsTemporarilyHeld = false;
            serviceAssignmentDetail.IsHeldForAction = false;
            _context.PilotServiceAssignmentDetails.Update(serviceAssignmentDetail);
            return await SaveChanges();
        }

        public async Task<bool> UpdatePilotServiceAssignmentDetailHeldByAssignmentId(PilotServiceAssignmentDetail serviceAssignmentDetail)
        {
            serviceAssignmentDetail.IsHeldForAction = true;
            serviceAssignmentDetail.DateHeldForAction = DateTime.UtcNow;
            _context.PilotServiceAssignmentDetails.Update(serviceAssignmentDetail);
            return await SaveChanges();
        }

        public async Task<VehicleServiceAssignmentDetail> UpdateVehicleServiceAssignmentDetail(VehicleServiceAssignmentDetail serviceAssignmentDetail)
        {
            var updatedEntity = _context.VehicleServiceAssignmentDetails.Update(serviceAssignmentDetail);
            if (await SaveChanges())
            {
                return updatedEntity.Entity;
            }
            return null;
        }

        public async Task<bool> UpdateVehicleServiceAssignmentDetailForEndJourneyByAssignmentId(VehicleServiceAssignmentDetail serviceAssignmentDetail)
        {
            serviceAssignmentDetail.IsTemporarilyHeld = false;
            serviceAssignmentDetail.IsHeldForAction = false;
            _context.VehicleServiceAssignmentDetails.Update(serviceAssignmentDetail);
            return await SaveChanges();
        }

        public async Task<bool> UpdateVehicleServiceAssignmentDetailHeldByAssignmentId(VehicleServiceAssignmentDetail serviceAssignmentDetail)
        {
            serviceAssignmentDetail.IsHeldForAction = true;
            serviceAssignmentDetail.DateHeldForAction = DateTime.Now;
            _context.VehicleServiceAssignmentDetails.Update(serviceAssignmentDetail);
            return await SaveChanges();
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
