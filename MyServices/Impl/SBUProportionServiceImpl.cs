using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using HaloBiz.DTOs.ApiDTOs;
using HaloBiz.DTOs.ReceivingDTOs;
using HaloBiz.DTOs.TransferDTOs;
using HaloBiz.Helpers;
using HaloBiz.Model;
using HaloBiz.Repository;
using Microsoft.AspNetCore.Http;

namespace HaloBiz.MyServices.Impl
{
    public class SBUProportionServiceImpl : ISBUProportionService
    {
        private readonly IModificationHistoryRepository _historyRepo;
        private readonly ISBUProportionRepository _sbuProportionRepo;
        private readonly IMapper _mapper;

        public SBUProportionServiceImpl(IModificationHistoryRepository historyRepo, IMapper mapper, ISBUProportionRepository sbuProportionRepo)
        {
            this._mapper = mapper;
            this._historyRepo = historyRepo;
            this._sbuProportionRepo = sbuProportionRepo;
        }

        public async Task<ApiResponse> AddSBUProportion(HttpContext context, SBUProportionReceivingDTO sBUProportionReceivingDTO)
        {
            var sBUProportion = _mapper.Map<SBUProportion>(sBUProportionReceivingDTO);
            if((sBUProportion.LeadClosureProportion + sBUProportion.LeadGenerationProportion) > 100)
            {
                return new ApiResponse(409, "Total proportion cannot exceed 100%");
            }
            sBUProportion.CreatedById = context.GetLoggedInUserId();
            var savedSBUProportion = await _sbuProportionRepo.SaveSBUProportion(sBUProportion);
            if (savedSBUProportion == null)
            {
                return new ApiResponse(500);
            }
            var sBUProportionTransferDTO = _mapper.Map<SBUProportionTransferDTO>(sBUProportion);
            return new ApiOkResponse(sBUProportionTransferDTO);
        }

        public async Task<ApiResponse> GetAllSBUProportions()
        {
            var sBUProportions = await _sbuProportionRepo.FindAllSBUProportions();
            if (sBUProportions == null)
            {
                return new ApiResponse(404);
            }
            var sBUProportionTransferDTO = _mapper.Map<IEnumerable<SBUProportionTransferDTO>>(sBUProportions);
            return new ApiOkResponse(sBUProportionTransferDTO);
        }

        public async Task<ApiResponse> GetSBUProportionById(long id)
        {
            var sBUProportion = await _sbuProportionRepo.FindSBUProportionById(id);
            if (sBUProportion == null)
            {
                return new ApiResponse(404);
            }
            var sBUProportionTransferDTO = _mapper.Map<SBUProportionTransferDTO>(sBUProportion);
            return new ApiOkResponse(sBUProportionTransferDTO);
        }


        public async Task<ApiResponse> UpdateSBUProportion(HttpContext context, long id, SBUProportionReceivingDTO sBUProportionReceivingDTO)
        {
            var sBUProportionToUpdate = await _sbuProportionRepo.FindSBUProportionById(id);
            if (sBUProportionToUpdate == null)
            {
                return new ApiResponse(404);
            }
            
            var summary = $"Initial details before change, \n {sBUProportionToUpdate.ToString()} \n" ;

            sBUProportionToUpdate.LeadClosureProportion = sBUProportionReceivingDTO.LeadClosureProportion;
            sBUProportionToUpdate.LeadGenerationProportion = sBUProportionReceivingDTO.LeadGenerationProportion;
            sBUProportionToUpdate.OperatingEntityId = sBUProportionReceivingDTO.OperatingEntityId;

            var updatedSBUProportion = await _sbuProportionRepo.UpdateSBUProportion(sBUProportionToUpdate);

            summary += $"Details after change, \n {updatedSBUProportion.ToString()} \n";

            if (updatedSBUProportion == null)
            {
                return new ApiResponse(500);
            }
            ModificationHistory history = new ModificationHistory(){
                ModelChanged = "SBUProportion",
                ChangeSummary = summary,
                ChangedById = context.GetLoggedInUserId(),
                ModifiedModelId = updatedSBUProportion.Id
            };

            await _historyRepo.SaveHistory(history);

            var SBUProportionTransferDTOs = _mapper.Map<SBUProportionTransferDTO>(updatedSBUProportion);
            return new ApiOkResponse(SBUProportionTransferDTOs);

        }

        public async Task<ApiResponse> DeleteSBUProportion(long id)
        {
            var SBUProportionToDelete = await _sbuProportionRepo.FindSBUProportionById(id);

            if (SBUProportionToDelete == null)
            {
                return new ApiResponse(404);
            }

            if (!await _sbuProportionRepo.DeleteSBUProportion(SBUProportionToDelete))
            {
                return new ApiResponse(500);
            }

            return new ApiOkResponse(true);
        }
    }
}