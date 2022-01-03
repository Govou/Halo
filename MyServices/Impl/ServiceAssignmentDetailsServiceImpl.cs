using AutoMapper;
using HaloBiz.DTOs.ApiDTOs;
using HaloBiz.DTOs.ReceivingDTOs;
using HaloBiz.DTOs.TransferDTOs;
using HaloBiz.Helpers;
using HaloBiz.Repository;
using HalobizMigrations.Models;
using HalobizMigrations.Models.Armada;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HaloBiz.MyServices.Impl
{
    public class ServiceAssignmentDetailsServiceImpl: IServiceAssignmentDetailsService
    {
        private readonly IServiceAssignmentDetailsRepository _serviceAssignmentDetailsRepository;
        private readonly ICommanderRegistrationRepository _commanderRegistrationRepository;
        private readonly IArmedEscortRegistrationRepository _armedEscortRegistrationRepository;
        private readonly IPilotRegistrationRepository _pilotRegistrationRepository;
        private readonly IVehicleRegistrationRepository _vehicleRegistrationRepository;
        private readonly IServiceRegistrationRepository _serviceRegistrationRepository;
        private readonly IServiceAssignmentMasterRepository _serviceAssignmentMasterRepository;
        private readonly IDTSMastersRepository _dTSMastersRepository;
        private readonly IDTSDetailGenericDaysRepository _dTSDetailGenericDaysRepository;
        private readonly IMapper _mapper;

        public ServiceAssignmentDetailsServiceImpl(IMapper mapper, IServiceAssignmentDetailsRepository serviceAssignmentDetailsRepository,
            ICommanderRegistrationRepository commanderRegistrationRepository, IArmedEscortRegistrationRepository armedEscortRegistrationRepository,
            IPilotRegistrationRepository pilotRegistrationRepository, IVehicleRegistrationRepository vehicleRegistrationRepository,  
            IServiceRegistrationRepository serviceRegistrationRepository, IServiceAssignmentMasterRepository serviceAssignmentMasterRepository, IDTSMastersRepository dTSMastersRepository,
            IDTSDetailGenericDaysRepository dTSDetailGenericDaysRepository)
        {
            _mapper = mapper;
            _serviceAssignmentDetailsRepository = serviceAssignmentDetailsRepository;
            _commanderRegistrationRepository = commanderRegistrationRepository;
            _armedEscortRegistrationRepository = armedEscortRegistrationRepository;
            _pilotRegistrationRepository = pilotRegistrationRepository;
            _vehicleRegistrationRepository = vehicleRegistrationRepository;
            _serviceRegistrationRepository = serviceRegistrationRepository;
            _serviceAssignmentMasterRepository = serviceAssignmentMasterRepository;
            _dTSMastersRepository = dTSMastersRepository;
            _dTSDetailGenericDaysRepository = dTSDetailGenericDaysRepository;
        }

        public async Task<ApiResponse> AddArmedEscortDetail(HttpContext context, ArmedEscortServiceAssignmentDetailsReceivingDTO armedEscortReceivingDTO)
        {
            var master = _mapper.Map<ArmedEscortServiceAssignmentDetail>(armedEscortReceivingDTO);
            var getEscort = await _serviceRegistrationRepository.FindServiceById(armedEscortReceivingDTO.ArmedEscortResourceId);
            
            var getServiceAssignment = await _serviceAssignmentMasterRepository.FindServiceAssignmentById(armedEscortReceivingDTO.ServiceAssignmentId);
            var getServiceRegistration = await _serviceRegistrationRepository.FindServiceById(getServiceAssignment.ServiceRegistration.Id);
            var getEscortDetail = await _serviceAssignmentDetailsRepository.FindEscortServiceAssignmentDetailByResourceId(armedEscortReceivingDTO.ArmedEscortResourceId);
            var getEscortDetailListById = await _serviceAssignmentDetailsRepository.FindAllEscortServiceAssignmentDetailsByAssignmentId(armedEscortReceivingDTO.ServiceAssignmentId);
            var RouteExists = _armedEscortRegistrationRepository.GetServiceRegIdRegionAndRoute(armedEscortReceivingDTO.ArmedEscortResourceId, getServiceAssignment.SMORouteId, getServiceAssignment.SMORegionId);
            var getResourceTypePerService = await _serviceRegistrationRepository.FindArmedEscortResourceByServiceRegId(getServiceAssignment.ServiceRegistration.Id);
            var typeExists = _serviceRegistrationRepository.GetArmedEscortResourceApplicableTypeReqById(getServiceRegistration.Id, getResourceTypePerService.ArmedEscortTypeId);
            var getResourceSchedule = await _dTSMastersRepository.FindArmedEscortMasterByResourceId(armedEscortReceivingDTO.ArmedEscortResourceId);
           // var getGenericDaysByDTSMasterId = await _dTSDetailGenericDaysRepository.FindArmedEscortGenericByMasterId(getResourceSchedule.Id);

            if (getResourceSchedule != null)
            {
                if(getResourceSchedule.AvailabilityStart >= getServiceAssignment.PickupDate )
                {
                    foreach(var item in getResourceSchedule.GenericDays)
                    {
                        if(item.OpeningTime.TimeOfDay >= getServiceAssignment.PickoffTime.TimeOfDay)
                        {
                            break;
                        }
                        else
                        {
                            return new ApiResponse(448);//for  schedule time
                        }
                    }
                   
                        if (RouteExists != null)
                        {
                            if (getEscortDetail != null)
                            {
                                if (getEscortDetail.IsTemporarilyHeld == true || getEscortDetail.IsHeldForAction == true)
                                {
                                    return new ApiResponse(444);
                                }
                            }
                            if (typeExists == null)
                            {
                                return new ApiResponse(446);
                            }
                            if (getEscortDetailListById.Count() == 0)
                            {

                                master.CreatedById = context.GetLoggedInUserId();
                                master.CreatedAt = DateTime.UtcNow;
                                master.IsTemporarilyHeld = true;
                                master.IsHeldForAction = true;
                                master.DateHeldForAction = DateTime.UtcNow;
                                master.RequiredCount = getEscortDetailListById.Count();
                                master.DateTemporarilyHeld = DateTime.UtcNow;
                                var savedItem = await _serviceAssignmentDetailsRepository.SaveEscortServiceAssignmentdetail(master);
                                if (savedItem == null)
                                {
                                    return new ApiResponse(500);
                                }
                            }
                            else
                            {
                                if (getEscortDetailListById.Count() < getServiceRegistration.ArmedEscortQuantityRequired)
                                {

                                    master.CreatedById = context.GetLoggedInUserId();
                                    master.CreatedAt = DateTime.UtcNow;
                                    master.IsHeldForAction = true;
                                    master.DateHeldForAction = DateTime.UtcNow;
                                    master.IsTemporarilyHeld = true;
                                    master.RequiredCount = getEscortDetailListById.Count();
                                    //master.RequiredCount = (int)getEscortDetail.ServiceAssignment.ServiceRegistration.ArmedEscortQuantityRequired;
                                    master.DateTemporarilyHeld = DateTime.UtcNow;
                                    var savedItem = await _serviceAssignmentDetailsRepository.SaveEscortServiceAssignmentdetail(master);
                                    if (savedItem == null)
                                    {
                                        return new ApiResponse(500);
                                    }
                                }
                                else
                                {
                                    return new ApiResponse(445);
                                }
                            }
                        }
                        else
                        {
                            return new ApiResponse(443); //for route
                        }
                }
                else
                {
                    return new ApiResponse(448); //for schedule date
                }
              
            }
            else
            {
                return new ApiResponse(447); //schedule check
            }
            var TransferDTO = _mapper.Map<ArmedEscortServiceAssignmentDetailsTransferDTO>(master);
            return new ApiOkResponse(TransferDTO);
        }

        public async Task<ApiResponse> AddCommanderDetail(HttpContext context, CommanderServiceAssignmentDetailsReceivingDTO commanderReceivingDTO)
        {
            var master = _mapper.Map<CommanderServiceAssignmentDetail>(commanderReceivingDTO);
            var getServiceAssignment = await _serviceAssignmentMasterRepository.FindServiceAssignmentById(commanderReceivingDTO.ServiceAssignmentId);
            var getServiceRegistration = await _serviceRegistrationRepository.FindServiceById(getServiceAssignment.ServiceRegistration.Id);
            var getCommanderDetail = await _serviceAssignmentDetailsRepository.FindCommanderServiceAssignmentDetailByResourceId(commanderReceivingDTO.CommanderResourceId);
            var getCommanderDetailListById = await _serviceAssignmentDetailsRepository.FindAllCommanderServiceAssignmentDetailsByAssignmentId(commanderReceivingDTO.ServiceAssignmentId);
            var RouteExists = _commanderRegistrationRepository.GetResourceRegIdRegionAndRouteId(commanderReceivingDTO.CommanderResourceId, getServiceAssignment.SMORouteId, getServiceAssignment.SMORegionId);
            var getResourceTypePerService = await _serviceRegistrationRepository.FindCommanderResourceByServiceRegId(getServiceAssignment.ServiceRegistration.Id);
            var typeExists = _serviceRegistrationRepository.GetCommanderResourceApplicableTypeReqById(getServiceRegistration.Id, getResourceTypePerService.CommanderTypeId);
            var getResourceSchedule = await _dTSMastersRepository.FindCommanderMasterByResourceId(commanderReceivingDTO.CommanderResourceId);

            if(getResourceSchedule != null)
            {
                if (getResourceSchedule.AvailabilityStart >= getServiceAssignment.PickupDate)
                {
                    foreach (var item in getResourceSchedule.GenericDays)
                    {
                        if (item.OpeningTime.TimeOfDay >= getServiceAssignment.PickoffTime.TimeOfDay)
                        {
                            break;
                        }
                        else
                        {
                            return new ApiResponse(448);//for  schedule time
                        }
                    }

                    if (RouteExists != null)
                    {
                        if (getCommanderDetail != null)
                        {
                            if (getCommanderDetail.IsTemporarilyHeld == true || getCommanderDetail.IsHeldForAction == true)
                            {
                                return new ApiResponse(444);
                            }
                        }
                        if (typeExists == null)
                        {
                            return new ApiResponse(446);
                        }
                        if (getCommanderDetailListById.Count() == 0)
                        {
                            master.IsTemporarilyHeld = true;
                            master.DateTemporarilyHeld = DateTime.UtcNow;
                            master.IsHeldForAction = true;
                            master.DateHeldForAction = DateTime.UtcNow;
                            master.RequiredCount = getCommanderDetailListById.Count();
                            master.CreatedById = context.GetLoggedInUserId();
                            master.CreatedAt = DateTime.UtcNow;
                            var savedItem = await _serviceAssignmentDetailsRepository.SaveCommanderServiceAssignmentdetail(master);
                            if (savedItem == null)
                            {
                                return new ApiResponse(500);
                            }
                        }
                        else
                        {
                            if (getCommanderDetailListById.Count() < getServiceRegistration.ArmedEscortQuantityRequired)
                            {
                                master.IsTemporarilyHeld = true;
                                master.DateTemporarilyHeld = DateTime.UtcNow;
                                master.IsHeldForAction = true;
                                master.DateHeldForAction = DateTime.UtcNow;
                                //master.RequiredCount = (int)getCommanderDetail.ServiceAssignment.ServiceRegistration.CommanderQuantityRequired;
                                master.RequiredCount = getCommanderDetailListById.Count();
                                master.CreatedById = context.GetLoggedInUserId();
                                master.CreatedAt = DateTime.UtcNow;
                                var savedItem = await _serviceAssignmentDetailsRepository.SaveCommanderServiceAssignmentdetail(master);
                                if (savedItem == null)
                                {
                                    return new ApiResponse(500);
                                }
                            }
                            else
                            {
                                return new ApiResponse(445);
                            }
                        }
                    }
                    else
                    {
                        return new ApiResponse(443);
                    }
                }
                else
                {
                    return new ApiResponse(448); //for schedule date
                }
            }
            else
            {
                return new ApiResponse(447);
            }
            var TransferDTO = _mapper.Map<CommanderServiceAssignmentDetailsTransferDTO>(master);
            return new ApiOkResponse(TransferDTO);
        }

        public async Task<ApiResponse> AddPassenger(HttpContext context, PassengerReceivingDTO passengerReceivingDTO)
        {
            var passenger = _mapper.Map<Passenger>(passengerReceivingDTO);
          
            passenger.CreatedById = context.GetLoggedInUserId();
            passenger.CreatedAt = DateTime.UtcNow;
            var savedItem = await _serviceAssignmentDetailsRepository.SavePassenger(passenger);
            if (savedItem == null)
            {
                return new ApiResponse(500);
            }
            var TransferDTO = _mapper.Map<PassengerTransferDTO>(passenger);
            return new ApiOkResponse(TransferDTO);
        }

        public async Task<ApiResponse> AddPilotDetail(HttpContext context, PilotServiceAssignmentDetailsReceivingDTO pilotReceivingDTO)
        {
            var master = _mapper.Map<PilotServiceAssignmentDetail>(pilotReceivingDTO);
            var getServiceAssignment = await _serviceAssignmentMasterRepository.FindServiceAssignmentById(pilotReceivingDTO.ServiceAssignmentId);
            var getServiceRegistration = await _serviceRegistrationRepository.FindServiceById(getServiceAssignment.ServiceRegistration.Id);
            var getPilotDetail = await _serviceAssignmentDetailsRepository.FindPilotServiceAssignmentDetailByResourceId(pilotReceivingDTO.PilotResourceId);
            var getPilotDetailListById = await _serviceAssignmentDetailsRepository.FindAllPilotServiceAssignmentDetailsByAssignmentId(pilotReceivingDTO.ServiceAssignmentId);
            var RouteExists = _pilotRegistrationRepository.GetResourceRegIdRegionAndRouteId(pilotReceivingDTO.PilotResourceId, getServiceAssignment.SMORouteId, getServiceAssignment.SMORegionId);
            var getResourceTypePerService = await _serviceRegistrationRepository.FindPilotResourceByServiceRegId(getServiceAssignment.ServiceRegistration.Id);
            var typeExists = _serviceRegistrationRepository.GetPilotResourceApplicableTypeReqById(getServiceRegistration.Id, getResourceTypePerService.PilotTypeId);
            var getResourceSchedule = await _dTSMastersRepository.FindPilotMasterByResourceId(pilotReceivingDTO.PilotResourceId);

            if (getResourceSchedule != null)
            {
                if (getResourceSchedule.AvailabilityStart >= getServiceAssignment.PickupDate)
                {
                    foreach (var item in getResourceSchedule.GenericDays)
                    {
                        if (item.OpeningTime.TimeOfDay >= getServiceAssignment.PickoffTime.TimeOfDay)
                        {
                            break;
                        }
                        else
                        {
                            return new ApiResponse(448);//for  schedule time
                        }
                    }

                    if (RouteExists != null)
                    {
                        if (getPilotDetail != null)
                        {
                            if (getPilotDetail.IsTemporarilyHeld == true || getPilotDetail.IsHeldForAction == true)
                            {
                                return new ApiResponse(444);
                            }
                        }
                        if (typeExists == null)
                        {
                            return new ApiResponse(446);
                        }
                        if (getPilotDetailListById.Count() == 0)
                        {
                            master.IsTemporarilyHeld = true;
                            master.DateTemporarilyHeld = DateTime.UtcNow;
                            master.IsHeldForAction = true;
                            master.DateHeldForAction = DateTime.UtcNow;
                            master.RequiredCount = getPilotDetailListById.Count();
                            //master.RequiredCount = (int)getPilotDetail.ServiceAssignment.ServiceRegistration.PilotQuantityRequired;
                            master.CreatedById = context.GetLoggedInUserId();
                            master.CreatedAt = DateTime.UtcNow;
                            var savedItem = await _serviceAssignmentDetailsRepository.SavePilotServiceAssignmentdetail(master);
                            if (savedItem == null)
                            {
                                return new ApiResponse(500);
                            }
                        }
                        else
                        {
                            if (getPilotDetailListById.Count() < getServiceRegistration.ArmedEscortQuantityRequired)
                            {
                                master.IsTemporarilyHeld = true;
                                master.DateTemporarilyHeld = DateTime.UtcNow;
                                master.IsHeldForAction = true;
                                master.DateHeldForAction = DateTime.UtcNow;
                                master.RequiredCount = getPilotDetailListById.Count();
                                master.CreatedById = context.GetLoggedInUserId();
                                master.CreatedAt = DateTime.UtcNow;
                                var savedItem = await _serviceAssignmentDetailsRepository.SavePilotServiceAssignmentdetail(master);
                                if (savedItem == null)
                                {
                                    return new ApiResponse(500);
                                }
                            }
                            else
                            {
                                return new ApiResponse(445);
                            }
                        }
                    }
                    else
                    {
                        return new ApiResponse(443);
                    }
                }
                else
                {
                    return new ApiResponse(448); //for schedule date
                }
            }
            else
            {
                return new ApiResponse(447);
            }
            var TransferDTO = _mapper.Map<PilotServiceAssignmentDetailsTransferDTO>(master);
            return new ApiOkResponse(TransferDTO);
        }

        public async Task<ApiResponse> AddVehicleDetail(HttpContext context, VehicleServiceAssignmentDetailsReceivingDTO vehicleReceivingDTO)
        {
            var master = _mapper.Map<VehicleServiceAssignmentDetail>(vehicleReceivingDTO);
            var getServiceAssignment = await _serviceAssignmentMasterRepository.FindServiceAssignmentById(vehicleReceivingDTO.ServiceAssignmentId);
            var getServiceRegistration = await _serviceRegistrationRepository.FindServiceById(getServiceAssignment.ServiceRegistration.Id);
            var getVehicleDetail = await _serviceAssignmentDetailsRepository.FindVehicleServiceAssignmentDetailByResourceId(vehicleReceivingDTO.VehicleResourceId);
            var getVehicleDetailListById = await _serviceAssignmentDetailsRepository.FindAllVehicleServiceAssignmentDetailsByAssignmentId(vehicleReceivingDTO.ServiceAssignmentId);
            var RouteExists = _vehicleRegistrationRepository.GetResourceRegIdRegionAndRouteId(vehicleReceivingDTO.VehicleResourceId, getServiceAssignment.SMORouteId, getServiceAssignment.SMORegionId);
            var getResourceTypePerService = await _serviceRegistrationRepository.FindVehicleResourceByServiceRegId(getServiceAssignment.ServiceRegistration.Id);
            var typeExists = _serviceRegistrationRepository.GetVehicleResourceApplicableTypeReqById(getServiceRegistration.Id, getResourceTypePerService.VehicleTypeId);
            var getResourceSchedule = await _dTSMastersRepository.FindVehicleMasterByResourceId(vehicleReceivingDTO.VehicleResourceId);

            if (getResourceSchedule != null)
            {
                if (getResourceSchedule.AvailabilityStart >= getServiceAssignment.PickupDate)
                {
                    foreach (var item in getResourceSchedule.GenericDays)
                    {
                        if (item.OpeningTime.TimeOfDay >= getServiceAssignment.PickoffTime.TimeOfDay)
                        {
                            break;
                        }
                        else
                        {
                            return new ApiResponse(448);//for  schedule time
                        }
                    }

                    if (RouteExists != null)
                    {
                        if (getVehicleDetail != null)
                        {
                            if (getVehicleDetail.IsTemporarilyHeld == true || getVehicleDetail.IsHeldForAction == true)
                            {
                                return new ApiResponse(444);
                            }
                        }
                        if (typeExists == null)
                        {
                            return new ApiResponse(446);
                        }
                        if (getVehicleDetailListById.Count() == 0)
                        {
                            master.IsTemporarilyHeld = true;
                            master.DateTemporarilyHeld = DateTime.UtcNow;
                            master.IsHeldForAction = true;
                            master.DateHeldForAction = DateTime.UtcNow;
                            master.RequiredCount = getVehicleDetailListById.Count();
                            master.CreatedById = context.GetLoggedInUserId();
                            master.CreatedAt = DateTime.UtcNow;
                            var savedItem = await _serviceAssignmentDetailsRepository.SaveVehicleServiceAssignmentdetail(master);
                            if (savedItem == null)
                            {
                                return new ApiResponse(500);
                            }
                        }
                        else
                        {
                            if (getVehicleDetailListById.Count() < getServiceRegistration.ArmedEscortQuantityRequired)
                            {
                                master.IsTemporarilyHeld = true;
                                master.DateTemporarilyHeld = DateTime.UtcNow;
                                master.IsHeldForAction = true;
                                master.DateHeldForAction = DateTime.UtcNow;
                                master.RequiredCount = getVehicleDetailListById.Count();
                                master.CreatedById = context.GetLoggedInUserId();
                                master.CreatedAt = DateTime.UtcNow;
                                var savedItem = await _serviceAssignmentDetailsRepository.SaveVehicleServiceAssignmentdetail(master);
                                if (savedItem == null)
                                {
                                    return new ApiResponse(500);
                                }
                            }
                            else
                            {
                                return new ApiResponse(445);
                            }
                        }
                    }
                    else
                    {
                        return new ApiResponse(443);
                    }
                }
                else
                {
                    return new ApiResponse(448); //for schedule date
                }
            }
            else
            {
                return new ApiResponse(447);
            }
          
            var TransferDTO = _mapper.Map<VehicleServiceAssignmentDetailsTransferDTO>(master);
            return new ApiOkResponse(TransferDTO);
        }

        public async Task<ApiResponse> DeleteArmedEscortDetail(long id)
        {
            var itemToDelete = await _serviceAssignmentDetailsRepository.FindEscortServiceAssignmentDetailById(id);

            if (itemToDelete == null)
            {
                return new ApiResponse(404);
            }

            if (!await _serviceAssignmentDetailsRepository.DeleteEscortServiceAssignmentDetail(itemToDelete))
            {
                return new ApiResponse(500);
            }

            return new ApiOkResponse(true);
        }

        public async Task<ApiResponse> DeleteCommanderDetail(long id)
        {
            var itemToDelete = await _serviceAssignmentDetailsRepository.FindCommanderServiceAssignmentDetailById(id);

            if (itemToDelete == null)
            {
                return new ApiResponse(404);
            }

            if (!await _serviceAssignmentDetailsRepository.DeleteCommanderServiceAssignmentDetail(itemToDelete))
            {
                return new ApiResponse(500);
            }

            return new ApiOkResponse(true);
        }

        public async Task<ApiResponse> DeletePassenger(long id)
        {
            var itemToDelete = await _serviceAssignmentDetailsRepository.FindPassengerById(id);

            if (itemToDelete == null)
            {
                return new ApiResponse(404);
            }

            if (!await _serviceAssignmentDetailsRepository.DeletePassenger(itemToDelete))
            {
                return new ApiResponse(500);
            }

            return new ApiOkResponse(true);
        }

        public async Task<ApiResponse> DeletePilotDetail(long id)
        {
            var itemToDelete = await _serviceAssignmentDetailsRepository.FindPilotServiceAssignmentDetailById(id);

            if (itemToDelete == null)
            {
                return new ApiResponse(404);
            }

            if (!await _serviceAssignmentDetailsRepository.DeletePilotServiceAssignmentDetail(itemToDelete))
            {
                return new ApiResponse(500);
            }

            return new ApiOkResponse(true);
        }

        public async Task<ApiResponse> DeleteVehicleDetail(long id)
        {
            var itemToDelete = await _serviceAssignmentDetailsRepository.FindVehicleServiceAssignmentDetailById(id);

            if (itemToDelete == null)
            {
                return new ApiResponse(404);
            }

            if (!await _serviceAssignmentDetailsRepository.DeleteVehicleServiceAssignmentDetail(itemToDelete))
            {
                return new ApiResponse(500);
            }

            return new ApiOkResponse(true);
        }

        public async Task<ApiResponse> GetAllArmedEscortDetails()
        {
            var master = await _serviceAssignmentDetailsRepository.FindAllEscortServiceAssignmentDetails();
            if (master == null)
            {
                return new ApiResponse(404);
            }
            var TransferDTO = _mapper.Map<IEnumerable<ArmedEscortServiceAssignmentDetailsTransferDTO>>(master);
            return new ApiOkResponse(TransferDTO);
        }

        public async Task<ApiResponse> GetAllArmedEscortDetailsByAssignmentId(long assignmentId)
        {
            var master = await _serviceAssignmentDetailsRepository.FindAllEscortServiceAssignmentDetailsByAssignmentId(assignmentId);
            if (master == null)
            {
                return new ApiResponse(404);
            }
            var TransferDTO = _mapper.Map<IEnumerable<ArmedEscortServiceAssignmentDetailsTransferDTO>>(master);
            return new ApiOkResponse(TransferDTO);
        }

        public async Task<ApiResponse> GetAllCommanderDetails()
        {
            var master = await _serviceAssignmentDetailsRepository.FindAllCommanderServiceAssignmentDetails();
            if (master == null)
            {
                return new ApiResponse(404);
            }
            var TransferDTO = _mapper.Map<IEnumerable<CommanderServiceAssignmentDetailsTransferDTO>>(master);
            return new ApiOkResponse(TransferDTO);
        }

        public async Task<ApiResponse> GetAllCommanderDetailsByAssignmentId(long assignmentId)
        {
            var master = await _serviceAssignmentDetailsRepository.FindAllCommanderServiceAssignmentDetailsByAssignmentId(assignmentId);
            if (master == null)
            {
                return new ApiResponse(404);
            }
            var TransferDTO = _mapper.Map<IEnumerable<CommanderServiceAssignmentDetailsTransferDTO>>(master);
            return new ApiOkResponse(TransferDTO);
        }

        public async Task<ApiResponse> GetAllPassengers()
        {
            var master = await _serviceAssignmentDetailsRepository.FindAllPassengers();
            if (master == null)
            {
                return new ApiResponse(404);
            }
            var TransferDTO = _mapper.Map<IEnumerable<PassengerTransferDTO>>(master);
            return new ApiOkResponse(TransferDTO);
        }

        public async Task<ApiResponse> GetAllPassengersByAssignmentId(long assignmentId)
        {
            var master = await _serviceAssignmentDetailsRepository.FindAllPassengersByAssignmentId(assignmentId);
            if (master == null)
            {
                return new ApiResponse(404);
            }
            var TransferDTO = _mapper.Map<IEnumerable<PassengerTransferDTO>>(master);
            return new ApiOkResponse(TransferDTO);
        }

        public async Task<ApiResponse> GetAllPilotDetails()
        {
            var master = await _serviceAssignmentDetailsRepository.FindAllPilotServiceAssignmentDetails();
            if (master == null)
            {
                return new ApiResponse(404);
            }
            var TransferDTO = _mapper.Map<IEnumerable<PilotServiceAssignmentDetailsTransferDTO>>(master);
            return new ApiOkResponse(TransferDTO);
        }

        public async Task<ApiResponse> GetAllPilotDetailsByAssignmentId(long assignmentId)
        {
            var master = await _serviceAssignmentDetailsRepository.FindAllPilotServiceAssignmentDetailsByAssignmentId(assignmentId);
            if (master == null)
            {
                return new ApiResponse(404);
            }
            var TransferDTO = _mapper.Map<IEnumerable<PilotServiceAssignmentDetailsTransferDTO>>(master);
            return new ApiOkResponse(TransferDTO);
        }

        public async Task<ApiResponse> GetAllVehicleDetails()
        {
            var master = await _serviceAssignmentDetailsRepository.FindAllVehicleServiceAssignmentDetails();
            if (master == null)
            {
                return new ApiResponse(404);
            }
            var TransferDTO = _mapper.Map<IEnumerable<VehicleServiceAssignmentDetailsTransferDTO>>(master);
            return new ApiOkResponse(TransferDTO);
        }

        public async Task<ApiResponse> GetAllVehicleDetailsByAssignmentId(long assignmentId)
        {
            var master = await _serviceAssignmentDetailsRepository.FindAllVehicleServiceAssignmentDetailsByAssignmentId(assignmentId);
            if (master == null)
            {
                return new ApiResponse(404);
            }
            var TransferDTO = _mapper.Map<IEnumerable<VehicleServiceAssignmentDetailsTransferDTO>>(master);
            return new ApiOkResponse(TransferDTO);
        }

        public async Task<ApiResponse> GetArmedEscortDetailById(long id)
        {
            var master = await _serviceAssignmentDetailsRepository.FindEscortServiceAssignmentDetailById(id);
            if (master == null)
            {
                return new ApiResponse(404);
            }
            var TransferDTO = _mapper.Map<ArmedEscortServiceAssignmentDetailsTransferDTO>(master);
            return new ApiOkResponse(TransferDTO);
        }

        public async Task<ApiResponse> GetCommanderDetailById(long id)
        {
            var master = await _serviceAssignmentDetailsRepository.FindCommanderServiceAssignmentDetailById(id);
            if (master == null)
            {
                return new ApiResponse(404);
            }
            var TransferDTO = _mapper.Map<CommanderServiceAssignmentDetailsTransferDTO>(master);
            return new ApiOkResponse(TransferDTO);
        }

        public async Task<ApiResponse> GetPassengerById(long id)
        {
            var master = await _serviceAssignmentDetailsRepository.FindPassengerById(id);
            if (master == null)
            {
                return new ApiResponse(404);
            }
            var TransferDTO = _mapper.Map<PassengerTransferDTO>(master);
            return new ApiOkResponse(TransferDTO);
        }

        public async Task<ApiResponse> GetPilotDetailById(long id)
        {
            var master = await _serviceAssignmentDetailsRepository.FindPilotServiceAssignmentDetailById(id);
            if (master == null)
            {
                return new ApiResponse(404);
            }
            var TransferDTO = _mapper.Map<PilotServiceAssignmentDetailsTransferDTO>(master);
            return new ApiOkResponse(TransferDTO);
        }

        public async Task<ApiResponse> GetVehicleDetailById(long id)
        {
            var master = await _serviceAssignmentDetailsRepository.FindVehicleServiceAssignmentDetailById(id);
            if (master == null)
            {
                return new ApiResponse(404);
            }
            var TransferDTO = _mapper.Map<VehicleServiceAssignmentDetailsTransferDTO>(master);
            return new ApiOkResponse(TransferDTO);
        }

        public async Task<ApiResponse> UpdateArmedEscortDetail(HttpContext context, long id, ArmedEscortServiceAssignmentDetailsReceivingDTO armedEscortReceivingDTO)
        {
            var itemToUpdate = await _serviceAssignmentDetailsRepository.FindEscortServiceAssignmentDetailById(id);
            if (itemToUpdate == null)
            {
                return new ApiResponse(404);
            }

            var summary = $"Initial details before change, \n {itemToUpdate.ToString()} \n";

            //itemToUpdate.ArmedEscortResourceId = masterReceivingDTO.DropoffLocation;
            //itemToUpdate.PickoffLocation = masterReceivingDTO.PickoffLocation;
            //itemToUpdate.SourceTypeId = masterReceivingDTO.SourceTypeId;

            itemToUpdate.UpdatedAt = DateTime.UtcNow;
            var updatedItem = await _serviceAssignmentDetailsRepository.UpdateEscortServiceAssignmentDetail(itemToUpdate);

            summary += $"Details after change, \n {updatedItem.ToString()} \n";

            if (updatedItem == null)
            {
                return new ApiResponse(500);
            }

            var TransferDTOs = _mapper.Map<ArmedEscortServiceAssignmentDetailsTransferDTO>(updatedItem);
            return new ApiOkResponse(TransferDTOs);
        }

        public async Task<ApiResponse> UpdateCommanderDetail(HttpContext context, long id, CommanderServiceAssignmentDetailsReceivingDTO commanderReceivingDTO)
        {
            var itemToUpdate = await _serviceAssignmentDetailsRepository.FindCommanderServiceAssignmentDetailById(id);
            if (itemToUpdate == null)
            {
                return new ApiResponse(404);
            }

            var summary = $"Initial details before change, \n {itemToUpdate.ToString()} \n";

            //itemToUpdate.ArmedEscortResourceId = masterReceivingDTO.DropoffLocation;
            //itemToUpdate.PickoffLocation = masterReceivingDTO.PickoffLocation;
            //itemToUpdate.SourceTypeId = masterReceivingDTO.SourceTypeId;

            itemToUpdate.UpdatedAt = DateTime.UtcNow;
            var updatedItem = await _serviceAssignmentDetailsRepository.UpdateCommanderServiceAssignmentDetail(itemToUpdate);

            summary += $"Details after change, \n {updatedItem.ToString()} \n";

            if (updatedItem == null)
            {
                return new ApiResponse(500);
            }

            var TransferDTOs = _mapper.Map<CommanderServiceAssignmentDetailsTransferDTO>(updatedItem);
            return new ApiOkResponse(TransferDTOs);
        }

        public async Task<ApiResponse> UpdatePassenger(HttpContext context, long id, PassengerReceivingDTO passengerReceivingDTO)
        {
            var itemToUpdate = await _serviceAssignmentDetailsRepository.FindPassengerById(id);
            if (itemToUpdate == null)
            {
                return new ApiResponse(404);
            }

            var summary = $"Initial details before change, \n {itemToUpdate.ToString()} \n";

            itemToUpdate.Firstname = passengerReceivingDTO.Firstname;
            itemToUpdate.Lastname = passengerReceivingDTO.Lastname;
            itemToUpdate.Othername = passengerReceivingDTO.Othername;
            itemToUpdate.InstagramHandle = passengerReceivingDTO.InstagramHandle;
            itemToUpdate.FacebooHandle = passengerReceivingDTO.FacebooHandle;
            itemToUpdate.PassengerTypeId = passengerReceivingDTO.PassengerTypeId;
            itemToUpdate.Mobile = passengerReceivingDTO.Mobile;
            itemToUpdate.TwitterHandle = passengerReceivingDTO.TwitterHandle;

            itemToUpdate.UpdatedAt = DateTime.UtcNow;
            var updatedItem = await _serviceAssignmentDetailsRepository.UpdatePassenger(itemToUpdate);

            summary += $"Details after change, \n {updatedItem.ToString()} \n";

            if (updatedItem == null)
            {
                return new ApiResponse(500);
            }

            var TransferDTOs = _mapper.Map<PassengerTransferDTO>(updatedItem);
            return new ApiOkResponse(TransferDTOs);
        }

        public async Task<ApiResponse> UpdatePilotDetail(HttpContext context, long id, PilotServiceAssignmentDetailsReceivingDTO pilotReceivingDTO)
        {
            var itemToUpdate = await _serviceAssignmentDetailsRepository.FindPilotServiceAssignmentDetailById(id);
            if (itemToUpdate == null)
            {
                return new ApiResponse(404);
            }

            var summary = $"Initial details before change, \n {itemToUpdate.ToString()} \n";

            //itemToUpdate.ArmedEscortResourceId = masterReceivingDTO.DropoffLocation;
            //itemToUpdate.PickoffLocation = masterReceivingDTO.PickoffLocation;
            //itemToUpdate.SourceTypeId = masterReceivingDTO.SourceTypeId;

            itemToUpdate.UpdatedAt = DateTime.UtcNow;
            var updatedItem = await _serviceAssignmentDetailsRepository.UpdatePilotServiceAssignmentDetail(itemToUpdate);

            summary += $"Details after change, \n {updatedItem.ToString()} \n";

            if (updatedItem == null)
            {
                return new ApiResponse(500);
            }

            var TransferDTOs = _mapper.Map<PilotServiceAssignmentDetailsTransferDTO>(updatedItem);
            return new ApiOkResponse(TransferDTOs);
        }

        public async Task<ApiResponse> UpdateVehicleDetail(HttpContext context, long id, VehicleServiceAssignmentDetailsReceivingDTO vehicleReceivingDTO)
        {
            var itemToUpdate = await _serviceAssignmentDetailsRepository.FindVehicleServiceAssignmentDetailById(id);
            if (itemToUpdate == null)
            {
                return new ApiResponse(404);
            }

            var summary = $"Initial details before change, \n {itemToUpdate.ToString()} \n";

            //itemToUpdate.ArmedEscortResourceId = masterReceivingDTO.DropoffLocation;
            //itemToUpdate.PickoffLocation = masterReceivingDTO.PickoffLocation;
            //itemToUpdate.SourceTypeId = masterReceivingDTO.SourceTypeId;

            itemToUpdate.UpdatedAt = DateTime.UtcNow;
            var updatedItem = await _serviceAssignmentDetailsRepository.UpdateVehicleServiceAssignmentDetail(itemToUpdate);

            summary += $"Details after change, \n {updatedItem.ToString()} \n";

            if (updatedItem == null)
            {
                return new ApiResponse(500);
            }

            var TransferDTOs = _mapper.Map<VehicleServiceAssignmentDetailsTransferDTO>(updatedItem);
            return new ApiOkResponse(TransferDTOs);
        }
    }
}
