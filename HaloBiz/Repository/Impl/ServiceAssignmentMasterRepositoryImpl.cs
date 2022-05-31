using HaloBiz.DTOs.TransferDTOs;
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

        public async Task<bool> DeleteSecondaryServiceAssignment(SecondaryServiceAssignment serviceAssignment)
        {
            serviceAssignment.IsDeleted = true;
            _context.SecondaryServiceAssignments.Update(serviceAssignment);
            return await SaveChanges();
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

        public async Task<IEnumerable<SecondaryServiceAssignment>> FindAllSecondaryServiceAssignments()
        {
            return await _context.SecondaryServiceAssignments.Where(type => type.IsDeleted == false)
              .Include(ct => ct.ServiceAssignment).Include(t => t.SecondaryContractService).Include(t => t.SecondaryServiceRegistration)
              
              .OrderByDescending(x => x.Id)
              .ToListAsync();
        }

        public async Task<IEnumerable<SecondaryServiceAssignment>> FindAllSecondaryServiceAssignmentsByAssignmentId(long Id)
        {
            return await _context.SecondaryServiceAssignments.Where(type => type.IsDeleted == false && type.ServiceAssignmentId == Id)
             .Include(ct => ct.ServiceAssignment).Include(t => t.SecondaryContractService).Include(t => t.SecondaryServiceRegistration)

             .OrderByDescending(x => x.Id)
             .ToListAsync();
        }

        public async Task<IEnumerable<MasterServiceAssignment>> FindAllServiceAssignments()
        {
            return await _context.MasterServiceAssignments.Where(type => type.IsDeleted == false && type.IsScheduled == false)
               .Include(ct => ct.ContractService).Include(t=>t.CustomerDivision).Include(t=>t.SMORegion).Include(t=>t.SMORegion)
               .Include(sec=>sec.SecondaryServiceAssignments.Where(x=>x.IsDeleted == false))
               .Include(t=>t.SMORoute).Include(t=>t.SourceType).Include(t=>t.TripType).Include(t=>t.CreatedBy).Include(t=>t.ServiceRegistration)
               .Include(t=>t.ServiceRegistration.Service).Include(t => t.ServiceRegistration.ApplicableArmedEscortTypes.Where(t=>t.IsDeleted == false))
               .Include(t => t.ServiceRegistration.ApplicableCommanderTypes.Where(t => t.IsDeleted == false))
               .Include(t => t.ServiceRegistration.ApplicablePilotTypes.Where(t => t.IsDeleted == false))
               .Include(t => t.ServiceRegistration.ApplicableVehicleTypes.Where(t => t.IsDeleted == false))
               .OrderByDescending(x => x.Id)
               .ToListAsync();
        }

        public async Task<IEnumerable<MasterServiceAssignment>> FindAllScheduledServiceAssignments()
        {
            return await _context.MasterServiceAssignments.Where(type => type.IsDeleted == false && type.IsScheduled == true)
               .Include(ct => ct.ContractService).Include(t => t.CustomerDivision).Include(t => t.SMORegion).Include(t => t.SMORegion)
               .Include(sec => sec.SecondaryServiceAssignments.Where(x => x.IsDeleted == false))
               .Include(t => t.SMORoute).Include(t => t.SourceType).Include(t => t.TripType).Include(t => t.CreatedBy).Include(t => t.ServiceRegistration)
               .Include(t => t.ServiceRegistration.Service).Include(t => t.ServiceRegistration.ApplicableArmedEscortTypes.Where(t => t.IsDeleted == false))
               .Include(t => t.ServiceRegistration.ApplicableCommanderTypes.Where(t => t.IsDeleted == false))
               .Include(t => t.ServiceRegistration.ApplicablePilotTypes.Where(t => t.IsDeleted == false))
               .Include(t => t.ServiceRegistration.ApplicableVehicleTypes.Where(t => t.IsDeleted == false))
               .OrderBy(x => x.PickupDate)
               .ToListAsync();
        }

        public async Task<SecondaryServiceAssignment> FindSecondaryServiceAssignmentById(long Id)
        {
            return await _context.SecondaryServiceAssignments.Where(type => type.IsDeleted == false && type.Id == Id)
           .Include(ct => ct.ServiceAssignment).Include(t => t.SecondaryContractService).Include(t => t.SecondaryServiceRegistration)
           .FirstOrDefaultAsync();
        }

        public async Task<MasterServiceAssignment> FindServiceAssignmentById(long Id)
        {
            return await _context.MasterServiceAssignments
               .Include(ct => ct.ContractService).Include(t => t.SMORegion).Include(t => t.SMORegion)
               .Include(sec => sec.SecondaryServiceAssignments.Where(x => x.IsDeleted == false))
               .Include(t => t.SMORoute).Include(t => t.SourceType).Include(t => t.TripType).Include(t => t.CreatedBy).Include(t => t.ServiceRegistration)
               .Include(t => t.ServiceRegistration.Service).Include(t => t.ServiceRegistration.ApplicableArmedEscortTypes.Where(t => t.IsDeleted == false))
               .Include(t => t.ServiceRegistration.ApplicableCommanderTypes.Where(t => t.IsDeleted == false))
               .Include(t => t.ServiceRegistration.ApplicablePilotTypes.Where(t => t.IsDeleted == false))
               .Include(t => t.ServiceRegistration.ApplicableVehicleTypes.Where(t => t.IsDeleted == false))
               .FirstOrDefaultAsync(aer => aer.Id == Id && aer.IsDeleted == false);
        }

        public async Task<SecondaryServiceAssignment> SaveSecondaryServiceAssignment(SecondaryServiceAssignment serviceAssignment)
        {
            var savedEntity = await _context.SecondaryServiceAssignments.AddAsync(serviceAssignment);
            if (await SaveChanges())
            {
                return savedEntity.Entity;
            }
            return null;
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

        public async Task<bool> UpdateisAddedToCartStatus(MasterServiceAssignment serviceAssignment)
        {
            serviceAssignment.IsAddedToCart = true;
            _context.MasterServiceAssignments.Update(serviceAssignment);
            return await SaveChanges();
        }

        public async Task<bool> UpdateisPaidForStatus(MasterServiceAssignment serviceAssignment)
        {
            serviceAssignment.IsPaidFor = true;
            _context.MasterServiceAssignments.Update(serviceAssignment);
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

        public async Task<IEnumerable<MasterServiceAssignmentWithRegisterTransferDTO>> FindAllServiceAssignmentsByClientId(long clientId)
        {
            //return await _context.MasterServiceAssignments.Where(aer => aer.CustomerDivisionId == clientId && aer.IsDeleted == false && aer.ReadyStatus == false && aer.IsScheduled == false && aer.IsPaidFor == false && aer.IsAddedToCart == true)
            //  .Include(ct => ct.ContractService).Include(t => t.SMORegion).Include(t => t.SMORegion)
            //  .Include(sec => sec.SecondaryServiceAssignments.Where(x => x.IsDeleted == false))
            //  .Include(t => t.SMORoute).Include(t => t.CreatedBy).Include(t => t.ServiceRegistration)
            //  .Include(t => t.ServiceRegistration.Service).ThenInclude(x=>x.ServiceCategory).ToListAsync();

            var q =  await (from pd in _context.MasterServiceAssignments
                     //join od in _context.PriceRegisters on pd.ServiceRegistrationId equals od.ServiceRegistrationId
                     join od in _context.PriceRegisters on
                     new { pd1 = pd.ServiceRegistrationId, pd2 = pd.SMORouteId } equals
                     new { pd1 = od.ServiceRegistrationId, pd2 = od.SMORouteId }
                     where pd.CustomerDivisionId == clientId && pd.IsDeleted == false && pd.ReadyStatus == false && pd.IsScheduled == false && pd.IsPaidFor == false && pd.IsAddedToCart == true
                     select new MasterServiceAssignmentWithRegisterTransferDTO
                     {
                        Id = pd.Id,
                        SMORouteId = pd.SMORouteId,
                        RouteName = pd.SMORoute.RouteName,
                        PickoffLocation = pd.PickoffLocation,
                         DropoffLocation = pd.DropoffLocation,
                         AssignmentStatus = pd.AssignmentStatus,
                         IsPaidFor = pd.IsPaidFor,
                         InhouseAssignment = pd.InhouseAssignment,
                         IsScheduled = pd.IsScheduled,
                         ContractServiceId = pd.ContractServiceId,
                         PickupDate = pd.PickupDate,
                         DropoffDate = pd.DropoffDate,
                         PickoffTime = pd.PickoffTime,
                         ServiceRegistration = pd.ServiceRegistration,
                         Service = pd.ServiceRegistration.Service,
                         ReadyStatus = pd.ReadyStatus,
                         SellingPrice = od.SellingPrice,
                         CostPrice = od.CostPrice,
                         MarkupPrice = od.MarkupPrice,
                         ServiceCategory = od.ServiceRegistration.Service.ServiceCategory,
                     }).ToListAsync();

            return  q;

        }

        public async Task<IEnumerable<MasterServiceAssignmentWithRegisterTransferDTO>> FindAllServiceAssignmentsForScheduleCartByClientId(long clientId)
        {
            //return await _context.MasterServiceAssignments.Where(aer => aer.CustomerDivisionId == clientId && aer.IsDeleted == false && aer.IsScheduled == true && aer.InhouseAssignment == false && aer.IsPaidFor == false && aer.IsAddedToCart == true)
            //  .Include(ct => ct.ContractService).Include(t => t.SMORegion).Include(t => t.SMORegion)
            //  .Include(sec => sec.SecondaryServiceAssignments.Where(x => x.IsDeleted == false))
            //  .Include(t => t.SMORoute).Include(t => t.CreatedBy).Include(t => t.ServiceRegistration)
            //  .Include(t => t.ServiceRegistration.Service).ThenInclude(x=>x.ServiceCategory).ToListAsync();
            var q = await (from pd in _context.MasterServiceAssignments
                           join od in _context.PriceRegisters on
                           new { pd1 = pd.ServiceRegistrationId, pd2 = pd.SMORouteId } equals
                           new { pd1 = od.ServiceRegistrationId, pd2 = od.SMORouteId }
                           where pd.CustomerDivisionId == clientId && pd.IsDeleted == false && pd.ReadyStatus == false && pd.IsScheduled == true && pd.IsPaidFor == false && pd.InhouseAssignment == false && pd.IsAddedToCart == true
                           select new MasterServiceAssignmentWithRegisterTransferDTO
                           {
                               Id = pd.Id,
                               SMORouteId = pd.SMORouteId,
                               RouteName = pd.SMORoute.RouteName,
                               PickoffLocation = pd.PickoffLocation,
                               DropoffLocation = pd.DropoffLocation,
                               AssignmentStatus = pd.AssignmentStatus,
                               IsPaidFor = pd.IsPaidFor,
                               InhouseAssignment = pd.InhouseAssignment,
                               IsScheduled = pd.IsScheduled,
                               ContractServiceId = pd.ContractServiceId,
                               PickupDate = pd.PickupDate,
                               DropoffDate = pd.DropoffDate,
                               PickoffTime = pd.PickoffTime,
                               Service = pd.ServiceRegistration.Service,
                               ServiceRegistration = pd.ServiceRegistration,
                               ReadyStatus = pd.ReadyStatus,
                               SellingPrice = od.SellingPrice,
                               CostPrice = od.CostPrice,
                               MarkupPrice = od.MarkupPrice,
                               ServiceCategory = od.ServiceRegistration.Service.ServiceCategory,
                           }).ToListAsync();

            return q;
        }
    }
}
