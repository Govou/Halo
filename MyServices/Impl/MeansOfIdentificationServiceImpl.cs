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
    public class MeansOfIdentificationServiceImpl : IMeansOfIdentificationService
    {
        private readonly IModificationHistoryRepository _historyRepo;
        private readonly IMeansOfIdentificationRepository _meansOfIdentificationRepo;
        private readonly IMapper _mapper;

        public MeansOfIdentificationServiceImpl(IModificationHistoryRepository historyRepo, IMapper mapper, IMeansOfIdentificationRepository meansOfIdentificationRepo)
        {
            this._mapper = mapper;
            this._historyRepo = historyRepo;
            this._meansOfIdentificationRepo = meansOfIdentificationRepo;
        }

        public async Task<ApiResponse> AddMeansOfIdentification(HttpContext context, MeansOfIdentificationReceivingDTO meansOfIdentificationReceivingDTO)
        {
            var meansOfIdentification = _mapper.Map<MeansOfIdentification>(meansOfIdentificationReceivingDTO);
            meansOfIdentification.CreatedById = context.GetLoggedInUserId();
            var savedMeansOfIdentification = await _meansOfIdentificationRepo.SaveMeansOfIdentification(meansOfIdentification);
            if (savedMeansOfIdentification == null)
            {
                return new ApiResponse(500);
            }
            var meansOfIdentificationTransferDTO = _mapper.Map<MeansOfIdentificationTransferDTO>(meansOfIdentification);
            return new ApiOkResponse(meansOfIdentificationTransferDTO);
        }

        public async Task<ApiResponse> GetAllMeansOfIdentification()
        {
            var meansOfIdentifications = await _meansOfIdentificationRepo.FindAllMeansOfIdentification();
            if (meansOfIdentifications == null)
            {
                return new ApiResponse(404);
            }
            var meansOfIdentificationTransferDTO = _mapper.Map<IEnumerable<MeansOfIdentificationTransferDTO>>(meansOfIdentifications);
            return new ApiOkResponse(meansOfIdentificationTransferDTO);
        }

        public async Task<ApiResponse> GetMeansOfIdentificationById(long id)
        {
            var meansOfIdentification = await _meansOfIdentificationRepo.FindMeansOfIdentificationById(id);
            if (meansOfIdentification == null)
            {
                return new ApiResponse(404);
            }
            var meansOfIdentificationTransferDTOs = _mapper.Map<MeansOfIdentificationTransferDTO>(meansOfIdentification);
            return new ApiOkResponse(meansOfIdentificationTransferDTOs);
        }

        public async Task<ApiResponse> GetMeansOfIdentificationByName(string name)
        {
            var meansOfIdentification = await _meansOfIdentificationRepo.FindMeansOfIdentificationByName(name);
            if (meansOfIdentification == null)
            {
                return new ApiResponse(404);
            }
            var meansOfIdentificationTransferDTOs = _mapper.Map<MeansOfIdentificationTransferDTO>(meansOfIdentification);
            return new ApiOkResponse(meansOfIdentificationTransferDTOs);
        }

        public async Task<ApiResponse> UpdateMeansOfIdentification(HttpContext context, long id, MeansOfIdentificationReceivingDTO meansOfIdentificationReceivingDTO)
        {
            var meansOfIdentificationToUpdate = await _meansOfIdentificationRepo.FindMeansOfIdentificationById(id);
            if (meansOfIdentificationToUpdate == null)
            {
                return new ApiResponse(404);
            }
            
            var summary = $"Initial details before change, \n {meansOfIdentificationToUpdate.ToString()} \n" ;

            meansOfIdentificationToUpdate.Caption = meansOfIdentificationReceivingDTO.Caption;
            meansOfIdentificationToUpdate.Description = meansOfIdentificationReceivingDTO.Description;
            var updatedMeansOfIdentification = await _meansOfIdentificationRepo.UpdateMeansOfIdentification(meansOfIdentificationToUpdate);

            summary += $"Details after change, \n {updatedMeansOfIdentification.ToString()} \n";

            if (updatedMeansOfIdentification == null)
            {
                return new ApiResponse(500);
            }
            ModificationHistory history = new ModificationHistory(){
                ModelChanged = "MeansOfIdentification",
                ChangeSummary = summary,
                ChangedById = context.GetLoggedInUserId(),
                ModifiedModelId = updatedMeansOfIdentification.Id
            };

            await _historyRepo.SaveHistory(history);

            var meansOfIdentificationTransferDTOs = _mapper.Map<MeansOfIdentificationTransferDTO>(updatedMeansOfIdentification);
            return new ApiOkResponse(meansOfIdentificationTransferDTOs);

        }

        public async Task<ApiResponse> DeleteMeansOfIdentification(long id)
        {
            var meansOfIdentificationToDelete = await _meansOfIdentificationRepo.FindMeansOfIdentificationById(id);
            if (meansOfIdentificationToDelete == null)
            {
                return new ApiResponse(404);
            }

            if (!await _meansOfIdentificationRepo.DeleteMeansOfIdentification(meansOfIdentificationToDelete))
            {
                return new ApiResponse(500);
            }

            return new ApiOkResponse(true);
        }
    }
}