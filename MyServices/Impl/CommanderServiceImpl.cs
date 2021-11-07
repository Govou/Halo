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
    public class CommanderServiceImpl:ICommanderService
    {
        private readonly ICommanderRepository _commanderRepository;
        private readonly IMapper _mapper;

        public CommanderServiceImpl( IMapper mapper, ICommanderRepository commanderRepository)
        {
            _mapper = mapper;
            _commanderRepository = commanderRepository;
        }

        public async Task<ApiResponse> AddCommanderRank(HttpContext context, CommanderRankReceivingDTO commanderRankReceivingDTO)
        {
            var cRank = _mapper.Map<CommanderRank>(commanderRankReceivingDTO);
            var getCommanderType = await _commanderRepository.FindCommanderTypeByName(cRank.CommanderType);
            if (getCommanderType.TypeName == cRank.CommanderType)
                cRank.Sequence = _commanderRepository.FindAllCommanderRanksCount(cRank.CommanderType) + 1;

            cRank.CreatedById = context.GetLoggedInUserId();
            cRank.IsDeleted = false;
            cRank.CreatedAt = DateTime.UtcNow;
            var savedRank = await _commanderRepository.SaveCommanderRank(cRank);
            if (savedRank == null)
            {
                return new ApiResponse(500);
            }
            var rankTransferDTO = _mapper.Map<CommanderRankTransferDTO>(cRank);
            return new ApiOkResponse(rankTransferDTO);
        }

        public async Task<ApiResponse> AddCommanderType(HttpContext context, CommanderTypeAndRankReceivingDTO commanderTypeReceivingDTO)
        {
            var cType = _mapper.Map<CommanderType>(commanderTypeReceivingDTO);
            cType.CreatedById = context.GetLoggedInUserId();
            //cType.IsDeleted = false;
            cType.CreatedAt = DateTime.UtcNow;
            var savedType = await _commanderRepository.SaveCommanderType(cType);
            if (savedType == null)
            {
                return new ApiResponse(500);
            }
            var typeTransferDTO = _mapper.Map<CommanderTypeAndRankTransferDTO>(cType);
            return new ApiOkResponse(typeTransferDTO);
        }

        public async Task<ApiResponse> DeleteCommanderRank(long id)
        {
            var rankToDelete = await _commanderRepository.FindCommanderRankById(id);

            if (rankToDelete == null)
            {
                return new ApiResponse(404);
            }

            if (!await _commanderRepository.DeleteCommanderRank(rankToDelete))
            {
                return new ApiResponse(500);
            }

            return new ApiOkResponse(true);
        }

        public async Task<ApiResponse> DeleteCommanderType(long id)
        {
            var typeToDelete = await _commanderRepository.FindCommanderTypeById(id);

            if (typeToDelete == null)
            {
                return new ApiResponse(404);
            }

            if (!await _commanderRepository.DeleteCommanderType(typeToDelete))
            {
                return new ApiResponse(500);
            }

            return new ApiOkResponse(true);
        }

        public async Task<ApiResponse> GetAllCommanderRanks()
        {
            var cRank = await _commanderRepository.FindAllCommanderRanks();
            if (cRank == null)
            {
                return new ApiResponse(404);
            }
            var rankTransferDTO = _mapper.Map<IEnumerable<CommanderRankTransferDTO>>(cRank);
            return new ApiOkResponse(rankTransferDTO);
        }

        public async Task<ApiResponse> GetAllCommanderTypes()
        {
            var cTypes = await _commanderRepository.FindAllCommanderTypes();
            if (cTypes == null)
            {
                return new ApiResponse(404);
            }
            var typeTransferDTO = _mapper.Map<IEnumerable<CommanderTypeAndRankTransferDTO>>(cTypes);
            return new ApiOkResponse(typeTransferDTO);
        }

        public async Task<ApiResponse> GetCommanderRankById(long id)
        {
            var cRank = await _commanderRepository.FindCommanderRankById(id);
            if (cRank == null)
            {
                return new ApiResponse(404);
            }
            var rankTransferDTO = _mapper.Map<CommanderRankTransferDTO>(cRank);
            return new ApiOkResponse(rankTransferDTO);
        }

        public async Task<ApiResponse> GetCommanderTypeById(long id)
        {
            var cType = await _commanderRepository.FindCommanderTypeById(id);
            if (cType == null)
            {
                return new ApiResponse(404);
            }
            var typeTransferDTO = _mapper.Map<CommanderTypeAndRankTransferDTO>(cType);
            return new ApiOkResponse(typeTransferDTO);
            //throw new NotImplementedException();
        }

        public async Task<ApiResponse> UpdateCommanderRank(HttpContext context, long id, CommanderRankReceivingDTO commanderRankReceivingDTO)
        {
            var rankToUpdate = await _commanderRepository.FindCommanderRankById(id);
            if (rankToUpdate == null)
            {
                return new ApiResponse(404);
            }

            var summary = $"Initial details before change, \n {rankToUpdate.ToString()} \n";

            rankToUpdate.Alias = commanderRankReceivingDTO.Alias;
            rankToUpdate.Description = commanderRankReceivingDTO.Description;
            rankToUpdate.CommanderType = commanderRankReceivingDTO.CommanderType;
            rankToUpdate.RankName = commanderRankReceivingDTO.RankName;
            rankToUpdate.UpdatedAt = DateTime.UtcNow;
            //regionToUpdate.BranchId = regionReceivingDTO.BranchId;
            var updatedRank = await _commanderRepository.UpdateCommanderRank(rankToUpdate);

            summary += $"Details after change, \n {updatedRank.ToString()} \n";

            if (updatedRank == null)
            {
                return new ApiResponse(500);
            }

            var rankTransferDTOs = _mapper.Map<CommanderRankTransferDTO>(updatedRank);
            return new ApiOkResponse(rankTransferDTOs);
        }

        public async Task<ApiResponse> UpdateCommanderType(HttpContext context, long id, CommanderTypeAndRankReceivingDTO commanderTypeReceivingDTO)
        {
            var typeToUpdate = await _commanderRepository.FindCommanderTypeById(id);
            if (typeToUpdate == null)
            {
                return new ApiResponse(404);
            }

            var summary = $"Initial details before change, \n {typeToUpdate.ToString()} \n";

            typeToUpdate.TypeName = commanderTypeReceivingDTO.TypeName;
            typeToUpdate.TypeDesc = commanderTypeReceivingDTO.TypeDesc;
            typeToUpdate.UpdatedAt = DateTime.UtcNow;
            //regionToUpdate.BranchId = regionReceivingDTO.BranchId;
            var updatedtype = await _commanderRepository.UpdateCommanderType(typeToUpdate);

            summary += $"Details after change, \n {updatedtype.ToString()} \n";

            if (updatedtype == null)
            {
                return new ApiResponse(500);
            }

            var typeTransferDTOs = _mapper.Map<CommanderTypeAndRankTransferDTO>(updatedtype);
            return new ApiOkResponse(typeTransferDTOs);
        }
    }
}
