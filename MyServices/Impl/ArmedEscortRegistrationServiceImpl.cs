using AutoMapper;
using HaloBiz.DTOs.ApiDTOs;
using HaloBiz.DTOs.ReceivingDTOs;
using HaloBiz.DTOs.TransferDTOs;
using HaloBiz.Helpers;
using HaloBiz.Repository;
using HalobizMigrations.Models;
using HalobizMigrations.Models.Armada;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HaloBiz.MyServices.Impl
{
    public class ArmedEscortRegistrationServiceImpl:IArmedEscortRegistrationService
    {
        private readonly IArmedEscortRegistrationRepository _armedEscortsRepository;
        private readonly IMapper _mapper;

        public ArmedEscortRegistrationServiceImpl(IMapper mapper, IArmedEscortRegistrationRepository armedEscortsRepository)
        {
            _mapper = mapper;
            _armedEscortsRepository = armedEscortsRepository;
        }

        public async Task<ApiCommonResponse> AddArmedEscort(HttpContext context, ArmedEscortProfileReceivingDTO armedEscortReceivingDTO)
        {
            var armedescort = _mapper.Map<ArmedEscortProfile>(armedEscortReceivingDTO);
            
            armedescort.CreatedById = context.GetLoggedInUserId();
            armedescort.IsDeleted = false;
            armedescort.CreatedAt = DateTime.UtcNow;
            var savedRank = await _armedEscortsRepository.SaveArmedEscort(armedescort);
            if (savedRank == null)
            {
                return CommonResponse.Send(ResponseCodes.FAILURE, null, "Some system errors occurred");
            }
            var typeTransferDTO = _mapper.Map<ArmedEscortProfileTransferDTO>(armedescort);
            return CommonResponse.Send(ResponseCodes.SUCCESS,typeTransferDTO);
        }

        public async Task<ApiCommonResponse> AddArmedEscortTie(HttpContext context, ArmedEscortSMORoutesResourceTieReceivingDTO armedEscortTieReceivingDTO)
        {
            var escort = new ArmedEscortSMORoutesResourceTie();

            for (int i = 0; i < armedEscortTieReceivingDTO.SMORouteId.Length; i++)
            {
                escort.Id = 0;
                escort.SMORegionId = armedEscortTieReceivingDTO.SMORegionId;
                escort.ResourceId = armedEscortTieReceivingDTO.ResourceId;
                escort.SMORouteId = armedEscortTieReceivingDTO.SMORouteId[i];
                var IdExist = _armedEscortsRepository.GetServiceRegIdRegionAndRoute(armedEscortTieReceivingDTO.ResourceId, armedEscortTieReceivingDTO.SMORouteId[i], armedEscortTieReceivingDTO.SMORegionId);
                if (IdExist == null)
                {
                    escort.CreatedById = context.GetLoggedInUserId();
                    escort.CreatedAt = DateTime.UtcNow;

                    var savedType = await _armedEscortsRepository.SaveArmedEscortTie(escort);
                    if (savedType == null)
                    {
                        return CommonResponse.Send(ResponseCodes.FAILURE, null, "Some system errors occurred");
                    }
                    //return                 return CommonResponse.Send(ResponseCodes.NO_DATA_AVAILABLE,null, "No record exists");;
                }

            }
            return CommonResponse.Send(ResponseCodes.SUCCESS,"Record(s) Added");
        }

        public async Task<ApiCommonResponse> DeleteArmedEscort(long id)
        {
            var itemToDelete = await _armedEscortsRepository.FindArmedEscortById(id);

            if (itemToDelete == null)
            {
                return CommonResponse.Send(ResponseCodes.NO_DATA_AVAILABLE);;
            }

            if (!await _armedEscortsRepository.DeleteArmedEscort(itemToDelete))
            {
                return CommonResponse.Send(ResponseCodes.FAILURE, null, "Some system errors occurred");
            }

            return CommonResponse.Send(ResponseCodes.SUCCESS);
        }

        public async Task<ApiCommonResponse> DeleteArmedEscortTie(long id)
        {
            var itemToDelete = await _armedEscortsRepository.FindArmedEscortTieById(id);

            if (itemToDelete == null)
            {
                return CommonResponse.Send(ResponseCodes.NO_DATA_AVAILABLE);;
            }

            if (!await _armedEscortsRepository.DeleteArmedEscortTie(itemToDelete))
            {
                return CommonResponse.Send(ResponseCodes.FAILURE, null, "Some system errors occurred");
            }

            return CommonResponse.Send(ResponseCodes.SUCCESS);
        }

        public async Task<ApiCommonResponse> GetAllArmedEscorts()
        {
            var armedescorts = await _armedEscortsRepository.FindAllArmedEscorts();
            if (armedescorts == null)
            {
                return CommonResponse.Send(ResponseCodes.NO_DATA_AVAILABLE);;
            }
            var rankTransferDTO = _mapper.Map<IEnumerable<ArmedEscortProfileTransferDTO>>(armedescorts);
            return CommonResponse.Send(ResponseCodes.SUCCESS,rankTransferDTO);
        }

        public async Task<ApiCommonResponse> GetAllArmedEscortTies()
        {
            var armedescorts = await _armedEscortsRepository.FindAllArmedEscortTies();
            if (armedescorts == null)
            {
                return CommonResponse.Send(ResponseCodes.NO_DATA_AVAILABLE);;
            }
            var rankTransferDTO = _mapper.Map<IEnumerable<ArmedEscortSMORoutesResourceTieTransferDTO>>(armedescorts);
            return CommonResponse.Send(ResponseCodes.SUCCESS,rankTransferDTO);
        }

        public async Task<ApiCommonResponse> GetArmedEscortById(long id)
        {
            var escort = await _armedEscortsRepository.FindArmedEscortById(id);
            if (escort == null)
            {
                return CommonResponse.Send(ResponseCodes.NO_DATA_AVAILABLE);;
            }
            var rankTransferDTO = _mapper.Map<ArmedEscortProfileTransferDTO>(escort);
            return CommonResponse.Send(ResponseCodes.SUCCESS,rankTransferDTO);
        }

        public async Task<ApiCommonResponse> GetArmedEscortTieById(long id)
        {
            var escort = await _armedEscortsRepository.FindArmedEscortTieById(id);
            if (escort == null)
            {
                return CommonResponse.Send(ResponseCodes.NO_DATA_AVAILABLE);;
            }
            var rankTransferDTO = _mapper.Map<ArmedEscortSMORoutesResourceTieTransferDTO>(escort);
            return CommonResponse.Send(ResponseCodes.SUCCESS,rankTransferDTO);
        }

        public async Task<ApiCommonResponse> UpdateArmedEscort(HttpContext context, long id, ArmedEscortProfileReceivingDTO armedEscortReceivingDTO)
        {
            var itemToUpdate = await _armedEscortsRepository.FindArmedEscortById(id);
            if (itemToUpdate == null)
            {
                return CommonResponse.Send(ResponseCodes.NO_DATA_AVAILABLE);;
            }

            var summary = $"Initial details before change, \n {itemToUpdate.ToString()} \n";

            itemToUpdate.FirstName = armedEscortReceivingDTO.FirstName;
            itemToUpdate.LastName = armedEscortReceivingDTO.LastName;
            itemToUpdate.ArmedEscortTypeId = armedEscortReceivingDTO.ArmedEscortTypeId;
            itemToUpdate.ImageUrl = armedEscortReceivingDTO.ImageUrl;
            itemToUpdate.Mobile = armedEscortReceivingDTO.Mobile;
            itemToUpdate.Gender = armedEscortReceivingDTO.Gender;
            itemToUpdate.RankId = armedEscortReceivingDTO.RankId;
            itemToUpdate.DateOfBirth = armedEscortReceivingDTO.DateOfBirth;
            itemToUpdate.Alias = armedEscortReceivingDTO.Alias;

            itemToUpdate.UpdatedAt = DateTime.UtcNow;
            var updatedRank = await _armedEscortsRepository.UpdateArmedEscort(itemToUpdate);

            summary += $"Details after change, \n {updatedRank.ToString()} \n";

            if (updatedRank == null)
            {
                return CommonResponse.Send(ResponseCodes.FAILURE, null, "Some system errors occurred");
            }

            var rankTransferDTOs = _mapper.Map<ArmedEscortProfileTransferDTO>(updatedRank);
            return CommonResponse.Send(ResponseCodes.SUCCESS,rankTransferDTOs);
        }
    }
}
