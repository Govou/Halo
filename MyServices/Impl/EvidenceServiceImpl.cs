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

        public async Task<ApiCommonResponse> AddEvidence(HttpContext context, EvidenceReceivingDTO evidenceReceivingDTO)
        {

            var evidence = _mapper.Map<Evidence>(evidenceReceivingDTO);
            evidence.CreatedById = context.GetLoggedInUserId();
            var savedevidence = await _evidenceRepo.SaveEvidence(evidence);
            if (savedevidence == null)
            {
                return CommonResponse.Send(ResponseCodes.FAILURE, null, "Some system errors occurred");
            }
            var evidenceTransferDTO = _mapper.Map<EvidenceTransferDTO>(evidence);
            return CommonResponse.Send(ResponseCodes.SUCCESS,evidenceTransferDTO);
        }

        public async Task<ApiCommonResponse> DeleteEvidence(long id)
        {
            var evidenceToDelete = await _evidenceRepo.FindEvidenceById(id);
            if (evidenceToDelete == null)
            {
                return CommonResponse.Send(ResponseCodes.NO_DATA_AVAILABLE);;
            }

            if (!await _evidenceRepo.DeleteEvidence(evidenceToDelete))
            {
                return CommonResponse.Send(ResponseCodes.FAILURE, null, "Some system errors occurred");
            }

            return CommonResponse.Send(ResponseCodes.SUCCESS);
        }

        public async Task<ApiCommonResponse> GetAllEvidence()
        {
            var evidences = await _evidenceRepo.FindAllEvidences();
            if (evidences == null)
            {
                return CommonResponse.Send(ResponseCodes.NO_DATA_AVAILABLE);;
            }
            var evidenceTransferDTO = _mapper.Map<IEnumerable<EvidenceTransferDTO>>(evidences);
            return CommonResponse.Send(ResponseCodes.SUCCESS,evidenceTransferDTO);
        }

        public async Task<ApiCommonResponse> GetEvidenceById(long id)
        {
            var evidence = await _evidenceRepo.FindEvidenceById(id);
            if (evidence == null)
            {
                return CommonResponse.Send(ResponseCodes.NO_DATA_AVAILABLE);;
            }
            var evidenceTransferDTOs = _mapper.Map<EvidenceTransferDTO>(evidence);
            return CommonResponse.Send(ResponseCodes.SUCCESS,evidenceTransferDTOs);
        }

        public async Task<ApiCommonResponse> GetEvidenceByName(string name)
        {
            var evidence = await _evidenceRepo.FindEvidenceByName(name);
            if (evidence == null)
            {
                return CommonResponse.Send(ResponseCodes.NO_DATA_AVAILABLE);;
            }
            var evidenceTransferDTOs = _mapper.Map<EvidenceTransferDTO>(evidence);
            return CommonResponse.Send(ResponseCodes.SUCCESS,evidenceTransferDTOs);
        }

        public async Task<ApiCommonResponse> UpdateEvidence(HttpContext context, long id, EvidenceReceivingDTO evidenceReceivingDTO)
        {
            var evidenceToUpdate = await _evidenceRepo.FindEvidenceById(id);
            if (evidenceToUpdate == null)
            {
                return CommonResponse.Send(ResponseCodes.NO_DATA_AVAILABLE);;
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
                return CommonResponse.Send(ResponseCodes.FAILURE, null, "Some system errors occurred");
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
            return CommonResponse.Send(ResponseCodes.SUCCESS,evidenceTransferDTOs);
        }
    }
}
