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

        public async Task<ApiCommonResponse> AddArmedEscortRank(HttpContext context, ArmedEscortRankReceivingDTO armedEscortRankReceivingDTO)
        {
            var armedescortRank = _mapper.Map<ArmedEscortRank>(armedEscortRankReceivingDTO);
            
            var getEscortType = await _armedEscortsRepository.FindArmedEscortTypeById(armedescortRank.ArmedEscortTypeId);
            var rankNameExist = _armedEscortsRepository.GetRankname(armedEscortRankReceivingDTO.RankName);
            if (rankNameExist != null)
            {
                return CommonResponse.Send(ResponseCodes.NO_DATA_AVAILABLE,null, "No record exists");
            }

            if (getEscortType.Id == armedescortRank.ArmedEscortTypeId)
                armedescortRank.Sequence = _armedEscortsRepository.FindAllArmedEscortRanksCount(armedescortRank.ArmedEscortTypeId) + 1;
            armedescortRank.CreatedById = context.GetLoggedInUserId();
            armedescortRank.IsDeleted = false;
            armedescortRank.CreatedAt = DateTime.UtcNow;

            //check if name already exists
           
            var savedRank = await _armedEscortsRepository.SaveArmedEscortRank(armedescortRank);
            if (savedRank == null)
            {
                return CommonResponse.Send(ResponseCodes.FAILURE, null, "Some system errors occurred");
            }
            var rankTransferDTO = _mapper.Map<ArmedEscortRankTransferDTO>(armedescortRank);
            return CommonResponse.Send(ResponseCodes.SUCCESS,rankTransferDTO);
        }

        public async Task<ApiCommonResponse> AddArmedEscortType(HttpContext context, ArmedEscortTypeReceivingDTO armedEscortTypeReceivingDTO)
        {
            var armedescortType = _mapper.Map<ArmedEscortType>(armedEscortTypeReceivingDTO);
            var NameExist = _armedEscortsRepository.GetTypename(armedEscortTypeReceivingDTO.Name);
            if (NameExist != null)
            {
                return CommonResponse.Send(ResponseCodes.NO_DATA_AVAILABLE, null, "No record exists");
            }
            armedescortType.CreatedById = context.GetLoggedInUserId();
            //armedescortType.IsDeleted = false;
            armedescortType.CreatedAt = DateTime.UtcNow;
            var savedRank = await _armedEscortsRepository.SaveArmedEscortType(armedescortType);
            if (savedRank == null)
            {
                return CommonResponse.Send(ResponseCodes.FAILURE, null, "Some system errors occurred");
            }
            var typeTransferDTO = _mapper.Map<ArmedEscortTypeTransferDTO>(armedescortType);
            return CommonResponse.Send(ResponseCodes.SUCCESS,typeTransferDTO);
        }

        public async Task<ApiCommonResponse> DeleteArmedEscortRank(long id)
        {
            var rankToDelete = await _armedEscortsRepository.FindArmedEscortRankById(id);

            if (rankToDelete == null)
            {
                return CommonResponse.Send(ResponseCodes.NO_DATA_AVAILABLE);;
            }

            if (!await _armedEscortsRepository.DeleteArmedEscortRank(rankToDelete))
            {
                return CommonResponse.Send(ResponseCodes.FAILURE, null, "Some system errors occurred");
            }

            return CommonResponse.Send(ResponseCodes.SUCCESS);
        }

        public async Task<ApiCommonResponse> DeleteArmedEscortType(long id)
        {
            var typeToDelete = await _armedEscortsRepository.FindArmedEscortTypeById(id);

            if (typeToDelete == null)
            {
                return CommonResponse.Send(ResponseCodes.NO_DATA_AVAILABLE);;
            }

            if (!await _armedEscortsRepository.DeleteArmedEscortType(typeToDelete))
            {
                return CommonResponse.Send(ResponseCodes.FAILURE, null, "Some system errors occurred");
            }

            return CommonResponse.Send(ResponseCodes.SUCCESS);
        }

        public async Task<ApiCommonResponse> GetAllArmedEscortRanks()
        {
            var cRank = await _armedEscortsRepository.FindAllArmedEscortRanks();
            if (cRank == null)
            {
                return CommonResponse.Send(ResponseCodes.NO_DATA_AVAILABLE);;
            }
            var rankTransferDTO = _mapper.Map<IEnumerable<ArmedEscortRankTransferDTO>>(cRank);
            return CommonResponse.Send(ResponseCodes.SUCCESS,rankTransferDTO);
        }

        public async Task<ApiCommonResponse> GetAllArmedEscortTypes()
        {
            var Type = await _armedEscortsRepository.FindAllArmedEscortTypes();
            if (Type == null)
            {
                return CommonResponse.Send(ResponseCodes.NO_DATA_AVAILABLE);;
            }
            var TransferDTO = _mapper.Map<IEnumerable<ArmedEscortTypeTransferDTO>>(Type);
            return CommonResponse.Send(ResponseCodes.SUCCESS,TransferDTO);
        }

        public async Task<ApiCommonResponse> GetArmedEscortRankById(long id)
        {
            var cRank = await _armedEscortsRepository.FindArmedEscortRankById(id);
            if (cRank == null)
            {
                return CommonResponse.Send(ResponseCodes.NO_DATA_AVAILABLE);;
            }
            var rankTransferDTO = _mapper.Map<ArmedEscortRankTransferDTO>(cRank);
            return CommonResponse.Send(ResponseCodes.SUCCESS,rankTransferDTO);
        }

        public async Task<ApiCommonResponse> GetArmedEscortTypeById(long id)
        {
            var Type = await _armedEscortsRepository.FindArmedEscortTypeById(id);
            if (Type == null)
            {
                return CommonResponse.Send(ResponseCodes.NO_DATA_AVAILABLE);;
            }
            var TransferDTO = _mapper.Map<ArmedEscortTypeTransferDTO>(Type);
            return CommonResponse.Send(ResponseCodes.SUCCESS,TransferDTO);
        }

        public async Task<ApiCommonResponse> UpdateArmedEscortRank(HttpContext context, long id, ArmedEscortRankReceivingDTO armedEscortRankReceivingDTO)
        {
            var rankToUpdate = await _armedEscortsRepository.FindArmedEscortRankById(id);
            if (rankToUpdate == null)
            {
                return CommonResponse.Send(ResponseCodes.NO_DATA_AVAILABLE);;
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
                return CommonResponse.Send(ResponseCodes.FAILURE, null, "Some system errors occurred");
            }

            var rankTransferDTOs = _mapper.Map<ArmedEscortRankTransferDTO>(updatedRank);
            return CommonResponse.Send(ResponseCodes.SUCCESS,rankTransferDTOs);
        }

        public async Task<ApiCommonResponse> UpdateArmedEscortType(HttpContext context, long id, ArmedEscortTypeReceivingDTO armedEscortTypeReceivingDTO)
        {
            var typeToUpdate = await _armedEscortsRepository.FindArmedEscortTypeById(id);
            if (typeToUpdate == null)
            {
                return CommonResponse.Send(ResponseCodes.NO_DATA_AVAILABLE);;
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
                return CommonResponse.Send(ResponseCodes.FAILURE, null, "Some system errors occurred");
            }

            var typeTransferDTOs = _mapper.Map<ArmedEscortTypeTransferDTO>(updatedType);
            return CommonResponse.Send(ResponseCodes.SUCCESS,typeTransferDTOs);
        }
    }
}
