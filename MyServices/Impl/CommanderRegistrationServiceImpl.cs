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
            var IdExist = _commanderRepository.FindCommanderById(commanderReceivingDTO.ProfileId);
            if (IdExist != null)
            {
                return new ApiResponse(409);
            }

            commander.CreatedById = context.GetLoggedInUserId();
            //cType.IsDeleted = false;
            commander.CreatedAt = DateTime.UtcNow;
            var savedType = await _commanderRepository.SaveCommander(commander);
            if (savedType == null)
            {
                return new ApiResponse(500);
            }
            var typeTransferDTO = _mapper.Map<CommanderProfileTransferDTO>(commander);
            return new ApiOkResponse(typeTransferDTO);
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
