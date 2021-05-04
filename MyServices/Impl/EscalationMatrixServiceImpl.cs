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

        public async Task<ApiResponse> AddEscalationMatrix(HttpContext context, EscalationMatrixReceivingDTO escalationMatrixReceivingDTO)
        {
            var matrix = _context.EscalationMatrices.SingleOrDefaultAsync(x => x.ComplaintTypeId == escalationMatrixReceivingDTO.ComplaintTypeId);

            if(matrix != null)
            {
                return new ApiResponse(400, "Matrix already exists for complaint type.");
            }

            var escalationMatrix = _mapper.Map<EscalationMatrix>(escalationMatrixReceivingDTO);

            escalationMatrix.CreatedById = context.GetLoggedInUserId();

            var savedescalationMatrix = await _escalationMatrixRepo.SaveEscalationMatrix(escalationMatrix);
            if (savedescalationMatrix == null)
            {
                return new ApiResponse(500);
            }
            var escalationMatrixTransferDTO = _mapper.Map<EscalationMatrixTransferDTO>(escalationMatrix);
            return new ApiOkResponse(escalationMatrixTransferDTO);
        }

        public async Task<ApiResponse> GetHandlers(long complaintTypeId)
        {
            var handlers = new List<UserProfile>();
            var matrix = await _context.EscalationMatrices.SingleOrDefaultAsync(x => x.ComplaintTypeId == complaintTypeId);
            if(matrix == null)
            {
                return new ApiOkResponse(handlers);
            }

            handlers = await _context.EscalationMatrixUserProfiles
                .Where(x => x.EscalationMatrixId == matrix.Id && x.EscalationLevel.Caption == "Handler")
                .Include(x => x.UserProfile)
                .Select(x => x.UserProfile)
                .ToListAsync();

            var handlersTransferDTO = _mapper.Map<UserProfileTransferDTO>(handlers);
            return new ApiOkResponse(handlersTransferDTO);
        }

        public async Task<ApiResponse> DeleteEscalationMatrix(long id)
        {
            var escalationMatrixToDelete = await _escalationMatrixRepo.FindEscalationMatrixById(id);
            if (escalationMatrixToDelete == null)
            {
                return new ApiResponse(404);
            }

            if (!await _escalationMatrixRepo.DeleteEscalationMatrix(escalationMatrixToDelete))
            {
                return new ApiResponse(500);
            }

            return new ApiOkResponse(true);
        }

        public async Task<ApiResponse> GetAllEscalationMatrix()
        {
            var escalationMatrixs = await _escalationMatrixRepo.FindAllEscalationMatrixs();
            if (escalationMatrixs == null)
            {
                return new ApiResponse(404);
            }
            var escalationMatrixTransferDTO = _mapper.Map<IEnumerable<EscalationMatrixTransferDTO>>(escalationMatrixs);
            return new ApiOkResponse(escalationMatrixTransferDTO);
        }

        public async Task<ApiResponse> GetEscalationMatrixById(long id)
        {
            var escalationMatrix = await _escalationMatrixRepo.FindEscalationMatrixById(id);
            if (escalationMatrix == null)
            {
                return new ApiResponse(404);
            }
            var escalationMatrixTransferDTOs = _mapper.Map<EscalationMatrixTransferDTO>(escalationMatrix);
            return new ApiOkResponse(escalationMatrixTransferDTOs);
        }

        /*public async Task<ApiResponse> GetEscalationMatrixByName(string name)
        {
            var escalationMatrix = await _escalationMatrixRepo.FindEscalationMatrixByName(name);
            if (escalationMatrix == null)
            {
                return new ApiResponse(404);
            }
            var escalationMatrixTransferDTOs = _mapper.Map<EscalationMatrixTransferDTO>(escalationMatrix);
            return new ApiOkResponse(escalationMatrixTransferDTOs);
        }*/

        public async Task<ApiResponse> UpdateEscalationMatrix(HttpContext context, long id, EscalationMatrixReceivingDTO escalationMatrixReceivingDTO)
        {
            var escalationMatrixToUpdate = await _escalationMatrixRepo.FindEscalationMatrixById(id);
            if (escalationMatrixToUpdate == null)
            {
                return new ApiResponse(404);
            }

            var summary = $"Initial details before change, \n {escalationMatrixToUpdate.ToString()} \n";

            escalationMatrixToUpdate.ComplaintTypeId = escalationMatrixReceivingDTO.ComplaintTypeId;
            escalationMatrixToUpdate.Level1MaxResolutionTimeInHrs = escalationMatrixReceivingDTO.Level1MaxResolutionTimeInHrs;
            escalationMatrixToUpdate.Level2MaxResolutionTimeInHrs = escalationMatrixReceivingDTO.Level2MaxResolutionTimeInHrs;
            escalationMatrixToUpdate.Level3MaxResolutionTimeInHrs = escalationMatrixReceivingDTO.Level3MaxResolutionTimeInHrs;
            var updatedescalationMatrix = await _escalationMatrixRepo.UpdateEscalationMatrix(escalationMatrixToUpdate);

            summary += $"Details after change, \n {updatedescalationMatrix.ToString()} \n";

            if (updatedescalationMatrix == null)
            {
                return new ApiResponse(500);
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
            return new ApiOkResponse(escalationMatrixTransferDTOs);
        }
    }
}
