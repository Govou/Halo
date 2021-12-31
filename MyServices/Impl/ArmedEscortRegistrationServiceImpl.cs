﻿using AutoMapper;
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

        public async Task<ApiResponse> AddArmedEscort(HttpContext context, ArmedEscortProfileReceivingDTO armedEscortReceivingDTO)
        {
            var armedescort = _mapper.Map<ArmedEscortProfile>(armedEscortReceivingDTO);
            
            armedescort.CreatedById = context.GetLoggedInUserId();
            armedescort.IsDeleted = false;
            armedescort.CreatedAt = DateTime.UtcNow;
            var savedRank = await _armedEscortsRepository.SaveArmedEscort(armedescort);
            if (savedRank == null)
            {
                return new ApiResponse(500);
            }
            var typeTransferDTO = _mapper.Map<ArmedEscortProfileTransferDTO>(armedescort);
            return new ApiOkResponse(typeTransferDTO);
        }

        public async Task<ApiResponse> AddArmedEscortTie(HttpContext context, ArmedEscortSMORoutesResourceTieReceivingDTO armedEscortTieReceivingDTO)
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
                        return new ApiResponse(500);
                    }
                    //return new ApiResponse(409);
                }

            }
            return new ApiOkResponse("Record(s) Added");
        }

        public async Task<ApiResponse> DeleteArmedEscort(long id)
        {
            var itemToDelete = await _armedEscortsRepository.FindArmedEscortById(id);

            if (itemToDelete == null)
            {
                return new ApiResponse(404);
            }

            if (!await _armedEscortsRepository.DeleteArmedEscort(itemToDelete))
            {
                return new ApiResponse(500);
            }

            return new ApiOkResponse(true);
        }

        public async Task<ApiResponse> DeleteArmedEscortTie(long id)
        {
            var itemToDelete = await _armedEscortsRepository.FindArmedEscortTieById(id);

            if (itemToDelete == null)
            {
                return new ApiResponse(404);
            }

            if (!await _armedEscortsRepository.DeleteArmedEscortTie(itemToDelete))
            {
                return new ApiResponse(500);
            }

            return new ApiOkResponse(true);
        }

        public async Task<ApiResponse> GetAllArmedEscorts()
        {
            var armedescorts = await _armedEscortsRepository.FindAllArmedEscorts();
            if (armedescorts == null)
            {
                return new ApiResponse(404);
            }
            var rankTransferDTO = _mapper.Map<IEnumerable<ArmedEscortProfileTransferDTO>>(armedescorts);
            return new ApiOkResponse(rankTransferDTO);
        }

        public async Task<ApiResponse> GetAllArmedEscortTies()
        {
            var armedescorts = await _armedEscortsRepository.FindAllArmedEscortTies();
            if (armedescorts == null)
            {
                return new ApiResponse(404);
            }
            var rankTransferDTO = _mapper.Map<IEnumerable<ArmedEscortSMORoutesResourceTieTransferDTO>>(armedescorts);
            return new ApiOkResponse(rankTransferDTO);
        }

        public async Task<ApiResponse> GetArmedEscortById(long id)
        {
            var escort = await _armedEscortsRepository.FindArmedEscortById(id);
            if (escort == null)
            {
                return new ApiResponse(404);
            }
            var rankTransferDTO = _mapper.Map<ArmedEscortProfileTransferDTO>(escort);
            return new ApiOkResponse(rankTransferDTO);
        }

        public async Task<ApiResponse> GetArmedEscortTieById(long id)
        {
            var escort = await _armedEscortsRepository.FindArmedEscortTieById(id);
            if (escort == null)
            {
                return new ApiResponse(404);
            }
            var rankTransferDTO = _mapper.Map<ArmedEscortSMORoutesResourceTieTransferDTO>(escort);
            return new ApiOkResponse(rankTransferDTO);
        }

        public async Task<ApiResponse> UpdateArmedEscort(HttpContext context, long id, ArmedEscortProfileReceivingDTO armedEscortReceivingDTO)
        {
            var itemToUpdate = await _armedEscortsRepository.FindArmedEscortById(id);
            if (itemToUpdate == null)
            {
                return new ApiResponse(404);
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
                return new ApiResponse(500);
            }

            var rankTransferDTOs = _mapper.Map<ArmedEscortProfileTransferDTO>(updatedRank);
            return new ApiOkResponse(rankTransferDTOs);
        }
    }
}
