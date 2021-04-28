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
    public class ComplaintTypeServiceImpl : IComplaintTypeService
    {
        private readonly ILogger<ComplaintTypeServiceImpl> _logger;
        private readonly IModificationHistoryRepository _historyRepo;
        private readonly IComplaintTypeRepository _complaintTypeRepo;
        private readonly IMapper _mapper;

        public ComplaintTypeServiceImpl(IModificationHistoryRepository historyRepo, IComplaintTypeRepository ComplaintTypeRepo, ILogger<ComplaintTypeServiceImpl> logger, IMapper mapper)
        {
            this._mapper = mapper;
            this._historyRepo = historyRepo;
            this._complaintTypeRepo = ComplaintTypeRepo;
            this._logger = logger;
        }

        public async Task<ApiResponse> AddComplaintType(HttpContext context, ComplaintTypeReceivingDTO complaintTypeReceivingDTO)
        {

            var complaintType = _mapper.Map<ComplaintType>(complaintTypeReceivingDTO);
            complaintType.CreatedById = context.GetLoggedInUserId();
            var savedcomplaintType = await _complaintTypeRepo.SaveComplaintType(complaintType);
            if (savedcomplaintType == null)
            {
                return new ApiResponse(500);
            }
            var complaintTypeTransferDTO = _mapper.Map<ComplaintTypeTransferDTO>(complaintType);
            return new ApiOkResponse(complaintTypeTransferDTO);
        }

        public async Task<ApiResponse> DeleteComplaintType(long id)
        {
            var complaintTypeToDelete = await _complaintTypeRepo.FindComplaintTypeById(id);
            if (complaintTypeToDelete == null)
            {
                return new ApiResponse(404);
            }

            if (!await _complaintTypeRepo.DeleteComplaintType(complaintTypeToDelete))
            {
                return new ApiResponse(500);
            }

            return new ApiOkResponse(true);
        }

        public async Task<ApiResponse> GetAllComplaintType()
        {
            var complaintTypes = await _complaintTypeRepo.FindAllComplaintTypes();
            if (complaintTypes == null)
            {
                return new ApiResponse(404);
            }
            var complaintTypeTransferDTO = _mapper.Map<IEnumerable<ComplaintTypeTransferDTO>>(complaintTypes);
            return new ApiOkResponse(complaintTypeTransferDTO);
        }

        public async Task<ApiResponse> GetComplaintTypeById(long id)
        {
            var complaintType = await _complaintTypeRepo.FindComplaintTypeById(id);
            if (complaintType == null)
            {
                return new ApiResponse(404);
            }
            var complaintTypeTransferDTOs = _mapper.Map<ComplaintTypeTransferDTO>(complaintType);
            return new ApiOkResponse(complaintTypeTransferDTOs);
        }

        public async Task<ApiResponse> GetComplaintTypeByName(string name)
        {
            var complaintType = await _complaintTypeRepo.FindComplaintTypeByName(name);
            if (complaintType == null)
            {
                return new ApiResponse(404);
            }
            var complaintTypeTransferDTOs = _mapper.Map<ComplaintTypeTransferDTO>(complaintType);
            return new ApiOkResponse(complaintTypeTransferDTOs);
        }

        public async Task<ApiResponse> UpdateComplaintType(HttpContext context, long id, ComplaintTypeReceivingDTO complaintTypeReceivingDTO)
        {
            var complaintTypeToUpdate = await _complaintTypeRepo.FindComplaintTypeById(id);
            if (complaintTypeToUpdate == null)
            {
                return new ApiResponse(404);
            }

            var summary = $"Initial details before change, \n {complaintTypeToUpdate.ToString()} \n";

            complaintTypeToUpdate.Caption = complaintTypeReceivingDTO.Caption;
            complaintTypeToUpdate.Description = complaintTypeReceivingDTO.Description;
            complaintTypeToUpdate.Code = complaintTypeReceivingDTO.Code;
            var updatedcomplaintType = await _complaintTypeRepo.UpdateComplaintType(complaintTypeToUpdate);

            summary += $"Details after change, \n {updatedcomplaintType.ToString()} \n";

            if (updatedcomplaintType == null)
            {
                return new ApiResponse(500);
            }
            ModificationHistory history = new ModificationHistory()
            {
                ModelChanged = "complaintType",
                ChangeSummary = summary,
                ChangedById = context.GetLoggedInUserId(),
                ModifiedModelId = updatedcomplaintType.Id
            };
            await _historyRepo.SaveHistory(history);

            var complaintTypeTransferDTOs = _mapper.Map<ComplaintTypeTransferDTO>(updatedcomplaintType);
            return new ApiOkResponse(complaintTypeTransferDTOs);
        }
    }
}
