using AutoMapper;
using Halobiz.Common.DTOs.ApiDTOs;
using HaloBiz.DTOs.ReceivingDTOs;
using HaloBiz.DTOs.TransferDTOs;
using HaloBiz.Helpers;
using HaloBiz.Repository;
using HalobizMigrations.Models.Armada;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HaloBiz.MyServices.Impl
{
    public class DTSDetailGenericDaysServiceImpl:IDTSDetailGenericDaysService
    {

        private readonly IDTSDetailGenericDaysRepository _dTSDetailGenericDaysRepository;
        private readonly IMapper _mapper;

        public DTSDetailGenericDaysServiceImpl(IMapper mapper, IDTSDetailGenericDaysRepository dTSDetailGenericDaysRepository)
        {
            _mapper = mapper;
            _dTSDetailGenericDaysRepository = dTSDetailGenericDaysRepository;
        }

        public async Task<ApiCommonResponse> AddArmedEscortGeneric(HttpContext context, ArmedEscortDTSDetailGenericDaysReceivingDTO armedEscortReceivingDTO)
        {
            var armedescort = _mapper.Map<ArmedEscortDTSDetailGenericDay>(armedEscortReceivingDTO);
            var masterIdExist = _dTSDetailGenericDaysRepository.GetArmedEscortDTSMasterId(armedEscortReceivingDTO.DTSMasterId);
            DateTime timeOpen = Convert.ToDateTime(armedEscortReceivingDTO.OpeningTime.AddHours(1));
            timeOpen = timeOpen.AddSeconds(-1 * timeOpen.Second);
            timeOpen = timeOpen.AddMilliseconds(-1 * timeOpen.Millisecond);
            DateTime timeClose = Convert.ToDateTime(armedEscortReceivingDTO.ClosingTime.AddHours(1));
            timeClose = timeClose.AddSeconds(-1 * timeClose.Second);
            timeClose = timeClose.AddMilliseconds(-1 * timeClose.Millisecond);

            if (timeClose.TimeOfDay.CompareTo(timeOpen.TimeOfDay) == 0)
            {
                return CommonResponse.Send(ResponseCodes.FAILURE);
            }
            if (timeClose.TimeOfDay.CompareTo(timeOpen.TimeOfDay) < 0)
            {
                return CommonResponse.Send(ResponseCodes.FAILURE);
            }
           
            if (masterIdExist != null)
            {
                DateTime IdExistClose = Convert.ToDateTime(masterIdExist.ClosingTime);
                IdExistClose = IdExistClose.AddSeconds(-1 * IdExistClose.Second);
                IdExistClose = IdExistClose.AddMilliseconds(-1 * IdExistClose.Millisecond);
                if (timeOpen.TimeOfDay < IdExistClose.TimeOfDay)
                {
                    return CommonResponse.Send(ResponseCodes.FAILURE);
                }

            }
            if(armedEscortReceivingDTO.Monday == false && armedEscortReceivingDTO.Tuesday == false && armedEscortReceivingDTO.Wednesday == false && armedEscortReceivingDTO.Thursday == false && armedEscortReceivingDTO.Friday == false
                && armedEscortReceivingDTO.Saturday == false && armedEscortReceivingDTO.Sunday == false )
            {
                return CommonResponse.Send(ResponseCodes.FAILURE);
            }
            armedescort.OpeningTime = timeOpen;
            armedescort.ClosingTime = timeClose;
            armedescort.CreatedById = context.GetLoggedInUserId();
            armedescort.CreatedAt = DateTime.UtcNow;
            var save = await _dTSDetailGenericDaysRepository.SaveArmedEscortGeneric(armedescort);
            if (save == null)
            {
                return CommonResponse.Send(ResponseCodes.FAILURE, null, "Some system errors occurred");
            }
            var TransferDTO = _mapper.Map<ArmedEscortDTSDetailGenericDaysTransferDTO>(armedescort);
            return CommonResponse.Send(ResponseCodes.SUCCESS,TransferDTO);
        }

        public async Task<ApiCommonResponse> AddCommanderGeneric(HttpContext context, CommanderDTSDetailGenericDaysReceivingDTO commanderReceivingDTO)
        {
            var commander = _mapper.Map<CommanderDTSDetailGenericDay>(commanderReceivingDTO);
            var masterIdExist = _dTSDetailGenericDaysRepository.GetCommanderDTSMasterId(commanderReceivingDTO.DTSMasterId);
            DateTime timeOpen = Convert.ToDateTime(commanderReceivingDTO.OpeningTime.AddHours(1));
            timeOpen = timeOpen.AddSeconds(-1 * timeOpen.Second);
            timeOpen = timeOpen.AddMilliseconds(-1 * timeOpen.Millisecond);
            DateTime timeClose = Convert.ToDateTime(commanderReceivingDTO.ClosingTime.AddHours(1));
            timeClose = timeClose.AddSeconds(-1 * timeClose.Second);
            timeClose = timeClose.AddMilliseconds(-1 * timeClose.Millisecond);

            if (timeClose.TimeOfDay.CompareTo(timeOpen.TimeOfDay) == 0)
            {
                return CommonResponse.Send(ResponseCodes.FAILURE);
            }
            if (timeClose.TimeOfDay.CompareTo(timeOpen.TimeOfDay) < 0)
            {
                return CommonResponse.Send(ResponseCodes.FAILURE);
            }

            if (masterIdExist != null)
            {
                DateTime IdExistClose = Convert.ToDateTime(masterIdExist.ClosingTime);
                IdExistClose = IdExistClose.AddSeconds(-1 * IdExistClose.Second);
                IdExistClose = IdExistClose.AddMilliseconds(-1 * IdExistClose.Millisecond);
                if (timeOpen.TimeOfDay < IdExistClose.TimeOfDay)
                {
                    return CommonResponse.Send(ResponseCodes.FAILURE);
                }

            }
            if (commanderReceivingDTO.Monday == false && commanderReceivingDTO.Tuesday == false && commanderReceivingDTO.Wednesday == false && commanderReceivingDTO.Thursday == false && commanderReceivingDTO.Friday == false
              && commanderReceivingDTO.Saturday == false && commanderReceivingDTO.Sunday == false)
            {
                return CommonResponse.Send(ResponseCodes.FAILURE);
            }
            commander.OpeningTime = timeOpen;
            commander.ClosingTime = timeClose;
            commander.CreatedById = context.GetLoggedInUserId();
            commander.CreatedAt = DateTime.UtcNow;
            var save = await _dTSDetailGenericDaysRepository.SaveCommanderGeneric(commander);
            if (save == null)
            {
                return CommonResponse.Send(ResponseCodes.FAILURE, null, "Some system errors occurred");
            }
            var TransferDTO = _mapper.Map<CommanderDTSDetailGenericDaysTransferDTO>(commander);
            return CommonResponse.Send(ResponseCodes.SUCCESS,TransferDTO);
        }

        public async Task<ApiCommonResponse> AddPilotGeneric(HttpContext context, PilotDTSDetailGenericDaysReceivingDTO pilotReceivingDTO)
        {
            var pilot = _mapper.Map<PilotDTSDetailGenericDay>(pilotReceivingDTO);
            var masterIdExist = _dTSDetailGenericDaysRepository.GetPilotDTSMasterId(pilotReceivingDTO.DTSMasterId);
            DateTime timeOpen = Convert.ToDateTime(pilotReceivingDTO.OpeningTime.AddHours(1));
            timeOpen = timeOpen.AddSeconds(-1 * timeOpen.Second);
            timeOpen = timeOpen.AddMilliseconds(-1 * timeOpen.Millisecond);
            DateTime timeClose = Convert.ToDateTime(pilotReceivingDTO.ClosingTime.AddHours(1));
            timeClose = timeClose.AddSeconds(-1 * timeClose.Second);
            timeClose = timeClose.AddMilliseconds(-1 * timeClose.Millisecond);

            if (timeClose.TimeOfDay.CompareTo(timeOpen.TimeOfDay) == 0)
            {
                return CommonResponse.Send(ResponseCodes.FAILURE);
            }
            if (timeClose.TimeOfDay.CompareTo(timeOpen.TimeOfDay) < 0)
            {
                return CommonResponse.Send(ResponseCodes.FAILURE);
            }

            if (masterIdExist != null)
            {
                DateTime IdExistClose = Convert.ToDateTime(masterIdExist.ClosingTime);
                IdExistClose = IdExistClose.AddSeconds(-1 * IdExistClose.Second);
                IdExistClose = IdExistClose.AddMilliseconds(-1 * IdExistClose.Millisecond);
                if (timeOpen.TimeOfDay < IdExistClose.TimeOfDay)
                {
                    return CommonResponse.Send(ResponseCodes.FAILURE);
                }

            }
            if (pilotReceivingDTO.Monday == false && pilotReceivingDTO.Tuesday == false && pilotReceivingDTO.Wednesday == false && pilotReceivingDTO.Thursday == false && pilotReceivingDTO.Friday == false
              && pilotReceivingDTO.Saturday == false && pilotReceivingDTO.Sunday == false)
            {
                return CommonResponse.Send(ResponseCodes.FAILURE);
            }
            pilot.OpeningTime = timeOpen;
            pilot.ClosingTime = timeClose;
            pilot.CreatedById = context.GetLoggedInUserId();
            pilot.CreatedAt = DateTime.UtcNow;
            var save = await _dTSDetailGenericDaysRepository.SavePilotGeneric(pilot);
            if (save == null)
            {
                return CommonResponse.Send(ResponseCodes.FAILURE, null, "Some system errors occurred");
            }
            var TransferDTO = _mapper.Map<PilotDTSDetailGenericDaysTransferDTO>(pilot);
            return CommonResponse.Send(ResponseCodes.SUCCESS,TransferDTO);
        }

        public async Task<ApiCommonResponse> AddVehicleGeneric(HttpContext context, VehicleDTSDetailGenericDaysReceivingDTO vehicleReceivingDTO)
        {
            var vehicle = _mapper.Map<VehicleDTSDetailGenericDay>(vehicleReceivingDTO);
            var masterIdExist = _dTSDetailGenericDaysRepository.GetVehicleDTSMasterId(vehicleReceivingDTO.DTSMasterId);
            DateTime timeOpen = Convert.ToDateTime(vehicleReceivingDTO.OpeningTime.AddHours(1));
            timeOpen = timeOpen.AddSeconds(-1 * timeOpen.Second);
            timeOpen = timeOpen.AddMilliseconds(-1 * timeOpen.Millisecond);
            DateTime timeClose = Convert.ToDateTime(vehicleReceivingDTO.ClosingTime.AddHours(1));
            timeClose = timeClose.AddSeconds(-1 * timeClose.Second);
            timeClose = timeClose.AddMilliseconds(-1 * timeClose.Millisecond);

            if (timeClose.TimeOfDay.CompareTo(timeOpen.TimeOfDay) == 0)
            {
                return CommonResponse.Send(ResponseCodes.FAILURE);
            }
            if (timeClose.TimeOfDay.CompareTo(timeOpen.TimeOfDay) < 0)
            {
                return CommonResponse.Send(ResponseCodes.FAILURE);
            }

            if (masterIdExist != null)
            {
                DateTime IdExistClose = Convert.ToDateTime(masterIdExist.ClosingTime);
                IdExistClose = IdExistClose.AddSeconds(-1 * IdExistClose.Second);
                IdExistClose = IdExistClose.AddMilliseconds(-1 * IdExistClose.Millisecond);
                if (timeOpen.TimeOfDay < IdExistClose.TimeOfDay)
                {
                    return CommonResponse.Send(ResponseCodes.FAILURE);
                }

            }
            if (vehicleReceivingDTO.Monday == false && vehicleReceivingDTO.Tuesday == false && vehicleReceivingDTO.Wednesday == false && vehicleReceivingDTO.Thursday == false && vehicleReceivingDTO.Friday == false
              && vehicleReceivingDTO.Saturday == false && vehicleReceivingDTO.Sunday == false)
            {
                return CommonResponse.Send(ResponseCodes.FAILURE);
            }
            vehicle.OpeningTime = timeOpen;
            vehicle.ClosingTime = timeClose;
            vehicle.CreatedById = context.GetLoggedInUserId();
            vehicle.CreatedAt = DateTime.UtcNow;
            var save = await _dTSDetailGenericDaysRepository.SaveVehicleGeneric(vehicle);
            if (save == null)
            {
                return CommonResponse.Send(ResponseCodes.FAILURE, null, "Some system errors occurred");
            }
            var TransferDTO = _mapper.Map<VehicleDTSDetailGenericDaysTransferDTO>(vehicle);
            return CommonResponse.Send(ResponseCodes.SUCCESS,TransferDTO);
        }

        public async Task<ApiCommonResponse> DeleteArmedEscortGeneric(long id)
        {
            var itemToDelete = await _dTSDetailGenericDaysRepository.FindArmedEscortGenericById(id);

            if (itemToDelete == null)
            {
                return CommonResponse.Send(ResponseCodes.NO_DATA_AVAILABLE);;
            }

            if (!await _dTSDetailGenericDaysRepository.DeleteArmedEscortGeneric(itemToDelete))
            {
                return CommonResponse.Send(ResponseCodes.FAILURE, null, "Some system errors occurred");
            }

            return CommonResponse.Send(ResponseCodes.SUCCESS);
        }

        public async Task<ApiCommonResponse> DeleteCommanderGeneric(long id)
        {
            var itemToDelete = await _dTSDetailGenericDaysRepository.FindCommanderGenericById(id);

            if (itemToDelete == null)
            {
                return CommonResponse.Send(ResponseCodes.NO_DATA_AVAILABLE);;
            }

            if (!await _dTSDetailGenericDaysRepository.DeleteCommanderGeneric(itemToDelete))
            {
                return CommonResponse.Send(ResponseCodes.FAILURE, null, "Some system errors occurred");
            }

            return CommonResponse.Send(ResponseCodes.SUCCESS);
        }

        public async Task<ApiCommonResponse> DeletePilotGeneric(long id)
        {
            var itemToDelete = await _dTSDetailGenericDaysRepository.FindPilotGenericById(id);

            if (itemToDelete == null)
            {
                return CommonResponse.Send(ResponseCodes.NO_DATA_AVAILABLE);;
            }

            if (!await _dTSDetailGenericDaysRepository.DeletePilotGeneric(itemToDelete))
            {
                return CommonResponse.Send(ResponseCodes.FAILURE, null, "Some system errors occurred");
            }

            return CommonResponse.Send(ResponseCodes.SUCCESS);
        }

        public async Task<ApiCommonResponse> DeleteVehicleGeneric(long id)
        {
            var itemToDelete = await _dTSDetailGenericDaysRepository.FindVehicleGenericById(id);

            if (itemToDelete == null)
            {
                return CommonResponse.Send(ResponseCodes.NO_DATA_AVAILABLE);;
            }

            if (!await _dTSDetailGenericDaysRepository.DeleteVehicleGeneric(itemToDelete))
            {
                return CommonResponse.Send(ResponseCodes.FAILURE, null, "Some system errors occurred");
            }

            return CommonResponse.Send(ResponseCodes.SUCCESS);
        }

        public async Task<ApiCommonResponse> GetAllArmedEscortGenerics()
        {
            var masters = await _dTSDetailGenericDaysRepository.FindAllArmedEscortGenerics();
            if (masters == null)
            {
                return CommonResponse.Send(ResponseCodes.NO_DATA_AVAILABLE);;
            }
            var TransferDTO = _mapper.Map<IEnumerable<ArmedEscortDTSDetailGenericDaysTransferDTO>>(masters);
            return CommonResponse.Send(ResponseCodes.SUCCESS,TransferDTO);
        }

        public async Task<ApiCommonResponse> GetAllCommanderGenerics()
        {
            var masters = await _dTSDetailGenericDaysRepository.FindAllCommanderGenerics();
            if (masters == null)
            {
                return CommonResponse.Send(ResponseCodes.NO_DATA_AVAILABLE);;
            }
            var TransferDTO = _mapper.Map<IEnumerable<CommanderDTSDetailGenericDaysTransferDTO>>(masters);
            return CommonResponse.Send(ResponseCodes.SUCCESS,TransferDTO);
        }

        public async Task<ApiCommonResponse> GetAllPilotGenerics()
        {
            var masters = await _dTSDetailGenericDaysRepository.FindAllPilotGenerics();
            if (masters == null)
            {
                return CommonResponse.Send(ResponseCodes.NO_DATA_AVAILABLE);;
            }
            var TransferDTO = _mapper.Map<IEnumerable<PilotDTSDetailGenericDaysTransferDTO>>(masters);
            return CommonResponse.Send(ResponseCodes.SUCCESS,TransferDTO);
        }

        public async Task<ApiCommonResponse> GetAllVehicleGenerics()
        {
            var masters = await _dTSDetailGenericDaysRepository.FindAllVehicleGenerics();
            if (masters == null)
            {
                return CommonResponse.Send(ResponseCodes.NO_DATA_AVAILABLE);;
            }
            var TransferDTO = _mapper.Map<IEnumerable<VehicleDTSDetailGenericDaysTransferDTO>>(masters);
            return CommonResponse.Send(ResponseCodes.SUCCESS,TransferDTO);
        }

        public async Task<ApiCommonResponse> GetArmedEscortGenericById(long id)
        {
            var master = await _dTSDetailGenericDaysRepository.FindArmedEscortGenericById(id);
            if (master == null)
            {
                return CommonResponse.Send(ResponseCodes.NO_DATA_AVAILABLE);;
            }
            var TransferDTO = _mapper.Map<ArmedEscortDTSDetailGenericDaysTransferDTO>(master);
            return CommonResponse.Send(ResponseCodes.SUCCESS,TransferDTO);
        }

        public async Task<ApiCommonResponse> GetArmedEscortGenericByMasterId(long id)
        {
            var master = await _dTSDetailGenericDaysRepository.FindArmedEscortGenericByMasterId(id);
            if (master == null)
            {
                return CommonResponse.Send(ResponseCodes.NO_DATA_AVAILABLE);;
            }
            var TransferDTO = _mapper.Map<IEnumerable<ArmedEscortDTSDetailGenericDaysTransferDTO>>(master);
            return CommonResponse.Send(ResponseCodes.SUCCESS,TransferDTO);
        }

        public async Task<ApiCommonResponse> GetCommanderGenericById(long id)
        {
            var master = await _dTSDetailGenericDaysRepository.FindCommanderGenericById(id);
            if (master == null)
            {
                return CommonResponse.Send(ResponseCodes.NO_DATA_AVAILABLE);;
            }
            var TransferDTO = _mapper.Map<CommanderDTSDetailGenericDaysTransferDTO>(master);
            return CommonResponse.Send(ResponseCodes.SUCCESS,TransferDTO);
        }

        public async Task<ApiCommonResponse> GetCommanderGenericByMasterId(long id)
        {
            var master = await _dTSDetailGenericDaysRepository.FindCommanderGenericByMasterId(id);
            if (master == null)
            {
                return CommonResponse.Send(ResponseCodes.NO_DATA_AVAILABLE);;
            }
           
            var TransferDTO = _mapper.Map<IEnumerable<CommanderDTSDetailGenericDaysTransferDTO>>(master);
            return CommonResponse.Send(ResponseCodes.SUCCESS,TransferDTO);
        }

        public async Task<ApiCommonResponse> GetPilotGenericById(long id)
        {
            var master = await _dTSDetailGenericDaysRepository.FindPilotGenericById(id);
            if (master == null)
            {
                return CommonResponse.Send(ResponseCodes.NO_DATA_AVAILABLE);;
            }
            var TransferDTO = _mapper.Map<PilotDTSDetailGenericDaysTransferDTO>(master);
            return CommonResponse.Send(ResponseCodes.SUCCESS,TransferDTO);
        }

        public async Task<ApiCommonResponse> GetPilotGenericByMasterId(long id)
        {
            var master = await _dTSDetailGenericDaysRepository.FindPilotGenericByMasterId(id);
            if (master == null)
            {
                return CommonResponse.Send(ResponseCodes.NO_DATA_AVAILABLE);;
            }
            var TransferDTO = _mapper.Map<IEnumerable<PilotDTSDetailGenericDaysTransferDTO>>(master);
            return CommonResponse.Send(ResponseCodes.SUCCESS,TransferDTO);
        }

        public async Task<ApiCommonResponse> GetVehicleGenericById(long id)
        {
            var master = await _dTSDetailGenericDaysRepository.FindVehicleGenericById(id);
            if (master == null)
            {
                return CommonResponse.Send(ResponseCodes.NO_DATA_AVAILABLE);;
            }
            var TransferDTO = _mapper.Map<VehicleDTSDetailGenericDaysTransferDTO>(master);
            return CommonResponse.Send(ResponseCodes.SUCCESS,TransferDTO);
        }

        public async Task<ApiCommonResponse> GetVehicleGenericByMasterId(long id)
        {
            var master = await _dTSDetailGenericDaysRepository.FindVehicleGenericByMasterId(id);
            if (master == null)
            {
                return CommonResponse.Send(ResponseCodes.NO_DATA_AVAILABLE);;
            }
            var TransferDTO = _mapper.Map<IEnumerable<VehicleDTSDetailGenericDaysTransferDTO>>(master);
            return CommonResponse.Send(ResponseCodes.SUCCESS,TransferDTO);
        }

        public async Task<ApiCommonResponse> UpdateArmedEscortGeneric(HttpContext context, long id, ArmedEscortDTSDetailGenericDaysReceivingDTO armedEscortReceivingDTO)
        {
            var itemToUpdate = await _dTSDetailGenericDaysRepository.FindArmedEscortGenericById(id);
            if (itemToUpdate == null)
            {
                return CommonResponse.Send(ResponseCodes.NO_DATA_AVAILABLE);;
            }

            var summary = $"Initial details before change, \n {itemToUpdate.ToString()} \n";

            itemToUpdate.Monday = armedEscortReceivingDTO.Monday;
            itemToUpdate.Tuesday = armedEscortReceivingDTO.Tuesday;
            itemToUpdate.Wednesday = armedEscortReceivingDTO.Wednesday;
            itemToUpdate.Thursday = armedEscortReceivingDTO.Thursday;

            itemToUpdate.Friday = armedEscortReceivingDTO.Friday;
            itemToUpdate.Saturday = armedEscortReceivingDTO.Saturday;
            itemToUpdate.Sunday = armedEscortReceivingDTO.Sunday;
            itemToUpdate.OpeningTime = armedEscortReceivingDTO.OpeningTime;
            itemToUpdate.OpeningTime = armedEscortReceivingDTO.ClosingTime;
            itemToUpdate.ClosingTime = DateTime.UtcNow;
            var updateMaster = await _dTSDetailGenericDaysRepository.UpdateArmedEscortGeneric(itemToUpdate);

            summary += $"Details after change, \n {updateMaster.ToString()} \n";

            if (updateMaster == null)
            {
                return CommonResponse.Send(ResponseCodes.FAILURE, null, "Some system errors occurred");
            }

            var TransferDTOs = _mapper.Map<ArmedEscortDTSDetailGenericDaysTransferDTO>(updateMaster);
            return CommonResponse.Send(ResponseCodes.SUCCESS,TransferDTOs);
        }

        public async Task<ApiCommonResponse> UpdateCommanderGeneric(HttpContext context, long id, CommanderDTSDetailGenericDaysReceivingDTO commanderReceivingDTO)
        {
            var itemToUpdate = await _dTSDetailGenericDaysRepository.FindCommanderGenericById(id);
            if (itemToUpdate == null)
            {
                return CommonResponse.Send(ResponseCodes.NO_DATA_AVAILABLE);;
            }

            var summary = $"Initial details before change, \n {itemToUpdate.ToString()} \n";

            itemToUpdate.Monday = commanderReceivingDTO.Monday;
            itemToUpdate.Tuesday = commanderReceivingDTO.Tuesday;
            itemToUpdate.Wednesday = commanderReceivingDTO.Wednesday;
            itemToUpdate.Thursday = commanderReceivingDTO.Thursday;

            itemToUpdate.Friday = commanderReceivingDTO.Friday;
            itemToUpdate.Saturday = commanderReceivingDTO.Saturday;
            itemToUpdate.Sunday = commanderReceivingDTO.Sunday;
            itemToUpdate.OpeningTime = commanderReceivingDTO.OpeningTime;
            itemToUpdate.OpeningTime = commanderReceivingDTO.ClosingTime;
            itemToUpdate.ClosingTime = DateTime.UtcNow;
            var updateMaster = await _dTSDetailGenericDaysRepository.UpdateCommanderGeneric(itemToUpdate);

            summary += $"Details after change, \n {updateMaster.ToString()} \n";

            if (updateMaster == null)
            {
                return CommonResponse.Send(ResponseCodes.FAILURE, null, "Some system errors occurred");
            }

            var TransferDTOs = _mapper.Map<ArmedEscortDTSDetailGenericDaysTransferDTO>(updateMaster);
            return CommonResponse.Send(ResponseCodes.SUCCESS,TransferDTOs);
        }

        public async Task<ApiCommonResponse> UpdatePilotGeneric(HttpContext context, long id, PilotDTSDetailGenericDaysReceivingDTO pilotReceivingDTO)
        {
            var itemToUpdate = await _dTSDetailGenericDaysRepository.FindPilotGenericById(id);
            if (itemToUpdate == null)
            {
                return CommonResponse.Send(ResponseCodes.NO_DATA_AVAILABLE);;
            }

            var summary = $"Initial details before change, \n {itemToUpdate.ToString()} \n";

            itemToUpdate.Monday = pilotReceivingDTO.Monday;
            itemToUpdate.Tuesday = pilotReceivingDTO.Tuesday;
            itemToUpdate.Wednesday = pilotReceivingDTO.Wednesday;
            itemToUpdate.Thursday = pilotReceivingDTO.Thursday;

            itemToUpdate.Friday = pilotReceivingDTO.Friday;
            itemToUpdate.Saturday = pilotReceivingDTO.Saturday;
            itemToUpdate.Sunday = pilotReceivingDTO.Sunday;
            itemToUpdate.OpeningTime = pilotReceivingDTO.OpeningTime;
            itemToUpdate.OpeningTime = pilotReceivingDTO.ClosingTime;
            itemToUpdate.ClosingTime = DateTime.UtcNow;
            var updateMaster = await _dTSDetailGenericDaysRepository.UpdatePilotGeneric(itemToUpdate);

            summary += $"Details after change, \n {updateMaster.ToString()} \n";

            if (updateMaster == null)
            {
                return CommonResponse.Send(ResponseCodes.FAILURE, null, "Some system errors occurred");
            }

            var TransferDTOs = _mapper.Map<ArmedEscortDTSDetailGenericDaysTransferDTO>(updateMaster);
            return CommonResponse.Send(ResponseCodes.SUCCESS,TransferDTOs);
        }

        public async Task<ApiCommonResponse> UpdateVehicleGeneric(HttpContext context, long id, VehicleDTSDetailGenericDaysReceivingDTO vehicleReceivingDTO)
        {
            var itemToUpdate = await _dTSDetailGenericDaysRepository.FindVehicleGenericById(id);
            if (itemToUpdate == null)
            {
                return CommonResponse.Send(ResponseCodes.NO_DATA_AVAILABLE);;
            }

            var summary = $"Initial details before change, \n {itemToUpdate.ToString()} \n";

            itemToUpdate.Monday = vehicleReceivingDTO.Monday;
            itemToUpdate.Tuesday = vehicleReceivingDTO.Tuesday;
            itemToUpdate.Wednesday = vehicleReceivingDTO.Wednesday;
            itemToUpdate.Thursday = vehicleReceivingDTO.Thursday;

            itemToUpdate.Friday = vehicleReceivingDTO.Friday;
            itemToUpdate.Saturday = vehicleReceivingDTO.Saturday;
            itemToUpdate.Sunday = vehicleReceivingDTO.Sunday;
            itemToUpdate.OpeningTime = vehicleReceivingDTO.OpeningTime;
            itemToUpdate.OpeningTime = vehicleReceivingDTO.ClosingTime;
            itemToUpdate.ClosingTime = DateTime.UtcNow;
            var updateMaster = await _dTSDetailGenericDaysRepository.UpdatevehicleGenric(itemToUpdate);

            summary += $"Details after change, \n {updateMaster.ToString()} \n";

            if (updateMaster == null)
            {
                return CommonResponse.Send(ResponseCodes.FAILURE, null, "Some system errors occurred");
            }

            var TransferDTOs = _mapper.Map<ArmedEscortDTSDetailGenericDaysTransferDTO>(updateMaster);
            return CommonResponse.Send(ResponseCodes.SUCCESS,TransferDTOs);
        }
    }
}
