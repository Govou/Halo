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
using HalobizMigrations.Data;
using Microsoft.EntityFrameworkCore;

namespace HaloBiz.MyServices.Impl
{
    public class SuspectQualificationServiceImpl : ISuspectQualificationService
    {
        private readonly ILogger<SuspectQualificationServiceImpl> _logger;
        private readonly IModificationHistoryRepository _historyRepo;
        private readonly ISuspectQualificationRepository _suspectQualificationRepo;
        private readonly HalobizContext _context;
        private readonly IMapper _mapper;

        public SuspectQualificationServiceImpl(IModificationHistoryRepository historyRepo,
            ISuspectQualificationRepository SuspectQualificationRepo, 
            HalobizContext context,
            ILogger<SuspectQualificationServiceImpl> logger, 
            IMapper mapper)
        {
            this._mapper = mapper;
            this._historyRepo = historyRepo;
            this._suspectQualificationRepo = SuspectQualificationRepo;
            _context = context;
            this._logger = logger;
        }

        public async Task<ApiResponse> AddSuspectQualification(HttpContext context, SuspectQualificationReceivingDTO suspectQualificationReceivingDTO)
        {

            var existingQualifications = await _context.SuspectQualifications
                                            .Where(x => x.SuspectId == suspectQualificationReceivingDTO.SuspectId && !x.IsDeleted)
                                            .ToListAsync();
            
            if(existingQualifications != null && existingQualifications.Count > 1)
            {
                foreach (var existingQualification in existingQualifications)
                {
                    existingQualification.IsActive = false;
                }

                _context.SuspectQualifications.UpdateRange(existingQualifications);
            }

            var loggedInUserId = context.GetLoggedInUserId();

            var suspectQualification = _mapper.Map<SuspectQualification>(suspectQualificationReceivingDTO);

            foreach (var service in suspectQualification.ServiceQualifications)
            {
                service.CreatedById = loggedInUserId;
            }

            suspectQualification.CreatedById = loggedInUserId;   
            suspectQualification.IsActive = true;
            suspectQualification.ChallengeCompleted = true;

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

            foreach (var qualification in suspectQualificationTransferDTO)
            {
                if (qualification.AuthorityCompleted)
                {
                    var totalScore = qualification.ChallengeScore + qualification.TimingScore +
                        qualification.BudgetScore + qualification.AuthorityScore;

                    if (totalScore == 10)
                    {
                        qualification.Rank = "Encourage";
                    }
                    else if (totalScore == 30)
                    {
                        qualification.Rank = "Target";
                    }
                    else if (totalScore == 20)
                    {
                        qualification.Rank = "KIV";
                    }
                }
            }

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
            var suspectQualificationToUpdate = await _context.SuspectQualifications
                                                        .Include(x => x.ServiceQualifications)
                                                        .Where(x => !x.IsDeleted && x.Id == id)
                                                        .SingleOrDefaultAsync();
            
            if (suspectQualificationToUpdate == null)
            {
                return new ApiResponse(404);
            }

            var summary = $"Initial details before change, \n {suspectQualificationToUpdate.ToString()} \n";

            suspectQualificationToUpdate.EmotionalDisposition = suspectQualificationReceivingDTO.EmotionalDisposition;
            suspectQualificationToUpdate.AuthorityScore = suspectQualificationReceivingDTO.AuthorityScore;
            suspectQualificationToUpdate.BudgetScore = suspectQualificationReceivingDTO.BudgetScore;
            suspectQualificationToUpdate.TimingScore = suspectQualificationReceivingDTO.TimingScore;
            suspectQualificationToUpdate.ChallengeScore = suspectQualificationReceivingDTO.ChallengeScore;
            suspectQualificationToUpdate.AuthorityCompleted = suspectQualificationReceivingDTO.AuthorityCompleted;
            suspectQualificationToUpdate.BudgetCompleted = suspectQualificationReceivingDTO.BudgetCompleted;
            suspectQualificationToUpdate.TimingCompleted = suspectQualificationReceivingDTO.TimingCompleted;
            suspectQualificationToUpdate.ChallengeCompleted = suspectQualificationReceivingDTO.ChallengeCompleted;
            suspectQualificationToUpdate.OwnersExists = suspectQualificationReceivingDTO.OwnersExists;
            suspectQualificationToUpdate.GateKeeperExists = suspectQualificationReceivingDTO.GateKeeperExists;
            suspectQualificationToUpdate.InfluencerExists = suspectQualificationReceivingDTO.InfluencerExists;
            suspectQualificationToUpdate.DecisionMakerExists = suspectQualificationReceivingDTO.DecisionMakerExists;           
            suspectQualificationToUpdate.UpcomingEvents = suspectQualificationReceivingDTO.UpcomingEvents;

            if(suspectQualificationReceivingDTO.ServiceQualifications != null && suspectQualificationReceivingDTO.ServiceQualifications.Count > 1)
            {
                var serviceQualifications = await _context.ServiceQualifications
                                                    .Where(x => !x.IsDeleted && x.SuspectQualificationId == suspectQualificationToUpdate.Id)
                                                    .ToListAsync();

                foreach (var serviceQualificationToUpdate in suspectQualificationToUpdate.ServiceQualifications)
                {
                    var serviceQualificationDTO = suspectQualificationReceivingDTO.ServiceQualifications.SingleOrDefault(x => x.Id == serviceQualificationToUpdate.Id);

                    serviceQualificationToUpdate.DateToStart = serviceQualificationDTO.DateToStart;
                    serviceQualificationToUpdate.QuantityEstimate = serviceQualificationDTO.QuantityEstimate;
                    serviceQualificationToUpdate.EstimatedDurationInMonths = serviceQualificationDTO.EstimatedDurationInMonths;
                    serviceQualificationToUpdate.Budget = serviceQualificationDTO.Budget;
                }
            }

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
