using AutoMapper;
using Halobiz.Common.DTOs.ApiDTOs;
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
using Halobiz.Common.DTOs.TransferDTOs;

namespace HaloBiz.MyServices.Impl
{
    public class EscalationMatrixServiceImpl : IEscalationMatrixService
    {
        private readonly ILogger<EscalationMatrixServiceImpl> _logger;
        private readonly IModificationHistoryRepository _historyRepo;
        private readonly IEscalationMatrixRepository _escalationMatrixRepo;
        private readonly HalobizContext _context;
        private readonly IMapper _mapper;

        public EscalationMatrixServiceImpl(IModificationHistoryRepository historyRepo, 
            IEscalationMatrixRepository EscalationMatrixRepo, 
            HalobizContext context,
            ILogger<EscalationMatrixServiceImpl> logger, 
            IMapper mapper)
        {
            this._mapper = mapper;
            this._historyRepo = historyRepo;
            this._escalationMatrixRepo = EscalationMatrixRepo;
            _context = context;
            this._logger = logger;
        }

        public async Task<ApiCommonResponse> AddEscalationMatrix(HttpContext context, EscalationMatrixReceivingDTO escalationMatrixReceivingDTO)
        {
            var matrix = await _context.EscalationMatrices.SingleOrDefaultAsync(x => x.ComplaintTypeId == escalationMatrixReceivingDTO.ComplaintTypeId && x.IsDeleted == false);

            if(matrix != null)
            {
                return CommonResponse.Send(ResponseCodes.FAILURE,null, "Matrix already exists for complaint type.");
            }

            var escalationMatrix = _mapper.Map<EscalationMatrix>(escalationMatrixReceivingDTO);

            escalationMatrix.CreatedById = context.GetLoggedInUserId();

            var savedescalationMatrix = await _escalationMatrixRepo.SaveEscalationMatrix(escalationMatrix);
            if (savedescalationMatrix == null)
            {
                return CommonResponse.Send(ResponseCodes.FAILURE, null, "Some system errors occurred");
            }
            var escalationMatrixTransferDTO = _mapper.Map<EscalationMatrixTransferDTO>(escalationMatrix);
            return CommonResponse.Send(ResponseCodes.SUCCESS,escalationMatrixTransferDTO);
        }

        public async Task<ApiCommonResponse> GetHandlers(long complaintTypeId)
        {
            var handlers = new List<UserProfile>();
            var matrix = await _context.EscalationMatrices.SingleOrDefaultAsync(x => x.ComplaintTypeId == complaintTypeId);
            if(matrix == null)
            {
                return CommonResponse.Send(ResponseCodes.SUCCESS,handlers);
            }

            handlers = await _context.EscalationMatrixUserProfiles
                .Where(x => x.EscalationMatrixId == matrix.Id && x.EscalationLevel.Caption == "Handler")
                .Include(x => x.UserProfile)
                .Select(x => x.UserProfile)
                .ToListAsync();

            var handlersTransferDTO = _mapper.Map<IEnumerable<UserProfileTransferDTO>>(handlers);
            return CommonResponse.Send(ResponseCodes.SUCCESS,handlersTransferDTO);
        }

        public async Task<ApiCommonResponse> DeleteEscalationMatrix(long id)
        {
            var escalationMatrixToDelete = await _escalationMatrixRepo.FindEscalationMatrixById(id);
            if (escalationMatrixToDelete == null)
            {
                return CommonResponse.Send(ResponseCodes.NO_DATA_AVAILABLE);;
            }

            if (!await _escalationMatrixRepo.DeleteEscalationMatrix(escalationMatrixToDelete))
            {
                return CommonResponse.Send(ResponseCodes.FAILURE, null, "Some system errors occurred");
            }

            return CommonResponse.Send(ResponseCodes.SUCCESS);
        }

