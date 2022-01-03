using AutoMapper;
using HaloBiz.DTOs.ApiDTOs;
using HaloBiz.DTOs.ReceivingDTOs;
using HaloBiz.DTOs.TransferDTOs;
using HaloBiz.Helpers;
using HaloBiz.Repository;
using HalobizMigrations.Models.Armada;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HaloBiz.MyServices.Impl
{
    public class PilotRegistrationServiceImpl:IPilotRegistrationService
    {
        private readonly IPilotRegistrationRepository _pilotProfileRepository;
        private readonly IMapper _mapper;

        public PilotRegistrationServiceImpl(IMapper mapper, IPilotRegistrationRepository pilotProfileRepository)
        {
            _mapper = mapper;
            _pilotProfileRepository = pilotProfileRepository;
        }

        public async Task<ApiResponse> AddPilot(HttpContext context, PilotProfileReceivingDTO pilotReceivingDTO)
        {
            var pilot = _mapper.Map<PilotProfile>(pilotReceivingDTO);
            //var NameExist = _pilotRepository.GetTypename(pilotReceivingDTO.na);
            //if (NameExist != null)
            //{
            //    return new ApiResponse(409);
            //}
            pilot.CreatedById = context.GetLoggedInUserId();
            pilot.IsDeleted = false;
            pilot.CreatedAt = DateTime.UtcNow;
            var savedRank = await _pilotProfileRepository.SavePilot(pilot);
            if (savedRank == null)
            {
                return new ApiResponse(500);
            }
            var TransferDTO = _mapper.Map<PilotProfileTransferDTO>(pilot);
            return new ApiOkResponse(TransferDTO);
        }

        public async Task<ApiResponse> AddPilotTie(HttpContext context, PilotSMORoutesResourceTieReceivingDTO pilotReceivingTieDTO)
        {
            var pilot = new PilotSMORoutesResourceTie();

            for (int i = 0; i < pilotReceivingTieDTO.SMORouteId.Length; i++)
            {
                pilot.Id = 0;
                pilot.SMORegionId = pilotReceivingTieDTO.SMORegionId;
                pilot.ResourceId = pilotReceivingTieDTO.ResourceId;
                pilot.SMORouteId = pilotReceivingTieDTO.SMORouteId[i];
                var IdExist = _pilotProfileRepository.GetResourceRegIdRegionAndRouteId(pilotReceivingTieDTO.ResourceId, pilotReceivingTieDTO.SMORouteId[i], pilotReceivingTieDTO.SMORegionId);
                if (IdExist == null)
                {
                    pilot.CreatedById = context.GetLoggedInUserId();
                    pilot.CreatedAt = DateTime.UtcNow;

                    var savedType = await _pilotProfileRepository.SavePilotTie(pilot);
                    if (savedType == null)
                    {
                        return new ApiResponse(500);
                    }
                    //return new ApiResponse(409);
                }

            }
            return new ApiOkResponse("Record(s) Added");
        }

        public async Task<ApiResponse> DeletePilot(long id)
        {
            var ToDelete = await _pilotProfileRepository.FindPilotById(id);

            if (ToDelete == null)
            {
                return new ApiResponse(404);
            }

            if (!await _pilotProfileRepository.DeletePilot(ToDelete))
            {
                return new ApiResponse(500);
            }

            return new ApiOkResponse(true);
        }

        public async Task<ApiResponse> DeletePilotTie(long id)
        {
            var ToDelete = await _pilotProfileRepository.FindPilotTieById(id);

            if (ToDelete == null)
            {
                return new ApiResponse(404);
            }

            if (!await _pilotProfileRepository.DeletePilotTie(ToDelete))
            {
                return new ApiResponse(500);
            }

            return new ApiOkResponse(true);
        }

        public async Task<ApiResponse> GetAllPilot()
        {
            var pilot = await _pilotProfileRepository.FindAllPilots();
            if (pilot == null)
            {
                return new ApiResponse(404);
            }
            var TransferDTO = _mapper.Map<IEnumerable<PilotProfileTransferDTO>>(pilot);
            return new ApiOkResponse(TransferDTO);
        }

        public async Task<ApiResponse> GetAllPilotTies()
        {
            var pilot = await _pilotProfileRepository.FindAllPilotTies();
            if (pilot == null)
            {
                return new ApiResponse(404);
            }
            var TransferDTO = _mapper.Map<IEnumerable<PilotSMORoutesResourceTieTransferDTO>>(pilot);
            return new ApiOkResponse(TransferDTO);
        }

        public async Task<ApiResponse> GetAllPilotTiesByResourceId(long resourceId)
        {
            var pilot = await _pilotProfileRepository.FindPilotTieByResourceId(resourceId);
            if (pilot == null)
            {
                return new ApiResponse(404);
            }
            var TransferDTO = _mapper.Map<IEnumerable<PilotSMORoutesResourceTieTransferDTO>>(pilot);
            return new ApiOkResponse(TransferDTO);
        }

        public async Task<ApiResponse> GetPilotById(long id)
        {
            var pilot = await _pilotProfileRepository.FindPilotById(id);
            if (pilot == null)
            {
                return new ApiResponse(404);
            }
            var TransferDTO = _mapper.Map<PilotProfileTransferDTO>(pilot);
            return new ApiOkResponse(TransferDTO);
        }

        public async Task<ApiResponse> GetPilotTieById(long id)
        {
            var pilot = await _pilotProfileRepository.FindPilotTieById(id);
            if (pilot == null)
            {
                return new ApiResponse(404);
            }
            var TransferDTO = _mapper.Map<PilotSMORoutesResourceTieTransferDTO>(pilot);
            return new ApiOkResponse(TransferDTO);
        }

        public async Task<ApiResponse> UpdatePilot(HttpContext context, long id, PilotProfileReceivingDTO pilotReceivingDTO)
        {
            var ToUpdate = await _pilotProfileRepository.FindPilotById(id);
            if (ToUpdate == null)
            {
                return new ApiResponse(404);
            }

            var summary = $"Initial details before change, \n {ToUpdate.ToString()} \n";

            ToUpdate.Firstname = pilotReceivingDTO.Firstname;
            ToUpdate.Lastname = pilotReceivingDTO.Lastname;
            ToUpdate.Mobile = pilotReceivingDTO.Mobile;
            ToUpdate.Gender = pilotReceivingDTO.Gender;
            ToUpdate.MeansOfIdentificationId = pilotReceivingDTO.MeansOfIdentificationId;
            ToUpdate.IDNumber = pilotReceivingDTO.IDNumber;
            ToUpdate.ImageUrl = pilotReceivingDTO.ImageUrl;
            ToUpdate.RankId = pilotReceivingDTO.RankId;
            ToUpdate.PilotTypeId = pilotReceivingDTO.PilotTypeId;
            ToUpdate.Address = pilotReceivingDTO.Address;
            ToUpdate.DOB = pilotReceivingDTO.DOB;
            ToUpdate.UpdatedAt = DateTime.UtcNow;
            var updatedRank = await _pilotProfileRepository.UpdatePilot(ToUpdate);

            summary += $"Details after change, \n {updatedRank.ToString()} \n";

            if (updatedRank == null)
            {
                return new ApiResponse(500);
            }

            var TransferDTOs = _mapper.Map<PilotProfileTransferDTO>(updatedRank);
            return new ApiOkResponse(TransferDTOs);
        }
    }
}
