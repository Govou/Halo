using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using HaloBiz.DTOs.ApiDTOs;
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

        public async Task<ApiCommonResponse> AddOtherLeadCaptureInfo(HttpContext context, OtherLeadCaptureInfoReceivingDTO otherLeadCaptureInfoReceivingDTO)
        {
            var otherLeadCaptureInfo = _mapper.Map<OtherLeadCaptureInfo>(otherLeadCaptureInfoReceivingDTO);
            otherLeadCaptureInfo.CreatedById = context.GetLoggedInUserId();
            var savedOtherLeadCaptureInfo = await _otherLeadCaptureInfoRepo.SaveOtherLeadCaptureInfo(otherLeadCaptureInfo);
            if (savedOtherLeadCaptureInfo == null)
            {
                return CommonResponse.Send(ResponseCodes.FAILURE, null, "Some system errors occurred");
            }
            var otherLeadCaptureInfoTransferDTO = _mapper.Map<OtherLeadCaptureInfoTransferDTO>(savedOtherLeadCaptureInfo);
            return new ApiOkResponse(otherLeadCaptureInfoTransferDTO);
        }

        public async Task<ApiCommonResponse> GetAllOtherLeadCaptureInfo()
        {
            var otherLeadCaptureInfos = await _otherLeadCaptureInfoRepo.FindAllOtherLeadCaptureInfo();
            if (otherLeadCaptureInfos == null)
            {
                return CommonResponse.Send(ResponseCodes.NO_DATA_AVAILABLE);;
            }
            var otherLeadCaptureInfoTransferDTO = _mapper.Map<IEnumerable<OtherLeadCaptureInfoTransferDTO>>(otherLeadCaptureInfos);
            return new ApiOkResponse(otherLeadCaptureInfoTransferDTO);
        }

        public async Task<ApiCommonResponse> GetOtherLeadCaptureInfoById(long id)
        {
            var otherLeadCaptureInfo = await _otherLeadCaptureInfoRepo.FindOtherLeadCaptureInfoById(id);
            if (otherLeadCaptureInfo == null)
            {
                return CommonResponse.Send(ResponseCodes.NO_DATA_AVAILABLE);;
            }
            var otherLeadCaptureInfoTransferDTOs = _mapper.Map<OtherLeadCaptureInfoTransferDTO>(otherLeadCaptureInfo);
            return new ApiOkResponse(otherLeadCaptureInfoTransferDTOs);
        }
        public async Task<ApiCommonResponse> GetOtherLeadCaptureInfoByLeadDivisionId(long leadDivisionId)
        {
            var otherLeadCaptureInfo = await _otherLeadCaptureInfoRepo.FindOtherLeadCaptureInfoByLeadDivisionId(leadDivisionId);
            if (otherLeadCaptureInfo == null)
            {
                return CommonResponse.Send(ResponseCodes.NO_DATA_AVAILABLE);;
            }
            var otherLeadCaptureInfoTransferDTOs = _mapper.Map<OtherLeadCaptureInfoTransferDTO>(otherLeadCaptureInfo);
            return new ApiOkResponse(otherLeadCaptureInfoTransferDTOs);
        }

        public async Task<ApiCommonResponse> UpdateOtherLeadCaptureInfo(HttpContext context, long id, OtherLeadCaptureInfoReceivingDTO otherLeadCaptureInfoReceivingDTO)
        {
            var otherLeadCaptureInfoToUpdate = await _otherLeadCaptureInfoRepo.FindOtherLeadCaptureInfoById(id);
            if (otherLeadCaptureInfoToUpdate == null)
            {
                return CommonResponse.Send(ResponseCodes.NO_DATA_AVAILABLE);;
            }
            
            var summary = $"Initial details before change, \n {otherLeadCaptureInfoToUpdate.ToString()} \n" ;

            otherLeadCaptureInfoToUpdate.CooperateEstimatedAnnualIncome = otherLeadCaptureInfoReceivingDTO.CooperateEstimatedAnnualIncome;
            otherLeadCaptureInfoToUpdate.CooperateEstimatedAnnualProfit = otherLeadCaptureInfoReceivingDTO.CooperateEstimatedAnnualProfit;
            otherLeadCaptureInfoToUpdate.CooperateCashBookSize = otherLeadCaptureInfoReceivingDTO.CooperateCashBookSize;
            otherLeadCaptureInfoToUpdate.CooperateBalanceSheetSize = otherLeadCaptureInfoReceivingDTO.CooperateBalanceSheetSize;

            otherLeadCaptureInfoToUpdate.IndividualDisposableIncome = otherLeadCaptureInfoReceivingDTO.IndividualDisposableIncome;
            otherLeadCaptureInfoToUpdate.IndividualEstimatedAnnualIncome = otherLeadCaptureInfoReceivingDTO.IndividualEstimatedAnnualIncome;
            otherLeadCaptureInfoToUpdate.IndividualResidenceSize = otherLeadCaptureInfoReceivingDTO.IndividualResidenceSize;
            otherLeadCaptureInfoToUpdate.LeadDivisionId = otherLeadCaptureInfoReceivingDTO.LeadDivisionId;

            var updatedOtherLeadCaptureInfo = await _otherLeadCaptureInfoRepo.UpdateOtherLeadCaptureInfo(otherLeadCaptureInfoToUpdate);

            summary += $"Details after change, \n {updatedOtherLeadCaptureInfo.ToString()} \n";

            if (updatedOtherLeadCaptureInfo == null)
            {
                return CommonResponse.Send(ResponseCodes.FAILURE, null, "Some system errors occurred");
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

        public async Task<ApiCommonResponse> DeleteOtherLeadCaptureInfo(long id)
        {
            var otherLeadCaptureInfoToDelete = await _otherLeadCaptureInfoRepo.FindOtherLeadCaptureInfoById(id);
            if (otherLeadCaptureInfoToDelete == null)
            {
                return CommonResponse.Send(ResponseCodes.NO_DATA_AVAILABLE);;
            }

            if (!await _otherLeadCaptureInfoRepo.DeleteOtherLeadCaptureInfo(otherLeadCaptureInfoToDelete))
            {
                return CommonResponse.Send(ResponseCodes.FAILURE, null, "Some system errors occurred");
            }

            return CommonResponse.Send(ResponseCodes.SUCCESS);
        }
    }
}