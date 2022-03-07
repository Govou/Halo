using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using Halobiz.Common.DTOs.ApiDTOs;
using HaloBiz.DTOs.ReceivingDTOs;
using HaloBiz.DTOs.TransferDTOs.LAMS;
using HaloBiz.Helpers;
using HalobizMigrations.Models;

using HaloBiz.MyServices.LAMS;
using HaloBiz.Repository;
using HaloBiz.Repository.LAMS;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace HaloBiz.MyServices.Impl.LAMS
{
    public class DropReasonServiceImpl : IDropReasonService
    {
        private readonly ILogger<DropReasonServiceImpl> _logger;
        private readonly IModificationHistoryRepository _historyRepo;
        private readonly IDropReasonRepository _DropReasonRepo;
        private readonly IMapper _mapper;

        public DropReasonServiceImpl(IModificationHistoryRepository historyRepo, IDropReasonRepository DropReasonRepo, ILogger<DropReasonServiceImpl> logger, IMapper mapper)
        {
            this._mapper = mapper;
            this._historyRepo = historyRepo;
            this._DropReasonRepo = DropReasonRepo;
            this._logger = logger;
        }

        public async Task<ApiCommonResponse> AddDropReason(HttpContext context, DropReasonReceivingDTO DropReasonReceivingDTO)
        {
            var DropReason = _mapper.Map<DropReason>(DropReasonReceivingDTO);
            DropReason.CreatedById = context.GetLoggedInUserId();
            var savedDropReason = await _DropReasonRepo.SaveDropReason(DropReason);
            if (savedDropReason == null)
            {
                return CommonResponse.Send(ResponseCodes.FAILURE, null, "Some system errors occurred");
            }
            var DropReasonTransferDTO = _mapper.Map<DropReasonTransferDTO>(DropReason);
            return CommonResponse.Send(ResponseCodes.SUCCESS,DropReasonTransferDTO);
        }

        public async Task<ApiCommonResponse> GetAllDropReason()
        {
            var DropReasons = await _DropReasonRepo.FindAllDropReason();
            if (DropReasons == null)
            {
                return CommonResponse.Send(ResponseCodes.NO_DATA_AVAILABLE);;
            }
            var DropReasonTransferDTO = _mapper.Map<IEnumerable<DropReasonTransferDTO>>(DropReasons);
            return CommonResponse.Send(ResponseCodes.SUCCESS,DropReasonTransferDTO);
        }

        public async Task<ApiCommonResponse> GetDropReasonById(long id)
        {
            var DropReason = await _DropReasonRepo.FindDropReasonById(id);
            if (DropReason == null)
            {
                return CommonResponse.Send(ResponseCodes.NO_DATA_AVAILABLE);;
            }
            var DropReasonTransferDTOs = _mapper.Map<DropReasonTransferDTO>(DropReason);
            return CommonResponse.Send(ResponseCodes.SUCCESS,DropReasonTransferDTOs);
        }

        public async Task<ApiCommonResponse> GetDropReasonByTitle(string title)
        {
            var DropReason = await _DropReasonRepo.FindDropReasonByTitle(title);
            if (DropReason == null)
            {
                return CommonResponse.Send(ResponseCodes.NO_DATA_AVAILABLE);;
            }
            var DropReasonTransferDTOs = _mapper.Map<DropReasonTransferDTO>(DropReason);
            return CommonResponse.Send(ResponseCodes.SUCCESS,DropReasonTransferDTOs);
        }

        public async Task<ApiCommonResponse> UpdateDropReason(HttpContext context, long id, DropReasonReceivingDTO DropReasonReceivingDTO)
        {
            var DropReasonToUpdate = await _DropReasonRepo.FindDropReasonById(id);
            if (DropReasonToUpdate == null)
            {
                return CommonResponse.Send(ResponseCodes.NO_DATA_AVAILABLE);;
            }
            
            var summary = $"Initial details before change, \n {DropReasonToUpdate.ToString()} \n" ;

            DropReasonToUpdate.Title = DropReasonReceivingDTO.Title;
            var updatedDropReason = await _DropReasonRepo.UpdateDropReason(DropReasonToUpdate);

            summary += $"Details after change, \n {updatedDropReason.ToString()} \n";

            if (updatedDropReason == null)
            {
                return CommonResponse.Send(ResponseCodes.FAILURE, null, "Some system errors occurred");
            }
            ModificationHistory history = new ModificationHistory(){
                ModelChanged = "DropReason",
                ChangeSummary = summary,
                ChangedById = context.GetLoggedInUserId(),
                ModifiedModelId = updatedDropReason.Id
            };

            await _historyRepo.SaveHistory(history);

            var DropReasonTransferDTOs = _mapper.Map<DropReasonTransferDTO>(updatedDropReason);
            return CommonResponse.Send(ResponseCodes.SUCCESS,DropReasonTransferDTOs);

        }

        public async Task<ApiCommonResponse> DeleteDropReason(long id)
        {
            var DropReasonToDelete = await _DropReasonRepo.FindDropReasonById(id);
            if (DropReasonToDelete == null)
            {
                return CommonResponse.Send(ResponseCodes.NO_DATA_AVAILABLE);;
            }

            if (!await _DropReasonRepo.DeleteDropReason(DropReasonToDelete))
            {
                return CommonResponse.Send(ResponseCodes.FAILURE, null, "Some system errors occurred");
            }

            return CommonResponse.Send(ResponseCodes.SUCCESS);
        }
    }
}