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
    public class ArmedEscortServiceImpl:IArmedEscortService
    {
        private readonly IArmedEscortsRepository _armedEscortsRepository;
        private readonly IMapper _mapper;

        public ArmedEscortServiceImpl(IMapper mapper, IArmedEscortsRepository armedEscortsRepository)
        {
            _mapper = mapper;
            _armedEscortsRepository = armedEscortsRepository;
        }

        public async Task<ApiResponse> AddArmedEscortRank(HttpContext context, ArmedEscortRankReceivingDTO armedEscortRankReceivingDTO)
        {
            var armedescortRank = _mapper.Map<ArmedEscortRank>(armedEscortRankReceivingDTO);
            var getEscortType = await _armedEscortsRepository.FindArmedEscortTypeById(armedescortRank.ArmedEscortTypeId);
            if (getEscortType.Id == armedescortRank.ArmedEscortTypeId)
                armedescortRank.Sequence = _armedEscortsRepository.FindAllArmedEscortRanksCount(armedescortRank.ArmedEscortTypeId) + 1;
            armedescortRank.CreatedById = context.GetLoggedInUserId();
            armedescortRank.IsDeleted = false;
            armedescortRank.CreatedAt = DateTime.UtcNow;
           
            var savedRank = await _armedEscortsRepository.SaveArmedEscortRank(armedescortRank);
            if (savedRank == null)
            {
                return new ApiResponse(500);
            }
            var rankTransferDTO = _mapper.Map<ArmedEscortRankTransferDTO>(armedescortRank);
            return new ApiOkResponse(rankTransferDTO);
        }

        public async Task<ApiResponse> AddArmedEscortType(HttpContext context, ArmedEscortTypeReceivingDTO armedEscortTypeReceivingDTO)
        {
            var armedescortType = _mapper.Map<ArmedEscortType>(armedEscortTypeReceivingDTO);
            armedescortType.CreatedById = context.GetLoggedInUserId();
            armedescortType.IsDeleted = false;
            armedescortType.CreatedAt = DateTime.UtcNow;
            var savedRank = await _armedEscortsRepository.SaveArmedEscortType(armedescortType);
            if (savedRank == null)
            {
                return new ApiResponse(500);
            }
            var typeTransferDTO = _mapper.Map<ArmedEscortTypeTransferDTO>(armedescortType);
            return new ApiOkResponse(typeTransferDTO);
        }

        public async Task<ApiResponse> DeleteArmedEscortRank(long id)
        {
            var rankToDelete = await _armedEscortsRepository.FindArmedEscortRankById(id);

            if (rankToDelete == null)
            {
                return new ApiResponse(404);
            }

            if (!await _armedEscortsRepository.DeleteArmedEscortRank(rankToDelete))
            {
                return new ApiResponse(500);
            }

            return new ApiOkResponse(true);
        }

        public async Task<ApiResponse> DeleteArmedEscortType(long id)
        {
            var typeToDelete = await _armedEscortsRepository.FindArmedEscortTypeById(id);

            if (typeToDelete == null)
            {
                return new ApiResponse(404);
            }

            if (!await _armedEscortsRepository.DeleteArmedEscortType(typeToDelete))
            {
                return new ApiResponse(500);
            }

            return new ApiOkResponse(true);
        }

        public async Task<ApiResponse> GetAllArmedEscortRanks()
        {
            var cRank = await _armedEscortsRepository.FindAllArmedEscortRanks();
            if (cRank == null)
            {
                return new ApiResponse(404);
            }
            var rankTransferDTO = _mapper.Map<IEnumerable<ArmedEscortRankTransferDTO>>(cRank);
            return new ApiOkResponse(rankTransferDTO);
        }

        public async Task<ApiResponse> GetAllArmedEscortTypes()
        {
            var Type = await _armedEscortsRepository.FindAllArmedEscortTypes();
            if (Type == null)
            {
                return new ApiResponse(404);
            }
            var rankTransferDTO = _mapper.Map<IEnumerable<ArmedEscortTypeTransferDTO>>(Type);
            return new ApiOkResponse(rankTransferDTO);
        }

        public async Task<ApiResponse> GetArmedEscortRankById(long id)
        {
            var cRank = await _armedEscortsRepository.FindArmedEscortRankById(id);
            if (cRank == null)
            {
                return new ApiResponse(404);
            }
            var rankTransferDTO = _mapper.Map<ArmedEscortRankTransferDTO>(cRank);
            return new ApiOkResponse(rankTransferDTO);
        }

        public async Task<ApiResponse> GetArmedEscortTypeById(long id)
        {
            var Type = await _armedEscortsRepository.FindArmedEscortTypeById(id);
            if (Type == null)
            {
                return new ApiResponse(404);
            }
            var rankTransferDTO = _mapper.Map<ArmedEscortTypeTransferDTO>(Type);
            return new ApiOkResponse(rankTransferDTO);
        }

        public async Task<ApiResponse> UpdateArmedEscortRank(HttpContext context, long id, ArmedEscortRankReceivingDTO armedEscortRankReceivingDTO)
        {
            var rankToUpdate = await _armedEscortsRepository.FindArmedEscortRankById(id);
            if (rankToUpdate == null)
            {
                return new ApiResponse(404);
            }

            var summary = $"Initial details before change, \n {rankToUpdate.ToString()} \n";

            rankToUpdate.Alias = armedEscortRankReceivingDTO.Alias;
            rankToUpdate.Description = armedEscortRankReceivingDTO.Description;
            rankToUpdate.ArmedEscortTypeId = armedEscortRankReceivingDTO.ArmedEscortTypeId;
            rankToUpdate.RankName = armedEscortRankReceivingDTO.RankName;
            rankToUpdate.UpdatedAt = DateTime.UtcNow;
            var updatedRank = await _armedEscortsRepository.UpdateArmedEscortRank(rankToUpdate);

            summary += $"Details after change, \n {updatedRank.ToString()} \n";

            if (updatedRank == null)
            {
                return new ApiResponse(500);
            }

            var rankTransferDTOs = _mapper.Map<ArmedEscortRankTransferDTO>(updatedRank);
            return new ApiOkResponse(rankTransferDTOs);
        }

        public async Task<ApiResponse> UpdateArmedEscortType(HttpContext context, long id, ArmedEscortTypeReceivingDTO armedEscortTypeReceivingDTO)
        {
            var typeToUpdate = await _armedEscortsRepository.FindArmedEscortTypeById(id);
            if (typeToUpdate == null)
            {
                return new ApiResponse(404);
            }

            var summary = $"Initial details before change, \n {typeToUpdate.ToString()} \n";

            typeToUpdate.Caption = armedEscortTypeReceivingDTO.Caption;
            typeToUpdate.Description = armedEscortTypeReceivingDTO.Description;
            typeToUpdate.Name = armedEscortTypeReceivingDTO.Name;
            typeToUpdate.UpdatedAt = DateTime.UtcNow;
            var updatedType = await _armedEscortsRepository.UpdateArmedEscortType(typeToUpdate);

            summary += $"Details after change, \n {updatedType.ToString()} \n";

            if (updatedType == null)
            {
                return new ApiResponse(500);
            }

            var typeTransferDTOs = _mapper.Map<ArmedEscortTypeTransferDTO>(updatedType);
            return new ApiOkResponse(typeTransferDTOs);
        }
    }
}
