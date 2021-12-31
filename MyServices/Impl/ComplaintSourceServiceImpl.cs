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

        public async Task<ApiResponse> AddComplaintSource(HttpContext context, ComplaintSourceReceivingDTO complaintSourceReceivingDTO)
        {

            var complaintSource = _mapper.Map<ComplaintSource>(complaintSourceReceivingDTO);
            complaintSource.CreatedById = context.GetLoggedInUserId();
            var savedcomplaintSource = await _complaintSourceRepo.SaveComplaintSource(complaintSource);
            if (savedcomplaintSource == null)
            {
                return new ApiResponse(500);
            }
            var complaintSourceTransferDTO = _mapper.Map<ComplaintSourceTransferDTO>(complaintSource);
            return new ApiOkResponse(complaintSourceTransferDTO);
        }

        public async Task<ApiResponse> DeleteComplaintSource(long id)
        {
            var complaintSourceToDelete = await _complaintSourceRepo.FindComplaintSourceById(id);
            if (complaintSourceToDelete == null)
            {
                return new ApiResponse(404);
            }

            if (!await _complaintSourceRepo.DeleteComplaintSource(complaintSourceToDelete))
            {
                return new ApiResponse(500);
            }

            return new ApiOkResponse(true);
        }

        public async Task<ApiResponse> GetAllComplaintSource()
        {
            var complaintSources = await _complaintSourceRepo.FindAllComplaintSources();
            if (complaintSources == null)
            {
                return new ApiResponse(404);
            }
            var complaintSourceTransferDTO = _mapper.Map<IEnumerable<ComplaintSourceTransferDTO>>(complaintSources);
            return new ApiOkResponse(complaintSourceTransferDTO);
        }

        public async Task<ApiResponse> GetComplaintSourceById(long id)
        {
            var complaintSource = await _complaintSourceRepo.FindComplaintSourceById(id);
            if (complaintSource == null)
            {
                return new ApiResponse(404);
            }
            var complaintSourceTransferDTOs = _mapper.Map<ComplaintSourceTransferDTO>(complaintSource);
            return new ApiOkResponse(complaintSourceTransferDTOs);
        }

        public async Task<ApiResponse> GetComplaintSourceByName(string name)
        {
            var complaintSource = await _complaintSourceRepo.FindComplaintSourceByName(name);
            if (complaintSource == null)
            {
                return new ApiResponse(404);
            }
            var complaintSourceTransferDTOs = _mapper.Map<ComplaintSourceTransferDTO>(complaintSource);
            return new ApiOkResponse(complaintSourceTransferDTOs);
        }

        public async Task<ApiResponse> UpdateComplaintSource(HttpContext context, long id, ComplaintSourceReceivingDTO complaintSourceReceivingDTO)
        {
            var complaintSourceToUpdate = await _complaintSourceRepo.FindComplaintSourceById(id);
            if (complaintSourceToUpdate == null)
            {
                return new ApiResponse(404);
            }

            var summary = $"Initial details before change, \n {complaintSourceToUpdate.ToString()} \n";

            complaintSourceToUpdate.Caption = complaintSourceReceivingDTO.Caption;
            complaintSourceToUpdate.Description = complaintSourceReceivingDTO.Description;
            var updatedcomplaintSource = await _complaintSourceRepo.UpdateComplaintSource(complaintSourceToUpdate);

            summary += $"Details after change, \n {updatedcomplaintSource.ToString()} \n";

            if (updatedcomplaintSource == null)
            {
                return new ApiResponse(500);
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
            return new ApiOkResponse(complaintSourceTransferDTOs);
        }
    }
}
