using AutoMapper;
using Halobiz.Common.DTOs.ApiDTOs;
using HaloBiz.DTOs.GenericResponseDTO;
using HaloBiz.DTOs.ReceivingDTOs;
using HaloBiz.DTOs.TransferDTOs;
using HaloBiz.Helpers;
using HaloBiz.Repository;
using HaloBiz.Repository.Impl;
using HalobizMigrations.Data;
using HalobizMigrations.Models.Armada;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HaloBiz.MyServices.Impl
{
    public class MasterServiceAssignmentServiceImpl: IMasterServiceAssignmentService
    {
        private readonly IServiceAssignmentMasterRepository _serviceAssignmentMasterRepository;
        private readonly IServiceRegistrationRepository _serviceRegistrationRepository;
        private readonly IServiceAssignmentDetailsRepository _serviceAssignmentDetailsRepository;
        private readonly IDTSMastersRepository _dTSMastersRepository;
        private readonly ICustomerDivisionRepository _CustomerDivisionRepo;
        private readonly ICommanderRegistrationRepository _commanderRegistrationRepository;
        private readonly IArmedEscortRegistrationRepository _armedEscortRegistrationRepository;
        private readonly IPilotRegistrationRepository _pilotRegistrationRepository;
        private readonly IVehicleRegistrationRepository _vehicleRegistrationRepository;
        private readonly IServiceAssignmentDetailsService _serviceAssignmentDetailsService;
        private readonly ILogger<ServiceAssignmentMasterRepositoryImpl> _logger;
        private readonly HalobizContext _context;

        private readonly IMapper _mapper;

        public MasterServiceAssignmentServiceImpl(IMapper mapper, IServiceAssignmentMasterRepository serviceAssignmentMasterRepository, 
            IServiceRegistrationRepository serviceRegistrationRepository, IDTSMastersRepository dTSMastersRepository, IServiceAssignmentDetailsRepository serviceAssignmentDetailsRepository, ICustomerDivisionRepository CustomerDivisionRepo,
            ICommanderRegistrationRepository commanderRegistrationRepository, IPilotRegistrationRepository pilotRegistrationRepository, IArmedEscortRegistrationRepository armedEscortRegistrationRepository,
            IVehicleRegistrationRepository vehicleRegistrationRepository, ILogger<ServiceAssignmentMasterRepositoryImpl> logger, IServiceAssignmentDetailsService serviceAssignmentDetailsService, HalobizContext context)
        {
            _mapper = mapper;
            _serviceAssignmentMasterRepository = serviceAssignmentMasterRepository;
            _serviceRegistrationRepository = serviceRegistrationRepository;
            _dTSMastersRepository = dTSMastersRepository;
            _serviceAssignmentDetailsRepository = serviceAssignmentDetailsRepository;
            _CustomerDivisionRepo = CustomerDivisionRepo;
            _armedEscortRegistrationRepository = armedEscortRegistrationRepository;
            _commanderRegistrationRepository = commanderRegistrationRepository;
            _pilotRegistrationRepository = pilotRegistrationRepository;
            _vehicleRegistrationRepository = vehicleRegistrationRepository;
            _logger = logger;
            _context = context;
            _serviceAssignmentDetailsService = serviceAssignmentDetailsService;

        }

        public async Task<ApiCommonResponse> AddMasterAutoServiceAssignment(HttpContext context, MasterServiceAssignmentReceivingDTO masterReceivingDTO)
        {
            var transaction = _context.Database.BeginTransaction();
            var master = _mapper.Map<MasterServiceAssignment>(masterReceivingDTO);
            //var master = new MasterServiceAssignment();
            var secondary = new SecondaryServiceAssignment();
            var vehicle = new VehicleServiceAssignmentDetail();
            var commander = new CommanderServiceAssignmentDetail();
            var armedEscort = new ArmedEscortServiceAssignmentDetail();
            var pilot = new PilotServiceAssignmentDetail();
            //CommanderChecks
          
            
            
            DateTime pickofftime = Convert.ToDateTime(masterReceivingDTO.PickoffTime.AddHours(1));
            pickofftime = pickofftime.AddSeconds(-1 * pickofftime.Second);
            pickofftime = pickofftime.AddMilliseconds(-1 * pickofftime.Millisecond);
            var getRegService = await _serviceRegistrationRepository.FindServiceById(masterReceivingDTO.ServiceRegistrationId);
            long getId = 0;
            long? TiedVehicleId = 0;
            var assignmentId  = new List<long>();
            List<long?> VehicleResourceIdsToTie = new List<long?>();
            List<long?> VehicleResourceIdsToTieComm = new List<long?>();
            //var RouteExistsForVehicle = _vehicleRegistrationRepository.GetAllVehiclesOnRouteByResourceAndRouteId(masterReceivingDTO.SMORouteId);

            try
            {
                master.CreatedById = context.GetLoggedInUserId();
                //master.PickoffTime = pickofftime;
                master.CreatedAt = DateTime.Now;
                master.TripTypeId = 1;
                master.SAExecutionStatus = 0;
                master.AssignmentStatus = "Open";
                var savedRank = await _serviceAssignmentMasterRepository.SaveServiceAssignment(master);

                if (savedRank == null)
                {
                    return CommonResponse.Send(ResponseCodes.FAILURE, null, ResponseMessage.InternalServer500);
                }
                else
                {
                    getId = savedRank.Id;
                    assignmentId.Add(savedRank.Id);
                }

                if (masterReceivingDTO.IsReturnJourney == true)
                {
                    master.Id = 0;
                    master.PickoffLocation = masterReceivingDTO.DropoffLocation;
                    master.DropoffLocation = masterReceivingDTO.PickoffLocation;
                    master.TripTypeId = 2;
                    master.SAExecutionStatus = 0;
                    //master.PickoffTime = pickofftime;
                    master.AssignmentStatus = "open";
                    master.PrimaryTripAssignmentId = getId;
                    master.CreatedById = context.GetLoggedInUserId();
                    master.CreatedAt = DateTime.Now;
                    var savedItem = await _serviceAssignmentMasterRepository.SaveServiceAssignment(master);
                    if (savedItem == null)
                    {
                       
                        return CommonResponse.Send(ResponseCodes.FAILURE, null, ResponseMessage.InternalServer500);
                    }
                }
                if (getId != 0)
                {
                    for (int i = 0; i < masterReceivingDTO.SecondaryServiceRegistrationId.Length; i++)
                    {
                        secondary.Id = 0;
                        secondary.SecondaryServiceRegistrationId = masterReceivingDTO.SecondaryServiceRegistrationId[i];

                        secondary.SecondaryContractServiceId = masterReceivingDTO.ContractServiceId;
                        secondary.ServiceAssignmentId = getId;
                        secondary.CreatedById = context.GetLoggedInUserId();
                        secondary.CreatedAt = DateTime.UtcNow;
                        var savedItem = await _serviceAssignmentMasterRepository.SaveSecondaryServiceAssignment(secondary);
                        if (savedItem == null)
                        {
                            transaction.Rollback();
                            return CommonResponse.Send(ResponseCodes.FAILURE, null, ResponseMessage.InternalServer500);
                        }

                    }

                }
                //For Vehicles
                if (getId > 0)
                {
                    var getVehicleServiceRegistration = await _serviceRegistrationRepository.FindServiceById(masterReceivingDTO.ServiceRegistrationId);
                    var getVehicleDetailNoneHeld = await _serviceAssignmentDetailsRepository.FindAllNoneHeldVehicleServiceAssignmentDetails();
                    //var getVehicleDetailListById = await _serviceAssignmentDetailsRepository.FindAllVehicleServiceAssignmentDetailsByAssignmentId(getId);
                    long? getVehicleResourceId = 0;
                    long? getTypeId = 0;
                    long count = 0;
                    var getResourceTypePerServiceForVehicle = await _serviceRegistrationRepository.FindVehicleResourceByServiceRegId(masterReceivingDTO.ServiceRegistrationId);
                    var RouteExistsForVehicle = _vehicleRegistrationRepository.GetAllVehiclesOnRouteByResourceAndRouteId(masterReceivingDTO.SMORouteId);

                    if (getVehicleServiceRegistration.RequiresVehicle == true)
                    {
                        
                        var getAllResourceSchedule = await _dTSMastersRepository.FindAllVehicleMastersForAutoAssignmentByPickupDate(masterReceivingDTO.ServiceRegistrationId, masterReceivingDTO.SMORouteId, masterReceivingDTO.PickupDate, masterReceivingDTO.PickoffTime);

                        var result = getAllResourceSchedule.eligibleVehiclesWithoutAssignment.Concat(getAllResourceSchedule.eligibleVehiclesWithAssignment).ToList();
                      

                        if (result.Count() >= getVehicleServiceRegistration.VehicleQuantityRequired)
                        {
                            int countSchedule = 0;
                            int resourceCount = 0;
                           
                            
                            //var _lastItem = getAllResourceSchedule.Last();
                                foreach (var schedule in result)
                                {
                                  //var breakOut = false;
                                //var innerLoopBreak = false;
                                 
                                     getVehicleResourceId = schedule.VehicleResourceId;

                                    vehicle.Id = 0;
                                    vehicle.IsTemporarilyHeld = true;
                                    vehicle.DateTemporarilyHeld = DateTime.Now;
                                    //vehicle.IsHeldForAction = true;
                                    //vehicle.DateHeldForAction = DateTime.UtcNow;
                                    vehicle.VehicleResourceId = getVehicleResourceId;
                                    vehicle.RequiredCount = resourceCount++;
                                    vehicle.CreatedById = context.GetLoggedInUserId();
                                    vehicle.CreatedAt = DateTime.Now;
                                    vehicle.ServiceAssignmentId = getId;
                                    var savedItem = await _serviceAssignmentDetailsRepository.SaveVehicleServiceAssignmentdetail(vehicle);
                                    if (savedItem == null)
                                    {
                                        transaction.Rollback();
                                        return CommonResponse.Send(ResponseCodes.FAILURE, null, ResponseMessage.InternalServer500);
                                    }
                                    else
                                    {
                                        VehicleResourceIdsToTie.Add(getVehicleResourceId);
                                        VehicleResourceIdsToTieComm.Add(getVehicleResourceId);
                                        countSchedule++;
                                        if(countSchedule == getVehicleServiceRegistration.VehicleQuantityRequired)
                                        {
                                            //TiedVehicleId = getVehicleResourceId;
                                            break;
                                        }
                                        else
                                        {
                                            continue;
                                        }

                                    }
                                
                                }
                        }
                        else
                        {
                            transaction.Rollback();
                            return CommonResponse.Send(ResponseCodes.FAILURE, null, ResponseMessage.ResourceNotAvailble450Veh);
                        }
                    }

                }

                //For Pilot
                if (getId > 0)
                {
                    var getVehicleServiceRegistration = await _serviceRegistrationRepository.FindServiceById(masterReceivingDTO.ServiceRegistrationId);
                    //var getVehicleDetail = await _serviceAssignmentDetailsRepository.FindAllVehicleServiceAssignmentDetails();
                    //var getVehicleDetailNoneHeld = await _serviceAssignmentDetailsRepository.FindAllNoneHeldVehicleServiceAssignmentDetails();
                    var getVehicleDetailListById = await _serviceAssignmentDetailsRepository.FindAllVehicleServiceAssignmentDetailsByAssignmentId(getId);
                    long? getResourceId = 0;
                    long? getTypeId = 0;
                    long count = 0;
                    var getResourceTypePerServiceForVehicle = await _serviceRegistrationRepository.FindPilotResourceByServiceRegId(masterReceivingDTO.ServiceRegistrationId);
                    //var getResourceTypePerServiceForVehicle = await _serviceRegistrationRepository.FindAllVehicleResourceByServiceRegId(masterReceivingDTO.ServiceRegistrationId);
                    //var getAllResourceSchedule = await _dTSMastersRepository.FindAllVehicleMastersForAutoAssignment();
                    var RouteExists = _pilotRegistrationRepository.GetAllPilotsOnRouteByResourceAndRouteId(masterReceivingDTO.SMORouteId);
                    

                    if (getVehicleServiceRegistration.RequiresPilot == true)
                    {
                        var getAllResourceSchedule = await _dTSMastersRepository.FindAllPilotMastersForAutoAssignmentByPickupDate(masterReceivingDTO.ServiceRegistrationId, masterReceivingDTO.SMORouteId, masterReceivingDTO.PickupDate, masterReceivingDTO.PickoffTime);
                        var result = getAllResourceSchedule.eligiblePilotsWithoutAssignment.Concat(getAllResourceSchedule.eligiblePilotsWithAssignment).ToList();
                        if (result.Count() >= getVehicleServiceRegistration.PilotQuantityRequired)
                        {
                            int countSchedule = 0;
                            int resourceCount = 0;

                            var _lastItem = result.Last();
                            var _lastItemTie = VehicleResourceIdsToTie.Last();
                            foreach (var schedule in result)
                            {
                               
                                getResourceId = schedule.PilotResourceId;
                             

                                //Add
                               
                                pilot.Id = 0;
                                pilot.IsTemporarilyHeld = true;
                                pilot.DateTemporarilyHeld = DateTime.Now;
                                pilot.PilotResourceId = getResourceId;
                                foreach (var item in VehicleResourceIdsToTie)
                                {
                                    if (_lastItemTie.Equals(item))
                                    {
                                        pilot.TiedVehicleResourceId = item;
                                        break;
                                    }
                                    else 
                                    {
                                        pilot.TiedVehicleResourceId = item;
                                        VehicleResourceIdsToTie.Remove(item);
                                        break;
                                    }
                                }
                                //pilot.TiedVehicleResourceId = TiedVehicleId;
                                pilot.RequiredCount = resourceCount++;
                                pilot.CreatedById = context.GetLoggedInUserId();
                                pilot.CreatedAt = DateTime.Now;
                                pilot.ServiceAssignmentId = getId;
                                var savedItem = await _serviceAssignmentDetailsRepository.SavePilotServiceAssignmentdetail(pilot);
                                if (savedItem == null)
                                {
                                    transaction.Rollback();
                                    return CommonResponse.Send(ResponseCodes.FAILURE, null, ResponseMessage.InternalServer500);
                                }
                                else
                                {
                                    //TiedVehicleId = savedItem.Id;
                                    countSchedule++;
                                    if (countSchedule == getVehicleServiceRegistration.PilotQuantityRequired)
                                    {
                                        break;
                                    }
                                    else
                                    {
                                        continue;
                                    }

                                }

                            }


                        }
                        else
                        {
                            transaction.Rollback();
                            return CommonResponse.Send(ResponseCodes.FAILURE, null, ResponseMessage.ResourceNotAvailble450Pi);
                        }
                    }

                }

                //For Commander
                if (getId > 0)
                {
                    var getVehicleServiceRegistration = await _serviceRegistrationRepository.FindServiceById(masterReceivingDTO.ServiceRegistrationId);
                    //var getVehicleDetail = await _serviceAssignmentDetailsRepository.FindAllVehicleServiceAssignmentDetails();
                    //var getVehicleDetailNoneHeld = await _serviceAssignmentDetailsRepository.FindAllNoneHeldVehicleServiceAssignmentDetails();
                    var getVehicleDetailListById = await _serviceAssignmentDetailsRepository.FindAllVehicleServiceAssignmentDetailsByAssignmentId(getId);
                    long? getResourceId = 0;
                    long? getTypeId = 0;
                    long count = 0;
                    var getResourceTypePerServiceForVehicle = await _serviceRegistrationRepository.FindCommanderResourceByServiceRegId(masterReceivingDTO.ServiceRegistrationId);
                    //var getResourceTypePerServiceForVehicle = await _serviceRegistrationRepository.FindAllVehicleResourceByServiceRegId(masterReceivingDTO.ServiceRegistrationId);
                    //var getAllResourceSchedule = await _dTSMastersRepository.FindAllVehicleMastersForAutoAssignment();
                    var RouteExists = _commanderRegistrationRepository.GetAllCommanderssOnRouteByResourceAndRouteId(masterReceivingDTO.SMORouteId);
                   

                    if (getVehicleServiceRegistration.RequiresCommander == true)
                    {
                        var getAllResourceSchedule = await _dTSMastersRepository.FindAllCommanderMastersForAutoAssignmentByPickupDate(masterReceivingDTO.ServiceRegistrationId, masterReceivingDTO.SMORouteId, masterReceivingDTO.PickupDate, masterReceivingDTO.PickoffTime);
                        var result = getAllResourceSchedule.eligibleCommandersWithoutAssignment.Concat(getAllResourceSchedule.eligibleCommandersWithAssignment).ToList();

                        if (result.Count() >= getVehicleServiceRegistration.CommanderQuantityRequired)
                        {
                            int countSchedule = 0;
                            int resourceCount = 0;

                            var _lastItem = result.Last();
                            var _lastItemTie = VehicleResourceIdsToTie.Last();
                            foreach (var schedule in result)
                            {
                                var breakOut = false;
                                getResourceId = schedule.CommanderResourceId;
                                //Add
                              
                                commander.Id = 0;
                                commander.IsTemporarilyHeld = true;
                                commander.DateTemporarilyHeld = DateTime.Now;
                                commander.CommanderResourceId = getResourceId;
                                //commander.TiedVehicleResourceId = TiedVehicleId;
                                foreach (var item in VehicleResourceIdsToTieComm)
                                {
                                    if (_lastItemTie.Equals(item))
                                    {
                                        commander.TiedVehicleResourceId = item;
                                        break;
                                    }
                                    else
                                    {
                                        commander.TiedVehicleResourceId = item;
                                        VehicleResourceIdsToTieComm.Remove(item);
                                        break;
                                    }
                                }
                                commander.RequiredCount = resourceCount++;
                                commander.CreatedById = context.GetLoggedInUserId();
                                commander.CreatedAt = DateTime.UtcNow;
                                commander.ServiceAssignmentId = getId;
                                var savedItem = await _serviceAssignmentDetailsRepository.SaveCommanderServiceAssignmentdetail(commander);
                                if (savedItem == null)
                                {
                                    transaction.Rollback();
                                    return CommonResponse.Send(ResponseCodes.FAILURE, null, ResponseMessage.InternalServer500);
                                }
                                else
                                {
                                    //TiedVehicleId = savedItem.Id;
                                    countSchedule++;
                                    if (countSchedule == getVehicleServiceRegistration.CommanderQuantityRequired)
                                    {
                                        break;
                                    }
                                    else
                                    {
                                        continue;
                                    }

                                }
                            }
                        }
                        else
                        {
                            transaction.Rollback();
                            return CommonResponse.Send(ResponseCodes.FAILURE, null, ResponseMessage.ResourceNotAvailble450Com);
                        }
                    }

                }

                //For ArmedEscort
                if (getId > 0)
                {
                    var getVehicleServiceRegistration = await _serviceRegistrationRepository.FindServiceById(masterReceivingDTO.ServiceRegistrationId);
                  
                    var getVehicleDetailListById = await _serviceAssignmentDetailsRepository.FindAllVehicleServiceAssignmentDetailsByAssignmentId(getId);
                    long? getResourceId = 0;
                    long? getTypeId = 0;
                    long count = 0;
                    //var getResourceTypePerServiceForVehicle = await _serviceRegistrationRepository.FindCommanderResourceByServiceRegId(masterReceivingDTO.ServiceRegistrationId);
                 
                    var RouteExists = _commanderRegistrationRepository.GetAllCommanderssOnRouteByResourceAndRouteId(masterReceivingDTO.SMORouteId);

                    if (getVehicleServiceRegistration.RequiresArmedEscort == true)
                    {
                        var getAllResourceSchedule = await _dTSMastersRepository.FindAllArmedEscortMastersForAutoAssignmentByPickupDate(masterReceivingDTO.ServiceRegistrationId, masterReceivingDTO.SMORouteId, masterReceivingDTO.PickupDate, masterReceivingDTO.PickoffTime);
                        var result = getAllResourceSchedule.eligibleArmedEscortsWithoutAssignment.Concat(getAllResourceSchedule.eligibleArmedEscortsWithAssignment).ToList();

                        if ( result.Count() >= getVehicleServiceRegistration.ArmedEscortQuantityRequired)
                        {
                            int countSchedule = 0;
                            int resourceCount = 0;

                            var _lastItem = result.Last();
                            foreach (var schedule in result)
                            {
                                var breakOut = false;
                                getResourceId = schedule.ArmedEscortResourceId;
                               
                                //Add
                                armedEscort.Id = 0;
                                armedEscort.IsTemporarilyHeld = true;
                                armedEscort.DateTemporarilyHeld = DateTime.Now;
                                armedEscort.ArmedEscortResourceId = getResourceId;
                                armedEscort.RequiredCount = resourceCount++;
                                armedEscort.CreatedById = context.GetLoggedInUserId();
                                armedEscort.CreatedAt = DateTime.UtcNow;
                                armedEscort.ServiceAssignmentId = getId;
                                var savedItem = await _serviceAssignmentDetailsRepository.SaveEscortServiceAssignmentdetail(armedEscort);
                                if (savedItem == null)
                                {
                                    transaction.Rollback();
                                    return CommonResponse.Send(ResponseCodes.FAILURE, null, ResponseMessage.InternalServer500);
                                }
                                else
                                {
                                    //TiedVehicleId = savedItem.Id;
                                    countSchedule++;
                                    if (countSchedule == getVehicleServiceRegistration.ArmedEscortQuantityRequired)
                                    {
                                        break;
                                    }
                                    else
                                    {
                                        continue;
                                    }

                                }
                            }
                        }
                        else
                        {
                            transaction.Rollback();
                            return CommonResponse.Send(ResponseCodes.FAILURE, null, ResponseMessage.ResourceNotAvailble450Arm);
                        }
                    }

                }

            }
            catch (Exception ex)
            {
                transaction.Rollback();
                _logger.LogError(ex.Message);
                return CommonResponse.Send(ResponseCodes.FAILURE, null, ex.Message);
            }

            
            transaction.Commit();
            if(masterReceivingDTO.InhouseAssignment == true)
            {
                //long[] ids = assignmentId.ToArray();
                //for (int i = 0; i < ids.Length; i++)
                //{
                //    await _serviceAssignmentDetailsService.UpdateServiceDetailsHeldForActionAndReadyStatusByAssignmentId(ids[i]);
                //}
                await _serviceAssignmentDetailsService.UpdateServiceDetailsHeldForActionAndReadyStatusByAssignmentId(getId);


            }
            if (masterReceivingDTO.InhouseAssignment == false)
            {
                var itemToUpdate = await _serviceAssignmentMasterRepository.FindServiceAssignmentById(getId);
                if (!await _serviceAssignmentMasterRepository.UpdateisAddedToCartStatus(itemToUpdate))
                {
                    transaction.Rollback();
                    //string resp = "But Added to cart Status couldn't be updated";
                    return CommonResponse.Send(ResponseCodes.FAILURE, null, "Added to cart Status couldn't be updated");
                }
            }
            var typeTransferDTO = _mapper.Map<MasterServiceAssignmentTransferDTO>(master);
            return CommonResponse.Send(ResponseCodes.SUCCESS, typeTransferDTO, "Auto Service Assignment Successful");
        }


        public async Task<ApiCommonResponse> AddMasterServiceAssignment(HttpContext context, MasterServiceAssignmentReceivingDTO masterReceivingDTO)
        {
            var master = _mapper.Map<MasterServiceAssignment>(masterReceivingDTO);
            var secondary = new SecondaryServiceAssignment();
            DateTime pickofftime = Convert.ToDateTime(masterReceivingDTO.PickoffTime.AddHours(1));
            pickofftime = pickofftime.AddSeconds(-1 * pickofftime.Second);
            pickofftime = pickofftime.AddMilliseconds(-1 * pickofftime.Millisecond);
            var getRegService = await _serviceRegistrationRepository.FindServiceById(masterReceivingDTO.ServiceRegistrationId);
            long getId = 0;

            
            master.CreatedById = context.GetLoggedInUserId();
            //master.PickoffTime = pickofftime;
            master.CreatedAt = DateTime.Now;
            master.TripTypeId = 1;
            master.SAExecutionStatus = 0;
            master.AssignmentStatus = "Open";
            if (masterReceivingDTO.InhouseAssignment == false)
            {
                master.IsAddedToCart = true;
            }
            var savedRank = await _serviceAssignmentMasterRepository.SaveServiceAssignment(master);
          
            if (savedRank == null)
            {
                return CommonResponse.Send(ResponseCodes.FAILURE, null, ResponseMessage.InternalServer500);
            }
            else
            {
                getId = savedRank.Id;
            }

            if(masterReceivingDTO.IsReturnJourney == true)
            {
                master.Id = 0;
                master.PickoffLocation = masterReceivingDTO.DropoffLocation;
                master.DropoffLocation = masterReceivingDTO.PickoffLocation;
                master.TripTypeId = 2;
                master.SAExecutionStatus = 0;
                //master.PickoffTime = pickofftime;
                master.AssignmentStatus = "open";
                master.PrimaryTripAssignmentId = getId ;
                master.CreatedById = context.GetLoggedInUserId();
                master.CreatedAt = DateTime.Now;
                 var savedItem = await _serviceAssignmentMasterRepository.SaveServiceAssignment(master);
                if (savedItem == null)
                {
                    return CommonResponse.Send(ResponseCodes.FAILURE, null, ResponseMessage.InternalServer500);
                }
            }
            if (getId !=0)
            {
                for (int i = 0; i < masterReceivingDTO.SecondaryServiceRegistrationId.Length; i++)
                {
                    secondary.Id = 0;
                    secondary.SecondaryServiceRegistrationId = masterReceivingDTO.SecondaryServiceRegistrationId[i];
                   
                        secondary.SecondaryContractServiceId = masterReceivingDTO.ContractServiceId;
                        secondary.ServiceAssignmentId = getId;
                        secondary.CreatedById = context.GetLoggedInUserId();
                        secondary.CreatedAt = DateTime.UtcNow;
                        var savedItem = await _serviceAssignmentMasterRepository.SaveSecondaryServiceAssignment(secondary);
                        if (savedItem == null)
                        {
                            return CommonResponse.Send(ResponseCodes.FAILURE, null, ResponseMessage.InternalServer500);
                        }

                }
               
            }


            var typeTransferDTO = _mapper.Map<MasterServiceAssignmentTransferDTO>(master);
            return CommonResponse.Send(ResponseCodes.SUCCESS, typeTransferDTO, ResponseMessage.Success200);
        }

        

        public async Task<ApiCommonResponse> AddSecondaryServiceAssignment(HttpContext context, SecondaryServiceAssignmentReceivingDTO secondaryReceivingDTO)
        {
            var addItem = _mapper.Map<SecondaryServiceAssignment>(secondaryReceivingDTO);
            //var NameExist = _sMORouteAndRegionRepository.GetRouteName(sMORouteReceivingDTO.RouteName);
            //if (NameExist != null)
            //{
            //    return CommonResponse.Send(ResponseCodes.NO_DATA_AVAILABLE, null, "No record exists"); ;
            //}
            addItem.CreatedById = context.GetLoggedInUserId();
            addItem.IsDeleted = false;
            addItem.CreatedAt = DateTime.UtcNow;
            var savedRank = await _serviceAssignmentMasterRepository.SaveSecondaryServiceAssignment(addItem);

            if (savedRank == null)
            {
                return CommonResponse.Send(ResponseCodes.FAILURE, null, "Some system errors occurred");
            }
            var TransferDTO = _mapper.Map<SecondaryServiceAssignmentTransferDTO>(addItem);
            return CommonResponse.Send(ResponseCodes.SUCCESS, TransferDTO);
        }

        public async Task<ApiCommonResponse> AllocateResourceForScheduledServiceAssignment(HttpContext context, long id)
        {
            var vehicle = new VehicleServiceAssignmentDetail();
            var commander = new CommanderServiceAssignmentDetail();
            var armedEscort = new ArmedEscortServiceAssignmentDetail();
            var pilot = new PilotServiceAssignmentDetail();
            List<long?> VehicleResourceIdsToTie = new List<long?>();
            List<long?> VehicleResourceIdsToTieComm = new List<long?>();

            try
            {
                var transaction = _context.Database.BeginTransaction();
                var itemToUpdate = await _serviceAssignmentMasterRepository.FindServiceAssignmentById(id);
                if (itemToUpdate == null)
                {
                    return CommonResponse.Send(ResponseCodes.NO_DATA_AVAILABLE);
                }
                //For Vehicle
                if (id > 0)
                {
                    var getVehicleServiceRegistration = await _serviceRegistrationRepository.FindServiceById((long)itemToUpdate.ServiceRegistrationId);
                    //var getVehicleDetailNoneHeld = await _serviceAssignmentDetailsRepository.FindAllNoneHeldVehicleServiceAssignmentDetails();
                    //var getVehicleDetailListById = await _serviceAssignmentDetailsRepository.FindAllVehicleServiceAssignmentDetailsByAssignmentId(getId);
                    long? getVehicleResourceId = 0;
                    long? getTypeId = 0;
                    long count = 0;
                    var getResourceTypePerServiceForVehicle = await _serviceRegistrationRepository.FindVehicleResourceByServiceRegId((long)itemToUpdate.ServiceRegistrationId);
                    var RouteExistsForVehicle = _vehicleRegistrationRepository.GetAllVehiclesOnRouteByResourceAndRouteId(itemToUpdate.SMORouteId);

                    if (getVehicleServiceRegistration.RequiresVehicle == true)
                    {

                        var getAllResourceSchedule = await _dTSMastersRepository.FindAllVehicleMastersForAutoAssignmentByPickupDate((long)itemToUpdate.ServiceRegistrationId, (long)itemToUpdate.SMORouteId, itemToUpdate.PickupDate, itemToUpdate.PickoffTime);

                        var result = getAllResourceSchedule.eligibleVehiclesWithoutAssignment.Concat(getAllResourceSchedule.eligibleVehiclesWithAssignment).ToList();


                        if (result.Count() >= getVehicleServiceRegistration.VehicleQuantityRequired)
                        {
                            int countSchedule = 0;
                            int resourceCount = 0;


                            //var _lastItem = getAllResourceSchedule.Last();
                            foreach (var schedule in result)
                            {
                                //var breakOut = false;
                                //var innerLoopBreak = false;

                                getVehicleResourceId = schedule.VehicleResourceId;

                                vehicle.Id = 0;
                                vehicle.IsTemporarilyHeld = true;
                                vehicle.DateTemporarilyHeld = DateTime.UtcNow;
                                //vehicle.IsHeldForAction = true;
                                //vehicle.DateHeldForAction = DateTime.UtcNow;
                                vehicle.VehicleResourceId = getVehicleResourceId;
                                vehicle.RequiredCount = resourceCount++;
                                vehicle.CreatedById = context.GetLoggedInUserId();
                                vehicle.CreatedAt = DateTime.Now;
                                vehicle.ServiceAssignmentId = id;
                                var savedItem = await _serviceAssignmentDetailsRepository.SaveVehicleServiceAssignmentdetail(vehicle);
                                if (savedItem == null)
                                {
                                    transaction.Rollback();
                                    return CommonResponse.Send(ResponseCodes.FAILURE, null, ResponseMessage.InternalServer500);
                                }
                                else
                                {
                                    VehicleResourceIdsToTie.Add(getVehicleResourceId);
                                    VehicleResourceIdsToTieComm.Add(getVehicleResourceId);
                                    countSchedule++;
                                    if (countSchedule == getVehicleServiceRegistration.VehicleQuantityRequired)
                                    {
                                        //TiedVehicleId = getVehicleResourceId;
                                        break;
                                    }
                                    else
                                    {
                                        continue;
                                    }

                                }

                            }
                        }
                        else
                        {
                            transaction.Rollback();
                            return CommonResponse.Send(ResponseCodes.FAILURE, null, ResponseMessage.ResourceNotAvailble450Veh);
                        }
                    }

                }

                //For Pilot
                if (id > 0)
                {
                    var getVehicleServiceRegistration = await _serviceRegistrationRepository.FindServiceById((long)itemToUpdate.ServiceRegistrationId);
                    //var getVehicleDetail = await _serviceAssignmentDetailsRepository.FindAllVehicleServiceAssignmentDetails();
                    //var getVehicleDetailNoneHeld = await _serviceAssignmentDetailsRepository.FindAllNoneHeldVehicleServiceAssignmentDetails();
                    var getVehicleDetailListById = await _serviceAssignmentDetailsRepository.FindAllVehicleServiceAssignmentDetailsByAssignmentId(id);
                    long? getResourceId = 0;
                    long? getTypeId = 0;
                    long count = 0;
                    var getResourceTypePerServiceForVehicle = await _serviceRegistrationRepository.FindPilotResourceByServiceRegId((long)itemToUpdate.ServiceRegistrationId);
                    //var getResourceTypePerServiceForVehicle = await _serviceRegistrationRepository.FindAllVehicleResourceByServiceRegId(itemToUpdate.ServiceRegistrationId);
                    //var getAllResourceSchedule = await _dTSMastersRepository.FindAllVehicleMastersForAutoAssignment();
                    var RouteExists = _pilotRegistrationRepository.GetAllPilotsOnRouteByResourceAndRouteId(itemToUpdate.SMORouteId);


                    if (getVehicleServiceRegistration.RequiresPilot == true)
                    {
                        var getAllResourceSchedule = await _dTSMastersRepository.FindAllPilotMastersForAutoAssignmentByPickupDate((long)itemToUpdate.ServiceRegistrationId, (long)itemToUpdate.SMORouteId, itemToUpdate.PickupDate, itemToUpdate.PickoffTime);
                        var result = getAllResourceSchedule.eligiblePilotsWithoutAssignment.Concat(getAllResourceSchedule.eligiblePilotsWithAssignment).ToList();
                        if (result.Count() >= getVehicleServiceRegistration.PilotQuantityRequired)
                        {
                            int countSchedule = 0;
                            int resourceCount = 0;

                            var _lastItem = result.Last();
                            var _lastItemTie = VehicleResourceIdsToTie.Last();
                            foreach (var schedule in result)
                            {

                                getResourceId = schedule.PilotResourceId;


                                //Add

                                pilot.Id = 0;
                                pilot.IsTemporarilyHeld = true;
                                pilot.DateTemporarilyHeld = DateTime.UtcNow;
                                pilot.PilotResourceId = getResourceId;
                                foreach (var item in VehicleResourceIdsToTie)
                                {
                                    if (_lastItemTie.Equals(item))
                                    {
                                        pilot.TiedVehicleResourceId = item;
                                        break;
                                    }
                                    else
                                    {
                                        pilot.TiedVehicleResourceId = item;
                                        VehicleResourceIdsToTie.Remove(item);
                                        break;
                                    }
                                }
                                //pilot.TiedVehicleResourceId = TiedVehicleId;
                                pilot.RequiredCount = resourceCount++;
                                pilot.CreatedById = context.GetLoggedInUserId();
                                pilot.CreatedAt = DateTime.Now;
                                pilot.ServiceAssignmentId = id;
                                var savedItem = await _serviceAssignmentDetailsRepository.SavePilotServiceAssignmentdetail(pilot);
                                if (savedItem == null)
                                {
                                    transaction.Rollback();
                                    return CommonResponse.Send(ResponseCodes.FAILURE, null, ResponseMessage.InternalServer500);
                                }
                                else
                                {
                                    //TiedVehicleId = savedItem.Id;
                                    countSchedule++;
                                    if (countSchedule == getVehicleServiceRegistration.PilotQuantityRequired)
                                    {
                                        break;
                                    }
                                    else
                                    {
                                        continue;
                                    }

                                }

                            }


                        }
                        else
                        {
                            transaction.Rollback();
                            return CommonResponse.Send(ResponseCodes.FAILURE, null, ResponseMessage.ResourceNotAvailble450Pi);
                        }
                    }

                }

                //For Commander
                if (id > 0)
                {
                    var getVehicleServiceRegistration = await _serviceRegistrationRepository.FindServiceById((long)itemToUpdate.ServiceRegistrationId);
                    //var getVehicleDetail = await _serviceAssignmentDetailsRepository.FindAllVehicleServiceAssignmentDetails();
                    //var getVehicleDetailNoneHeld = await _serviceAssignmentDetailsRepository.FindAllNoneHeldVehicleServiceAssignmentDetails();
                    var getVehicleDetailListById = await _serviceAssignmentDetailsRepository.FindAllVehicleServiceAssignmentDetailsByAssignmentId(id);
                    long? getResourceId = 0;
                    long? getTypeId = 0;
                    long count = 0;
                    var getResourceTypePerServiceForVehicle = await _serviceRegistrationRepository.FindCommanderResourceByServiceRegId((long)itemToUpdate.ServiceRegistrationId);
                    //var getResourceTypePerServiceForVehicle = await _serviceRegistrationRepository.FindAllVehicleResourceByServiceRegId(itemToUpdate.ServiceRegistrationId);
                    //var getAllResourceSchedule = await _dTSMastersRepository.FindAllVehicleMastersForAutoAssignment();
                    var RouteExists = _commanderRegistrationRepository.GetAllCommanderssOnRouteByResourceAndRouteId(itemToUpdate.SMORouteId);


                    if (getVehicleServiceRegistration.RequiresCommander == true)
                    {
                        var getAllResourceSchedule = await _dTSMastersRepository.FindAllCommanderMastersForAutoAssignmentByPickupDate((long)itemToUpdate.ServiceRegistrationId, (long)itemToUpdate.SMORouteId, itemToUpdate.PickupDate, itemToUpdate.PickoffTime);
                        var result = getAllResourceSchedule.eligibleCommandersWithoutAssignment.Concat(getAllResourceSchedule.eligibleCommandersWithAssignment).ToList();

                        if (result.Count() >= getVehicleServiceRegistration.CommanderQuantityRequired)
                        {
                            int countSchedule = 0;
                            int resourceCount = 0;

                            var _lastItem = result.Last();
                            var _lastItemTie = VehicleResourceIdsToTie.Last();
                            foreach (var schedule in result)
                            {
                                var breakOut = false;
                                getResourceId = schedule.CommanderResourceId;
                                //Add

                                commander.Id = 0;
                                commander.IsTemporarilyHeld = true;
                                commander.DateTemporarilyHeld = DateTime.UtcNow;
                                commander.CommanderResourceId = getResourceId;
                                //commander.TiedVehicleResourceId = TiedVehicleId;
                                foreach (var item in VehicleResourceIdsToTieComm)
                                {
                                    if (_lastItemTie.Equals(item))
                                    {
                                        commander.TiedVehicleResourceId = item;
                                        break;
                                    }
                                    else
                                    {
                                        commander.TiedVehicleResourceId = item;
                                        VehicleResourceIdsToTieComm.Remove(item);
                                        break;
                                    }
                                }
                                commander.RequiredCount = resourceCount++;
                                commander.CreatedById = context.GetLoggedInUserId();
                                commander.CreatedAt = DateTime.UtcNow;
                                commander.ServiceAssignmentId = id;
                                var savedItem = await _serviceAssignmentDetailsRepository.SaveCommanderServiceAssignmentdetail(commander);
                                if (savedItem == null)
                                {
                                    transaction.Rollback();
                                    return CommonResponse.Send(ResponseCodes.FAILURE, null, ResponseMessage.InternalServer500);
                                }
                                else
                                {
                                    //TiedVehicleId = savedItem.Id;
                                    countSchedule++;
                                    if (countSchedule == getVehicleServiceRegistration.CommanderQuantityRequired)
                                    {
                                        break;
                                    }
                                    else
                                    {
                                        continue;
                                    }

                                }
                            }
                        }
                        else
                        {
                            transaction.Rollback();
                            return CommonResponse.Send(ResponseCodes.FAILURE, null, ResponseMessage.ResourceNotAvailble450Com);
                        }
                    }

                }

                //For ArmedEscort
                if (id > 0)
                {
                    var getVehicleServiceRegistration = await _serviceRegistrationRepository.FindServiceById((long)itemToUpdate.ServiceRegistrationId);

                    var getVehicleDetailListById = await _serviceAssignmentDetailsRepository.FindAllVehicleServiceAssignmentDetailsByAssignmentId(id);
                    long? getResourceId = 0;
                    long? getTypeId = 0;
                    long count = 0;
                    //var getResourceTypePerServiceForVehicle = await _serviceRegistrationRepository.FindCommanderResourceByServiceRegId(itemToUpdate.ServiceRegistrationId);

                    var RouteExists = _commanderRegistrationRepository.GetAllCommanderssOnRouteByResourceAndRouteId(itemToUpdate.SMORouteId);

                    if (getVehicleServiceRegistration.RequiresArmedEscort == true)
                    {
                        var getAllResourceSchedule = await _dTSMastersRepository.FindAllArmedEscortMastersForAutoAssignmentByPickupDate((long)itemToUpdate.ServiceRegistrationId, (long)itemToUpdate.SMORouteId, itemToUpdate.PickupDate, itemToUpdate.PickoffTime);
                        var result = getAllResourceSchedule.eligibleArmedEscortsWithoutAssignment.Concat(getAllResourceSchedule.eligibleArmedEscortsWithAssignment).ToList();

                        if (result.Count() >= getVehicleServiceRegistration.ArmedEscortQuantityRequired)
                        {
                            int countSchedule = 0;
                            int resourceCount = 0;

                            var _lastItem = result.Last();
                            foreach (var schedule in result)
                            {
                                var breakOut = false;
                                getResourceId = schedule.ArmedEscortResourceId;

                                //Add
                                armedEscort.Id = 0;
                                armedEscort.IsTemporarilyHeld = true;
                                armedEscort.DateTemporarilyHeld = DateTime.UtcNow;
                                armedEscort.ArmedEscortResourceId = getResourceId;
                                armedEscort.RequiredCount = resourceCount++;
                                armedEscort.CreatedById = context.GetLoggedInUserId();
                                armedEscort.CreatedAt = DateTime.UtcNow;
                                armedEscort.ServiceAssignmentId = id;
                                var savedItem = await _serviceAssignmentDetailsRepository.SaveEscortServiceAssignmentdetail(armedEscort);
                                if (savedItem == null)
                                {
                                    transaction.Rollback();
                                    return CommonResponse.Send(ResponseCodes.FAILURE, null, ResponseMessage.InternalServer500);
                                }
                                else
                                {
                                    //TiedVehicleId = savedItem.Id;
                                    countSchedule++;
                                    if (countSchedule == getVehicleServiceRegistration.ArmedEscortQuantityRequired)
                                    {
                                        break;
                                    }
                                    else
                                    {
                                        continue;
                                    }

                                }
                            }
                        }
                        else
                        {
                            transaction.Rollback();
                            return CommonResponse.Send(ResponseCodes.FAILURE, null, ResponseMessage.ResourceNotAvailble450Arm);
                        }
                    }

                }

              

                itemToUpdate.UpdatedAt = DateTime.Now;
                itemToUpdate.IsScheduled = false;
                var updatedItem = await _serviceAssignmentMasterRepository.UpdateServiceAssignment(itemToUpdate);
                if (updatedItem == null)
                {
                    transaction.Rollback();
                    return CommonResponse.Send(ResponseCodes.FAILURE, null, ResponseMessage.InternalServer500);
                }
                transaction.Commit();
               
                    await _serviceAssignmentDetailsService.UpdateServiceDetailsHeldForActionAndReadyStatusByAssignmentId(id);
                

                //var TransferDTOs = _mapper.Map<MasterServiceAssignmentTransferDTO>(updatedItem);

                return CommonResponse.Send(ResponseCodes.SUCCESS, "Resource Allocation Successful", ResponseMessage.Success200);
            }
            catch (Exception ex)
            {
                return CommonResponse.Send(ResponseCodes.FAILURE, null, ex.StackTrace);
            } 
        }

        public async Task<ApiCommonResponse> DeleteMasterServiceAssignment(long id)
        {

            var itemToDelete = await _serviceAssignmentMasterRepository.FindServiceAssignmentById(id);
            var escortToDelete = await _serviceAssignmentDetailsRepository.FindAllEscortServiceAssignmentDetailsByAssignmentId(id);
            var commanderToDelete = await _serviceAssignmentDetailsRepository.FindAllCommanderServiceAssignmentDetailsByAssignmentId(id);
            var pilotToDelete = await _serviceAssignmentDetailsRepository.FindAllPilotServiceAssignmentDetailsByAssignmentId(id);
            var vehicleToDelete = await _serviceAssignmentDetailsRepository.FindAllVehicleServiceAssignmentDetailsByAssignmentId(id);
            var passengerToDelete = await _serviceAssignmentDetailsRepository.FindAllPassengersByAssignmentId(id);
            var secondaryAssignmentToDelete = await _serviceAssignmentMasterRepository.FindAllSecondaryServiceAssignmentsByAssignmentId(id);

            if (itemToDelete == null)
            {
                return CommonResponse.Send(ResponseCodes.NO_DATA_AVAILABLE);
            }

            if (!await _serviceAssignmentMasterRepository.DeleteServiceAssignment(itemToDelete))
            {
                return CommonResponse.Send(ResponseCodes.FAILURE, null, ResponseMessage.InternalServer500);
            }
            if (escortToDelete.Count() != 0)
            {
                foreach (var item in escortToDelete)
                {
                    await _serviceAssignmentDetailsRepository.DeleteEscortServiceAssignmentDetail(item);
                }
                
            }
            if (commanderToDelete.Count() != 0)
            {
                foreach (var item in commanderToDelete)
                {
                    await _serviceAssignmentDetailsRepository.DeleteCommanderServiceAssignmentDetail(item);
                }
            }
            if (pilotToDelete.Count() != 0)
            {
                foreach (var item in pilotToDelete)
                {
                    await _serviceAssignmentDetailsRepository.DeletePilotServiceAssignmentDetail(item);
                }
            }
            if (vehicleToDelete.Count() != 0)
            {
                foreach (var item in vehicleToDelete)
                {
                    await _serviceAssignmentDetailsRepository.DeleteVehicleServiceAssignmentDetail(item);
                }
            }
            if (secondaryAssignmentToDelete.Count() != 0)
            {
                foreach (var item in secondaryAssignmentToDelete)
                {
                    await _serviceAssignmentMasterRepository.DeleteSecondaryServiceAssignment(item);
                }
            }
            if (passengerToDelete.Count() != 0)
            {
                foreach (var item in passengerToDelete)
                {
                    await _serviceAssignmentDetailsRepository.DeletePassenger(item);
                }
            }

            return CommonResponse.Send(ResponseCodes.SUCCESS, null, ResponseMessage.Success200);
        }

        public async Task<ApiCommonResponse> DeleteMasterServiceAssignmentSchedule(long id)
        {
            var itemToDelete = await _serviceAssignmentMasterRepository.FindServiceAssignmentById(id);

            if (itemToDelete == null)
            {
                return CommonResponse.Send(ResponseCodes.NO_DATA_AVAILABLE); ;
            }

            if (!await _serviceAssignmentMasterRepository.DeleteServiceAssignment(itemToDelete))
            {
                return CommonResponse.Send(ResponseCodes.FAILURE, null, "Some system errors occurred");
            }

            return CommonResponse.Send(ResponseCodes.SUCCESS);
        }

        public async Task<ApiCommonResponse> DeleteSecondaryServiceAssignment(long id)
        {
            var itemToDelete = await _serviceAssignmentMasterRepository.FindSecondaryServiceAssignmentById(id);

            if (itemToDelete == null)
            {
                return CommonResponse.Send(ResponseCodes.NO_DATA_AVAILABLE); ;
            }

            if (!await _serviceAssignmentMasterRepository.DeleteSecondaryServiceAssignment(itemToDelete))
            {
                return CommonResponse.Send(ResponseCodes.FAILURE, null, "Some system errors occurred");
            }

            return CommonResponse.Send(ResponseCodes.SUCCESS);
        }

        public async Task<ApiCommonResponse> GetAllCompletedTripsByClientId(long clientId)
        {
            var master = await _serviceAssignmentMasterRepository.FindAllCompletedTripsByClientId(clientId);
            if (master == null)
            {
                return CommonResponse.Send(ResponseCodes.NO_DATA_AVAILABLE);
            }
            var TransferDTO = _mapper.Map<IEnumerable<MasterServiceAssignmentTransferDTO>>(master);
            return CommonResponse.Send(ResponseCodes.SUCCESS, TransferDTO, ResponseMessage.Success200);
        }

        public async Task<ApiCommonResponse> GetAllCompletedTripsCountByClientId(long clientId)
        {
            var master = await _serviceAssignmentMasterRepository.FindAllCompletedTripsCountByClientId(clientId);
            if (master == null)
            {
                return CommonResponse.Send(ResponseCodes.NO_DATA_AVAILABLE);
            }
            return CommonResponse.Send(ResponseCodes.SUCCESS, master.Count(), ResponseMessage.Success200);
        }

        public async Task<ApiCommonResponse> GetAllFrequentRouteCountByClientId(long clientId)
        {
            var master = await _serviceAssignmentMasterRepository.FindAllFrequentRoutesCountByClientId(clientId);
            if (master == null)
            {
                return CommonResponse.Send(ResponseCodes.NO_DATA_AVAILABLE);
            }
            return CommonResponse.Send(ResponseCodes.SUCCESS, master.Count(), ResponseMessage.Success200);
        }

        

        public async Task<ApiCommonResponse> GetAllCustomerDivisions()
        {
            var CustomerDivisions = await _serviceAssignmentMasterRepository.FindAllCustomerDivision();
            if (CustomerDivisions == null)
            {
                return CommonResponse.Send(ResponseCodes.NO_DATA_AVAILABLE); ;
            }
            
            // var CustomerDivisionTransferDTOs = _mapper.Map<IEnumerable<CustomerDivisionTransferDTO>>(CustomerDivisions);
            return CommonResponse.Send(ResponseCodes.SUCCESS, CustomerDivisions);
        }

        public async Task<ApiCommonResponse> GetAllMasterServiceAssignments()
        {
            var master = await _serviceAssignmentMasterRepository.FindAllServiceAssignments();
            if (master == null)
            {
                return CommonResponse.Send(ResponseCodes.NO_DATA_AVAILABLE);
            }
            var TransferDTO = _mapper.Map<IEnumerable<MasterServiceAssignmentTransferDTO>>(master);
            return CommonResponse.Send(ResponseCodes.SUCCESS, TransferDTO, ResponseMessage.Success200);
        }

        public async Task<ApiCommonResponse> GetAllMasterServiceAssignmentsByClientId(long clientId)
        {
            var master = await _serviceAssignmentMasterRepository.FindAllServiceAssignmentsByClientId(clientId);
            if (master == null)
            {
                return CommonResponse.Send(ResponseCodes.NO_DATA_AVAILABLE);
            }
            //var TransferDTO = _mapper.Map<IEnumerable<MasterServiceAssignmentTransferDTO>>(master);
            //var TransferDTO = _mapper.Map<IEnumerable<MasterServiceAssignmentWithRegisterTransferDTO>>(master);
            return CommonResponse.Send(ResponseCodes.SUCCESS, master, ResponseMessage.Success200);
        }

        public async Task<ApiCommonResponse> GetAllMasterServiceAssignmentsForScheduleCartByClientId(long clientId)
        {
            var master = await _serviceAssignmentMasterRepository.FindAllServiceAssignmentsForScheduleCartByClientId(clientId);
            if (master == null)
            {
                return CommonResponse.Send(ResponseCodes.NO_DATA_AVAILABLE);
            }
            //var TransferDTO = _mapper.Map<IEnumerable<MasterServiceAssignmentTransferDTO>>(master);
            return CommonResponse.Send(ResponseCodes.SUCCESS, master, ResponseMessage.Success200);
        }

        public async Task<ApiCommonResponse> GetAllScheduledMasterServiceAssignments()
        {
            var master = await _serviceAssignmentMasterRepository.FindAllScheduledServiceAssignments();
            if (master == null)
            {
                return CommonResponse.Send(ResponseCodes.NO_DATA_AVAILABLE);
            }
            var TransferDTO = _mapper.Map<IEnumerable<MasterServiceAssignmentTransferDTO>>(master);
            return CommonResponse.Send(ResponseCodes.SUCCESS, TransferDTO, ResponseMessage.Success200);
        }

        public Task<ApiCommonResponse> GetAllSecondaryServiceAssignments()
        {
            throw new NotImplementedException();
        }

        public async Task<ApiCommonResponse> GetMasterServiceAssignmentById(long id)
        {
            var master = await _serviceAssignmentMasterRepository.FindServiceAssignmentById(id);
            if (master == null)
            {
                return CommonResponse.Send(ResponseCodes.NO_DATA_AVAILABLE);
            }
            var TransferDTO = _mapper.Map<MasterServiceAssignmentTransferDTO>(master);
            return CommonResponse.Send(ResponseCodes.SUCCESS, TransferDTO, ResponseMessage.Success200);
        }

        public Task<ApiCommonResponse> GetsecondaryServiceAssignmentById(long id)
        {
            throw new NotImplementedException();
        }
        
        public async Task<ApiCommonResponse> UpdateisAddedToCartStatus(long id)
        {
            var itemToUpdate = await _serviceAssignmentMasterRepository.FindServiceAssignmentById(id);

            if (itemToUpdate == null)
            {
                return CommonResponse.Send(ResponseCodes.NO_DATA_AVAILABLE);
            }

            if (!await _serviceAssignmentMasterRepository.UpdateisAddedToCartStatus(itemToUpdate))
            {
                return CommonResponse.Send(ResponseCodes.FAILURE, null, ResponseMessage.InternalServer500);
            }

            return CommonResponse.Send(ResponseCodes.SUCCESS, null, ResponseMessage.Success200);
        }

        public async Task<ApiCommonResponse> UpdatehasPassengerStatusToTrue(long id)
        {
            var itemToUpdate = await _serviceAssignmentMasterRepository.FindServiceAssignmentById(id);

            if (itemToUpdate == null)
            {
                return CommonResponse.Send(ResponseCodes.NO_DATA_AVAILABLE);
            }

            if (!await _serviceAssignmentMasterRepository.UpdatehasPassengerStatusToTrue(itemToUpdate))
            {
                return CommonResponse.Send(ResponseCodes.FAILURE, null, ResponseMessage.InternalServer500);
            }

            return CommonResponse.Send(ResponseCodes.SUCCESS, null, ResponseMessage.Success200);
        }

        public async Task<ApiCommonResponse> UpdatehasPassengerStatusToFalse(long id)
        {
            var itemToUpdate = await _serviceAssignmentMasterRepository.FindServiceAssignmentById(id);

            if (itemToUpdate == null)
            {
                return CommonResponse.Send(ResponseCodes.NO_DATA_AVAILABLE);
            }

            if (!await _serviceAssignmentMasterRepository.UpdatehasPassengerStatusToFalse(itemToUpdate))
            {
                return CommonResponse.Send(ResponseCodes.FAILURE, null, ResponseMessage.InternalServer500);
            }

            return CommonResponse.Send(ResponseCodes.SUCCESS, null, ResponseMessage.Success200);
        }

        public async Task<ApiCommonResponse> UpdateisPaidForStatus(long id)
        {
            var itemToUpdate = await _serviceAssignmentMasterRepository.FindServiceAssignmentById(id);

            if (itemToUpdate == null)
            {
                return CommonResponse.Send(ResponseCodes.NO_DATA_AVAILABLE);
            }

            if (!await _serviceAssignmentMasterRepository.UpdateisPaidForStatus(itemToUpdate))
            {
                return CommonResponse.Send(ResponseCodes.FAILURE, null, ResponseMessage.InternalServer500);
            }

            return CommonResponse.Send(ResponseCodes.SUCCESS, null, ResponseMessage.Success200);
        }

        public async Task<ApiCommonResponse> UpdateMasterServiceAssignment(HttpContext context, long id, MasterServiceAssignmentReceivingDTO masterReceivingDTO)
        {
            var itemToUpdate = await _serviceAssignmentMasterRepository.FindServiceAssignmentById(id);
            if (itemToUpdate == null)
            {
                return CommonResponse.Send(ResponseCodes.NO_DATA_AVAILABLE);
            }

            var summary = $"Initial details before change, \n {itemToUpdate.ToString()} \n";

            itemToUpdate.DropoffLocation = masterReceivingDTO.DropoffLocation;
            itemToUpdate.PickoffLocation = masterReceivingDTO.PickoffLocation;
            itemToUpdate.ServiceRegistrationId = masterReceivingDTO.ServiceRegistrationId;

            itemToUpdate.PickoffTime = masterReceivingDTO.PickoffTime;
            itemToUpdate.CustomerDivisionId = masterReceivingDTO.CustomerDivisionId;
            //itemToUpdate.TripTypeId = masterReceivingDTO.TripTypeId;
            itemToUpdate.SMORouteId = masterReceivingDTO.SMORouteId;
            itemToUpdate.SMORegionId = masterReceivingDTO.SMORegionId;
            itemToUpdate.UpdatedAt = DateTime.UtcNow;
            var updatedItem = await _serviceAssignmentMasterRepository.UpdateServiceAssignment(itemToUpdate);

            summary += $"Details after change, \n {updatedItem.ToString()} \n";

            if (updatedItem == null)
            {
                return CommonResponse.Send(ResponseCodes.FAILURE, null, ResponseMessage.InternalServer500);
            }

            var TransferDTOs = _mapper.Map<MasterServiceAssignmentTransferDTO>(updatedItem);
            return CommonResponse.Send(ResponseCodes.SUCCESS, TransferDTOs, ResponseMessage.Success200);
        }

        public async Task<ApiCommonResponse> UpdateReadyStatus(long id)
        {
            var itemToUpdate = await _serviceAssignmentMasterRepository.FindServiceAssignmentById(id);

            if (itemToUpdate == null)
            {
                return CommonResponse.Send(ResponseCodes.NO_DATA_AVAILABLE);
            }

            if (!await _serviceAssignmentMasterRepository.UpdateReadyStatus(itemToUpdate))
            {
                return CommonResponse.Send(ResponseCodes.FAILURE, null, ResponseMessage.InternalServer500 );
            }

            return CommonResponse.Send(ResponseCodes.SUCCESS, null, ResponseMessage.Success200);
        }
    }
}
