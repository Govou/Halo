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

        public async Task<ApiResponse> AddCommander(HttpContext context, CommanderProfileReceivingDTO commanderReceivingDTO)
        {
            var commander = _mapper.Map<CommanderProfile>(commanderReceivingDTO);
            var IdExist = _commanderRepository.FindCommanderUserProfileById(commanderReceivingDTO.ProfileId);
            if (IdExist != null)
            {
                return new ApiResponse(409);
            }

            commander.CreatedById = context.GetLoggedInUserId();
            commander.CreatedAt = DateTime.UtcNow;
            var savedType = await _commanderRepository.SaveCommander(commander);
            if (savedType == null)
            {
                return new ApiResponse(500);
            }
            var typeTransferDTO = _mapper.Map<CommanderProfileTransferDTO>(commander);
            return new ApiOkResponse(typeTransferDTO);
        }

        public async Task<ApiResponse> AddCommanderTie(HttpContext context, CommanderSMORoutesResourceTieReceivingDTO commanderReceivingDTO)
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
                        return new ApiResponse(500);
                    }
                    //return new ApiResponse(409);
                }

            }
            return new ApiOkResponse("Record(s) Added");
        }

        public async Task<ApiResponse> DeleteCommander(long id)
        {
            var itemToDelete = await _commanderRepository.FindCommanderById(id);

            if (itemToDelete == null)
            {
                return new ApiResponse(404);
            }

            if (!await _commanderRepository.DeleteCommander(itemToDelete))
            {
                return new ApiResponse(500);
            }

            return new ApiOkResponse(true);
        }

        public async Task<ApiResponse> DeleteCommanderTie(long id)
        {
            var itemToDelete = await _commanderRepository.FindCommanderTieById(id);

            if (itemToDelete == null)
            {
                return new ApiResponse(404);
            }

            if (!await _commanderRepository.DeleteCommanderTie(itemToDelete))
            {
                return new ApiResponse(500);
            }

            return new ApiOkResponse(true);
        }

        public async Task<ApiResponse> GetAllCommanders()
        {
            var commanders = await _commanderRepository.FindAllCommanders();
            if (commanders == null)
            {
                return new ApiResponse(404);
            }
            var TransferDTO = _mapper.Map<IEnumerable<CommanderProfileTransferDTO>>(commanders);
            return new ApiOkResponse(TransferDTO);
        }

        public async Task<ApiResponse> GetAllCommanderTies()
        {
            var commanders = await _commanderRepository.FindAllCommanderTies();
            if (commanders == null)
            {
                return new ApiResponse(404);
            }
            var TransferDTO = _mapper.Map<IEnumerable<CommanderSMORoutesResourceTieTransferDTO>>(commanders);
            return new ApiOkResponse(TransferDTO);
        }

        public async Task<ApiResponse> GetAllCommanderTiesByResourceId(long resourceId)
        {
            var commanders = await _commanderRepository.FindAllCommanderTiesByResourceId(resourceId);
            if (commanders == null)
            {
                return new ApiResponse(404);
            }
            var TransferDTO = _mapper.Map<IEnumerable<CommanderSMORoutesResourceTieTransferDTO>>(commanders);
            return new ApiOkResponse(TransferDTO);
        }

        public async Task<ApiResponse> GetCommanderById(long id)
        {
            var commander = await _commanderRepository.FindCommanderById(id);
            if (commander == null)
            {
                return new ApiResponse(404);
            }
            var TransferDTO = _mapper.Map<CommanderProfileTransferDTO>(commander);
            return new ApiOkResponse(TransferDTO);
        }

        public async Task<ApiResponse> GetCommanderTieById(long id)
        {
            var commander = await _commanderRepository.FindCommanderTieById(id);
            if (commander == null)
            {
                return new ApiResponse(404);
            }
            var TransferDTO = _mapper.Map<CommanderSMORoutesResourceTieTransferDTO>(commander);
            return new ApiOkResponse(TransferDTO);
        }

        public async Task<ApiResponse> UpdateCommander(HttpContext context, long id, CommanderProfileReceivingDTO commanderReceivingDTO)
        {
            var itemToUpdate = await _commanderRepository.FindCommanderById(id);
            if (itemToUpdate == null)
            {
                return new ApiResponse(404);
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
                return new ApiResponse(500);
            }

            var rankTransferDTOs = _mapper.Map<CommanderProfileTransferDTO>(updatedRank);
            return new ApiOkResponse(rankTransferDTOs);
        }
    }
}
