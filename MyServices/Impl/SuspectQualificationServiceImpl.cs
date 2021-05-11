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
using HalobizMigrations.Models.Halobiz;

namespace HaloBiz.MyServices.Impl
{
    public class SuspectQualificationServiceImpl : ISuspectQualificationService
    {
        private readonly ILogger<SuspectQualificationServiceImpl> _logger;
        private readonly IModificationHistoryRepository _historyRepo;
        private readonly ISuspectQualificationRepository _suspectQualificationRepo;
        private readonly IMapper _mapper;

        public SuspectQualificationServiceImpl(IModificationHistoryRepository historyRepo, ISuspectQualificationRepository SuspectQualificationRepo, ILogger<SuspectQualificationServiceImpl> logger, IMapper mapper)
        {
            this._mapper = mapper;
            this._historyRepo = historyRepo;
            this._suspectQualificationRepo = SuspectQualificationRepo;
            this._logger = logger;
        }

        public async Task<ApiResponse> AddSuspectQualification(HttpContext context, SuspectQualificationReceivingDTO suspectQualificationReceivingDTO)
        {

            var suspectQualification = _mapper.Map<SuspectQualification>(suspectQualificationReceivingDTO);
            suspectQualification.CreatedById = context.GetLoggedInUserId();
            var savedsuspectQualification = await _suspectQualificationRepo.SaveSuspectQualification(suspectQualification);
            if (savedsuspectQualification == null)
            {
                return new ApiResponse(500);
            }
            var suspectQualificationTransferDTO = _mapper.Map<SuspectQualificationTransferDTO>(suspectQualification);
            return new ApiOkResponse(suspectQualificationTransferDTO);
        }

        public async Task<ApiResponse> DeleteSuspectQualification(long id)
        {
            var suspectQualificationToDelete = await _suspectQualificationRepo.FindSuspectQualificationById(id);
            if (suspectQualificationToDelete == null)
            {
                return new ApiResponse(404);
            }

            if (!await _suspectQualificationRepo.DeleteSuspectQualification(suspectQualificationToDelete))
            {
                return new ApiResponse(500);
            }

            return new ApiOkResponse(true);
        }

        public async Task<ApiResponse> GetAllSuspectQualification()
        {
            var suspectQualifications = await _suspectQualificationRepo.FindAllSuspectQualifications();
            if (suspectQualifications == null)
            {
                return new ApiResponse(404);
            }
            var suspectQualificationTransferDTO = _mapper.Map<IEnumerable<SuspectQualificationTransferDTO>>(suspectQualifications);
            return new ApiOkResponse(suspectQualificationTransferDTO);
        }

        public async Task<ApiResponse> GetSuspectQualificationById(long id)
        {
            var suspectQualification = await _suspectQualificationRepo.FindSuspectQualificationById(id);
            if (suspectQualification == null)
            {
                return new ApiResponse(404);
            }
            var suspectQualificationTransferDTOs = _mapper.Map<SuspectQualificationTransferDTO>(suspectQualification);
            return new ApiOkResponse(suspectQualificationTransferDTOs);
        }

        /*public async Task<ApiResponse> GetSuspectQualificationByName(string name)
        {
            var suspectQualification = await _suspectQualificationRepo.FindSuspectQualificationByName(name);
            if (suspectQualification == null)
            {
                return new ApiResponse(404);
            }
            var suspectQualificationTransferDTOs = _mapper.Map<SuspectQualificationTransferDTO>(suspectQualification);
            return new ApiOkResponse(suspectQualificationTransferDTOs);
        }*/

        public async Task<ApiResponse> UpdateSuspectQualification(HttpContext context, long id, SuspectQualificationReceivingDTO suspectQualificationReceivingDTO)
        {
            var suspectQualificationToUpdate = await _suspectQualificationRepo.FindSuspectQualificationById(id);
            if (suspectQualificationToUpdate == null)
            {
                return new ApiResponse(404);
            }

            var summary = $"Initial details before change, \n {suspectQualificationToUpdate.ToString()} \n";

            suspectQualificationToUpdate.Plan = suspectQualificationReceivingDTO.Plan;
            suspectQualificationToUpdate.Goal = suspectQualificationReceivingDTO.Goal;
            var updatedsuspectQualification = await _suspectQualificationRepo.UpdateSuspectQualification(suspectQualificationToUpdate);

            summary += $"Details after change, \n {updatedsuspectQualification.ToString()} \n";

            if (updatedsuspectQualification == null)
            {
                return new ApiResponse(500);
            }
            ModificationHistory history = new ModificationHistory()
            {
                ModelChanged = "suspectQualification",
                ChangeSummary = summary,
                ChangedById = context.GetLoggedInUserId(),
                ModifiedModelId = updatedsuspectQualification.Id
            };
            await _historyRepo.SaveHistory(history);

            var suspectQualificationTransferDTOs = _mapper.Map<SuspectQualificationTransferDTO>(updatedsuspectQualification);
            return new ApiOkResponse(suspectQualificationTransferDTOs);
        }
    }
}