        public async Task<ApiCommonResponse> GetAllEscalationMatrix()
        {
            var escalationMatrixs = await _escalationMatrixRepo.FindAllEscalationMatrixs();
            if (escalationMatrixs == null)
            {
                return CommonResponse.Send(ResponseCodes.NO_DATA_AVAILABLE);;
            }
            var escalationMatrixTransferDTO = _mapper.Map<IEnumerable<EscalationMatrixTransferDTO>>(escalationMatrixs);
            return CommonResponse.Send(ResponseCodes.SUCCESS,escalationMatrixTransferDTO);
        }

        public async Task<ApiCommonResponse> GetEscalationMatrixById(long id)
        {
            var escalationMatrix = await _escalationMatrixRepo.FindEscalationMatrixById(id);
            if (escalationMatrix == null)
            {
                return CommonResponse.Send(ResponseCodes.NO_DATA_AVAILABLE);;
            }
            var escalationMatrixTransferDTOs = _mapper.Map<EscalationMatrixTransferDTO>(escalationMatrix);
            return CommonResponse.Send(ResponseCodes.SUCCESS,escalationMatrixTransferDTOs);
        }

        /*public async Task<ApiCommonResponse> GetEscalationMatrixByName(string name)
        {
            var escalationMatrix = await _escalationMatrixRepo.FindEscalationMatrixByName(name);
            if (escalationMatrix == null)
            {
                return CommonResponse.Send(ResponseCodes.NO_DATA_AVAILABLE);;
            }
            var escalationMatrixTransferDTOs = _mapper.Map<EscalationMatrixTransferDTO>(escalationMatrix);
            return CommonResponse.Send(ResponseCodes.SUCCESS,escalationMatrixTransferDTOs);
        }*/

        public async Task<ApiCommonResponse> UpdateEscalationMatrix(HttpContext context, long id, EscalationMatrixReceivingDTO escalationMatrixReceivingDTO)
        {
            var escalationMatrixToUpdate = await _escalationMatrixRepo.FindEscalationMatrixById(id);
            if (escalationMatrixToUpdate == null)
            {
                return CommonResponse.Send(ResponseCodes.NO_DATA_AVAILABLE);;
            }

            if(escalationMatrixToUpdate.ComplaintTypeId != escalationMatrixReceivingDTO.ComplaintTypeId)
            {
                return CommonResponse.Send(ResponseCodes.FAILURE,null, "You cannot update complaint type of an escalation matrix.");
            }

            var summary = $"Initial details before change, \n {escalationMatrixToUpdate} \n";

            //escalationMatrixToUpdate.ComplaintTypeId = escalationMatrixReceivingDTO.ComplaintTypeId;
            escalationMatrixToUpdate.Level1MaxResolutionTimeInHrs = escalationMatrixReceivingDTO.Level1MaxResolutionTimeInHrs;
            escalationMatrixToUpdate.Level2MaxResolutionTimeInHrs = escalationMatrixReceivingDTO.Level2MaxResolutionTimeInHrs;
            escalationMatrixToUpdate.ComplaintAttendants = _mapper.Map<ICollection<EscalationMatrixUserProfile>>(escalationMatrixReceivingDTO.ComplaintAttendants);

            var updatedescalationMatrix = await _escalationMatrixRepo.UpdateEscalationMatrix(escalationMatrixToUpdate);

            summary += $"Details after change, \n {updatedescalationMatrix} \n";

            if (updatedescalationMatrix == null)
            {
                return CommonResponse.Send(ResponseCodes.FAILURE, null, "Some system errors occurred");
            }
            ModificationHistory history = new ModificationHistory()
            {
                ModelChanged = "EscalationMatrix",
                ChangeSummary = summary,
                ChangedById = context.GetLoggedInUserId(),
                ModifiedModelId = updatedescalationMatrix.Id
            };
            await _historyRepo.SaveHistory(history);

            var escalationMatrixTransferDTOs = _mapper.Map<EscalationMatrixTransferDTO>(updatedescalationMatrix);
            return CommonResponse.Send(ResponseCodes.SUCCESS,escalationMatrixTransferDTOs);
        }
    }
}
