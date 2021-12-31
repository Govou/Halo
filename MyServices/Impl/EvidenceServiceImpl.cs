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
    public class EvidenceServiceImpl : IEvidenceService
    {
        private readonly ILogger<EvidenceServiceImpl> _logger;
        private readonly IModificationHistoryRepository _historyRepo;
        private readonly IEvidenceRepository _evidenceRepo;
        private readonly IMapper _mapper;

        public EvidenceServiceImpl(IModificationHistoryRepository historyRepo, IEvidenceRepository EvidenceRepo, ILogger<EvidenceServiceImpl> logger, IMapper mapper)
        {
            this._mapper = mapper;
            this._historyRepo = historyRepo;
            this._evidenceRepo = EvidenceRepo;
            this._logger = logger;
        }

        public async Task<ApiResponse> AddEvidence(HttpContext context, EvidenceReceivingDTO evidenceReceivingDTO)
        {

            var evidence = _mapper.Map<Evidence>(evidenceReceivingDTO);
            evidence.CreatedById = context.GetLoggedInUserId();
            var savedevidence = await _evidenceRepo.SaveEvidence(evidence);
            if (savedevidence == null)
            {
                return new ApiResponse(500);
            }
            var evidenceTransferDTO = _mapper.Map<EvidenceTransferDTO>(evidence);
            return new ApiOkResponse(evidenceTransferDTO);
        }

        public async Task<ApiResponse> DeleteEvidence(long id)
        {
            var evidenceToDelete = await _evidenceRepo.FindEvidenceById(id);
            if (evidenceToDelete == null)
            {
                return new ApiResponse(404);
            }

            if (!await _evidenceRepo.DeleteEvidence(evidenceToDelete))
            {
                return new ApiResponse(500);
            }

            return new ApiOkResponse(true);
        }

        public async Task<ApiResponse> GetAllEvidence()
        {
            var evidences = await _evidenceRepo.FindAllEvidences();
            if (evidences == null)
            {
                return new ApiResponse(404);
            }
            var evidenceTransferDTO = _mapper.Map<IEnumerable<EvidenceTransferDTO>>(evidences);
            return new ApiOkResponse(evidenceTransferDTO);
        }

        public async Task<ApiResponse> GetEvidenceById(long id)
        {
            var evidence = await _evidenceRepo.FindEvidenceById(id);
            if (evidence == null)
            {
                return new ApiResponse(404);
            }
            var evidenceTransferDTOs = _mapper.Map<EvidenceTransferDTO>(evidence);
            return new ApiOkResponse(evidenceTransferDTOs);
        }

        public async Task<ApiResponse> GetEvidenceByName(string name)
        {
            var evidence = await _evidenceRepo.FindEvidenceByName(name);
            if (evidence == null)
            {
                return new ApiResponse(404);
            }
            var evidenceTransferDTOs = _mapper.Map<EvidenceTransferDTO>(evidence);
            return new ApiOkResponse(evidenceTransferDTOs);
        }

        public async Task<ApiResponse> UpdateEvidence(HttpContext context, long id, EvidenceReceivingDTO evidenceReceivingDTO)
        {
            var evidenceToUpdate = await _evidenceRepo.FindEvidenceById(id);
            if (evidenceToUpdate == null)
            {
                return new ApiResponse(404);
            }

            var summary = $"Initial details before change, \n {evidenceToUpdate.ToString()} \n";

            evidenceToUpdate.Caption = evidenceReceivingDTO.Caption;
            evidenceToUpdate.ComplaintId = evidenceReceivingDTO.ComplaintId.Value;
            evidenceToUpdate.ComplaintStage = evidenceReceivingDTO.ComplaintStage;
            evidenceToUpdate.ImageUrl = evidenceReceivingDTO.ImageUrl;

            var updatedevidence = await _evidenceRepo.UpdateEvidence(evidenceToUpdate);

            summary += $"Details after change, \n {updatedevidence.ToString()} \n";

            if (updatedevidence == null)
            {
                return new ApiResponse(500);
            }
            ModificationHistory history = new ModificationHistory()
            {
                ModelChanged = "evidence",
                ChangeSummary = summary,
                ChangedById = context.GetLoggedInUserId(),
                ModifiedModelId = updatedevidence.Id
            };
            await _historyRepo.SaveHistory(history);

            var evidenceTransferDTOs = _mapper.Map<EvidenceTransferDTO>(updatedevidence);
            return new ApiOkResponse(evidenceTransferDTOs);
        }
    }
}
