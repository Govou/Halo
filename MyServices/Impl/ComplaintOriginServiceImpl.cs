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
using HalobizMigrations.Data;
using Microsoft.EntityFrameworkCore;

namespace HaloBiz.MyServices.Impl
{
    public class ComplaintOriginServiceImpl : IComplaintOriginService
    {
        private readonly ILogger<ComplaintOriginServiceImpl> _logger;
        private readonly IModificationHistoryRepository _historyRepo;
        private readonly IComplaintOriginRepository _complaintOriginRepo;
        private readonly HalobizContext _context;
        private readonly IMapper _mapper;

        public ComplaintOriginServiceImpl(IModificationHistoryRepository historyRepo, 
            IComplaintOriginRepository ComplaintOriginRepo, 
            HalobizContext context,
            ILogger<ComplaintOriginServiceImpl> logger, 
            IMapper mapper)
        {
            this._mapper = mapper;
            this._historyRepo = historyRepo;
            this._complaintOriginRepo = ComplaintOriginRepo;
            _context = context;
            this._logger = logger;
        }

        public async Task<ApiCommonResponse> AddComplaintOrigin(HttpContext context, ComplaintOriginReceivingDTO complaintOriginReceivingDTO)
        {
            long code = 1;
            var lastComplaintOrigin = await _context.ComplaintOrigins.OrderByDescending(x => x.Id).FirstOrDefaultAsync();

            if (lastComplaintOrigin != null)
            {
                var complaintCode = lastComplaintOrigin.Code.Split('-')[1];
                code = Convert.ToInt64(complaintCode) + 1;
            }

            var complaintOrigin = _mapper.Map<ComplaintOrigin>(complaintOriginReceivingDTO);
            complaintOrigin.CreatedById = context.GetLoggedInUserId();
            complaintOrigin.Code = $"CO-{Convert.ToString(code).PadLeft(5, '0')}";

            var savedcomplaintOrigin = await _complaintOriginRepo.SaveComplaintOrigin(complaintOrigin);
            if (savedcomplaintOrigin == null)
            {
                return CommonResponse.Send(ResponseCodes.FAILURE, null, "Some system errors occurred");
            }
            var complaintOriginTransferDTO = _mapper.Map<ComplaintOriginTransferDTO>(complaintOrigin);
            return new ApiOkResponse(complaintOriginTransferDTO);
        }

        public async Task<ApiCommonResponse> DeleteComplaintOrigin(long id)
        {
            var complaintOriginToDelete = await _complaintOriginRepo.FindComplaintOriginById(id);
            if (complaintOriginToDelete == null)
            {
                return CommonResponse.Send(ResponseCodes.NO_DATA_AVAILABLE);;
            }

            if (!await _complaintOriginRepo.DeleteComplaintOrigin(complaintOriginToDelete))
            {
                return CommonResponse.Send(ResponseCodes.FAILURE, null, "Some system errors occurred");
            }

            return CommonResponse.Send(ResponseCodes.SUCCESS);
        }

        public async Task<ApiCommonResponse> GetAllComplaintOrigin()
        {
            var complaintOrigins = await _complaintOriginRepo.FindAllComplaintOrigins();
            if (complaintOrigins == null)
            {
                return CommonResponse.Send(ResponseCodes.NO_DATA_AVAILABLE);;
            }
            var complaintOriginTransferDTO = _mapper.Map<IEnumerable<ComplaintOriginTransferDTO>>(complaintOrigins);
            return new ApiOkResponse(complaintOriginTransferDTO);
        }

        public async Task<ApiCommonResponse> GetComplaintOriginById(long id)
        {
            var complaintOrigin = await _complaintOriginRepo.FindComplaintOriginById(id);
            if (complaintOrigin == null)
            {
                return CommonResponse.Send(ResponseCodes.NO_DATA_AVAILABLE);;
            }
            var complaintOriginTransferDTOs = _mapper.Map<ComplaintOriginTransferDTO>(complaintOrigin);
            return new ApiOkResponse(complaintOriginTransferDTOs);
        }

        public async Task<ApiCommonResponse> GetComplaintOriginByName(string name)
        {
            var complaintOrigin = await _complaintOriginRepo.FindComplaintOriginByName(name);
            if (complaintOrigin == null)
            {
                return CommonResponse.Send(ResponseCodes.NO_DATA_AVAILABLE);;
            }
            var complaintOriginTransferDTOs = _mapper.Map<ComplaintOriginTransferDTO>(complaintOrigin);
            return new ApiOkResponse(complaintOriginTransferDTOs);
        }

        public async Task<ApiCommonResponse> UpdateComplaintOrigin(HttpContext context, long id, ComplaintOriginReceivingDTO complaintOriginReceivingDTO)
        {
            var complaintOriginToUpdate = await _complaintOriginRepo.FindComplaintOriginById(id);
            if (complaintOriginToUpdate == null)
            {
                return CommonResponse.Send(ResponseCodes.NO_DATA_AVAILABLE);;
            }

            var summary = $"Initial details before change, \n {complaintOriginToUpdate.ToString()} \n";

            complaintOriginToUpdate.Caption = complaintOriginReceivingDTO.Caption;
            complaintOriginToUpdate.Description = complaintOriginReceivingDTO.Description;
            var updatedcomplaintOrigin = await _complaintOriginRepo.UpdateComplaintOrigin(complaintOriginToUpdate);

            summary += $"Details after change, \n {updatedcomplaintOrigin.ToString()} \n";

            if (updatedcomplaintOrigin == null)
            {
                return CommonResponse.Send(ResponseCodes.FAILURE, null, "Some system errors occurred");
            }
            ModificationHistory history = new ModificationHistory()
            {
                ModelChanged = "complaintOrigin",
                ChangeSummary = summary,
                ChangedById = context.GetLoggedInUserId(),
                ModifiedModelId = updatedcomplaintOrigin.Id
            };
            await _historyRepo.SaveHistory(history);

            var complaintOriginTransferDTOs = _mapper.Map<ComplaintOriginTransferDTO>(updatedcomplaintOrigin);
            return new ApiOkResponse(complaintOriginTransferDTOs);
        }
    }
}
