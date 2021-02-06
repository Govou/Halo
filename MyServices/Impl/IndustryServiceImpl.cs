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
        public async  Task<ApiResponse> AddIndustry(HttpContext context, IndustryReceivingDTO industryReceivingDTO)
        {
            var industry = _mapper.Map<Industry>(industryReceivingDTO);
            industry.CreatedById = context.GetLoggedInUserId();
            var savedIndustry = await _industryRepo.SaveIndustry(industry);
            if (savedIndustry == null)
            {
                return new ApiResponse(500);
            }
            var industryTransferDTO = _mapper.Map<IndustryTransferDTO>(industry);
            return new ApiOkResponse(industryTransferDTO);
        }

        public async Task<ApiResponse> DeleteIndustry(long id)
        {
            var industryToDelete = await _industryRepo.FindIndustryById(id);
            if(industryToDelete == null)
            {
                return new ApiResponse(404);
            }
            if (!await _industryRepo.DeleteIndustry(industryToDelete))
            {
                return new ApiResponse(500);
            }

            return new ApiOkResponse(true);
        }

        public async Task<ApiResponse> GetAllIndustry()
        {
            var industry = await _industryRepo.GetIndustries();
            if (industry == null)
            {
                return new ApiResponse(404);
            }
            var industryTransferDTO = _mapper.Map<IEnumerable<IndustryTransferDTO>>(industry);
            return new ApiOkResponse(industryTransferDTO);
        }

        public  async Task<ApiResponse> UpdateIndustry(HttpContext context, long id, IndustryReceivingDTO industryReceivingDTO)
        {
            var industryToUpdate = await _industryRepo.FindIndustryById(id);
            if (industryToUpdate == null)
            {
                return new ApiResponse(404);
            }
            
            var summary = $"Initial details before change, \n {industryToUpdate.ToString()} \n" ;

            industryToUpdate.Caption = industryReceivingDTO.Caption;
            industryToUpdate.Description = industryReceivingDTO.Description;
            var updatedIndustry = await _industryRepo.UpdateIndustry(industryToUpdate);

            summary += $"Details after change, \n {updatedIndustry.ToString()} \n";

            if (updatedIndustry == null)
            {
                return new ApiResponse(500);
            }
            ModificationHistory history = new ModificationHistory(){
                ModelChanged = "Industry",
                ChangeSummary = summary,
                ChangedById = context.GetLoggedInUserId(),
                ModifiedModelId = updatedIndustry.Id
            };

            await _historyRepo.SaveHistory(history);

            var industryTransferDTOs = _mapper.Map<IndustryTransferDTO>(updatedIndustry);
            return new ApiOkResponse(industryTransferDTOs);
        }
    }
}