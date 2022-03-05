using AutoMapper;
using HaloBiz.DTOs.ApiDTOs;
using HaloBiz.DTOs.ReceivingDTOs;
using HaloBiz.DTOs.TransferDTOs;
using HaloBiz.Helpers;
using HalobizMigrations.Models;
using HaloBiz.Repository;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HalobizMigrations.Models.Complaints;

namespace HaloBiz.MyServices.Impl
{
    public class EscalationLevelServiceImpl : IEscalationLevelService
    {
        private readonly ILogger<EscalationLevelServiceImpl> _logger;
        private readonly IModificationHistoryRepository _historyRepo;
        private readonly IEscalationLevelRepository _escalationLevelRepo;
        private readonly IMapper _mapper;

        public EscalationLevelServiceImpl(IModificationHistoryRepository historyRepo, IEscalationLevelRepository EscalationLevelRepo, ILogger<EscalationLevelServiceImpl> logger, IMapper mapper)
        {
            this._mapper = mapper;
            this._historyRepo = historyRepo;
            this._escalationLevelRepo = EscalationLevelRepo;
            this._logger = logger;
        }

        public async Task<ApiCommonResponse> AddEscalationLevel(HttpContext context, EscalationLevelReceivingDTO escalationLevelReceivingDTO)
        {

            var escalationLevel = _mapper.Map<EscalationLevel>(escalationLevelReceivingDTO);
            escalationLevel.CreatedById = context.GetLoggedInUserId();
            var savedescalationLevel = await _escalationLevelRepo.SaveEscalationLevel(escalationLevel);
            if (savedescalationLevel == null)
            {
                return CommonResponse.Send(ResponseCodes.FAILURE, null, "Some system errors occurred");
            }
            var escalationLevelTransferDTO = _mapper.Map<EscalationLevelTransferDTO>(escalationLevel);
            return CommonResponse.Send(ResponseCodes.SUCCESS,escalationLevelTransferDTO);
        }

        public async Task<ApiCommonResponse> DeleteEscalationLevel(long id)
        {
            var escalationLevelToDelete = await _escalationLevelRepo.FindEscalationLevelById(id);
            if (escalationLevelToDelete == null)
            {
                return CommonResponse.Send(ResponseCodes.NO_DATA_AVAILABLE);;
            }

            if (!await _escalationLevelRepo.DeleteEscalationLevel(escalationLevelToDelete))
            {
                return CommonResponse.Send(ResponseCodes.FAILURE, null, "Some system errors occurred");
            }

            return CommonResponse.Send(ResponseCodes.SUCCESS);
        }

        public async Task<ApiCommonResponse> GetAllEscalationLevel()
        {
            var escalationLevels = await _escalationLevelRepo.FindAllEscalationLevels();
            if (escalationLevels == null)
            {
                return CommonResponse.Send(ResponseCodes.NO_DATA_AVAILABLE);;
            }
            var escalationLevelTransferDTO = _mapper.Map<IEnumerable<EscalationLevelTransferDTO>>(escalationLevels);
            return CommonResponse.Send(ResponseCodes.SUCCESS,escalationLevelTransferDTO);
        }

        public async Task<ApiCommonResponse> GetEscalationLevelById(long id)
        {
            var escalationLevel = await _escalationLevelRepo.FindEscalationLevelById(id);
            if (escalationLevel == null)
            {
                return CommonResponse.Send(ResponseCodes.NO_DATA_AVAILABLE);;
            }
            var escalationLevelTransferDTOs = _mapper.Map<EscalationLevelTransferDTO>(escalationLevel);
            return CommonResponse.Send(ResponseCodes.SUCCESS,escalationLevelTransferDTOs);
        }

        public async Task<ApiCommonResponse> GetEscalationLevelByName(string name)
        {
            var escalationLevel = await _escalationLevelRepo.FindEscalationLevelByName(name);
            if (escalationLevel == null)
            {
                return CommonResponse.Send(ResponseCodes.NO_DATA_AVAILABLE);;
            }
            var escalationLevelTransferDTOs = _mapper.Map<EscalationLevelTransferDTO>(escalationLevel);
            return CommonResponse.Send(ResponseCodes.SUCCESS,escalationLevelTransferDTOs);
        }

        public async Task<ApiCommonResponse> UpdateEscalationLevel(HttpContext context, long id, EscalationLevelReceivingDTO escalationLevelReceivingDTO)
        {
            var escalationLevelToUpdate = await _escalationLevelRepo.FindEscalationLevelById(id);
            if (escalationLevelToUpdate == null)
            {
                return CommonResponse.Send(ResponseCodes.NO_DATA_AVAILABLE);;
            }

            var summary = $"Initial details before change, \n {escalationLevelToUpdate.ToString()} \n";

            escalationLevelToUpdate.Caption = escalationLevelReceivingDTO.Caption;
            escalationLevelToUpdate.Description = escalationLevelReceivingDTO.Description;
            var updatedescalationLevel = await _escalationLevelRepo.UpdateEscalationLevel(escalationLevelToUpdate);

            summary += $"Details after change, \n {updatedescalationLevel.ToString()} \n";

            if (updatedescalationLevel == null)
            {
                return CommonResponse.Send(ResponseCodes.FAILURE, null, "Some system errors occurred");
            }
            ModificationHistory history = new ModificationHistory()
            {
                ModelChanged = "escalationLevel",
                ChangeSummary = summary,
                ChangedById = context.GetLoggedInUserId(),
                ModifiedModelId = updatedescalationLevel.Id
            };
            await _historyRepo.SaveHistory(history);

            var escalationLevelTransferDTOs = _mapper.Map<EscalationLevelTransferDTO>(updatedescalationLevel);
            return CommonResponse.Send(ResponseCodes.SUCCESS,escalationLevelTransferDTOs);
        }
    }
}
