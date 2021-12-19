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
    public class DTSMastersServiceImpl:IDTSMastersService
    {
        private readonly IDTSMastersRepository _dTSMastersRepository;
        private readonly IMapper _mapper;

        public DTSMastersServiceImpl(IMapper mapper, IDTSMastersRepository dTSMastersRepository)
        {
            _mapper = mapper;
            _dTSMastersRepository = dTSMastersRepository;
        }

        public async Task<ApiResponse> AddArmedEscortMaster(HttpContext context, ArmedEscortDTSMastersReceivingDTO armedEscortReceivingDTO)
        {
            var armedescort = _mapper.Map<ArmedEscortDTSMaster>(armedEscortReceivingDTO);
            //var NameExist = _armedEscortsRepository.GetTypename(armedEscortReceivingDTO.Name);
            //if (NameExist != null)
            //{
            //    return new ApiResponse(409);
            //}
            armedescort.CreatedById = context.GetLoggedInUserId();
            armedescort.CreatedAt = DateTime.UtcNow;
            var save = await _dTSMastersRepository.SaveArmedEscortMaster(armedescort);
            if (save == null)
            {
                return new ApiResponse(500);
            }
            var TransferDTO = _mapper.Map<ArmedEscortDTSMastersTransferDTO>(armedescort);
            return new ApiOkResponse(TransferDTO);
        }

        public async Task<ApiResponse> AddCommanderMaster(HttpContext context, CommanderDTSMastersReceivingDTO commanderReceivingDTO)
        {
            var commander = _mapper.Map<CommanderDTSMaster>(commanderReceivingDTO);
            commander.CreatedById = context.GetLoggedInUserId();
            commander.CreatedAt = DateTime.UtcNow;
            var save = await _dTSMastersRepository.SaveCommanderMaster(commander);
            if (save == null)
            {
                return new ApiResponse(500);
            }
            var TransferDTO = _mapper.Map<CommanderDTSMastersTransferDTO>(commander);
            return new ApiOkResponse(TransferDTO);
        }

        public async Task<ApiResponse> AddPilotMaster(HttpContext context, PilotDTSMastersReceivingDTO pilotReceivingDTO)
        {
            var pilot = _mapper.Map<PilotDTSMaster>(pilotReceivingDTO);
            pilot.CreatedById = context.GetLoggedInUserId();
            pilot.CreatedAt = DateTime.UtcNow;
            var save = await _dTSMastersRepository.SavePilotMaster(pilot);
            if (save == null)
            {
                return new ApiResponse(500);
            }
            var TransferDTO = _mapper.Map<PilotDTSMastersTransferDTO>(pilot);
            return new ApiOkResponse(TransferDTO);
        }

        public async Task<ApiResponse> AddVehicleMaster(HttpContext context, VehicleDTSMastersReceivingDTO vehicleReceivingDTO)
        {
            var vehicle = _mapper.Map<VehicleDTSMaster>(vehicleReceivingDTO);
            vehicle.CreatedById = context.GetLoggedInUserId();
            vehicle.CreatedAt = DateTime.UtcNow;
            var save = await _dTSMastersRepository.SaveVehicleMaster(vehicle);
            if (save == null)
            {
                return new ApiResponse(500);
            }
            var TransferDTO = _mapper.Map<VehicleDTSMastersTransferDTO>(vehicle);
            return new ApiOkResponse(TransferDTO);
        }

        public async Task<ApiResponse> DeleteArmedEscortMaster(long id)
        {
            var itemToDelete = await _dTSMastersRepository.FindArmedEscortMasterById(id);

            if (itemToDelete == null)
            {
                return new ApiResponse(404);
            }

            if (!await _dTSMastersRepository.DeleteArmedEscortMaster(itemToDelete))
            {
                return new ApiResponse(500);
            }

            return new ApiOkResponse(true);
        }

        public async Task<ApiResponse> DeleteCommanderMaster(long id)
        {
            var itemToDelete = await _dTSMastersRepository.FindCommanderMasterById(id);

            if (itemToDelete == null)
            {
                return new ApiResponse(404);
            }

            if (!await _dTSMastersRepository.DeleteCommanderMaster(itemToDelete))
            {
                return new ApiResponse(500);
            }

            return new ApiOkResponse(true);
        }

        public async Task<ApiResponse> DeletePilotMaster(long id)
        {
            var itemToDelete = await _dTSMastersRepository.FindPilotMasterById(id);

            if (itemToDelete == null)
            {
                return new ApiResponse(404);
            }

            if (!await _dTSMastersRepository.DeletePilotMaster(itemToDelete))
            {
                return new ApiResponse(500);
            }

            return new ApiOkResponse(true);
        }

        public async Task<ApiResponse> DeleteVehicleMaster(long id)
        {
            var itemToDelete = await _dTSMastersRepository.FindVehicleMasterById(id);

            if (itemToDelete == null)
            {
                return new ApiResponse(404);
            }

            if (!await _dTSMastersRepository.DeleteVehicleMaster(itemToDelete))
            {
                return new ApiResponse(500);
            }

            return new ApiOkResponse(true);
        }

        public async Task<ApiResponse> GetAllArmedEscortMasters()
        {
            var masters = await _dTSMastersRepository.FindAllArmedEscortMasters();
            if (masters == null)
            {
                return new ApiResponse(404);
            }
            var TransferDTO = _mapper.Map<IEnumerable<ArmedEscortDTSMastersTransferDTO>>(masters);
            return new ApiOkResponse(TransferDTO);
        }

        public async Task<ApiResponse> GetAllCommanderMasters()
        {
            var masters = await _dTSMastersRepository.FindAllCommanderMasters();
            if (masters == null)
            {
                return new ApiResponse(404);
            }
            var TransferDTO = _mapper.Map<IEnumerable<CommanderDTSMastersTransferDTO>>(masters);
            return new ApiOkResponse(TransferDTO);
        }

        public async Task<ApiResponse> GetAllPilotMasters()
        {
            var masters = await _dTSMastersRepository.FindAllPilotMasters();
            if (masters == null)
            {
                return new ApiResponse(404);
            }
            var TransferDTO = _mapper.Map<IEnumerable<PilotDTSMastersTransferDTO>>(masters);
            return new ApiOkResponse(TransferDTO);
        }

        public async Task<ApiResponse> GetAllVehicleMasters()
        {
            var masters = await _dTSMastersRepository.FindAllVehicleMasters();
            if (masters == null)
            {
                return new ApiResponse(404);
            }
            var TransferDTO = _mapper.Map<IEnumerable<VehicleDTSMastersTransferDTO>>(masters);
            return new ApiOkResponse(TransferDTO);
        }

        public async Task<ApiResponse> GetArmedEscortMasterById(long id)
        {
            var master = await _dTSMastersRepository.FindArmedEscortMasterById(id);
            if (master == null)
            {
                return new ApiResponse(404);
            }
            var TransferDTO = _mapper.Map<ArmedEscortDTSMastersTransferDTO>(master);
            return new ApiOkResponse(TransferDTO);
        }

        public async Task<ApiResponse> GetCommanderMasterById(long id)
        {
            var master = await _dTSMastersRepository.FindCommanderMasterById(id);
            if (master == null)
            {
                return new ApiResponse(404);
            }
            var TransferDTO = _mapper.Map<CommanderDTSMastersTransferDTO>(master);
            return new ApiOkResponse(TransferDTO);
        }

        public async Task<ApiResponse> GetPilotMasterById(long id)
        {
            var master = await _dTSMastersRepository.FindPilotMasterById(id);
            if (master == null)
            {
                return new ApiResponse(404);
            }
            var TransferDTO = _mapper.Map<PilotDTSMastersTransferDTO>(master);
            return new ApiOkResponse(TransferDTO);
        }

        public async Task<ApiResponse> GetVehicleMasterById(long id)
        {
            var master = await _dTSMastersRepository.FindVehicleMasterById(id);
            if (master == null)
            {
                return new ApiResponse(404);
            }
            var TransferDTO = _mapper.Map<VehicleDTSMastersTransferDTO>(master);
            return new ApiOkResponse(TransferDTO);
        }

        public async Task<ApiResponse> UpdateArmedEscortMaster(HttpContext context, long id, ArmedEscortDTSMastersReceivingDTO armedEscortReceivingDTO)
        {
            var itemToUpdate = await _dTSMastersRepository.FindArmedEscortMasterById(id);
            if (itemToUpdate == null)
            {
                return new ApiResponse(404);
            }

            var summary = $"Initial details before change, \n {itemToUpdate.ToString()} \n";

            itemToUpdate.AvailabilityStart = armedEscortReceivingDTO.AvailabilityStart;
            itemToUpdate.AvailablilityEnd = armedEscortReceivingDTO.AvailablilityEnd;
            itemToUpdate.Caption = armedEscortReceivingDTO.Caption;
            //itemToUpdate.RankName = armedEscortRankReceivingDTO.RankName;
            itemToUpdate.UpdatedAt = DateTime.UtcNow;
            var updateMaster = await _dTSMastersRepository.UpdateArmedEscortMaster(itemToUpdate);

            summary += $"Details after change, \n {updateMaster.ToString()} \n";

            if (updateMaster == null)
            {
                return new ApiResponse(500);
            }

            var TransferDTOs = _mapper.Map<ArmedEscortDTSMastersTransferDTO>(updateMaster);
            return new ApiOkResponse(TransferDTOs);
        }

        public async Task<ApiResponse> UpdateCommanderMaster(HttpContext context, long id, CommanderDTSMastersReceivingDTO commanderReceivingDTO)
        {
            var itemToUpdate = await _dTSMastersRepository.FindCommanderMasterById(id);
            if (itemToUpdate == null)
            {
                return new ApiResponse(404);
            }

            var summary = $"Initial details before change, \n {itemToUpdate.ToString()} \n";

            itemToUpdate.AvailabilityStart = commanderReceivingDTO.AvailabilityStart;
            itemToUpdate.AvailablilityEnd = commanderReceivingDTO.AvailablilityEnd;
            itemToUpdate.Caption = commanderReceivingDTO.Caption;
            //itemToUpdate.RankName = armedEscortRankReceivingDTO.RankName;
            itemToUpdate.UpdatedAt = DateTime.UtcNow;
            var updateMaster = await _dTSMastersRepository.UpdateCommanderMaster(itemToUpdate);

            summary += $"Details after change, \n {updateMaster.ToString()} \n";

            if (updateMaster == null)
            {
                return new ApiResponse(500);
            }

            var TransferDTOs = _mapper.Map<CommanderDTSMastersTransferDTO>(updateMaster);
            return new ApiOkResponse(TransferDTOs);
        }

        public async Task<ApiResponse> UpdatePilotMaster(HttpContext context, long id, PilotDTSMastersReceivingDTO pilotReceivingDTO)
        {
            var itemToUpdate = await _dTSMastersRepository.FindPilotMasterById(id);
            if (itemToUpdate == null)
            {
                return new ApiResponse(404);
            }

            var summary = $"Initial details before change, \n {itemToUpdate.ToString()} \n";

            itemToUpdate.AvailabilityStart = pilotReceivingDTO.AvailabilityStart;
            itemToUpdate.AvailablilityEnd = pilotReceivingDTO.AvailablilityEnd;
            itemToUpdate.Caption = pilotReceivingDTO.Caption;
            //itemToUpdate.RankName = armedEscortRankReceivingDTO.RankName;
            itemToUpdate.UpdatedAt = DateTime.UtcNow;
            var updateMaster = await _dTSMastersRepository.UpdatePilotMaster(itemToUpdate);

            summary += $"Details after change, \n {updateMaster.ToString()} \n";

            if (updateMaster == null)
            {
                return new ApiResponse(500);
            }

            var TransferDTOs = _mapper.Map<PilotDTSMastersTransferDTO>(updateMaster);
            return new ApiOkResponse(TransferDTOs);
        }

        public async Task<ApiResponse> UpdateVehicleMaster(HttpContext context, long id, VehicleDTSMastersReceivingDTO vehicleReceivingDTO)
        {
            var itemToUpdate = await _dTSMastersRepository.FindVehicleMasterById(id);
            if (itemToUpdate == null)
            {
                return new ApiResponse(404);
            }

            var summary = $"Initial details before change, \n {itemToUpdate.ToString()} \n";

            itemToUpdate.AvailabilityStart = vehicleReceivingDTO.AvailabilityStart;
            itemToUpdate.AvailablilityEnd = vehicleReceivingDTO.AvailablilityEnd;
            itemToUpdate.Caption = vehicleReceivingDTO.Caption;
            //itemToUpdate.RankName = armedEscortRankReceivingDTO.RankName;
            itemToUpdate.UpdatedAt = DateTime.UtcNow;
            var updateMaster = await _dTSMastersRepository.UpdatevehicleMaster(itemToUpdate);

            summary += $"Details after change, \n {updateMaster.ToString()} \n";

            if (updateMaster == null)
            {
                return new ApiResponse(500);
            }

            var TransferDTOs = _mapper.Map<VehicleDTSMastersTransferDTO>(updateMaster);
            return new ApiOkResponse(TransferDTOs);
        }
    }
}
