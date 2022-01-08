using AutoMapper;
using HaloBiz.DTOs;
using HaloBiz.DTOs.ApiDTOs;
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
    public class PilotServiceImpl:IPilotService
    {
        private readonly IPilotRepository _pilotRepository;
        private readonly IMapper _mapper;

        public PilotServiceImpl(IMapper mapper, IPilotRepository pilotRepository)
        {
            _mapper = mapper;
            _pilotRepository = pilotRepository;
        }

        public async Task<ApiCommonResponse> AddPilotRank(HttpContext context, PilotRankReceivingDTO pilotRankReceivingDTO)
        {
            var Rank = _mapper.Map<PilotRank>(pilotRankReceivingDTO);
            var getPilotType = await _pilotRepository.FindPilotTypeById(pilotRankReceivingDTO.PilotTypeId);
            var NameExist = _pilotRepository.GetRankname(pilotRankReceivingDTO.RankName);
            if (NameExist != null)
            {
                                 return CommonResponse.Send(ResponseCodes.NO_DATA_AVAILABLE,null, "No record exists");;
            }
            if (getPilotType.Id == Rank.PilotTypeId)
                Rank.Sequence = _pilotRepository.FindAllPilotRanksCount(Rank.PilotTypeId) + 1;
            Rank.CreatedById = context.GetLoggedInUserId();
            Rank.IsDeleted = false;
            Rank.CreatedAt = DateTime.UtcNow;
            //Rank.Sequence = 1;
            var savedRank = await _pilotRepository.SavePilotRank(Rank);
            if (savedRank == null)
            {
                return CommonResponse.Send(ResponseCodes.FAILURE, null, "Some system errors occurred");
            }
            var TransferDTO = _mapper.Map<PilotRankTransferDTO>(Rank);
            return CommonResponse.Send(ResponseCodes.SUCCESS,TransferDTO);
        }

        public async Task<ApiCommonResponse> AddPilotType(HttpContext context, PilotTypeReceivingDTO pilotTypeReceivingDTO)
        {
            var Type = _mapper.Map<PilotType>(pilotTypeReceivingDTO);
            var NameExist = _pilotRepository.GetTypename(pilotTypeReceivingDTO.TypeName);
            if (NameExist != null)
            {
                                 return CommonResponse.Send(ResponseCodes.NO_DATA_AVAILABLE,null, "No record exists");;
            }
            Type.CreatedById = context.GetLoggedInUserId();
            Type.IsDeleted = false;
            Type.CreatedAt = DateTime.UtcNow;
            var savedRank = await _pilotRepository.SavePilotType(Type);
            if (savedRank == null)
            {
                return CommonResponse.Send(ResponseCodes.FAILURE, null, "Some system errors occurred");
            }
            var TransferDTO = _mapper.Map<PilotTypeTransferDTO>(Type);
            return CommonResponse.Send(ResponseCodes.SUCCESS,TransferDTO);
        }

        public async Task<ApiCommonResponse> DeletePilotRank(long id)
        {
            var ToDelete = await _pilotRepository.FindPilotRankById(id);

            if (ToDelete == null)
            {
                return CommonResponse.Send(ResponseCodes.NO_DATA_AVAILABLE);;
            }

            if (!await _pilotRepository.DeletePilotRank(ToDelete))
            {
                return CommonResponse.Send(ResponseCodes.FAILURE, null, "Some system errors occurred");
            }

            return CommonResponse.Send(ResponseCodes.SUCCESS);
        }

        public async Task<ApiCommonResponse> DeletePilotType(long id)
        {
            var ToDelete = await _pilotRepository.FindPilotTypeById(id);

            if (ToDelete == null)
            {
                return CommonResponse.Send(ResponseCodes.NO_DATA_AVAILABLE);;
            }

            if (!await _pilotRepository.DeletePilotType(ToDelete))
            {
                return CommonResponse.Send(ResponseCodes.FAILURE, null, "Some system errors occurred");
            }

            return CommonResponse.Send(ResponseCodes.SUCCESS);
        }

        public async Task<ApiCommonResponse> GetAllPilotRanks()
        {
            var cRank = await _pilotRepository.FindAllPilotRanks();
            if (cRank == null)
            {
                return CommonResponse.Send(ResponseCodes.NO_DATA_AVAILABLE);;
            }
            var TransferDTO = _mapper.Map<IEnumerable<PilotRankTransferDTO>>(cRank);
            return CommonResponse.Send(ResponseCodes.SUCCESS,TransferDTO);
        }

        public async Task<ApiCommonResponse> GetAllPilotTypes()
        {
            var Type = await _pilotRepository.FindAllPilotTypes();
            if (Type == null)
            {
                return CommonResponse.Send(ResponseCodes.NO_DATA_AVAILABLE);;
            }
            var TransferDTO = _mapper.Map<IEnumerable<PilotTypeTransferDTO>>(Type);
            return CommonResponse.Send(ResponseCodes.SUCCESS,TransferDTO);
        }

        public async Task<ApiCommonResponse> GetPilotRankById(long id)
        {
            var cRank = await _pilotRepository.FindPilotRankById(id);
            if (cRank == null)
            {
                return CommonResponse.Send(ResponseCodes.NO_DATA_AVAILABLE);;
            }
            var TransferDTO = _mapper.Map<PilotRankTransferDTO>(cRank);
            return CommonResponse.Send(ResponseCodes.SUCCESS,TransferDTO);
        }

        public async Task<ApiCommonResponse> GetPilotTypeById(long id)
        {
            var Type = await _pilotRepository.FindPilotTypeById(id);
            if (Type == null)
            {
                return CommonResponse.Send(ResponseCodes.NO_DATA_AVAILABLE);;
            }
            var TransferDTO = _mapper.Map<PilotTypeTransferDTO>(Type);
            return CommonResponse.Send(ResponseCodes.SUCCESS,TransferDTO);
        }

        public async Task<ApiCommonResponse> UpdatePilotRank(HttpContext context, long id, PilotRankReceivingDTO pilotRankReceivingDTO)
        {
            var ToUpdate = await _pilotRepository.FindPilotRankById(id);
            if (ToUpdate == null)
            {
                return CommonResponse.Send(ResponseCodes.NO_DATA_AVAILABLE);;
            }

            var summary = $"Initial details before change, \n {ToUpdate.ToString()} \n";

            ToUpdate.Alias = pilotRankReceivingDTO.Alias;
            ToUpdate.Description = pilotRankReceivingDTO.Description;
            ToUpdate.RankName = pilotRankReceivingDTO.RankName;
            //ToUpdate.PilotTypeId = pilotRankReceivingDTO.RankName;
            ToUpdate.UpdatedAt = DateTime.UtcNow;
            var updatedRank = await _pilotRepository.UpdatePilotRank(ToUpdate);

            summary += $"Details after change, \n {updatedRank.ToString()} \n";

            if (updatedRank == null)
            {
                return CommonResponse.Send(ResponseCodes.FAILURE, null, "Some system errors occurred");
            }

            var TransferDTOs = _mapper.Map<PilotRankTransferDTO>(updatedRank);
            return CommonResponse.Send(ResponseCodes.SUCCESS,TransferDTOs);
        }

        public async Task<ApiCommonResponse> UpdatePilotType(HttpContext context, long id, PilotTypeReceivingDTO pilotTypeReceivingDTO)
        {
            var ToUpdate = await _pilotRepository.FindPilotTypeById(id);
            if (ToUpdate == null)
            {
                return CommonResponse.Send(ResponseCodes.NO_DATA_AVAILABLE);;
            }

            var summary = $"Initial details before change, \n {ToUpdate.ToString()} \n";

            ToUpdate.TypeName = pilotTypeReceivingDTO.TypeName;
            ToUpdate.TypeDesc = pilotTypeReceivingDTO.TypeDesc;
            //ToUpdate.PilotTypeId = pilotRankReceivingDTO.RankName;
            ToUpdate.UpdatedAt = DateTime.UtcNow;
            var updatedRank = await _pilotRepository.UpdatePilotType(ToUpdate);

            summary += $"Details after change, \n {updatedRank.ToString()} \n";

            if (updatedRank == null)
            {
                return CommonResponse.Send(ResponseCodes.FAILURE, null, "Some system errors occurred");
            }

            var TransferDTOs = _mapper.Map<PilotTypeTransferDTO>(updatedRank);
            return CommonResponse.Send(ResponseCodes.SUCCESS,TransferDTOs);
        }
    }
}
