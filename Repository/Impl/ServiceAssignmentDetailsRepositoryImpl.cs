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
            serviceAssignmentDetail.IsDeleted = true;
            _context.CommanderServiceAssignmentDetails.Update(serviceAssignmentDetail);
            return await SaveChanges();
        }

        public async Task<bool> DeleteCommanderServiceAssignmentDetailByAssignmentId(CommanderServiceAssignmentDetail serviceAssignmentDetail)
        {
            serviceAssignmentDetail.IsDeleted = true;
            _context.CommanderServiceAssignmentDetails.Update(serviceAssignmentDetail);
            return await SaveChanges();
        }

        public async Task<bool> DeleteEscortServiceAssignmentDetail(ArmedEscortServiceAssignmentDetail serviceAssignmentDetail)
        {
            serviceAssignmentDetail.IsDeleted = true;
            _context.ArmedEscortServiceAssignmentDetails.Update(serviceAssignmentDetail);
            return await SaveChanges();
        }

        public async Task<bool> DeleteEscortServiceAssignmentDetailByAssignmentId(ArmedEscortServiceAssignmentDetail serviceAssignmentDetail)
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
            serviceAssignmentDetail.IsDeleted = true;
            _context.PilotServiceAssignmentDetails.Update(serviceAssignmentDetail);
            return await SaveChanges();
        }

        public async Task<bool> DeletePilotServiceAssignmentDetailByAssignmentId(PilotServiceAssignmentDetail serviceAssignmentDetail)
        {
            serviceAssignmentDetail.IsDeleted = true;
            _context.PilotServiceAssignmentDetails.Update(serviceAssignmentDetail);
            return await SaveChanges();
        }

        public async Task<bool> DeleteVehicleServiceAssignmentDetail(VehicleServiceAssignmentDetail serviceAssignmentDetail)
        {
            serviceAssignmentDetail.IsDeleted = true;
            _context.VehicleServiceAssignmentDetails.Update(serviceAssignmentDetail);
            return await SaveChanges();
        }

        public async Task<bool> DeleteVehicleServiceAssignmentDetailByAssignmentId(VehicleServiceAssignmentDetail serviceAssignmentDetail)
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
            return await _context.CommanderServiceAssignmentDetails.Where(type => type.IsDeleted == false && type.ServiceAssignmentId == assignmentId)
               .Include(ct => ct.CommanderResource).Include(t => t.CommanderResource.Profile).Include(t => t.TiedVehicleResource).Include(t => t.ServiceAssignment)
               .Include(t => t.TempReleaseType).Include(t => t.CreatedBy).Include(t => t.ActionReleaseType).Include(t => t.CommanderResource.CommanderType)
               .OrderByDescending(x => x.Id)
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
            return await _context.ArmedEscortServiceAssignmentDetails.Where(type => type.IsDeleted == false && type.ServiceAssignmentId == assignmentId)
              .Include(ct => ct.ArmedEscortResource).Include(t => t.ActionReleaseType).Include(t => t.TempReleaseType).Include(t => t.ServiceAssignment)
              .Include(t => t.TempReleaseType).Include(t => t.CreatedBy).Include(t => t.ArmedEscortResource.ArmedEscortType)
              .OrderByDescending(x => x.Id)
              .ToListAsync();
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
            return await _context.PilotServiceAssignmentDetails.Where(type => type.IsDeleted == false && type.ServiceAssignmentId == assignmentId)
              .Include(ct => ct.PilotResource).Include(t => t.ActionReleaseType).Include(t => t.TempReleaseType).Include(t => t.ServiceAssignment)
              .Include(t => t.TempReleaseType).Include(t => t.CreatedBy).Include(t => t.PilotResource.PilotType)
              .OrderByDescending(x => x.Id)
              .ToListAsync();
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
            return await _context.VehicleServiceAssignmentDetails.Where(type => type.IsDeleted == false && type.ServiceAssignmentId == assignmentId)
            .Include(ct => ct.VehicleResource).Include(t => t.ActionReleaseType).Include(t => t.TempReleaseType).Include(t => t.ServiceAssignment)
            .Include(t => t.TempReleaseType).Include(t => t.CreatedBy).Include(t => t.VehicleResource.SupplierService).Include(t => t.VehicleResource.VehicleType)
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
             .FirstOrDefaultAsync(aer => aer.CommanderResourceId == resourceId && aer.IsDeleted == false);
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
             .Include(t => t.TempReleaseType).Include(t => t.CreatedBy).Include(t => t.ArmedEscortResource.ArmedEscortType)
               .FirstOrDefaultAsync(aer => aer.ArmedEscortResourceId == resourceId && aer.IsDeleted == false);
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
           .Include(t => t.TempReleaseType).Include(t => t.CreatedBy).Include(t => t.PilotResource.PilotType)
             .FirstOrDefaultAsync(aer => aer.PilotResourceId == resourceId && aer.IsDeleted == false);
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
               .FirstOrDefaultAsync(aer => aer.VehicleResourceId == resourceId && aer.IsDeleted == false);
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

        public async Task<CommanderServiceAssignmentDetail> UpdateCommanderServiceAssignmentDetail(CommanderServiceAssignmentDetail serviceAssignmentDetail)
        {
            var updatedEntity = _context.CommanderServiceAssignmentDetails.Update(serviceAssignmentDetail);
            if (await SaveChanges())
            {
                return updatedEntity.Entity;
            }
            return null;
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

        public async Task<VehicleServiceAssignmentDetail> UpdateVehicleServiceAssignmentDetail(VehicleServiceAssignmentDetail serviceAssignmentDetail)
        {
            var updatedEntity = _context.VehicleServiceAssignmentDetails.Update(serviceAssignmentDetail);
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
