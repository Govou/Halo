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
using HaloBiz.MyServices.LAMS;
using HaloBiz.DTOs.ReceivingDTOs.LAMS;
using HaloBiz.DTOs.TransferDTOs.LAMS;
using HalobizMigrations.Data;
using HaloBiz.Repository.LAMS;

namespace HaloBiz.MyServices.Impl
{
    public class SuspectServiceImpl : ISuspectService
    {
        private readonly ILogger<SuspectServiceImpl> _logger;
        private readonly IModificationHistoryRepository _historyRepo;
        private readonly ISuspectRepository _suspectRepo;
        private readonly ILeadService _leadService;
        private readonly ILeadDivisionRepository _leadDivisionRepo;
        private readonly HalobizContext _context;
        private readonly IMapper _mapper;

        public SuspectServiceImpl(IModificationHistoryRepository historyRepo, 
            ISuspectRepository SuspectRepo, 
            ILogger<SuspectServiceImpl> logger, 
            ILeadService leadService,
            HalobizContext context,
            ILeadDivisionRepository leadDivisionRepo,
            IMapper mapper)
        {
            this._mapper = mapper;
            this._historyRepo = historyRepo;
            this._suspectRepo = SuspectRepo;
            _leadService = leadService;
            _leadDivisionRepo = leadDivisionRepo;
            _context = context;
            this._logger = logger;
        }

        public async Task<ApiCommonResponse> AddSuspect(HttpContext context, SuspectReceivingDTO suspectReceivingDTO)
        {

            var suspect = _mapper.Map<Suspect>(suspectReceivingDTO);
            suspect.CreatedById = context.GetLoggedInUserId();
            var savedsuspect = await _suspectRepo.SaveSuspect(suspect);
            if (savedsuspect == null)
            {
                return CommonResponse.Send(ResponseCodes.FAILURE, null, "Some system errors occurred");
            }
            var suspectTransferDTO = _mapper.Map<SuspectTransferDTO>(suspect);
            return new ApiOkResponse(suspectTransferDTO);
        }

        public async Task<ApiCommonResponse> DeleteSuspect(long id)
        {
            var suspectToDelete = await _suspectRepo.FindSuspectById(id);
            if (suspectToDelete == null)
            {
                return CommonResponse.Send(ResponseCodes.NO_DATA_AVAILABLE);;
            }

            if (!await _suspectRepo.DeleteSuspect(suspectToDelete))
            {
                return CommonResponse.Send(ResponseCodes.FAILURE, null, "Some system errors occurred");
            }

            return CommonResponse.Send(ResponseCodes.SUCCESS);
        }

        public async Task<ApiCommonResponse> GetAllSuspect()
        {
            var suspects = await _suspectRepo.FindAllSuspects();
            if (suspects == null)
            {
                return CommonResponse.Send(ResponseCodes.NO_DATA_AVAILABLE);;
            }
            var suspectTransferDTO = _mapper.Map<IEnumerable<SuspectTransferDTO>>(suspects);
            return new ApiOkResponse(suspectTransferDTO);
        }

        public async Task<ApiCommonResponse> GetUserSuspects(HttpContext context)
        {
            var suspects = await _suspectRepo.FindAllUserSuspects(context.GetLoggedInUserId());
            if (suspects == null)
            {
                return CommonResponse.Send(ResponseCodes.NO_DATA_AVAILABLE);;
            }
            var suspectTransferDTO = _mapper.Map<IEnumerable<SuspectTransferDTO>>(suspects);
            return new ApiOkResponse(suspectTransferDTO);
        }

        public async Task<ApiCommonResponse> GetSuspectById(long id)
        {
            var suspect = await _suspectRepo.FindSuspectById(id);
            if (suspect == null)
            {
                return CommonResponse.Send(ResponseCodes.NO_DATA_AVAILABLE);;
            }
            var suspectTransferDTOs = _mapper.Map<SuspectTransferDTO>(suspect);

            if(suspectTransferDTOs.SuspectQualifications != null && suspectTransferDTOs.SuspectQualifications.Count > 0)
            {
                var qualification = suspectTransferDTOs.SuspectQualifications.First();
                if (qualification.AuthorityCompleted)
                {
                    var totalScore = qualification.ChallengeScore + qualification.TimingScore + 
                        qualification.BudgetScore + qualification.AuthorityScore;

                    if(totalScore == 10)
                    {
                        qualification.Rank = "Encourage";
                    }
                    else if (totalScore == 20)
                    {
                        qualification.Rank = "KIV";
                    }
                    else if(totalScore >= 30)
                    {
                        qualification.Rank = "Target";
                    }
                }
            }

            return new ApiOkResponse(suspectTransferDTOs);
        }

