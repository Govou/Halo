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

        public async Task<ApiCommonResponse> AddMeansOfIdentification(HttpContext context, MeansOfIdentificationReceivingDTO meansOfIdentificationReceivingDTO)
        {
            var meansOfIdentification = _mapper.Map<MeansOfIdentification>(meansOfIdentificationReceivingDTO);
            meansOfIdentification.CreatedById = context.GetLoggedInUserId();
            var savedMeansOfIdentification = await _meansOfIdentificationRepo.SaveMeansOfIdentification(meansOfIdentification);
            if (savedMeansOfIdentification == null)
            {
                return CommonResponse.Send(ResponseCodes.FAILURE, null, "Some system errors occurred");
            }
            var meansOfIdentificationTransferDTO = _mapper.Map<MeansOfIdentificationTransferDTO>(meansOfIdentification);
            return CommonResponse.Send(ResponseCodes.SUCCESS,meansOfIdentificationTransferDTO);
        }

        public async Task<ApiCommonResponse> GetAllMeansOfIdentification()
        {
            var meansOfIdentifications = await _meansOfIdentificationRepo.FindAllMeansOfIdentification();
            if (meansOfIdentifications == null)
            {
                return CommonResponse.Send(ResponseCodes.NO_DATA_AVAILABLE);;
            }
            var meansOfIdentificationTransferDTO = _mapper.Map<IEnumerable<MeansOfIdentificationTransferDTO>>(meansOfIdentifications);
            return CommonResponse.Send(ResponseCodes.SUCCESS,meansOfIdentificationTransferDTO);
        }

        public async Task<ApiCommonResponse> GetMeansOfIdentificationById(long id)
        {
            var meansOfIdentification = await _meansOfIdentificationRepo.FindMeansOfIdentificationById(id);
            if (meansOfIdentification == null)
            {
                return CommonResponse.Send(ResponseCodes.NO_DATA_AVAILABLE);;
            }
            var meansOfIdentificationTransferDTOs = _mapper.Map<MeansOfIdentificationTransferDTO>(meansOfIdentification);
            return CommonResponse.Send(ResponseCodes.SUCCESS,meansOfIdentificationTransferDTOs);
        }

        public async Task<ApiCommonResponse> GetMeansOfIdentificationByName(string name)
        {
            var meansOfIdentification = await _meansOfIdentificationRepo.FindMeansOfIdentificationByName(name);
            if (meansOfIdentification == null)
            {
                return CommonResponse.Send(ResponseCodes.NO_DATA_AVAILABLE);;
            }
            var meansOfIdentificationTransferDTOs = _mapper.Map<MeansOfIdentificationTransferDTO>(meansOfIdentification);
            return CommonResponse.Send(ResponseCodes.SUCCESS,meansOfIdentificationTransferDTOs);
        }

        public async Task<ApiCommonResponse> UpdateMeansOfIdentification(HttpContext context, long id, MeansOfIdentificationReceivingDTO meansOfIdentificationReceivingDTO)
        {
            var meansOfIdentificationToUpdate = await _meansOfIdentificationRepo.FindMeansOfIdentificationById(id);
            if (meansOfIdentificationToUpdate == null)
            {
                return CommonResponse.Send(ResponseCodes.NO_DATA_AVAILABLE);;
            }
            
            var summary = $"Initial details before change, \n {meansOfIdentificationToUpdate.ToString()} \n" ;

            meansOfIdentificationToUpdate.Caption = meansOfIdentificationReceivingDTO.Caption;
            meansOfIdentificationToUpdate.Description = meansOfIdentificationReceivingDTO.Description;
            var updatedMeansOfIdentification = await _meansOfIdentificationRepo.UpdateMeansOfIdentification(meansOfIdentificationToUpdate);

            summary += $"Details after change, \n {updatedMeansOfIdentification.ToString()} \n";

            if (updatedMeansOfIdentification == null)
            {
                return CommonResponse.Send(ResponseCodes.FAILURE, null, "Some system errors occurred");
            }
            ModificationHistory history = new ModificationHistory(){
                ModelChanged = "MeansOfIdentification",
                ChangeSummary = summary,
                ChangedById = context.GetLoggedInUserId(),
                ModifiedModelId = updatedMeansOfIdentification.Id
            };

            await _historyRepo.SaveHistory(history);

            var meansOfIdentificationTransferDTOs = _mapper.Map<MeansOfIdentificationTransferDTO>(updatedMeansOfIdentification);
            return CommonResponse.Send(ResponseCodes.SUCCESS,meansOfIdentificationTransferDTOs);

        }

        public async Task<ApiCommonResponse> DeleteMeansOfIdentification(long id)
        {
            var meansOfIdentificationToDelete = await _meansOfIdentificationRepo.FindMeansOfIdentificationById(id);
            if (meansOfIdentificationToDelete == null)
            {
                return CommonResponse.Send(ResponseCodes.NO_DATA_AVAILABLE);;
            }

            if (!await _meansOfIdentificationRepo.DeleteMeansOfIdentification(meansOfIdentificationToDelete))
            {
                return CommonResponse.Send(ResponseCodes.FAILURE, null, "Some system errors occurred");
            }

            return CommonResponse.Send(ResponseCodes.SUCCESS);
        }
    }
}