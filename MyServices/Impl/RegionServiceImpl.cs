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
    public class RegionServiceImpl : IRegionService
    {
        private readonly IModificationHistoryRepository _historyRepo;
        private readonly IRegionRepository _regionRepo;
        private readonly IMapper _mapper;

        public RegionServiceImpl(IModificationHistoryRepository historyRepo, IMapper mapper, IRegionRepository _regionRepo)
        {
            this._mapper = mapper;
            this._historyRepo = historyRepo;
            this._regionRepo = _regionRepo;
        }

        public async Task<ApiResponse> AddRegion(HttpContext context, RegionReceivingDTO regionReceivingDTO)
        {
            var region = _mapper.Map<Region>(regionReceivingDTO);
            region.CreatedById = context.GetLoggedInUserId();
            var savedRegion = await _regionRepo.SaveRegion(region);
            if (savedRegion == null)
            {
                return new ApiResponse(500);
            }
            var regionTransferDTO = _mapper.Map<RegionTransferDTO>(region);
            return new ApiOkResponse(regionTransferDTO);
        }

        public async Task<ApiResponse> GetAllRegions()
        {
            var regions = await _regionRepo.FindAllRegions();
            if (regions == null)
            {
                return new ApiResponse(404);
            }
            var regionTransferDTO = _mapper.Map<IEnumerable<RegionTransferDTO>>(regions);
            return new ApiOkResponse(regionTransferDTO);
        }

        public async Task<ApiResponse> GetRegionById(long id)
        {
            var region = await _regionRepo.FindRegionById(id);
            if (region == null)
            {
                return new ApiResponse(404);
            }
            var regionTransferDTO = _mapper.Map<RegionTransferDTO>(region);
            return new ApiOkResponse(regionTransferDTO);
        }

        public async Task<ApiResponse> GetRegionByName(string name)
        {
            var region = await _regionRepo.FindRegionByName(name);
            if (region == null)
            {
                return new ApiResponse(404);
            }
            var regionTransferDTOs = _mapper.Map<RegionTransferDTO>(region);
            return new ApiOkResponse(regionTransferDTOs);
        }

        public async Task<ApiResponse> UpdateRegion(HttpContext context, long id, RegionReceivingDTO regionReceivingDTO)
        {
            var regionToUpdate = await _regionRepo.FindRegionById(id);
            if (regionToUpdate == null)
            {
                return new ApiResponse(404);
            }
            
            var summary = $"Initial details before change, \n {regionToUpdate.ToString()} \n" ;

            regionToUpdate.Name = regionReceivingDTO.Name;
            regionToUpdate.Description = regionReceivingDTO.Description;
            regionToUpdate.HeadId = regionReceivingDTO.HeadId;
            regionToUpdate.BranchId = regionReceivingDTO.BranchId;
            var updatedRegion = await _regionRepo.UpdateRegion(regionToUpdate);

            summary += $"Details after change, \n {updatedRegion.ToString()} \n";

            if (updatedRegion == null)
            {
                return new ApiResponse(500);
            }
            ModificationHistory history = new ModificationHistory(){
                ModelChanged = "Region",
                ChangeSummary = summary,
                ChangedById = context.GetLoggedInUserId(),
                ModifiedModelId = updatedRegion.Id
            };

            await _historyRepo.SaveHistory(history);

            var regionTransferDTOs = _mapper.Map<RegionTransferDTO>(updatedRegion);
            return new ApiOkResponse(regionTransferDTOs);

        }

        public async Task<ApiResponse> DeleteRegion(long id)
        {
            var regionToDelete = await _regionRepo.FindRegionById(id);

            if (regionToDelete == null)
            {
                return new ApiResponse(404);
            }

            if (!await _regionRepo.DeleteRegion(regionToDelete))
            {
                return new ApiResponse(500);
            }

            return new ApiOkResponse(true);
        }

       
    }
}