using AutoMapper;
using HaloBiz.DTOs.ApiDTOs;
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

        public async Task<ApiResponse> AddArmedEscortGeneric(HttpContext context, ArmedEscortDTSDetailGenericDaysReceivingDTO armedEscortReceivingDTO)
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
                return new ApiResponse(441);
            }
            if (timeClose.TimeOfDay.CompareTo(timeOpen.TimeOfDay) < 0)
            {
                return new ApiResponse(441);
            }
           
            if (masterIdExist != null)
            {
                DateTime IdExistClose = Convert.ToDateTime(masterIdExist.ClosingTime);
                IdExistClose = IdExistClose.AddSeconds(-1 * IdExistClose.Second);
                IdExistClose = IdExistClose.AddMilliseconds(-1 * IdExistClose.Millisecond);
                if (timeOpen.TimeOfDay < IdExistClose.TimeOfDay)
                {
                    return new ApiResponse(441);
                }

            }
            if(armedEscortReceivingDTO.Monday == false && armedEscortReceivingDTO.Tuesday == false && armedEscortReceivingDTO.Wednesday == false && armedEscortReceivingDTO.Thursday == false && armedEscortReceivingDTO.Friday == false
                && armedEscortReceivingDTO.Saturday == false && armedEscortReceivingDTO.Sunday == false )
            {
                return new ApiResponse(442);
            }
            armedescort.OpeningTime = timeOpen;
            armedescort.ClosingTime = timeClose;
            armedescort.CreatedById = context.GetLoggedInUserId();
            armedescort.CreatedAt = DateTime.UtcNow;
            var save = await _dTSDetailGenericDaysRepository.SaveArmedEscortGeneric(armedescort);
            if (save == null)
            {
                return new ApiResponse(500);
            }
            var TransferDTO = _mapper.Map<ArmedEscortDTSDetailGenericDaysTransferDTO>(armedescort);
            return new ApiOkResponse(TransferDTO);
        }

        public async Task<ApiResponse> AddCommanderGeneric(HttpContext context, CommanderDTSDetailGenericDaysReceivingDTO commanderReceivingDTO)
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
                return new ApiResponse(441);
            }
            if (timeClose.TimeOfDay.CompareTo(timeOpen.TimeOfDay) < 0)
            {
                return new ApiResponse(441);
            }

            if (masterIdExist != null)
            {
                DateTime IdExistClose = Convert.ToDateTime(masterIdExist.ClosingTime);
                IdExistClose = IdExistClose.AddSeconds(-1 * IdExistClose.Second);
                IdExistClose = IdExistClose.AddMilliseconds(-1 * IdExistClose.Millisecond);
                if (timeOpen.TimeOfDay < IdExistClose.TimeOfDay)
                {
                    return new ApiResponse(441);
                }

            }
            if (commanderReceivingDTO.Monday == false && commanderReceivingDTO.Tuesday == false && commanderReceivingDTO.Wednesday == false && commanderReceivingDTO.Thursday == false && commanderReceivingDTO.Friday == false
              && commanderReceivingDTO.Saturday == false && commanderReceivingDTO.Sunday == false)
            {
                return new ApiResponse(442);
            }
            commander.OpeningTime = timeOpen;
            commander.ClosingTime = timeClose;
            commander.CreatedById = context.GetLoggedInUserId();
            commander.CreatedAt = DateTime.UtcNow;
            var save = await _dTSDetailGenericDaysRepository.SaveCommanderGeneric(commander);
            if (save == null)
            {
                return new ApiResponse(500);
            }
            var TransferDTO = _mapper.Map<CommanderDTSDetailGenericDaysTransferDTO>(commander);
            return new ApiOkResponse(TransferDTO);
        }

        public async Task<ApiResponse> AddPilotGeneric(HttpContext context, PilotDTSDetailGenericDaysReceivingDTO pilotReceivingDTO)
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
                return new ApiResponse(441);
            }
            if (timeClose.TimeOfDay.CompareTo(timeOpen.TimeOfDay) < 0)
            {
                return new ApiResponse(441);
            }

            if (masterIdExist != null)
            {
                DateTime IdExistClose = Convert.ToDateTime(masterIdExist.ClosingTime);
                IdExistClose = IdExistClose.AddSeconds(-1 * IdExistClose.Second);
                IdExistClose = IdExistClose.AddMilliseconds(-1 * IdExistClose.Millisecond);
                if (timeOpen.TimeOfDay < IdExistClose.TimeOfDay)
                {
                    return new ApiResponse(441);
                }

            }
            if (pilotReceivingDTO.Monday == false && pilotReceivingDTO.Tuesday == false && pilotReceivingDTO.Wednesday == false && pilotReceivingDTO.Thursday == false && pilotReceivingDTO.Friday == false
              && pilotReceivingDTO.Saturday == false && pilotReceivingDTO.Sunday == false)
            {
                return new ApiResponse(442);
            }
            pilot.OpeningTime = timeOpen;
            pilot.ClosingTime = timeClose;
            pilot.CreatedById = context.GetLoggedInUserId();
            pilot.CreatedAt = DateTime.UtcNow;
            var save = await _dTSDetailGenericDaysRepository.SavePilotGeneric(pilot);
            if (save == null)
            {
                return new ApiResponse(500);
            }
            var TransferDTO = _mapper.Map<PilotDTSDetailGenericDaysTransferDTO>(pilot);
            return new ApiOkResponse(TransferDTO);
        }

        public async Task<ApiResponse> AddVehicleGeneric(HttpContext context, VehicleDTSDetailGenericDaysReceivingDTO vehicleReceivingDTO)
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
                return new ApiResponse(441);
            }
            if (timeClose.TimeOfDay.CompareTo(timeOpen.TimeOfDay) < 0)
            {
                return new ApiResponse(441);
            }

            if (masterIdExist != null)
            {
                DateTime IdExistClose = Convert.ToDateTime(masterIdExist.ClosingTime);
                IdExistClose = IdExistClose.AddSeconds(-1 * IdExistClose.Second);
                IdExistClose = IdExistClose.AddMilliseconds(-1 * IdExistClose.Millisecond);
                if (timeOpen.TimeOfDay < IdExistClose.TimeOfDay)
                {
                    return new ApiResponse(441);
                }

            }
            if (vehicleReceivingDTO.Monday == false && vehicleReceivingDTO.Tuesday == false && vehicleReceivingDTO.Wednesday == false && vehicleReceivingDTO.Thursday == false && vehicleReceivingDTO.Friday == false
              && vehicleReceivingDTO.Saturday == false && vehicleReceivingDTO.Sunday == false)
            {
                return new ApiResponse(442);
            }
            vehicle.OpeningTime = timeOpen;
            vehicle.ClosingTime = timeClose;
            vehicle.CreatedById = context.GetLoggedInUserId();
            vehicle.CreatedAt = DateTime.UtcNow;
            var save = await _dTSDetailGenericDaysRepository.SaveVehicleGeneric(vehicle);
            if (save == null)
            {
                return new ApiResponse(500);
            }
            var TransferDTO = _mapper.Map<VehicleDTSDetailGenericDaysTransferDTO>(vehicle);
            return new ApiOkResponse(TransferDTO);
        }

        public async Task<ApiResponse> DeleteArmedEscortGeneric(long id)
        {
            var itemToDelete = await _dTSDetailGenericDaysRepository.FindArmedEscortGenericById(id);

            if (itemToDelete == null)
            {
                return new ApiResponse(404);
            }

            if (!await _dTSDetailGenericDaysRepository.DeleteArmedEscortGeneric(itemToDelete))
            {
                return new ApiResponse(500);
            }

            return new ApiOkResponse(true);
        }

        public async Task<ApiResponse> DeleteCommanderGeneric(long id)
        {
            var itemToDelete = await _dTSDetailGenericDaysRepository.FindCommanderGenericById(id);

            if (itemToDelete == null)
            {
                return new ApiResponse(404);
            }

            if (!await _dTSDetailGenericDaysRepository.DeleteCommanderGeneric(itemToDelete))
            {
                return new ApiResponse(500);
            }

            return new ApiOkResponse(true);
        }

        public async Task<ApiResponse> DeletePilotGeneric(long id)
        {
            var itemToDelete = await _dTSDetailGenericDaysRepository.FindPilotGenericById(id);

            if (itemToDelete == null)
            {
                return new ApiResponse(404);
            }

            if (!await _dTSDetailGenericDaysRepository.DeletePilotGeneric(itemToDelete))
            {
                return new ApiResponse(500);
            }

            return new ApiOkResponse(true);
        }

        public async Task<ApiResponse> DeleteVehicleGeneric(long id)
        {
            var itemToDelete = await _dTSDetailGenericDaysRepository.FindVehicleGenericById(id);

            if (itemToDelete == null)
            {
                return new ApiResponse(404);
            }

            if (!await _dTSDetailGenericDaysRepository.DeleteVehicleGeneric(itemToDelete))
            {
                return new ApiResponse(500);
            }

            return new ApiOkResponse(true);
        }

        public async Task<ApiResponse> GetAllArmedEscortGenerics()
        {
            var masters = await _dTSDetailGenericDaysRepository.FindAllArmedEscortGenerics();
            if (masters == null)
            {
                return new ApiResponse(404);
            }
            var TransferDTO = _mapper.Map<IEnumerable<ArmedEscortDTSDetailGenericDaysTransferDTO>>(masters);
            return new ApiOkResponse(TransferDTO);
        }

        public async Task<ApiResponse> GetAllCommanderGenerics()
        {
            var masters = await _dTSDetailGenericDaysRepository.FindAllCommanderGenerics();
            if (masters == null)
            {
                return new ApiResponse(404);
            }
            var TransferDTO = _mapper.Map<IEnumerable<CommanderDTSDetailGenericDaysTransferDTO>>(masters);
            return new ApiOkResponse(TransferDTO);
        }

        public async Task<ApiResponse> GetAllPilotGenerics()
        {
            var masters = await _dTSDetailGenericDaysRepository.FindAllPilotGenerics();
            if (masters == null)
            {
                return new ApiResponse(404);
            }
            var TransferDTO = _mapper.Map<IEnumerable<PilotDTSDetailGenericDaysTransferDTO>>(masters);
            return new ApiOkResponse(TransferDTO);
        }

        public async Task<ApiResponse> GetAllVehicleGenerics()
        {
            var masters = await _dTSDetailGenericDaysRepository.FindAllVehicleGenerics();
            if (masters == null)
            {
                return new ApiResponse(404);
            }
            var TransferDTO = _mapper.Map<IEnumerable<VehicleDTSDetailGenericDaysTransferDTO>>(masters);
            return new ApiOkResponse(TransferDTO);
        }

        public async Task<ApiResponse> GetArmedEscortGenericById(long id)
        {
            var master = await _dTSDetailGenericDaysRepository.FindArmedEscortGenericById(id);
            if (master == null)
            {
                return new ApiResponse(404);
            }
            var TransferDTO = _mapper.Map<ArmedEscortDTSDetailGenericDaysTransferDTO>(master);
            return new ApiOkResponse(TransferDTO);
        }

        public async Task<ApiResponse> GetArmedEscortGenericByMasterId(long id)
        {
            var master = await _dTSDetailGenericDaysRepository.FindArmedEscortGenericByMasterId(id);
            if (master == null)
            {
                return new ApiResponse(404);
            }
            var TransferDTO = _mapper.Map<IEnumerable<ArmedEscortDTSDetailGenericDaysTransferDTO>>(master);
            return new ApiOkResponse(TransferDTO);
        }

        public async Task<ApiResponse> GetCommanderGenericById(long id)
        {
            var master = await _dTSDetailGenericDaysRepository.FindCommanderGenericById(id);
            if (master == null)
            {
                return new ApiResponse(404);
            }
            var TransferDTO = _mapper.Map<CommanderDTSDetailGenericDaysTransferDTO>(master);
            return new ApiOkResponse(TransferDTO);
        }

        public async Task<ApiResponse> GetCommanderGenericByMasterId(long id)
        {
            var master = await _dTSDetailGenericDaysRepository.FindCommanderGenericByMasterId(id);
            if (master == null)
            {
                return new ApiResponse(404);
            }
           
            var TransferDTO = _mapper.Map<IEnumerable<CommanderDTSDetailGenericDaysTransferDTO>>(master);
            return new ApiOkResponse(TransferDTO);
        }

        public async Task<ApiResponse> GetPilotGenericById(long id)
        {
            var master = await _dTSDetailGenericDaysRepository.FindPilotGenericById(id);
            if (master == null)
            {
                return new ApiResponse(404);
            }
            var TransferDTO = _mapper.Map<PilotDTSDetailGenericDaysTransferDTO>(master);
            return new ApiOkResponse(TransferDTO);
        }

        public async Task<ApiResponse> GetPilotGenericByMasterId(long id)
        {
            var master = await _dTSDetailGenericDaysRepository.FindPilotGenericByMasterId(id);
            if (master == null)
            {
                return new ApiResponse(404);
            }
            var TransferDTO = _mapper.Map<IEnumerable<PilotDTSDetailGenericDaysTransferDTO>>(master);
            return new ApiOkResponse(TransferDTO);
        }

        public async Task<ApiResponse> GetVehicleGenericById(long id)
        {
            var master = await _dTSDetailGenericDaysRepository.FindVehicleGenericById(id);
            if (master == null)
            {
                return new ApiResponse(404);
            }
            var TransferDTO = _mapper.Map<VehicleDTSDetailGenericDaysTransferDTO>(master);
            return new ApiOkResponse(TransferDTO);
        }

        public async Task<ApiResponse> GetVehicleGenericByMasterId(long id)
        {
            var master = await _dTSDetailGenericDaysRepository.FindVehicleGenericByMasterId(id);
            if (master == null)
            {
                return new ApiResponse(404);
            }
            var TransferDTO = _mapper.Map<IEnumerable<VehicleDTSDetailGenericDaysTransferDTO>>(master);
            return new ApiOkResponse(TransferDTO);
        }

        public async Task<ApiResponse> UpdateArmedEscortGeneric(HttpContext context, long id, ArmedEscortDTSDetailGenericDaysReceivingDTO armedEscortReceivingDTO)
        {
            var itemToUpdate = await _dTSDetailGenericDaysRepository.FindArmedEscortGenericById(id);
            if (itemToUpdate == null)
            {
                return new ApiResponse(404);
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
                return new ApiResponse(500);
            }

            var TransferDTOs = _mapper.Map<ArmedEscortDTSDetailGenericDaysTransferDTO>(updateMaster);
            return new ApiOkResponse(TransferDTOs);
        }

        public async Task<ApiResponse> UpdateCommanderGeneric(HttpContext context, long id, CommanderDTSDetailGenericDaysReceivingDTO commanderReceivingDTO)
        {
            var itemToUpdate = await _dTSDetailGenericDaysRepository.FindCommanderGenericById(id);
            if (itemToUpdate == null)
            {
                return new ApiResponse(404);
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
                return new ApiResponse(500);
            }

            var TransferDTOs = _mapper.Map<ArmedEscortDTSDetailGenericDaysTransferDTO>(updateMaster);
            return new ApiOkResponse(TransferDTOs);
        }

        public async Task<ApiResponse> UpdatePilotGeneric(HttpContext context, long id, PilotDTSDetailGenericDaysReceivingDTO pilotReceivingDTO)
        {
            var itemToUpdate = await _dTSDetailGenericDaysRepository.FindPilotGenericById(id);
            if (itemToUpdate == null)
            {
                return new ApiResponse(404);
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
                return new ApiResponse(500);
            }

            var TransferDTOs = _mapper.Map<ArmedEscortDTSDetailGenericDaysTransferDTO>(updateMaster);
            return new ApiOkResponse(TransferDTOs);
        }

        public async Task<ApiResponse> UpdateVehicleGeneric(HttpContext context, long id, VehicleDTSDetailGenericDaysReceivingDTO vehicleReceivingDTO)
        {
            var itemToUpdate = await _dTSDetailGenericDaysRepository.FindVehicleGenericById(id);
            if (itemToUpdate == null)
            {
                return new ApiResponse(404);
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
                return new ApiResponse(500);
            }

            var TransferDTOs = _mapper.Map<ArmedEscortDTSDetailGenericDaysTransferDTO>(updateMaster);
            return new ApiOkResponse(TransferDTOs);
        }
    }
}
