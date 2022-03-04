using AutoMapper;
using HaloBiz.DTOs.ApiDTOs;
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
            var RouteExistsForVehicle = _vehicleRegistrationRepository.GetAllVehiclesOnRouteByResourceAndRouteId(masterReceivingDTO.SMORouteId);

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
                    transaction.Commit();
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
                            //transaction.Rollback();
                            return CommonResponse.Send(ResponseCodes.FAILURE, null, ResponseMessage.InternalServer500);
                        }
                        else
                        {
                            transaction.Commit();
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
                    var getAllResourceSchedule = await _dTSMastersRepository.FindAllVehicleMastersForAutoAssignmentByPickupDate(masterReceivingDTO.PickupDate, masterReceivingDTO.PickoffTime);
                    //var RouteExistsForVehicle = _vehicleRegistrationRepository.GetAllVehiclesOnRouteByResourceAndRouteId(masterReceivingDTO.SMORouteId);

                    if (getVehicleServiceRegistration.RequiresVehicle == true)
                    {
                        if (RouteExistsForVehicle.Count() >= getVehicleServiceRegistration.VehicleQuantityRequired && getAllResourceSchedule.Count() >= getVehicleServiceRegistration.VehicleQuantityRequired)
                        {
                            int countSchedule = 0;
                            int resourceCount = 0;
                            
                            var _lastItem = getAllResourceSchedule.Last();
                                foreach (var schedule in getAllResourceSchedule)
                                {
                                  var breakOut = false;
                                  getVehicleResourceId = schedule.VehicleResourceId;
                                  getTypeId = schedule.VehicleResource.VehicleTypeId;
                                    var getVehicleDetail = await _serviceAssignmentDetailsRepository.FindVehicleServiceAssignmentDetailByResourceId2(getVehicleResourceId);
                                    if (getVehicleDetail != null)
                                    {
                                        if (getVehicleDetail.IsTemporarilyHeld == true || getVehicleDetail.IsHeldForAction == true)
                                        {
                                            if(schedule.Equals(_lastItem))
                                            return CommonResponse.Send(ResponseCodes.FAILURE, null, ResponseMessage.Held444);
                                            else
                                           continue;
                                            
                                        }
                                    }
                                    var getResourceSchedule = await _dTSMastersRepository.FindVehicleMasterByResourceId2(getVehicleResourceId);
                                    if (masterReceivingDTO.PickupDate >= schedule.AvailabilityStart && masterReceivingDTO.PickupDate <= schedule.AvailablilityEnd)
                                    {
                                        if (getResourceSchedule.GenericDays.Count() != 0)
                                        {
                                            var genericVLastItem = schedule.GenericDays.Last();
                                            var lastItem_ = schedule.GenericDays.Last();
                                            foreach (var item in schedule.GenericDays)
                                            {

                                                if (!(masterReceivingDTO.PickoffTime.TimeOfDay >= item.OpeningTime.TimeOfDay && masterReceivingDTO.PickoffTime.TimeOfDay <= item.ClosingTime.TimeOfDay))
                                                {
                                                    if (item.Equals(genericVLastItem))
                                                    {
                                                        transaction.Rollback();
                                                        return CommonResponse.Send(ResponseCodes.FAILURE, null, ResponseMessage.NoGenericDay449);

                                                    }
                                                    else
                                                    {
                                                        continue;
                                                    }

                                                }
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
                                             
                                                //else
                                                //{
                                                //    transaction.Rollback();
                                                //    return CommonResponse.Send(ResponseCodes.FAILURE, null, ResponseMessage.ScheduleTimeMismatch448);
                                                //}
                                            }
                                                else
                                                {
                                                    transaction.Rollback();
                                                    return CommonResponse.Send(ResponseCodes.FAILURE, null, ResponseMessage.ScheduleTimeMismatch448);
                                                }
                                            if (breakOut)
                                            {

                                                break;
                                            }

                                        }
                                      
                                    }
                                        else
                                        {
                                            transaction.Rollback();
                                            return CommonResponse.Send(ResponseCodes.FAILURE, null, ResponseMessage.NoGenericDay449);
                                        }
                                    }
                                    else
                                    {
                                        transaction.Rollback();
                                        return CommonResponse.Send(ResponseCodes.FAILURE, null, ResponseMessage.ScheduleTimeMismatch448);
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


                            //Stops Here

                                //foreach (var _item in getVehicleDetailNoneHeld)
                                //{
                                //    getVehicleResourceId = _item.VehicleResourceId;
                                //    var getResourceSchedule = await _dTSMastersRepository.FindVehicleMasterByResourceId2(getVehicleResourceId);
                                //    if (!_item.Equals(lastItem) && getResourceSchedule == null )
                                //    {
                                //        continue;
                                //    }


                                //    if (getResourceSchedule != null)
                                //    {
                                //        if (masterReceivingDTO.PickupDate >= getResourceSchedule.AvailabilityStart && masterReceivingDTO.PickupDate <= getResourceSchedule.AvailablilityEnd)
                                //        {
                                //            if (getResourceSchedule.GenericDays.Count() != 0)
                                //            {
                                //                var genericVLastItem = getResourceSchedule.GenericDays.Last();
                                //                foreach (var item in getResourceSchedule.GenericDays)
                                //                {

                                //                    if (!(masterReceivingDTO.PickoffTime.TimeOfDay >= item.OpeningTime.TimeOfDay && masterReceivingDTO.PickoffTime.TimeOfDay <= item.ClosingTime.TimeOfDay))
                                //                    {
                                //                        if (item.Equals(lastItem))
                                //                        {
                                //                            transaction.Rollback();
                                //                            return CommonResponse.Send(ResponseCodes.FAILURE, null, ResponseMessage.NoGenericDay449);

                                //                        }
                                //                        else
                                //                        {
                                //                            continue;
                                //                        }

                                //                    }
                                //                    if (masterReceivingDTO.PickoffTime.TimeOfDay >= item.OpeningTime.TimeOfDay && masterReceivingDTO.PickoffTime.TimeOfDay <= item.ClosingTime.TimeOfDay)
                                //                    {
                                //                        if (masterReceivingDTO.PickupDate.DayOfWeek.ToString() == "Monday" && item.Monday == true)
                                //                        {
                                //                            break;
                                //                        }
                                //                        else if (masterReceivingDTO.PickupDate.DayOfWeek.ToString() == "Tuesday" && item.Tuesday == true)
                                //                        {
                                //                            break;
                                //                        }
                                //                        else if (masterReceivingDTO.PickupDate.DayOfWeek.ToString() == "Wednesday" && item.Wednesday == true)
                                //                        {
                                //                            break;
                                //                        }
                                //                        else if (masterReceivingDTO.PickupDate.DayOfWeek.ToString() == "Thursday" && item.Thursday == true)
                                //                        {
                                //                            break;
                                //                        }
                                //                        else if (masterReceivingDTO.PickupDate.DayOfWeek.ToString() == "Friday" && item.Friday == true)
                                //                        {
                                //                            break;
                                //                        }
                                //                        else if (masterReceivingDTO.PickupDate.DayOfWeek.ToString() == "Saturday" && item.Saturday == true)
                                //                        {
                                //                            break;
                                //                        }
                                //                        else if (masterReceivingDTO.PickupDate.DayOfWeek.ToString() == "Sunday" && item.Sunday == true)
                                //                        {
                                //                            break;
                                //                        }
                                //                        else
                                //                        {
                                //                            transaction.Rollback();
                                //                            return CommonResponse.Send(ResponseCodes.FAILURE, null, ResponseMessage.ScheduleTimeMismatch448);
                                //                        }
                                //                    }
                                //                    else
                                //                    {
                                //                        transaction.Rollback();
                                //                        return CommonResponse.Send(ResponseCodes.FAILURE, null, ResponseMessage.ScheduleTimeMismatch448);
                                //                    }
                                //                }
                                //            }
                                //            else
                                //            {
                                //                transaction.Rollback();
                                //                return CommonResponse.Send(ResponseCodes.FAILURE, null, ResponseMessage.NoGenericDay449);
                                //            }
                                //        }
                                //        else
                                //        {
                                //            transaction.Rollback();
                                //            return CommonResponse.Send(ResponseCodes.FAILURE, null, ResponseMessage.ScheduleTimeMismatch448);
                                //        }
                                //    }
                                //    else
                                //    {
                                //        transaction.Rollback();
                                //        return CommonResponse.Send(ResponseCodes.FAILURE, null, ResponseMessage.NoSchedule447);
                                //    }
                                //    if (RouteExistsForVehicle != null)
                                //    {
                                //        int resourceCount = 0;
                                //        if (getResourceTypePerServiceForVehicle != null)
                                //        {
                                //            var typeExists = _serviceRegistrationRepository.GetVehicleResourceApplicableTypeReqById(getVehicleServiceRegistration.Id, getResourceTypePerServiceForVehicle.VehicleTypeId);
                                //            if (typeExists == null)
                                //            {
                                //                transaction.Rollback();
                                //                return CommonResponse.Send(ResponseCodes.FAILURE, null, ResponseMessage.NoApplicableType446);
                                //            }
                                //        }
                                //        else
                                //        {
                                //            transaction.Rollback();
                                //            return CommonResponse.Send(ResponseCodes.FAILURE, null, ResponseMessage.NoApplicableType446);
                                //        }
                                //        //if (getVehicleDetail != null)
                                //        //{
                                //        //    if (getVehicleDetail.IsTemporarilyHeld == true || getVehicleDetail.IsHeldForAction == true)
                                //        //    {
                                //        //        return CommonResponse.Send(ResponseCodes.FAILURE, null, ResponseMessage.Held444);
                                //        //    }
                                //        //}
                                //        vehicle.Id = 0;
                                //        vehicle.IsTemporarilyHeld = true;
                                //        vehicle.DateTemporarilyHeld = DateTime.UtcNow;
                                //        //vehicle.IsHeldForAction = false;
                                //        //vehicle.DateHeldForAction = DateTime.UtcNow;
                                //        vehicle.RequiredCount = resourceCount + 1;
                                //        vehicle.CreatedById = context.GetLoggedInUserId();
                                //        vehicle.CreatedAt = DateTime.UtcNow;
                                //        vehicle.VehicleResourceId =
                                //        vehicle.ServiceAssignmentId = getId;
                                //        var savedItem = await _serviceAssignmentDetailsRepository.SaveVehicleServiceAssignmentdetail(vehicle);
                                //        if (savedItem == null)
                                //        {
                                //            transaction.Rollback();
                                //            return CommonResponse.Send(ResponseCodes.FAILURE, null, ResponseMessage.InternalServer500);
                                //        }
                                //        else
                                //        {
                                //            TiedVehicleId = savedItem.Id;
                                //        }
                                      
                                //    }
                                //    else
                                //    {
                                //        transaction.Rollback();
                                //        return CommonResponse.Send(ResponseCodes.FAILURE, null, ResponseMessage.NoResourceOnRoute443);
                                //    }

                                //    count++;
                                //    if (count == getVehicleServiceRegistration.VehicleQuantityRequired)
                                //    {
                                //        break;
                                //    }
                                //}
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
                    var getServiceRegistration = await _serviceRegistrationRepository.FindServiceById(masterReceivingDTO.ServiceRegistrationId);
                    //var getVehicleDetail = await _serviceAssignmentDetailsRepository.FindAllVehicleServiceAssignmentDetails();
                    //var getVehicleDetailNoneHeld = await _serviceAssignmentDetailsRepository.FindAllNoneHeldVehicleServiceAssignmentDetails();
                    //var getVehicleDetailListById = await _serviceAssignmentDetailsRepository.FindAllVehicleServiceAssignmentDetailsByAssignmentId(getId);
                    long? getResourceId = 0;
                    long? getTypeId = 0;
                    long count = 0;
                    var getResourceTypePerService = await _serviceRegistrationRepository.FindPilotResourceByServiceRegId(masterReceivingDTO.ServiceRegistrationId);
                    //var getResourceTypePerServiceForVehicle = await _serviceRegistrationRepository.FindAllVehicleResourceByServiceRegId(masterReceivingDTO.ServiceRegistrationId);
                    //var getAllResourceSchedule = await _dTSMastersRepository.FindAllVehicleMastersForAutoAssignment();
                    var getAllResourceSchedule = await _dTSMastersRepository.FindAllPilotMastersForAutoAssignmentByPickupDate(masterReceivingDTO.PickupDate, masterReceivingDTO.PickoffTime);
                    //var RouteExistsForVehicle = _vehicleRegistrationRepository.GetAllVehiclesOnRouteByResourceAndRouteId(masterReceivingDTO.SMORouteId);

                    if (getServiceRegistration.RequiresVehicle == true)
                    {
                        if (RouteExistsForVehicle.Count() >= getServiceRegistration.VehicleQuantityRequired && getAllResourceSchedule.Count() >= getServiceRegistration.VehicleQuantityRequired)
                        {
                            int countSchedule = 0;
                            int resourceCount = 0;

                            var _lastItem = getAllResourceSchedule.Last();
                            foreach (var schedule in getAllResourceSchedule)
                            {
                                var breakOut = false;
                                getResourceId = schedule.PilotResourceId;
                                getTypeId = schedule.PilotResource.PilotTypeId;
                                //not checking for null bcoz it has already been checked bedore reaching this stage. so it can't be null
                                var getResourceSchedule = await _dTSMastersRepository.FindVehicleMasterByResourceId2(getResourceId);
                                if (masterReceivingDTO.PickupDate >= schedule.AvailabilityStart && masterReceivingDTO.PickupDate <= schedule.AvailablilityEnd)
                                {
                                    if (getResourceSchedule.GenericDays.Count() != 0)
                                    {
                                        var genericVLastItem = schedule.GenericDays.Last();
                                        var lastItem_ = schedule.GenericDays.Last();
                                        foreach (var item in schedule.GenericDays)
                                        {

                                            if (!(masterReceivingDTO.PickoffTime.TimeOfDay >= item.OpeningTime.TimeOfDay && masterReceivingDTO.PickoffTime.TimeOfDay <= item.ClosingTime.TimeOfDay))
                                            {
                                                if (item.Equals(genericVLastItem))
                                                {
                                                    transaction.Rollback();
                                                    return CommonResponse.Send(ResponseCodes.FAILURE, null, ResponseMessage.NoGenericDay449);

                                                }
                                                else
                                                {
                                                    continue;
                                                }

                                            }
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
                                            else
                                            {
                                                transaction.Rollback();
                                                return CommonResponse.Send(ResponseCodes.FAILURE, null, ResponseMessage.ScheduleTimeMismatch448);
                                            }
                                            if (breakOut)
                                            {

                                                break;
                                            }

                                        }

                                    }
                                    else
                                    {
                                        transaction.Rollback();
                                        return CommonResponse.Send(ResponseCodes.FAILURE, null, ResponseMessage.NoGenericDay449);
                                    }
                                }
                                else
                                {
                                    transaction.Rollback();
                                    return CommonResponse.Send(ResponseCodes.FAILURE, null, ResponseMessage.ScheduleTimeMismatch448);
                                }

                                //Add
                                if (getResourceTypePerService != null)
                                {
                                    var typeExists = _serviceRegistrationRepository.GetVehicleResourceApplicableTypeReqById(getServiceRegistration.Id, getTypeId);
                                    if (typeExists == null)
                                    {
                                        if (schedule.Equals(_lastItem))
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
                                vehicle.VehicleResourceId = getResourceId;
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
                                    if (countSchedule == getServiceRegistration.VehicleQuantityRequired)
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
                    var getServiceRegistration = await _serviceRegistrationRepository.FindServiceById(masterReceivingDTO.ServiceRegistrationId);
                    //var getVehicleDetail = await _serviceAssignmentDetailsRepository.FindVehicleServiceAssignmentDetailByResourceId(vehicleReceivingDTO.VehicleResourceId);
                    //var getResourceDetail = await _serviceAssignmentDetailsRepository.FindAllCommanderServiceAssignmentDetails();
                    var getResourceDetailNoneHeld = await _serviceAssignmentDetailsRepository.FindAllNoneHeldCommanderServiceAssignmentDetails();
                    //var getResourceDetailListById = await _serviceAssignmentDetailsRepository.FindAllEscortServiceAssignmentDetailsByAssignmentId(getId);
                    long? getResourceId = 0;
                    long count = 0;
                    var getResourceTypePerService = await _serviceRegistrationRepository.FindCommanderResourceByServiceRegId(masterReceivingDTO.ServiceRegistrationId);
                    //var getResourceSchedule = await _dTSMastersRepository.FindVehicleMasterByResourceId(vehicleReceivingDTO.VehicleResourceId);
                    if (getServiceRegistration.RequiresPilot == true)
                    {
                        if (getResourceDetailNoneHeld.Count() >= getServiceRegistration.CommanderQuantityRequired)
                        {
                            if (getResourceDetailNoneHeld.Count() != 0)
                            {
                                var lastItem = getResourceDetailNoneHeld.Last();
                                foreach (var _item in getResourceDetailNoneHeld)
                                {
                                    getResourceId = _item.CommanderResourceId;
                                    var getResourceSchedule = await _dTSMastersRepository.FindCommanderMasterByResourceId2(getResourceId);
                                    var RouteExists = _commanderRegistrationRepository.GetResourceRegIdRegionAndRouteId2(getResourceId, masterReceivingDTO.SMORouteId);
                                    if(getResourceSchedule == null)
                                    {
                                        continue;
                                    }

                                    if (getResourceSchedule != null)
                                    {
                                        if (masterReceivingDTO.PickupDate >= getResourceSchedule.AvailabilityStart && masterReceivingDTO.PickupDate <= getResourceSchedule.AvailablilityEnd)
                                        {
                                            if (getResourceSchedule.GenericDays.Count() != 0)
                                            {
                                                var genericVLastItem = getResourceSchedule.GenericDays.Last();
                                                foreach (var item in getResourceSchedule.GenericDays)
                                                {

                                                    if (!(masterReceivingDTO.PickoffTime.TimeOfDay >= item.OpeningTime.TimeOfDay && masterReceivingDTO.PickoffTime.TimeOfDay <= item.ClosingTime.TimeOfDay))
                                                    {
                                                        if (item.Equals(lastItem))
                                                        {
                                                            transaction.Rollback();
                                                            return CommonResponse.Send(ResponseCodes.FAILURE, null, ResponseMessage.NoGenericDay449);

                                                        }
                                                        else
                                                        {
                                                            continue;
                                                        }

                                                    }
                                                    if (masterReceivingDTO.PickoffTime.TimeOfDay >= item.OpeningTime.TimeOfDay && masterReceivingDTO.PickoffTime.TimeOfDay <= item.ClosingTime.TimeOfDay)
                                                    {
                                                        if (masterReceivingDTO.PickupDate.DayOfWeek.ToString() == "Monday" && item.Monday == true)
                                                        {
                                                            break;
                                                        }
                                                        else if (masterReceivingDTO.PickupDate.DayOfWeek.ToString() == "Tuesday" && item.Tuesday == true)
                                                        {
                                                            break;
                                                        }
                                                        else if (masterReceivingDTO.PickupDate.DayOfWeek.ToString() == "Wednesday" && item.Wednesday == true)
                                                        {
                                                            break;
                                                        }
                                                        else if (masterReceivingDTO.PickupDate.DayOfWeek.ToString() == "Thursday" && item.Thursday == true)
                                                        {
                                                            break;
                                                        }
                                                        else if (masterReceivingDTO.PickupDate.DayOfWeek.ToString() == "Friday" && item.Friday == true)
                                                        {
                                                            break;
                                                        }
                                                        else if (masterReceivingDTO.PickupDate.DayOfWeek.ToString() == "Saturday" && item.Saturday == true)
                                                        {
                                                            break;
                                                        }
                                                        else if (masterReceivingDTO.PickupDate.DayOfWeek.ToString() == "Sunday" && item.Sunday == true)
                                                        {
                                                            break;
                                                        }
                                                        else
                                                        {
                                                            transaction.Rollback();
                                                            return CommonResponse.Send(ResponseCodes.FAILURE, null, ResponseMessage.ScheduleTimeMismatch448);
                                                        }
                                                    }
                                                    else
                                                    {
                                                        transaction.Rollback();
                                                        return CommonResponse.Send(ResponseCodes.FAILURE, null, ResponseMessage.ScheduleTimeMismatch448);
                                                    }
                                                }
                                            }
                                            else
                                            {
                                                transaction.Rollback();
                                                return CommonResponse.Send(ResponseCodes.FAILURE, null, ResponseMessage.NoGenericDay449);
                                            }
                                        }
                                        else
                                        {
                                            transaction.Rollback();
                                            return CommonResponse.Send(ResponseCodes.FAILURE, null, ResponseMessage.ScheduleTimeMismatch448);
                                        }
                                    }
                                    else
                                    {
                                        transaction.Rollback();
                                        return CommonResponse.Send(ResponseCodes.FAILURE, null, ResponseMessage.NoSchedule447);
                                    }
                                    if (RouteExists != null)
                                    {
                                        int resourceCount = 0;
                                        if (getResourceTypePerService != null)
                                        {
                                            var typeExists = _serviceRegistrationRepository.GetCommanderResourceApplicableTypeReqById(getServiceRegistration.Id, getResourceTypePerService.CommanderTypeId);
                                            if (typeExists == null)
                                            {
                                                transaction.Rollback();
                                                return CommonResponse.Send(ResponseCodes.FAILURE, null, ResponseMessage.NoApplicableType446);
                                            }

                                        }
                                        else
                                        {
                                            transaction.Rollback();
                                            return CommonResponse.Send(ResponseCodes.FAILURE, null, ResponseMessage.NoApplicableType446);
                                        }
                                        //if (getVehicleDetail != null)
                                        //{
                                        //    if (getVehicleDetail.IsTemporarilyHeld == true || getVehicleDetail.IsHeldForAction == true)
                                        //    {
                                        //        return CommonResponse.Send(ResponseCodes.FAILURE, null, ResponseMessage.Held444);
                                        //    }
                                        //}
                                        //for (int i = 0; i < getServiceRegistration.VehicleQuantityRequired; i++)
                                        //{
                                        //    vehicle.Id = 0;
                                        //    vehicle.IsTemporarilyHeld = true;
                                        //    vehicle.DateTemporarilyHeld = DateTime.UtcNow;
                                        //    vehicle.IsHeldForAction = true;
                                        //    vehicle.DateHeldForAction = DateTime.UtcNow;
                                        //    vehicle.RequiredCount = getVehicleDetailListById.Count() + 1;
                                        //    vehicle.CreatedById = context.GetLoggedInUserId();
                                        //    vehicle.CreatedAt = DateTime.UtcNow;
                                        //    var savedItem = await _serviceAssignmentDetailsRepository.SaveVehicleServiceAssignmentdetail(vehicle);
                                        //    if (savedItem == null)
                                        //    {
                                        //        return CommonResponse.Send(ResponseCodes.FAILURE, null, ResponseMessage.InternalServer500);
                                        //    }
                                        //}

                                        commander.Id = 0;
                                        commander.IsTemporarilyHeld = true;
                                        commander.DateTemporarilyHeld = DateTime.UtcNow;
                                        commander.TiedVehicleResourceId = TiedVehicleId;
                                        //commander.IsHeldForAction = true;
                                        //commander.DateHeldForAction = DateTime.UtcNow;
                                        commander.RequiredCount = resourceCount + 1;
                                        commander.CreatedById = context.GetLoggedInUserId();
                                        commander.CreatedAt = DateTime.UtcNow;
                                        var savedItem = await _serviceAssignmentDetailsRepository.SaveCommanderServiceAssignmentdetail(commander);
                                        if (savedItem == null)
                                        {
                                            transaction.Rollback();
                                            return CommonResponse.Send(ResponseCodes.FAILURE, null, ResponseMessage.InternalServer500);
                                        }

                                        //else
                                        //{
                                        //    if (getVehicleDetailListById.Count() < getVehicleServiceRegistration.VehicleQuantityRequired)
                                        //    {
                                        //        vehicle.IsTemporarilyHeld = true;
                                        //        vehicle.DateTemporarilyHeld = DateTime.UtcNow;
                                        //        vehicle.IsHeldForAction = true;
                                        //        vehicle.DateHeldForAction = DateTime.UtcNow;
                                        //        vehicle.RequiredCount = getVehicleDetailListById.Count() + 1;
                                        //        vehicle.CreatedById = context.GetLoggedInUserId();
                                        //        vehicle.CreatedAt = DateTime.UtcNow;
                                        //        var savedItem = await _serviceAssignmentDetailsRepository.SaveVehicleServiceAssignmentdetail(vehicle);
                                        //        if (savedItem == null)
                                        //        {
                                        //            return CommonResponse.Send(ResponseCodes.FAILURE, null, ResponseMessage.InternalServer500);
                                        //        }
                                        //    }
                                        //    else
                                        //    {
                                        //        return CommonResponse.Send(ResponseCodes.FAILURE, null, ResponseMessage.MaxQuantity445);
                                        //    }
                                        //}
                                    }
                                    else
                                    {
                                        transaction.Rollback();
                                        return CommonResponse.Send(ResponseCodes.FAILURE, null, ResponseMessage.NoResourceOnRoute443);
                                    }
                                    count++;
                                    if (count == getServiceRegistration.CommanderQuantityRequired)
                                    {
                                        break;
                                    }
                                }


                            }
                            else
                            {
                                transaction.Rollback();
                                return CommonResponse.Send(ResponseCodes.FAILURE, null, ResponseMessage.ResourceNotAvailble450);
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
                    var getServiceRegistration = await _serviceRegistrationRepository.FindServiceById(masterReceivingDTO.ServiceRegistrationId);
                    //var getVehicleDetail = await _serviceAssignmentDetailsRepository.FindVehicleServiceAssignmentDetailByResourceId(vehicleReceivingDTO.VehicleResourceId);
                    //var getResourceDetail = await _serviceAssignmentDetailsRepository.FindAllEscortServiceAssignmentDetails();
                    var getResourceDetailNoneHeld = await _serviceAssignmentDetailsRepository.FindAllNoneHeldEscortServiceAssignmentDetails();
                    //var getResourceDetailListById = await _serviceAssignmentDetailsRepository.FindAllEscortServiceAssignmentDetailsByAssignmentId(getId);
                    long? getResourceId = 0;
                    long count = 0;
                    var getResourceTypePerService = await _serviceRegistrationRepository.FindArmedEscortResourceByServiceRegId(masterReceivingDTO.ServiceRegistrationId);
                    //var getResourceSchedule = await _dTSMastersRepository.FindVehicleMasterByResourceId(vehicleReceivingDTO.VehicleResourceId);
                    if (getServiceRegistration.RequiresArmedEscort == true)
                    {
                        if (getResourceDetailNoneHeld.Count() >= getServiceRegistration.ArmedEscortQuantityRequired)
                        {
                            if (getResourceDetailNoneHeld.Count() != 0)
                            {
                                var lastItem = getResourceDetailNoneHeld.Last();
                                foreach (var _item in getResourceDetailNoneHeld)
                                {
                                    getResourceId = _item.ArmedEscortResourceId;
                                    var getResourceSchedule = await _dTSMastersRepository.FindArmedEscortMasterByResourceId2(getResourceId);
                                    var RouteExists = _armedEscortRegistrationRepository.GetServiceRegIdRegionAndRoute2(getResourceId, masterReceivingDTO.SMORouteId);


                                    if (getResourceSchedule != null)
                                    {
                                        if (masterReceivingDTO.PickupDate >= getResourceSchedule.AvailabilityStart && masterReceivingDTO.PickupDate <= getResourceSchedule.AvailablilityEnd)
                                        {
                                            if (getResourceSchedule.GenericDays.Count() != 0)
                                            {
                                                var genericVLastItem = getResourceSchedule.GenericDays.Last();
                                                foreach (var item in getResourceSchedule.GenericDays)
                                                {

                                                    if (!(masterReceivingDTO.PickoffTime.TimeOfDay >= item.OpeningTime.TimeOfDay && masterReceivingDTO.PickoffTime.TimeOfDay <= item.ClosingTime.TimeOfDay))
                                                    {
                                                        if (item.Equals(lastItem))
                                                        {
                                                            transaction.Rollback();
                                                            return CommonResponse.Send(ResponseCodes.FAILURE, null, ResponseMessage.NoGenericDay449);

                                                        }
                                                        else
                                                        {
                                                            continue;
                                                        }

                                                    }
                                                    if (masterReceivingDTO.PickoffTime.TimeOfDay >= item.OpeningTime.TimeOfDay && masterReceivingDTO.PickoffTime.TimeOfDay <= item.ClosingTime.TimeOfDay)
                                                    {
                                                        if (masterReceivingDTO.PickupDate.DayOfWeek.ToString() == "Monday" && item.Monday == true)
                                                        {
                                                            break;
                                                        }
                                                        else if (masterReceivingDTO.PickupDate.DayOfWeek.ToString() == "Tuesday" && item.Tuesday == true)
                                                        {
                                                            break;
                                                        }
                                                        else if (masterReceivingDTO.PickupDate.DayOfWeek.ToString() == "Wednesday" && item.Wednesday == true)
                                                        {
                                                            break;
                                                        }
                                                        else if (masterReceivingDTO.PickupDate.DayOfWeek.ToString() == "Thursday" && item.Thursday == true)
                                                        {
                                                            break;
                                                        }
                                                        else if (masterReceivingDTO.PickupDate.DayOfWeek.ToString() == "Friday" && item.Friday == true)
                                                        {
                                                            break;
                                                        }
                                                        else if (masterReceivingDTO.PickupDate.DayOfWeek.ToString() == "Saturday" && item.Saturday == true)
                                                        {
                                                            break;
                                                        }
                                                        else if (masterReceivingDTO.PickupDate.DayOfWeek.ToString() == "Sunday" && item.Sunday == true)
                                                        {
                                                            break;
                                                        }
                                                        else
                                                        {
                                                            return CommonResponse.Send(ResponseCodes.FAILURE, null, ResponseMessage.ScheduleTimeMismatch448);
                                                        }
                                                    }
                                                    else
                                                    {
                                                        return CommonResponse.Send(ResponseCodes.FAILURE, null, ResponseMessage.ScheduleTimeMismatch448);
                                                    }
                                                }
                                            }
                                            else
                                            {
                                                return CommonResponse.Send(ResponseCodes.FAILURE, null, ResponseMessage.NoGenericDay449);
                                            }
                                        }
                                        else
                                        {
                                            return CommonResponse.Send(ResponseCodes.FAILURE, null, ResponseMessage.ScheduleTimeMismatch448);
                                        }
                                    }
                                    else
                                    {
                                        return CommonResponse.Send(ResponseCodes.FAILURE, null, ResponseMessage.NoSchedule447);
                                    }
                                    if (RouteExists != null)
                                    {
                                        int resourceCount = 0;
                                        if (getResourceTypePerService != null)
                                        {
                                            var typeExists = _serviceRegistrationRepository.GetArmedEscortResourceApplicableTypeReqById(getServiceRegistration.Id, getResourceTypePerService.ArmedEscortTypeId);
                                            if (typeExists == null)
                                            {
                                                transaction.Rollback();
                                                return CommonResponse.Send(ResponseCodes.FAILURE, null, ResponseMessage.NoApplicableType446);
                                            }

                                        }
                                        else
                                        {
                                            transaction.Rollback();
                                            return CommonResponse.Send(ResponseCodes.FAILURE, null, ResponseMessage.NoApplicableType446);
                                        }
                                        //if (getVehicleDetail != null)
                                        //{
                                        //    if (getVehicleDetail.IsTemporarilyHeld == true || getVehicleDetail.IsHeldForAction == true)
                                        //    {
                                        //        return CommonResponse.Send(ResponseCodes.FAILURE, null, ResponseMessage.Held444);
                                        //    }
                                        //}
                                        //for (int i = 0; i < getServiceRegistration.VehicleQuantityRequired; i++)
                                        //{
                                        //    vehicle.Id = 0;
                                        //    vehicle.IsTemporarilyHeld = true;
                                        //    vehicle.DateTemporarilyHeld = DateTime.UtcNow;
                                        //    vehicle.IsHeldForAction = true;
                                        //    vehicle.DateHeldForAction = DateTime.UtcNow;
                                        //    vehicle.RequiredCount = getVehicleDetailListById.Count() + 1;
                                        //    vehicle.CreatedById = context.GetLoggedInUserId();
                                        //    vehicle.CreatedAt = DateTime.UtcNow;
                                        //    var savedItem = await _serviceAssignmentDetailsRepository.SaveVehicleServiceAssignmentdetail(vehicle);
                                        //    if (savedItem == null)
                                        //    {
                                        //        return CommonResponse.Send(ResponseCodes.FAILURE, null, ResponseMessage.InternalServer500);
                                        //    }
                                        //}

                                        armedEscort.Id = 0;
                                        armedEscort.IsTemporarilyHeld = true;
                                        armedEscort.DateTemporarilyHeld = DateTime.UtcNow;
                                        //armedEscort.IsHeldForAction = true;
                                        //armedEscort.DateHeldForAction = DateTime.UtcNow;
                                        armedEscort.RequiredCount = resourceCount + 1;
                                        armedEscort.CreatedById = context.GetLoggedInUserId();
                                        armedEscort.CreatedAt = DateTime.UtcNow;
                                        var savedItem = await _serviceAssignmentDetailsRepository.SaveEscortServiceAssignmentdetail(armedEscort);
                                        if (savedItem == null)
                                        {
                                            transaction.Rollback();
                                            return CommonResponse.Send(ResponseCodes.FAILURE, null, ResponseMessage.InternalServer500);
                                        }

                                        //else
                                        //{
                                        //    if (getVehicleDetailListById.Count() < getVehicleServiceRegistration.VehicleQuantityRequired)
                                        //    {
                                        //        vehicle.IsTemporarilyHeld = true;
                                        //        vehicle.DateTemporarilyHeld = DateTime.UtcNow;
                                        //        vehicle.IsHeldForAction = true;
                                        //        vehicle.DateHeldForAction = DateTime.UtcNow;
                                        //        vehicle.RequiredCount = getVehicleDetailListById.Count() + 1;
                                        //        vehicle.CreatedById = context.GetLoggedInUserId();
                                        //        vehicle.CreatedAt = DateTime.UtcNow;
                                        //        var savedItem = await _serviceAssignmentDetailsRepository.SaveVehicleServiceAssignmentdetail(vehicle);
                                        //        if (savedItem == null)
                                        //        {
                                        //            return CommonResponse.Send(ResponseCodes.FAILURE, null, ResponseMessage.InternalServer500);
                                        //        }
                                        //    }
                                        //    else
                                        //    {
                                        //        return CommonResponse.Send(ResponseCodes.FAILURE, null, ResponseMessage.MaxQuantity445);
                                        //    }
                                        //}
                                    }
                                    else
                                    {
                                        transaction.Rollback();
                                        return CommonResponse.Send(ResponseCodes.FAILURE, null, ResponseMessage.NoResourceOnRoute443);
                                    }
                                    count++;
                                    if (count == getServiceRegistration.ArmedEscortQuantityRequired)
                                    {
                                        break;
                                    }
                                }


                            }
                            else
                            {
                                transaction.Rollback();
                                return CommonResponse.Send(ResponseCodes.FAILURE, null, ResponseMessage.ResourceNotAvailble450);
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
            return CommonResponse.Send(ResponseCodes.SUCCESS, null, ResponseMessage.Success200);
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
