using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using HaloBiz.DTOs.ApiDTOs;
using HaloBiz.DTOs.ReceivingDTOs;
using HaloBiz.DTOs.TransferDTOs.LAMS;
using HaloBiz.Helpers;
using HaloBiz.Model;
using HaloBiz.Model.LAMS;
using HaloBiz.MyServices.LAMS;
using HaloBiz.Repository;
using HaloBiz.Repository.LAMS;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace HaloBiz.MyServices.Impl.LAMS
{
    public class OtherLeadCaptureInfoServiceImpl : IOtherLeadCaptureInfoService
    {
        private readonly ILogger<OtherLeadCaptureInfoServiceImpl> _logger;
        private readonly IModificationHistoryRepository _historyRepo;
        private readonly IOtherLeadCaptureInfoRepository _otherLeadCaptureInfoRepo;
        private readonly IMapper _mapper;

        public OtherLeadCaptureInfoServiceImpl(IModificationHistoryRepository historyRepo, IOtherLeadCaptureInfoRepository otherLeadCaptureInfoRepo, ILogger<OtherLeadCaptureInfoServiceImpl> logger, IMapper mapper)
        {
            this._mapper = mapper;
            this._historyRepo = historyRepo;
            this._otherLeadCaptureInfoRepo = otherLeadCaptureInfoRepo;
            this._logger = logger;
        }

        public async Task<ApiResponse> AddOtherLeadCaptureInfo(HttpContext context, OtherLeadCaptureInfoReceivingDTO otherLeadCaptureInfoReceivingDTO)
        {
            var otherLeadCaptureInfo = _mapper.Map<OtherLeadCaptureInfo>(otherLeadCaptureInfoReceivingDTO);
            otherLeadCaptureInfo.CreatedById = context.GetLoggedInUserId();
            var savedOtherLeadCaptureInfo = await _otherLeadCaptureInfoRepo.SaveOtherLeadCaptureInfo(otherLeadCaptureInfo);
            if (savedOtherLeadCaptureInfo == null)
            {
                return new ApiResponse(500);
            }
            var otherLeadCaptureInfoTransferDTO = _mapper.Map<OtherLeadCaptureInfoTransferDTO>(savedOtherLeadCaptureInfo);
            return new ApiOkResponse(otherLeadCaptureInfoTransferDTO);
        }

        public async Task<ApiResponse> GetAllOtherLeadCaptureInfo()
        {
            var otherLeadCaptureInfos = await _otherLeadCaptureInfoRepo.FindAllOtherLeadCaptureInfo();
            if (otherLeadCaptureInfos == null)
            {
                return new ApiResponse(404);
            }
            var otherLeadCaptureInfoTransferDTO = _mapper.Map<IEnumerable<OtherLeadCaptureInfoTransferDTO>>(otherLeadCaptureInfos);
            return new ApiOkResponse(otherLeadCaptureInfoTransferDTO);
        }

        public async Task<ApiResponse> GetOtherLeadCaptureInfoById(long id)
        {
            var otherLeadCaptureInfo = await _otherLeadCaptureInfoRepo.FindOtherLeadCaptureInfoById(id);
            if (otherLeadCaptureInfo == null)
            {
                return new ApiResponse(404);
            }
            var otherLeadCaptureInfoTransferDTOs = _mapper.Map<OtherLeadCaptureInfoTransferDTO>(otherLeadCaptureInfo);
            return new ApiOkResponse(otherLeadCaptureInfoTransferDTOs);
        }

        public async Task<ApiResponse> UpdateOtherLeadCaptureInfo(HttpContext context, long id, OtherLeadCaptureInfoReceivingDTO otherLeadCaptureInfoReceivingDTO)
        {
            var otherLeadCaptureInfoToUpdate = await _otherLeadCaptureInfoRepo.FindOtherLeadCaptureInfoById(id);
            if (otherLeadCaptureInfoToUpdate == null)
            {
                return new ApiResponse(404);
            }
            
            var summary = $"Initial details before change, \n {otherLeadCaptureInfoToUpdate.ToString()} \n" ;

            otherLeadCaptureInfoToUpdate.CooperateEstimatedAnnualIncome = otherLeadCaptureInfoReceivingDTO.CooperateEstimatedAnnualIncome;
            otherLeadCaptureInfoToUpdate.CooperateEstimatedAnnualProfit = otherLeadCaptureInfoReceivingDTO.CooperateEstimatedAnnualProfit;
            otherLeadCaptureInfoToUpdate.CooperateCashBookSize = otherLeadCaptureInfoReceivingDTO.CooperateCashBookSize;
            otherLeadCaptureInfoToUpdate.CooperateBalanceSheetSize = otherLeadCaptureInfoReceivingDTO.CooperateBalanceSheetSize;

            otherLeadCaptureInfoToUpdate.IndividualDisposableIncome = otherLeadCaptureInfoReceivingDTO.IndividualDisposableIncome;
            otherLeadCaptureInfoToUpdate.IndividualEstimatedAnnualIncome = otherLeadCaptureInfoReceivingDTO.IndividualEstimatedAnnualIncome;
            otherLeadCaptureInfoToUpdate.IndividualResidenceSize = otherLeadCaptureInfoReceivingDTO.IndividualResidenceSize;

            var updatedOtherLeadCaptureInfo = await _otherLeadCaptureInfoRepo.UpdateOtherLeadCaptureInfo(otherLeadCaptureInfoToUpdate);

            summary += $"Details after change, \n {updatedOtherLeadCaptureInfo.ToString()} \n";

            if (updatedOtherLeadCaptureInfo == null)
            {
                return new ApiResponse(500);
            }
            ModificationHistory history = new ModificationHistory(){
                ModelChanged = "OtherLeadCaptureInfo",
                ChangeSummary = summary,
                ChangedById = context.GetLoggedInUserId(),
                ModifiedModelId = updatedOtherLeadCaptureInfo.Id
            };

            await _historyRepo.SaveHistory(history);

            var otherLeadCaptureInfoTransferDTOs = _mapper.Map<OtherLeadCaptureInfoTransferDTO>(updatedOtherLeadCaptureInfo);
            return new ApiOkResponse(otherLeadCaptureInfoTransferDTOs);

        }

        public async Task<ApiResponse> DeleteOtherLeadCaptureInfo(long id)
        {
            var otherLeadCaptureInfoToDelete = await _otherLeadCaptureInfoRepo.FindOtherLeadCaptureInfoById(id);
            if (otherLeadCaptureInfoToDelete == null)
            {
                return new ApiResponse(404);
            }

            if (!await _otherLeadCaptureInfoRepo.DeleteOtherLeadCaptureInfo(otherLeadCaptureInfoToDelete))
            {
                return new ApiResponse(500);
            }

            return new ApiOkResponse(true);
        }
    }
}