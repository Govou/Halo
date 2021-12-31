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
    public class ZoneServiceImpl : IZoneService
    {
        private readonly IModificationHistoryRepository _historyRepo;
        private readonly IZoneRepository _zoneRepo;
        private readonly IMapper _mapper;

        public ZoneServiceImpl(IModificationHistoryRepository historyRepo, IMapper mapper, IZoneRepository _zoneRepo)
        {
            this._mapper = mapper;
            this._historyRepo = historyRepo;
            this._zoneRepo = _zoneRepo;
        }

        public async Task<ApiCommonResponse> AddZone(HttpContext context, ZoneReceivingDTO zoneReceivingDTO)
        {
            var zone = _mapper.Map<Zone>(zoneReceivingDTO);
            zone.CreatedById = context.GetLoggedInUserId();
            var savedZone = await _zoneRepo.SaveZone(zone);
            if (savedZone == null)
            {
                return CommonResponse.Send(ResponseCodes.FAILURE, null, "Some system errors occurred");
            }
            var zoneTransferDTO = _mapper.Map<ZoneTransferDTO>(zone);
            return new ApiOkResponse(zoneTransferDTO);
        }

        public async Task<ApiCommonResponse> GetAllZones()
        {
            var zones = await _zoneRepo.FindAllZones();
            if (zones == null)
            {
                return CommonResponse.Send(ResponseCodes.NO_DATA_AVAILABLE);;
            }
            var zoneTransferDTO = _mapper.Map<IEnumerable<ZoneTransferDTO>>(zones);
            return new ApiOkResponse(zoneTransferDTO);
        }

        public async Task<ApiCommonResponse> GetZoneById(long id)
        {
            var zone = await _zoneRepo.FindZoneById(id);
            if (zone == null)
            {
                return CommonResponse.Send(ResponseCodes.NO_DATA_AVAILABLE);;
            }
            var zoneTransferDTO = _mapper.Map<ZoneTransferDTO>(zone);
            return new ApiOkResponse(zoneTransferDTO);
        }

        public async Task<ApiCommonResponse> GetZoneByName(string name)
        {
            var zone = await _zoneRepo.FindZoneByName(name);
            if (zone == null)
            {
                return CommonResponse.Send(ResponseCodes.NO_DATA_AVAILABLE);;
            }
            var zoneTransferDTOs = _mapper.Map<ZoneTransferDTO>(zone);
            return new ApiOkResponse(zoneTransferDTOs);
        }

        public async Task<ApiCommonResponse> UpdateZone(HttpContext context, long id, ZoneReceivingDTO zoneReceivingDTO)
        {
            var zoneToUpdate = await _zoneRepo.FindZoneById(id);
            if (zoneToUpdate == null)
            {
                return CommonResponse.Send(ResponseCodes.NO_DATA_AVAILABLE);;
            }
            
            var summary = $"Initial details before change, \n {zoneToUpdate.ToString()} \n" ;

            zoneToUpdate.Name = zoneReceivingDTO.Name;
            zoneToUpdate.Description = zoneReceivingDTO.Description;
            zoneToUpdate.HeadId = zoneReceivingDTO.HeadId;
            zoneToUpdate.RegionId = zoneReceivingDTO.RegionId;
            zoneToUpdate.StateId = zoneReceivingDTO.StateId;
            zoneToUpdate.Lgaid = zoneReceivingDTO.LGAId;
            var updatedZone = await _zoneRepo.UpdateZone(zoneToUpdate);

            summary += $"Details after change, \n {updatedZone.ToString()} \n";

            if (updatedZone == null)
            {
                return CommonResponse.Send(ResponseCodes.FAILURE, null, "Some system errors occurred");
            }
            ModificationHistory history = new ModificationHistory(){
                ModelChanged = "Zone",
                ChangeSummary = summary,
                ChangedById = context.GetLoggedInUserId(),
                ModifiedModelId = updatedZone.Id
            };

            await _historyRepo.SaveHistory(history);

            var zoneTransferDTOs = _mapper.Map<ZoneTransferDTO>(updatedZone);
            return new ApiOkResponse(zoneTransferDTOs);

        }

        public async Task<ApiCommonResponse> DeleteZone(long id)
        {
            var zoneToDelete = await _zoneRepo.FindZoneById(id);

            if (zoneToDelete == null)
            {
                return CommonResponse.Send(ResponseCodes.NO_DATA_AVAILABLE);;
            }

            if (!await _zoneRepo.DeleteZone(zoneToDelete))
            {
                return CommonResponse.Send(ResponseCodes.FAILURE, null, "Some system errors occurred");
            }

            return CommonResponse.Send(ResponseCodes.SUCCESS);
        }

       
    }
}