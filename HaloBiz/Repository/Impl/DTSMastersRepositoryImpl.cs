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

        public async Task<IEnumerable<ArmedEscortDTSMaster>> FindAllArmedEscortMastersForAutoAssignmentByPickupDate(long seviceRegId, long RouteId, DateTime pickupDate, DateTime pickUpTime)
        {
            var services = new List<ArmedEscortDTSMaster>();
            var services2 = new List<ArmedEscortDTSMaster>();
            var resources = new List<ArmedEscortProfile>();
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
            //Held
            var resourceNotHeld = _context.ArmedEscortServiceAssignmentDetails.Where(type => type.IsDeleted == false && type.IsTemporarilyHeld != true)
             .Include(ct => ct.ArmedEscortResource).Include(t => t.ActionReleaseType).Include(t => t.TempReleaseType).Include(t => t.ServiceAssignment)
             .Include(t => t.TempReleaseType).Include(t => t.CreatedBy).Include(t => t.ArmedEscortResource.ArmedEscortType)
             .OrderByDescending(x => x.CreatedAt);
            //Check For Held
            foreach (var items in getResources)
            {
                //checks if selected item is held in serviceDetail table
                var getDetail = await _serviceAssignmentDetailsRepository.FindEscortServiceAssignmentDetailByResourceId2(items.Id);
                if (getDetail == null)
                {
                    //add to list if not held
                    resources.Add(items);
                }

            }
            resources.OrderBy(x => x.CreatedAt).ToList();


            //check for route
            foreach (var items in resources)
            {
                services3_.AddRange(query.Where(x => x.ResourceId == items.Id));
            }
            services3_.ToList();

            //check for type
            foreach (var items in services3_)
            {
                var typeExists = _serviceRegistrationRepository.GetArmedEscortResourceApplicableTypeReqById(seviceRegId, items.Resource.ArmedEscortTypeId);
                if (typeExists != null)
                {
                    services_.Add(items);
                }
                //servicesType_.AddRange(getTypesByServiceRegid.Where(x => x.VehicleTypeId == items.VehicleResource.VehicleTypeId));
            }
            services_.ToList();

            var scheduleQuery = _context.ArmedEscortDTSMasters.Where(dts => dts.IsDeleted == false && (pickupDate >= dts.AvailabilityStart && pickupDate <= dts.AvailablilityEnd))
            .Include(dts => dts.CreatedBy).Include(dts => dts.ArmedEscortResource).
            Include(dts => dts.GenericDays.Where(x => x.IsDeleted == false))
            .OrderByDescending(x => x.Id);

            //Check For active contrcat and Date 
            foreach (var items in services_)
            {
                services.AddRange(scheduleQuery.Where(x => x.ArmedEscortResourceId == items.ResourceId));
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

            //Arrange based on released date 
            return services2.ToList();
        }

        public async Task<IEnumerable<CommanderDTSMaster>> FindAllCommanderMasters()
        {
            return await _context.CommanderDTSMasters.Where(dts => dts.IsDeleted == false)
            .Include(dts => dts.CreatedBy).Include(dts => dts.GenericDays).Include(dts => dts.CommanderResource).Include(dts=>dts.CommanderResource.Profile)
            .OrderByDescending(x => x.Id)
            .ToListAsync();
        }

        public async Task<IEnumerable<CommanderDTSMaster>> FindAllCommanderMastersForAutoAssignmentByPickupDate(long seviceRegId, long RouteId, DateTime pickupDate, DateTime pickUpTime)
        {

            var services = new List<CommanderDTSMaster>();
            var services2 = new List<CommanderDTSMaster>();
            var resources = new List<CommanderProfile>();
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
            //Held
            var resourceNotHeld = _context.CommanderServiceAssignmentDetails.Where(type => type.IsDeleted == false && type.IsTemporarilyHeld != true)
             .Include(ct => ct.CommanderResource).Include(t => t.ActionReleaseType).Include(t => t.TempReleaseType).Include(t => t.ServiceAssignment)
             .Include(t => t.TempReleaseType).Include(t => t.CreatedBy).Include(t => t.CommanderResource.CommanderType)
             .OrderByDescending(x => x.CreatedAt);
            //Check For Held
            foreach (var items in getResources)
            {
                //checks if selected item is held in serviceDetail table
                var getDetail = await _serviceAssignmentDetailsRepository.FindCommanderServiceAssignmentDetailByResourceId2(items.Id);
                if (getDetail == null)
                {
                    //add to list if not held
                    resources.Add(items);
                }

            }
            resources.OrderBy(x => x.CreatedAt).ToList();


            //check for route
            foreach (var items in resources)
            {
                services3_.AddRange(query.Where(x => x.ResourceId == items.Id));
            }
            services3_.ToList();

            //check for type
            foreach (var items in services3_)
            {
                var typeExists = _serviceRegistrationRepository.GetCommanderResourceApplicableTypeReqById(seviceRegId, items.Resource.CommanderTypeId);
                if (typeExists != null)
                {
                    services_.Add(items);
                }
                //servicesType_.AddRange(getTypesByServiceRegid.Where(x => x.VehicleTypeId == items.VehicleResource.VehicleTypeId));
            }
            services_.ToList();

            var scheduleQuery = _context.CommanderDTSMasters.Where(dts => dts.IsDeleted == false && (pickupDate >= dts.AvailabilityStart && pickupDate <= dts.AvailablilityEnd))
            .Include(dts => dts.CreatedBy).Include(dts => dts.CommanderResource).
            Include(dts => dts.GenericDays.Where(x => x.IsDeleted == false))
            .OrderByDescending(x => x.Id);

            //Check For active contrcat and Date 
            foreach (var items in services_)
            {
                services.AddRange(scheduleQuery.Where(x => x.CommanderResourceId == items.ResourceId));
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

            //Arrange based on released date 
            return services2.ToList();
        }

        public async Task<IEnumerable<PilotDTSMaster>> FindAllPilotMasters()
        {
            return await _context.PilotDTSMasters.Where(dts => dts.IsDeleted == false)
            .Include(dts => dts.CreatedBy).Include(dts => dts.GenericDays).Include(dts => dts.PilotResource)
            .OrderByDescending(x => x.Id)
            .ToListAsync();
        }

        public async Task<IEnumerable<PilotDTSMaster>> FindAllPilotMastersForAutoAssignmentByPickupDate(long seviceRegId, long RouteId, DateTime pickupDate, DateTime pickUpTime)
        {
            var services = new List<PilotDTSMaster>();
            var services2 = new List<PilotDTSMaster>();
            var resources = new List<PilotProfile>();
            var services_ = new List<PilotSMORoutesResourceTie>();
            var services2_ = new List<PilotSMORoutesResourceTie>();
            var services3_ = new List<PilotSMORoutesResourceTie>();
            var servicesType_ = new List<PilotResourceRequiredPerService>();
            //var servicesDetail_ = new List<VehicleServiceAssignmentDetail>();
            var query = _context.PilotSMORoutesResourceTies.Where
                         (ct => ct.SMORouteId == RouteId && ct.IsDeleted == false);
            var getResources = _context.PilotProfiles.Where(r => r.IsDeleted == false)
            .Include(s => s.MeansOfIdentification).Include(t => t.PilotType)
            .Include(office => office.Rank)
                                  .ToList();
            //Held
            var resourceNotHeld = _context.PilotServiceAssignmentDetails.Where(type => type.IsDeleted == false && type.IsTemporarilyHeld != true)
             .Include(ct => ct.PilotResource).Include(t => t.ActionReleaseType).Include(t => t.TempReleaseType).Include(t => t.ServiceAssignment)
             .Include(t => t.TempReleaseType).Include(t => t.CreatedBy).Include(t => t.PilotResource.PilotType)
             .OrderByDescending(x => x.CreatedAt);
            //Check For Held
            foreach (var items in getResources)
            {
                //checks if selected item is held in serviceDetail table
                var getVehicleDetail = await _serviceAssignmentDetailsRepository.FindPilotServiceAssignmentDetailByResourceId2(items.Id);
                if (getVehicleDetail == null)
                {
                    //add to list if not held
                    resources.Add(items);
                }

            }
            resources.OrderBy(x=>x.CreatedAt).ToList();


            //check for route
            foreach (var items in resources)
            {
                services3_.AddRange(query.Where(x => x.ResourceId == items.Id));
            }
            services3_.ToList();

            //check for type
            foreach (var items in services3_)
            {
                var typeExists = _serviceRegistrationRepository.GetPilotResourceApplicableTypeReqById(seviceRegId, items.Resource.PilotTypeId);
                if (typeExists != null)
                {
                    services_.Add(items);
                }
                //servicesType_.AddRange(getTypesByServiceRegid.Where(x => x.VehicleTypeId == items.VehicleResource.VehicleTypeId));
            }
            services_.ToList();

            var scheduleQuery = _context.PilotDTSMasters.Where(dts => dts.IsDeleted == false && (pickupDate >= dts.AvailabilityStart && pickupDate <= dts.AvailablilityEnd))
            .Include(dts => dts.CreatedBy).Include(dts => dts.PilotResource).
            Include(dts => dts.GenericDays.Where(x => x.IsDeleted == false))
            .OrderByDescending(x => x.Id);

            //Check For active Contract and Date
            foreach (var items in services_)
            {
                services.AddRange(scheduleQuery.Where(x => x.PilotResourceId == items.ResourceId));
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

            //Arrange based on released date 
            return services2.ToList();
        }

        public async Task<IEnumerable<VehicleDTSMaster>> FindAllVehicleMasters()
        {
            return await _context.VehicleDTSMasters.Where(dts => dts.IsDeleted == false)
            .Include(dts => dts.CreatedBy).Include(dts=>dts.GenericDays).Include(dts => dts.VehicleResource).Include(dts => dts.VehicleResource.SupplierService)
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

        public async Task<IEnumerable<VehicleDTSMaster>> FindAllVehicleMastersForAutoAssignmentByPickupDate(long seviceRegId, long RouteId, DateTime pickupDate, DateTime pickUpTime)
        {
            var services = new List<VehicleDTSMaster>();
            var services2 = new List<VehicleDTSMaster>();
            var resources = new List<Vehicle>();
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
            //Held
            var resourceNotHeld = _context.VehicleServiceAssignmentDetails.Where(type => type.IsDeleted == false && type.IsTemporarilyHeld != true)
             .Include(ct => ct.VehicleResource).Include(t => t.ActionReleaseType).Include(t => t.TempReleaseType).Include(t => t.ServiceAssignment)
             .Include(t => t.TempReleaseType).Include(t => t.CreatedBy).Include(t => t.VehicleResource.SupplierService).Include(t => t.VehicleResource.VehicleType)
             .OrderByDescending(x => x.CreatedAt);
            //Check For Held
            foreach (var items in getResources)
            {
                var getVehicleDetail = await _serviceAssignmentDetailsRepository.FindVehicleServiceAssignmentDetailByResourceId2(items.Id);
                if(getVehicleDetail == null)
                {
                    resources.Add(items);
                }
                
            }
            resources.OrderBy(x=>x.CreatedAt).ToList();


            //check for route
            foreach (var items in resources)
            {
                services3_.AddRange(query.Where(x => x.ResourceId == items.Id));
            }
            services3_.ToList();

            //type check
            foreach (var items in services3_)
            {
                var typeExists = _serviceRegistrationRepository.GetVehicleResourceApplicableTypeReqById(seviceRegId, items.Resource.VehicleTypeId);
                if (typeExists != null)
                {
                    services_.Add(items);
                }
                //servicesType_.AddRange(getTypesByServiceRegid.Where(x => x.VehicleTypeId == items.VehicleResource.VehicleTypeId));
            }
            services_.ToList();

            var scheduleQuery = _context.VehicleDTSMasters.Where(dts => dts.IsDeleted == false)
            .Include(dts => dts.CreatedBy).Include(dts => dts.VehicleResource).Include(dts => dts.VehicleResource.SupplierService).
            Include(dts => dts.GenericDays.Where(x => x.IsDeleted == false))
            .OrderByDescending(x => x.Id).ToList();
           
            //Check For active contract and Date
            foreach (var items in services_)
            {
                services.AddRange(scheduleQuery.Where(x => x.VehicleResourceId == items.ResourceId && pickupDate>=x.AvailabilityStart && pickupDate<=x.AvailablilityEnd));
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

           


            //Arrange based on released date 
            return services2.ToList();
        }

        //public async Task<IEnumerable<VehicleDTSMaster>> FindAllVehicleMastersForAutoAssignmentByPickupDate(DateTime pickupDate, DateTime pickUpTime)
        //{
        //   
        //    return await _context.VehicleDTSMasters.Where(dts => dts.IsDeleted == false && (pickupDate >= dts.AvailabilityStart && pickupDate <= dts.AvailablilityEnd))
        //    .Include(dts => dts.CreatedBy).Include(dts => dts.VehicleResource).Include(dts => dts.VehicleResource.SupplierService).
        //    Include(dts => dts.GenericDays.Where(x => x.IsDeleted == false ))
        //    .OrderByDescending(x => x.Id)
        //    .ToListAsync();
        //    // var query = _context.VehicleDTSMasters.Where(dts => dts.IsDeleted == false && (pickupDate >= dts.AvailabilityStart && pickupDate <= dts.AvailablilityEnd))
        //    //.Include(dts => dts.CreatedBy).Include(dts => dts.VehicleResource).Include(dts => dts.VehicleResource.SupplierService);
        //    //return await query.Include(dts => dts.GenericDays.Where(x => x.IsDeleted == false && (pickUpTime.TimeOfDay >= x.OpeningTime.TimeOfDay && pickUpTime.TimeOfDay <= x.ClosingTime.TimeOfDay)))
        //    //.OrderByDescending(x => x.Id)
        //    //.ToListAsync();
        //}

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
               .FirstOrDefaultAsync(aer => aer.VehicleResourceId  == resourceId && aer.AvailablilityEnd >= DateTime.UtcNow  && aer.IsDeleted == false);
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
