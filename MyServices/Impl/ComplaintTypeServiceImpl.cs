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
    public class ComplaintTypeServiceImpl : IComplaintTypeService
    {
        private readonly ILogger<ComplaintTypeServiceImpl> _logger;
        private readonly IModificationHistoryRepository _historyRepo;
        private readonly IComplaintTypeRepository _complaintTypeRepo;
        private readonly HalobizContext _context;
        private readonly IMapper _mapper;

        public ComplaintTypeServiceImpl(IModificationHistoryRepository historyRepo, 
            IComplaintTypeRepository ComplaintTypeRepo,
            HalobizContext context,
            ILogger<ComplaintTypeServiceImpl> logger, 
            IMapper mapper)
        {
            this._mapper = mapper;
            this._historyRepo = historyRepo;
            this._complaintTypeRepo = ComplaintTypeRepo;
            _context = context;
            this._logger = logger;
        }

        public async Task<ApiCommonResponse> AddComplaintType(HttpContext context, ComplaintTypeReceivingDTO complaintTypeReceivingDTO)
        {
            long code = 1;
            var lastComplaintType = await _context.ComplaintTypes.OrderByDescending(x => x.Id).FirstOrDefaultAsync();

            if (lastComplaintType != null)
            {
                var complaintCode = lastComplaintType.Code.Split('-')[1];
                code = Convert.ToInt64(complaintCode) + 1;
            }

            var complaintType = _mapper.Map<ComplaintType>(complaintTypeReceivingDTO);
            complaintType.CreatedById = context.GetLoggedInUserId();
            complaintType.Code = $"CT-{Convert.ToString(code).PadLeft(5, '0')}";

            var savedcomplaintType = await _complaintTypeRepo.SaveComplaintType(complaintType);
            if (savedcomplaintType == null)
            {
                return CommonResponse.Send(ResponseCodes.FAILURE, null, "Some system errors occurred");
            }
            var complaintTypeTransferDTO = _mapper.Map<ComplaintTypeTransferDTO>(complaintType);
            return new ApiOkResponse(complaintTypeTransferDTO);
        }

        public async Task<ApiCommonResponse> DeleteComplaintType(long id)
        {
            var complaintTypeToDelete = await _complaintTypeRepo.FindComplaintTypeById(id);
            if (complaintTypeToDelete == null)
            {
                return CommonResponse.Send(ResponseCodes.NO_DATA_AVAILABLE);;
            }

            if (!await _complaintTypeRepo.DeleteComplaintType(complaintTypeToDelete))
            {
                return CommonResponse.Send(ResponseCodes.FAILURE, null, "Some system errors occurred");
            }

            return CommonResponse.Send(ResponseCodes.SUCCESS);
        }

        public async Task<ApiCommonResponse> GetAllComplaintType()
        {
            var complaintTypes = await _complaintTypeRepo.FindAllComplaintTypes();
            if (complaintTypes == null)
            {
                return CommonResponse.Send(ResponseCodes.NO_DATA_AVAILABLE);;
            }
            var complaintTypeTransferDTO = _mapper.Map<IEnumerable<ComplaintTypeTransferDTO>>(complaintTypes);
            return new ApiOkResponse(complaintTypeTransferDTO);
        }

        public async Task<ApiCommonResponse> GetComplaintTypeById(long id)
        {
            var complaintType = await _complaintTypeRepo.FindComplaintTypeById(id);
            if (complaintType == null)
            {
                return CommonResponse.Send(ResponseCodes.NO_DATA_AVAILABLE);;
            }
            var complaintTypeTransferDTOs = _mapper.Map<ComplaintTypeTransferDTO>(complaintType);
            return new ApiOkResponse(complaintTypeTransferDTOs);
        }

        public async Task<ApiCommonResponse> GetComplaintTypeByName(string name)
        {
            var complaintType = await _complaintTypeRepo.FindComplaintTypeByName(name);
            if (complaintType == null)
            {
                return CommonResponse.Send(ResponseCodes.NO_DATA_AVAILABLE);;
            }
            var complaintTypeTransferDTOs = _mapper.Map<ComplaintTypeTransferDTO>(complaintType);
            return new ApiOkResponse(complaintTypeTransferDTOs);
        }

        public async Task<ApiCommonResponse> UpdateComplaintType(HttpContext context, long id, ComplaintTypeReceivingDTO complaintTypeReceivingDTO)
        {
            var complaintTypeToUpdate = await _complaintTypeRepo.FindComplaintTypeById(id);
            if (complaintTypeToUpdate == null)
            {
                return CommonResponse.Send(ResponseCodes.NO_DATA_AVAILABLE);;
            }

            var summary = $"Initial details before change, \n {complaintTypeToUpdate.ToString()} \n";

            complaintTypeToUpdate.Caption = complaintTypeReceivingDTO.Caption;
            complaintTypeToUpdate.Description = complaintTypeReceivingDTO.Description;
            var updatedcomplaintType = await _complaintTypeRepo.UpdateComplaintType(complaintTypeToUpdate);

            summary += $"Details after change, \n {updatedcomplaintType.ToString()} \n";

            if (updatedcomplaintType == null)
            {
                return CommonResponse.Send(ResponseCodes.FAILURE, null, "Some system errors occurred");
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
