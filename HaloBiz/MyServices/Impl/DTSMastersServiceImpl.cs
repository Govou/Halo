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
    public class DTSMastersServiceImpl:IDTSMastersService
    {
        private readonly IDTSMastersRepository _dTSMastersRepository;
        private readonly IMapper _mapper;

        public DTSMastersServiceImpl(IMapper mapper, IDTSMastersRepository dTSMastersRepository)
        {
            _mapper = mapper;
            _dTSMastersRepository = dTSMastersRepository;
        }

        public async Task<ApiCommonResponse> AddArmedEscortMaster(HttpContext context, ArmedEscortDTSMastersReceivingDTO armedEscortReceivingDTO)
        {
            var armedescort = _mapper.Map<ArmedEscortDTSMaster>(armedEscortReceivingDTO);
            var profileIdExist = _dTSMastersRepository.GetArmedEscortProfileId(armedEscortReceivingDTO.ArmedEscortResourceId);
            if (armedEscortReceivingDTO.AvailablilityEnd < armedEscortReceivingDTO.AvailabilityStart || armedEscortReceivingDTO.AvailablilityEnd == armedEscortReceivingDTO.AvailabilityStart)
            {
                return CommonResponse.Send(ResponseCodes.FAILURE);
            }
            if (profileIdExist != null)
            {
                if (armedEscortReceivingDTO.AvailabilityStart < profileIdExist.AvailablilityEnd)
                {
                    return CommonResponse.Send(ResponseCodes.FAILURE);
                }
                    
            }

            armedescort.CreatedById = context.GetLoggedInUserId();
            armedescort.CreatedAt = DateTime.UtcNow;
            var save = await _dTSMastersRepository.SaveArmedEscortMaster(armedescort);
            if (save == null)
            {
                return CommonResponse.Send(ResponseCodes.FAILURE, null, "Some system errors occurred");
            }
            var TransferDTO = _mapper.Map<ArmedEscortDTSMastersTransferDTO>(armedescort);
            return CommonResponse.Send(ResponseCodes.SUCCESS,TransferDTO);
        }

        public async Task<ApiCommonResponse> AddCommanderMaster(HttpContext context, CommanderDTSMastersReceivingDTO commanderReceivingDTO)
        {
            var commander = _mapper.Map<CommanderDTSMaster>(commanderReceivingDTO);
            var profileIdExist = _dTSMastersRepository.GetCommanderProfileId(commanderReceivingDTO.CommanderResourceId);
            if (commanderReceivingDTO.AvailablilityEnd < commanderReceivingDTO.AvailabilityStart || commanderReceivingDTO.AvailablilityEnd == commanderReceivingDTO.AvailabilityStart)
            {
                return CommonResponse.Send(ResponseCodes.FAILURE);
            }
            if (profileIdExist != null)
            {
                if (commanderReceivingDTO.AvailabilityStart < profileIdExist.AvailablilityEnd)
                {
                    return CommonResponse.Send(ResponseCodes.FAILURE);
                }

            }
            commander.CreatedById = context.GetLoggedInUserId();
            commander.CreatedAt = DateTime.UtcNow;
            var save = await _dTSMastersRepository.SaveCommanderMaster(commander);
            if (save == null)
            {
                return CommonResponse.Send(ResponseCodes.FAILURE, null, "Some system errors occurred");
            }
            var TransferDTO = _mapper.Map<CommanderDTSMastersTransferDTO>(commander);
            return CommonResponse.Send(ResponseCodes.SUCCESS,TransferDTO);
        }

        public async Task<ApiCommonResponse> AddPilotMaster(HttpContext context, PilotDTSMastersReceivingDTO pilotReceivingDTO)
        {
            var pilot = _mapper.Map<PilotDTSMaster>(pilotReceivingDTO);
            var profileIdExist = _dTSMastersRepository.GetPilotProfileId(pilotReceivingDTO.PilotResourceId);
            if (pilotReceivingDTO.AvailablilityEnd < pilotReceivingDTO.AvailabilityStart || pilotReceivingDTO.AvailablilityEnd == pilotReceivingDTO.AvailabilityStart)
            {
                return CommonResponse.Send(ResponseCodes.FAILURE);
            }
            if (profileIdExist != null)
            {
                if (pilotReceivingDTO.AvailabilityStart < profileIdExist.AvailablilityEnd)
                {
                    return CommonResponse.Send(ResponseCodes.FAILURE);
                }

            }
            pilot.CreatedById = context.GetLoggedInUserId();
            pilot.CreatedAt = DateTime.UtcNow;
            var save = await _dTSMastersRepository.SavePilotMaster(pilot);
            if (save == null)
            {
                return CommonResponse.Send(ResponseCodes.FAILURE, null, "Some system errors occurred");
            }
            var TransferDTO = _mapper.Map<PilotDTSMastersTransferDTO>(pilot);
            return CommonResponse.Send(ResponseCodes.SUCCESS,TransferDTO);
        }

        public async Task<ApiCommonResponse> AddVehicleMaster(HttpContext context, VehicleDTSMastersReceivingDTO vehicleReceivingDTO)
        {
            var vehicle = _mapper.Map<VehicleDTSMaster>(vehicleReceivingDTO);
            var profileIdExist = _dTSMastersRepository.GetVehicleProfileId(vehicleReceivingDTO.VehicleResourceId);
            if (vehicleReceivingDTO.AvailablilityEnd < vehicleReceivingDTO.AvailabilityStart || vehicleReceivingDTO.AvailablilityEnd == vehicleReceivingDTO.AvailabilityStart)
            {
                return CommonResponse.Send(ResponseCodes.FAILURE);
            }
            if (profileIdExist != null)
            {
                if (vehicleReceivingDTO.AvailabilityStart < profileIdExist.AvailablilityEnd)
                {
                    return CommonResponse.Send(ResponseCodes.FAILURE);
                }

            }
            vehicle.CreatedById = context.GetLoggedInUserId();
            vehicle.CreatedAt = DateTime.UtcNow;
            var save = await _dTSMastersRepository.SaveVehicleMaster(vehicle);
            if (save == null)
            {
                return CommonResponse.Send(ResponseCodes.FAILURE, null, "Some system errors occurred");
            }
            var TransferDTO = _mapper.Map<VehicleDTSMastersTransferDTO>(vehicle);
            return CommonResponse.Send(ResponseCodes.SUCCESS,TransferDTO);
        }

        public async Task<ApiCommonResponse> DeleteArmedEscortMaster(long id)
        {
            var itemToDelete = await _dTSMastersRepository.FindArmedEscortMasterById(id);

            if (itemToDelete == null)
            {
                return CommonResponse.Send(ResponseCodes.NO_DATA_AVAILABLE);;
            }

            if (!await _dTSMastersRepository.DeleteArmedEscortMaster(itemToDelete))
            {
                return CommonResponse.Send(ResponseCodes.FAILURE, null, "Some system errors occurred");
            }

            return CommonResponse.Send(ResponseCodes.SUCCESS);
        }

        public async Task<ApiCommonResponse> DeleteCommanderMaster(long id)
        {
            var itemToDelete = await _dTSMastersRepository.FindCommanderMasterById(id);

            if (itemToDelete == null)
            {
                return CommonResponse.Send(ResponseCodes.NO_DATA_AVAILABLE);;
            }

            if (!await _dTSMastersRepository.DeleteCommanderMaster(itemToDelete))
            {
                return CommonResponse.Send(ResponseCodes.FAILURE, null, "Some system errors occurred");
            }

            return CommonResponse.Send(ResponseCodes.SUCCESS);
        }

        public async Task<ApiCommonResponse> DeletePilotMaster(long id)
        {
            var itemToDelete = await _dTSMastersRepository.FindPilotMasterById(id);

            if (itemToDelete == null)
            {
                return CommonResponse.Send(ResponseCodes.NO_DATA_AVAILABLE);;
            }

            if (!await _dTSMastersRepository.DeletePilotMaster(itemToDelete))
            {
                return CommonResponse.Send(ResponseCodes.FAILURE, null, "Some system errors occurred");
            }

            return CommonResponse.Send(ResponseCodes.SUCCESS);
        }

        public async Task<ApiCommonResponse> DeleteVehicleMaster(long id)
        {
            var itemToDelete = await _dTSMastersRepository.FindVehicleMasterById(id);

            if (itemToDelete == null)
            {
                return CommonResponse.Send(ResponseCodes.NO_DATA_AVAILABLE);;
            }

            if (!await _dTSMastersRepository.DeleteVehicleMaster(itemToDelete))
            {
                return CommonResponse.Send(ResponseCodes.FAILURE, null, "Some system errors occurred");
            }

            return CommonResponse.Send(ResponseCodes.SUCCESS);
        }

        public async Task<ApiCommonResponse> GetAllArmedEscortMasters()
        {
            var masters = await _dTSMastersRepository.FindAllArmedEscortMasters();
            if (masters == null)
            {
                return CommonResponse.Send(ResponseCodes.NO_DATA_AVAILABLE);;
            }
            var TransferDTO = _mapper.Map<IEnumerable<ArmedEscortDTSMastersTransferDTO>>(masters);
            return CommonResponse.Send(ResponseCodes.SUCCESS,TransferDTO);
        }

        public async Task<ApiCommonResponse> GetAllCommanderMasters()
        {
            var masters = await _dTSMastersRepository.FindAllCommanderMasters();
            if (masters == null)
            {
                return CommonResponse.Send(ResponseCodes.NO_DATA_AVAILABLE);;
            }
            var TransferDTO = _mapper.Map<IEnumerable<CommanderDTSMastersTransferDTO>>(masters);
            return CommonResponse.Send(ResponseCodes.SUCCESS,TransferDTO);
        }

        public async Task<ApiCommonResponse> GetAllPilotMasters()
        {
            var masters = await _dTSMastersRepository.FindAllPilotMasters();
            if (masters == null)
            {
                return CommonResponse.Send(ResponseCodes.NO_DATA_AVAILABLE);;
            }
            var TransferDTO = _mapper.Map<IEnumerable<PilotDTSMastersTransferDTO>>(masters);
            return CommonResponse.Send(ResponseCodes.SUCCESS,TransferDTO);
        }

        public async Task<ApiCommonResponse> GetAllVehicleMasters()
        {
            var masters = await _dTSMastersRepository.FindAllVehicleMasters();
            if (masters == null)
            {
                return CommonResponse.Send(ResponseCodes.NO_DATA_AVAILABLE);;
            }
            var TransferDTO = _mapper.Map<IEnumerable<VehicleDTSMastersTransferDTO>>(masters);
            return CommonResponse.Send(ResponseCodes.SUCCESS,TransferDTO);
        }

        public async Task<ApiCommonResponse> GetArmedEscortMasterById(long id)
        {
            var master = await _dTSMastersRepository.FindArmedEscortMasterById(id);
            if (master == null)
            {
                return CommonResponse.Send(ResponseCodes.NO_DATA_AVAILABLE);;
            }
            var TransferDTO = _mapper.Map<ArmedEscortDTSMastersTransferDTO>(master);
            return CommonResponse.Send(ResponseCodes.SUCCESS,TransferDTO);
        }

        public async Task<ApiCommonResponse> GetCommanderMasterById(long id)
        {
            var master = await _dTSMastersRepository.FindCommanderMasterById(id);
            if (master == null)
            {
                return CommonResponse.Send(ResponseCodes.NO_DATA_AVAILABLE);;
            }
            var TransferDTO = _mapper.Map<CommanderDTSMastersTransferDTO>(master);
            return CommonResponse.Send(ResponseCodes.SUCCESS,TransferDTO);
        }

        public async Task<ApiCommonResponse> GetPilotMasterById(long id)
        {
            var master = await _dTSMastersRepository.FindPilotMasterById(id);
            if (master == null)
            {
                return CommonResponse.Send(ResponseCodes.NO_DATA_AVAILABLE);;
            }
            var TransferDTO = _mapper.Map<PilotDTSMastersTransferDTO>(master);
            return CommonResponse.Send(ResponseCodes.SUCCESS,TransferDTO);
        }

        public async Task<ApiCommonResponse> GetVehicleMasterById(long id)
        {
            var master = await _dTSMastersRepository.FindVehicleMasterById(id);
            if (master == null)
            {
                return CommonResponse.Send(ResponseCodes.NO_DATA_AVAILABLE);;
            }
            var TransferDTO = _mapper.Map<VehicleDTSMastersTransferDTO>(master);
            return CommonResponse.Send(ResponseCodes.SUCCESS,TransferDTO);
        }

        public async Task<ApiCommonResponse> UpdateArmedEscortMaster(HttpContext context, long id, ArmedEscortDTSMastersReceivingDTO armedEscortReceivingDTO)
        {
            var itemToUpdate = await _dTSMastersRepository.FindArmedEscortMasterById(id);
            if (itemToUpdate == null)
            {
                return CommonResponse.Send(ResponseCodes.NO_DATA_AVAILABLE);;
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
                return CommonResponse.Send(ResponseCodes.FAILURE, null, "Some system errors occurred");
            }

            var TransferDTOs = _mapper.Map<ArmedEscortDTSMastersTransferDTO>(updateMaster);
            return CommonResponse.Send(ResponseCodes.SUCCESS,TransferDTOs);
        }

        public async Task<ApiCommonResponse> UpdateCommanderMaster(HttpContext context, long id, CommanderDTSMastersReceivingDTO commanderReceivingDTO)
        {
            var itemToUpdate = await _dTSMastersRepository.FindCommanderMasterById(id);
            if (itemToUpdate == null)
            {
                return CommonResponse.Send(ResponseCodes.NO_DATA_AVAILABLE);;
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
                return CommonResponse.Send(ResponseCodes.FAILURE, null, "Some system errors occurred");
            }

            var TransferDTOs = _mapper.Map<CommanderDTSMastersTransferDTO>(updateMaster);
            return CommonResponse.Send(ResponseCodes.SUCCESS,TransferDTOs);
        }

        public async Task<ApiCommonResponse> UpdatePilotMaster(HttpContext context, long id, PilotDTSMastersReceivingDTO pilotReceivingDTO)
        {
            var itemToUpdate = await _dTSMastersRepository.FindPilotMasterById(id);
            if (itemToUpdate == null)
            {
                return CommonResponse.Send(ResponseCodes.NO_DATA_AVAILABLE);;
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
                return CommonResponse.Send(ResponseCodes.FAILURE, null, "Some system errors occurred");
            }

            var TransferDTOs = _mapper.Map<PilotDTSMastersTransferDTO>(updateMaster);
            return CommonResponse.Send(ResponseCodes.SUCCESS,TransferDTOs);
        }

        public async Task<ApiCommonResponse> UpdateVehicleMaster(HttpContext context, long id, VehicleDTSMastersReceivingDTO vehicleReceivingDTO)
        {
            var itemToUpdate = await _dTSMastersRepository.FindVehicleMasterById(id);
            if (itemToUpdate == null)
            {
                return CommonResponse.Send(ResponseCodes.NO_DATA_AVAILABLE);;
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
                return CommonResponse.Send(ResponseCodes.FAILURE, null, "Some system errors occurred");
            }

            var TransferDTOs = _mapper.Map<VehicleDTSMastersTransferDTO>(updateMaster);
            return CommonResponse.Send(ResponseCodes.SUCCESS,TransferDTOs);
        }
    }
}
