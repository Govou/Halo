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
    public class ModeOfTransportServiceImpl : IModeOfTransportService
    {
        private readonly IModificationHistoryRepository _historyRepo;
        private readonly IModeOfTransportRepository _modeOfTransportRepo;
        private readonly IMapper _mapper;

        public ModeOfTransportServiceImpl(IModificationHistoryRepository historyRepo, IMapper mapper, IModeOfTransportRepository modeOfTransportRepo)
        {
            this._mapper = mapper;
            this._historyRepo = historyRepo;
            this._modeOfTransportRepo = modeOfTransportRepo;
        }

        public async Task<ApiCommonResponse> AddModeOfTransport(HttpContext context, ModeOfTransportReceivingDTO modeOfTransportReceivingDTO)
        {
            var modeOfTransport = _mapper.Map<ModeOfTransport>(modeOfTransportReceivingDTO);
            modeOfTransport.CreatedById = context.GetLoggedInUserId();
            var savedModeOfTransport = await _modeOfTransportRepo.SaveModeOfTransport(modeOfTransport);
            if (savedModeOfTransport == null)
            {
                return CommonResponse.Send(ResponseCodes.FAILURE, null, "Some system errors occurred");
            }
            var modeOfTransportTransferDTO = _mapper.Map<ModeOfTransportTransferDTO>(modeOfTransport);
            return new ApiOkResponse(modeOfTransportTransferDTO);
        }

        public async Task<ApiCommonResponse> GetAllModeOfTransport()
        {
            var modeOfTransports = await _modeOfTransportRepo.FindAllModeOfTransport();
            if (modeOfTransports == null)
            {
                return CommonResponse.Send(ResponseCodes.NO_DATA_AVAILABLE);;
            }
            var modeOfTransportTransferDTO = _mapper.Map<IEnumerable<ModeOfTransportTransferDTO>>(modeOfTransports);
            return new ApiOkResponse(modeOfTransportTransferDTO);
        }

        public async Task<ApiCommonResponse> GetModeOfTransportById(long id)
        {
            var modeOfTransport = await _modeOfTransportRepo.FindModeOfTransportById(id);
            if (modeOfTransport == null)
            {
                return CommonResponse.Send(ResponseCodes.NO_DATA_AVAILABLE);;
            }
            var modeOfTransportTransferDTOs = _mapper.Map<ModeOfTransportTransferDTO>(modeOfTransport);
            return new ApiOkResponse(modeOfTransportTransferDTOs);
        }

        public async Task<ApiCommonResponse> GetModeOfTransportByName(string name)
        {
            var modeOfTransport = await _modeOfTransportRepo.FindModeOfTransportByName(name);
            if (modeOfTransport == null)
            {
                return CommonResponse.Send(ResponseCodes.NO_DATA_AVAILABLE);;
            }
            var modeOfTransportTransferDTOs = _mapper.Map<ModeOfTransportTransferDTO>(modeOfTransport);
            return new ApiOkResponse(modeOfTransportTransferDTOs);
        }

        public async Task<ApiCommonResponse> UpdateModeOfTransport(HttpContext context, long id, ModeOfTransportReceivingDTO modeOfTransportReceivingDTO)
        {
            var modeOfTransportToUpdate = await _modeOfTransportRepo.FindModeOfTransportById(id);
            if (modeOfTransportToUpdate == null)
            {
                return CommonResponse.Send(ResponseCodes.NO_DATA_AVAILABLE);;
            }
            
            var summary = $"Initial details before change, \n {modeOfTransportToUpdate.ToString()} \n" ;

            modeOfTransportToUpdate.Caption = modeOfTransportReceivingDTO.Caption;
            modeOfTransportToUpdate.Description = modeOfTransportReceivingDTO.Description;
            var updatedModeOfTransport = await _modeOfTransportRepo.UpdateModeOfTransport(modeOfTransportToUpdate);

            summary += $"Details after change, \n {updatedModeOfTransport.ToString()} \n";

            if (updatedModeOfTransport == null)
            {
                return CommonResponse.Send(ResponseCodes.FAILURE, null, "Some system errors occurred");
            }
            ModificationHistory history = new ModificationHistory(){
                ModelChanged = "ModeOfTransport",
                ChangeSummary = summary,
                ChangedById = context.GetLoggedInUserId(),
                ModifiedModelId = updatedModeOfTransport.Id
            };

            await _historyRepo.SaveHistory(history);

            var modeOfTransportTransferDTOs = _mapper.Map<ModeOfTransportTransferDTO>(updatedModeOfTransport);
            return new ApiOkResponse(modeOfTransportTransferDTOs);

        }

        public async Task<ApiCommonResponse> DeleteModeOfTransport(long id)
        {
            var modeOfTransportToDelete = await _modeOfTransportRepo.FindModeOfTransportById(id);
            if (modeOfTransportToDelete == null)
            {
                return CommonResponse.Send(ResponseCodes.NO_DATA_AVAILABLE);;
            }

            if (!await _modeOfTransportRepo.DeleteModeOfTransport(modeOfTransportToDelete))
            {
                return CommonResponse.Send(ResponseCodes.FAILURE, null, "Some system errors occurred");
            }

            return CommonResponse.Send(ResponseCodes.SUCCESS);
        }
    }
}