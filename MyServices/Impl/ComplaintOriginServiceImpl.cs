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

        public async Task<ApiResponse> AddComplaintOrigin(HttpContext context, ComplaintOriginReceivingDTO complaintOriginReceivingDTO)
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
                return new ApiResponse(500);
            }
            var complaintOriginTransferDTO = _mapper.Map<ComplaintOriginTransferDTO>(complaintOrigin);
            return new ApiOkResponse(complaintOriginTransferDTO);
        }

        public async Task<ApiResponse> DeleteComplaintOrigin(long id)
        {
            var complaintOriginToDelete = await _complaintOriginRepo.FindComplaintOriginById(id);
            if (complaintOriginToDelete == null)
            {
                return new ApiResponse(404);
            }

            if (!await _complaintOriginRepo.DeleteComplaintOrigin(complaintOriginToDelete))
            {
                return new ApiResponse(500);
            }

            return new ApiOkResponse(true);
        }

        public async Task<ApiResponse> GetAllComplaintOrigin()
        {
            var complaintOrigins = await _complaintOriginRepo.FindAllComplaintOrigins();
            if (complaintOrigins == null)
            {
                return new ApiResponse(404);
            }
            var complaintOriginTransferDTO = _mapper.Map<IEnumerable<ComplaintOriginTransferDTO>>(complaintOrigins);
            return new ApiOkResponse(complaintOriginTransferDTO);
        }

        public async Task<ApiResponse> GetComplaintOriginById(long id)
        {
            var complaintOrigin = await _complaintOriginRepo.FindComplaintOriginById(id);
            if (complaintOrigin == null)
            {
                return new ApiResponse(404);
            }
            var complaintOriginTransferDTOs = _mapper.Map<ComplaintOriginTransferDTO>(complaintOrigin);
            return new ApiOkResponse(complaintOriginTransferDTOs);
        }

        public async Task<ApiResponse> GetComplaintOriginByName(string name)
        {
            var complaintOrigin = await _complaintOriginRepo.FindComplaintOriginByName(name);
            if (complaintOrigin == null)
            {
                return new ApiResponse(404);
            }
            var complaintOriginTransferDTOs = _mapper.Map<ComplaintOriginTransferDTO>(complaintOrigin);
            return new ApiOkResponse(complaintOriginTransferDTOs);
        }

        public async Task<ApiResponse> UpdateComplaintOrigin(HttpContext context, long id, ComplaintOriginReceivingDTO complaintOriginReceivingDTO)
        {
            var complaintOriginToUpdate = await _complaintOriginRepo.FindComplaintOriginById(id);
            if (complaintOriginToUpdate == null)
            {
                return new ApiResponse(404);
            }

            var summary = $"Initial details before change, \n {complaintOriginToUpdate.ToString()} \n";

            complaintOriginToUpdate.Caption = complaintOriginReceivingDTO.Caption;
            complaintOriginToUpdate.Description = complaintOriginReceivingDTO.Description;
            var updatedcomplaintOrigin = await _complaintOriginRepo.UpdateComplaintOrigin(complaintOriginToUpdate);

            summary += $"Details after change, \n {updatedcomplaintOrigin.ToString()} \n";

            if (updatedcomplaintOrigin == null)
            {
                return new ApiResponse(500);
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
