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

        public async Task<ApiResponse> AddEscalationLevel(HttpContext context, EscalationLevelReceivingDTO escalationLevelReceivingDTO)
        {

            var escalationLevel = _mapper.Map<EscalationLevel>(escalationLevelReceivingDTO);
            escalationLevel.CreatedById = context.GetLoggedInUserId();
            var savedescalationLevel = await _escalationLevelRepo.SaveEscalationLevel(escalationLevel);
            if (savedescalationLevel == null)
            {
                return new ApiResponse(500);
            }
            var escalationLevelTransferDTO = _mapper.Map<EscalationLevelTransferDTO>(escalationLevel);
            return new ApiOkResponse(escalationLevelTransferDTO);
        }

        public async Task<ApiResponse> DeleteEscalationLevel(long id)
        {
            var escalationLevelToDelete = await _escalationLevelRepo.FindEscalationLevelById(id);
            if (escalationLevelToDelete == null)
            {
                return new ApiResponse(404);
            }

            if (!await _escalationLevelRepo.DeleteEscalationLevel(escalationLevelToDelete))
            {
                return new ApiResponse(500);
            }

            return new ApiOkResponse(true);
        }

        public async Task<ApiResponse> GetAllEscalationLevel()
        {
            var escalationLevels = await _escalationLevelRepo.FindAllEscalationLevels();
            if (escalationLevels == null)
            {
                return new ApiResponse(404);
            }
            var escalationLevelTransferDTO = _mapper.Map<IEnumerable<EscalationLevelTransferDTO>>(escalationLevels);
            return new ApiOkResponse(escalationLevelTransferDTO);
        }

        public async Task<ApiResponse> GetEscalationLevelById(long id)
        {
            var escalationLevel = await _escalationLevelRepo.FindEscalationLevelById(id);
            if (escalationLevel == null)
            {
                return new ApiResponse(404);
            }
            var escalationLevelTransferDTOs = _mapper.Map<EscalationLevelTransferDTO>(escalationLevel);
            return new ApiOkResponse(escalationLevelTransferDTOs);
        }

        public async Task<ApiResponse> GetEscalationLevelByName(string name)
        {
            var escalationLevel = await _escalationLevelRepo.FindEscalationLevelByName(name);
            if (escalationLevel == null)
            {
                return new ApiResponse(404);
            }
            var escalationLevelTransferDTOs = _mapper.Map<EscalationLevelTransferDTO>(escalationLevel);
            return new ApiOkResponse(escalationLevelTransferDTOs);
        }

        public async Task<ApiResponse> UpdateEscalationLevel(HttpContext context, long id, EscalationLevelReceivingDTO escalationLevelReceivingDTO)
        {
            var escalationLevelToUpdate = await _escalationLevelRepo.FindEscalationLevelById(id);
            if (escalationLevelToUpdate == null)
            {
                return new ApiResponse(404);
            }

            var summary = $"Initial details before change, \n {escalationLevelToUpdate.ToString()} \n";

            escalationLevelToUpdate.Caption = escalationLevelReceivingDTO.Caption;
            escalationLevelToUpdate.Description = escalationLevelReceivingDTO.Description;
            var updatedescalationLevel = await _escalationLevelRepo.UpdateEscalationLevel(escalationLevelToUpdate);

            summary += $"Details after change, \n {updatedescalationLevel.ToString()} \n";

            if (updatedescalationLevel == null)
            {
                return new ApiResponse(500);
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
            return new ApiOkResponse(escalationLevelTransferDTOs);
        }
    }
}
