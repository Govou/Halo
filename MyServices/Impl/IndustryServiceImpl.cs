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
using Microsoft.Extensions.Logging;

namespace HaloBiz.MyServices.Impl
{
    public class IndustryServiceImpl : IIndustryService
    {
        private readonly ILogger<IndustryServiceImpl> _logger;
        private readonly IModificationHistoryRepository _historyRepo;
        private readonly IIndustryRepository _industryRepo;
        private readonly IMapper _mapper;

        public IndustryServiceImpl(IModificationHistoryRepository historyRepo, IIndustryRepository industryRepo, ILogger<IndustryServiceImpl> logger, IMapper mapper)
        {
            this._mapper = mapper;
            this._historyRepo = historyRepo;
            this._industryRepo = industryRepo;
            this._logger = logger;
        }
        public async  Task<ApiCommonResponse> AddIndustry(HttpContext context, IndustryReceivingDTO industryReceivingDTO)
        {
            var industry = _mapper.Map<Industry>(industryReceivingDTO);
            industry.CreatedById = context.GetLoggedInUserId();
            var savedIndustry = await _industryRepo.SaveIndustry(industry);
            if (savedIndustry == null)
            {
                return CommonResponse.Send(ResponseCodes.FAILURE, null, "Some system errors occurred");
            }
            var industryTransferDTO = _mapper.Map<IndustryTransferDTO>(industry);
            return CommonResponse.Send(ResponseCodes.SUCCESS,industryTransferDTO);
        }

        public async Task<ApiCommonResponse> DeleteIndustry(long id)
        {
            var industryToDelete = await _industryRepo.FindIndustryById(id);
            if(industryToDelete == null)
            {
                return CommonResponse.Send(ResponseCodes.NO_DATA_AVAILABLE);;
            }
            if (!await _industryRepo.DeleteIndustry(industryToDelete))
            {
                return CommonResponse.Send(ResponseCodes.FAILURE, null, "Some system errors occurred");
            }

            return CommonResponse.Send(ResponseCodes.SUCCESS);
        }

        public async Task<ApiCommonResponse> GetAllIndustry()
        {
            var industry = await _industryRepo.GetIndustries();
            if (industry == null)
            {
                return CommonResponse.Send(ResponseCodes.NO_DATA_AVAILABLE);;
            }
            var industryTransferDTO = _mapper.Map<IEnumerable<IndustryTransferDTO>>(industry);
            return CommonResponse.Send(ResponseCodes.SUCCESS,industryTransferDTO);
        }

        public  async Task<ApiCommonResponse> UpdateIndustry(HttpContext context, long id, IndustryReceivingDTO industryReceivingDTO)
        {
            var industryToUpdate = await _industryRepo.FindIndustryById(id);
            if (industryToUpdate == null)
            {
                return CommonResponse.Send(ResponseCodes.NO_DATA_AVAILABLE);;
            }
            
            var summary = $"Initial details before change, \n {industryToUpdate.ToString()} \n" ;

            industryToUpdate.Caption = industryReceivingDTO.Caption;
            industryToUpdate.Description = industryReceivingDTO.Description;
            var updatedIndustry = await _industryRepo.UpdateIndustry(industryToUpdate);

            summary += $"Details after change, \n {updatedIndustry.ToString()} \n";

            if (updatedIndustry == null)
            {
                return CommonResponse.Send(ResponseCodes.FAILURE, null, "Some system errors occurred");
            }
            ModificationHistory history = new ModificationHistory(){
                ModelChanged = "Industry",
                ChangeSummary = summary,
                ChangedById = context.GetLoggedInUserId(),
                ModifiedModelId = updatedIndustry.Id
            };

            await _historyRepo.SaveHistory(history);

            var industryTransferDTOs = _mapper.Map<IndustryTransferDTO>(updatedIndustry);
            return CommonResponse.Send(ResponseCodes.SUCCESS,industryTransferDTOs);
        }
    }
}