        /*public async Task<ApiCommonResponse> GetSuspectByName(string name)
        {
            var suspect = await _suspectRepo.FindSuspectByName(name);
            if (suspect == null)
            {
                return CommonResponse.Send(ResponseCodes.NO_DATA_AVAILABLE);;
            }
            var suspectTransferDTOs = _mapper.Map<SuspectTransferDTO>(suspect);
            return new ApiOkResponse(suspectTransferDTOs);
        }*/

        public async Task<ApiCommonResponse> UpdateSuspect(HttpContext context, long id, SuspectReceivingDTO suspectReceivingDTO)
        {
            var suspectToUpdate = await _suspectRepo.FindSuspectById(id);
            if (suspectToUpdate == null)
            {
                return CommonResponse.Send(ResponseCodes.NO_DATA_AVAILABLE);;
            }

            var summary = $"Initial details before change, \n {suspectToUpdate.ToString()} \n";

            suspectToUpdate.Address = suspectReceivingDTO.Address;
            suspectToUpdate.BranchId = suspectReceivingDTO.BranchId;

            var updatedsuspect = await _suspectRepo.UpdateSuspect(suspectToUpdate);

            summary += $"Details after change, \n {updatedsuspect.ToString()} \n";

            if (updatedsuspect == null)
            {
                return CommonResponse.Send(ResponseCodes.FAILURE, null, "Some system errors occurred");
            }
            ModificationHistory history = new ModificationHistory()
            {
                ModelChanged = "suspect",
                ChangeSummary = summary,
                ChangedById = context.GetLoggedInUserId(),
                ModifiedModelId = updatedsuspect.Id
            };
            await _historyRepo.SaveHistory(history);

            var suspectTransferDTOs = _mapper.Map<SuspectTransferDTO>(updatedsuspect);
            return new ApiOkResponse(suspectTransferDTOs);
        }

        public async Task<ApiCommonResponse> ConvertSuspect(HttpContext context, long suspectId)
        {
            var suspect = await _suspectRepo.FindSuspectById(suspectId);
            if (suspect == null)
            {
                return CommonResponse.Send(ResponseCodes.NO_DATA_AVAILABLE);;
            }

            var loggedInUserId = context.GetLoggedInUserId();

            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                var suspectName = suspect.BusinessName ?? (suspect.FirstName + ' ' + suspect.LastName);

                var leadSaveResponse = await _leadService.AddLead(context, new LeadReceivingDTO
                {
                    GroupName = suspectName,
                    GroupTypeId = suspect.GroupTypeId,
                    Industry = suspect.Industry?.Caption,
                    LeadOriginId = suspect.LeadOriginId.Value,
                    LeadTypeId = suspect.LeadTypeId ?? 1,
                    LogoUrl = suspect.ImageUrl,
                    RCNumber = suspect?.RCNumber,
                });

                if (leadSaveResponse is ApiOkResponse response)
                {
                    var lead = (LeadTransferDTO)response.Result;

                    var savedLeadDivision = await _leadDivisionRepo.SaveLeadDivision(new LeadDivision 
                    {
                        Address = suspect.Address,     
                        BranchId = suspect.BranchId,
                        CreatedById = loggedInUserId,
                        DivisionName = suspectName,
                        Email = suspect.Email,
                        Industry = suspect.Industry?.Caption,
                        LeadId = lead.Id,
                        LeadOriginId = suspect.LeadOriginId.Value,
                        LeadTypeId = suspect.LeadTypeId ?? 1,
                        Lgaid = suspect.LgaId,
                        LogoUrl = suspect.ImageUrl,
                        OfficeId = suspect.OfficeId,
                        PhoneNumber = suspect.MobileNumber,
                        Rcnumber = suspect?.RCNumber,
                        StateId = suspect.StateId,
                        Street = suspect.Street
                    });

                    if (savedLeadDivision == null)
                    {
                        return CommonResponse.Send(ResponseCodes.FAILURE, null, "Some system errors occurred");
                    }

                    suspect.LeadId = lead.Id;
                    suspect.IsConverted = true;

                    _context.Suspects.Update(suspect);
                    await _context.SaveChangesAsync();

                    await transaction.CommitAsync();
                    return new ApiOkResponse(lead.ReferenceNo);
                }
                else
                {
                    return new ApiResponse(leadSaveResponse.StatusCode, leadSaveResponse.Message);
                }
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                _logger.LogError(ex.Message);
                _logger.LogError(ex.StackTrace);
                return  CommonResponse.Send(ResponseCodes.FAILURE, null, "Some system errors occurred");
            }      
        }
    }
}
