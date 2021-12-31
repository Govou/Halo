using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using HaloBiz.DTOs.ApiDTOs;
using HaloBiz.DTOs.ReceivingDTOs;
using HaloBiz.DTOs.TransferDTOs;
using HaloBiz.Helpers;
using HalobizMigrations.Models;
using HaloBiz.Repository;
using Microsoft.AspNetCore.Http;

namespace HaloBiz.MyServices.Impl
{
    public class SbuproportionServiceImpl : ISbuproportionService
    {
        private readonly IModificationHistoryRepository _historyRepo;
        private readonly ISbuproportionRepository _sbuProportionRepo;
        private readonly IMapper _mapper;

        public SbuproportionServiceImpl(IModificationHistoryRepository historyRepo, IMapper mapper, ISbuproportionRepository sbuProportionRepo)
        {
            this._mapper = mapper;
            this._historyRepo = historyRepo;
            this._sbuProportionRepo = sbuProportionRepo;
        }

        public async Task<ApiCommonResponse> AddSbuproportion(HttpContext context, SbuproportionReceivingDTO sBUProportionReceivingDTO)
        {
            var sBUProportion = _mapper.Map<Sbuproportion>(sBUProportionReceivingDTO);
            if((sBUProportion.LeadClosureProportion + sBUProportion.LeadGenerationProportion) > 100)
            {
                return new ApiResponse(409, "Total proportion cannot exceed 100%");
            }
            sBUProportion.CreatedById = context.GetLoggedInUserId();
            var savedSbuproportion = await _sbuProportionRepo.SaveSbuproportion(sBUProportion);
            if (savedSbuproportion == null)
            {
                return CommonResponse.Send(ResponseCodes.FAILURE, null, "Some system errors occurred");
            }
            var sBUProportionTransferDTO = _mapper.Map<SbuproportionTransferDTO>(sBUProportion);
            return new ApiOkResponse(sBUProportionTransferDTO);
        }

        public async Task<ApiCommonResponse> GetAllSbuproportions()
        {
            var sBUProportions = await _sbuProportionRepo.FindAllSbuproportions();
            if (sBUProportions == null)
            {
                return CommonResponse.Send(ResponseCodes.NO_DATA_AVAILABLE);;
            }
            var sBUProportionTransferDTO = _mapper.Map<IEnumerable<SbuproportionTransferDTO>>(sBUProportions);
            return new ApiOkResponse(sBUProportionTransferDTO);
        }

        public async Task<ApiCommonResponse> GetSbuproportionById(long id)
        {
            var sBUProportion = await _sbuProportionRepo.FindSbuproportionById(id);
            if (sBUProportion == null)
            {
                return CommonResponse.Send(ResponseCodes.NO_DATA_AVAILABLE);;
            }
            var sBUProportionTransferDTO = _mapper.Map<SbuproportionTransferDTO>(sBUProportion);
            return new ApiOkResponse(sBUProportionTransferDTO);
        }


        public async Task<ApiCommonResponse> UpdateSbuproportion(HttpContext context, long id, SbuproportionReceivingDTO sBUProportionReceivingDTO)
        {
            var sBUProportionToUpdate = await _sbuProportionRepo.FindSbuproportionById(id);
            if (sBUProportionToUpdate == null)
            {
                return CommonResponse.Send(ResponseCodes.NO_DATA_AVAILABLE);;
            }
            
            var summary = $"Initial details before change, \n {sBUProportionToUpdate.ToString()} \n" ;

            sBUProportionToUpdate.LeadClosureProportion = sBUProportionReceivingDTO.LeadClosureProportion;
            sBUProportionToUpdate.LeadGenerationProportion = sBUProportionReceivingDTO.LeadGenerationProportion;
            sBUProportionToUpdate.OperatingEntityId = sBUProportionReceivingDTO.OperatingEntityId;

            var updatedSbuproportion = await _sbuProportionRepo.UpdateSbuproportion(sBUProportionToUpdate);

            summary += $"Details after change, \n {updatedSbuproportion.ToString()} \n";

            if (updatedSbuproportion == null)
            {
                return CommonResponse.Send(ResponseCodes.FAILURE, null, "Some system errors occurred");
            }
            ModificationHistory history = new ModificationHistory(){
                ModelChanged = "Sbuproportion",
                ChangeSummary = summary,
                ChangedById = context.GetLoggedInUserId(),
                ModifiedModelId = updatedSbuproportion.Id
            };

            await _historyRepo.SaveHistory(history);

            var SbuproportionTransferDTOs = _mapper.Map<SbuproportionTransferDTO>(updatedSbuproportion);
            return new ApiOkResponse(SbuproportionTransferDTOs);

        }

        public async Task<ApiCommonResponse> DeleteSbuproportion(long id)
        {
            var SbuproportionToDelete = await _sbuProportionRepo.FindSbuproportionById(id);

            if (SbuproportionToDelete == null)
            {
                return CommonResponse.Send(ResponseCodes.NO_DATA_AVAILABLE);;
            }

            if (!await _sbuProportionRepo.DeleteSbuproportion(SbuproportionToDelete))
            {
                return CommonResponse.Send(ResponseCodes.FAILURE, null, "Some system errors occurred");
            }

            return CommonResponse.Send(ResponseCodes.SUCCESS);
        }
    }
}