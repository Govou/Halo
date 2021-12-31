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
    public class CommanderRegistrationServiceImpl:ICommanderRegistrationService
    {
        private readonly ICommanderRegistrationRepository _commanderRepository;
        private readonly IMapper _mapper;

        public CommanderRegistrationServiceImpl(IMapper mapper, ICommanderRegistrationRepository commanderRepository)
        {
            _mapper = mapper;
            _commanderRepository = commanderRepository;
        }

        public async Task<ApiCommonResponse> AddCommander(HttpContext context, CommanderProfileReceivingDTO commanderReceivingDTO)
        {
            var commander = _mapper.Map<CommanderProfile>(commanderReceivingDTO);
            var IdExist = _commanderRepository.FindCommanderUserProfileById(commanderReceivingDTO.ProfileId);
            if (IdExist != null)
            {
                return CommonResponse.Send(ResponseCodes.NO_DATA_AVAILABLE, null, "No record exists");
            }

            commander.CreatedById = context.GetLoggedInUserId();
            commander.CreatedAt = DateTime.UtcNow;
            var savedType = await _commanderRepository.SaveCommander(commander);
            if (savedType == null)
            {
                return CommonResponse.Send(ResponseCodes.FAILURE, null, "Some system errors occurred");
            }
            var typeTransferDTO = _mapper.Map<CommanderProfileTransferDTO>(commander);
            return CommonResponse.Send(ResponseCodes.SUCCESS,typeTransferDTO);
        }

        public async Task<ApiCommonResponse> AddCommanderTie(HttpContext context, CommanderSMORoutesResourceTieReceivingDTO commanderReceivingDTO)
        {
            var commander = new CommanderSMORoutesResourceTie();

            for (int i = 0; i < commanderReceivingDTO.SMORouteId.Length; i++)
            {
                commander.Id = 0;
                commander.SMORegionId = commanderReceivingDTO.SMORegionId;
                commander.ResourceId = commanderReceivingDTO.ResourceId;
                commander.SMORouteId = commanderReceivingDTO.SMORouteId[i];
                var IdExist = _commanderRepository.GetResourceRegIdRegionAndRouteId(commanderReceivingDTO.ResourceId, commanderReceivingDTO.SMORouteId[i], commanderReceivingDTO.SMORegionId);
                if (IdExist == null)
                {
                    commander.CreatedById = context.GetLoggedInUserId();
                    commander.CreatedAt = DateTime.UtcNow;

                    var savedType = await _commanderRepository.SaveCommanderTie(commander);
                    if (savedType == null)
                    {
                        return CommonResponse.Send(ResponseCodes.FAILURE, null, "Some system errors occurred");
                    }
                    //return                 return CommonResponse.Send(ResponseCodes.NO_DATA_AVAILABLE,null, "No record exists");;
                }

            }
            return CommonResponse.Send(ResponseCodes.SUCCESS,"Record(s) Added");
        }

        public async Task<ApiCommonResponse> DeleteCommander(long id)
        {
            var itemToDelete = await _commanderRepository.FindCommanderById(id);

            if (itemToDelete == null)
            {
                return CommonResponse.Send(ResponseCodes.NO_DATA_AVAILABLE);;
            }

            if (!await _commanderRepository.DeleteCommander(itemToDelete))
            {
                return CommonResponse.Send(ResponseCodes.FAILURE, null, "Some system errors occurred");
            }

            return CommonResponse.Send(ResponseCodes.SUCCESS);
        }

        public async Task<ApiCommonResponse> DeleteCommanderTie(long id)
        {
            var itemToDelete = await _commanderRepository.FindCommanderTieById(id);

            if (itemToDelete == null)
            {
                return CommonResponse.Send(ResponseCodes.NO_DATA_AVAILABLE);;
            }

            if (!await _commanderRepository.DeleteCommanderTie(itemToDelete))
            {
                return CommonResponse.Send(ResponseCodes.FAILURE, null, "Some system errors occurred");
            }

            return CommonResponse.Send(ResponseCodes.SUCCESS);
        }

        public async Task<ApiCommonResponse> GetAllCommanders()
        {
            var commanders = await _commanderRepository.FindAllCommanders();
            if (commanders == null)
            {
                return CommonResponse.Send(ResponseCodes.NO_DATA_AVAILABLE);;
            }
            var TransferDTO = _mapper.Map<IEnumerable<CommanderProfileTransferDTO>>(commanders);
            return CommonResponse.Send(ResponseCodes.SUCCESS,TransferDTO);
        }

        public async Task<ApiCommonResponse> GetAllCommanderTies()
        {
            var commanders = await _commanderRepository.FindAllCommanderTies();
            if (commanders == null)
            {
                return CommonResponse.Send(ResponseCodes.NO_DATA_AVAILABLE);;
            }
            var TransferDTO = _mapper.Map<IEnumerable<CommanderSMORoutesResourceTieTransferDTO>>(commanders);
            return CommonResponse.Send(ResponseCodes.SUCCESS,TransferDTO);
        }

        public async Task<ApiCommonResponse> GetCommanderById(long id)
        {
            var commander = await _commanderRepository.FindCommanderById(id);
            if (commander == null)
            {
                return CommonResponse.Send(ResponseCodes.NO_DATA_AVAILABLE);;
            }
            var TransferDTO = _mapper.Map<CommanderProfileTransferDTO>(commander);
            return CommonResponse.Send(ResponseCodes.SUCCESS,TransferDTO);
        }

        public async Task<ApiCommonResponse> GetCommanderTieById(long id)
        {
            var commander = await _commanderRepository.FindCommanderTieById(id);
            if (commander == null)
            {
                return CommonResponse.Send(ResponseCodes.NO_DATA_AVAILABLE);;
            }
            var TransferDTO = _mapper.Map<CommanderSMORoutesResourceTieTransferDTO>(commander);
            return CommonResponse.Send(ResponseCodes.SUCCESS,TransferDTO);
        }

        public async Task<ApiCommonResponse> UpdateCommander(HttpContext context, long id, CommanderProfileReceivingDTO commanderReceivingDTO)
        {
            var itemToUpdate = await _commanderRepository.FindCommanderById(id);
            if (itemToUpdate == null)
            {
                return CommonResponse.Send(ResponseCodes.NO_DATA_AVAILABLE);;
            }

            var summary = $"Initial details before change, \n {itemToUpdate.ToString()} \n";

            itemToUpdate.AttachedBranchId = commanderReceivingDTO.AttachedBranchId;
            itemToUpdate.AttachedOfficeId = commanderReceivingDTO.AttachedOfficeId;
            itemToUpdate.CommanderTypeId = commanderReceivingDTO.CommanderTypeId;
            itemToUpdate.RankId = commanderReceivingDTO.RankId;
            
            itemToUpdate.UpdatedAt = DateTime.UtcNow;
            //regionToUpdate.BranchId = regionReceivingDTO.BranchId;
            var updatedRank = await _commanderRepository.UpdateCommander(itemToUpdate);

            summary += $"Details after change, \n {updatedRank.ToString()} \n";

            if (updatedRank == null)
            {
                return CommonResponse.Send(ResponseCodes.FAILURE, null, "Some system errors occurred");
            }

            var rankTransferDTOs = _mapper.Map<CommanderProfileTransferDTO>(updatedRank);
            return CommonResponse.Send(ResponseCodes.SUCCESS,rankTransferDTOs);
        }
    }
}
