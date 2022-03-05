﻿using AutoMapper;
using HaloBiz.DTOs.ApiDTOs;
using HaloBiz.DTOs.GenericResponseDTO;
using HaloBiz.DTOs.ReceivingDTOs;
using HaloBiz.DTOs.TransferDTOs;
using HaloBiz.DTOs.TransferDTOs.LAMS;
using HaloBiz.Helpers;
using HaloBiz.Repository;
using HalobizMigrations.Models;
using HalobizMigrations.Models.Armada;
using Microsoft.AspNetCore.Http;
using RestSharp;
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

        public async Task<ApiCommonResponse> AddArmedEscortDetail(HttpContext context, ArmedEscortServiceAssignmentDetailsReceivingDTO armedEscortReceivingDTO)
        {
            var master = _mapper.Map<ArmedEscortServiceAssignmentDetail>(armedEscortReceivingDTO);
            var getEscort = await _serviceRegistrationRepository.FindServiceById(armedEscortReceivingDTO.ArmedEscortResourceId);
            
            var getServiceAssignment = await _serviceAssignmentMasterRepository.FindServiceAssignmentById(armedEscortReceivingDTO.ServiceAssignmentId);
            var getServiceRegistration = await _serviceRegistrationRepository.FindServiceById(getServiceAssignment.ServiceRegistration.Id);
            var getEscortDetail = await _serviceAssignmentDetailsRepository.FindEscortServiceAssignmentDetailByResourceId(armedEscortReceivingDTO.ArmedEscortResourceId);
            var getEscortDetailListById = await _serviceAssignmentDetailsRepository.FindAllEscortServiceAssignmentDetailsByAssignmentId(armedEscortReceivingDTO.ServiceAssignmentId);
            var RouteExists = _armedEscortRegistrationRepository.GetServiceRegIdRegionAndRoute(armedEscortReceivingDTO.ArmedEscortResourceId, getServiceAssignment.SMORouteId);
            var getResourceTypePerService = await _serviceRegistrationRepository.FindArmedEscortResourceByServiceRegId(getServiceAssignment.ServiceRegistration.Id);
            var getResourceSchedule = await _dTSMastersRepository.FindArmedEscortMasterByResourceId(armedEscortReceivingDTO.ArmedEscortResourceId);
           // var getGenericDaysByDTSMasterId = await _dTSDetailGenericDaysRepository.FindArmedEscortGenericByMasterId(getResourceSchedule.Id);

            if (getResourceSchedule != null)
            {
                if(  getServiceAssignment.PickupDate >= getResourceSchedule.AvailabilityStart && getServiceAssignment.PickupDate <= getResourceSchedule.AvailablilityEnd)
                {

                    if (getResourceSchedule.GenericDays.Count() != 0)
                    {
                        var lastItem = getResourceSchedule.GenericDays.Last();
                        foreach (var item in getResourceSchedule.GenericDays)
                        {
                            if (!(getServiceAssignment.PickoffTime.TimeOfDay >= item.OpeningTime.TimeOfDay && getServiceAssignment.PickoffTime.TimeOfDay <= item.ClosingTime.TimeOfDay))
                            {
                                if (item.Equals(lastItem))
                                {
                                    return CommonResponse.Send(ResponseCodes.FAILURE, null, ResponseMessage.NoGenericDay449);

                                }
                                else
                                {
                                    continue;
                                }

                            }
                            if (getServiceAssignment.PickoffTime.TimeOfDay >= item.OpeningTime.TimeOfDay && getServiceAssignment.PickoffTime.TimeOfDay <= item.ClosingTime.TimeOfDay)
                            {
                                if (getServiceAssignment.PickupDate.DayOfWeek.ToString() == "Monday" && item.Monday == true)
                                {
                                    break;
                                }
                                else if (getServiceAssignment.PickupDate.DayOfWeek.ToString() == "Tuesday" && item.Tuesday == true)
                                {
                                    break;
                                }
                                else if (getServiceAssignment.PickupDate.DayOfWeek.ToString() == "Wednesday" && item.Wednesday == true)
                                {
                                    break;
                                }
                                else if (getServiceAssignment.PickupDate.DayOfWeek.ToString() == "Thursday" && item.Thursday == true)
                                {
                                    break;
                                }
                                else if (getServiceAssignment.PickupDate.DayOfWeek.ToString() == "Friday" && item.Friday == true)
                                {
                                    break;
                                }
                                else if (getServiceAssignment.PickupDate.DayOfWeek.ToString() == "Saturday" && item.Saturday == true)
                                {
                                    break;
                                }
                                else if (getServiceAssignment.PickupDate.DayOfWeek.ToString() == "Sunday" && item.Sunday == true)
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
                                //return new ApiResponse(448);//for  schedule time
                                return CommonResponse.Send(ResponseCodes.FAILURE, null, ResponseMessage.ScheduleTimeMismatch448);
                            }
                        }
                    }
                    else
                    {
                        return CommonResponse.Send(ResponseCodes.FAILURE, null, ResponseMessage.NoGenericDay449);
                    }
                   
                   
                        if (RouteExists != null)
                        {
                        if (getResourceTypePerService != null)
                        {
                            var typeExists = _serviceRegistrationRepository.GetArmedEscortResourceApplicableTypeReqById(getServiceRegistration.Id, getResourceTypePerService.ArmedEscortTypeId);
                            if (typeExists == null)
                            {
                                return CommonResponse.Send(ResponseCodes.FAILURE, null, ResponseMessage.NoApplicableType446);
                            }
                        }
                        else
                        {
                            return CommonResponse.Send(ResponseCodes.FAILURE, null, ResponseMessage.NoApplicableType446);
                        }
                        if (getEscortDetail != null)
                            {
                                if (getEscortDetail.IsTemporarilyHeld == true || getEscortDetail.IsHeldForAction == true)
                                {
                                    //return new ApiResponse(444);
                                return CommonResponse.Send(ResponseCodes.FAILURE, null, ResponseMessage.Held444);
                            }
                            }
                           
                            if (getEscortDetailListById.Count() == 0)
                            {

                                master.CreatedById = context.GetLoggedInUserId();
                                master.CreatedAt = DateTime.UtcNow;
                                master.IsTemporarilyHeld = true;
                                master.IsHeldForAction = true;
                                master.DateHeldForAction = DateTime.UtcNow;
                                master.RequiredCount = getEscortDetailListById.Count()+1;
                                master.DateTemporarilyHeld = DateTime.UtcNow;
                                var savedItem = await _serviceAssignmentDetailsRepository.SaveEscortServiceAssignmentdetail(master);
                                if (savedItem == null)
                                {
                                //return  CommonResponse.Send(ResponseCodes.FAILURE, null, ResponseMessage.InternalServer500);
                                return CommonResponse.Send(ResponseCodes.FAILURE, null, ResponseMessage.InternalServer500);
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
                                    master.RequiredCount = getEscortDetailListById.Count()+1;
                                    //master.RequiredCount = (int)getEscortDetail.ServiceAssignment.ServiceRegistration.ArmedEscortQuantityRequired;
                                    master.DateTemporarilyHeld = DateTime.UtcNow;
                                    var savedItem = await _serviceAssignmentDetailsRepository.SaveEscortServiceAssignmentdetail(master);
                                    if (savedItem == null)
                                    {
                                        //return  CommonResponse.Send(ResponseCodes.FAILURE, null, ResponseMessage.InternalServer500);
                                    return CommonResponse.Send(ResponseCodes.FAILURE, null, ResponseMessage.InternalServer500);
                                }
                                }
                                else
                                {
                                    //return new ApiResponse(445);
                                return CommonResponse.Send(ResponseCodes.FAILURE, null, ResponseMessage.MaxQuantity445);
                            }
                            }
                        }
                        else
                        {
                        return CommonResponse.Send(ResponseCodes.FAILURE, null, ResponseMessage.NoResourceOnRoute443);
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

            var _getEscortDetailListById = await _serviceAssignmentDetailsRepository.FindAllEscortServiceAssignmentDetailsByAssignmentId(armedEscortReceivingDTO.ServiceAssignmentId);
            if (_getEscortDetailListById.Count() == getServiceRegistration.ArmedEscortQuantityRequired)
            {
                var getPilotDetailListById = await _serviceAssignmentDetailsRepository.FindAllPilotServiceAssignmentDetailsByAssignmentId(getServiceAssignment.Id);
                var getVehicleDetailListById = await _serviceAssignmentDetailsRepository.FindAllVehicleServiceAssignmentDetailsByAssignmentId(getServiceAssignment.Id);
                var getCommanderDetailListById = await _serviceAssignmentDetailsRepository.FindAllCommanderServiceAssignmentDetailsByAssignmentId(getServiceAssignment.Id);

                //Check For all Required
                if((getCommanderDetailListById.Count() == getServiceRegistration.CommanderQuantityRequired && getServiceRegistration.RequiresCommander == true)
                    && (getVehicleDetailListById.Count() == getServiceRegistration.VehicleQuantityRequired && getServiceRegistration.RequiresVehicle == true)
                    && (getPilotDetailListById.Count() == getServiceRegistration.PilotQuantityRequired && getServiceRegistration.RequiresPilot == true))
                {
                    var itemToUpdate = await _serviceAssignmentMasterRepository.FindServiceAssignmentById(armedEscortReceivingDTO.ServiceAssignmentId);

                    if (itemToUpdate == null)
                    {
                        return CommonResponse.Send(ResponseCodes.NO_DATA_AVAILABLE);
                    }

                    if (!await _serviceAssignmentMasterRepository.UpdateReadyStatus(itemToUpdate))
                    {
                        return CommonResponse.Send(ResponseCodes.FAILURE, null, ResponseMessage.InternalServer500);
                    }
                }
                else  //Check For all but where pilot not required
                if ((getCommanderDetailListById.Count() == getServiceRegistration.CommanderQuantityRequired && getServiceRegistration.RequiresCommander == true)
                    && (getVehicleDetailListById.Count() == getServiceRegistration.VehicleQuantityRequired && getServiceRegistration.RequiresVehicle == true)
                    && (getServiceRegistration.RequiresPilot == false))
                {
                    var itemToUpdate = await _serviceAssignmentMasterRepository.FindServiceAssignmentById(armedEscortReceivingDTO.ServiceAssignmentId);

                    if (itemToUpdate == null)
                    {
                        return CommonResponse.Send(ResponseCodes.NO_DATA_AVAILABLE);
                    }

                    if (!await _serviceAssignmentMasterRepository.UpdateReadyStatus(itemToUpdate))
                    {
                        return CommonResponse.Send(ResponseCodes.FAILURE, null, ResponseMessage.InternalServer500);
                    }
                }
                //check for all but vehicle not required
                else if ((getCommanderDetailListById.Count() == getServiceRegistration.CommanderQuantityRequired && getServiceRegistration.RequiresCommander == true)
                 && ( getServiceRegistration.RequiresVehicle == false)
                 && (getPilotDetailListById.Count() == getServiceRegistration.PilotQuantityRequired && getServiceRegistration.RequiresPilot == true))
                {
                    var itemToUpdate = await _serviceAssignmentMasterRepository.FindServiceAssignmentById(armedEscortReceivingDTO.ServiceAssignmentId);

                    if (itemToUpdate == null)
                    {
                        return CommonResponse.Send(ResponseCodes.NO_DATA_AVAILABLE);
                    }

                    if (!await _serviceAssignmentMasterRepository.UpdateReadyStatus(itemToUpdate))
                    {
                        return CommonResponse.Send(ResponseCodes.FAILURE, null, ResponseMessage.InternalServer500);
                    }
                }
                //Check For all but commander not required
                else if (( getServiceRegistration.RequiresCommander == false)
                    && (getVehicleDetailListById.Count() == getServiceRegistration.VehicleQuantityRequired && getServiceRegistration.RequiresVehicle == true)
                    && (getPilotDetailListById.Count() == getServiceRegistration.PilotQuantityRequired && getServiceRegistration.RequiresPilot == true))
                {
                    var itemToUpdate = await _serviceAssignmentMasterRepository.FindServiceAssignmentById(armedEscortReceivingDTO.ServiceAssignmentId);

                    if (itemToUpdate == null)
                    {
                        return CommonResponse.Send(ResponseCodes.NO_DATA_AVAILABLE);
                    }

                    if (!await _serviceAssignmentMasterRepository.UpdateReadyStatus(itemToUpdate))
                    {
                        return CommonResponse.Send(ResponseCodes.FAILURE, null, ResponseMessage.InternalServer500);
                    }
                }

                //Only commander required and escort
                else if ((getCommanderDetailListById.Count() == getServiceRegistration.CommanderQuantityRequired && getServiceRegistration.RequiresCommander == true)
                    && ( getServiceRegistration.RequiresVehicle == false)
                    && ( getServiceRegistration.RequiresPilot == false))
                {
                    var itemToUpdate = await _serviceAssignmentMasterRepository.FindServiceAssignmentById(armedEscortReceivingDTO.ServiceAssignmentId);

                    if (itemToUpdate == null)
                    {
                        return CommonResponse.Send(ResponseCodes.NO_DATA_AVAILABLE);
                    }

                    if (!await _serviceAssignmentMasterRepository.UpdateReadyStatus(itemToUpdate))
                    {
                        return CommonResponse.Send(ResponseCodes.FAILURE, null, ResponseMessage.InternalServer500);
                    }
                }
                //Only Vehicle required and escort
                else if (( getServiceRegistration.RequiresCommander == false)
                    && (getVehicleDetailListById.Count() == getServiceRegistration.VehicleQuantityRequired && getServiceRegistration.RequiresVehicle == true)
                    && (getServiceRegistration.RequiresPilot == false))
                {
                    var itemToUpdate = await _serviceAssignmentMasterRepository.FindServiceAssignmentById(armedEscortReceivingDTO.ServiceAssignmentId);

                    if (itemToUpdate == null)
                    {
                        return CommonResponse.Send(ResponseCodes.NO_DATA_AVAILABLE);
                    }

                    if (!await _serviceAssignmentMasterRepository.UpdateReadyStatus(itemToUpdate))
                    {
                        return CommonResponse.Send(ResponseCodes.FAILURE, null, ResponseMessage.InternalServer500);
                    }
                }

                //Only Pilot required and escort
                else if ((getServiceRegistration.RequiresCommander == false)
                    && ( getServiceRegistration.RequiresVehicle == false)
                    && (getPilotDetailListById.Count() == getServiceRegistration.PilotQuantityRequired && getServiceRegistration.RequiresPilot == true))
                {
                    var itemToUpdate = await _serviceAssignmentMasterRepository.FindServiceAssignmentById(armedEscortReceivingDTO.ServiceAssignmentId);

                    if (itemToUpdate == null)
                    {
                        return CommonResponse.Send(ResponseCodes.NO_DATA_AVAILABLE);
                    }

                    if (!await _serviceAssignmentMasterRepository.UpdateReadyStatus(itemToUpdate))
                    {
                        return CommonResponse.Send(ResponseCodes.FAILURE, null, ResponseMessage.InternalServer500);
                    }
                }

                //Only escort required 
                else if ((getServiceRegistration.RequiresCommander == false)
                    && (getServiceRegistration.RequiresVehicle == false)
                    && ( getServiceRegistration.RequiresPilot == false))
                {
                    var itemToUpdate = await _serviceAssignmentMasterRepository.FindServiceAssignmentById(armedEscortReceivingDTO.ServiceAssignmentId);

                    if (itemToUpdate == null)
                    {
                        return CommonResponse.Send(ResponseCodes.NO_DATA_AVAILABLE);
                    }

                    if (!await _serviceAssignmentMasterRepository.UpdateReadyStatus(itemToUpdate))
                    {
                        return CommonResponse.Send(ResponseCodes.FAILURE, null, ResponseMessage.InternalServer500);
                    }
                }


                //if (getServiceRegistration.RequiresVehicle == true  || getServiceRegistration.RequiresCommander == true || getServiceRegistration.RequiresPilot == true)
                //{
                //    if ( getServiceRegistration.RequiresVehicle == true)
                //    {
                //        if (getVehicleDetailListById.Count() == getServiceRegistration.VehicleQuantityRequired )
                //        {
                //            if ( getServiceRegistration.RequiresCommander == true)
                //            {
                //                if(getCommanderDetailListById.Count() == getServiceRegistration.CommanderQuantityRequired)
                //                {
                //                    if (getPilotDetailListById.Count() == getServiceRegistration.PilotQuantityRequired && getServiceRegistration.RequiresPilot == true)
                //                    {
                //                        var itemToUpdate = await _serviceAssignmentMasterRepository.FindServiceAssignmentById(armedEscortReceivingDTO.ServiceAssignmentId);

                //                        if (itemToUpdate == null)
                //                        {
                //                            return CommonResponse.Send(ResponseCodes.NO_DATA_AVAILABLE);
                //                        }

                //                        if (!await _serviceAssignmentMasterRepository.UpdateReadyStatus(itemToUpdate))
                //                        {
                //                            return CommonResponse.Send(ResponseCodes.FAILURE, null, ResponseMessage.InternalServer500);
                //                        }
                //                    }
                //                }

                //            }
                //        }

                //    }
                //}
            }
            var TransferDTO = _mapper.Map<ArmedEscortServiceAssignmentDetailsTransferDTO>(master);
            //return CommonResponse.Send(ResponseCodes.SUCCESS, null, ResponseMessage.Success200);
            return CommonResponse.Send(ResponseCodes.SUCCESS, TransferDTO);
        }

        public async Task<ApiCommonResponse> AddCommanderDetail(HttpContext context, CommanderServiceAssignmentDetailsReceivingDTO commanderReceivingDTO)
        {
            var master = _mapper.Map<CommanderServiceAssignmentDetail>(commanderReceivingDTO);
            var getServiceAssignment = await _serviceAssignmentMasterRepository.FindServiceAssignmentById(commanderReceivingDTO.ServiceAssignmentId);
            var getServiceRegistration = await _serviceRegistrationRepository.FindServiceById(getServiceAssignment.ServiceRegistration.Id);
            var getCommanderDetail = await _serviceAssignmentDetailsRepository.FindCommanderServiceAssignmentDetailByResourceId(commanderReceivingDTO.CommanderResourceId);
            var getCommanderDetailListById = await _serviceAssignmentDetailsRepository.FindAllCommanderServiceAssignmentDetailsByAssignmentId(commanderReceivingDTO.ServiceAssignmentId);
            var RouteExists = _commanderRegistrationRepository.GetResourceRegIdRegionAndRouteId(commanderReceivingDTO.CommanderResourceId, getServiceAssignment.SMORouteId);
            var getResourceTypePerService = await _serviceRegistrationRepository.FindCommanderResourceByServiceRegId(getServiceAssignment.ServiceRegistration.Id);
            //var typeExists = _serviceRegistrationRepository.GetCommanderResourceApplicableTypeReqById(getServiceRegistration.Id, getResourceTypePerService.CommanderTypeId);
           
            var getResourceSchedule = await _dTSMastersRepository.FindCommanderMasterByResourceId(commanderReceivingDTO.CommanderResourceId);

            if(getResourceSchedule != null)
            {
                if (getServiceAssignment.PickupDate >= getResourceSchedule.AvailabilityStart && getServiceAssignment.PickupDate <= getResourceSchedule.AvailablilityEnd)
                {
                    if (getResourceSchedule.GenericDays.Count() != 0)
                    {
                        var lastItem = getResourceSchedule.GenericDays.Last();
                        foreach (var item in getResourceSchedule.GenericDays)
                        {
                            if (!(getServiceAssignment.PickoffTime.TimeOfDay >= item.OpeningTime.TimeOfDay && getServiceAssignment.PickoffTime.TimeOfDay <= item.ClosingTime.TimeOfDay))
                            {
                                if (item.Equals(lastItem))
                                {
                                    return CommonResponse.Send(ResponseCodes.FAILURE, null, ResponseMessage.NoGenericDay449);
                                   
                                }
                                else
                                {
                                    continue;
                                }
                               
                            }
                            if (getServiceAssignment.PickoffTime.TimeOfDay >= item.OpeningTime.TimeOfDay && getServiceAssignment.PickoffTime.TimeOfDay <= item.ClosingTime.TimeOfDay)
                            {
                                if (getServiceAssignment.PickupDate.DayOfWeek.ToString() == "Monday" && item.Monday == true)
                                {
                                    break;
                                }
                                else if (getServiceAssignment.PickupDate.DayOfWeek.ToString() == "Tuesday" && item.Tuesday == true)
                                {
                                    break;
                                }
                                else if (getServiceAssignment.PickupDate.DayOfWeek.ToString() == "Wednesday" && item.Wednesday == true)
                                {
                                    break;
                                }
                                else if (getServiceAssignment.PickupDate.DayOfWeek.ToString() == "Thursday" && item.Thursday == true)
                                {
                                    break;
                                }
                                else if (getServiceAssignment.PickupDate.DayOfWeek.ToString() == "Friday" && item.Friday == true)
                                {
                                    break;
                                }
                                else if (getServiceAssignment.PickupDate.DayOfWeek.ToString() == "Saturday" && item.Saturday == true)
                                {
                                    break;
                                }
                                else if (getServiceAssignment.PickupDate.DayOfWeek.ToString() == "Sunday" && item.Sunday == true)
                                {
                                    break;
                                }
                                else
                                {
                                    return CommonResponse.Send(ResponseCodes.FAILURE, null, ResponseMessage.NoGenericDay449);
                                }
                            }
                            else
                            {
                                //return new ApiResponse(448);//for  schedule time
                                return CommonResponse.Send(ResponseCodes.FAILURE, null, ResponseMessage.ScheduleTimeMismatch448);
                            }
                        }
                    }
                    else
                    {
                        return CommonResponse.Send(ResponseCodes.FAILURE, null, ResponseMessage.NoGenericDay449);
                    }
                    

                    if (RouteExists != null)
                    {
                        if (getResourceTypePerService != null)
                        {
                            var typeExists = _serviceRegistrationRepository.GetCommanderResourceApplicableTypeReqById(getServiceRegistration.Id, getResourceTypePerService.CommanderTypeId);
                            if (typeExists == null)
                            {
                                return CommonResponse.Send(ResponseCodes.FAILURE, null, ResponseMessage.NoApplicableType446);
                            }
                        }
                        else
                        {
                            return CommonResponse.Send(ResponseCodes.FAILURE, null, ResponseMessage.NoApplicableType446);
                        }
                        if (getCommanderDetail != null)
                        {
                            if (getCommanderDetail.IsTemporarilyHeld == true || getCommanderDetail.IsHeldForAction == true)
                            {
                                //return new ApiResponse(444);
                                return CommonResponse.Send(ResponseCodes.FAILURE, null, ResponseMessage.Held444);
                            }
                        }
                       
                        if (getCommanderDetailListById.Count() == 0)
                        {
                            master.IsTemporarilyHeld = true;
                            master.DateTemporarilyHeld = DateTime.UtcNow;
                            master.IsHeldForAction = true;
                            master.DateHeldForAction = DateTime.UtcNow;
                            master.RequiredCount = getCommanderDetailListById.Count()+1;
                            master.CreatedById = context.GetLoggedInUserId();
                            master.CreatedAt = DateTime.UtcNow;
                            var savedItem = await _serviceAssignmentDetailsRepository.SaveCommanderServiceAssignmentdetail(master);
                            if (savedItem == null)
                            {
                                return CommonResponse.Send(ResponseCodes.FAILURE, null, ResponseMessage.InternalServer500);
                            }
                        }
                        else
                        {
                            if (getCommanderDetailListById.Count() < getServiceRegistration.CommanderQuantityRequired)
                            {
                                master.IsTemporarilyHeld = true;
                                master.DateTemporarilyHeld = DateTime.UtcNow;
                                master.IsHeldForAction = true;
                                master.DateHeldForAction = DateTime.UtcNow;
                                //master.RequiredCount = (int)getCommanderDetail.ServiceAssignment.ServiceRegistration.CommanderQuantityRequired;
                                master.RequiredCount = getCommanderDetailListById.Count()+1;
                                master.CreatedById = context.GetLoggedInUserId();
                                master.CreatedAt = DateTime.UtcNow;
                                var savedItem = await _serviceAssignmentDetailsRepository.SaveCommanderServiceAssignmentdetail(master);
                                if (savedItem == null)
                                {
                                    //return  CommonResponse.Send(ResponseCodes.FAILURE, null, ResponseMessage.InternalServer500);
                                    return CommonResponse.Send(ResponseCodes.FAILURE, null, ResponseMessage.InternalServer500);

                                }
                            }
                            else
                            {
                                return CommonResponse.Send(ResponseCodes.FAILURE, null, ResponseMessage.MaxQuantity445);

                            }
                        }
                    }
                    else
                    {
                        return CommonResponse.Send(ResponseCodes.FAILURE, null, ResponseMessage.NoResourceOnRoute443);

                    }
                }
                else
                {
                    //return new ApiResponse(448); //for schedule date
                    return CommonResponse.Send(ResponseCodes.FAILURE, null, ResponseMessage.ScheduleTimeMismatch448);

                }
            }
            else
            {
                //return new ApiResponse(447);
                return CommonResponse.Send(ResponseCodes.FAILURE, null, ResponseMessage.NoSchedule447);

            }


            var _getCommandersDetailListById = await _serviceAssignmentDetailsRepository.FindAllCommanderServiceAssignmentDetailsByAssignmentId(commanderReceivingDTO.ServiceAssignmentId);
            if (_getCommandersDetailListById.Count() == getServiceRegistration.CommanderQuantityRequired)
            {
                var getPilotDetailListById = await _serviceAssignmentDetailsRepository.FindAllPilotServiceAssignmentDetailsByAssignmentId(getServiceAssignment.Id);
                var getVehicleDetailListById = await _serviceAssignmentDetailsRepository.FindAllVehicleServiceAssignmentDetailsByAssignmentId(getServiceAssignment.Id);
                var getEscortDetailListById = await _serviceAssignmentDetailsRepository.FindAllEscortServiceAssignmentDetailsByAssignmentId(getServiceAssignment.Id);



                //Check For all Required
                if ((getEscortDetailListById.Count() == getServiceRegistration.ArmedEscortQuantityRequired && getServiceRegistration.RequiresArmedEscort == true)
                    && (getVehicleDetailListById.Count() == getServiceRegistration.VehicleQuantityRequired && getServiceRegistration.RequiresVehicle == true)
                    && (getPilotDetailListById.Count() == getServiceRegistration.PilotQuantityRequired && getServiceRegistration.RequiresPilot == true))
                {
                    var itemToUpdate = await _serviceAssignmentMasterRepository.FindServiceAssignmentById(commanderReceivingDTO.ServiceAssignmentId);

                    if (itemToUpdate == null)
                    {
                        return CommonResponse.Send(ResponseCodes.NO_DATA_AVAILABLE);
                    }

                    if (!await _serviceAssignmentMasterRepository.UpdateReadyStatus(itemToUpdate))
                    {
                        return CommonResponse.Send(ResponseCodes.FAILURE, null, ResponseMessage.InternalServer500);
                    }
                }
                else  //Check For all but where pilot not required
                if ((getEscortDetailListById.Count() == getServiceRegistration.ArmedEscortQuantityRequired && getServiceRegistration.RequiresArmedEscort == true)
                    && (getVehicleDetailListById.Count() == getServiceRegistration.VehicleQuantityRequired && getServiceRegistration.RequiresVehicle == true)
                    && (getServiceRegistration.RequiresPilot == false))
                {
                    var itemToUpdate = await _serviceAssignmentMasterRepository.FindServiceAssignmentById(commanderReceivingDTO.ServiceAssignmentId);

                    if (itemToUpdate == null)
                    {
                        return CommonResponse.Send(ResponseCodes.NO_DATA_AVAILABLE);
                    }

                    if (!await _serviceAssignmentMasterRepository.UpdateReadyStatus(itemToUpdate))
                    {
                        return CommonResponse.Send(ResponseCodes.FAILURE, null, ResponseMessage.InternalServer500);
                    }
                }
                //check for all but vehicle not required
                else if ((getEscortDetailListById.Count() == getServiceRegistration.ArmedEscortQuantityRequired && getServiceRegistration.RequiresArmedEscort == true)
                 && (getServiceRegistration.RequiresVehicle == false)
                 && (getPilotDetailListById.Count() == getServiceRegistration.PilotQuantityRequired && getServiceRegistration.RequiresPilot == true))
                {
                    var itemToUpdate = await _serviceAssignmentMasterRepository.FindServiceAssignmentById(commanderReceivingDTO.ServiceAssignmentId);

                    if (itemToUpdate == null)
                    {
                        return CommonResponse.Send(ResponseCodes.NO_DATA_AVAILABLE);
                    }

                    if (!await _serviceAssignmentMasterRepository.UpdateReadyStatus(itemToUpdate))
                    {
                        return CommonResponse.Send(ResponseCodes.FAILURE, null, ResponseMessage.InternalServer500);
                    }
                }
                //Check For all but Escort not required
                else if ((getServiceRegistration.RequiresArmedEscort == false)
                    && (getVehicleDetailListById.Count() == getServiceRegistration.VehicleQuantityRequired && getServiceRegistration.RequiresVehicle == true)
                    && (getPilotDetailListById.Count() == getServiceRegistration.PilotQuantityRequired && getServiceRegistration.RequiresPilot == true))
                {
                    var itemToUpdate = await _serviceAssignmentMasterRepository.FindServiceAssignmentById(commanderReceivingDTO.ServiceAssignmentId);

                    if (itemToUpdate == null)
                    {
                        return CommonResponse.Send(ResponseCodes.NO_DATA_AVAILABLE);
                    }

                    if (!await _serviceAssignmentMasterRepository.UpdateReadyStatus(itemToUpdate))
                    {
                        return CommonResponse.Send(ResponseCodes.FAILURE, null, ResponseMessage.InternalServer500);
                    }
                }

                //Only Escort required and commander
                else if ((getEscortDetailListById.Count() == getServiceRegistration.ArmedEscortQuantityRequired && getServiceRegistration.RequiresArmedEscort == true)
                    && (getServiceRegistration.RequiresVehicle == false)
                    && (getServiceRegistration.RequiresPilot == false))
                {
                    var itemToUpdate = await _serviceAssignmentMasterRepository.FindServiceAssignmentById(commanderReceivingDTO.ServiceAssignmentId);

                    if (itemToUpdate == null)
                    {
                        return CommonResponse.Send(ResponseCodes.NO_DATA_AVAILABLE);
                    }

                    if (!await _serviceAssignmentMasterRepository.UpdateReadyStatus(itemToUpdate))
                    {
                        return CommonResponse.Send(ResponseCodes.FAILURE, null, ResponseMessage.InternalServer500);
                    }
                }
                //Only Vehicle required and commander
                else if ((getServiceRegistration.RequiresArmedEscort == false)
                    && (getVehicleDetailListById.Count() == getServiceRegistration.VehicleQuantityRequired && getServiceRegistration.RequiresVehicle == true)
                    && (getServiceRegistration.RequiresPilot == false))
                {
                    var itemToUpdate = await _serviceAssignmentMasterRepository.FindServiceAssignmentById(commanderReceivingDTO.ServiceAssignmentId);

                    if (itemToUpdate == null)
                    {
                        return CommonResponse.Send(ResponseCodes.NO_DATA_AVAILABLE);
                    }

                    if (!await _serviceAssignmentMasterRepository.UpdateReadyStatus(itemToUpdate))
                    {
                        return CommonResponse.Send(ResponseCodes.FAILURE, null, ResponseMessage.InternalServer500);
                    }
                }

                //Only Pilot required and escort
                else if ((getServiceRegistration.RequiresArmedEscort == false)
                    && (getServiceRegistration.RequiresVehicle == false)
                    && (getPilotDetailListById.Count() == getServiceRegistration.PilotQuantityRequired && getServiceRegistration.RequiresPilot == true))
                {
                    var itemToUpdate = await _serviceAssignmentMasterRepository.FindServiceAssignmentById(commanderReceivingDTO.ServiceAssignmentId);

                    if (itemToUpdate == null)
                    {
                        return CommonResponse.Send(ResponseCodes.NO_DATA_AVAILABLE);
                    }

                    if (!await _serviceAssignmentMasterRepository.UpdateReadyStatus(itemToUpdate))
                    {
                        return CommonResponse.Send(ResponseCodes.FAILURE, null, ResponseMessage.InternalServer500);
                    }
                }

                //Only commander required 
                else if ((getServiceRegistration.RequiresArmedEscort == false)
                    && (getServiceRegistration.RequiresVehicle == false)
                    && (getServiceRegistration.RequiresPilot == false))
                {
                    var itemToUpdate = await _serviceAssignmentMasterRepository.FindServiceAssignmentById(commanderReceivingDTO.ServiceAssignmentId);

                    if (itemToUpdate == null)
                    {
                        return CommonResponse.Send(ResponseCodes.NO_DATA_AVAILABLE);
                    }

                    if (!await _serviceAssignmentMasterRepository.UpdateReadyStatus(itemToUpdate))
                    {
                        return CommonResponse.Send(ResponseCodes.FAILURE, null, ResponseMessage.InternalServer500);
                    }
                }
            }
            var TransferDTO = _mapper.Map<CommanderServiceAssignmentDetailsTransferDTO>(master);
            return CommonResponse.Send(ResponseCodes.SUCCESS, TransferDTO);
            //return CommonResponse.Send(ResponseCodes.SUCCESS, null, ResponseMessage.Success200);
        }

        public async Task<ApiCommonResponse> AddPassenger(HttpContext context, PassengerReceivingDTO passengerReceivingDTO)
        {
            var passenger = _mapper.Map<Passenger>(passengerReceivingDTO);
          
            passenger.CreatedById = context.GetLoggedInUserId();
            passenger.CreatedAt = DateTime.UtcNow;
            var savedItem = await _serviceAssignmentDetailsRepository.SavePassenger(passenger);
            if (savedItem == null)
            {
                return CommonResponse.Send(ResponseCodes.FAILURE, null, ResponseMessage.InternalServer500);
            }
            var TransferDTO = _mapper.Map<PassengerTransferDTO>(passenger);
            return CommonResponse.Send(ResponseCodes.SUCCESS, TransferDTO, ResponseMessage.Success200);
        }

        public async Task<ApiCommonResponse> AddPilotDetail(HttpContext context, PilotServiceAssignmentDetailsReceivingDTO pilotReceivingDTO)
        {
            var master = _mapper.Map<PilotServiceAssignmentDetail>(pilotReceivingDTO);
            var getServiceAssignment = await _serviceAssignmentMasterRepository.FindServiceAssignmentById(pilotReceivingDTO.ServiceAssignmentId);
            var getServiceRegistration = await _serviceRegistrationRepository.FindServiceById(getServiceAssignment.ServiceRegistration.Id);
            var getPilotDetail = await _serviceAssignmentDetailsRepository.FindPilotServiceAssignmentDetailByResourceId(pilotReceivingDTO.PilotResourceId);
            var getPilotDetailListById = await _serviceAssignmentDetailsRepository.FindAllPilotServiceAssignmentDetailsByAssignmentId(pilotReceivingDTO.ServiceAssignmentId);
            var RouteExists = _pilotRegistrationRepository.GetResourceRegIdRegionAndRouteId(pilotReceivingDTO.PilotResourceId, getServiceAssignment.SMORouteId);
            var getResourceTypePerService = await _serviceRegistrationRepository.FindPilotResourceByServiceRegId(getServiceAssignment.ServiceRegistration.Id);
            //var typeExists = _serviceRegistrationRepository.GetPilotResourceApplicableTypeReqById(getServiceRegistration.Id, getResourceTypePerService.PilotTypeId);
            //Get Active Pilot resource schedule
            var getResourceSchedule = await _dTSMastersRepository.FindPilotMasterByResourceId(pilotReceivingDTO.PilotResourceId);
          

            if (getResourceSchedule != null)
            {
                if (getServiceAssignment.PickupDate >= getResourceSchedule.AvailabilityStart && getServiceAssignment.PickupDate <= getResourceSchedule.AvailablilityEnd)
                {
                    if (getResourceSchedule.GenericDays.Count() !=0)
                    {
                        var lastItem = getResourceSchedule.GenericDays.Last();
                        foreach (var item in getResourceSchedule.GenericDays)
                        {
                            if (!(getServiceAssignment.PickoffTime.TimeOfDay >= item.OpeningTime.TimeOfDay && getServiceAssignment.PickoffTime.TimeOfDay <= item.ClosingTime.TimeOfDay))
                            {
                                if (item.Equals(lastItem))
                                {
                                    return CommonResponse.Send(ResponseCodes.FAILURE, null, ResponseMessage.NoGenericDay449);

                                }
                                else
                                {
                                    continue;
                                }

                            }
                            if (getServiceAssignment.PickoffTime.TimeOfDay >= item.OpeningTime.TimeOfDay && getServiceAssignment.PickoffTime.TimeOfDay <= item.ClosingTime.TimeOfDay)
                            {
                                if (getServiceAssignment.PickupDate.DayOfWeek.ToString() == "Monday" && item.Monday == true)
                                {
                                    break;
                                }
                                else if (getServiceAssignment.PickupDate.DayOfWeek.ToString() == "Tuesday" && item.Tuesday == true)
                                {
                                    break;
                                }
                                else if (getServiceAssignment.PickupDate.DayOfWeek.ToString() == "Wednesday" && item.Wednesday == true)
                                {
                                    break;
                                }
                                else if (getServiceAssignment.PickupDate.DayOfWeek.ToString() == "Thursday" && item.Thursday == true)
                                {
                                    break;
                                }
                                else if (getServiceAssignment.PickupDate.DayOfWeek.ToString() == "Friday" && item.Friday == true)
                                {
                                    break;
                                }
                                else if (getServiceAssignment.PickupDate.DayOfWeek.ToString() == "Saturday" && item.Saturday == true)
                                {
                                    break;
                                }
                                else if (getServiceAssignment.PickupDate.DayOfWeek.ToString() == "Sunday" && item.Sunday == true)
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
                    

                    if (RouteExists != null)
                    {
                        if (getResourceTypePerService != null)
                        {
                            var typeExists = _serviceRegistrationRepository.GetPilotResourceApplicableTypeReqById(getServiceRegistration.Id, getResourceTypePerService.PilotTypeId);
                            if (typeExists == null)
                            {
                                return CommonResponse.Send(ResponseCodes.FAILURE, null, ResponseMessage.NoApplicableType446);
                            }
                        }
                        else
                        {
                            return CommonResponse.Send(ResponseCodes.FAILURE, null, ResponseMessage.NoApplicableType446);
                        }
                        if (getPilotDetail != null)
                        {
                            if (getPilotDetail.IsTemporarilyHeld == true || getPilotDetail.IsHeldForAction == true)
                            {
                                return CommonResponse.Send(ResponseCodes.FAILURE, null, ResponseMessage.Held444);
                            }
                        }
                       
                        if (getPilotDetailListById.Count() == 0)
                        {
                            master.IsTemporarilyHeld = true;
                            master.DateTemporarilyHeld = DateTime.UtcNow;
                            master.IsHeldForAction = true;
                            master.DateHeldForAction = DateTime.UtcNow;
                            master.RequiredCount = getPilotDetailListById.Count() + 1;
                            //master.RequiredCount = (int)getPilotDetail.ServiceAssignment.ServiceRegistration.PilotQuantityRequired;
                            master.CreatedById = context.GetLoggedInUserId();
                            master.CreatedAt = DateTime.UtcNow;
                            var savedItem = await _serviceAssignmentDetailsRepository.SavePilotServiceAssignmentdetail(master);
                            if (savedItem == null)
                            {
                                return CommonResponse.Send(ResponseCodes.FAILURE, null, ResponseMessage.InternalServer500);
                            }
                        }
                        else
                        {
                            if (getPilotDetailListById.Count() < getServiceRegistration.PilotQuantityRequired)
                            {
                                master.IsTemporarilyHeld = true;
                                master.DateTemporarilyHeld = DateTime.UtcNow;
                                master.IsHeldForAction = true;
                                master.DateHeldForAction = DateTime.UtcNow;
                                master.RequiredCount = getPilotDetailListById.Count()+1;
                                master.CreatedById = context.GetLoggedInUserId();
                                master.CreatedAt = DateTime.UtcNow;
                                var savedItem = await _serviceAssignmentDetailsRepository.SavePilotServiceAssignmentdetail(master);
                                if (savedItem == null)
                                {
                                    return CommonResponse.Send(ResponseCodes.FAILURE, null, ResponseMessage.InternalServer500);
                                }
                            }
                            else
                            {
                                return CommonResponse.Send(ResponseCodes.FAILURE, null, ResponseMessage.MaxQuantity445);
                            }
                        }
                    }
                    else
                    {
                        return CommonResponse.Send(ResponseCodes.FAILURE, null, ResponseMessage.NoResourceOnRoute443);
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

            var _getPilotsDetailListById = await _serviceAssignmentDetailsRepository.FindAllPilotServiceAssignmentDetailsByAssignmentId(pilotReceivingDTO.ServiceAssignmentId);
            if (_getPilotsDetailListById.Count() == getServiceRegistration.PilotQuantityRequired)
            {
                var getCommanderDetailListById = await _serviceAssignmentDetailsRepository.FindAllCommanderServiceAssignmentDetailsByAssignmentId(getServiceAssignment.Id);
                var getVehicleDetailListById = await _serviceAssignmentDetailsRepository.FindAllVehicleServiceAssignmentDetailsByAssignmentId(getServiceAssignment.Id);
                var getEscortDetailListById = await _serviceAssignmentDetailsRepository.FindAllEscortServiceAssignmentDetailsByAssignmentId(getServiceAssignment.Id);



                //Check For all Required
                if ((getEscortDetailListById.Count() == getServiceRegistration.ArmedEscortQuantityRequired && getServiceRegistration.RequiresArmedEscort == true)
                    && (getVehicleDetailListById.Count() == getServiceRegistration.VehicleQuantityRequired && getServiceRegistration.RequiresVehicle == true)
                    && (getCommanderDetailListById.Count() == getServiceRegistration.CommanderQuantityRequired && getServiceRegistration.RequiresCommander == true))
                {
                    var itemToUpdate = await _serviceAssignmentMasterRepository.FindServiceAssignmentById(pilotReceivingDTO.ServiceAssignmentId);

                    if (itemToUpdate == null)
                    {
                        return CommonResponse.Send(ResponseCodes.NO_DATA_AVAILABLE);
                    }

                    if (!await _serviceAssignmentMasterRepository.UpdateReadyStatus(itemToUpdate))
                    {
                        return CommonResponse.Send(ResponseCodes.FAILURE, null, ResponseMessage.InternalServer500);
                    }
                }
                else  //Check For all but where commander not required
                if ((getEscortDetailListById.Count() == getServiceRegistration.ArmedEscortQuantityRequired && getServiceRegistration.RequiresArmedEscort == true)
                    && (getVehicleDetailListById.Count() == getServiceRegistration.VehicleQuantityRequired && getServiceRegistration.RequiresVehicle == true)
                    && (getServiceRegistration.RequiresCommander == false))
                {
                    var itemToUpdate = await _serviceAssignmentMasterRepository.FindServiceAssignmentById(pilotReceivingDTO.ServiceAssignmentId);

                    if (itemToUpdate == null)
                    {
                        return CommonResponse.Send(ResponseCodes.NO_DATA_AVAILABLE);
                    }

                    if (!await _serviceAssignmentMasterRepository.UpdateReadyStatus(itemToUpdate))
                    {
                        return CommonResponse.Send(ResponseCodes.FAILURE, null, ResponseMessage.InternalServer500);
                    }
                }
                //check for all but vehicle not required
                else if ((getEscortDetailListById.Count() == getServiceRegistration.ArmedEscortQuantityRequired && getServiceRegistration.RequiresArmedEscort == true)
                 && (getServiceRegistration.RequiresVehicle == false)
                 && (getCommanderDetailListById.Count() == getServiceRegistration.CommanderQuantityRequired && getServiceRegistration.RequiresCommander == true))
                {
                    var itemToUpdate = await _serviceAssignmentMasterRepository.FindServiceAssignmentById(pilotReceivingDTO.ServiceAssignmentId);

                    if (itemToUpdate == null)
                    {
                        return CommonResponse.Send(ResponseCodes.NO_DATA_AVAILABLE);
                    }

                    if (!await _serviceAssignmentMasterRepository.UpdateReadyStatus(itemToUpdate))
                    {
                        return CommonResponse.Send(ResponseCodes.FAILURE, null, ResponseMessage.InternalServer500);
                    }
                }
                //Check For all but Escort not required
                else if ((getServiceRegistration.RequiresArmedEscort == false)
                    && (getVehicleDetailListById.Count() == getServiceRegistration.VehicleQuantityRequired && getServiceRegistration.RequiresVehicle == true)
                    && (getCommanderDetailListById.Count() == getServiceRegistration.CommanderQuantityRequired && getServiceRegistration.RequiresCommander == true))
                {
                    var itemToUpdate = await _serviceAssignmentMasterRepository.FindServiceAssignmentById(pilotReceivingDTO.ServiceAssignmentId);

                    if (itemToUpdate == null)
                    {
                        return CommonResponse.Send(ResponseCodes.NO_DATA_AVAILABLE);
                    }

                    if (!await _serviceAssignmentMasterRepository.UpdateReadyStatus(itemToUpdate))
                    {
                        return CommonResponse.Send(ResponseCodes.FAILURE, null, ResponseMessage.InternalServer500);
                    }
                }

                //Only Pilot required and escort
                else if ((getEscortDetailListById.Count() == getServiceRegistration.ArmedEscortQuantityRequired && getServiceRegistration.RequiresArmedEscort == true)
                    && (getServiceRegistration.RequiresVehicle == false)
                    && (getServiceRegistration.RequiresCommander == false))
                {
                    var itemToUpdate = await _serviceAssignmentMasterRepository.FindServiceAssignmentById(pilotReceivingDTO.ServiceAssignmentId);

                    if (itemToUpdate == null)
                    {
                        return CommonResponse.Send(ResponseCodes.NO_DATA_AVAILABLE);
                    }

                    if (!await _serviceAssignmentMasterRepository.UpdateReadyStatus(itemToUpdate))
                    {
                        return CommonResponse.Send(ResponseCodes.FAILURE, null, ResponseMessage.InternalServer500);
                    }
                }
                //Only Vehicle required and pilot
                else if ((getServiceRegistration.RequiresArmedEscort == false)
                    && (getVehicleDetailListById.Count() == getServiceRegistration.VehicleQuantityRequired && getServiceRegistration.RequiresVehicle == true)
                    && (getServiceRegistration.RequiresCommander == false))
                {
                    var itemToUpdate = await _serviceAssignmentMasterRepository.FindServiceAssignmentById(pilotReceivingDTO.ServiceAssignmentId);

                    if (itemToUpdate == null)
                    {
                        return CommonResponse.Send(ResponseCodes.NO_DATA_AVAILABLE);
                    }

                    if (!await _serviceAssignmentMasterRepository.UpdateReadyStatus(itemToUpdate))
                    {
                        return CommonResponse.Send(ResponseCodes.FAILURE, null, ResponseMessage.InternalServer500);
                    }
                }

                //Only Pilot required and commander
                else if ((getServiceRegistration.RequiresArmedEscort == false)
                    && (getServiceRegistration.RequiresVehicle == false)
                    && (getCommanderDetailListById.Count() == getServiceRegistration.CommanderQuantityRequired && getServiceRegistration.RequiresCommander == true))
                {
                    var itemToUpdate = await _serviceAssignmentMasterRepository.FindServiceAssignmentById(pilotReceivingDTO.ServiceAssignmentId);

                    if (itemToUpdate == null)
                    {
                        return CommonResponse.Send(ResponseCodes.NO_DATA_AVAILABLE);
                    }

                    if (!await _serviceAssignmentMasterRepository.UpdateReadyStatus(itemToUpdate))
                    {
                        return CommonResponse.Send(ResponseCodes.FAILURE, null, ResponseMessage.InternalServer500);
                    }
                }

                //Only pilot required 
                else if ((getServiceRegistration.RequiresArmedEscort == false)
                    && (getServiceRegistration.RequiresVehicle == false)
                    && (getServiceRegistration.RequiresCommander == false))
                {
                    var itemToUpdate = await _serviceAssignmentMasterRepository.FindServiceAssignmentById(pilotReceivingDTO.ServiceAssignmentId);

                    if (itemToUpdate == null)
                    {
                        return CommonResponse.Send(ResponseCodes.NO_DATA_AVAILABLE);
                    }

                    if (!await _serviceAssignmentMasterRepository.UpdateReadyStatus(itemToUpdate))
                    {
                        return CommonResponse.Send(ResponseCodes.FAILURE, null, ResponseMessage.InternalServer500);
                    }
                }
            }
            var TransferDTO = _mapper.Map<PilotServiceAssignmentDetailsTransferDTO>(master);
            return CommonResponse.Send(ResponseCodes.SUCCESS, TransferDTO);
            //return CommonResponse.Send(ResponseCodes.SUCCESS, null, ResponseMessage.Success200);
        }

        public async Task<ApiCommonResponse> AddVehicleDetail(HttpContext context, VehicleServiceAssignmentDetailsReceivingDTO vehicleReceivingDTO)
        {
            var master = _mapper.Map<VehicleServiceAssignmentDetail>(vehicleReceivingDTO);
            var getServiceAssignment = await _serviceAssignmentMasterRepository.FindServiceAssignmentById(vehicleReceivingDTO.ServiceAssignmentId);
            var getServiceRegistration = await _serviceRegistrationRepository.FindServiceById(getServiceAssignment.ServiceRegistration.Id);
            var getVehicleDetail = await _serviceAssignmentDetailsRepository.FindVehicleServiceAssignmentDetailByResourceId(vehicleReceivingDTO.VehicleResourceId);
            var getVehicleDetailListById = await _serviceAssignmentDetailsRepository.FindAllVehicleServiceAssignmentDetailsByAssignmentId(vehicleReceivingDTO.ServiceAssignmentId);
            var RouteExists = _vehicleRegistrationRepository.GetResourceRegIdRegionAndRouteId(vehicleReceivingDTO.VehicleResourceId, getServiceAssignment.SMORouteId);
            var getResourceTypePerService = await _serviceRegistrationRepository.FindVehicleResourceByServiceRegId(getServiceAssignment.ServiceRegistration.Id);
             
            var getResourceSchedule = await _dTSMastersRepository.FindVehicleMasterByResourceId(vehicleReceivingDTO.VehicleResourceId);
           

            if (getResourceSchedule != null)
            {
                if (getServiceAssignment.PickupDate >= getResourceSchedule.AvailabilityStart && getServiceAssignment.PickupDate <= getResourceSchedule.AvailablilityEnd)
                {
                    if (getResourceSchedule.GenericDays.Count() != 0)
                    {
                        var lastItem = getResourceSchedule.GenericDays.Last();
                        foreach (var item in getResourceSchedule.GenericDays)
                        {
                            
                            if (!(getServiceAssignment.PickoffTime.TimeOfDay >= item.OpeningTime.TimeOfDay && getServiceAssignment.PickoffTime.TimeOfDay <= item.ClosingTime.TimeOfDay))
                            {
                                if (item.Equals(lastItem))
                                {
                                    return CommonResponse.Send(ResponseCodes.FAILURE, null, ResponseMessage.NoGenericDay449);

                                }
                                else
                                {
                                    continue;
                                }

                            }
                            if (getServiceAssignment.PickoffTime.TimeOfDay >= item.OpeningTime.TimeOfDay && getServiceAssignment.PickoffTime.TimeOfDay <= item.ClosingTime.TimeOfDay)
                            {
                                if (getServiceAssignment.PickupDate.DayOfWeek.ToString() == "Monday" && item.Monday == true)
                                {
                                    break;
                                }
                                else if (getServiceAssignment.PickupDate.DayOfWeek.ToString() == "Tuesday" && item.Tuesday == true)
                                {
                                    break;
                                }
                                else if (getServiceAssignment.PickupDate.DayOfWeek.ToString() == "Wednesday" && item.Wednesday == true)
                                {
                                    break;
                                }
                                else if (getServiceAssignment.PickupDate.DayOfWeek.ToString() == "Thursday" && item.Thursday == true)
                                {
                                    break;
                                }
                                else if (getServiceAssignment.PickupDate.DayOfWeek.ToString() == "Friday" && item.Friday == true)
                                {
                                    break;
                                }
                                else if (getServiceAssignment.PickupDate.DayOfWeek.ToString() == "Saturday" && item.Saturday == true)
                                {
                                    break;
                                }
                                else if (getServiceAssignment.PickupDate.DayOfWeek.ToString() == "Sunday" && item.Sunday == true)
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
                   

                    if (RouteExists != null)
                    {
                        if (getResourceTypePerService != null)
                        {
                            var typeExists = _serviceRegistrationRepository.GetVehicleResourceApplicableTypeReqById(getServiceRegistration.Id, getResourceTypePerService.VehicleTypeId);
                            if(typeExists == null)
                            {
                                return CommonResponse.Send(ResponseCodes.FAILURE, null, ResponseMessage.NoApplicableType446);
                            }

                        }
                        else
                        {
                            return CommonResponse.Send(ResponseCodes.FAILURE, null, ResponseMessage.NoApplicableType446);
                        }
                        if (getVehicleDetail != null)
                        {
                            if (getVehicleDetail.IsTemporarilyHeld == true || getVehicleDetail.IsHeldForAction == true)
                            {
                                return CommonResponse.Send(ResponseCodes.FAILURE, null, ResponseMessage.Held444);
                            }
                        }
                       
                        if (getVehicleDetailListById.Count() == 0)
                        {
                            master.IsTemporarilyHeld = true;
                            master.DateTemporarilyHeld = DateTime.UtcNow;
                            master.IsHeldForAction = true;
                            master.DateHeldForAction = DateTime.UtcNow;
                            master.RequiredCount = getVehicleDetailListById.Count()+1;
                            master.CreatedById = context.GetLoggedInUserId();
                            master.CreatedAt = DateTime.UtcNow;
                            var savedItem = await _serviceAssignmentDetailsRepository.SaveVehicleServiceAssignmentdetail(master);
                            if (savedItem == null)
                            {
                                return CommonResponse.Send(ResponseCodes.FAILURE, null, ResponseMessage.InternalServer500);
                            }
                        }
                        else
                        {
                            if (getVehicleDetailListById.Count() < getServiceRegistration.VehicleQuantityRequired)
                            {
                                master.IsTemporarilyHeld = true;
                                master.DateTemporarilyHeld = DateTime.UtcNow;
                                master.IsHeldForAction = true;
                                master.DateHeldForAction = DateTime.UtcNow;
                                master.RequiredCount = getVehicleDetailListById.Count()+1;
                                master.CreatedById = context.GetLoggedInUserId();
                                master.CreatedAt = DateTime.UtcNow;
                                var savedItem = await _serviceAssignmentDetailsRepository.SaveVehicleServiceAssignmentdetail(master);
                                if (savedItem == null)
                                {
                                    return CommonResponse.Send(ResponseCodes.FAILURE, null, ResponseMessage.InternalServer500);
                                }
                            }
                            else
                            {
                                return CommonResponse.Send(ResponseCodes.FAILURE, null, ResponseMessage.MaxQuantity445);
                            }
                        }
                    }
                    else
                    {
                        return CommonResponse.Send(ResponseCodes.FAILURE, null, ResponseMessage.NoResourceOnRoute443);
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

            var _getVehiclesDetailListById = await _serviceAssignmentDetailsRepository.FindAllVehicleServiceAssignmentDetailsByAssignmentId(vehicleReceivingDTO.ServiceAssignmentId);
            if (_getVehiclesDetailListById.Count() == getServiceRegistration.VehicleQuantityRequired)
            {
                var getCommanderDetailListById = await _serviceAssignmentDetailsRepository.FindAllCommanderServiceAssignmentDetailsByAssignmentId(getServiceAssignment.Id);
                var getPilotDetailListById = await _serviceAssignmentDetailsRepository.FindAllPilotServiceAssignmentDetailsByAssignmentId(getServiceAssignment.Id);
                var getEscortDetailListById = await _serviceAssignmentDetailsRepository.FindAllEscortServiceAssignmentDetailsByAssignmentId(getServiceAssignment.Id);



                //Check For all Required
                if ((getEscortDetailListById.Count() == getServiceRegistration.ArmedEscortQuantityRequired && getServiceRegistration.RequiresArmedEscort == true)
                    && (getPilotDetailListById.Count() == getServiceRegistration.PilotQuantityRequired && getServiceRegistration.RequiresPilot == true)
                    && (getCommanderDetailListById.Count() == getServiceRegistration.CommanderQuantityRequired && getServiceRegistration.RequiresCommander == true))
                {
                    var itemToUpdate = await _serviceAssignmentMasterRepository.FindServiceAssignmentById(vehicleReceivingDTO.ServiceAssignmentId);

                    if (itemToUpdate == null)
                    {
                        return CommonResponse.Send(ResponseCodes.NO_DATA_AVAILABLE);
                    }

                    if (!await _serviceAssignmentMasterRepository.UpdateReadyStatus(itemToUpdate))
                    {
                        return CommonResponse.Send(ResponseCodes.FAILURE, null, ResponseMessage.InternalServer500);
                    }
                }
                else  //Check For all but where commander not required
                if ((getEscortDetailListById.Count() == getServiceRegistration.ArmedEscortQuantityRequired && getServiceRegistration.RequiresArmedEscort == true)
                    && (getPilotDetailListById.Count() == getServiceRegistration.PilotQuantityRequired && getServiceRegistration.RequiresPilot == true)
                    && (getServiceRegistration.RequiresCommander == false))
                {
                    var itemToUpdate = await _serviceAssignmentMasterRepository.FindServiceAssignmentById(vehicleReceivingDTO.ServiceAssignmentId);

                    if (itemToUpdate == null)
                    {
                        return CommonResponse.Send(ResponseCodes.NO_DATA_AVAILABLE);
                    }

                    if (!await _serviceAssignmentMasterRepository.UpdateReadyStatus(itemToUpdate))
                    {
                        return CommonResponse.Send(ResponseCodes.FAILURE, null, ResponseMessage.InternalServer500);
                    }
                }
                //check for all but pilot not required
                else if ((getEscortDetailListById.Count() == getServiceRegistration.ArmedEscortQuantityRequired && getServiceRegistration.RequiresArmedEscort == true)
                 && (getServiceRegistration.RequiresPilot == false)
                 && (getCommanderDetailListById.Count() == getServiceRegistration.CommanderQuantityRequired && getServiceRegistration.RequiresCommander == true))
                {
                    var itemToUpdate = await _serviceAssignmentMasterRepository.FindServiceAssignmentById(vehicleReceivingDTO.ServiceAssignmentId);

                    if (itemToUpdate == null)
                    {
                        return CommonResponse.Send(ResponseCodes.NO_DATA_AVAILABLE);
                    }

                    if (!await _serviceAssignmentMasterRepository.UpdateReadyStatus(itemToUpdate))
                    {
                        return CommonResponse.Send(ResponseCodes.FAILURE, null, ResponseMessage.InternalServer500);
                    }
                }
                //Check For all but Escort not required
                else if ((getServiceRegistration.RequiresArmedEscort == false)
                    && (getPilotDetailListById.Count() == getServiceRegistration.PilotQuantityRequired && getServiceRegistration.RequiresPilot == true)
                    && (getCommanderDetailListById.Count() == getServiceRegistration.CommanderQuantityRequired && getServiceRegistration.RequiresCommander == true))
                {
                    var itemToUpdate = await _serviceAssignmentMasterRepository.FindServiceAssignmentById(vehicleReceivingDTO.ServiceAssignmentId);

                    if (itemToUpdate == null)
                    {
                        return CommonResponse.Send(ResponseCodes.NO_DATA_AVAILABLE);
                    }

                    if (!await _serviceAssignmentMasterRepository.UpdateReadyStatus(itemToUpdate))
                    {
                        return CommonResponse.Send(ResponseCodes.FAILURE, null, ResponseMessage.InternalServer500);
                    }
                }

                //Only vehicle required and escort
                else if ((getEscortDetailListById.Count() == getServiceRegistration.ArmedEscortQuantityRequired && getServiceRegistration.RequiresArmedEscort == true)
                    && (getServiceRegistration.RequiresPilot == false)
                    && (getServiceRegistration.RequiresCommander == false))
                {
                    var itemToUpdate = await _serviceAssignmentMasterRepository.FindServiceAssignmentById(vehicleReceivingDTO.ServiceAssignmentId);

                    if (itemToUpdate == null)
                    {
                        return CommonResponse.Send(ResponseCodes.NO_DATA_AVAILABLE);
                    }

                    if (!await _serviceAssignmentMasterRepository.UpdateReadyStatus(itemToUpdate))
                    {
                        return CommonResponse.Send(ResponseCodes.FAILURE, null, ResponseMessage.InternalServer500);
                    }
                }
                //Only Vehicle required and pilot
                else if ((getServiceRegistration.RequiresArmedEscort == false)
                    && (getPilotDetailListById.Count() == getServiceRegistration.PilotQuantityRequired && getServiceRegistration.RequiresPilot == true)
                    && (getServiceRegistration.RequiresCommander == false))
                {
                    var itemToUpdate = await _serviceAssignmentMasterRepository.FindServiceAssignmentById(vehicleReceivingDTO.ServiceAssignmentId);

                    if (itemToUpdate == null)
                    {
                        return CommonResponse.Send(ResponseCodes.NO_DATA_AVAILABLE);
                    }

                    if (!await _serviceAssignmentMasterRepository.UpdateReadyStatus(itemToUpdate))
                    {
                        return CommonResponse.Send(ResponseCodes.FAILURE, null, ResponseMessage.InternalServer500);
                    }
                }

                //Only vehicle required and commander
                else if ((getServiceRegistration.RequiresArmedEscort == false)
                    && (getServiceRegistration.RequiresPilot == false)
                    && (getCommanderDetailListById.Count() == getServiceRegistration.CommanderQuantityRequired && getServiceRegistration.RequiresCommander == true))
                {
                    var itemToUpdate = await _serviceAssignmentMasterRepository.FindServiceAssignmentById(vehicleReceivingDTO.ServiceAssignmentId);

                    if (itemToUpdate == null)
                    {
                        return CommonResponse.Send(ResponseCodes.NO_DATA_AVAILABLE);
                    }

                    if (!await _serviceAssignmentMasterRepository.UpdateReadyStatus(itemToUpdate))
                    {
                        return CommonResponse.Send(ResponseCodes.FAILURE, null, ResponseMessage.InternalServer500);
                    }
                }

                //Only vehicle required 
                else if ((getServiceRegistration.RequiresArmedEscort == false)
                    && (getServiceRegistration.RequiresPilot == false)
                    && (getServiceRegistration.RequiresCommander == false))
                {
                    var itemToUpdate = await _serviceAssignmentMasterRepository.FindServiceAssignmentById(vehicleReceivingDTO.ServiceAssignmentId);

                    if (itemToUpdate == null)
                    {
                        return CommonResponse.Send(ResponseCodes.NO_DATA_AVAILABLE);
                    }

                    if (!await _serviceAssignmentMasterRepository.UpdateReadyStatus(itemToUpdate))
                    {
                        return CommonResponse.Send(ResponseCodes.FAILURE, null, ResponseMessage.InternalServer500);
                    }
                }
            }

            var TransferDTO = _mapper.Map<VehicleServiceAssignmentDetailsTransferDTO>(master);
            return CommonResponse.Send(ResponseCodes.SUCCESS, TransferDTO);
        }

        public async Task<ApiCommonResponse> DeleteArmedEscortDetail(long id)
        {
            var itemToDelete = await _serviceAssignmentDetailsRepository.FindEscortServiceAssignmentDetailById(id);

            if (itemToDelete == null)
            {
                return CommonResponse.Send(ResponseCodes.NO_DATA_AVAILABLE);
            }

            if (!await _serviceAssignmentDetailsRepository.DeleteEscortServiceAssignmentDetail(itemToDelete))
            {
                return CommonResponse.Send(ResponseCodes.FAILURE, null, ResponseMessage.InternalServer500);
            }

            //return new ApiOkResponse(true);
            return CommonResponse.Send(ResponseCodes.SUCCESS, null, ResponseMessage.Success200);
        }

        public async Task<ApiCommonResponse> DeleteCommanderDetail(long id)
        {
            var itemToDelete = await _serviceAssignmentDetailsRepository.FindCommanderServiceAssignmentDetailById(id);

            if (itemToDelete == null)
            {
                return CommonResponse.Send(ResponseCodes.NO_DATA_AVAILABLE);
            }

            if (!await _serviceAssignmentDetailsRepository.DeleteCommanderServiceAssignmentDetail(itemToDelete))
            {
                return CommonResponse.Send(ResponseCodes.FAILURE, null, ResponseMessage.InternalServer500);
            }

            return CommonResponse.Send(ResponseCodes.SUCCESS, null, ResponseMessage.Success200);
        }

        public async Task<ApiCommonResponse> DeletePassenger(long id)
        {
            var itemToDelete = await _serviceAssignmentDetailsRepository.FindPassengerById(id);

            if (itemToDelete == null)
            {
                return CommonResponse.Send(ResponseCodes.NO_DATA_AVAILABLE);
            }

            if (!await _serviceAssignmentDetailsRepository.DeletePassenger(itemToDelete))
            {
                return CommonResponse.Send(ResponseCodes.FAILURE, null, ResponseMessage.InternalServer500);
            }

            return CommonResponse.Send(ResponseCodes.SUCCESS, null, ResponseMessage.Success200);
        }

        public async Task<ApiCommonResponse> DeletePilotDetail(long id)
        {
            var itemToDelete = await _serviceAssignmentDetailsRepository.FindPilotServiceAssignmentDetailById(id);

            if (itemToDelete == null)
            {
                return CommonResponse.Send(ResponseCodes.NO_DATA_AVAILABLE);
            }

            if (!await _serviceAssignmentDetailsRepository.DeletePilotServiceAssignmentDetail(itemToDelete))
            {
                return CommonResponse.Send(ResponseCodes.FAILURE, null, ResponseMessage.InternalServer500);
            }

            return CommonResponse.Send(ResponseCodes.SUCCESS, null, ResponseMessage.Success200);
        }

        public async Task<ApiCommonResponse> DeleteVehicleDetail(long id)
        {
            var itemToDelete = await _serviceAssignmentDetailsRepository.FindVehicleServiceAssignmentDetailById(id);

            if (itemToDelete == null)
            {
                return CommonResponse.Send(ResponseCodes.NO_DATA_AVAILABLE);
            }

            if (!await _serviceAssignmentDetailsRepository.DeleteVehicleServiceAssignmentDetail(itemToDelete))
            {
                return CommonResponse.Send(ResponseCodes.FAILURE, null, ResponseMessage.InternalServer500);
            }

            return CommonResponse.Send(ResponseCodes.SUCCESS, null, ResponseMessage.Success200);
        }

        public async Task<ApiCommonResponse> GetAllArmedEscortDetails()
        {
            var master = await _serviceAssignmentDetailsRepository.FindAllEscortServiceAssignmentDetails();
            if (master == null)
            {
                return CommonResponse.Send(ResponseCodes.NO_DATA_AVAILABLE);
            }
            var TransferDTO = _mapper.Map<IEnumerable<ArmedEscortServiceAssignmentDetailsTransferDTO>>(master);
            return CommonResponse.Send(ResponseCodes.SUCCESS, TransferDTO);
        }

        public async Task<ApiCommonResponse> GetAllArmedEscortDetailsByAssignmentId(long assignmentId)
        {
            var master = await _serviceAssignmentDetailsRepository.FindAllEscortServiceAssignmentDetailsByAssignmentId(assignmentId);
            if (master == null)
            {
                return CommonResponse.Send(ResponseCodes.NO_DATA_AVAILABLE);
            }
            var TransferDTO = _mapper.Map<IEnumerable<ArmedEscortServiceAssignmentDetailsTransferDTO>>(master);
            return CommonResponse.Send(ResponseCodes.SUCCESS, TransferDTO);
        }

        public async Task<ApiCommonResponse> GetAllCommanderDetails()
        {
            var master = await _serviceAssignmentDetailsRepository.FindAllCommanderServiceAssignmentDetails();
            if (master == null)
            {
                return CommonResponse.Send(ResponseCodes.NO_DATA_AVAILABLE);
            }
            var TransferDTO = _mapper.Map<IEnumerable<CommanderServiceAssignmentDetailsTransferDTO>>(master);
            return CommonResponse.Send(ResponseCodes.SUCCESS, TransferDTO);
        }

        public async Task<ApiCommonResponse> GetAllCommanderDetailsByAssignmentId(long assignmentId)
        {
            var master = await _serviceAssignmentDetailsRepository.FindAllCommanderServiceAssignmentDetailsByAssignmentId(assignmentId);
            if (master == null)
            {
                return CommonResponse.Send(ResponseCodes.NO_DATA_AVAILABLE);
            }
            var TransferDTO = _mapper.Map<IEnumerable<CommanderServiceAssignmentDetailsTransferDTO>>(master);
            return CommonResponse.Send(ResponseCodes.SUCCESS, TransferDTO);
        }

        public async Task<ApiCommonResponse> GetAllCommanderDetailsByProfileId(long profileId)
        {
            var master = await _serviceAssignmentDetailsRepository.FindAllCommanderServiceAssignmentDetailsByProfileId(profileId);
            if (master == null)
            {
                return CommonResponse.Send(ResponseCodes.NO_DATA_AVAILABLE);
            }
            var TransferDTO = _mapper.Map<IEnumerable<CommanderServiceAssignmentDetailsTransferDTO>>(master);
            return CommonResponse.Send(ResponseCodes.SUCCESS, TransferDTO);
        }

        public async Task<ApiCommonResponse> GetAllPassengers()
        {
            var master = await _serviceAssignmentDetailsRepository.FindAllPassengers();
            if (master == null)
            {
                return CommonResponse.Send(ResponseCodes.NO_DATA_AVAILABLE);
            }
            var TransferDTO = _mapper.Map<IEnumerable<PassengerTransferDTO>>(master);
            return CommonResponse.Send(ResponseCodes.SUCCESS, TransferDTO);
        }

        public async Task<ApiCommonResponse> GetAllPassengersByAssignmentId(long assignmentId)
        {
            var master = await _serviceAssignmentDetailsRepository.FindAllPassengersByAssignmentId(assignmentId);
            if (master == null)
            {
                return CommonResponse.Send(ResponseCodes.NO_DATA_AVAILABLE);
            }
            var TransferDTO = _mapper.Map<IEnumerable<PassengerTransferDTO>>(master);
            return CommonResponse.Send(ResponseCodes.SUCCESS, TransferDTO);
        }

        public async Task<ApiCommonResponse> GetAllPilotDetails()
        {
            var master = await _serviceAssignmentDetailsRepository.FindAllPilotServiceAssignmentDetails();
            if (master == null)
            {
                return CommonResponse.Send(ResponseCodes.NO_DATA_AVAILABLE);
            }
            var TransferDTO = _mapper.Map<IEnumerable<PilotServiceAssignmentDetailsTransferDTO>>(master);
            return CommonResponse.Send(ResponseCodes.SUCCESS, TransferDTO);
        }

        public async Task<ApiCommonResponse> GetAllPilotDetailsByAssignmentId(long assignmentId)
        {
            var master = await _serviceAssignmentDetailsRepository.FindAllPilotServiceAssignmentDetailsByAssignmentId(assignmentId);
            if (master == null)
            {
                return CommonResponse.Send(ResponseCodes.NO_DATA_AVAILABLE);
            }
            var TransferDTO = _mapper.Map<IEnumerable<PilotServiceAssignmentDetailsTransferDTO>>(master);
            return CommonResponse.Send(ResponseCodes.SUCCESS, TransferDTO);
        }

        public async Task<ApiCommonResponse> GetAllVehicleDetails()
        {
            var master = await _serviceAssignmentDetailsRepository.FindAllVehicleServiceAssignmentDetails();
            if (master == null)
            {
                return CommonResponse.Send(ResponseCodes.NO_DATA_AVAILABLE);
            }
            var TransferDTO = _mapper.Map<IEnumerable<VehicleServiceAssignmentDetailsTransferDTO>>(master);
            return CommonResponse.Send(ResponseCodes.SUCCESS, TransferDTO);
        }

        public async Task<ApiCommonResponse> GetAllVehicleDetailsByAssignmentId(long assignmentId)
        {
            var master = await _serviceAssignmentDetailsRepository.FindAllVehicleServiceAssignmentDetailsByAssignmentId(assignmentId);
            if (master == null)
            {
                return CommonResponse.Send(ResponseCodes.NO_DATA_AVAILABLE);
            }
            var TransferDTO = _mapper.Map<IEnumerable<VehicleServiceAssignmentDetailsTransferDTO>>(master);
            return CommonResponse.Send(ResponseCodes.SUCCESS, TransferDTO);
        }

        public async Task<ApiCommonResponse> GetArmedEscortDetailById(long id)
        {
            var master = await _serviceAssignmentDetailsRepository.FindEscortServiceAssignmentDetailById(id);
            if (master == null)
            {
                return CommonResponse.Send(ResponseCodes.NO_DATA_AVAILABLE);
            }
            var TransferDTO = _mapper.Map<ArmedEscortServiceAssignmentDetailsTransferDTO>(master);
            return CommonResponse.Send(ResponseCodes.SUCCESS, TransferDTO);
        }

        public async Task<ApiCommonResponse> GetCommanderDetailById(long id)
        {
            var master = await _serviceAssignmentDetailsRepository.FindCommanderServiceAssignmentDetailById(id);
            if (master == null)
            {
                return CommonResponse.Send(ResponseCodes.NO_DATA_AVAILABLE);
            }
            var TransferDTO = _mapper.Map<CommanderServiceAssignmentDetailsTransferDTO>(master);
            return CommonResponse.Send(ResponseCodes.SUCCESS, TransferDTO);
        }

        public async Task<ApiCommonResponse> GetPassengerById(long id)
        {
            var master = await _serviceAssignmentDetailsRepository.FindPassengerById(id);
            if (master == null)
            {
                return CommonResponse.Send(ResponseCodes.NO_DATA_AVAILABLE);
            }
            var TransferDTO = _mapper.Map<PassengerTransferDTO>(master);
            return CommonResponse.Send(ResponseCodes.SUCCESS, TransferDTO);
        }

        public async Task<ApiCommonResponse> GetPilotDetailById(long id)
        {
            var master = await _serviceAssignmentDetailsRepository.FindPilotServiceAssignmentDetailById(id);
            if (master == null)
            {
                return CommonResponse.Send(ResponseCodes.NO_DATA_AVAILABLE);
            }
            var TransferDTO = _mapper.Map<PilotServiceAssignmentDetailsTransferDTO>(master);
            return CommonResponse.Send(ResponseCodes.SUCCESS, TransferDTO);
        }

        public async Task<ApiCommonResponse> GetVehicleDetailById(long id)
        {
            var master = await _serviceAssignmentDetailsRepository.FindVehicleServiceAssignmentDetailById(id);
            if (master == null)
            {
                return CommonResponse.Send(ResponseCodes.NO_DATA_AVAILABLE);
            }
            var TransferDTO = _mapper.Map<VehicleServiceAssignmentDetailsTransferDTO>(master);
            return CommonResponse.Send(ResponseCodes.SUCCESS, TransferDTO);
        }

        public async Task<ApiCommonResponse> UpdateArmedEscortDetail(HttpContext context, long id, ArmedEscortServiceAssignmentDetailsReceivingDTO armedEscortReceivingDTO)
        {
            var itemToUpdate = await _serviceAssignmentDetailsRepository.FindEscortServiceAssignmentDetailById(id);
            if (itemToUpdate == null)
            {
                return CommonResponse.Send(ResponseCodes.NO_DATA_AVAILABLE);
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
                return CommonResponse.Send(ResponseCodes.FAILURE, null, ResponseMessage.InternalServer500);
            }

            var TransferDTOs = _mapper.Map<ArmedEscortServiceAssignmentDetailsTransferDTO>(updatedItem);
            return CommonResponse.Send(ResponseCodes.SUCCESS, TransferDTOs);
        }

        public async Task<ApiCommonResponse> UpdateCommanderDetail(HttpContext context, long id, CommanderServiceAssignmentDetailsReceivingDTO commanderReceivingDTO)
        {
            var itemToUpdate = await _serviceAssignmentDetailsRepository.FindCommanderServiceAssignmentDetailById(id);
            if (itemToUpdate == null)
            {
                return CommonResponse.Send(ResponseCodes.NO_DATA_AVAILABLE);
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
                return CommonResponse.Send(ResponseCodes.FAILURE, null, ResponseMessage.InternalServer500);
            }

            var TransferDTOs = _mapper.Map<CommanderServiceAssignmentDetailsTransferDTO>(updatedItem);
            return CommonResponse.Send(ResponseCodes.SUCCESS, TransferDTOs);
        }

        public async Task<ApiCommonResponse> UpdatePassenger(HttpContext context, long id, PassengerReceivingDTO passengerReceivingDTO)
        {
            var itemToUpdate = await _serviceAssignmentDetailsRepository.FindPassengerById(id);
            if (itemToUpdate == null)
            {
                return CommonResponse.Send(ResponseCodes.NO_DATA_AVAILABLE);
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
                return CommonResponse.Send(ResponseCodes.FAILURE, null, ResponseMessage.InternalServer500);
            }

            var TransferDTOs = _mapper.Map<PassengerTransferDTO>(updatedItem);
            return CommonResponse.Send(ResponseCodes.SUCCESS, TransferDTOs);
        }

        public async Task<ApiCommonResponse> UpdatePilotDetail(HttpContext context, long id, PilotServiceAssignmentDetailsReceivingDTO pilotReceivingDTO)
        {
            var itemToUpdate = await _serviceAssignmentDetailsRepository.FindPilotServiceAssignmentDetailById(id);
            if (itemToUpdate == null)
            {
                return CommonResponse.Send(ResponseCodes.NO_DATA_AVAILABLE);
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
                return CommonResponse.Send(ResponseCodes.FAILURE, null, ResponseMessage.InternalServer500);
            }

            var TransferDTOs = _mapper.Map<PilotServiceAssignmentDetailsTransferDTO>(updatedItem);
            return CommonResponse.Send(ResponseCodes.SUCCESS, TransferDTOs);
        }

        public async Task<ApiCommonResponse> UpdateVehicleDetail(HttpContext context, long id, VehicleServiceAssignmentDetailsReceivingDTO vehicleReceivingDTO)
        {
            var itemToUpdate = await _serviceAssignmentDetailsRepository.FindVehicleServiceAssignmentDetailById(id);
            if (itemToUpdate == null)
            {
                return CommonResponse.Send(ResponseCodes.NO_DATA_AVAILABLE);
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
                return CommonResponse.Send(ResponseCodes.FAILURE, null, ResponseMessage.InternalServer500);
            }

            var TransferDTOs = _mapper.Map<VehicleServiceAssignmentDetailsTransferDTO>(updatedItem);
            return CommonResponse.Send(ResponseCodes.SUCCESS, TransferDTOs, ResponseMessage.Success200);
        }

        public static void CheckForAllForRequiredResource()
        {

        }

        public async Task<ApiCommonResponse> GetAllContracts()
        {
            var contracts = await _serviceAssignmentDetailsRepository.FindAllContracts();
            if (contracts == null)
            {
                return CommonResponse.Send(ResponseCodes.NO_DATA_AVAILABLE); ;
            }
            var contractTransferDTOs = _mapper.Map<IEnumerable<ContractTransferDTO>>(contracts);
            return CommonResponse.Send(ResponseCodes.SUCCESS, contractTransferDTOs);
        }

       

      
    }
}