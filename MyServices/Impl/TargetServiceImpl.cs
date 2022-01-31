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

namespace HaloBiz.MyServices.Impl
{
    public class TargetServiceImpl : ITargetService
    {
        private readonly ILogger<TargetServiceImpl> _logger;
        private readonly IModificationHistoryRepository _historyRepo;
        private readonly ITargetRepository _targetRepo;
        private readonly IMapper _mapper;

        public TargetServiceImpl(IModificationHistoryRepository historyRepo, ITargetRepository TargetRepo, ILogger<TargetServiceImpl> logger, IMapper mapper)
        {
            this._mapper = mapper;
            this._historyRepo = historyRepo;
            this._targetRepo = TargetRepo;
            this._logger = logger;
        }

        public async Task<ApiCommonResponse> AddTarget(HttpContext context, TargetReceivingDTO targetReceivingDTO)
        {

            var target = _mapper.Map<Target>(targetReceivingDTO);
            target.CreatedById = context.GetLoggedInUserId();
            var savedTarget = await _targetRepo.SaveTarget(target);
            if (savedTarget == null)
            {
                return CommonResponse.Send(ResponseCodes.FAILURE, null, "Some system errors occurred");
            }
            var targetTransferDTO = _mapper.Map<TargetTransferDTO>(target);
            return CommonResponse.Send(ResponseCodes.SUCCESS,targetTransferDTO);
        }

        public async Task<ApiCommonResponse> DeleteTarget(long id)
        {
            var targetToDelete = await _targetRepo.FindTargetById(id);
            if (targetToDelete == null)
            {
                return CommonResponse.Send(ResponseCodes.NO_DATA_AVAILABLE);;
            }

            if (!await _targetRepo.DeleteTarget(targetToDelete))
            {
                return CommonResponse.Send(ResponseCodes.FAILURE, null, "Some system errors occurred");
            }

            return CommonResponse.Send(ResponseCodes.SUCCESS);
        }

        public async Task<ApiCommonResponse> GetAllTarget()
        {
            var targets = await _targetRepo.FindAllTargets();
            if (targets == null)
            {
                return CommonResponse.Send(ResponseCodes.NO_DATA_AVAILABLE);;
            }
            var targetTransferDTO = _mapper.Map<IEnumerable<TargetTransferDTO>>(targets);
            return CommonResponse.Send(ResponseCodes.SUCCESS,targetTransferDTO);
        }

        public async Task<ApiCommonResponse> GetTargetById(long id)
        {
            var target = await _targetRepo.FindTargetById(id);
            if (target == null)
            {
                return CommonResponse.Send(ResponseCodes.NO_DATA_AVAILABLE);;
            }
            var targetTransferDTOs = _mapper.Map<TargetTransferDTO>(target);
            return CommonResponse.Send(ResponseCodes.SUCCESS,targetTransferDTOs);
        }

        public async Task<ApiCommonResponse> GetTargetByName(string name)
        {
            var target = await _targetRepo.FindTargetByName(name);
            if (target == null)
            {
                return CommonResponse.Send(ResponseCodes.NO_DATA_AVAILABLE);;
            }
            var targetTransferDTOs = _mapper.Map<TargetTransferDTO>(target);
            return CommonResponse.Send(ResponseCodes.SUCCESS,targetTransferDTOs);
        }

        public async Task<ApiCommonResponse> UpdateTarget(HttpContext context, long id, TargetReceivingDTO targetReceivingDTO)
        {
            var targetToUpdate = await _targetRepo.FindTargetById(id);
            if (targetToUpdate == null)
            {
                return CommonResponse.Send(ResponseCodes.NO_DATA_AVAILABLE);;
            }

            var summary = $"Initial details before change, \n {targetToUpdate.ToString()} \n";

            targetToUpdate.Caption = targetReceivingDTO.Caption;
            targetToUpdate.Description = targetReceivingDTO.Description;
            var updatedtarget = await _targetRepo.UpdateTarget(targetToUpdate);

            summary += $"Details after change, \n {updatedtarget.ToString()} \n";

            if (updatedtarget == null)
            {
                return CommonResponse.Send(ResponseCodes.FAILURE, null, "Some system errors occurred");
            }
            ModificationHistory history = new ModificationHistory()
            {
                ModelChanged = "Target",
                ChangeSummary = summary,
                ChangedById = context.GetLoggedInUserId(),
                ModifiedModelId = updatedtarget.Id
            };
            await _historyRepo.SaveHistory(history);

            var targetTransferDTOs = _mapper.Map<TargetTransferDTO>(updatedtarget);
            return CommonResponse.Send(ResponseCodes.SUCCESS,targetTransferDTOs);
        }
    }
}
