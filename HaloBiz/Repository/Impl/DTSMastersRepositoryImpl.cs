using HaloBiz.DTOs.ReceivingDTOs;
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
    public class DTSMastersRepositoryImpl:IDTSMastersRepository
    {
        private readonly HalobizContext _context;
        private readonly ILogger<DTSMastersRepositoryImpl> _logger;
        private readonly IServiceAssignmentDetailsRepository _serviceAssignmentDetailsRepository;
        private readonly IServiceRegistrationRepository _serviceRegistrationRepository;
       // private readonly IVehicleRegistrationRepository _vehiclesRepository;
        //private readonly IPilotRegistrationRepository _pilotProfileRepository;
        public DTSMastersRepositoryImpl(HalobizContext context, ILogger<DTSMastersRepositoryImpl> logger, IServiceAssignmentDetailsRepository serviceAssignmentDetailsRepository, IServiceRegistrationRepository serviceRegistrationRepository)
        {
            this._logger = logger;
            this._context = context;
            _serviceAssignmentDetailsRepository = serviceAssignmentDetailsRepository;
            _serviceRegistrationRepository = serviceRegistrationRepository;
        }

        public async Task<bool> DeleteArmedEscortMaster(ArmedEscortDTSMaster armedEscort)
        {
            armedEscort.IsDeleted = true;
            _context.ArmedEscortDTSMasters.Update(armedEscort);
            return await SaveChanges();
        }

        public async Task<bool> DeleteCommanderMaster(CommanderDTSMaster commander)
        {
            commander.IsDeleted = true;
            _context.CommanderDTSMasters.Update(commander);
            return await SaveChanges();
        }

        public async Task<bool> DeletePilotMaster(PilotDTSMaster pilot)
        {
            pilot.IsDeleted = true;
            _context.PilotDTSMasters.Update(pilot);
            return await SaveChanges();
        }

        public async Task<bool> DeleteVehicleMaster(VehicleDTSMaster vehicle)
        {
            vehicle.IsDeleted = true;
            _context.VehicleDTSMasters.Update(vehicle);
            return await SaveChanges();
        }

        public async Task<IEnumerable<ArmedEscortDTSMaster>> FindAllArmedEscortMasters()
        {
            return await _context.ArmedEscortDTSMasters.Where(dts => dts.IsDeleted == false)
             .Include(dts => dts.CreatedBy).Include(dts => dts.GenericDays).Include(dts=>dts.ArmedEscortResource).Include(dts=>dts.ArmedEscortResource.SupplierService)
             .OrderByDescending(x=>x.Id)
             .ToListAsync();
        }

        public async Task<IEnumerable<ArmedEscortDTSMaster>> FindAllArmedEscortMastersByResourceId(long resourceId)
        {
            return await _context.ArmedEscortDTSMasters.Where(dts => dts.IsDeleted == false && dts.ArmedEscortResourceId == resourceId)
            .Include(dts => dts.CreatedBy).Include(dts => dts.GenericDays).Include(dts => dts.ArmedEscortResource).Include(dts => dts.ArmedEscortResource.SupplierService)
            .OrderByDescending(x => x.Id)
            .ToListAsync();
        }

        public async Task<ArmedEscortDTSMasterExtended> FindAllArmedEscortMastersForAutoAssignmentByPickupDate(long seviceRegId, long RouteId, DateTime pickupDate, DateTime pickUpTime)
        {
            var services = new List<ArmedEscortDTSMaster>();
            var services2 = new List<ArmedEscortDTSMaster>();
            var eligibleArmedEscorts = new List<ArmedEscortProfile>();
            var services_ = new List<ArmedEscortSMORoutesResourceTie>();
            var services2_ = new List<ArmedEscortSMORoutesResourceTie>();
            var services3_ = new List<ArmedEscortSMORoutesResourceTie>();
            var servicesType_ = new List<ArmedEscortResourceRequiredPerService>();
            //var servicesDetail_ = new List<VehicleServiceAssignmentDetail>();
            var query = _context.ArmedEscortSMORoutesResourceTies.Where
                         (ct => ct.SMORouteId == RouteId && ct.IsDeleted == false);
            var getResources = _context.ArmedEscortProfiles.Where(r => r.IsDeleted == false)
            .Include(s => s.ArmedEscortType).Include(t => t.SupplierService).Include(x=>x.ServiceAssignment)
            .Include(office => office.Rank)
                                  .ToList();

            var getAllResourceDetails = await _serviceAssignmentDetailsRepository.FindAllEscortServiceAssignmentDetails();
            var AEscortAssignmentSorted = getAllResourceDetails.Where(x => x.IsTemporarilyHeld != true && x.DateTemporarilyHeld != pickupDate && (pickupDate >= x.RecoveryDateTime || x.RecoveryDateTime == null)).OrderBy(x => x.DateTemporarilyHeld).DistinctBy(y => y.ArmedEscortResourceId);
            var AEscortWithAssignment = getAllResourceDetails.Where(x => x.DateTemporarilyHeld == pickupDate || x.IsTemporarilyHeld == true );
            //var eligibleArmedEscorts = getResources.Where(x => !AEscortWithAssignment.Any(y => y.ArmedEscortResourceId == x.Id)).OrderBy(x => x.CreatedAt).ToList();

            foreach (var items in getResources)
            {
                var getDetail = await _serviceAssignmentDetailsRepository.FindEscortServiceAssignmentDetailByResourceId2(items.Id);
                if (getDetail == null)
                {
                    eligibleArmedEscorts.Add(items);
                }

            }

            //check for route
            eligibleArmedEscorts = eligibleArmedEscorts.Where(x => query.Any(y => y.ResourceId == x.Id && y.SMORouteId == RouteId)).ToList();


            //type check
            var getType = _context.ArmedEscortResourceRequiredPerServices.Where
             (ct => ct.ServiceRegistrationId == seviceRegId && ct.IsDeleted == false).ToList();

            eligibleArmedEscorts = eligibleArmedEscorts.Where(x => getType.Any(y => y.ArmedEscortTypeId == x.ArmedEscortTypeId)).OrderBy(x => x.CreatedAt).ToList();
            
            //foreach (var items in eligibleArmedEscorts)
            //{
            //    var typeExists = _serviceRegistrationRepository.GetArmedEscortResourceApplicableTypeReqById(seviceRegId, items.ArmedEscortTypeId);
            //    if (typeExists == null)
            //    {
            //        eligibleArmedEscorts.Remove(items);
            //        //services_.Add(items);
            //    }

            //}

            var scheduleQuery = _context.ArmedEscortDTSMasters.Where(dts => dts.IsDeleted == false && pickupDate >= dts.AvailabilityStart && pickupDate <= dts.AvailablilityEnd)
            .Include(dts => dts.CreatedBy).Include(dts => dts.ArmedEscortResource).
            Include(dts => dts.GenericDays.Where(x => x.IsDeleted == false))
            .OrderByDescending(x => x.Id);

            //Check For active contrcat and Date 
            foreach (var items in eligibleArmedEscorts)
            {
                services.AddRange(scheduleQuery.Where(x => x.ArmedEscortResourceId == items.Id));
            }

            services.ToList();
            //Check For Days

            //Check For Day and Time
            foreach (var items in services)
            {
                foreach (var item in items.GenericDays)
                {

                    if (pickUpTime.TimeOfDay >= item.OpeningTime.TimeOfDay && pickUpTime.TimeOfDay <= item.ClosingTime.TimeOfDay)
                    {
                        if (pickupDate.DayOfWeek.ToString() == "Monday" && item.Monday == true)
                        {
                            //services2.AddRange(services.Where(x => x.VehicleResourceId == items.VehicleResourceId));
                            services2.Add(items);
                            break;
                        }
                        else if (pickupDate.DayOfWeek.ToString() == "Tuesday" && item.Tuesday == true)
                        {
                            services2.Add(items);
                            break;
                        }
                        else if (pickupDate.DayOfWeek.ToString() == "Wednesday" && item.Wednesday == true)
                        {
                            services2.Add(items);
                            break;
                        }
                        else if (pickupDate.DayOfWeek.ToString() == "Thursday" && item.Thursday == true)
                        {
                            services2.Add(items);
                            break;
                        }
                        else if (pickupDate.DayOfWeek.ToString() == "Friday" && item.Friday == true)
                        {
                            services2.Add(items);
                            break;
                        }
                        else if (pickupDate.DayOfWeek.ToString() == "Saturday" && item.Saturday == true)
                        {
                            services2.Add(items);
                            break;
                        }
                        else if (pickupDate.DayOfWeek.ToString() == "Sunday" && item.Sunday == true)
                        {
                            services2.Add(items);
                            break;
                        }

                    }
                    else
                    {
                        continue;
                    }

                }
                // services2.AddRange(services.Where(x => x.VehicleResourceId == items.ResourceId));
            }

            var eligibleArmedEscortsWithAssignment = services2.Where(x => AEscortAssignmentSorted.Any(y => y.ArmedEscortResourceId == x.ArmedEscortResourceId));
            //var eligibleArmedEscortsWithoutAssignment = services2.Where(x => !AEscortAssignmentSorted.Any(y => y.ArmedEscortResourceId == x.ArmedEscortResourceId));
            var eligibleArmedEscortsWithoutAssignment = new List<ArmedEscortDTSMaster>();
            var armedEscortAssignmentSortedDetails = getAllResourceDetails.DistinctBy(y => y.ArmedEscortResourceId).ToList();
            eligibleArmedEscortsWithoutAssignment = services2.Where(x => !armedEscortAssignmentSortedDetails.Any(y => y.ArmedEscortResourceId == x.ArmedEscortResourceId)).ToList();

            var resultModel = new ArmedEscortDTSMasterExtended()
            {
                eligibleArmedEscortsWithAssignment = eligibleArmedEscortsWithAssignment,
                eligibleArmedEscortsWithoutAssignment = eligibleArmedEscortsWithoutAssignment
            };

            return resultModel;
            //return services2.ToList();
        }

        public async Task<IEnumerable<CommanderDTSMaster>> FindAllCommanderMasters()
        {
            return await _context.CommanderDTSMasters.Where(dts => dts.IsDeleted == false)
            .Include(dts => dts.CreatedBy).Include(dts => dts.GenericDays).Include(dts => dts.CommanderResource).Include(dts=>dts.CommanderResource.Profile)
            .OrderByDescending(x => x.Id)
            .ToListAsync();
        }

        public async Task<IEnumerable<CommanderDTSMaster>> FindAllCommanderMastersByResourceId(long resourceId)
        {
            return await _context.CommanderDTSMasters.Where(dts => dts.IsDeleted == false && dts.CommanderResourceId == resourceId)
           .Include(dts => dts.CreatedBy).Include(dts => dts.GenericDays).Include(dts => dts.CommanderResource).Include(dts => dts.CommanderResource.Profile)
           .OrderByDescending(x => x.Id)
           .ToListAsync();
        }

        public async Task<CommanderDTSMasterExtended> FindAllCommanderMastersForAutoAssignmentByPickupDate(long seviceRegId, long RouteId, DateTime pickupDate, DateTime pickUpTime)
        {

            var services = new List<CommanderDTSMaster>();
            var services2 = new List<CommanderDTSMaster>();
            var eligibleCommanders = new List<CommanderProfile>();
            var services_ = new List<CommanderSMORoutesResourceTie>();
            var services2_ = new List<CommanderSMORoutesResourceTie>();
            var services3_ = new List<CommanderSMORoutesResourceTie>();
            var servicesType_ = new List<CommanderResourceRequiredPerService>();
            //var servicesDetail_ = new List<VehicleServiceAssignmentDetail>();
            var query = _context.CommanderSMORoutesResourceTies.Where
                         (ct => ct.SMORouteId == RouteId && ct.IsDeleted == false);
            var getResources = _context.CommanderProfiles.Where(r => r.IsDeleted == false)
            .Include(s => s.Profile).Include(t => t.CommanderType)
            .Include(office => office.Rank)
                                  .ToList();
            var getAllResourceDetails = await _serviceAssignmentDetailsRepository.FindAllCommanderServiceAssignmentDetails();
            var commanderAssignmentSorted = getAllResourceDetails.Where(x => x.IsTemporarilyHeld != true && x.DateTemporarilyHeld != pickupDate && (pickupDate >= x.RecoveryDateTime || x.RecoveryDateTime == null)).OrderBy(x => x.DateTemporarilyHeld).DistinctBy(y => y.CommanderResourceId);
            var commanderWithAssignment = getAllResourceDetails.Where(x => x.DateTemporarilyHeld == pickupDate || x.IsTemporarilyHeld == true );
            //var eligibleCommanders = getResources.Where(x => !commanderWithAssignment.Any(y => y.CommanderResourceId == x.Id)).OrderBy(x => x.CreatedAt).ToList();

            foreach (var items in getResources)
            {
                var getDetail = await _serviceAssignmentDetailsRepository.FindCommanderServiceAssignmentDetailByResourceId2(items.Id);
                if (getDetail == null)
                {
                    eligibleCommanders.Add(items);
                }

            }

            //check for route
            eligibleCommanders = eligibleCommanders.Where(x => query.Any(y => y.ResourceId == x.Id && y.SMORouteId == RouteId)).ToList();


            //type check
            var getType = _context.CommanderResourceRequiredPerServices.Where
              (ct => ct.ServiceRegistrationId == seviceRegId && ct.IsDeleted == false).ToList();

            eligibleCommanders = eligibleCommanders.Where(x => getType.Any(y => y.CommanderTypeId == x.CommanderTypeId)).OrderBy(x => x.CreatedAt).DistinctBy(y => y.CommanderTypeId).ToList();

            //foreach (var items in eligibleCommanders)
            //{
            //    var typeExists = _serviceRegistrationRepository.GetCommanderResourceApplicableTypeReqById(seviceRegId, items.CommanderTypeId);
            //    if (typeExists == null)
            //    {
            //        eligibleCommanders.Remove(items);
            //        //services_.Add(items);
            //    }

            //}
            

            var scheduleQuery = _context.CommanderDTSMasters.Where(dts => dts.IsDeleted == false && pickupDate >= dts.AvailabilityStart && pickupDate <= dts.AvailablilityEnd)
            .Include(dts => dts.CreatedBy).Include(dts => dts.CommanderResource).
            Include(dts => dts.GenericDays.Where(x => x.IsDeleted == false))
            .OrderByDescending(x => x.Id);

            //Check For active contrcat and Date 
            foreach (var items in eligibleCommanders)
            {
                services.AddRange(scheduleQuery.Where(x => x.CommanderResourceId == items.Id));
            }

            services.ToList();
            //Check For Days

            //Check For Day and Time
            foreach (var items in services)
            {
                foreach (var item in items.GenericDays)
                {
                    if (pickUpTime.TimeOfDay >= item.OpeningTime.TimeOfDay && pickUpTime.TimeOfDay <= item.ClosingTime.TimeOfDay)
                    {
                        if (pickupDate.DayOfWeek.ToString() == "Monday" && item.Monday == true)
                        {
                            //services2.AddRange(services.Where(x => x.VehicleResourceId == items.VehicleResourceId));
                            services2.Add(items);
                            break;
                        }
                        else if (pickupDate.DayOfWeek.ToString() == "Tuesday" && item.Tuesday == true)
                        {
                            services2.Add(items);
                            break;
                        }
                        else if (pickupDate.DayOfWeek.ToString() == "Wednesday" && item.Wednesday == true)
                        {
                            services2.Add(items);
                            break;
                        }
                        else if (pickupDate.DayOfWeek.ToString() == "Thursday" && item.Thursday == true)
                        {
                            services2.Add(items);
                            break;
                        }
                        else if (pickupDate.DayOfWeek.ToString() == "Friday" && item.Friday == true)
                        {
                            services2.Add(items);
                            break;
                        }
                        else if (pickupDate.DayOfWeek.ToString() == "Saturday" && item.Saturday == true)
                        {
                            services2.Add(items);
                            break;
                        }
                        else if (pickupDate.DayOfWeek.ToString() == "Sunday" && item.Sunday == true)
                        {
                            services2.Add(items);
                            break;
                        }

                    }
                    else
                    {
                        continue;
                    }

                }
                // services2.AddRange(services.Where(x => x.VehicleResourceId == items.ResourceId));
            }

            var eligibleCommandersWithAssignment = services2.Where(x => commanderAssignmentSorted.Any(y => y.CommanderResourceId == x.CommanderResourceId));
            //var eligibleCommandersWithoutAssignment = services2.Where(x => !commanderAssignmentSorted.Any(y => y.CommanderResourceId == x.CommanderResourceId));
            var eligibleCommandersWithoutAssignment = new List<CommanderDTSMaster>();
            var commanderAssignmentSortedDetails = getAllResourceDetails.DistinctBy(y => y.CommanderResourceId).ToList();
            eligibleCommandersWithoutAssignment = services2.Where(x => !commanderAssignmentSortedDetails.Any(y => y.CommanderResourceId == x.CommanderResourceId)).ToList();

            var resultModel = new CommanderDTSMasterExtended()
            {
                eligibleCommandersWithAssignment = eligibleCommandersWithAssignment,
                eligibleCommandersWithoutAssignment = eligibleCommandersWithoutAssignment
            };

            return resultModel;
            //return services2.ToList();
        }

        public async Task<IEnumerable<PilotDTSMaster>> FindAllPilotMasters()
        {
            return await _context.PilotDTSMasters.Where(dts => dts.IsDeleted == false)
            .Include(dts => dts.CreatedBy).Include(dts => dts.GenericDays).Include(dts => dts.PilotResource)
            .OrderByDescending(x => x.Id)
            .ToListAsync();
        }

        public async Task<IEnumerable<PilotDTSMaster>> FindAllPilotMastersByResourceId(long resourceId)
        {
            return await _context.PilotDTSMasters.Where(dts => dts.IsDeleted == false && dts.PilotResourceId == resourceId)
           .Include(dts => dts.CreatedBy).Include(dts => dts.GenericDays).Include(dts => dts.PilotResource)
           .OrderByDescending(x => x.Id)
           .ToListAsync();
        }

        public async Task<PilotDTSMasterExtended> FindAllPilotMastersForAutoAssignmentByPickupDate(long seviceRegId, long RouteId, DateTime pickupDate, DateTime pickUpTime)
        {
            var services = new List<PilotDTSMaster>();
            var services2 = new List<PilotDTSMaster>();
            var eligiblePilots = new List<PilotProfile>();
            var services_ = new List<PilotSMORoutesResourceTie>();
            var services2_ = new List<PilotSMORoutesResourceTie>();
            var services3_ = new List<PilotSMORoutesResourceTie>();
            var servicesType_ = new List<PilotResourceRequiredPerService>();
            //var servicesDetail_ = new List<VehicleServiceAssignmentDetail>();
            var query = _context.PilotSMORoutesResourceTies.Where
                         (ct => ct.SMORouteId == RouteId && ct.IsDeleted == false).ToList();
            var getResources = _context.PilotProfiles.Where(r => r.IsDeleted == false)
            .Include(s => s.MeansOfIdentification).Include(t => t.PilotType)
            .Include(office => office.Rank)
                                  .ToList();
            var getAllResourceDetails = await _serviceAssignmentDetailsRepository.FindAllPilotServiceAssignmentDetails();
            var pilotAssignmentSorted = getAllResourceDetails.Where(x => x.IsTemporarilyHeld != true && x.DateTemporarilyHeld != pickupDate && (pickupDate >= x.RecoveryDateTime || x.RecoveryDateTime == null)).OrderBy(x => x.DateTemporarilyHeld).DistinctBy(y => y.PilotResourceId);
            var pilotWithAssignment = getAllResourceDetails.Where(x => x.DateTemporarilyHeld == pickupDate || x.IsTemporarilyHeld == true );

            //var eligiblePilots = getResources.Where(x => !pilotWithAssignment.Any(y => y.PilotResourceId == x.Id)).OrderBy(x => x.CreatedAt).ToList();
            // resources.OrderBy(x=>x.CreatedAt).ToList();
            foreach (var items in getResources)
            {
                var getDetail = await _serviceAssignmentDetailsRepository.FindPilotServiceAssignmentDetailByResourceId2(items.Id);
                if (getDetail == null)
                {
                    eligiblePilots.Add(items);
                }

            }

            //check for route
            eligiblePilots = eligiblePilots.Where(x => query.Any(y => y.ResourceId == x.Id &&  y.SMORouteId == RouteId)).ToList();


            //type check
            var getType = _context.PilotResourceRequiredPerService.Where
               (ct => ct.ServiceRegistrationId == seviceRegId && ct.IsDeleted == false).ToList();

            eligiblePilots = eligiblePilots.Where(x => getType.Any(y => y.PilotTypeId == x.PilotTypeId)).OrderBy(x => x.CreatedAt).ToList();

            //foreach (var items in eligiblePilots)
            //{
            //    var typeExists = _serviceRegistrationRepository.GetPilotResourceApplicableTypeReqById(seviceRegId, items.PilotTypeId);
            //    if (typeExists == null)
            //    {
            //        eligiblePilots.Remove(items);
            //    }

            //}
           

            var scheduleQuery = _context.PilotDTSMasters.Where(dts => dts.IsDeleted == false && pickupDate >= dts.AvailabilityStart && pickupDate <= dts.AvailablilityEnd)
            .Include(dts => dts.CreatedBy).Include(dts => dts.PilotResource).
            Include(dts => dts.GenericDays.Where(x => x.IsDeleted == false));

            //Check For active Contract and Date
            foreach (var items in eligiblePilots)
            {
                services.AddRange(scheduleQuery.Where(x => x.PilotResourceId == items.Id));
            }

            services.ToList();
            //Check For Days

            //Check For Date and Time
            foreach (var items in services)
            {
                foreach (var item in items.GenericDays)
                {
                    if (pickUpTime.TimeOfDay >= item.OpeningTime.TimeOfDay && pickUpTime.TimeOfDay <= item.ClosingTime.TimeOfDay)
                    {
                        if (pickupDate.DayOfWeek.ToString() == "Monday" && item.Monday == true)
                        {
                            //services2.AddRange(services.Where(x => x.VehicleResourceId == items.VehicleResourceId));
                            services2.Add(items);
                            break;
                        }
                        else if (pickupDate.DayOfWeek.ToString() == "Tuesday" && item.Tuesday == true)
                        {
                            services2.Add(items);
                            break;
                        }
                        else if (pickupDate.DayOfWeek.ToString() == "Wednesday" && item.Wednesday == true)
                        {
                            services2.Add(items);
                            break;
                        }
                        else if (pickupDate.DayOfWeek.ToString() == "Thursday" && item.Thursday == true)
                        {
                            services2.Add(items);
                            break;
                        }
                        else if (pickupDate.DayOfWeek.ToString() == "Friday" && item.Friday == true)
                        {
                            services2.Add(items);
                            break;
                        }
                        else if (pickupDate.DayOfWeek.ToString() == "Saturday" && item.Saturday == true)
                        {
                            services2.Add(items);
                            break;
                        }
                        else if (pickupDate.DayOfWeek.ToString() == "Sunday" && item.Sunday == true)
                        {
                            services2.Add(items);
                            break;
                        }

                    }
                    else
                    {
                        continue;
                    }

                }
                // services2.AddRange(services.Where(x => x.VehicleResourceId == items.ResourceId));
            }

            var eligiblePilotsWithAssignment = services2.Where(x => pilotAssignmentSorted.Any(y => y.PilotResourceId == x.PilotResourceId));
            //var eligiblePilotsWithoutAssignment = services2.Where(x => !pilotAssignmentSorted.Any(y => y.PilotResourceId == x.PilotResourceId));
            var eligiblePilotsWithoutAssignment = new List<PilotDTSMaster>();
            var pilotAssignmentSortedDetails = getAllResourceDetails.DistinctBy(y => y.PilotResourceId).ToList();
            eligiblePilotsWithoutAssignment = services2.Where(x => !pilotAssignmentSortedDetails.Any(y => y.PilotResourceId == x.PilotResourceId)).ToList();

            var resultModel = new PilotDTSMasterExtended()
            {
                eligiblePilotsWithAssignment = eligiblePilotsWithAssignment,
                eligiblePilotsWithoutAssignment = eligiblePilotsWithoutAssignment
            };

            return resultModel;
            //Arrange based on released date 
            //return services2.ToList();
        }

        public async Task<IEnumerable<VehicleDTSMaster>> FindAllVehicleMasters()
        {
            return await _context.VehicleDTSMasters.Where(dts => dts.IsDeleted == false)
            .Include(dts => dts.CreatedBy).Include(dts=>dts.GenericDays).Include(dts => dts.VehicleResource).Include(dts => dts.VehicleResource.SupplierService)
            .OrderByDescending(x => x.Id)
            .ToListAsync();
        }

        public async Task<IEnumerable<VehicleDTSMaster>> FindAllVehicleMastersByResourceId(long resourceId)
        {
            return await _context.VehicleDTSMasters.Where(dts => dts.IsDeleted == false && dts.VehicleResourceId == resourceId)
           .Include(dts => dts.CreatedBy).Include(dts => dts.GenericDays).Include(dts => dts.VehicleResource).Include(dts => dts.VehicleResource.SupplierService)
           .OrderByDescending(x => x.Id)
           .ToListAsync();
        }

        public async Task<IEnumerable<VehicleDTSMaster>> FindAllVehicleMastersForAutoAssignment()
        {
            return await _context.VehicleDTSMasters.Where(dts => dts.IsDeleted == false && dts.AvailablilityEnd > DateTime.Now)
           .Include(dts => dts.CreatedBy).Include(dts => dts.GenericDays.Where(x=>x.IsDeleted == false)).Include(dts => dts.VehicleResource).Include(dts => dts.VehicleResource.SupplierService)
           .OrderByDescending(x => x.Id)
           .ToListAsync();
        }

        //public async Task<IEnumerable<VehicleDTSMaster>> FindAllVehicleMastersForAutoAssignmentByPickupDate_(long seviceRegId, long RouteId, DateTime pickupDate, DateTime pickUpTime)
        //{
        //    var services = new List<VehicleDTSMaster>();
        //    var services2 = new List<VehicleDTSMaster>();
        //    var resources = new List<Vehicle>();
        //    var services_ = new List<VehicleSMORoutesResourceTie>();
        //    var services2_ = new List<VehicleSMORoutesResourceTie>();
        //    var services3_ = new List<VehicleSMORoutesResourceTie>();
        //    var servicesType_ = new List<VehicleResourceRequiredPerService>();
        //    var servicesDetail_ = new List<VehicleServiceAssignmentDetail>();
        //    var query = _context.VehicleSMORoutesResourceTies.Where
        //                 (ct => ct.SMORouteId == RouteId && ct.IsDeleted == false);
        //    //Getting all  registered resources
        //    var getResources = _context.Vehicles.Where(r => r.IsDeleted == false)
        //    .Include(s => s.SupplierService).Include(t => t.VehicleType)
        //    .Include(office => office.AttachedOffice).Include(br => br.AttachedBranch)
        //                          .ToList();
        //    //Held
        //    var resourceNotHeld = _context.VehicleServiceAssignmentDetails.Where(type => type.IsDeleted == false && type.IsTemporarilyHeld != true)
        //     .Include(ct => ct.VehicleResource).Include(t => t.ActionReleaseType).Include(t => t.TempReleaseType).Include(t => t.ServiceAssignment)
        //     .Include(t => t.TempReleaseType).Include(t => t.CreatedBy).Include(t => t.VehicleResource.SupplierService).Include(t => t.VehicleResource.VehicleType)
        //     .OrderByDescending(x => x.CreatedAt);
        //    //Check For Held
        //    foreach (var items in getResources)
        //    {
        //        var getVehicleDetail = await _serviceAssignmentDetailsRepository.FindVehicleServiceAssignmentDetailByResourceId2(items.Id);
        //        if (getVehicleDetail == null)
        //        {
        //            resources.Add(items);
        //        }

        //    }
        //    resources.OrderBy(x => x.CreatedAt).ToList();


        //    //check for route
        //    foreach (var items in resources)
        //    {
        //        services3_.AddRange(query.Where(x => x.ResourceId == items.Id));
        //    }
        //    services3_.ToList();

        //    //type check
        //    foreach (var items in services3_)
        //    {
        //        var typeExists = _serviceRegistrationRepository.GetVehicleResourceApplicableTypeReqById(seviceRegId, items.Resource.VehicleTypeId);
        //        if (typeExists != null)
        //        {
        //            services_.Add(items);
        //        }
        //        //servicesType_.AddRange(getTypesByServiceRegid.Where(x => x.VehicleTypeId == items.VehicleResource.VehicleTypeId));
        //    }
        //    services_.ToList();

        //    var scheduleQuery = _context.VehicleDTSMasters.Where(dts => dts.IsDeleted == false)
        //    .Include(dts => dts.CreatedBy).Include(dts => dts.VehicleResource).Include(dts => dts.VehicleResource.SupplierService).
        //    Include(dts => dts.GenericDays.Where(x => x.IsDeleted == false))
        //    .OrderByDescending(x => x.Id).ToList();

        //    //Check For active contract and Date
        //    foreach (var items in services_)
        //    {
        //        services.AddRange(scheduleQuery.Where(x => x.VehicleResourceId == items.ResourceId && pickupDate >= x.AvailabilityStart && pickupDate <= x.AvailablilityEnd));
        //    }

        //    services.ToList();
        //    //Check For Days

        //    //Check Time
        //    foreach (var items in services)
        //    {
        //        foreach (var item in items.GenericDays)
        //        {
        //            if (pickUpTime.TimeOfDay >= item.OpeningTime.TimeOfDay && pickUpTime.TimeOfDay <= item.ClosingTime.TimeOfDay)
        //            {
        //                if (pickupDate.DayOfWeek.ToString() == "Monday" && item.Monday == true)
        //                {
        //                    //services2.AddRange(services.Where(x => x.VehicleResourceId == items.VehicleResourceId));
        //                    services2.Add(items);
        //                    break;
        //                }
        //                else if (pickupDate.DayOfWeek.ToString() == "Tuesday" && item.Tuesday == true)
        //                {
        //                    services2.Add(items);
        //                    break;
        //                }
        //                else if (pickupDate.DayOfWeek.ToString() == "Wednesday" && item.Wednesday == true)
        //                {
        //                    services2.Add(items);
        //                    break;
        //                }
        //                else if (pickupDate.DayOfWeek.ToString() == "Thursday" && item.Thursday == true)
        //                {
        //                    services2.Add(items);
        //                    break;
        //                }
        //                else if (pickupDate.DayOfWeek.ToString() == "Friday" && item.Friday == true)
        //                {
        //                    services2.Add(items);
        //                    break;
        //                }
        //                else if (pickupDate.DayOfWeek.ToString() == "Saturday" && item.Saturday == true)
        //                {
        //                    services2.Add(items);
        //                    break;
        //                }
        //                else if (pickupDate.DayOfWeek.ToString() == "Sunday" && item.Sunday == true)
        //                {
        //                    services2.Add(items);
        //                    break;
        //                }

        //            }
        //            else
        //            {
        //                continue;
        //            }


        //        }
        //        // services2.AddRange(services.Where(x => x.VehicleResourceId == items.ResourceId));
        //    }




        //    //Arrange based on released date 
        //    return services2.ToList();
        //}
        public async Task<VehicleDTSMasterExtended> FindAllVehicleMastersForAutoAssignmentByPickupDate(long seviceRegId, long RouteId, DateTime pickupDate, DateTime pickUpTime)
        {
            var services = new List<VehicleDTSMaster>();
            var services2 = new List<VehicleDTSMaster>();
            var eligibleVehicles = new List<Vehicle>();
            var services_ = new List<VehicleSMORoutesResourceTie>();
            var services2_ = new List<VehicleSMORoutesResourceTie>();
            var services3_ = new List<VehicleSMORoutesResourceTie>();
            var servicesType_ = new List<VehicleResourceRequiredPerService>();
            var servicesDetail_ = new List<VehicleServiceAssignmentDetail>();
            var query = _context.VehicleSMORoutesResourceTies.Where
                         (ct => ct.SMORouteId == RouteId && ct.IsDeleted == false);
            //Getting all  registered resources
            var getResources = _context.Vehicles.Where(r => r.IsDeleted == false)
            .Include(s => s.SupplierService).Include(t => t.VehicleType)
            .Include(office => office.AttachedOffice).Include(br => br.AttachedBranch)
                                  .ToList();
          
            var getAllResourceDetails = await _serviceAssignmentDetailsRepository.FindAllVehicleServiceAssignmentDetails();
            //var vehicleAssignmentSorted = getAllResourceDetails.Where(x=>x.DateTemporarilyHeld > DateTime.Now).OrderBy(x=>x.DateTemporarilyHeld).ToList();&& pickupDate >= x.RecoveryDateTime
            var vehicleAssignmentSorted = getAllResourceDetails.Where(x => x.IsTemporarilyHeld != true && x.DateTemporarilyHeld != pickupDate && ( pickupDate >= x.RecoveryDateTime || x.RecoveryDateTime == null)).OrderBy(x => x.DateTemporarilyHeld).DistinctBy(y => y.VehicleResourceId).ToList();
            //var vehicleWithAssignment = getAllResourceDetails.Where(x => x.DateTemporarilyHeld.Date == pickupDate.Date);
            var vehicleWithAssignment = getAllResourceDetails.Where(x => x.DateTemporarilyHeld == pickupDate || x.IsTemporarilyHeld == true );

            //var vehiclesWithoutAssignment = getResources.Where(x => vehicleAssignmentSorted.Any(y => y.VehicleResourceId == x.Id)).ToList();
            //var eligibleVehicles = getResources.Where(x => !vehicleWithAssignment.Any(y => y.VehicleResourceId == x.Id)).OrderBy(x=>x.CreatedAt).ToList();
            foreach (var items in getResources)
            {
                var getDetail = await _serviceAssignmentDetailsRepository.FindVehicleServiceAssignmentDetailByResourceId2(items.Id);
                if (getDetail == null)
                {
                    eligibleVehicles.Add(items);
                }

            }

            //check for route
            eligibleVehicles = eligibleVehicles.Where(x => query.Any(y => y.ResourceId == x.Id && y.SMORouteId == RouteId)).OrderBy(x=>x.CreatedAt).ToList();


            //type check
            var getType =  _context.VehicleResourceRequiredPerServices.Where
               (ct => ct.ServiceRegistrationId == seviceRegId && ct.IsDeleted == false).ToList();

            //eligibleVehicles = eligibleVehicles.Where(x => getType.Any(y =>  y.VehicleTypeId == x.VehicleTypeId)).OrderBy(x => x.CreatedAt).DistinctBy(y=>y.VehicleTypeId).ToList();
            eligibleVehicles = eligibleVehicles.Where(x => getType.Any(y =>  y.VehicleTypeId == x.VehicleTypeId)).OrderBy(x => x.CreatedAt).ToList();

            //foreach (var items in eligibleVehicles)
            //{
            //    var typeExists = _serviceRegistrationRepository.GetVehicleResourceApplicableTypeReqById(seviceRegId, items.VehicleTypeId);
            //    if (typeExists == null)
            //    {
            //        eligibleVehicles.Remove(items);
            //        //services_.Add(items);
            //    }

            //}
            //services_.ToList();
            var neverAssigned = eligibleVehicles.ToList();

            var scheduleQuery = _context.VehicleDTSMasters.Where(dts => dts.IsDeleted == false)
            .Include(dts => dts.CreatedBy).Include(dts => dts.VehicleResource).Include(dts => dts.VehicleResource.SupplierService).
            Include(dts => dts.GenericDays.Where(x => x.IsDeleted == false))
            .OrderByDescending(x => x.Id).ToList();

            //Check For active contract and Date
            foreach (var items in eligibleVehicles)
            {
                services.AddRange(scheduleQuery.Where(x => x.VehicleResourceId == items.Id && pickupDate >= x.AvailabilityStart && pickupDate <= x.AvailablilityEnd));
            }

            services.ToList();
            //Check For Days

            //Check Time
            foreach (var items in services)
            {
                foreach (var item in items.GenericDays)
                {
                    if (pickUpTime.TimeOfDay >= item.OpeningTime.TimeOfDay && pickUpTime.TimeOfDay <= item.ClosingTime.TimeOfDay)
                    {
                        if (pickupDate.DayOfWeek.ToString() == "Monday" && item.Monday == true)
                        {
                            //services2.AddRange(services.Where(x => x.VehicleResourceId == items.VehicleResourceId));
                            services2.Add(items);
                            break;
                        }
                        else if (pickupDate.DayOfWeek.ToString() == "Tuesday" && item.Tuesday == true)
                        {
                            services2.Add(items);
                            break;
                        }
                        else if (pickupDate.DayOfWeek.ToString() == "Wednesday" && item.Wednesday == true)
                        {
                            services2.Add(items);
                            break;
                        }
                        else if (pickupDate.DayOfWeek.ToString() == "Thursday" && item.Thursday == true)
                        {
                            services2.Add(items);
                            break;
                        }
                        else if (pickupDate.DayOfWeek.ToString() == "Friday" && item.Friday == true)
                        {
                            services2.Add(items);
                            break;
                        }
                        else if (pickupDate.DayOfWeek.ToString() == "Saturday" && item.Saturday == true)
                        {
                            services2.Add(items);
                            break;
                        }
                        else if (pickupDate.DayOfWeek.ToString() == "Sunday" && item.Sunday == true)
                        {
                            services2.Add(items);
                            break;
                        }

                    }
                    else
                    {
                        continue;
                    }


                }
                // services2.AddRange(services.Where(x => x.VehicleResourceId == items.ResourceId));
            }
            var eligibleVehiclesWithAssignment = services2.Where(x => vehicleAssignmentSorted.Any(y => y.VehicleResourceId == x.VehicleResourceId)).ToList();
            //var eligibleVehiclesWithoutAssignment = services2.Where(x => !vehicleAssignmentSorted.Any(y => y.VehicleResourceId == x.VehicleResourceId));
            var eligibleVehiclesWithoutAssignment = new List<VehicleDTSMaster>();
            var vehicleAssignmentSortedDetails = getAllResourceDetails.DistinctBy(y => y.VehicleResourceId).ToList();
             eligibleVehiclesWithoutAssignment = services2.Where(x => !vehicleAssignmentSortedDetails.Any(y => y.VehicleResourceId == x.VehicleResourceId)).ToList();
            //foreach (var item in services2)
            //{
            //    var getAllResourceDetailsById = vehicleAssignmentSortedDetails.Where(x => x.VehicleResourceId == item.VehicleResourceId).FirstOrDefault();
            //    if (getAllResourceDetailsById == null)
            //    {
            //        eligibleVehiclesWithoutAssignment.Add(item);
            //    }
            //}

            var resultModel = new VehicleDTSMasterExtended()
            {
                eligibleVehiclesWithAssignment = eligibleVehiclesWithAssignment,
                eligibleVehiclesWithoutAssignment = eligibleVehiclesWithoutAssignment
            };

            return resultModel;
        }



        public async Task<ArmedEscortDTSMaster> FindArmedEscortMasterById(long Id)
        {
            return await _context.ArmedEscortDTSMasters.Include(dts => dts.GenericDays.Select(x=>x.OpeningTime))
                .Include(dts=>dts.CreatedBy).Include(dts => dts.ArmedEscortResource).Include(dts => dts.ArmedEscortResource.SupplierService)
                .FirstOrDefaultAsync(aer => aer.Id == Id && aer.IsDeleted == false);
        }

        public async Task<ArmedEscortDTSMaster> FindArmedEscortMasterByResourceId(long resourceId)
        {
            return await _context.ArmedEscortDTSMasters.Include(dts => dts.GenericDays.Where(x => x.IsDeleted == false))
                .Include(dts => dts.CreatedBy).Include(dts => dts.ArmedEscortResource).Include(dts => dts.ArmedEscortResource.SupplierService)
                .FirstOrDefaultAsync(aer => aer.ArmedEscortResourceId == resourceId && aer.AvailablilityEnd >= DateTime.UtcNow &&  aer.IsDeleted == false);
        }

        public async Task<ArmedEscortDTSMaster> FindArmedEscortMasterByResourceId2(long? resourceId)
        {
            return await _context.ArmedEscortDTSMasters.Include(dts => dts.GenericDays.Where(x => x.IsDeleted == false))
                .Include(dts => dts.CreatedBy).Include(dts => dts.ArmedEscortResource).Include(dts => dts.ArmedEscortResource.SupplierService)
                .FirstOrDefaultAsync(aer => aer.ArmedEscortResourceId == resourceId && aer.AvailablilityEnd >= DateTime.UtcNow && aer.IsDeleted == false);
        }

        public async Task<CommanderDTSMaster> FindCommanderMasterById(long Id)
        {
            return await _context.CommanderDTSMasters.Include(dts => dts.GenericDays.Where(x => x.IsDeleted == false))
                .Include(dts => dts.CreatedBy).Include(dts => dts.CommanderResource).Include(dts => dts.CommanderResource.Profile)
                .FirstOrDefaultAsync(aer => aer.Id == Id && aer.IsDeleted == false);
        }

        public async Task<CommanderDTSMaster> FindCommanderMasterByResourceId(long resourceId)
        {
            return await _context.CommanderDTSMasters.Include(dts => dts.GenericDays.Where(x => x.IsDeleted == false))
               .Include(dts => dts.CreatedBy).Include(dts => dts.CommanderResource).Include(dts => dts.CommanderResource.Profile)
               .FirstOrDefaultAsync(aer => aer.CommanderResourceId == resourceId &&  aer.AvailablilityEnd >= DateTime.UtcNow && aer.IsDeleted == false);
        }

        public async Task<CommanderDTSMaster> FindCommanderMasterByResourceId2(long? resourceId)
        {
            return await _context.CommanderDTSMasters.Include(dts => dts.GenericDays.Where(x => x.IsDeleted == false))
              .Include(dts => dts.CreatedBy).Include(dts => dts.CommanderResource).Include(dts => dts.CommanderResource.Profile)
              .FirstOrDefaultAsync(aer => aer.CommanderResourceId == resourceId && aer.AvailablilityEnd >= DateTime.UtcNow && aer.IsDeleted == false);
        }

        public async Task<PilotDTSMaster> FindPilotMasterById(long Id)
        {
            return await _context.PilotDTSMasters.Include(dts => dts.GenericDays.Where(x => x.IsDeleted == false))
               .Include(dts => dts.CreatedBy).Include(dts => dts.PilotResource)
               .FirstOrDefaultAsync(aer => aer.Id == Id && aer.IsDeleted == false);
        }

        public async Task<PilotDTSMaster> FindPilotMasterByResourceId(long resourceId)
        {
            return await _context.PilotDTSMasters.Include(dts => dts.GenericDays.Where(x => x.IsDeleted == false))
             .Include(dts => dts.CreatedBy).Include(dts => dts.PilotResource)
             .FirstOrDefaultAsync(aer => aer.PilotResourceId == resourceId && aer.AvailablilityEnd >= DateTime.UtcNow && aer.IsDeleted == false);
        }

        public async Task<PilotDTSMaster> FindPilotMasterByResourceId2(long? resourceId)
        {
            return await _context.PilotDTSMasters.Include(dts => dts.GenericDays.Where(x => x.IsDeleted == false))
          .Include(dts => dts.CreatedBy).Include(dts => dts.PilotResource)
          .FirstOrDefaultAsync(aer => aer.PilotResourceId == resourceId && aer.AvailablilityEnd >= DateTime.UtcNow && aer.IsDeleted == false);
        }

        public async Task<VehicleDTSMaster> FindVehicleMasterById(long Id)
        {
            return await _context.VehicleDTSMasters.Include(dts => dts.GenericDays.Where(x => x.IsDeleted == false))
              .Include(dts => dts.CreatedBy).Include(dts => dts.VehicleResource).Include(dts => dts.VehicleResource.SupplierService)
              .FirstOrDefaultAsync(aer => aer.Id == Id && aer.IsDeleted == false);
        }

        public async Task<VehicleDTSMaster> FindVehicleMasterByResourceId(long resourceId)
        {
            return await _context.VehicleDTSMasters.Include(dts => dts.GenericDays.Where(x => x.IsDeleted == false))
               .Include(dts => dts.CreatedBy).Include(dts => dts.VehicleResource).Include(dts => dts.VehicleResource.SupplierService)
               .FirstOrDefaultAsync(aer => aer.VehicleResourceId  == resourceId && aer.AvailablilityEnd >= DateTime.Now  && aer.IsDeleted == false);
        }

        public async Task<VehicleDTSMaster> FindVehicleMasterByResourceId2(long? resourceId)
        {
            return await _context.VehicleDTSMasters.Include(dts => dts.GenericDays.Where(x => x.IsDeleted == false))
               .Include(dts => dts.CreatedBy).Include(dts => dts.VehicleResource).Include(dts => dts.VehicleResource.SupplierService)
               .FirstOrDefaultAsync(aer => aer.VehicleResourceId == resourceId && aer.IsDeleted == false);
        }

        public ArmedEscortDTSMaster GetArmedEscortProfileId(long? profileId)
        {
            return _context.ArmedEscortDTSMasters
              .Where(aer => aer.ArmedEscortResourceId == profileId && aer.IsDeleted == false).ToList().LastOrDefault();
            //return _context.ArmedEscortDTSMasters
            //   .LastOrDefault(aer => aer.ArmedEscortResource.Id == profileId && aer.IsDeleted == false);
        }

        public CommanderDTSMaster GetCommandername(string Name)
        {
            throw new NotImplementedException();
        }

        public CommanderDTSMaster GetCommanderProfileId(long? profileId)
        {
            return _context.CommanderDTSMasters
             .Where(aer => aer.CommanderResourceId == profileId && aer.IsDeleted == false).ToList().LastOrDefault();
        }

        public PilotDTSMaster GetPilotname(string Name)
        {
            throw new NotImplementedException();
        }

        public PilotDTSMaster GetPilotProfileId(long? profileId)
        {
            return _context.PilotDTSMasters
             .Where(aer => aer.PilotResourceId == profileId && aer.IsDeleted == false).ToList().LastOrDefault();
        }

        public ArmedEscortDTSMaster GetTypename(string Name)
        {
            throw new NotImplementedException();
        }

        public VehicleDTSMaster GetVehiclename(string Name)
        {
            throw new NotImplementedException();
        }

        public VehicleDTSMaster GetVehicleProfileId(long? profileId)
        {
            return _context.VehicleDTSMasters
             .Where(aer => aer.VehicleResourceId == profileId && aer.IsDeleted == false).ToList().LastOrDefault();
        }

        public async Task<ArmedEscortDTSMaster> SaveArmedEscortMaster(ArmedEscortDTSMaster armedEscort)
        {
            var savedEntity = await _context.ArmedEscortDTSMasters.AddAsync(armedEscort);
            if (await SaveChanges())
            {
                return savedEntity.Entity;
            }
            return null;
        }

        public async Task<CommanderDTSMaster> SaveCommanderMaster(CommanderDTSMaster commander)
        {
            var savedEntity = await _context.CommanderDTSMasters.AddAsync(commander);
            if (await SaveChanges())
            {
                return savedEntity.Entity;
            }
            return null;
        }

        public async Task<PilotDTSMaster> SavePilotMaster(PilotDTSMaster pilot)
        {
            var savedEntity = await _context.PilotDTSMasters.AddAsync(pilot);
            if (await SaveChanges())
            {
                return savedEntity.Entity;
            }
            return null;
        }

        public async Task<VehicleDTSMaster> SaveVehicleMaster(VehicleDTSMaster vehicle)
        {
            var savedEntity = await _context.VehicleDTSMasters.AddAsync(vehicle);
            if (await SaveChanges())
            {
                return savedEntity.Entity;
            }
            return null;
        }

        public async Task<ArmedEscortDTSMaster> UpdateArmedEscortMaster(ArmedEscortDTSMaster armedEscort)
        {
            var updatedEntity = _context.ArmedEscortDTSMasters.Update(armedEscort);
            if (await SaveChanges())
            {
                return updatedEntity.Entity;
            }
            return null;
        }

        public async Task<CommanderDTSMaster> UpdateCommanderMaster(CommanderDTSMaster commander)
        {
            var updatedEntity = _context.CommanderDTSMasters.Update(commander);
            if (await SaveChanges())
            {
                return updatedEntity.Entity;
            }
            return null;
        }

        public async Task<PilotDTSMaster> UpdatePilotMaster(PilotDTSMaster pilot)
        {
            var updatedEntity = _context.PilotDTSMasters.Update(pilot);
            if (await SaveChanges())
            {
                return updatedEntity.Entity;
            }
            return null;
        }

        public async Task<VehicleDTSMaster> UpdatevehicleMaster(VehicleDTSMaster vehicle)
        {
            var updatedEntity = _context.VehicleDTSMasters.Update(vehicle);
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
