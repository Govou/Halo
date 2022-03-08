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
        private readonly ILogger<ServiceAssignmentMasterRepositoryImpl> _logger;
        private readonly HalobizContext _context;

        private readonly IMapper _mapper;

        public MasterServiceAssignmentServiceImpl(IMapper mapper, IServiceAssignmentMasterRepository serviceAssignmentMasterRepository, 
            IServiceRegistrationRepository serviceRegistrationRepository, IDTSMastersRepository dTSMastersRepository, IServiceAssignmentDetailsRepository serviceAssignmentDetailsRepository, ICustomerDivisionRepository CustomerDivisionRepo,
            ICommanderRegistrationRepository commanderRegistrationRepository, IPilotRegistrationRepository pilotRegistrationRepository, IArmedEscortRegistrationRepository armedEscortRegistrationRepository,
            IVehicleRegistrationRepository vehicleRegistrationRepository, ILogger<ServiceAssignmentMasterRepositoryImpl> logger, HalobizContext context)
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
            long TiedVehicleId = 0;
            //var RouteExistsForVehicle = _vehicleRegistrationRepository.GetAllVehiclesOnRouteByResourceAndRouteId(masterReceivingDTO.SMORouteId);

            try
            {
                master.CreatedById = context.GetLoggedInUserId();
                master.PickoffTime = pickofftime;
                master.CreatedAt = DateTime.UtcNow;
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
                }

                if (masterReceivingDTO.IsReturnJourney == true)
                {
                    master.Id = 0;
                    master.PickoffLocation = masterReceivingDTO.DropoffLocation;
                    master.DropoffLocation = masterReceivingDTO.PickoffLocation;
                    master.TripTypeId = 2;
                    master.SAExecutionStatus = 0;
                    master.PickoffTime = pickofftime;
                    master.AssignmentStatus = "open";
                    master.PrimaryTripAssignmentId = getId;
                    master.CreatedById = context.GetLoggedInUserId();
                    master.CreatedAt = DateTime.UtcNow;
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
                if (getId != 0)
                {
                    var getVehicleServiceRegistration = await _serviceRegistrationRepository.FindServiceById(masterReceivingDTO.ServiceRegistrationId);
                    //var getVehicleDetail = await _serviceAssignmentDetailsRepository.FindAllVehicleServiceAssignmentDetails();
                    var getVehicleDetailNoneHeld = await _serviceAssignmentDetailsRepository.FindAllNoneHeldVehicleServiceAssignmentDetails();
                    var getVehicleDetailListById = await _serviceAssignmentDetailsRepository.FindAllVehicleServiceAssignmentDetailsByAssignmentId(getId);
                    long? getVehicleResourceId = 0;
                    long? getTypeId = 0;
                    long count = 0;
                    var getResourceTypePerServiceForVehicle = await _serviceRegistrationRepository.FindVehicleResourceByServiceRegId(masterReceivingDTO.ServiceRegistrationId);
                    //var getResourceTypePerServiceForVehicle = await _serviceRegistrationRepository.FindAllVehicleResourceByServiceRegId(masterReceivingDTO.ServiceRegistrationId);
                    //var getAllResourceSchedule = await _dTSMastersRepository.FindAllVehicleMastersForAutoAssignment();
                    var getAllResourceSchedule = await _dTSMastersRepository.FindAllVehicleMastersForAutoAssignmentByPickupDate(masterReceivingDTO.SMORouteId,masterReceivingDTO.PickupDate, masterReceivingDTO.PickoffTime);
                    var RouteExistsForVehicle = _vehicleRegistrationRepository.GetAllVehiclesOnRouteByResourceAndRouteId(masterReceivingDTO.SMORouteId);

                    if (getVehicleServiceRegistration.RequiresVehicle == true)
                    {
                        if ( getAllResourceSchedule.Count() >= getVehicleServiceRegistration.VehicleQuantityRequired)
                        {
                            int countSchedule = 0;
                            int resourceCount = 0;
                            int countResource = getAllResourceSchedule.Count();
                            int countBalance = 0;
                            int countPassed = 0;
                            int countFailed = 0;
                            
                            var _lastItem = getAllResourceSchedule.Last();
                                foreach (var schedule in getAllResourceSchedule)
                                {
                                  var breakOut = false;
                                //var innerLoopBreak = false;
                                  countBalance = countResource - 1;
                                  getVehicleResourceId = schedule.VehicleResourceId;
                                  getTypeId = schedule.VehicleResource.VehicleTypeId;
                                    var getVehicleDetail = await _serviceAssignmentDetailsRepository.FindVehicleServiceAssignmentDetailByResourceId2(getVehicleResourceId);
                                    if (getVehicleDetail != null)
                                    {
                                        countFailed += 1;
                                        if (schedule.Equals(_lastItem)) { transaction.Rollback(); return CommonResponse.Send(ResponseCodes.FAILURE, null, ResponseMessage.ResourceNotAvailble450); }

                                        else { continue; }
                                          
                                    }
                                    
                                    var getResourceSchedule = await _dTSMastersRepository.FindVehicleMasterByResourceId2(getVehicleResourceId);
                                    if (masterReceivingDTO.PickupDate >= schedule.AvailabilityStart && masterReceivingDTO.PickupDate <= schedule.AvailablilityEnd)
                                    {
                                            var genericVLastItem = schedule.GenericDays.Last();
                                            var lastItem_ = schedule.GenericDays.Last();

                                            foreach (var item in schedule.GenericDays)
                                            {
                                               
                                                if (masterReceivingDTO.PickoffTime.TimeOfDay >= item.OpeningTime.TimeOfDay && masterReceivingDTO.PickoffTime.TimeOfDay <= item.ClosingTime.TimeOfDay)
                                                {
                                                    if (masterReceivingDTO.PickupDate.DayOfWeek.ToString() == "Monday" && item.Monday == true)
                                                    {
                                                        breakOut = true;
                                                        //break;
                                                    }
                                                    else if (masterReceivingDTO.PickupDate.DayOfWeek.ToString() == "Tuesday" && item.Tuesday == true)
                                                    {
                                                    breakOut = true;
                                                    //break;
                                                    }
                                                    else if (masterReceivingDTO.PickupDate.DayOfWeek.ToString() == "Wednesday" && item.Wednesday == true)
                                                    {
                                                    breakOut = true;
                                                    //break;
                                                    }
                                                    else if (masterReceivingDTO.PickupDate.DayOfWeek.ToString() == "Thursday" && item.Thursday == true)
                                                    {
                                                    breakOut = true;
                                                    //break;
                                                    }
                                                    else if (masterReceivingDTO.PickupDate.DayOfWeek.ToString() == "Friday" && item.Friday == true)
                                                    {
                                                    breakOut = true;
                                                    //break;
                                                    }
                                                    else if (masterReceivingDTO.PickupDate.DayOfWeek.ToString() == "Saturday" && item.Saturday == true)
                                                    {
                                                    breakOut = true;
                                                    //break;
                                                    }
                                                    else if (masterReceivingDTO.PickupDate.DayOfWeek.ToString() == "Sunday" && item.Sunday == true)
                                                    {
                                                    breakOut = true;
                                                    //break;
                                                    }
                                                }
                                                //else
                                                //{
                                                ////transaction.Rollback();
                                                //    return CommonResponse.Send(ResponseCodes.FAILURE, null, ResponseMessage.ScheduleTimeMismatch448);
                                                //}
                                                if (breakOut)
                                                {

                                                    break;
                                                }
                                                

                                            }
                                            if (!breakOut)
                                            {
                                            if (schedule.Equals(_lastItem))
                                            {
                                                transaction.Rollback();
                                                return CommonResponse.Send(ResponseCodes.FAILURE, null, ResponseMessage.ResourceNotAvailble450);
                                            }
                                            else
                                                continue;
                                            }
                                       
                                    }
                                    else
                                    {
                                        if (schedule.Equals(_lastItem)) 
                                        {
                                        transaction.Rollback();
                                        return CommonResponse.Send(ResponseCodes.FAILURE, null, ResponseMessage.ResourceNotAvailble450);
                                        }
                                        else
                                        {
                                        continue;
                                        }

                                    }

                                    //Add
                                    if (getResourceTypePerServiceForVehicle != null)
                                    {
                                        var typeExists = _serviceRegistrationRepository.GetVehicleResourceApplicableTypeReqById(getVehicleServiceRegistration.Id, getTypeId);
                                        if (typeExists == null)
                                        {
                                                if ( schedule.Equals(_lastItem))
                                                {
                                                    //transaction.Rollback();
                                                    return CommonResponse.Send(ResponseCodes.FAILURE, null, ResponseMessage.NoApplicableType446);
                                                }
                                                else
                                                {
                                                continue;
                                                }
                                        }
                                         
                                    }
                                    else
                                    {
                                        //transaction.Rollback();
                                        return CommonResponse.Send(ResponseCodes.FAILURE, null, ResponseMessage.NoApplicableType446);
                                    }

                                    vehicle.Id = 0;
                                    vehicle.IsTemporarilyHeld = true;
                                    vehicle.DateTemporarilyHeld = DateTime.UtcNow;
                                    vehicle.VehicleResourceId = getVehicleResourceId;
                                    vehicle.RequiredCount = resourceCount + 1;
                                    vehicle.CreatedById = context.GetLoggedInUserId();
                                    vehicle.CreatedAt = DateTime.UtcNow;
                                    vehicle.ServiceAssignmentId = getId;
                                    var savedItem = await _serviceAssignmentDetailsRepository.SaveVehicleServiceAssignmentdetail(vehicle);
                                    if (savedItem == null)
                                    {
                                       // transaction.Rollback();
                                        return CommonResponse.Send(ResponseCodes.FAILURE, null, ResponseMessage.InternalServer500);
                                    }
                                    else
                                    {
                                        TiedVehicleId = savedItem.Id;
                                        countSchedule++;
                                        if(countSchedule == getVehicleServiceRegistration.VehicleQuantityRequired)
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
                            return CommonResponse.Send(ResponseCodes.FAILURE, null, ResponseMessage.ResourceNotAvailble450);
                        }
                    }

                }

                //For Pilot
                if (getId != 0)
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
                    var getAllResourceSchedule = await _dTSMastersRepository.FindAllPilotMastersForAutoAssignmentByPickupDate(masterReceivingDTO.SMORouteId,masterReceivingDTO.PickupDate, masterReceivingDTO.PickoffTime);
                    var RouteExists = _pilotRegistrationRepository.GetAllPilotsOnRouteByResourceAndRouteId(masterReceivingDTO.SMORouteId);

                    if (getVehicleServiceRegistration.RequiresPilot == true)
                    {
                        if ( getAllResourceSchedule.Count() >= getVehicleServiceRegistration.PilotQuantityRequired)
                        {
                            int countSchedule = 0;
                            int resourceCount = 0;

                            var _lastItem = getAllResourceSchedule.Last();
                            foreach (var schedule in getAllResourceSchedule)
                            {
                                var breakOut = false;
                                getResourceId = schedule.PilotResourceId;
                                getTypeId = schedule.PilotResource.PilotTypeId;
                                var getVehicleDetail = await _serviceAssignmentDetailsRepository.FindPilotServiceAssignmentDetailByResourceId2(getResourceId);
                                if (getVehicleDetail != null)
                                {
                                    if (schedule.Equals(_lastItem))
                                    {
                                        transaction.Rollback();
                                        return CommonResponse.Send(ResponseCodes.FAILURE, null, ResponseMessage.ResourceNotAvailble450);
                                    }

                                    else { continue; }
                                       
                                }
                                var getResourceSchedule = await _dTSMastersRepository.FindPilotMasterByResourceId2(getResourceId);
                                if (masterReceivingDTO.PickupDate >= schedule.AvailabilityStart && masterReceivingDTO.PickupDate <= schedule.AvailablilityEnd)
                                {
                                    var genericVLastItem = schedule.GenericDays.Last();
                                    var lastItem_ = schedule.GenericDays.Last();

                                    foreach (var item in schedule.GenericDays)
                                    {

                                        if (masterReceivingDTO.PickoffTime.TimeOfDay >= item.OpeningTime.TimeOfDay && masterReceivingDTO.PickoffTime.TimeOfDay <= item.ClosingTime.TimeOfDay)
                                        {
                                            if (masterReceivingDTO.PickupDate.DayOfWeek.ToString() == "Monday" && item.Monday == true)
                                            {
                                                breakOut = true;
                                                //break;
                                            }
                                            else if (masterReceivingDTO.PickupDate.DayOfWeek.ToString() == "Tuesday" && item.Tuesday == true)
                                            {
                                                breakOut = true;
                                                //break;
                                            }
                                            else if (masterReceivingDTO.PickupDate.DayOfWeek.ToString() == "Wednesday" && item.Wednesday == true)
                                            {
                                                breakOut = true;
                                                //break;
                                            }
                                            else if (masterReceivingDTO.PickupDate.DayOfWeek.ToString() == "Thursday" && item.Thursday == true)
                                            {
                                                breakOut = true;
                                                //break;
                                            }
                                            else if (masterReceivingDTO.PickupDate.DayOfWeek.ToString() == "Friday" && item.Friday == true)
                                            {
                                                breakOut = true;
                                                //break;
                                            }
                                            else if (masterReceivingDTO.PickupDate.DayOfWeek.ToString() == "Saturday" && item.Saturday == true)
                                            {
                                                breakOut = true;
                                                //break;
                                            }
                                            else if (masterReceivingDTO.PickupDate.DayOfWeek.ToString() == "Sunday" && item.Sunday == true)
                                            {
                                                breakOut = true;
                                                //break;
                                            }
                                        }
                                        //else
                                        //{
                                        //    //transaction.Rollback();
                                        //    return CommonResponse.Send(ResponseCodes.FAILURE, null, ResponseMessage.ScheduleTimeMismatch448);
                                        //}
                                        if (breakOut)
                                        {
                                            break;
                                        }


                                    }
                                    if (!breakOut)
                                    {
                                        if (schedule.Equals(_lastItem))
                                        {
                                            transaction.Rollback();
                                            return CommonResponse.Send(ResponseCodes.FAILURE, null, ResponseMessage.ResourceNotAvailble450);
                                        }
                                        else
                                            continue;
                                    }

                                }
                                else
                                {
                                    if (schedule.Equals(_lastItem))
                                    {
                                        transaction.Rollback();
                                        return CommonResponse.Send(ResponseCodes.FAILURE, null, ResponseMessage.ResourceNotAvailble450);
                                    }
                                    else
                                    {
                                        continue;
                                    }

                                }

                                //Add
                                if (getResourceTypePerServiceForVehicle != null)
                                {
                                    var typeExists = _serviceRegistrationRepository.GetPilotResourceApplicableTypeReqById(getVehicleServiceRegistration.Id, getTypeId);
                                    if (typeExists == null)
                                    {
                                        if (schedule.Equals(_lastItem))
                                        {
                                            transaction.Rollback();
                                            return CommonResponse.Send(ResponseCodes.FAILURE, null, ResponseMessage.ResourceNotAvailble450);
                                        }
                                        else
                                        {
                                            continue;
                                        }
                                    }

                                }
                                else
                                {
                                    transaction.Rollback();
                                    return CommonResponse.Send(ResponseCodes.FAILURE, null, ResponseMessage.ResourceNotAvailble450);
                                }

                                pilot.Id = 0;
                                pilot.IsTemporarilyHeld = true;
                                pilot.DateTemporarilyHeld = DateTime.UtcNow;
                                pilot.PilotResourceId = getResourceId;
                                pilot.TiedVehicleResourceId = TiedVehicleId;
                                pilot.RequiredCount = resourceCount + 1;
                                pilot.CreatedById = context.GetLoggedInUserId();
                                pilot.CreatedAt = DateTime.UtcNow;
                                pilot.ServiceAssignmentId = getId;
                                var savedItem = await _serviceAssignmentDetailsRepository.SavePilotServiceAssignmentdetail(pilot);
                                if (savedItem == null)
                                {
                                    // transaction.Rollback();
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
                            return CommonResponse.Send(ResponseCodes.FAILURE, null, ResponseMessage.ResourceNotAvailble450);
                        }
                    }

                }

                //For Commander
                if (getId != 0)
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
                    var getAllResourceSchedule = await _dTSMastersRepository.FindAllCommanderMastersForAutoAssignmentByPickupDate(masterReceivingDTO.PickupDate, masterReceivingDTO.PickoffTime);
                    var RouteExists = _commanderRegistrationRepository.GetAllCommanderssOnRouteByResourceAndRouteId(masterReceivingDTO.SMORouteId);

                    if (getVehicleServiceRegistration.RequiresCommander == true)
                    {
                        if (RouteExists.Count() >= getVehicleServiceRegistration.CommanderQuantityRequired && getAllResourceSchedule.Count() >= getVehicleServiceRegistration.CommanderQuantityRequired)
                        {
                            int countSchedule = 0;
                            int resourceCount = 0;

                            var _lastItem = getAllResourceSchedule.Last();
                            foreach (var schedule in getAllResourceSchedule)
                            {
                                var breakOut = false;
                                getResourceId = schedule.CommanderResourceId;
                                getTypeId = schedule.CommanderResource.CommanderTypeId;
                                //double check if resource is temp held
                                var getVehicleDetail = await _serviceAssignmentDetailsRepository.FindCommanderServiceAssignmentDetailByResourceId2(getResourceId);
                                if (getVehicleDetail != null)
                                {
                                    if (schedule.Equals(_lastItem))
                                    {
                                        transaction.Rollback();
                                        return CommonResponse.Send(ResponseCodes.FAILURE, null, ResponseMessage.ResourceNotAvailble450);
                                    }

                                    else { continue; }

                                }
                                var getResourceSchedule = await _dTSMastersRepository.FindCommanderMasterByResourceId2(getResourceId);
                                if (masterReceivingDTO.PickupDate >= schedule.AvailabilityStart && masterReceivingDTO.PickupDate <= schedule.AvailablilityEnd)
                                {
                                    var genericVLastItem = schedule.GenericDays.Last();
                                    var lastItem_ = schedule.GenericDays.Last();

                                    foreach (var item in schedule.GenericDays)
                                    {

                                        if (masterReceivingDTO.PickoffTime.TimeOfDay >= item.OpeningTime.TimeOfDay && masterReceivingDTO.PickoffTime.TimeOfDay <= item.ClosingTime.TimeOfDay)
                                        {
                                            if (masterReceivingDTO.PickupDate.DayOfWeek.ToString() == "Monday" && item.Monday == true)
                                            {
                                                breakOut = true;
                                                //break;
                                            }
                                            else if (masterReceivingDTO.PickupDate.DayOfWeek.ToString() == "Tuesday" && item.Tuesday == true)
                                            {
                                                breakOut = true;
                                                //break;
                                            }
                                            else if (masterReceivingDTO.PickupDate.DayOfWeek.ToString() == "Wednesday" && item.Wednesday == true)
                                            {
                                                breakOut = true;
                                                //break;
                                            }
                                            else if (masterReceivingDTO.PickupDate.DayOfWeek.ToString() == "Thursday" && item.Thursday == true)
                                            {
                                                breakOut = true;
                                                //break;
                                            }
                                            else if (masterReceivingDTO.PickupDate.DayOfWeek.ToString() == "Friday" && item.Friday == true)
                                            {
                                                breakOut = true;
                                                //break;
                                            }
                                            else if (masterReceivingDTO.PickupDate.DayOfWeek.ToString() == "Saturday" && item.Saturday == true)
                                            {
                                                breakOut = true;
                                                //break;
                                            }
                                            else if (masterReceivingDTO.PickupDate.DayOfWeek.ToString() == "Sunday" && item.Sunday == true)
                                            {
                                                breakOut = true;
                                                //break;
                                            }
                                        }
                                        //else
                                        //{
                                        //    //transaction.Rollback();
                                        //    return CommonResponse.Send(ResponseCodes.FAILURE, null, ResponseMessage.ScheduleTimeMismatch448);
                                        //}
                                        if (breakOut)
                                        {
                                            break;
                                        }


                                    }
                                    if (!breakOut)
                                    {
                                        if (schedule.Equals(_lastItem))
                                        {
                                            transaction.Rollback();
                                            return CommonResponse.Send(ResponseCodes.FAILURE, null, ResponseMessage.ResourceNotAvailble450);
                                        }
                                        else
                                            continue;
                                    }

                                }
                                else
                                {
                                    if (schedule.Equals(_lastItem))
                                    {
                                        transaction.Rollback();
                                        return CommonResponse.Send(ResponseCodes.FAILURE, null, ResponseMessage.ResourceNotAvailble450);
                                    }
                                    else
                                    {
                                        continue;
                                    }

                                }

                                //Add
                                if (getResourceTypePerServiceForVehicle != null)
                                {
                                    var typeExists = _serviceRegistrationRepository.GetCommanderResourceApplicableTypeReqById(getVehicleServiceRegistration.Id, getTypeId);
                                    if (typeExists == null)
                                    {
                                        if (schedule.Equals(_lastItem))
                                        {
                                            transaction.Rollback();
                                            return CommonResponse.Send(ResponseCodes.FAILURE, null, ResponseMessage.ResourceNotAvailble450);
                                        }
                                        else
                                        {
                                            continue;
                                        }
                                    }

                                }
                                else
                                {
                                    transaction.Rollback();
                                    return CommonResponse.Send(ResponseCodes.FAILURE, null, ResponseMessage.ResourceNotAvailble450);
                                }

                                commander.Id = 0;
                                commander.IsTemporarilyHeld = true;
                                commander.DateTemporarilyHeld = DateTime.UtcNow;
                                commander.CommanderResourceId = getResourceId;
                                commander.TiedVehicleResourceId = TiedVehicleId;
                                commander.RequiredCount = resourceCount + 1;
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
                            return CommonResponse.Send(ResponseCodes.FAILURE, null, ResponseMessage.ResourceNotAvailble450);
                        }
                    }

                }

                //For ArmedEscort
                if (getId != 0)
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
                    var getAllResourceSchedule = await _dTSMastersRepository.FindAllArmedEscortMastersForAutoAssignmentByPickupDate(masterReceivingDTO.PickupDate, masterReceivingDTO.PickoffTime);
                    var RouteExists = _commanderRegistrationRepository.GetAllCommanderssOnRouteByResourceAndRouteId(masterReceivingDTO.SMORouteId);

                    if (getVehicleServiceRegistration.RequiresArmedEscort == true)
                    {
                        if (RouteExists.Count() >= getVehicleServiceRegistration.ArmedEscortQuantityRequired && getAllResourceSchedule.Count() >= getVehicleServiceRegistration.ArmedEscortQuantityRequired)
                        {
                            int countSchedule = 0;
                            int resourceCount = 0;

                            var _lastItem = getAllResourceSchedule.Last();
                            foreach (var schedule in getAllResourceSchedule)
                            {
                                var breakOut = false;
                                getResourceId = schedule.ArmedEscortResourceId;
                                getTypeId = schedule.ArmedEscortResource.ArmedEscortTypeId;
                                //double check if resource is temp held
                                var getVehicleDetail = await _serviceAssignmentDetailsRepository.FindEscortServiceAssignmentDetailByResourceId2(getResourceId);
                                if (getVehicleDetail != null)
                                {
                                    if (schedule.Equals(_lastItem))
                                    {
                                        transaction.Rollback();
                                        return CommonResponse.Send(ResponseCodes.FAILURE, null, ResponseMessage.ResourceNotAvailble450);
                                    }

                                    else { continue; }

                                }
                                var getResourceSchedule = await _dTSMastersRepository.FindArmedEscortMasterByResourceId2(getResourceId);
                                if (masterReceivingDTO.PickupDate >= schedule.AvailabilityStart && masterReceivingDTO.PickupDate <= schedule.AvailablilityEnd)
                                {
                                    var genericVLastItem = schedule.GenericDays.Last();
                                    var lastItem_ = schedule.GenericDays.Last();

                                    foreach (var item in schedule.GenericDays)
                                    {

                                        if (masterReceivingDTO.PickoffTime.TimeOfDay >= item.OpeningTime.TimeOfDay && masterReceivingDTO.PickoffTime.TimeOfDay <= item.ClosingTime.TimeOfDay)
                                        {
                                            if (masterReceivingDTO.PickupDate.DayOfWeek.ToString() == "Monday" && item.Monday == true)
                                            {
                                                breakOut = true;
                                                //break;
                                            }
                                            else if (masterReceivingDTO.PickupDate.DayOfWeek.ToString() == "Tuesday" && item.Tuesday == true)
                                            {
                                                breakOut = true;
                                                //break;
                                            }
                                            else if (masterReceivingDTO.PickupDate.DayOfWeek.ToString() == "Wednesday" && item.Wednesday == true)
                                            {
                                                breakOut = true;
                                                //break;
                                            }
                                            else if (masterReceivingDTO.PickupDate.DayOfWeek.ToString() == "Thursday" && item.Thursday == true)
                                            {
                                                breakOut = true;
                                                //break;
                                            }
                                            else if (masterReceivingDTO.PickupDate.DayOfWeek.ToString() == "Friday" && item.Friday == true)
                                            {
                                                breakOut = true;
                                                //break;
                                            }
                                            else if (masterReceivingDTO.PickupDate.DayOfWeek.ToString() == "Saturday" && item.Saturday == true)
                                            {
                                                breakOut = true;
                                                //break;
                                            }
                                            else if (masterReceivingDTO.PickupDate.DayOfWeek.ToString() == "Sunday" && item.Sunday == true)
                                            {
                                                breakOut = true;
                                                //break;
                                            }
                                        }
                                        //else
                                        //{
                                        //    //transaction.Rollback();
                                        //    return CommonResponse.Send(ResponseCodes.FAILURE, null, ResponseMessage.ScheduleTimeMismatch448);
                                        //}
                                        if (breakOut)
                                        {
                                            break;
                                        }


                                    }
                                    if (!breakOut)
                                    {
                                        if (schedule.Equals(_lastItem))
                                        {
                                            transaction.Rollback();
                                            return CommonResponse.Send(ResponseCodes.FAILURE, null, ResponseMessage.ResourceNotAvailble450);
                                        }
                                        else
                                            continue;
                                    }

                                }
                                else
                                {
                                    if (schedule.Equals(_lastItem))
                                    {
                                        transaction.Rollback();
                                        return CommonResponse.Send(ResponseCodes.FAILURE, null, ResponseMessage.ResourceNotAvailble450);
                                    }
                                    else
                                    {
                                        continue;
                                    }

                                }

                                //Add
                                if (getResourceTypePerServiceForVehicle != null)
                                {
                                    var typeExists = _serviceRegistrationRepository.GetArmedEscortResourceApplicableTypeReqById(getVehicleServiceRegistration.Id, getTypeId);
                                    if (typeExists == null)
                                    {
                                        if (schedule.Equals(_lastItem))
                                        {
                                            transaction.Rollback();
                                            return CommonResponse.Send(ResponseCodes.FAILURE, null, ResponseMessage.ResourceNotAvailble450);
                                        }
                                        else
                                        {
                                            continue;
                                        }
                                    }

                                }
                                else
                                {
                                    transaction.Rollback();
                                    return CommonResponse.Send(ResponseCodes.FAILURE, null, ResponseMessage.ResourceNotAvailble450);
                                }

                                armedEscort.Id = 0;
                                armedEscort.IsTemporarilyHeld = true;
                                armedEscort.DateTemporarilyHeld = DateTime.UtcNow;
                                armedEscort.ArmedEscortResourceId = getResourceId;
                                armedEscort.RequiredCount = resourceCount + 1;
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
                            return CommonResponse.Send(ResponseCodes.FAILURE, null, ResponseMessage.ResourceNotAvailble450);
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









            // var typeTransferDTO = _mapper.Map<MasterServiceAssignmentTransferDTO>(master);
            transaction.Commit();
            return CommonResponse.Send(ResponseCodes.SUCCESS, null, "Auto Assignment Successful");
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

            //var NameExist = _armedEscortsRepository.GetTypename(armedEscortTypeReceivingDTO.Name);
            //if (NameExist != null)
            //{
            //    return CommonResponse.Send(ResponseCodes.FAILURE, null, ResponseMessage.RecordExists409);
            //}

            //master.CreatedBy = context.User.Claims();
            master.CreatedById = context.GetLoggedInUserId();
            master.PickoffTime = pickofftime;
            master.CreatedAt = DateTime.UtcNow;
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
            }

            if(masterReceivingDTO.IsReturnJourney == true)
            {
                master.Id = 0;
                master.PickoffLocation = masterReceivingDTO.DropoffLocation;
                master.DropoffLocation = masterReceivingDTO.PickoffLocation;
                master.TripTypeId = 2;
                master.SAExecutionStatus = 0;
                master.PickoffTime = pickofftime;
                master.AssignmentStatus = "open";
                master.PrimaryTripAssignmentId = getId ;
                master.CreatedById = context.GetLoggedInUserId();
                master.CreatedAt = DateTime.UtcNow;
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
            return CommonResponse.Send(ResponseCodes.SUCCESS, null, ResponseMessage.Success200);
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
                return CommonResponse.Send(ResponseCodes.FAILURE, null, ResponseMessage.InternalServer500);
            }

            return CommonResponse.Send(ResponseCodes.SUCCESS, null, ResponseMessage.Success200);
        }
    }
}
