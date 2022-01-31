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

        public async Task<ApiCommonResponse> AddPilot(HttpContext context, PilotProfileReceivingDTO pilotReceivingDTO)
        {
            var pilot = _mapper.Map<PilotProfile>(pilotReceivingDTO);
            //var NameExist = _pilotRepository.GetTypename(pilotReceivingDTO.na);
            //if (NameExist != null)
            //{
            //    return                 return CommonResponse.Send(ResponseCodes.NO_DATA_AVAILABLE,null, "No record exists");;
            //}
            pilot.CreatedById = context.GetLoggedInUserId();
            pilot.IsDeleted = false;
            pilot.CreatedAt = DateTime.UtcNow;
            var savedRank = await _pilotProfileRepository.SavePilot(pilot);
            if (savedRank == null)
            {
                return CommonResponse.Send(ResponseCodes.FAILURE, null, "Some system errors occurred");
            }
            var TransferDTO = _mapper.Map<PilotProfileTransferDTO>(pilot);
            return CommonResponse.Send(ResponseCodes.SUCCESS,TransferDTO);
        }

        public async Task<ApiCommonResponse> AddPilotTie(HttpContext context, PilotSMORoutesResourceTieReceivingDTO pilotReceivingTieDTO)
        {
            var pilot = new PilotSMORoutesResourceTie();

            for (int i = 0; i < pilotReceivingTieDTO.SMORouteId.Length; i++)
            {
                pilot.Id = 0;
                pilot.SMORegionId = pilotReceivingTieDTO.SMORegionId;
                pilot.ResourceId = pilotReceivingTieDTO.ResourceId;
                pilot.SMORouteId = pilotReceivingTieDTO.SMORouteId[i];
                var IdExist = _pilotProfileRepository.GetResourceRegIdRegionAndRouteId(pilotReceivingTieDTO.ResourceId, pilotReceivingTieDTO.SMORouteId[i]);
                if (IdExist == null)
                {
                    pilot.CreatedById = context.GetLoggedInUserId();
                    pilot.CreatedAt = DateTime.UtcNow;

                    var savedType = await _pilotProfileRepository.SavePilotTie(pilot);
                    if (savedType == null)
                    {
                        return CommonResponse.Send(ResponseCodes.FAILURE, null, "Some system errors occurred");
                    }
                    //return                 return CommonResponse.Send(ResponseCodes.NO_DATA_AVAILABLE,null, "No record exists");;
                }

            }
            return CommonResponse.Send(ResponseCodes.SUCCESS,"Record(s) Added");
        }

        public async Task<ApiCommonResponse> DeletePilot(long id)
        {
            var ToDelete = await _pilotProfileRepository.FindPilotById(id);

            if (ToDelete == null)
            {
                return CommonResponse.Send(ResponseCodes.NO_DATA_AVAILABLE);;
            }

            if (!await _pilotProfileRepository.DeletePilot(ToDelete))
            {
                return CommonResponse.Send(ResponseCodes.FAILURE, null, "Some system errors occurred");
            }

            return CommonResponse.Send(ResponseCodes.SUCCESS);
        }

        public async Task<ApiCommonResponse> DeletePilotTie(long id)
        {
            var ToDelete = await _pilotProfileRepository.FindPilotTieById(id);

            if (ToDelete == null)
            {
                return CommonResponse.Send(ResponseCodes.NO_DATA_AVAILABLE);;
            }

            if (!await _pilotProfileRepository.DeletePilotTie(ToDelete))
            {
                return CommonResponse.Send(ResponseCodes.FAILURE, null, "Some system errors occurred");
            }

            return CommonResponse.Send(ResponseCodes.SUCCESS);
        }

        public async Task<ApiCommonResponse> GetAllPilot()
        {
            var pilot = await _pilotProfileRepository.FindAllPilots();
            if (pilot == null)
            {
                return CommonResponse.Send(ResponseCodes.NO_DATA_AVAILABLE);;
            }
            var TransferDTO = _mapper.Map<IEnumerable<PilotProfileTransferDTO>>(pilot);
            return CommonResponse.Send(ResponseCodes.SUCCESS,TransferDTO);
        }

        public async Task<ApiCommonResponse> GetAllPilotTies()
        {
            var pilot = await _pilotProfileRepository.FindAllPilotTies();
            if (pilot == null)
            {
                return CommonResponse.Send(ResponseCodes.NO_DATA_AVAILABLE);;
            }
            var TransferDTO = _mapper.Map<IEnumerable<PilotSMORoutesResourceTieTransferDTO>>(pilot);
            return CommonResponse.Send(ResponseCodes.SUCCESS,TransferDTO);
        }

        
        public async Task<ApiCommonResponse> GetAllPilotTiesByResourceId(long resourceId)
        {
            var pilot = await _pilotProfileRepository.FindPilotTieByResourceId(resourceId);
            if (pilot == null)
            {
                return CommonResponse.Send(ResponseCodes.NO_DATA_AVAILABLE);
            }
            var TransferDTO = _mapper.Map<IEnumerable<PilotSMORoutesResourceTieTransferDTO>>(pilot);
            return CommonResponse.Send(ResponseCodes.SUCCESS, TransferDTO);
        }

        public async Task<ApiCommonResponse> GetPilotById(long id)
        {
            var pilot = await _pilotProfileRepository.FindPilotById(id);
            if (pilot == null)
            {
                return CommonResponse.Send(ResponseCodes.NO_DATA_AVAILABLE);;
            }
            var TransferDTO = _mapper.Map<PilotProfileTransferDTO>(pilot);
            return CommonResponse.Send(ResponseCodes.SUCCESS,TransferDTO);
        }

        public async Task<ApiCommonResponse> GetPilotTieById(long id)
        {
            var pilot = await _pilotProfileRepository.FindPilotTieById(id);
            if (pilot == null)
            {
                return CommonResponse.Send(ResponseCodes.NO_DATA_AVAILABLE);;
            }
            var TransferDTO = _mapper.Map<PilotSMORoutesResourceTieTransferDTO>(pilot);
            return CommonResponse.Send(ResponseCodes.SUCCESS,TransferDTO);
        }

        public async Task<ApiCommonResponse> UpdatePilot(HttpContext context, long id, PilotProfileReceivingDTO pilotReceivingDTO)
        {
            var ToUpdate = await _pilotProfileRepository.FindPilotById(id);
            if (ToUpdate == null)
            {
                return CommonResponse.Send(ResponseCodes.NO_DATA_AVAILABLE);;
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
                return CommonResponse.Send(ResponseCodes.FAILURE, null, "Some system errors occurred");
            }

            var TransferDTOs = _mapper.Map<PilotProfileTransferDTO>(updatedRank);
            return CommonResponse.Send(ResponseCodes.SUCCESS,TransferDTOs);
        }
    }
}
