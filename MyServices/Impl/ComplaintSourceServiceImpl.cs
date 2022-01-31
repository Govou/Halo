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
    public class ComplaintSourceServiceImpl : IComplaintSourceService
    {
        private readonly ILogger<ComplaintSourceServiceImpl> _logger;
        private readonly IModificationHistoryRepository _historyRepo;
        private readonly IComplaintSourceRepository _complaintSourceRepo;
        private readonly IMapper _mapper;

        public ComplaintSourceServiceImpl(IModificationHistoryRepository historyRepo, IComplaintSourceRepository ComplaintSourceRepo, ILogger<ComplaintSourceServiceImpl> logger, IMapper mapper)
        {
            this._mapper = mapper;
            this._historyRepo = historyRepo;
            this._complaintSourceRepo = ComplaintSourceRepo;
            this._logger = logger;
        }

        public async Task<ApiCommonResponse> AddComplaintSource(HttpContext context, ComplaintSourceReceivingDTO complaintSourceReceivingDTO)
        {

            var complaintSource = _mapper.Map<ComplaintSource>(complaintSourceReceivingDTO);
            complaintSource.CreatedById = context.GetLoggedInUserId();
            var savedcomplaintSource = await _complaintSourceRepo.SaveComplaintSource(complaintSource);
            if (savedcomplaintSource == null)
            {
                return CommonResponse.Send(ResponseCodes.FAILURE, null, "Some system errors occurred");
            }
            var complaintSourceTransferDTO = _mapper.Map<ComplaintSourceTransferDTO>(complaintSource);
            return CommonResponse.Send(ResponseCodes.SUCCESS,complaintSourceTransferDTO);
        }

        public async Task<ApiCommonResponse> DeleteComplaintSource(long id)
        {
            var complaintSourceToDelete = await _complaintSourceRepo.FindComplaintSourceById(id);
            if (complaintSourceToDelete == null)
            {
                return CommonResponse.Send(ResponseCodes.NO_DATA_AVAILABLE);;
            }

            if (!await _complaintSourceRepo.DeleteComplaintSource(complaintSourceToDelete))
            {
                return CommonResponse.Send(ResponseCodes.FAILURE, null, "Some system errors occurred");
            }

            return CommonResponse.Send(ResponseCodes.SUCCESS);
        }

        public async Task<ApiCommonResponse> GetAllComplaintSource()
        {
            var complaintSources = await _complaintSourceRepo.FindAllComplaintSources();
            if (complaintSources == null)
            {
                return CommonResponse.Send(ResponseCodes.NO_DATA_AVAILABLE);;
            }
            var complaintSourceTransferDTO = _mapper.Map<IEnumerable<ComplaintSourceTransferDTO>>(complaintSources);
            return CommonResponse.Send(ResponseCodes.SUCCESS,complaintSourceTransferDTO);
        }

        public async Task<ApiCommonResponse> GetComplaintSourceById(long id)
        {
            var complaintSource = await _complaintSourceRepo.FindComplaintSourceById(id);
            if (complaintSource == null)
            {
                return CommonResponse.Send(ResponseCodes.NO_DATA_AVAILABLE);;
            }
            var complaintSourceTransferDTOs = _mapper.Map<ComplaintSourceTransferDTO>(complaintSource);
            return CommonResponse.Send(ResponseCodes.SUCCESS,complaintSourceTransferDTOs);
        }

        public async Task<ApiCommonResponse> GetComplaintSourceByName(string name)
        {
            var complaintSource = await _complaintSourceRepo.FindComplaintSourceByName(name);
            if (complaintSource == null)
            {
                return CommonResponse.Send(ResponseCodes.NO_DATA_AVAILABLE);;
            }
            var complaintSourceTransferDTOs = _mapper.Map<ComplaintSourceTransferDTO>(complaintSource);
            return CommonResponse.Send(ResponseCodes.SUCCESS,complaintSourceTransferDTOs);
        }

        public async Task<ApiCommonResponse> UpdateComplaintSource(HttpContext context, long id, ComplaintSourceReceivingDTO complaintSourceReceivingDTO)
        {
            var complaintSourceToUpdate = await _complaintSourceRepo.FindComplaintSourceById(id);
            if (complaintSourceToUpdate == null)
            {
                return CommonResponse.Send(ResponseCodes.NO_DATA_AVAILABLE);;
            }

            var summary = $"Initial details before change, \n {complaintSourceToUpdate.ToString()} \n";

            complaintSourceToUpdate.Caption = complaintSourceReceivingDTO.Caption;
            complaintSourceToUpdate.Description = complaintSourceReceivingDTO.Description;
            var updatedcomplaintSource = await _complaintSourceRepo.UpdateComplaintSource(complaintSourceToUpdate);

            summary += $"Details after change, \n {updatedcomplaintSource.ToString()} \n";

            if (updatedcomplaintSource == null)
            {
                return CommonResponse.Send(ResponseCodes.FAILURE, null, "Some system errors occurred");
            }
            ModificationHistory history = new ModificationHistory()
            {
                ModelChanged = "complaintSource",
                ChangeSummary = summary,
                ChangedById = context.GetLoggedInUserId(),
                ModifiedModelId = updatedcomplaintSource.Id
            };
            await _historyRepo.SaveHistory(history);

            var complaintSourceTransferDTOs = _mapper.Map<ComplaintSourceTransferDTO>(updatedcomplaintSource);
            return CommonResponse.Send(ResponseCodes.SUCCESS,complaintSourceTransferDTOs);
        }
    }
}
